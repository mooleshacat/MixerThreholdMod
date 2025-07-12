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

            Exception loadError = null;
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

                if (File.Exists(saveFile))
                {
                    Main.logger.Msg(2, "LoadMixerValuesWhenReady: Loading saved mixer values");

                    string json = File.ReadAllText(saveFile);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    if (data != null && data.ContainsKey("MixerValues"))
                    {
                        var mixerValues = JsonConvert.DeserializeObject<Dictionary<int, float>>(data["MixerValues"].ToString());

                        foreach (var kvp in mixerValues)
                        {
                            // .NET 4.8.1 compatible ConcurrentDictionary usage
                            Main.savedMixerValues.TryAdd(kvp.Key, kvp.Value);
                        }

                        Main.logger.Msg(2, string.Format("LoadMixerValuesWhenReady: Loaded {0} mixer values", mixerValues.Count));
                    }
                }
                else
                {
                    Main.logger.Msg(2, "LoadMixerValuesWhenReady: No save file found, starting fresh");
                }
            }
            catch (Exception ex)
            {
                loadError = ex;
            }

            if (loadError != null)
            {
                Main.logger.Err(string.Format("LoadMixerValuesWhenReady error: {0}\nStackTrace: {1}", loadError.Message, loadError.StackTrace));
            }

            Main.logger.Msg(3, "LoadMixerValuesWhenReady: Finished");
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
                        Main.logger.Msg(2, string.Format("AttachListenerWhenReady: Found event {0} for Mixer {1}", eventName, mixerID));

                        // Create a delegate that matches the event signature
                        var handler = CreateEventHandler(eventInfo.EventHandlerType, mixerID);
                        if (handler != null)
                        {
                            eventInfo.AddEventHandler(config.StartThrehold, handler);
                            eventAttached = true;
                            Main.logger.Msg(2, string.Format("AttachListenerWhenReady: Successfully attached to {0} event for Mixer {1}", eventName, mixerID));
                            break;
                        }
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
                            Main.logger.Msg(3, string.Format("AttachListenerWhenReady: Found potential event field: {0} of type {1}", field.Name, field.FieldType.Name));
                        }
                    }
                }

                // Method 3: Fallback to polling if no events available
                if (!eventAttached)
                {
                    Main.logger.Msg(2, string.Format("AttachListenerWhenReady: No events found, using polling method for Mixer {0}", mixerID));
                    MelonCoroutines.Start(PollValueChanges(config, mixerID));
                    eventAttached = true; // Mark as handled
                }
            }
            catch (Exception ex)
            {
                attachError = ex;
            }

            if (attachError != null)
            {
                Main.logger.Err(string.Format("AttachListenerWhenReady error for Mixer {0}: {1}\nStackTrace: {2}", mixerID, attachError.Message, attachError.StackTrace));

                // Fallback to polling on error
                if (!eventAttached)
                {
                    Main.logger.Msg(2, string.Format("AttachListenerWhenReady: Falling back to polling for Mixer {0} due to error", mixerID));
                    MelonCoroutines.Start(PollValueChanges(config, mixerID));
                }
            }

            Main.logger.Msg(3, string.Format("AttachListenerWhenReady: Finished for Mixer {0}", mixerID));
        }

        private static Delegate CreateEventHandler(Type eventHandlerType, int mixerID)
        {
            try
            {
                // Try to create a handler for common event signatures
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
            Exception listenerError = null;
            try
            {
                // .NET 4.8.1 compatible - use manual update instead of AddOrUpdate
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
                listenerError = ex;
            }

            if (listenerError != null)
            {
                Main.logger.Err(string.Format("OnValueChanged error for Mixer {0}: {1}", mixerID, listenerError.Message));
            }
        }

        private static void OnValueChangedGeneric(int mixerID, object sender)
        {
            Exception listenerError = null;
            try
            {
                // Try to get the current value from the sender
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
                listenerError = ex;
            }

            if (listenerError != null)
            {
                Main.logger.Err(string.Format("OnValueChangedGeneric error for Mixer {0}: {1}", mixerID, listenerError.Message));
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
                try
                {
                    // Try to get current value using reflection
                    var currentValue = GetCurrentValue(config.StartThrehold);

                    if (currentValue.HasValue)
                    {
                        if (!hasInitialValue)
                        {
                            lastKnownValue = currentValue.Value;
                            hasInitialValue = true;
                            Main.logger.Msg(3, string.Format("PollValueChanges: Initial value for Mixer {0}: {1}", mixerID, lastKnownValue));
                        }
                        else if (Math.Abs(currentValue.Value - lastKnownValue) > 0.001f) // Small threshold for float comparison
                        {
                            Main.logger.Msg(3, string.Format("PollValueChanges: Value changed for Mixer {0}: {1} -> {2}", mixerID, lastKnownValue, currentValue.Value));
                            lastKnownValue = currentValue.Value;
                            OnValueChanged(mixerID, currentValue.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    pollError = ex;
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

                // Try common property names for getting the current value
                var propertyNames = new[] { "Value", "CurrentValue", "GetValue", "value" };

                foreach (var propName in propertyNames)
                {
                    var property = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.PropertyType == typeof(float) && property.CanRead)
                    {
                        return (float)property.GetValue(numberField, null);
                    }
                }

                // Try common method names
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

            // Perform the actual save
            Exception saveError = null;
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

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
                    ["Version"] = "0.0.1"
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                // Write to temp file first, then rename for atomic operation
                string tempFile = saveFile + ".tmp";
                File.WriteAllText(tempFile, json);

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

            // CRITICAL: Always reset the flag
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

            // Add timeout mechanism to prevent infinite loops
            float startTime = Time.time;
            const float BACKUP_TIMEOUT = 10f; // 10 seconds max

            string backupDir = Path.Combine(Main.CurrentSavePath, "MixerThresholdBackups");
            string sourceFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

            // Check if source file exists with timeout - NO try-catch around yield
            while (!File.Exists(sourceFile) && (Time.time - startTime) < BACKUP_TIMEOUT)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (!File.Exists(sourceFile))
            {
                Main.logger.Msg(2, "BackupSaveFolder: Source file doesn't exist, no backup needed");
                lock (backupLock)
                {
                    isBackupInProgress = false;
                }
                yield break;
            }

            // Create backup directory with error handling
            Exception dirError = null;
            try
            {
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                    Main.logger.Msg(3, "BackupSaveFolder: Created backup directory");
                }
            }
            catch (Exception ex)
            {
                dirError = ex;
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

        private static IEnumerator PerformCleanupCoroutine(string backupDir, float startTime, float timeoutSeconds)
        {
            Main.logger.Msg(3, "PerformCleanupCoroutine: Starting cleanup of old backups");

            // Get list of backup files outside of try-catch
            string[] allBackupFiles = null;
            Exception listError = null;

            try
            {
                allBackupFiles = Directory.GetFiles(backupDir, "MixerThresholdSave_backup_*.json");
            }
            catch (Exception ex)
            {
                listError = ex;
            }

            if (listError != null)
            {
                Main.logger.Warn(1, string.Format("PerformCleanupCoroutine: Error getting backup file list: {0}", listError.Message));
                yield break;
            }

            if (allBackupFiles == null || allBackupFiles.Length <= 5)
            {
                Main.logger.Msg(3, "PerformCleanupCoroutine: No cleanup needed, backup count is acceptable");
                yield break;
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

        private static bool ShouldCreateBackup()
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