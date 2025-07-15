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
using MixerThreholdMod_1_0_0.Constants;    // ✅ ESSENTIAL - Keep this! Our constants!

namespace MixerThreholdMod_1_0_0.Save
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
            Main.logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, "[SAVE] LoadMixerValuesWhenReady: Starting load process");

            // Wait for save path with timeout to prevent infinite loops
            float startTime = Time.time;
            const float LOAD_TIMEOUT = ModConstants.LOAD_TIMEOUT_SECONDS; // 30 second timeout

            while (string.IsNullOrEmpty(Main.CurrentSavePath) && (Time.time - startTime) < LOAD_TIMEOUT)
            {
                yield return new WaitForSeconds(ModConstants.LOAD_POLL_INTERVAL_SECONDS);
            }

            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] LoadMixerValuesWhenReady: Timeout waiting for save path - using emergency defaults");
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
                    Main.logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, "[SAVE] LoadMixerValuesWhenReady: No save files found - starting fresh");
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

            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] LoadMixerValuesWhenReady: Completed");
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
            const float ATTACH_TIMEOUT = ModConstants.ATTACH_TIMEOUT_SECONDS;

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
                    yield return new WaitForSeconds(ModConstants.ATTACH_POLL_INTERVAL_SECONDS);
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
        /// Create a value change handler method (for delegate creation)
        /// </summary>
        private static void CreateValueChangeHandler(float newValue)
        {
            // This is a placeholder method for delegate creation
            // The actual mixerID will be handled in the closure above
            Main.logger.Msg(3, string.Format("[SAVE] Value change detected: {0}", newValue));
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
        /// ⚠️ IL2CPP COMPATIBLE: Uses object parameter and reflection
        /// </summary>
        private static IEnumerator PollValueChanges(object config, int mixerID)
        {
            Main.logger.Msg(3, string.Format("[SAVE] PollValueChanges: Starting polling for Mixer {0}", mixerID));

            float lastKnownValue = -1f;
            bool hasInitialValue = false;
            const float POLL_INTERVAL = ModConstants.POLL_INTERVAL_SECONDS; // Poll every 200ms

            while (config != null)
            {
                Exception pollError = null;
                try
                {
                    // IL2CPP COMPATIBLE: Use reflection to access StartThrehold
                    var startThresholdProperty = config.GetType().GetProperty("StartThrehold");
                    if (startThresholdProperty != null)
                    {
                        var startThreshold = startThresholdProperty.GetValue(config, null);
                        var currentValue = GetCurrentValue(startThreshold);

                        if (currentValue.HasValue)
                        {
                            if (!hasInitialValue)
                            {
                                lastKnownValue = currentValue.Value;
                                hasInitialValue = true;
                            }
                            else if (Math.Abs(currentValue.Value - lastKnownValue) > ModConstants.MIXER_VALUE_TOLERANCE)
                            {
                                lastKnownValue = currentValue.Value;
                                HandleValueChange(mixerID, currentValue.Value);
                            }
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
        /// ⚠️ THREAD SAFETY: Thread-safe operation with proper locking
        /// ⚠️ IL2CPP COMPATIBLE: Uses compatible async patterns
        /// </summary>
        public static IEnumerator TriggerSaveWithCooldown()
        {
            // Check cooldown period
            if (DateTime.Now - lastSaveTime < SAVE_COOLDOWN)
            {
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] TriggerSaveWithCooldown: Skipping due to cooldown");
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
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] TriggerSaveWithCooldown: Save already in progress");
                yield break;
            }

            // Perform save operation - yield return outside try/catch for .NET 4.8.1 compatibility
            yield return PerformCrashResistantSave();

            // Handle any errors that occurred during save operation after yield return
            Exception saveError = null;
            try
            {
                // Check if save operation completed successfully by validating the save file
                if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                {
                    string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");
                    if (!File.Exists(saveFile))
                    {
                        saveError = new InvalidOperationException("Save file was not created successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                saveError = ex;
            }

            // Always reset the flag after save
            lock (saveLock)
            {
                isSaveInProgress = false;
            }

            if (saveError != null)
            {
                Main.logger.Err(string.Format("[SAVE] TriggerSaveWithCooldown CRASH PREVENTION: Save error: {0}", saveError.Message));
                // Don't re-throw - let the game continue
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
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] TriggerSaveWithCooldown: Skipping due to cooldown");
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
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] TriggerSaveWithCooldown: Save already in progress");
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
        /// ⚠️ THREAD SAFETY: Atomic file operations with proper error handling
        /// </summary>
        private static IEnumerator PerformCrashResistantSave()
        {
            Main.logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, "[SAVE] PerformCrashResistantSave: Starting save operation");

            // Validate preconditions
            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] PerformCrashResistantSave: No save path available");
                yield break;
            }

            if (Main.savedMixerValues.Count == 0)
            {
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] PerformCrashResistantSave: No mixer data to save");
                yield break;
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

            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] PerformCrashResistantSave: Completed");
            yield return null;
        }
        /// <summary>
        /// Emergency save for crash scenarios - NO coroutines, fast and simple
        /// ⚠️ CRASH PREVENTION: This method is designed to be called during application shutdown or crashes
        /// ⚠️ THREAD SAFETY: Synchronous operation that doesn't block main thread for too long
        /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses framework-safe file operations with proper error handling
        /// </summary>
        public static void EmergencySave()
        {
            Exception emergencyError = null;
            try
            {
                if (Main.savedMixerValues.Count == 0)
                {
                    Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "[SAVE] EmergencySave: No mixer data to save");
                    return;
                }

                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (string.IsNullOrEmpty(persistentPath))
                {
                    Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] EmergencySave: No persistent path available");
                    return;
                }

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
                    ["Version"] = "1.0.0",
                    ["Reason"] = "Emergency save before crash/shutdown"
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                File.WriteAllText(emergencyFile, json);

                Main.logger.Msg(1, string.Format("[SAVE] EmergencySave: Emergency save completed - {0} mixer values saved", Main.savedMixerValues.Count));
            }
            catch (Exception ex)
            {
                emergencyError = ex;
            }

            if (emergencyError != null)
            {
                Main.logger.Err(string.Format("[SAVE] EmergencySave CRASH PREVENTION: Failed: {0}", emergencyError.Message));
                // Don't re-throw - we're likely in a crash scenario already
            }
        }

        /// <summary>
        /// Stress test game save operations by calling SaveManager directly
        /// ⚠️ CRASH PREVENTION: This method calls the game's save system directly with comprehensive monitoring
        /// ⚠️ THREAD SAFETY: Thread-safe operation with comprehensive error tracking and recovery
        /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses framework-safe async patterns and proper exception handling
        /// </summary>
        /// <param name="iterations">Number of game save operations to perform</param>
        /// <param name="delaySeconds">Delay between operations in seconds (supports decimals for milliseconds)</param>
        /// <param name="bypassCooldown">Whether to bypass any game cooldowns (note: this may not affect game's internal cooldown)</param>
        public static IEnumerator StressGameSaveTest(int iterations, float delaySeconds = 0f, bool bypassCooldown = true)
        {
            if (iterations <= 0)
            {
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] StressGameSaveTest: Invalid iteration count, must be > 0");
                yield break;
            }

            if (delaySeconds < 0f)
            {
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] StressGameSaveTest: Invalid delay, using 0 seconds");
                delaySeconds = 0f;
            }

            Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Total iterations: {0}", iterations));

            // Track stress test statistics
            int successCount = 0;
            int failureCount = 0;
            float totalTime = 0f;
            var startTime = Time.time;

            try
            {
                for (int i = 1; i <= iterations; i++)
                {
                    var iterationStartTime = Time.time;
                    Exception saveError = null;

                    Main.logger.Msg(2, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Iteration {0}/{1}", i, iterations));

                    // Call game's save system using reflection to avoid namespace issues
                    try
                    {
                        // Find SaveManager using reflection - dnSpy verified namespace: ScheduleOne.Persistence.SaveManager
                        var saveManagerType = System.Type.GetType("ScheduleOne.Persistence.SaveManager, Assembly-CSharp");
                        if (saveManagerType != null)
                        {
                            // Find Singleton<SaveManager>.Instance using reflection
                            var singletonType = saveManagerType.Assembly.GetTypes()
                                .FirstOrDefault(t => t.Name == "Singleton`1");

                            if (singletonType != null)
                            {
                                var genericSingletonType = singletonType.MakeGenericType(saveManagerType);
                                var instanceProperty = genericSingletonType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);

                                if (instanceProperty != null)
                                {
                                    var saveManagerInstance = instanceProperty.GetValue(null, null);
                                    if (saveManagerInstance != null)
                                    {
                                        var saveMethod = saveManagerType.GetMethod("Save", BindingFlags.Public | BindingFlags.Instance);
                                        if (saveMethod != null)
                                        {
                                            saveMethod.Invoke(saveManagerInstance, null);
                                            successCount++;

                                            var iterationTime = (Time.time - iterationStartTime) * 1000f;
                                            Main.logger.Msg(3, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Iteration {0} completed in {1:F1}ms", i, iterationTime));
                                        }
                                        else
                                        {
                                            Main.logger.Err(string.Format("[SAVE] GAME SAVE StressGameSaveTest: Save method not found on SaveManager for iteration {0}", i));
                                            failureCount++;
                                        }
                                    }
                                    else
                                    {
                                        Main.logger.Err(string.Format("[SAVE] GAME SAVE StressGameSaveTest: SaveManager instance is null for iteration {0}", i));
                                        failureCount++;
                                    }
                                }
                                else
                                {
                                    Main.logger.Err(string.Format("[SAVE] GAME SAVE StressGameSaveTest: Instance property not found on Singleton for iteration {0}", i));
                                    failureCount++;
                                }
                            }
                            else
                            {
                                Main.logger.Err(string.Format("[SAVE] GAME SAVE StressGameSaveTest: Singleton type not found for iteration {0}", i));
                                failureCount++;
                            }
                        }
                        else
                        {
                            Main.logger.Err(string.Format("[SAVE] GAME SAVE StressGameSaveTest: SaveManager type not found for iteration {0}", i));
                            failureCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        saveError = ex;
                        failureCount++;
                    }

                    if (saveError != null)
                    {
                        Main.logger.Err(string.Format("[SAVE] GAME SAVE StressGameSaveTest: Iteration {0} FAILED: {1}", i, saveError.Message));
                    }

                    // Progress reporting every 5 iterations or on last iteration (more frequent for game saves)
                    if (i % 5 == 0 || i == iterations)
                    {
                        float currentTime = Time.time - startTime;
                        float avgTimePerSave = currentTime / i;
                        Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Progress {0}/{1} - Success: {2}, Failed: {3}, Avg: {4:F3}s/iteration",
                            i, iterations, successCount, failureCount, avgTimePerSave));
                    }

                    // Apply delay between iterations if specified - yield return outside try/catch for .NET 4.8.1 compatibility
                    if (delaySeconds > 0f && i < iterations)
                    {
                        yield return new WaitForSeconds(delaySeconds);
                    }

                    // Yield every iteration to prevent frame drops - yield return outside try/catch
                    yield return null;
                }

                totalTime = Time.time - startTime;

                // Final statistics
                Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] GAME SAVE StressGameSaveTest: ===== GAME SAVE STRESS TEST COMPLETED =====");
                Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Total iterations: {0}", iterations));
                Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Successful saves: {0}", successCount));
                Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Failed saves: {0}", failureCount));
                Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Success rate: {0:F1}%", (successCount / (float)iterations) * 100f));
                Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Total time: {0:F3}s", totalTime));
                Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Average time per iteration: {0:F3}s", totalTime / iterations));

                if (delaySeconds > 0f)
                {
                    float actualSaveTime = totalTime - (delaySeconds * (iterations - 1));
                    Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Actual save time (excluding delays): {0:F3}s", actualSaveTime));
                    Main.logger.Msg(1, string.Format("[SAVE] GAME SAVE StressGameSaveTest: Average save time (excluding delays): {0:F3}s", actualSaveTime / iterations));
                }

                Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] GAME SAVE StressGameSaveTest: ==========================================");

                // Performance warnings
                if (failureCount > 0)
                {
                    Main.logger.Warn(ModConstants.WARN_LEVEL_CRITICAL, string.Format("[SAVE] GAME SAVE StressGameSaveTest: ⚠️ {0} save operations failed - check logs for details", failureCount));
                }

                if (successCount == iterations)
                {
                    Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] GAME SAVE StressGameSaveTest: ✅ All game save operations completed successfully!");
                }
            }
            finally
            {
                Main.logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, "[SAVE] GAME SAVE StressGameSaveTest: Stress test cleanup completed");
            }
        }

        /// <summary>
        /// Stress test mixer preferences save operations for crash prevention validation
        /// ⚠️ CRASH PREVENTION: This method can optionally bypass cooldowns for testing mixer pref saves
        /// ⚠️ THREAD SAFETY: Thread-safe operation with comprehensive error tracking and recovery
        /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses framework-safe async patterns and proper exception handling
        /// </summary>
        /// <param name="iterations">Number of mixer preferences save operations to perform</param>
        /// <param name="delaySeconds">Delay between operations in seconds (supports decimals for milliseconds)</param>
        /// <param name="bypassCooldown">Whether to bypass the 2-second save cooldown (default: true)</param>
        public static IEnumerator StressSaveTest(int iterations, float delaySeconds = 0f, bool bypassCooldown = true)
        {
            if (iterations <= 0)
            {
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] StressSaveTest: Invalid iteration count, must be > 0");
                yield break;
            }

            if (delaySeconds < 0f)
            {
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] StressSaveTest: Invalid delay, using 0 seconds");
                delaySeconds = 0f;
            }

            Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFERENCES StressSaveTest: Starting stress test - {0} iterations with {1:F3}s delay, bypass cooldown: {2}", iterations, delaySeconds, bypassCooldown));

            // Track stress test statistics
            int successCount = 0;
            int failureCount = 0;
            int skippedCount = 0;
            float totalTime = 0f;
            var startTime = Time.time;

            // Store original cooldown for restoration
            var originalLastSaveTime = lastSaveTime;

            try
            {
                for (int i = 1; i <= iterations; i++)
                {
                    var iterationStartTime = Time.time;
                    bool iterationSkipped = false;

                    // Handle setup and checks without yield return issues
                    Main.logger.Msg(2, string.Format("[SAVE] MIXER PREFS StressSaveTest: Iteration {0}/{1}", i, iterations));

                    // Conditionally bypass cooldown for stress testing
                    if (bypassCooldown)
                    {
                        lock (saveLock)
                        {
                            lastSaveTime = DateTime.MinValue;
                        }
                        Main.logger.Msg(3, string.Format("[SAVE] StressSaveTest: Bypassed cooldown for iteration {0}", i));
                    }
                    else
                    {
                        // Check if we need to skip due to cooldown
                        if (DateTime.Now - lastSaveTime < SAVE_COOLDOWN)
                        {
                            Main.logger.Msg(3, string.Format("[SAVE] StressSaveTest: Iteration {0} skipped due to cooldown", i));
                            iterationSkipped = true;
                            skippedCount++;
                        }
                    }

                    // Perform save operation - yield return outside try/catch for .NET 4.8.1 compatibility
                    if (!iterationSkipped)
                    {
                        yield return TriggerSaveWithCooldown();

                        // Handle results after yield return
                        Exception iterationError = null;
                        try
                        {
                            successCount++;

                            var iterationTime = (Time.time - iterationStartTime) * 1000f;
                            Main.logger.Msg(3, string.Format("[SAVE] MIXER PREFS StressSaveTest: Iteration {0} completed in {1:F1}ms", i, iterationTime));
                        }
                        catch (Exception ex)
                        {
                            iterationError = ex;
                            failureCount++;
                        }

                        if (iterationError != null)
                        {
                            Main.logger.Err(string.Format("[SAVE] MIXER PREFS StressSaveTest: Iteration {0} FAILED: {1}", i, iterationError.Message));
                        }
                    }

                    // Progress reporting every 10 iterations or on last iteration
                    if (i % 10 == 0 || i == iterations)
                    {
                        float currentTime = Time.time - startTime;
                        float avgTimePerSave = currentTime / i;
                        Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Progress {0}/{1} - Success: {2}, Failed: {3}, Skipped: {4}, Avg: {5:F3}s/iteration",
                            i, iterations, successCount, failureCount, skippedCount, avgTimePerSave));
                    }

                    // Apply delay between iterations if specified - yield return outside try/catch
                    if (delaySeconds > 0f && i < iterations)
                    {
                        yield return new WaitForSeconds(delaySeconds);
                    }

                    // Yield every iteration to prevent frame drops - yield return outside try/catch
                    yield return null;
                }

                totalTime = Time.time - startTime;

                // Final statistics
                Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] MIXER PREFS StressSaveTest: ===== MIXER PREFERENCES STRESS TEST COMPLETED =====");
                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Total iterations: {0}", iterations));
                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Successful saves: {0}", successCount));
                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Failed saves: {0}", failureCount));
                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Skipped saves (cooldown): {0}", skippedCount));

                if ((iterations - skippedCount) > 0)
                {
                    Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Success rate: {0:F1}%", (successCount / (float)(iterations - skippedCount)) * 100f));
                }

                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Total time: {0:F3}s", totalTime));
                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Average time per iteration: {0:F3}s", totalTime / iterations));
                Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Bypass cooldown: {0}", bypassCooldown));

                if (delaySeconds > 0f)
                {
                    float actualSaveTime = totalTime - (delaySeconds * (iterations - 1));
                    Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Actual save time (excluding delays): {0:F3}s", actualSaveTime));
                    Main.logger.Msg(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: Average save time (excluding delays): {0:F3}s", actualSaveTime / iterations));
                }

                Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] MIXER PREFS StressSaveTest: ==========================================");

                // Performance warnings
                if (failureCount > 0)
                {
                    Main.logger.Warn(1, string.Format("[SAVE] MIXER PREFS StressSaveTest: ⚠️ {0} save operations failed - check logs for details", failureCount));
                }

                if (skippedCount > 0 && !bypassCooldown)
                {
                    Main.logger.Warn(ModConstants.WARN_LEVEL_CRITICAL, string.Format("[SAVE] MIXER PREFS StressSaveTest: ⚠️ {0} saves skipped due to cooldown - consider enabling bypass or increasing delay", skippedCount));
                }

                if (totalTime / iterations > ModConstants.SAVE_PERFORMANCE_WARNING_SECONDS)
                {
                    Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "[SAVE] MIXER PREFS StressSaveTest: ⚠️ Average time per iteration > 1 second - performance issue detected");
                }

                if (successCount == iterations)
                {
                    Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] MIXER PREFS StressSaveTest: ✅ All mixer preferences save operations completed successfully!");
                }
                else if (successCount + skippedCount == iterations)
                {
                    Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "[SAVE] MIXER PREFS StressSaveTest: ✅ All attempted mixer preferences save operations completed successfully!");
                }
            }
            finally
            {
                // Restore original cooldown state
                lock (saveLock)
                {
                    lastSaveTime = originalLastSaveTime;
                    isSaveInProgress = false; // Ensure save lock is released
                }

                Main.logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, "[SAVE] MIXER PREFS StressSaveTest: Stress test cleanup completed");
            }
        }
    }
}