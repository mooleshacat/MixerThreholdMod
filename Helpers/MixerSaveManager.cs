using MelonLoader;
using MelonLoader.TinyJSON;
using MelonLoader.Utils;
using Newtonsoft.Json;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Properties;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static HarmonyLib.Code;
using static MelonLoader.Modules.MelonModule;
using static MixerThreholdMod_0_0_1.Main;
using static ScheduleOne.Console;

namespace MixerThreholdMod_0_0_1
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
                waitAttempts++;
                yield return null;
            }

            // Double-check instance still exists after waiting
            if (instance == null)
            {
                Main.logger.Warn(1, $"Instance destroyed before threshold became available — Mixer {uniqueID}");
                yield break;
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

        private static void ProcessMixerSetupSafely(MixingStationConfiguration instance, int uniqueID)
        {
            try
            {
                // Restore saved value if exists
                if (Main.savedMixerValues != null && Main.savedMixerValues.TryGetValue(uniqueID, out float savedValue))
                {
                    try
                    {
                        instance.StartThrehold.SetValue(savedValue, true);
                        Main.logger.Msg(3, $"Restored Mixer {uniqueID} to saved value: {savedValue}");
                    }
                    catch (Exception setEx)
                    {
                        Main.logger.Err($"ProcessMixerSetupSafely: Failed to set saved value for Mixer {uniqueID}: {setEx.Message}");
                    }
                }

                // Attach listener
                try
                {
                    if (instance.StartThrehold.onItemChanged != null)
                    {
                        instance.StartThrehold.onItemChanged.AddListener((float val) =>
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
                        Main.logger.Warn(1, $"onItemChanged is null for Mixer {uniqueID}, cannot attach listener");
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
            if (string.IsNullOrEmpty(saveFolderPath))
            {
                Main.logger.Warn(1, "WriteMixerValuesAsync: saveFolderPath is null or empty");
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

                // Run file save on background thread
                Task.Run(async () =>
                {
                    try
                    {
                        await SaveMixerValueAsync(id, value, saveCts.Token);
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Err($"SaveMixerValue: Error in background task for Mixer {id}: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Main.logger.Err($"SaveMixerValue: Error starting save operation for Mixer {id}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static async Task SaveMixerValueAsync(int id, float value, CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                {
                    Main.logger.Msg(3, $"SaveMixerValueAsync: Cancellation requested for Mixer {id}");
                    return;
                }

                string path = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');

                if (string.IsNullOrEmpty(path))
                {
                    Main.logger.Warn(1, "SaveMixerValueAsync: Invalid file path");
                    return;
                }

                var saveData = new MixerThresholdSaveData
                {
                    MixerValues = new Dictionary<int, float> { { id, value } }
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                bool success = await SafeWriteAllTextWithCancellationAsync(path, json, ct);

                if (!success)
                {
                    Main.logger.Warn(1, $"SaveMixerValueAsync: Mixer value save was canceled or failed for Mixer {id}.");
                }
                else
                {
                    Main.logger.Msg(3, $"SaveMixerValueAsync: Successfully saved Mixer {id} value {value}");
                }
            }
            catch (OperationCanceledException)
            {
                Main.logger.Msg(3, $"SaveMixerValueAsync: Operation canceled for Mixer {id}");
            }
            catch (Exception ex)
            {
                Main.logger.Err($"SaveMixerValueAsync: Error saving mixer value for {id}: {ex.Message}\n{ex.StackTrace}");
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
                attempts++;
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

        private static void HandleMixerLoadingSafely()
        {
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
        }

        public static Dictionary<int, float> LoadMixerValues()
        {
            Main.logger.Msg(1, "Loading mixer values from file...");
            string saveDir = !string.IsNullOrEmpty(Main.CurrentSavePath)
                ? Path.GetFullPath(Main.CurrentSavePath)
                : MelonEnvironment.UserDataDirectory;
            string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
            if (!File.Exists(path))
            {
                Main.logger.Warn(1, "No mixer save file found. Returning empty dictionary.");
                return new Dictionary<int, float>();
            }
            try
            {
                string json = FileOperations.SafeReadAllText(path);
                var saveData = JsonConvert.DeserializeObject<MixerThresholdSaveData>(json);
                Main.logger.Msg(1, $"Loaded {saveData.MixerValues.Count} mixer values from: {path}");
                return saveData.MixerValues;
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Failed to load mixer values: {ex}");
                return new Dictionary<int, float>();
            }
        }

        public async static void LoadAndApplyMixerThresholds(string saveFolderPath)
        {
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
        }

        [Serializable]
        public class MixerThresholdSaveData
        {
            public Dictionary<int, float> MixerValues = new Dictionary<int, float>();
        }
    }
}