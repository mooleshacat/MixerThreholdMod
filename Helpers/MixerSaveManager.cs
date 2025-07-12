using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using ScheduleOne.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MixerThreholdMod_0_0_1.Utils
{
    public static class MixerSaveManager
    {
        private static CancellationTokenSource saveCts = new CancellationTokenSource();
        public static ConcurrentDictionary<int, float> SavedMixerValues = new ConcurrentDictionary<int, float>();

        // Constants for file operations
        private const int MaxRetries = 5;
        private const int RetryDelayMs = 500;
        private static readonly string LockFilePath = Path.Combine(MelonEnvironment.UserDataDirectory, "mixer_save.lock");

        public static IEnumerator AttachListenerWhenReady(MixingStationConfiguration instance, int uniqueID)
        {
            Main.logger.Msg(3, $"AttachListenerWhenReady started for Mixer {uniqueID}");

            // Safety check: bail if instance is null
            if (instance == null)
            {
                Main.logger.Warn(1, $"Instance is null — cannot attach listener for Mixer {uniqueID}");
                yield break;
            }

            // Wait for StartThrehold to become available, but only while instance exists
            int waitAttempts = 0;
            const int maxWaitAttempts = 100; // Prevent infinite loop

            while (instance != null && instance.StartThrehold == null && waitAttempts < maxWaitAttempts)
            {
                loadError = ex;
            }

            if (loadError != null)
            {
                Main.logger.Err(string.Format("LoadMixerValuesWhenReady error: {0}\nStackTrace: {1}", loadError.Message, loadError.StackTrace));
            }

            if (instance.StartThrehold == null)
            {
                Main.logger.Warn(1, $"StartThrehold never became available after {maxWaitAttempts} attempts — Mixer {uniqueID}");
                yield break;
            }

            // Process mixer setup on a separate thread to handle exceptions properly
            Task.Run(() =>
            {
                try
                {
                    ProcessMixerSetupSafely(instance, uniqueID);
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"AttachListenerWhenReady: Error in background setup for Mixer {uniqueID}: {ex.Message}\n{ex.StackTrace}");
                }
            });
        }

        public static IEnumerator AttachListenerWhenReady(MixingStationConfiguration config, int mixerID)
        {
            Main.logger.Msg(3, string.Format("AttachListenerWhenReady: Started for Mixer {0}", mixerID));

            // Wait until the StartThrehold is properly initialized - NO try-catch around yield
            while (config?.StartThrehold == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Main.logger.Msg(3, string.Format("AttachListenerWhenReady: StartThrehold found for Mixer {0}", mixerID));

            // Instead of using OnValueChanged, we'll use reflection to find the correct event or polling
            Exception attachError = null;
            bool eventAttached = false;

            try
            {
                // Method 1: Try to find an event using reflection
                var numberFieldType = config.StartThrehold.GetType();
                Main.logger.Msg(3, string.Format("AttachListenerWhenReady: NumberField type: {0}", numberFieldType.Name));

                // Look for common event names
                var eventNames = new[] { "OnValueChanged", "ValueChanged", "onValueChanged", "OnChange", "Changed" };

                foreach (var eventName in eventNames)
                {
                    var eventInfo = numberFieldType.GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
                    if (eventInfo != null && eventInfo.EventHandlerType != null)
                    {
                        instance.StartThrehold.SetValue(savedValue, true);
                        Main.logger.Msg(3, $"Restored Mixer {uniqueID} to saved value: {savedValue}");
                    }
                    catch (Exception setEx)
                    {
                        Main.logger.Err($"ProcessMixerSetupSafely: Failed to set saved value for Mixer {uniqueID}: {setEx.Message}");
                    }
                }

                // Method 2: If no event found, try to find a field/property that might have changed events
                if (!eventAttached)
                {
                    var fields = numberFieldType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        if (field.FieldType.Name.Contains("UnityEvent") || field.FieldType.Name.Contains("Action"))
                        {
                            try
                            {
                                MixerSaveManager.SaveMixerValue(uniqueID, val);
                                Main.logger.Msg(3, $"Mixer {uniqueID} changed to {val}");
                            }
                            catch (Exception saveEx)
                            {
                                Main.logger.Err($"ProcessMixerSetupSafely: Error in mixer value change listener for {uniqueID}: {saveEx.Message}");
                            }
                        });
                        Main.logger.Msg(3, $"Successfully attached listener for Mixer {uniqueID}");
                    }
                    else
                    {
                        Main.logger.Msg(3, string.Format("OnValueChangedGeneric: Could not determine value for Mixer {0}", mixerID));
                    }
                }
                catch (Exception listenerEx)
                {
                    Main.logger.Err($"ProcessMixerSetupSafely: Failed to attach listener for Mixer {uniqueID}: {listenerEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err($"ProcessMixerSetupSafely: Critical error for Mixer {uniqueID}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static IEnumerator WriteMixerValuesAsync(string saveFolderPath)
        {
            // Check cooldown period
            if (DateTime.Now - lastSaveTime < SAVE_COOLDOWN)
            {
                Main.logger.Msg(3, "SaveMixerValues: Skipping save due to cooldown period");
                yield break;
            }

            // Move the Task.Run call outside of try-catch to avoid yield issues
            var task = Task.Run(() => SaveMixerThresholds(saveFolderPath));

            // Wait for completion without yielding inside try-catch
            while (!task.IsCompleted)
            {
                yield return null;
            }

            // Check for errors after completion
            if (task.IsFaulted)
            {
                Main.logger.Err($"WriteMixerValuesAsync: Task failed: {task.Exception?.GetBaseException()?.Message}");
            }
        }

        private static void SaveMixerValue(int id, float value)
        {
            try
            {
                Main.logger.Msg(3, $"SaveMixerValue called for Mixer {id} with value {value}");

                // Cancel any previous save operation
                if (saveCts != null)
                {
                    saveCts.Cancel();
                    saveCts.Dispose();
                }
                saveCts = new CancellationTokenSource();

                // Convert to regular dictionary for JSON serialization - .NET 4.8.1 compatible
                var mixerValuesDict = new Dictionary<int, float>();
                foreach (var kvp in Main.savedMixerValues)
                {
                    mixerValuesDict[kvp.Key] = kvp.Value;
                }

                string path = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');

                if (string.IsNullOrEmpty(path))
                {
                    ["MixerValues"] = mixerValuesDict,
                    ["SaveTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["Version"] = "0.0.1"
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                bool success = await SafeWriteAllTextWithCancellationAsync(path, json, ct);

                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                }

                File.Move(tempFile, saveFile);

                Main.logger.Msg(2, string.Format("SaveMixerValues: Saved {0} mixer values to {1}", Main.savedMixerValues.Count, saveFile));

                // Also save to persistent location
                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (!string.IsNullOrEmpty(persistentPath))
                {
                    string persistentFile = Path.Combine(persistentPath, "MixerThresholdSave.json");
                    File.Copy(saveFile, persistentFile, overwrite: true);
                    Main.logger.Msg(3, "SaveMixerValues: Copied to persistent location");
                }
            }
            catch (Exception ex)
            {
                saveError = ex;
            }
        }

        public static async Task<bool> SafeWriteAllTextWithCancellationAsync(string path, string output, CancellationToken ct)
        {
            string normalizedPath = path.Replace('/', '\\');
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                if (ct.IsCancellationRequested)
                {
                    Main.logger.Warn(1, $"Operation canceled during SafeWriteAllTextWithCancellation for [{normalizedPath}]");
                    return false;
                }

                try
                {
                    // Simple file write with retry logic - no complex locking for now
                    using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        await writer.WriteAsync(output).ConfigureAwait(false);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Main.logger.Warn(1, $"Error writing to file [{path}] (attempt {attempt}/{MaxRetries}): {ex.Message}");
                    if (attempt < MaxRetries)
                    {
                        await Task.Delay(RetryDelayMs * attempt, ct).ConfigureAwait(false);
                    }
                }
            }
            return false;
        }

        public static void SaveMixerThresholds(string saveFolderPath)
        {
            Utils.CoroutineHelper.RunCoroutine(SaveMixerThresholdsAsync(saveFolderPath));
        }

        public static bool _hasLoggedZeroMixers = false;

        private static IEnumerator SaveMixerThresholdsAsync(string saveFolderPath)
        {
            if (string.IsNullOrEmpty(saveFolderPath))
            {
                Main.logger.Warn(1, "SaveMixerThresholdsAsync: saveFolderPath is null or empty");
                yield break;
            }

            // Move Task.Run outside of try-catch to avoid yield issues
            var saveTask = Task.Run(async () =>
            {
                string filePath = null;
                try
                {
                    Main.logger.Msg(3, $"SaveMixerThresholdsAsync: Starting save operation for folder: {saveFolderPath}");

                    filePath = Path.Combine(saveFolderPath, "MixerThresholdSave.json").Replace('/', '\\');
                    Main.logger.Msg(3, $"SaveMixerThresholdsAsync: Target file path: {filePath}");

                    var _mixerCount = await TrackedMixers.CountAsync(tm => tm != null);
                    var mixerIds = await TrackedMixers.SelectAsync(tm => tm?.MixerInstanceID ?? -1);
                    var validMixerIds = mixerIds?.Where(id => id != -1)?.ToList() ?? new List<int>();

                    Main.logger.Msg(3, $"Currently tracking {_mixerCount} mixers: {string.Join(", ", validMixerIds)}");

                    if (_mixerCount == 0)
                    {
                        if (!_hasLoggedZeroMixers)
                        {
                            Main.logger.Msg(2, "No mixers tracked (maybe you have none?) — skipping mixer threshold prefs save/create.");
                            Main.logger.Warn(1, "No mixers tracked (maybe you have none?) — suppressing future logs until reload save.");
                            _hasLoggedZeroMixers = true;
                        }
                        return;
                    }

                    var mixerDataSnapshot = new Dictionary<int, float>();
                    var trackedMixers = await TrackedMixers.ToListAsync();

                    if (trackedMixers == null)
                    {
                        Main.logger.Warn(1, "SaveMixerThresholdsAsync: TrackedMixers.ToListAsync returned null");
                        return;
                    }

                    foreach (var tm in trackedMixers)
                    {
                        try
                        {
                            if (tm == null)
                            {
                                Main.logger.Msg(3, $"Skipping null tracker mixer entry");
                                continue;
                            }

                            if (tm.ConfigInstance == null)
                            {
                                Main.logger.Warn(1, $"Removing tracker with null ConfigInstance: {tm.MixerInstanceID}");
                                await TrackedMixers.RemoveAsync(tm.ConfigInstance);
                                continue;
                            }

                            if (tm.ConfigInstance.StartThrehold == null)
                            {
                                Main.logger.Warn(1, $"Removing tracker with null StartThrehold: {tm.MixerInstanceID}");
                                await TrackedMixers.RemoveAsync(tm.ConfigInstance);
                                continue;
                            }

                            try
                            {
                                float currentValue = tm.ConfigInstance.StartThrehold.Value;
                                mixerDataSnapshot[tm.MixerInstanceID] = currentValue;
                                Main.logger.Msg(3, $"Captured Mixer {tm.MixerInstanceID} value: {currentValue}");
                            }
                            catch (Exception valueEx)
                            {
                                Main.logger.Err($"SaveMixerThresholdsAsync: Error reading value from mixer {tm.MixerInstanceID}: {valueEx.Message}");
                            }
                        }
                        catch (Exception tmEx)
                        {
                            Main.logger.Err($"SaveMixerThresholdsAsync: Error processing tracked mixer {tm?.MixerInstanceID ?? -1}: {tmEx.Message}");
                        }
                    }

                    if (mixerDataSnapshot.Count == 0)
                    {
                        Main.logger.Msg(2, "No valid mixer data to save");
                        return;
                    }

                    string json = JsonConvert.SerializeObject(new MixerThresholdSaveData
                    {
                        MixerValues = mixerDataSnapshot
                    }, Formatting.Indented);

                    bool success = false;
                    int attempts = 0;
                    const int maxAttempts = 5;

                    while (!success && attempts < maxAttempts)
                    {
                        try
                        {
                            attempts++;
                            Main.logger.Msg(3, $"SaveMixerThresholdsAsync: File write attempt {attempts}/{maxAttempts}");

                            // Using SafeWriteAllText from FileOperations
                            FileOperations.SafeWriteAllText(filePath, json);
                            Main.logger.Msg(1, $"Saved {mixerDataSnapshot.Count} mixer thresholds to {filePath}.");
                            success = true;
                        }
                        catch (IOException ioEx)
                        {
                            Main.logger.Warn(1, $"File write failed [{filePath}] (attempt {attempts}/{maxAttempts}), retrying... {ioEx.Message}");
                            if (attempts < maxAttempts)
                            {
                                Thread.Sleep(500);
                            }
                        }
                        catch (Exception writeEx)
                        {
                            Main.logger.Err($"SaveMixerThresholdsAsync: Unexpected error saving mixer file [{filePath}] (attempt {attempts}): {writeEx.Message}\n{writeEx.StackTrace}");
                            break;
                        }
                    }

                    if (!success)
                    {
                        Main.logger.Err($"SaveMixerThresholdsAsync: Failed to save mixer file after {maxAttempts} attempts: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"SaveMixerThresholdsAsync: Critical error during saving [{filePath ?? "unknown"}]");
                    Main.logger.Err($"SaveMixerThresholdsAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
                }
                finally
                {
                    Main.logger.Msg(3, "SaveMixerThresholdsAsync: Mixer save operation completed.");
                }
            });

            // Wait for task completion without yielding inside try-catch
            while (!saveTask.IsCompleted)
            {
                yield return null;
            }

            // Check for errors after completion
            if (saveTask.IsFaulted)
            {
                Main.logger.Err($"SaveMixerThresholdsAsync: Task failed: {saveTask.Exception?.GetBaseException()?.Message}");
            }
        }

        private static bool _hasLoaded = false;
        private static object _loadLock = new object();

        public static IEnumerator LoadMixerValuesWhenReady()
        {
            Main.logger.Msg(3, "LoadMixerValuesWhenReady: Starting");

            // Check the flag at the very start
            lock (_loadLock)
            {
                if (_hasLoaded)
                {
                    Main.logger.Msg(3, "LoadMixerValuesWhenReady: Already loaded, skipping");
                    yield break;
                }

                // First one in, mark as loading
                _hasLoaded = true;
            }

            int attempts = 0;
            const int maxAttempts = 50;

            while (string.IsNullOrEmpty(Main.CurrentSavePath) && attempts < maxAttempts)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Handle the loading operation on a background thread
            Task.Run(() =>
            {
                try
                {
                    HandleMixerLoadingSafely();
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"LoadMixerValuesWhenReady: Error in background loading: {ex.Message}\n{ex.StackTrace}");
                }
            });
        }

            // Create backup directory with error handling
            Exception dirError = null;
            try
            {
                if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                {
                    try
                    {
                        LoadAndApplyMixerThresholds(Main.CurrentSavePath);
                        logger.Msg(3, $"HandleMixerLoadingSafely: Loaded mixer values after save path became available: {Main.CurrentSavePath}");
                    }
                    catch (Exception loadEx)
                    {
                        logger.Err($"HandleMixerLoadingSafely: Error loading mixer thresholds: {loadEx.Message}");
                    }
                }
                else
                {
                    logger.Warn(1, $"HandleMixerLoadingSafely: Save path never became available after attempts. Using empty/default mixer values.");
                }
            }
            catch (Exception ex)
            {
                logger.Err($"HandleMixerLoadingSafely: Critical error: {ex.Message}\n{ex.StackTrace}");
            }

            if (dirError != null)
            {
                Main.logger.Err(string.Format("BackupSaveFolder: Failed to create backup directory: {0}", dirError.Message));
                lock (backupLock)
                {
                    isBackupInProgress = false;
                }
                yield break;
            }

            // Perform backup with timeout protection
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupFile = Path.Combine(backupDir, string.Format("MixerThresholdSave_backup_{0}.json", timestamp));

            Exception copyError = null;
            try
            {
                // Use File.Copy instead of FileOperations.SafeCopy to avoid potential deadlocks
                File.Copy(sourceFile, backupFile, overwrite: true);
                Main.logger.Msg(2, string.Format("BackupSaveFolder: Backup created: {0}", backupFile));
            }
            catch (Exception ex)
            {
                copyError = ex;
            }

            if (copyError != null)
            {
                Main.logger.Err(string.Format("BackupSaveFolder: Backup failed: {0}", copyError.Message));
            }

            // Perform cleanup without try-catch around yield
            yield return PerformCleanupCoroutine(backupDir, startTime, BACKUP_TIMEOUT);

            // CRITICAL: Always reset the flag, even on exceptions
            lock (backupLock)
            {
                isBackupInProgress = false;
            }

            Main.logger.Msg(3, "BackupSaveFolder: Finished and cleanup completed");
        }

        public async static void LoadAndApplyMixerThresholds(string saveFolderPath)
        {
            Main.logger.Msg(3, "PerformCleanupCoroutine: Starting cleanup of old backups");

            // Get list of backup files outside of try-catch
            string[] allBackupFiles = null;
            Exception listError = null;

            try
            {
                Main.logger.Msg(3, $"LoadAndApplyMixerThresholds: Starting for folder: {saveFolderPath ?? "null"}");

                string[] pathsToTry;
                if (!string.IsNullOrEmpty(saveFolderPath))
                {
                    string savePath = Path.Combine(saveFolderPath, "MixerThresholdSave.json").Replace('/', '\\');
                    string persistentPath = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');
                    pathsToTry = new[] { savePath, persistentPath };
                }
                else
                {
                    string persistentPath = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');
                    pathsToTry = new[] { persistentPath };
                }

                foreach (var path in pathsToTry)
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        Main.logger.Warn(1, "LoadAndApplyMixerThresholds: Empty path in pathsToTry");
                        continue;
                    }

                    if (File.Exists(path))
                    {
                        try
                        {
                            Main.logger.Msg(3, $"LoadAndApplyMixerThresholds: Found file at {path}");

                            string json = FileOperations.SafeReadAllText(path);
                            if (string.IsNullOrEmpty(json))
                            {
                                Main.logger.Warn(1, $"LoadAndApplyMixerThresholds: File exists but content is empty: {path}");
                                continue;
                            }

                            MixerThresholdSaveData data = JsonConvert.DeserializeObject<MixerThresholdSaveData>(json);
                            if (data?.MixerValues == null)
                            {
                                Main.logger.Warn(1, $"LoadAndApplyMixerThresholds: Deserialized data is null or MixerValues is null: {path}");
                                continue;
                            }

                            foreach (var kvp in data.MixerValues)
                            {
                                try
                                {
                                    if (SavedMixerValues != null)
                                    {
                                        SavedMixerValues[kvp.Key] = kvp.Value;
                                    }
                                }
                                catch (Exception kvpEx)
                                {
                                    Main.logger.Err($"LoadAndApplyMixerThresholds: Error processing mixer value {kvp.Key}={kvp.Value}: {kvpEx.Message}");
                                }
                            }

                            var trackedMixers = await TrackedMixers.GetAllAsync();
                            if (trackedMixers != null)
                            {
                                foreach (var tm in trackedMixers)
                                {
                                    try
                                    {
                                        if (tm == null)
                                        {
                                            Main.logger.Msg(3, "LoadAndApplyMixerThresholds: Skipping null tracked mixer");
                                            continue;
                                        }

                                        if (tm.ConfigInstance?.StartThrehold != null &&
                                            SavedMixerValues != null &&
                                            SavedMixerValues.TryGetValue(tm.MixerInstanceID, out float savedValue))
                                        {
                                            tm.ConfigInstance.StartThrehold.SetValue(savedValue, true);
                                            Main.logger.Msg(1, $"Applied saved value {savedValue} to Mixer {tm.MixerInstanceID}");
                                        }
                                    }
                                    catch (Exception tmEx)
                                    {
                                        Main.logger.Err($"LoadAndApplyMixerThresholds: Error applying value to mixer {tm?.MixerInstanceID ?? -1}: {tmEx.Message}");
                                    }
                                }
                            }

                            Main.logger.Msg(1, $"Loaded {data.MixerValues.Count} mixer thresholds from: {path}");
                            return;
                        }
                        catch (Exception pathEx)
                        {
                            Main.logger.Err($"LoadAndApplyMixerThresholds: Error loading MixerThresholdSave.json from {path}: {pathEx.Message}\n{pathEx.StackTrace}");
                        }
                    }
                    else
                    {
                        Main.logger.Msg(3, $"LoadAndApplyMixerThresholds: File does not exist: {path}");
                    }
                }

                // Only warn if we're past initial load (e.g., save path was set and we still couldn't find the file)
                if (!string.IsNullOrEmpty(saveFolderPath))
                {
                    Main.logger.Warn(1, "No MixerThresholdSave.json found in any location. Do you have any mixers?");
                }
                else
                {
                    Main.logger.Msg(3, "LoadAndApplyMixerThresholds: No save folder path provided, using defaults");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err($"LoadAndApplyMixerThresholds: Critical error: {ex.Message}\n{ex.StackTrace}");
            }

            // Sort and identify old backups outside of try-catch
            var sortedBackups = allBackupFiles.OrderByDescending(f => File.GetCreationTime(f)).ToList();
            var oldBackups = sortedBackups.Skip(5).ToList();

            Main.logger.Msg(3, string.Format("PerformCleanupCoroutine: Found {0} old backups to delete", oldBackups.Count));

            foreach (var oldBackup in oldBackups)
            {
                if ((Time.time - startTime) > timeoutSeconds)
                {
                    Main.logger.Warn(1, "PerformCleanupCoroutine: Timeout during cleanup, stopping");
                    break;
                }

                Exception deleteError = null;
                try
                {
                    File.Delete(oldBackup);
                    Main.logger.Msg(3, string.Format("PerformCleanupCoroutine: Deleted old backup: {0}", oldBackup));
                }
                catch (Exception deleteEx)
                {
                    deleteError = deleteEx;
                }

                if (deleteError != null)
                {
                    Main.logger.Warn(1, string.Format("PerformCleanupCoroutine: Failed to delete old backup {0}: {1}", oldBackup, deleteError.Message));
                }

                // Yield between file operations - NO try-catch around this
                yield return null;
            }

            Main.logger.Msg(3, "PerformCleanupCoroutine: Cleanup completed");
        }

        [Serializable]
        public class MixerThresholdSaveData
        {
            Exception backupCheckError = null;
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(Main.CurrentSavePath)) return false;

                string backupDir = Path.Combine(Main.CurrentSavePath, "MixerThresholdBackups");
                if (!Directory.Exists(backupDir)) return true;

                var allBackupFiles = Directory.GetFiles(backupDir, "MixerThresholdSave_backup_*.json");
                var latestBackup = allBackupFiles.OrderByDescending(f => File.GetCreationTime(f)).FirstOrDefault();

                if (latestBackup == null) return true;

                // Only create backup if last one is older than 5 minutes
                result = DateTime.Now - File.GetCreationTime(latestBackup) > TimeSpan.FromMinutes(5);
            }
            catch (Exception ex)
            {
                backupCheckError = ex;
            }

            if (backupCheckError != null)
            {
                Main.logger.Warn(1, string.Format("ShouldCreateBackup: Error checking backup status: {0}", backupCheckError.Message));
                return false; // Don't backup if we can't determine status safely
            }

            return result;
        }

        // Emergency save method for crash scenarios - NO coroutines here
        public static void EmergencySave()
        {
            Exception emergencyError = null;
            try
            {
                if (Main.savedMixerValues.Count == 0) return;

                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (string.IsNullOrEmpty(persistentPath)) return;

                string emergencyFile = Path.Combine(persistentPath, "MixerThresholdSave_Emergency.json");

                // Convert to regular dictionary for JSON serialization - .NET 4.8.1 compatible
                var mixerValuesDict = new Dictionary<int, float>();
                foreach (var kvp in Main.savedMixerValues)
                {
                    mixerValuesDict[kvp.Key] = kvp.Value;
                }

                var saveData = new Dictionary<string, object>
                {
                    ["MixerValues"] = mixerValuesDict,
                    ["SaveTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["Reason"] = "Emergency save before crash/shutdown"
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                File.WriteAllText(emergencyFile, json);

                Main.logger.Msg(1, "Emergency save completed successfully");
            }
            catch (Exception ex)
            {
                emergencyError = ex;
            }

            if (emergencyError != null)
            {
                Main.logger.Err(string.Format("Emergency save failed: {0}", emergencyError.Message));
            }
        }
    }
}