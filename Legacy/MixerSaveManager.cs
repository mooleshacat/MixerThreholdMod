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
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Comprehensive mixer save/load management system with crash prevention focus.
    /// Handles mixer value persistence, backup management, and event attachment.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This system is specifically designed to prevent 
    /// save corruption and data loss during crashes, repeated saves, and extended gameplay.
    /// 
    /// ⚠️ THREAD SAFETY: All save operations are thread-safe with proper locking mechanisms.
    /// Coroutines are used to prevent blocking Unity's main thread during file operations.
    /// 
    /// ⚠️ MAIN THREAD WARNING: Emergency save methods are designed for crash scenarios and 
    /// use blocking I/O. Regular save operations use coroutines to avoid main thread blocking.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible async patterns with proper ConfigureAwait usage
    /// - Manual dictionary operations instead of modern LINQ where needed
    /// - Proper exception handling and resource cleanup
    /// 
    /// Key Features:
    /// - Save cooldown system to prevent corruption from rapid saves
    /// - Automatic backup creation and cleanup (maintains 5 most recent)
    /// - Event attachment with multiple fallback strategies
    /// - Emergency save functionality for crash scenarios
    /// - Atomic file operations with temp file + rename strategy
    /// </summary>
    public static class MixerSaveManager
    {
        // Concurrency protection fields
        private static bool isBackupInProgress = false;
        private static readonly object backupLock = new object();
        private static bool isSaveInProgress = false;
        private static readonly object saveLock = new object();
        private static DateTime lastSaveTime = DateTime.MinValue;
        private static readonly TimeSpan SAVE_COOLDOWN = TimeSpan.FromSeconds(1);

        public static IEnumerator LoadMixerValuesWhenReady()
        {
            Main.logger.Msg(3, "LoadMixerValuesWhenReady: Started");

            // Wait until we have a valid save path - NO try-catch around yield
            while (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                yield return new WaitForSeconds(0.5f);
            }

            Main.logger.Msg(3, string.Format("LoadMixerValuesWhenReady: Save path available: {0}", Main.CurrentSavePath));

            // Perform loading on background thread to avoid blocking main thread
            bool loadCompleted = false;
            Exception loadError = null;

            Task.Run(async () =>
            {
                try
                {
                    await LoadMixerValuesFromFileAsync();
                    loadCompleted = true;
                }
                catch (Exception ex)
                {
                    loadError = ex;
                    loadCompleted = true;
                }
            });

            // Wait for background loading to complete - NO try-catch around yield
            while (!loadCompleted)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (loadError != null)
            {
                Main.logger.Err(string.Format("LoadMixerValuesWhenReady error: {0}\nStackTrace: {1}", loadError.Message, loadError.StackTrace));
            }

            Main.logger.Msg(3, "LoadMixerValuesWhenReady: Finished");
        }

        private static async Task LoadMixerValuesFromFileAsync()
        {
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

                if (File.Exists(saveFile))
                {
                    Main.logger.Msg(2, "LoadMixerValuesFromFileAsync: Loading saved mixer values");

                    string json = await ThreadSafeFileOperations.SafeReadAllTextAsync(saveFile);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                        if (data != null && data.ContainsKey("MixerValues"))
                        {
                            var mixerValues = JsonConvert.DeserializeObject<Dictionary<int, float>>(data["MixerValues"].ToString());

                            foreach (var kvp in mixerValues)
                            {
                                Main.savedMixerValues.TryAdd(kvp.Key, kvp.Value);
                            }

                            Main.logger.Msg(2, string.Format("LoadMixerValuesFromFileAsync: Loaded {0} mixer values", mixerValues.Count));
                        }
                    }
                }
                else
                {
                    Main.logger.Msg(2, "LoadMixerValuesFromFileAsync: No save file found, starting fresh");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("LoadMixerValuesFromFileAsync error: {0}", ex));
                throw;
            }
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

            // Attempt event attachment on background thread
            bool attachCompleted = false;
            bool eventAttached = false;
            Exception attachError = null;

            Task.Run(() =>
            {
                try
                {
                    eventAttached = TryAttachEventListener(config, mixerID);
                    attachCompleted = true;
                }
                catch (Exception ex)
                {
                    attachError = ex;
                    attachCompleted = true;
                }
            });

            // Wait for attachment attempt to complete - NO try-catch around yield
            while (!attachCompleted)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (attachError != null)
            {
                Main.logger.Err(string.Format("AttachListenerWhenReady error for Mixer {0}: {1}\nStackTrace: {2}", mixerID, attachError.Message, attachError.StackTrace));
            }

            // Fallback to polling if event attachment failed
            if (!eventAttached)
            {
                Main.logger.Msg(2, string.Format("AttachListenerWhenReady: Using polling method for Mixer {0}", mixerID));
                MelonCoroutines.Start(PollValueChanges(config, mixerID));
            }

            Main.logger.Msg(3, string.Format("AttachListenerWhenReady: Finished for Mixer {0}", mixerID));
        }

        private static bool TryAttachEventListener(MixingStationConfiguration config, int mixerID)
        {
            try
            {
                var numberFieldType = config.StartThrehold.GetType();
                Main.logger.Msg(3, string.Format("TryAttachEventListener: NumberField type: {0}", numberFieldType.Name));

                // Look for common event names
                var eventNames = new[] { "OnValueChanged", "ValueChanged", "onValueChanged", "OnChange", "Changed" };

                foreach (var eventName in eventNames)
                {
                    var eventInfo = numberFieldType.GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
                    if (eventInfo != null && eventInfo.EventHandlerType != null)
                    {
                        Main.logger.Msg(2, string.Format("TryAttachEventListener: Found event {0} for Mixer {1}", eventName, mixerID));

                        var handler = CreateEventHandler(eventInfo.EventHandlerType, mixerID);
                        if (handler != null)
                        {
                            eventInfo.AddEventHandler(config.StartThrehold, handler);
                            Main.logger.Msg(2, string.Format("TryAttachEventListener: Successfully attached to {0} event for Mixer {1}", eventName, mixerID));
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TryAttachEventListener error: {0}", ex));
                return false;
            }
        }

        private static Delegate CreateEventHandler(Type eventHandlerType, int mixerID)
        {
            try
            {
                if (eventHandlerType == typeof(Action<float>))
                {
                    return new Action<float>((float newValue) => OnValueChanged(mixerID, newValue));
                }
                else if (eventHandlerType == typeof(System.EventHandler))
                {
                    return new System.EventHandler((object sender, EventArgs e) => OnValueChangedGeneric(mixerID, sender));
                }
                else if (eventHandlerType.IsGenericType && eventHandlerType.GetGenericTypeDefinition() == typeof(Action<>))
                {
                    var paramType = eventHandlerType.GetGenericArguments()[0];
                    if (paramType == typeof(float))
                    {
                        return new Action<float>((float newValue) => OnValueChanged(mixerID, newValue));
                    }
                }

                Main.logger.Msg(3, string.Format("CreateEventHandler: Unsupported event handler type: {0}", eventHandlerType.Name));
                return null;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("CreateEventHandler error: {0}", ex.Message));
                return null;
            }
        }

        private static void OnValueChanged(int mixerID, float newValue)
        {
            try
            {
                float oldValue;
                bool hasOldValue = Main.savedMixerValues.TryGetValue(mixerID, out oldValue);

                if (!hasOldValue)
                {
                    Main.savedMixerValues.TryAdd(mixerID, newValue);
                }
                else
                {
                    Main.savedMixerValues.TryUpdate(mixerID, newValue, oldValue);
                }

                Main.logger.Msg(3, string.Format("Value changed for Mixer {0}: {1}", mixerID, newValue));

                // Start save coroutine
                MelonCoroutines.Start(SaveMixerValues());
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("OnValueChanged error for Mixer {0}: {1}", mixerID, ex));
            }
        }

        private static void OnValueChangedGeneric(int mixerID, object sender)
        {
            try
            {
                if (sender != null)
                {
                    var senderType = sender.GetType();
                    var valueProperty = senderType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

                    if (valueProperty != null && valueProperty.PropertyType == typeof(float))
                    {
                        float currentValue = (float)valueProperty.GetValue(sender, null);
                        OnValueChanged(mixerID, currentValue);
                    }
                    else
                    {
                        Main.logger.Msg(3, string.Format("OnValueChangedGeneric: Could not determine value for Mixer {0}", mixerID));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("OnValueChangedGeneric error for Mixer {0}: {1}", mixerID, ex));
            }
        }

        private static IEnumerator PollValueChanges(MixingStationConfiguration config, int mixerID)
        {
            Main.logger.Msg(3, string.Format("PollValueChanges: Started polling for Mixer {0}", mixerID));

            float lastKnownValue = -1f;
            bool hasInitialValue = false;

            while (config?.StartThrehold != null)
            {
                Exception pollError = null;
                float? currentValue = null;

                try
                {
                    currentValue = GetCurrentValue(config.StartThrehold);
                }
                catch (Exception ex)
                {
                    pollError = ex;
                }

                if (pollError == null && currentValue.HasValue)
                {
                    if (!hasInitialValue)
                    {
                        lastKnownValue = currentValue.Value;
                        hasInitialValue = true;
                        Main.logger.Msg(3, string.Format("PollValueChanges: Initial value for Mixer {0}: {1}", mixerID, lastKnownValue));
                    }
                    else if (Math.Abs(currentValue.Value - lastKnownValue) > 0.001f)
                    {
                        Main.logger.Msg(3, string.Format("PollValueChanges: Value changed for Mixer {0}: {1} -> {2}", mixerID, lastKnownValue, currentValue.Value));
                        lastKnownValue = currentValue.Value;
                        OnValueChanged(mixerID, currentValue.Value);
                    }
                }

                if (pollError != null)
                {
                    Main.logger.Err(string.Format("PollValueChanges error for Mixer {0}: {1}", mixerID, pollError.Message));
                }

                // Poll every 100ms - NO try-catch around yield
                yield return new WaitForSeconds(0.1f);
            }

            Main.logger.Msg(3, string.Format("PollValueChanges: Stopped polling for Mixer {0}", mixerID));
        }

        private static float? GetCurrentValue(object numberField)
        {
            try
            {
                if (numberField == null) return null;

                var type = numberField.GetType();

                var propertyNames = new[] { "Value", "CurrentValue", "GetValue", "value" };

                foreach (var propName in propertyNames)
                {
                    var property = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.PropertyType == typeof(float) && property.CanRead)
                    {
                        return (float)property.GetValue(numberField, null);
                    }
                }

                var methodNames = new[] { "GetValue", "getValue", "Value" };

                foreach (var methodName in methodNames)
                {
                    var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    if (method != null && method.ReturnType == typeof(float))
                    {
                        return (float)method.Invoke(numberField, null);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("GetCurrentValue error: {0}", ex.Message));
                return null;
            }
        }

        public static IEnumerator SaveMixerValues()
        {
            // Check cooldown period
            if (DateTime.Now - lastSaveTime < SAVE_COOLDOWN)
            {
                Main.logger.Msg(3, "SaveMixerValues: Skipping save due to cooldown period");
                yield break;
            }

            // Prevent concurrent saves
            bool canProceed = false;
            lock (saveLock)
            {
                if (!isSaveInProgress)
                {
                    isSaveInProgress = true;
                    lastSaveTime = DateTime.Now;
                    canProceed = true;
                }
            }

            if (!canProceed)
            {
                Main.logger.Warn(1, "SaveMixerValues: Already in progress, skipping duplicate call");
                yield break;
            }

            Main.logger.Msg(3, "SaveMixerValues: Started");

            // Pre-validate before any yield returns
            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                Main.logger.Warn(1, "SaveMixerValues: CurrentSavePath is null/empty, cannot save");
                lock (saveLock)
                {
                    isSaveInProgress = false;
                }
                yield break;
            }

            if (Main.savedMixerValues.Count == 0)
            {
                Main.logger.Msg(3, "SaveMixerValues: No mixer values to save");
                lock (saveLock)
                {
                    isSaveInProgress = false;
                }
                yield break;
            }

            // Check if backup is needed
            bool needsBackup = ShouldCreateBackup();

            if (needsBackup)
            {
                Main.logger.Msg(3, "SaveMixerValues: Starting backup coroutine");
                // NO try-catch around yield return for .NET 4.8.1
                yield return MelonCoroutines.Start(BackupSaveFolder());
                Main.logger.Msg(3, "SaveMixerValues: Backup coroutine completed");
            }
            else
            {
                Main.logger.Msg(3, "SaveMixerValues: Skipping backup (recent backup exists)");
            }

            // Perform save on background thread
            bool saveCompleted = false;
            Exception saveError = null;

            Task.Run(async () =>
            {
                try
                {
                    await SaveMixerValuesToFileAsync();
                    saveCompleted = true;
                }
                catch (Exception ex)
                {
                    saveError = ex;
                    saveCompleted = true;
                }
            });

            // Wait for save to complete - NO try-catch around yield
            while (!saveCompleted)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Always reset the flag
            lock (saveLock)
            {
                isSaveInProgress = false;
            }

            if (saveError != null)
            {
                Main.logger.Err(string.Format("SaveMixerValues error: {0}\nStackTrace: {1}", saveError.Message, saveError.StackTrace));
            }

            Main.logger.Msg(3, "SaveMixerValues: Finished and cleanup completed");
        }

        private static async Task SaveMixerValuesToFileAsync()
        {
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

                // Convert to regular dictionary for JSON serialization
                var mixerValuesDict = new Dictionary<int, float>();
                foreach (var kvp in Main.savedMixerValues)
                {
                    mixerValuesDict[kvp.Key] = kvp.Value;
                }

                var saveData = new Dictionary<string, object>
                {
                    ["MixerValues"] = mixerValuesDict,
                    ["SaveTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["Version"] = "1.0.0"
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                await ThreadSafeFileOperations.SafeWriteAllTextAsync(saveFile, json);

                Main.logger.Msg(2, string.Format("SaveMixerValuesToFileAsync: Saved {0} mixer values to {1}", Main.savedMixerValues.Count, saveFile));

                // Also save to persistent location
                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (!string.IsNullOrEmpty(persistentPath))
                {
                    string persistentFile = Path.Combine(persistentPath, "MixerThresholdSave.json");
                    await ThreadSafeFileOperations.SafeWriteAllTextAsync(persistentFile, json);
                    Main.logger.Msg(3, "SaveMixerValuesToFileAsync: Copied to persistent location");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("SaveMixerValuesToFileAsync error: {0}", ex));
                throw;
            }
        }

        public static IEnumerator BackupSaveFolder()
        {
            // Prevent multiple concurrent backup operations
            bool canProceed = false;
            lock (backupLock)
            {
                if (!isBackupInProgress)
                {
                    isBackupInProgress = true;
                    canProceed = true;
                }
            }

            if (!canProceed)
            {
                Main.logger.Warn(1, "BackupSaveFolder: Already in progress, skipping duplicate call");
                yield break;
            }

            Main.logger.Msg(3, "BackupSaveFolder: Started");

            // Pre-validate before any yield returns
            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                Main.logger.Warn(1, "BackupSaveFolder: CurrentSavePath is null/empty, cannot backup");
                lock (backupLock)
                {
                    isBackupInProgress = false;
                }
                yield break;
            }

            // Perform backup on background thread
            bool backupCompleted = false;
            Exception backupError = null;

            Task.Run(() =>
            {
                try
                {
                    PerformBackupOperations();
                    backupCompleted = true;
                }
                catch (Exception ex)
                {
                    backupError = ex;
                    backupCompleted = true;
                }
            });

            // Wait for backup to complete - NO try-catch around yield
            while (!backupCompleted)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Always reset the flag
            lock (backupLock)
            {
                isBackupInProgress = false;
            }

            if (backupError != null)
            {
                Main.logger.Err(string.Format("BackupSaveFolder error: {0}", backupError));
            }

            Main.logger.Msg(3, "BackupSaveFolder: Finished and cleanup completed");
        }

        private static void PerformBackupOperations()
        {
            string backupDir = Path.Combine(Main.CurrentSavePath, "MixerThresholdBackups");
            string sourceFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

            if (!File.Exists(sourceFile))
            {
                Main.logger.Msg(2, "PerformBackupOperations: Source file doesn't exist, no backup needed");
                return;
            }

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
                Main.logger.Msg(3, "PerformBackupOperations: Created backup directory");
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupFile = Path.Combine(backupDir, string.Format("MixerThresholdSave_backup_{0}.json", timestamp));

            File.Copy(sourceFile, backupFile, overwrite: true);
            Main.logger.Msg(2, string.Format("PerformBackupOperations: Backup created: {0}", backupFile));

            // Cleanup old backups
            var allBackupFiles = Directory.GetFiles(backupDir, "MixerThresholdSave_backup_*.json");
            if (allBackupFiles.Length > 5)
            {
                var sortedBackups = allBackupFiles.OrderByDescending(f => File.GetCreationTime(f)).ToList();
                var oldBackups = sortedBackups.Skip(5).ToList();

                foreach (var oldBackup in oldBackups)
                {
                    try
                    {
                        File.Delete(oldBackup);
                        Main.logger.Msg(3, string.Format("PerformBackupOperations: Deleted old backup: {0}", oldBackup));
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Warn(1, string.Format("PerformBackupOperations: Failed to delete old backup {0}: {1}", oldBackup, ex.Message));
                    }
                }
            }
        }

        private static bool ShouldCreateBackup()
        {
            try
            {
                if (string.IsNullOrEmpty(Main.CurrentSavePath)) return false;

                string backupDir = Path.Combine(Main.CurrentSavePath, "MixerThresholdBackups");
                if (!Directory.Exists(backupDir)) return true;

                var allBackupFiles = Directory.GetFiles(backupDir, "MixerThresholdSave_backup_*.json");
                var latestBackup = allBackupFiles.OrderByDescending(f => File.GetCreationTime(f)).FirstOrDefault();

                if (latestBackup == null) return true;

                // Only create backup if last one is older than 5 minutes
                return DateTime.Now - File.GetCreationTime(latestBackup) > TimeSpan.FromMinutes(5);
            }
            catch (Exception ex)
            {
                Main.logger.Warn(1, string.Format("ShouldCreateBackup: Error checking backup status: {0}", ex.Message));
                return false;
            }
        }

        // Emergency save method for crash scenarios - NO coroutines here
        public static void EmergencySave()
        {
            try
            {
                if (Main.savedMixerValues.Count == 0) return;

                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (string.IsNullOrEmpty(persistentPath)) return;

                string emergencyFile = Path.Combine(persistentPath, "MixerThresholdSave_Emergency.json");

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
                Main.logger.Err(string.Format("Emergency save failed: {0}", ex.Message));
            }
        }
    }
}