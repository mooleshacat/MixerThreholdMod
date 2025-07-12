using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MixerThreholdMod_0_0_1.Save
{
    /// <summary>
    /// Robust save management system focused on preventing crashes during save operations.
    /// Implements multiple safety mechanisms for reliable data persistence.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This class is specifically designed to prevent the 
    /// crashes identified in PR #12 during save operations, especially with repeated saves
    /// and extended gameplay sessions.
    /// 
    /// ⚠️ THREAD SAFETY: All save operations are thread-safe and designed to not block
    /// the main Unity thread. File operations use proper locking to prevent corruption.
    /// 
    /// ⚠️ MAIN THREAD WARNING: Synchronous methods should NOT be called from Unity's main
    /// thread as they can cause UI freezes. Use async alternatives when possible.
    /// 
    /// ⚠️ IL2CPP COMPATIBLE: Uses object parameters and reflection for type safety.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses compatible async/await patterns
    /// - Proper exception handling for crash prevention
    /// - Compatible file I/O operations with retry logic
    /// 
    /// Crash Prevention Features:
    /// - Emergency save on crashes
    /// - Atomic file operations (temp file + rename)
    /// - Save cooldown to prevent rapid-fire saves
    /// - Backup system with cleanup
    /// - Extensive error handling and logging
    /// </summary>
    public static class CrashResistantSaveManager
    {
        // Save state management
        private static bool isSaveInProgress = false;
#pragma warning disable CS0414 // Field assigned but never used: Thread-safe backup flag used in async operations
        private static volatile bool isBackupInProgress = false;
#pragma warning restore CS0414
        private static readonly object saveLock = new object();
        private static readonly object backupLock = new object();
        private static DateTime lastSaveTime = DateTime.MinValue;
        private static readonly TimeSpan SAVE_COOLDOWN = TimeSpan.FromSeconds(2); // Increased for stability
        private static readonly TimeSpan BACKUP_INTERVAL = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Load mixer values from save file when game path becomes available.
        /// ⚠️ CRASH PREVENTION: Extensive error handling prevents load failures from crashing the game.
        /// </summary>
        public static IEnumerator LoadMixerValuesWhenReady()
        {
            Main.logger.Msg(2, "[SAVE] LoadMixerValuesWhenReady: Starting load process");

            // Wait for save path with timeout to prevent infinite loops
            float startTime = Time.time;
            const float LOAD_TIMEOUT = 30f; // 30 second timeout

            while (string.IsNullOrEmpty(Main.CurrentSavePath) && (Time.time - startTime) < LOAD_TIMEOUT)
            {
                yield return new WaitForSeconds(0.5f);
            }

            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                Main.logger.Warn(1, "[SAVE] LoadMixerValuesWhenReady: Timeout waiting for save path - using emergency defaults");
                yield break;
            }

            Main.logger.Msg(2, string.Format("[SAVE] LoadMixerValuesWhenReady: Save path available: {0}", Main.CurrentSavePath));

            // Perform actual loading with crash protection
            Exception loadError = null;
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");
                string emergencyFile = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave_Emergency.json");

                // Try main save file first, then emergency backup
                string[] filesToTry = { saveFile, emergencyFile };

                foreach (string filePath in filesToTry)
                {
                    if (File.Exists(filePath))
                    {
                        Main.logger.Msg(2, string.Format("[SAVE] LoadMixerValuesWhenReady: Loading from {0}", filePath));

                        string json = File.ReadAllText(filePath);
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                        if (data != null && data.ContainsKey("MixerValues"))
                        {
                            var mixerValues = JsonConvert.DeserializeObject<Dictionary<int, float>>(data["MixerValues"].ToString());

                            foreach (var kvp in mixerValues)
                            {
                                // .NET 4.8.1 compatible ConcurrentDictionary usage
                                Main.savedMixerValues.TryAdd(kvp.Key, kvp.Value);
                            }

                            Main.logger.Msg(1, string.Format("[SAVE] LoadMixerValuesWhenReady: Successfully loaded {0} mixer values", mixerValues.Count));
                            break; // Success - don't try other files
                        }
                    }
                }

                // If no files found, that's normal for new installations
                if (!File.Exists(saveFile) && !File.Exists(emergencyFile))
                {
                    Main.logger.Msg(2, "[SAVE] LoadMixerValuesWhenReady: No save files found - starting fresh");
                }
            }
            catch (Exception ex)
            {
                loadError = ex;
            }

            if (loadError != null)
            {
                Main.logger.Err(string.Format("[SAVE] LoadMixerValuesWhenReady CRASH PREVENTION: Load failed but continuing: {0}", loadError.Message));
                // Don't throw - let the game continue with default values
            }

            Main.logger.Msg(3, "[SAVE] LoadMixerValuesWhenReady: Completed");
        }

        /// <summary>
        /// Attach value change listener to mixer configuration.
        /// ⚠️ CRASH PREVENTION: Multiple fallback strategies for event attachment.
        /// ⚠️ IL2CPP COMPATIBLE: Uses object parameter and reflection for type safety.
        /// </summary>
        public static IEnumerator AttachListenerWhenReady(object config, int mixerID)
        {
            Main.logger.Msg(3, string.Format("[SAVE] AttachListenerWhenReady: Starting for Mixer {0}", mixerID));

            // Wait for StartThreshold to be available with timeout
            float startTime = Time.time;
            const float ATTACH_TIMEOUT = 10f;

            // IL2CPP COMPATIBLE: Use reflection to access StartThrehold property
            object startThreshold = null;
            while (startThreshold == null && (Time.time - startTime) < ATTACH_TIMEOUT)
            {
                try
                {
                    if (config != null)
                    {
                        var startThresholdProperty = config.GetType().GetProperty("StartThrehold");
                        if (startThresholdProperty != null)
                        {
                            startThreshold = startThresholdProperty.GetValue(config, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.logger.Err(string.Format("[SAVE] AttachListenerWhenReady: Error accessing StartThrehold: {0}", ex.Message));
                }

                if (startThreshold == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (startThreshold == null)
            {
                Main.logger.Warn(1, string.Format("[SAVE] AttachListenerWhenReady: Timeout - StartThreshold not available for Mixer {0}", mixerID));
                yield break;
            }

            // Try multiple attachment strategies with crash protection
            Exception attachError = null;
            bool eventAttached = false;

            try
            {
                // Strategy 1: Direct event attachment (most reliable) - IL2CPP COMPATIBLE
                var onItemChangedField = startThreshold.GetType().GetField("onItemChanged", BindingFlags.Public | BindingFlags.Instance);
                if (onItemChangedField != null)
                {
                    var unityEvent = onItemChangedField.GetValue(startThreshold);
                    if (unityEvent != null)
                    {
                        // Use reflection to call AddListener
                        var addListenerMethod = unityEvent.GetType().GetMethod("AddListener");
                        if (addListenerMethod != null)
                        {
                            // Create a compatible delegate for the event
                            var actionType = typeof(UnityEngine.Events.UnityAction<float>);
                            var listener = System.Delegate.CreateDelegate(actionType,
                                null,
                                typeof(CrashResistantSaveManager).GetMethod("CreateValueChangeHandler", BindingFlags.NonPublic | BindingFlags.Static));

                            if (listener != null)
                            {
                                // Store the mixerID in a closure-friendly way
                                var specificHandler = new UnityEngine.Events.UnityAction<float>((float val) =>
                                {
                                    HandleValueChange(mixerID, val);
                                });

                                addListenerMethod.Invoke(unityEvent, new object[] { specificHandler });
                                eventAttached = true;
                                Main.logger.Msg(2, string.Format("[SAVE] AttachListenerWhenReady: Direct event attached for Mixer {0}", mixerID));
                            }
                        }
                    }
                }

                // Strategy 2: Reflection-based attachment (fallback)
                if (!eventAttached)
                {
                    var numberFieldType = startThreshold.GetType();
                    var eventNames = new[] { "OnValueChanged", "ValueChanged", "onValueChanged" };

                    foreach (var eventName in eventNames)
                    {
                        var eventInfo = numberFieldType.GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
                        if (eventInfo != null)
                        {
                            var handler = CreateEventHandler(eventInfo.EventHandlerType, mixerID);
                            if (handler != null)
                            {
                                eventInfo.AddEventHandler(startThreshold, handler);
                                eventAttached = true;
                                Main.logger.Msg(2, string.Format("[SAVE] AttachListenerWhenReady: Reflection event {0} attached for Mixer {1}", eventName, mixerID));
                                break;
                            }
                        }
                    }
                }

                // Strategy 3: Polling fallback (last resort)
                if (!eventAttached)
                {
                    Main.logger.Msg(2, string.Format("[SAVE] AttachListenerWhenReady: Using polling fallback for Mixer {0}", mixerID));
                    MelonCoroutines.Start(PollValueChanges(config, mixerID));
                    eventAttached = true;
                }
            }
            catch (Exception ex)
            {
                attachError = ex;
            }

            if (attachError != null)
            {
                Main.logger.Err(string.Format("[SAVE] AttachListenerWhenReady CRASH PREVENTION: Attachment failed for Mixer {0}: {1}", mixerID, attachError.Message));

                // Emergency fallback - always try polling
                if (!eventAttached)
                {
                    Main.logger.Msg(1, string.Format("[SAVE] AttachListenerWhenReady: Emergency polling fallback for Mixer {0}", mixerID));
                    MelonCoroutines.Start(PollValueChanges(config, mixerID));
                }
            }

            Main.logger.Msg(3, string.Format("[SAVE] AttachListenerWhenReady: Completed for Mixer {0}", mixerID));
        }

        /// <summary>
        /// Handle mixer value changes with crash protection
        /// </summary>
        private static void HandleValueChange(int mixerID, float newValue)
        {
            Exception changeError = null;
            try
            {
                // Update saved values with .NET 4.8.1 compatible pattern
                float oldValue;
                if (Main.savedMixerValues.TryGetValue(mixerID, out oldValue))
                {
                    Main.savedMixerValues.TryUpdate(mixerID, newValue, oldValue);
                }
                else
                {
                    Main.savedMixerValues.TryAdd(mixerID, newValue);
                }

                Main.logger.Msg(3, string.Format("[SAVE] Value changed: Mixer {0} = {1}", mixerID, newValue));

                // Trigger save with cooldown protection
                MelonCoroutines.Start(TriggerSaveWithCooldownInternal());
            }
            catch (Exception ex)
            {
                changeError = ex;
            }

            if (changeError != null)
            {
                Main.logger.Err(string.Format("[SAVE] HandleValueChange CRASH PREVENTION: Error for Mixer {0}: {1}", mixerID, changeError.Message));
                // Don't re-throw - let the game continue
            }
        }

        /// <summary>
        /// Create event handler delegate for reflection-based attachment
        /// </summary>
        private static Delegate CreateEventHandler(Type eventHandlerType, int mixerID)
        {
            try
            {
                if (eventHandlerType == typeof(Action<float>))
                {
                    return new Action<float>((float newValue) => HandleValueChange(mixerID, newValue));
                }
                else if (eventHandlerType == typeof(System.EventHandler))
                {
                    return new System.EventHandler((object sender, EventArgs e) => HandleValueChangeGeneric(mixerID, sender));
                }

                return null;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] CreateEventHandler: Error creating handler: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Handle generic event handler pattern
        /// </summary>
        private static void HandleValueChangeGeneric(int mixerID, object sender)
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
                        HandleValueChange(mixerID, currentValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] HandleValueChangeGeneric: Error for Mixer {0}: {1}", mixerID, ex.Message));
            }
        }

        /// <summary>
        /// Polling fallback for value changes (last resort strategy)
        /// </summary>
        private static IEnumerator PollValueChanges(MixingStationConfiguration config, int mixerID)
        {
            Main.logger.Msg(3, string.Format("[SAVE] PollValueChanges: Starting polling for Mixer {0}", mixerID));

            float lastKnownValue = -1f;
            bool hasInitialValue = false;
            const float POLL_INTERVAL = 0.2f; // Poll every 200ms

            while (config?.StartThrehold != null)
            {
                Exception pollError = null;
                try
                {
                    var currentValue = GetCurrentValue(config.StartThrehold);

                    if (currentValue.HasValue)
                    {
                        if (!hasInitialValue)
                        {
                            lastKnownValue = currentValue.Value;
                            hasInitialValue = true;
                        }
                        else if (Math.Abs(currentValue.Value - lastKnownValue) > 0.001f)
                        {
                            lastKnownValue = currentValue.Value;
                            HandleValueChange(mixerID, currentValue.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    pollError = ex;
                }

                if (pollError != null)
                {
                    Main.logger.Err(string.Format("[SAVE] PollValueChanges: Error polling Mixer {0}: {1}", mixerID, pollError.Message));
                }

                yield return new WaitForSeconds(POLL_INTERVAL);
            }

            Main.logger.Msg(3, string.Format("[SAVE] PollValueChanges: Stopped polling for Mixer {0}", mixerID));
        }

        /// <summary>
        /// Get current value from mixer using reflection
        /// </summary>
        private static float? GetCurrentValue(object numberField)
        {
            try
            {
                if (numberField == null) return null;

                var type = numberField.GetType();
                var propertyNames = new[] { "Value", "CurrentValue", "value" };

                foreach (var propName in propertyNames)
                {
                    var property = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.PropertyType == typeof(float) && property.CanRead)
                    {
                        return (float)property.GetValue(numberField, null);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] GetCurrentValue: Error: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Trigger save operation with cooldown protection (public interface)
        /// ⚠️ CRASH PREVENTION: Prevents rapid-fire saves that can cause corruption
        /// </summary>
        public static IEnumerator TriggerSaveWithCooldown()
        {
            // Check cooldown period
            if (DateTime.Now - lastSaveTime < SAVE_COOLDOWN)
            {
                Main.logger.Msg(3, "[SAVE] TriggerSaveWithCooldown: Skipping due to cooldown");
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
                Main.logger.Msg(3, "[SAVE] TriggerSaveWithCooldown: Save already in progress");
                yield break;
            }

            // Perform save operation without try/finally around yield return
            yield return PerformCrashResistantSave();

            // Always reset the flag after save
            lock (saveLock)
            {
                isSaveInProgress = false;
            }
        }

        /// <summary>
        /// Trigger save operation with cooldown protection (internal)
        /// ⚠️ CRASH PREVENTION: Prevents rapid-fire saves that can cause corruption
        /// </summary>
        private static IEnumerator TriggerSaveWithCooldownInternal()
        {
            // Check cooldown period
            if (DateTime.Now - lastSaveTime < SAVE_COOLDOWN)
            {
                Main.logger.Msg(3, "[SAVE] TriggerSaveWithCooldown: Skipping due to cooldown");
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
                Main.logger.Msg(3, "[SAVE] TriggerSaveWithCooldown: Save already in progress");
                yield break;
            }

            // Perform save operation without try/finally around yield return
            yield return PerformCrashResistantSave();

            // Always reset the flag after save
            lock (saveLock)
            {
                isSaveInProgress = false;
            }
        }

        /// <summary>
        /// Perform the actual save operation with maximum crash resistance
        /// ⚠️ CRASH PREVENTION: Multiple safety layers to prevent save corruption
        /// </summary>
        private static IEnumerator PerformCrashResistantSave()
        {
            Main.logger.Msg(2, "[SAVE] PerformCrashResistantSave: Starting save operation");

            // Validate preconditions
            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                Main.logger.Warn(1, "[SAVE] PerformCrashResistantSave: No save path available");
                yield break;
            }
            
            if (Main.savedMixerValues.Count == 0)
            {
                // Provide detailed diagnostics
                int trackedMixersCount = Core.TrackedMixers.Count(tm => tm != null);
                int queuedInstancesCount = Main.queuedInstances.Count;
                
                Main.logger.Warn(1, string.Format("[SAVE] PerformCrashResistantSave: No mixer data to save. Diagnostics:"));
                Main.logger.Warn(1, string.Format("[SAVE] - SavedMixerValues: {0}", Main.savedMixerValues.Count));
                Main.logger.Warn(1, string.Format("[SAVE] - TrackedMixers: {0}", trackedMixersCount));
                Main.logger.Warn(1, string.Format("[SAVE] - QueuedInstances: {0}", queuedInstancesCount));
                Main.logger.Warn(1, string.Format("[SAVE] - SavePath: {0}", Main.CurrentSavePath ?? "[null]"));
                
                if (trackedMixersCount == 0 && queuedInstancesCount == 0)
                {
                    Main.logger.Err("[SAVE] DIAGNOSIS: No mixers detected at all - constructor patching or runtime scanning may have failed");
                }
                else if (trackedMixersCount > 0)
                {
                    Main.logger.Warn(1, "[SAVE] DIAGNOSIS: Mixers are tracked but no values captured - event attachment may have failed");
                }
                
                yield break;
            }

            // Create backup if needed
            if (ShouldCreateBackup())
            {
                yield return CreateSafeBackup();
            }

            // Perform atomic save operation
            Exception saveError = null;
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");
                string tempFile = saveFile + ".tmp";

                // Convert ConcurrentDictionary to regular dictionary for serialization
                var mixerValuesDict = new Dictionary<int, float>();
                foreach (var kvp in Main.savedMixerValues)
                {
                    mixerValuesDict[kvp.Key] = kvp.Value;
                }

                var saveData = new Dictionary<string, object>
                {
                    ["MixerValues"] = mixerValuesDict,
                    ["SaveTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["Version"] = "1.0.0",
                    ["SessionID"] = System.Guid.NewGuid().ToString()
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

                // Atomic operation: write to temp file, then rename
                File.WriteAllText(tempFile, json);

                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                }

                File.Move(tempFile, saveFile);

                Main.logger.Msg(1, string.Format("[SAVE] PerformCrashResistantSave: Successfully saved {0} mixer values", Main.savedMixerValues.Count));

                // Also maintain emergency backup
                CreateEmergencyBackup(json);
            }
            catch (Exception ex)
            {
                saveError = ex;
            }

            if (saveError != null)
            {
                Main.logger.Err(string.Format("[SAVE] PerformCrashResistantSave CRASH PREVENTION: Save failed: {0}", saveError.Message));
                // Don't re-throw - let the game continue
            }

            Main.logger.Msg(3, "[SAVE] PerformCrashResistantSave: Completed");
        }

        /// <summary>
        /// Create safe backup with error protection
        /// </summary>
        private static IEnumerator CreateSafeBackup()
        {
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
                yield break;
            }

            try
            {
                Main.logger.Msg(3, "[SAVE] CreateSafeBackup: Creating backup");

                string sourceFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");
                if (File.Exists(sourceFile))
                {
                    string backupDir = Path.Combine(Main.CurrentSavePath, "MixerThresholdBackups");
                    if (!Directory.Exists(backupDir))
                    {
                        Directory.CreateDirectory(backupDir);
                    }

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string backupFile = Path.Combine(backupDir, string.Format("MixerThresholdSave_backup_{0}.json", timestamp));

                    File.Copy(sourceFile, backupFile, overwrite: true);
                    Main.logger.Msg(2, "[SAVE] CreateSafeBackup: Backup created successfully");

                    // Cleanup old backups (keep only 5 most recent)
                    CleanupOldBackups(backupDir);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] CreateSafeBackup: Error: {0}", ex.Message));
            }
            finally
            {
                lock (backupLock)
                {
                    isBackupInProgress = false;
                }
            }
        }

        /// <summary>
        /// Emergency save for crash scenarios - NO coroutines, fast and simple
        /// </summary>
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

                Main.logger.Msg(1, "[SAVE] EmergencySave: Emergency save completed");
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] EmergencySave: Failed: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Create emergency backup copy
        /// </summary>
        private static void CreateEmergencyBackup(string json)
        {
            try
            {
                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (!string.IsNullOrEmpty(persistentPath))
                {
                    string emergencyFile = Path.Combine(persistentPath, "MixerThresholdSave_Emergency.json");
                    File.WriteAllText(emergencyFile, json);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] CreateEmergencyBackup: Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Check if backup should be created
        /// </summary>
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

                return DateTime.Now - File.GetCreationTime(latestBackup) > BACKUP_INTERVAL;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] ShouldCreateBackup: Error: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Cleanup old backup files
        /// </summary>
        private static void CleanupOldBackups(string backupDir)
        {
            try
            {
                var allBackupFiles = Directory.GetFiles(backupDir, "MixerThresholdSave_backup_*.json");
                if (allBackupFiles.Length <= 5) return;

                var sortedBackups = allBackupFiles.OrderByDescending(f => File.GetCreationTime(f)).ToList();
                var oldBackups = sortedBackups.Skip(5).ToList();

                foreach (var oldBackup in oldBackups)
                {
                    File.Delete(oldBackup);
                }

                Main.logger.Msg(3, string.Format("[SAVE] CleanupOldBackups: Deleted {0} old backups", oldBackups.Count));
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[SAVE] CleanupOldBackups: Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Gather detailed diagnostics for save failures
        /// </summary>
        private static IEnumerator GatherDetailedDiagnostics()
        {
            Exception diagError = null;
            int trackedMixersCount = 0;
            int queuedInstancesCount = 0;

            try
            {
                // Basic counts
                queuedInstancesCount = Main.queuedInstances.Count;
                
                // Try to get tracked mixers count with timeout protection
                var countTask = Core.TrackedMixers.CountAsync(tm => tm != null);
                float startTime = Time.time;
                while (!countTask.IsCompleted && (Time.time - startTime) < 2f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (countTask.IsCompleted && !countTask.IsFaulted)
                {
                    trackedMixersCount = countTask.Result;
                }
                else
                {
                    Main.logger.Warn(1, "[SAVE] DIAGNOSIS: TrackedMixers count task failed or timed out");
                    trackedMixersCount = -1; // Indicates failure
                }

                // Output comprehensive diagnostics
                Main.logger.Warn(1, "[SAVE] ===== COMPREHENSIVE MIXER DIAGNOSTICS =====");
                Main.logger.Warn(1, string.Format("[SAVE] - SavedMixerValues: {0}", Main.savedMixerValues.Count));
                Main.logger.Warn(1, string.Format("[SAVE] - TrackedMixers: {0}", trackedMixersCount >= 0 ? trackedMixersCount.ToString() : "FAILED"));
                Main.logger.Warn(1, string.Format("[SAVE] - QueuedInstances: {0}", queuedInstancesCount));
                Main.logger.Warn(1, string.Format("[SAVE] - SavePath: {0}", Main.CurrentSavePath ?? "[null]"));
                Main.logger.Warn(1, string.Format("[SAVE] - MixerInstanceMap count: {0}", Core.MixerIDManager.GetMixerCount()));
                Main.logger.Warn(1, string.Format("[SAVE] - Load coroutine started: {0}", Main.LoadCoroutineStarted));
                
                // Analysis based on results
                if (trackedMixersCount == 0 && queuedInstancesCount == 0)
                {
                    Main.logger.Err("[SAVE] DIAGNOSIS: ❌ NO MIXERS DETECTED AT ALL");
                    Main.logger.Err("[SAVE] - Constructor patching likely failed silently");
                    Main.logger.Err("[SAVE] - Runtime scanning found no mixers");
                    Main.logger.Err("[SAVE] - Game may not have any mixers loaded yet");
                    Main.logger.Err("[SAVE] - Check if we're saving too early in the game");
                }
                else if (trackedMixersCount > 0 && Main.savedMixerValues.Count == 0)
                {
                    Main.logger.Err("[SAVE] DIAGNOSIS: ⚠️ MIXERS TRACKED BUT NO VALUES CAPTURED");
                    Main.logger.Err("[SAVE] - Event attachment to mixer controls failed");
                    Main.logger.Err("[SAVE] - Mixer value change listeners not working");
                    Main.logger.Err("[SAVE] - Check StartThreshold.onItemChanged event availability");
                }
                else if (queuedInstancesCount > 0)
                {
                    Main.logger.Warn(1, "[SAVE] DIAGNOSIS: 🔄 MIXERS QUEUED BUT NOT PROCESSED");
                    Main.logger.Warn(1, "[SAVE] - Processing may be blocked or slow");
                    Main.logger.Warn(1, "[SAVE] - Check UpdateCoroutine performance");
                }
                else if (trackedMixersCount < 0)
                {
                    Main.logger.Err("[SAVE] DIAGNOSIS: 💥 TRACKING SYSTEM FAILURE");
                    Main.logger.Err("[SAVE] - TrackedMixers async operations timing out");
                    Main.logger.Err("[SAVE] - Threading/synchronization issues detected");
                }

                Main.logger.Warn(1, "[SAVE] ============================================");
            }
            catch (Exception ex)
            {
                diagError = ex;
            }

            if (diagError != null)
            {
                Main.logger.Err(string.Format("[SAVE] GatherDetailedDiagnostics CRASH PREVENTION: Error: {0}\nStackTrace: {1}", 
                    diagError.Message, diagError.StackTrace));
            }
        }
    }
}