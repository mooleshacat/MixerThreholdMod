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
        private static bool isBackupInProgress = false;
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
                    Main.logger.Msg(1, 