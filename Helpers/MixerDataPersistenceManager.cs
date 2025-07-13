using MelonLoader;
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
using MelonLoader.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD:Helpers/MixerSaveManager.cs
using System.Text;
using System.Text.RegularExpressions;
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static HarmonyLib.Code;
using static MelonLoader.Modules.MelonModule;
using static MixerThreholdMod_0_0_1.Main;
using static ScheduleOne.Console;

<<<<<<< HEAD:Helpers/MixerSaveManager.cs
namespace MixerThreholdMod_0_0_1
=======
namespace MixerThreholdMod_1_0_0.Helpers
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using MixerThreholdMod_1_0_0.Constants;    // ✅ ESSENTIAL - Keep this! Our constants!

namespace MixerThreholdMod_1_0_0.Utils
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
        private static CancellationTokenSource saveCts = new CancellationTokenSource();
        public static ConcurrentDictionary<int, float> SavedMixerValues = new ConcurrentDictionary<int, float>();
        public static IEnumerator AttachListenerWhenReady(MixingStationConfiguration instance, int uniqueID)
        {
<<<<<<< HEAD
            Main.logger.Msg(3, $"AttachListenerWhenReady started for Mixer {uniqueID}");
            
            // Safety check: bail if instance is null
            if (instance == null)
=======
            try
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            {
                // Safety check: bail if instance is null
                if (instance == null)
                {
                    Main.logger.Warn(1, $"Instance is null — cannot attach listener for Mixer {uniqueID}");
                    yield break;
                }

<<<<<<< HEAD
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

<<<<<<< HEAD:Helpers/MixerSaveManager.cs
            if (instance.StartThrehold == null)
            {
                Main.logger.Warn(1, $"StartThrehold never became available after {maxWaitAttempts} attempts — Mixer {uniqueID}");
                yield break;
            }

            // Now handle the operations that might throw exceptions
            ProcessMixerSetupSafely(instance, uniqueID);
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
                        Main.logger.Err($"Failed to set saved value for Mixer {uniqueID}: {setEx.Message}");
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
                                Main.logger.Err($"Error in mixer value change listener for {uniqueID}: {saveEx.Message}");
                            }
                        });
                        Main.logger.Msg(3, $"Successfully attached listener for Mixer {uniqueID}");
=======
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
        // Concurrency protection fields
        private static bool isBackupInProgress = false;
        private static readonly object backupLock = new object();
        private static bool isSaveInProgress = false;
        private static readonly object saveLock = new object();
        private static DateTime lastSaveTime = DateTime.MinValue;
		private static readonly TimeSpan SAVE_COOLDOWN = TimeSpan.FromSeconds(ModConstants.SAVE_COOLDOWN_SECONDS);
		private static readonly TimeSpan BACKUP_INTERVAL = TimeSpan.FromMinutes(ModConstants.BACKUP_INTERVAL_MINUTES);

		public static IEnumerator LoadMixerValuesWhenReady()
        {
            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "LoadMixerValuesWhenReady: Started");

            // Wait until we have a valid save path - NO try-catch around yield
            while (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                yield return new WaitForSeconds(0.5f);
            }

            Main.logger.Msg(3, string.Format("LoadMixerValuesWhenReady: Save path available: {0}", Main.CurrentSavePath));

<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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

<<<<<<< HEAD
<<<<<<< HEAD
            Main.logger.Msg(3, "LoadMixerValuesWhenReady: Finished");
=======
            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "LoadMixerValuesWhenReady: Finished");
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "LoadMixerValuesWhenReady: Finished");
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
<<<<<<<< HEAD:Legacy/MixerDataPersistenceManager.cs
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
<<<<<<<< HEAD:Legacy/MixerDataPersistenceManager.cs
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

        private static async Task LoadMixerValuesFromFileAsync()
        {
            try
            {
                string saveFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json");

                if (File.Exists(saveFile))
                {
                    Main.logger.Msg(2, "LoadMixerValuesFromFileAsync: Loading saved mixer values");

                    string json = await ThreadSafeFileOperations.SafeReadAllTextAsync(saveFile);
========
>>>>>>>> aa94715 (performance optimizations, cache manager):Helpers/MixerDataPersistenceManager.cs
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
========
>>>>>>>> 2bf7ffe (performance optimizations, cache manager):Helpers/MixerDataPersistenceManager.cs
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
            while (config?.StartThrehold == null)
=======
            while (!HasValidStartThreshold(config))
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
            while (!HasValidStartThreshold(config))
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
                    }
                    else
                    {
                        Main.logger.Warn(1, $"onItemChanged is null for Mixer {uniqueID}, cannot attach listener");
                    }
                }
                catch (Exception listenerEx)
                {
                    Main.logger.Err($"Failed to attach listener for Mixer {uniqueID}: {listenerEx.Message}");
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD:Helpers/MixerSaveManager.cs
                Main.logger.Err($"ProcessMixerSetupSafely: Critical error for Mixer {uniqueID}: {ex.Message}\n{ex.StackTrace}");
=======
                // Wait for StartThrehold to become available, but only while instance exists
                while (instance != null && instance.StartThrehold == null)
                {
                    yield return null;
                }

                // Double-check instance still exists
                if (instance == null)
                {
                    Main.logger.Warn(1, $"Instance destroyed before threshold became available — Mixer {uniqueID}");
                    yield break;
                }
=======
                Main.logger.Err(string.Format("OnValueChangedGeneric error for Mixer {0}: {1}", mixerID, ex));
            }
        }
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs

                // Additional null check for StartThrehold after the wait
                if (instance.StartThrehold == null)
                {
                    Main.logger.Warn(1, $"StartThrehold is still null after wait — Mixer {uniqueID}");
                    yield break;
                }

<<<<<<< HEAD:Helpers/MixerSaveManager.cs
                // Restore saved value if exists
                try
                {
                    if (Main.savedMixerValues.TryGetValue(uniqueID, out float savedValue))
                    {
                        instance.StartThrehold.SetValue(savedValue, true);
                        Main.logger.Msg(3, $"Restored Mixer {uniqueID} to saved value: {savedValue}");
                    }
=======
            float lastKnownValue = -1f;
            bool hasInitialValue = false;

            while (config?.StartThrehold != null)
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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

        private static IEnumerator PollValueChanges(object config, int mixerID)
        {
            Main.logger.Msg(3, string.Format("PollValueChanges: Started polling for Mixer {0}", mixerID));

            float lastKnownValue = -1f;
            bool hasInitialValue = false;

            // IL2CPP COMPATIBLE: Use helper method instead of direct property access
            while (HasValidStartThreshold(config))
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
            {
                Exception pollError = null;
                float? currentValue = null;

                try
                {
                    currentValue = GetCurrentValue(config.StartThrehold);
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"Error restoring saved value for Mixer {uniqueID}: {ex}");
                }

<<<<<<< HEAD:Helpers/MixerSaveManager.cs
                // Attach listener with null safety
                try
                {
                    instance.StartThrehold.onItemChanged.AddListener((float val) =>
                    {
                        try
                        {
                            MixerSaveManager.SaveMixerValue(uniqueID, val);
                            Main.logger.Msg(3, $"Mixer {uniqueID} changed to {val}");
                        }
                        catch (Exception ex)
                        {
                            Main.logger.Err($"Error in mixer value change listener for Mixer {uniqueID}: {ex}");
                        }
                    });
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"Error attaching listener for Mixer {uniqueID}: {ex}");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Coroutine continue failure in AttachListenerWhenReady for Mixer {uniqueID}: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
=======
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                }
                catch (Exception ex)
                {
                    pollError = ex;
                }

<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Msg(3, "SaveMixerValues: Skipping save due to cooldown period");
=======
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Skipping save due to cooldown period");
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Skipping save due to cooldown period");
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Warn(1, "SaveMixerValues: Already in progress, skipping duplicate call");
                yield break;
            }

            Main.logger.Msg(3, "SaveMixerValues: Started");
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "SaveMixerValues: Already in progress, skipping duplicate call");
                yield break;
            }

            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Started");
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)

            // Pre-validate before any yield returns
            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Warn(1, "SaveMixerValues: CurrentSavePath is null/empty, cannot save");
=======
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "SaveMixerValues: CurrentSavePath is null/empty, cannot save");
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "SaveMixerValues: CurrentSavePath is null/empty, cannot save");
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                lock (saveLock)
                {
                    isSaveInProgress = false;
                }
                yield break;
            }

            if (Main.savedMixerValues.Count == 0)
            {
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Msg(3, "SaveMixerValues: No mixer values to save");
=======
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: No mixer values to save");
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: No mixer values to save");
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Msg(3, "SaveMixerValues: Starting backup coroutine");
                // NO try-catch around yield return for .NET 4.8.1
                yield return MelonCoroutines.Start(BackupSaveFolder());
                Main.logger.Msg(3, "SaveMixerValues: Backup coroutine completed");
            }
            else
            {
                Main.logger.Msg(3, "SaveMixerValues: Skipping backup (recent backup exists)");
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Starting backup coroutine");
                // NO try-catch around yield return for .NET 4.8.1
                yield return MelonCoroutines.Start(BackupSaveFolder());
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Backup coroutine completed");
            }
            else
            {
                Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Skipping backup (recent backup exists)");
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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

<<<<<<< HEAD
<<<<<<< HEAD
            Main.logger.Msg(3, "SaveMixerValues: Finished and cleanup completed");
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "SaveMixerValues: Finished and cleanup completed");
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

<<<<<<<< HEAD:Legacy/MixerDataPersistenceManager.cs
<<<<<<< HEAD
=======
<<<<<<<< HEAD:Legacy/MixerDataPersistenceManager.cs
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                await Helpers.ThreadSafeFileOperations.SafeWriteAllTextAsync(saveFile, json);
========
                await ThreadSafeFileOperations.SafeWriteAllTextAsync(saveFile, json);
>>>>>>>> aa94715 (performance optimizations, cache manager):Helpers/MixerDataPersistenceManager.cs
<<<<<<< HEAD
=======
========
                await ThreadSafeFileOperations.SafeWriteAllTextAsync(saveFile, json);
>>>>>>>> 2bf7ffe (performance optimizations, cache manager):Helpers/MixerDataPersistenceManager.cs
>>>>>>> 2bf7ffe (performance optimizations, cache manager)

                Main.logger.Msg(2, string.Format("SaveMixerValuesToFileAsync: Saved {0} mixer values to {1}", Main.savedMixerValues.Count, saveFile));

                // Also save to persistent location
                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (!string.IsNullOrEmpty(persistentPath))
                {
                    string persistentFile = Path.Combine(persistentPath, "MixerThresholdSave.json");
<<<<<<<< HEAD:Legacy/MixerDataPersistenceManager.cs
<<<<<<< HEAD
=======
<<<<<<<< HEAD:Legacy/MixerDataPersistenceManager.cs
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                    await Helpers.ThreadSafeFileOperations.SafeWriteAllTextAsync(persistentFile, json);
                    Main.logger.Msg(3, "SaveMixerValuesToFileAsync: Copied to persistent location");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("SaveMixerValuesToFileAsync error: {0}", ex));
                throw;
            }
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
========
>>>>>>>> aa94715 (performance optimizations, cache manager):Helpers/MixerDataPersistenceManager.cs
<<<<<<< HEAD
=======
========
>>>>>>>> 2bf7ffe (performance optimizations, cache manager):Helpers/MixerDataPersistenceManager.cs
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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

<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Warn(1, "BackupSaveFolder: Already in progress, skipping duplicate call");
                yield break;
            }

            Main.logger.Msg(3, "BackupSaveFolder: Started");
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "BackupSaveFolder: Already in progress, skipping duplicate call");
                yield break;
            }

            Main.logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, "BackupSaveFolder: Started");
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)

            // Pre-validate before any yield returns
            if (string.IsNullOrEmpty(Main.CurrentSavePath))
            {
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger.Warn(1, "BackupSaveFolder: CurrentSavePath is null/empty, cannot backup");
=======
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "BackupSaveFolder: CurrentSavePath is null/empty, cannot backup");
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
                Main.logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, "BackupSaveFolder: CurrentSavePath is null/empty, cannot backup");
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
            }
        }
        public static IEnumerator WriteMixerValuesAsync(string saveFolderPath)
        {
<<<<<<< HEAD:Helpers/MixerSaveManager.cs
            if (string.IsNullOrEmpty(saveFolderPath))
            {
                Main.logger.Warn(1, "WriteMixerValuesAsync: saveFolderPath is null or empty");
                yield break;
            }

            try
            {
                yield return Task.Run(() => SaveMixerThresholds(saveFolderPath));
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Coroutine continue failure in WriteMixerValuesAsync: {ex}");
            }
        }
        private static void SaveMixerValue(int id, float value)
        {
            try
            {
                Main.logger.Msg(3, $"SaveMixerValue called for Mixer {id} with value {value}");
                
                // Cancel any previous save operation
                if (saveCts != null)
=======
            try
            {
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
            }
        }

        // Emergency save method for crash scenarios - NO coroutines here
        public static void EmergencySave()
        {
            try
            {
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
                if (Main.savedMixerValues.Count == 0) return;

                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (string.IsNullOrEmpty(persistentPath)) return;

                string emergencyFile = Path.Combine(persistentPath, "MixerThresholdSave_Emergency.json");

                var mixerValuesDict = new Dictionary<int, float>();
                foreach (var kvp in Main.savedMixerValues)
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
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

                bool success = await CancellableIoRunner.Run(
                    token => FileOperations.SafeWriteAllTextWithCancellationAsync(path, json, token, Main.logger.Warn),
                    ct,
                    Main.logger.Warn
                );

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
<<<<<<< HEAD:Helpers/MixerSaveManager.cs
                Main.logger.Err($"SaveMixerValueAsync: Error saving mixer value for {id}: {ex.Message}\n{ex.StackTrace}");
            }
        }
        public static async Task SafeWriteAllTextWithCancellationAsync(
            string path,
            string output,
            CancellationToken ct,
            Action<int, string> logger = null)
        {
            string normalizedPath = path.Replace('/', '\\');
            for (int attempt = 1; attempt <= FileOperations.MaxRetries; attempt++)
            {
                if (ct.IsCancellationRequested)
                {
                    logger?.Invoke(1, $"Operation canceled during SafeWriteAllTextWithCancellation for [{normalizedPath}]");
                    ct.ThrowIfCancellationRequested();
                }

                try
                {
                    using (var locker = new FileLockHelper(FileOperations.LockFilePath))
                    {
                        if (!locker.AcquireExclusiveLock())
                        {
                            if (attempt < FileOperations.MaxRetries)
                            {
                                logger?.Invoke(1, $"Could not acquire lock for [{normalizedPath}], retrying...");
                                await Task.Delay(FileOperations.RetryDelayMs * attempt, ct);
                                continue;
                            }
                            else
                            {
                                logger?.Invoke(1, $"Failed to acquire lock after {FileOperations.MaxRetries} attempts for [{normalizedPath}]");
                                return;
                            }
                        }

                        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                        {
                            await writer.WriteAsync(output).ConfigureAwait(false);
                        }

                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger?.Invoke(1, $"Error writing to file [{path}]: {ex}");
                    if (attempt < FileOperations.MaxRetries)
                    {
                        await Task.Delay(FileOperations.RetryDelayMs * attempt, ct).ConfigureAwait(false);
                    }
                }
            }
        }
        public static void SaveMixerThresholds(string saveFolderPath)
        {
            Utils.CoroutineHelper.RunCoroutine(SaveMixerThresholdsAsync(saveFolderPath));
        }
        public static bool _hasLoggedZeroMixers = false;
        private static IEnumerator SaveMixerThresholdsAsync(string saveFolderPath)
        {
<<<<<<< HEAD
            yield return Task.Run(async () =>
            {
                string filePath = null;
                try
                {
                    Main.logger.Msg(3, $"SaveMixerThresholdsAsync: Starting save operation for folder: {saveFolderPath}");
                    
                    if (string.IsNullOrEmpty(saveFolderPath))
                    {
                        Main.logger.Warn(1, "SaveMixerThresholdsAsync: Save folder path is null or empty");
                        return;
                    }

                    filePath = Path.Combine(saveFolderPath, "MixerThresholdSave.json").Replace('/', '\\');
                    Main.logger.Msg(3, $"SaveMixerThresholdsAsync: Target file path: {filePath}");

                    var _mixerCount = await TrackedMixers.CountAsync(tm => tm != null);
                    var mixerIds = await TrackedMixers.SelectAsync(tm => tm?.MixerInstanceID ?? -1);
                    var validMixerIds = mixerIds?.Where(id => id != -1)?.ToList() ?? new List<int>();
                    
                    Main.logger.Msg(3, $"Currently tracking {_mixerCount} mixers: {string.Join(", ", validMixerIds)}");
                    
                    if (_mixerCount == 0)
=======
            if (string.IsNullOrEmpty(saveFolderPath))
            {
                Main.logger.Warn(1, "SaveMixerThresholdsAsync: saveFolderPath is null or empty");
                yield break;
            }

            try
            {
                yield return Task.Run(async () =>
                {
                    string filePath = Path.Combine(saveFolderPath, "MixerThresholdSave.json").Replace('/', '\\');
                    try
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                    {
                        var _mixerCount = await TrackedMixers.CountAsync(tm => true);
                        Main.logger.Msg(3, $"Currently tracking {_mixerCount} mixers: {string.Join(", ", await TrackedMixers.SelectAsync(tm => tm.MixerInstanceID))}");
                        if (_mixerCount == 0)
                        {
<<<<<<< HEAD
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
                                Main.logger.Err($"Error reading value from mixer {tm.MixerInstanceID}: {valueEx.Message}");
=======
                            if (!_hasLoggedZeroMixers)
                            {
                                Main.logger.Msg(2, "No mixers tracked (maybe you have none?) — skipping mixer threshold prefs save/create.");
                                Main.logger.Warn(1, "No mixers tracked (maybe you have none?) — suppressing future logs until reload save.");
                                _hasLoggedZeroMixers = true;
                            }
                            return;
                        }
                        var mixerDataSnapshot = new Dictionary<int, float>();
                        
                        try
                        {
                            foreach (var tm in await TrackedMixers.ToListAsync())
                            {
                                if (tm == null || tm.ConfigInstance == null || tm.ConfigInstance.StartThrehold == null)
                                {
                                    Main.logger.Warn(1, $"Removing invalid mixer from tracked list: {tm?.MixerInstanceID ?? -1}");
                                    try
                                    {
                                        await TrackedMixers.RemoveAsync(tm?.ConfigInstance);
                                    }
                                    catch (Exception removeEx)
                                    {
                                        Main.logger.Err($"Error removing invalid mixer: {removeEx}");
                                    }
                                    continue;
                                }
                                try
                                {
                                    mixerDataSnapshot[tm.MixerInstanceID] = tm.ConfigInstance.StartThrehold.Value;
                                }
                                catch (Exception ex)
                                {
                                    Main.logger.Err($"Error capturing mixer {tm.MixerInstanceID}: {ex}");
                                }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                            }
                        }
                        catch (Exception tmEx)
                        {
<<<<<<< HEAD
                            Main.logger.Err($"Error processing tracked mixer {tm?.MixerInstanceID ?? -1}: {tmEx.Message}");
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
                            
                            // Using SafeWriteAllText locks the file first
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
=======
                            Main.logger.Err($"Error iterating through tracked mixers: {ex}");
                            return;
                        }

                        try
                        {
                            string json = JsonConvert.SerializeObject(new MixerThresholdSaveData
                            {
                                MixerValues = mixerDataSnapshot
                            }, Formatting.Indented);
                            
                            bool success = false;
                            int attempts = 0;
                            while (!success && attempts < 5)
                            {
                                try
                                {
                                    // Using SafeWriteAllText locks the file first
                                    // Moved into own class, figured I could wrap the whole thing
                                    // Same block of code that was here is now there :)
                                    FileOperations.SafeWriteAllText(filePath, json);
                                    Main.logger.Msg(1, $"Saved {mixerDataSnapshot.Count} mixer thresholds.");
                                    success = true;
                                }
                                catch (IOException ex)
                                {
                                    Main.logger.Warn(1, $"File write failed [{filePath}] (attempt {attempts + 1}), retrying... {ex.Message}");
                                    attempts++;
                                    Thread.Sleep(500);
                                }
                                catch (Exception ex)
                                {
                                    Main.logger.Err($"Unexpected error saving mixer file [{filePath}]: {ex}");
                                    break;
                                }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                            }
                        }
                        catch (Exception writeEx)
                        {
<<<<<<< HEAD
                            Main.logger.Err($"Unexpected error saving mixer file [{filePath}] (attempt {attempts}): {writeEx.Message}\n{writeEx.StackTrace}");
                            break;
                        }
                    }

                    if (!success)
                    {
                        Main.logger.Err($"Failed to save mixer file after {maxAttempts} attempts: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    // catchall at patch level, where my DLL interacts with the game and it's engine
                    // hopefully should catch errors in entire project?
                    Main.logger.Err($"SaveMixerThresholdsAsync: Critical error during saving [{filePath ?? "unknown"}]");
                    Main.logger.Err($"SaveMixerThresholdsAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
                }
                finally
                {
                    Main.logger.Msg(3, "SaveMixerThresholdsAsync: Mixer save operation completed.");
                    // IMPORTANT: Previously crashes occurred after this log line
                    // Added comprehensive error handling above to prevent crashes
                    // ORIGINAL COMMENT: CRASHING HERE AFTER SAVING GAME! WHY? CATCHALL ABOVE DOES NOT TRIGGER AND SHOW ANY EXCEPTION?
                    // ANSWER: this function is called, completed successfully (somewhere) and then crashes after it was called
                    //         within that function. Find where it was. Related log below:
                    // 
                    // CRASH ANALYSIS: The crash was likely happening in the calling method after this completed
                    // We've now wrapped the caller (BackupSaveFolder) with comprehensive error handling
                }
            });
=======
                            Main.logger.Err($"Error serializing or saving mixer data: {ex}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // catchall at patch level, where my DLL interacts with the game and it's engine
                        // hopefully should catch errors in entire project?
                        Main.logger.Err($"SaveMixerThresholdsAsync: Critical error during saving [{filePath}]");
                        Main.logger.Err($"SaveMixerThresholdsAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
                    }
                    finally
                    {
                        Main.logger.Msg(3, "Mixer save operation completed.");
                        // CRASHING HERE AFTER SAVING GAME! WHY? CATCHALL ABOVE DOES NOT TRIGGER AND SHOW ANY EXCEPTION?
                        // ANSWER: this function is called, completed successfully (somewhere) and then crashes after it was called
                        //         within that function. Find where it was. Related log below:
                        //
                        // [17:13:44.473] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Menu
                        // [17:13:56.917] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Main
                        // [17:13:56.918] [MixerThreholdMod] Stable ID counter reset.
                        // [17:13:56.918] [MixerThreholdMod] [Info][3]>=[3] Current Save Path at scene load: C:/Users/shawn/AppData/LocalLow/TVGS/Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:14:22.920] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                        // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                        // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                        // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:14:22.922] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                        // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                        // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.
                        // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] BACKUP ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\
                        // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:14:24.411] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT PREFIX: SaveGame_2
                        // [17:14:24.433] [MixerThreholdMod] [Info][3]>=[2] Saved backup to: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                        // [17:14:24.434] [MixerThreholdMod] [Info][3]>=[2] Filtering to keep latest 5 backups from: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                        // [17:20:02.907] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                        // [17:20:02.908] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:20:02.909] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                        // [17:20:02.910] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                        // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:20:02.911] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                        // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                        // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.[17:13:44.473] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Menu
                        // [17:13:56.917] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Main
                        // [17:13:56.918] [MixerThreholdMod] Stable ID counter reset.
                        // [17:13:56.918] [MixerThreholdMod] [Info][3]>=[3] Current Save Path at scene load: C:/Users/shawn/AppData/LocalLow/TVGS/Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:14:22.920] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                        // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                        // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                        // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:14:22.922] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                        // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                        // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.
                        // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] BACKUP ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\
                        // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:14:24.411] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT PREFIX: SaveGame_2
                        // [17:14:24.433] [MixerThreholdMod] [Info][3]>=[2] Saved backup to: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                        // [17:14:24.434] [MixerThreholdMod] [Info][3]>=[2] Filtering to keep latest 5 backups from: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                        // [17:20:02.907] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                        // [17:20:02.908] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                        // [17:20:02.909] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                        // [17:20:02.910] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                        // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:20:02.911] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                        // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                        // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                        // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.
                        // *CRASH*
                        // RESULT: MIXER SAVE COMPLETES, BUT CALLING METHOD CRASHES - INVESTIGATE CALLER!
                    }
                });
=======
                Main.logger.Err(string.Format("Emergency save failed: {0}", ex.Message));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/MixerDataPersistenceManager.cs
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Coroutine continue failure in SaveMixerThresholdsAsync: {ex}");
            }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
        }
        private static bool _hasLoaded = false;
        private static object _loadLock = new object(); // Optional: for thread safety

        public static IEnumerator LoadMixerValuesWhenReady()
        {
<<<<<<< HEAD
            Main.logger.Msg(3, "LoadMixerValuesWhenReady: Starting");
            
            // Check the flag at the very start
            lock (_loadLock)
            {
                if (_hasLoaded)
                {
                    Main.logger.Msg(3, "LoadMixerValuesWhenReady: Already loaded, skipping");
                    yield break;
                }
=======
            try
            {
                // 👇 Check the flag at the very start
                lock (_loadLock)
                {
                    if (_hasLoaded)
                        yield break;
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)

                    // First one in, mark as loading
                    _hasLoaded = true;
                }

<<<<<<< HEAD
            int attempts = 0;
            const int maxAttempts = 50;
            
            while (string.IsNullOrEmpty(Main.CurrentSavePath) && attempts < maxAttempts)
            {
                attempts++;
                yield return new WaitForSeconds(0.1f);
            }

            // Handle the loading operation
            HandleMixerLoadingSafely();
        }

        private static void HandleMixerLoadingSafely()
        {
            try
            {
=======
                int attempts = 0;
                while (string.IsNullOrEmpty(Main.CurrentSavePath) && attempts < 50)
                {
                    yield return new WaitForSeconds(0.1f);
                    attempts++;
                }

>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                {
                    try
                    {
                        LoadAndApplyMixerThresholds(Main.CurrentSavePath);
<<<<<<< HEAD
                        logger.Msg(3, $"Loaded mixer values after save path became available: {Main.CurrentSavePath}");
                    }
                    catch (Exception loadEx)
                    {
                        logger.Err($"LoadMixerValuesWhenReady: Error loading mixer thresholds: {loadEx.Message}");
=======
                        logger.Msg(3, $"Loaded mixer values after save path became available.");
                    }
                    catch (Exception ex)
                    {
                        logger.Err($"Error loading and applying mixer thresholds: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                    }
                }
                else
                {
<<<<<<< HEAD
                    logger.Warn(1, $"Save path never became available after attempts. Using empty/default mixer values.");
=======
                    logger.Warn(1, "Save path never became available. Using empty/default mixer values.");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                logger.Err($"HandleMixerLoadingSafely: Critical error: {ex.Message}\n{ex.StackTrace}");
=======
                logger.Err($"Coroutine continue failure in LoadMixerValuesWhenReady: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
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
<<<<<<< HEAD
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
=======
                string[] pathsToTry;
                if (!string.IsNullOrEmpty(saveFolderPath))
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
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
                    if (File.Exists(path))
                    {
<<<<<<< HEAD
                        if (string.IsNullOrEmpty(path))
                        {
                            Main.logger.Warn(1, "LoadAndApplyMixerThresholds: Empty path in pathsToTry");
                            continue;
                        }

                        if (File.Exists(path))
                        {
                            Main.logger.Msg(3, $"LoadAndApplyMixerThresholds: Found file at {path}");
                            
                            string json = FileOperations.SafeReadAllText(path);
                            if (string.IsNullOrEmpty(json))
                            {
                                Main.logger.Warn(1, $"LoadAndApplyMixerThresholds: File exists but content is empty: {path}");
=======
                        try
                        {
                            string json = FileOperations.SafeReadAllText(path);
                            if (string.IsNullOrEmpty(json))
                            {
                                Main.logger.Warn(1, $"Empty or null JSON content from {path}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                continue;
                            }

                            MixerThresholdSaveData data = JsonConvert.DeserializeObject<MixerThresholdSaveData>(json);
                            if (data?.MixerValues == null)
                            {
<<<<<<< HEAD
                                Main.logger.Warn(1, $"LoadAndApplyMixerThresholds: Deserialized data is null or MixerValues is null: {path}");
=======
                                Main.logger.Warn(1, $"Invalid or null MixerValues from {path}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                continue;
                            }

                            foreach (var kvp in data.MixerValues)
                            {
                                try
                                {
<<<<<<< HEAD
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
=======
                                    SavedMixerValues[kvp.Key] = kvp.Value;
                                }
                                catch (Exception ex)
                                {
                                    Main.logger.Err($"Error adding mixer value {kvp.Key}={kvp.Value} to SavedMixerValues: {ex}");
                                }
                            }

                            try
                            {
                                foreach (var tm in await TrackedMixers.GetAllAsync())
                                {
                                    if (tm == null) continue;
                                    if (tm.ConfigInstance?.StartThrehold != null &&
                                        SavedMixerValues.TryGetValue(tm.MixerInstanceID, out float savedValue))
                                    {
                                        try
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                        {
                                            tm.ConfigInstance.StartThrehold.SetValue(savedValue, true);
                                            Main.logger.Msg(1, $"Applied saved value {savedValue} to Mixer {tm.MixerInstanceID}");
                                        }
<<<<<<< HEAD
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
                        else
                        {
                            Main.logger.Msg(3, $"LoadAndApplyMixerThresholds: File does not exist: {path}");
                        }
                    }
                    catch (Exception pathEx)
                    {
                        Main.logger.Err($"Error loading MixerThresholdSave.json from {path}: {pathEx.Message}\n{pathEx.StackTrace}");
=======
                                        catch (Exception ex)
                                        {
                                            Main.logger.Err($"Error applying saved value to Mixer {tm.MixerInstanceID}: {ex}");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Main.logger.Err($"Error iterating through tracked mixers for value application: {ex}");
                            }

                            Main.logger.Msg(1, $"Loaded {data.MixerValues.Count} mixer thresholds from: {path}");
                            return;
                        }
                        catch (Exception ex)
                        {
                            Main.logger.Err($"Error loading MixerThresholdSave.json from {path}: {ex}");
                        }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                    }
                }

                // Only warn if we're past initial load (e.g., save path was set and we still couldn't find the file)
                if (!string.IsNullOrEmpty(saveFolderPath))
                {
                    Main.logger.Warn(1, "No MixerThresholdSave.json found in any location. Do you have any mixers?");
                }
<<<<<<< HEAD
                else
                {
                    Main.logger.Msg(3, "LoadAndApplyMixerThresholds: No save folder path provided, using defaults");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err($"LoadAndApplyMixerThresholds: Critical error: {ex.Message}\n{ex.StackTrace}");
=======
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Critical error in LoadAndApplyMixerThresholds: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
        }
        [Serializable]
        public class MixerThresholdSaveData
        {
            public Dictionary<int, float> MixerValues = new Dictionary<int, float>();
=======
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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

                Main.logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, "Emergency save completed successfully");
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Emergency save failed: {0}", ex.Message));
            }
<<<<<<< HEAD
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
        }
    }
}