using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using ScheduleOne.Management;
using ScheduleOne.Noise;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static MelonLoader.MelonLaunchOptions;
using static MelonLoader.MelonLogger;
using static ScheduleOne.Console;
using static VLB.Consts;

// Reminder: Add to steam game startup command: "--melonloader.captureplayerlogs" for extra MelonLogger verbosity :)

[assembly: MelonInfo(typeof(MixerThreholdMod_0_0_1.Main), "MixerThreholdMod", "0.0.1", "mooleshacat")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_0_0_1
{
    public class Main : MelonMod
    {
        public static MelonPreferences_Entry<bool> debugLoggingEnabledEntry;
        public static MelonPreferences_Entry<bool> showLogLevelCalcEntry;
        public static MelonPreferences_Entry<int> currentMsgLogLevelEntry;
        public static MelonPreferences_Entry<int> currentWarnLogLevelEntry;
        public static readonly Logger logger = new Logger();
        private static List<MixingStationConfiguration> queuedInstances = new List<MixingStationConfiguration>();
        public static Dictionary<MixingStationConfiguration, float> userSetValues = new Dictionary<MixingStationConfiguration, float>();
        
        public static Dictionary<int, float> savedMixerValues = new Dictionary<int, float>();
        public static string CurrentSavePath = null;
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            // Test log
            Logger logger = new Logger();
            logger.Msg(1, "MixerThreholdMod initializing...");
            logger.Msg(1, $"currentMsgLogLevel: {logger.CurrentMsgLogLevel}");
            logger.Msg(1, $"currentWarnLogLevel: {logger.CurrentWarnLogLevel}");
            // Patch constructor to queue instance
            var constructor = typeof(MixingStationConfiguration).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] {
                    typeof(ConfigurationReplicator),
                    typeof(IConfigurable),
                    typeof(MixingStation)
                },
                null
            );
            if (constructor == null)
            {
                logger.Err("Target constructor NOT found!");
                return;
            }
            HarmonyInstance.Patch(
                constructor,
                prefix: new HarmonyMethod(typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic))
            );
            logger.Msg(2, "Patched constructor");
            Console.RegisterConsoleCommandViaReflection();
        }
        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            try
            {
                if (__instance == null)
                {
                    logger.Warn(1, "QueueInstance: Attempting to queue null MixingStationConfiguration");
                    return;
                }

                if (queuedInstances == null)
                {
                    logger.Warn(1, "QueueInstance: queuedInstances is null, initializing");
                    queuedInstances = new List<MixingStationConfiguration>();
                }

                queuedInstances.Add(__instance);
                logger.Msg(3, "Queued new MixingStationConfiguration");
            }
            catch (Exception ex)
            {
                logger.Err($"QueueInstance: Error queuing instance: {ex.Message}\n{ex.StackTrace}");
            }
        }
        public async override void OnUpdate()
        {
            try
            {
                // 🔁 Clean up tracked mixers at the start of each update
                try
                {
                    await TrackedMixers.RemoveAllAsync(tm => tm?.ConfigInstance == null);
                }
                catch (Exception cleanupEx)
                {
                    logger.Err($"OnUpdate: Error during mixer cleanup: {cleanupEx.Message}");
                }

                if (queuedInstances == null || queuedInstances.Count == 0) return;

                var toProcess = queuedInstances?.ToList() ?? new List<MixingStationConfiguration>();
                foreach (var instance in toProcess)
                {
                    try
                    {
                        if (instance == null)
                        {
                            logger.Warn(1, "OnUpdate: Skipping null instance in queuedInstances");
                            continue;
                        }

                        // Prevent duplicate processing of the same instance
                        bool alreadyTracked = await TrackedMixers.AnyAsync(tm => tm?.ConfigInstance == instance);
                        if (alreadyTracked)
                        {
                            logger.Warn(1, $"Instance already tracked — skipping duplicate: {instance}");
                            continue;
                        }

                        if (instance.StartThrehold == null)
                        {
                            logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                            continue;
                        }

                        var existingMixerData = await TrackedMixers.FirstOrDefaultAsync(tm => tm?.ConfigInstance == instance);
                        if (existingMixerData == null)
                        {
                            try
                            {
                                // Configure threshold
                                instance.StartThrehold.Configure(1f, 20f, true);

                                var newTrackedMixer = new TrackedMixer
                                {
                                    ConfigInstance = instance,
                                    MixerInstanceID = MixerIDManager.GetMixerID(instance)
                                };
                                
                                await TrackedMixers.AddAsync(newTrackedMixer);
                                logger.Msg(3, $"Created mixer with Stable ID: {newTrackedMixer.MixerInstanceID}");

                                // Now safely add listener
                                if (!newTrackedMixer.ListenerAdded)
                                {
                                    logger.Msg(3, $"Attaching listener for Mixer {newTrackedMixer.MixerInstanceID}");
                                    try
                                    {
                                        Utils.CoroutineHelper.RunCoroutine(MixerSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                                        newTrackedMixer.ListenerAdded = true;
                                    }
                                    catch (Exception listenerEx)
                                    {
                                        logger.Err($"OnUpdate: Failed to start listener attachment coroutine: {listenerEx.Message}");
                                    }
                                }
                                
                                // Restore saved value if exists
                                if (savedMixerValues != null && savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out float savedValue))
                                {
                                    try
                                    {
                                        logger.Msg(2, $"Restoring Mixer {newTrackedMixer.MixerInstanceID} to {savedValue}");
                                        instance.StartThrehold.SetValue(savedValue, true);
                                    }
                                    catch (Exception restoreEx)
                                    {
                                        logger.Err($"OnUpdate: Failed to restore saved value: {restoreEx.Message}");
                                    }
                                }
                            }
                            catch (Exception configEx)
                            {
                                Main.logger.Err("Error configuring mixer: " + configEx.Message + "\n" + configEx.StackTrace);
                            }
                        }
                    }
                    catch (Exception instanceEx)
                    {
                        logger.Err($"OnUpdate: Error processing mixer instance: {instanceEx.Message}\n{instanceEx.StackTrace}");
                    }
                }
                
                queuedInstances?.Clear();
            }
            catch (Exception ex)
            {
                logger.Err($"OnUpdate: Critical error in main update loop: {ex.Message}\n{ex.StackTrace}");
            }
        }



        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            return await TrackedMixers.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            try
            {
                base.OnSceneWasLoaded(buildIndex, sceneName);
                logger.Msg(2, "Scene loaded: " + sceneName);
                
                if (sceneName == "Main")
                {
                    try
                    {
                        // Reset mixer IDs
                        if (MixerIDManager.MixerInstanceMap != null)
                        {
                            MixerIDManager.MixerInstanceMap.Clear();
                        }
                        MixerIDManager.ResetStableIDCounter();

                        // Clear previous mixer values
                        if (savedMixerValues != null)
                        {
                            savedMixerValues.Clear();
                        }
                        
                        logger.Msg(3, "Current Save Path at scene load: " + (Main.CurrentSavePath ?? "[not available yet]"));

                        // Start coroutine to wait for save path
                        try
                        {
                            StartLoadCoroutine();
                        }
                        catch (Exception coroutineEx)
                        {
                            logger.Err($"OnSceneWasLoaded: Failed to start load coroutine: {coroutineEx.Message}");
                        }
                        
                        // Force-refresh file copy to save folder
                        if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                        {
                            try
                            {
                                string persistentPath = MelonEnvironment.UserDataDirectory;
                                if (!string.IsNullOrEmpty(persistentPath))
                                {
                                    string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json").Replace('/', '\\');
                                    string targetFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json").Replace('/', '\\');
                                    
                                    if (File.Exists(sourceFile))
                                    {
                                        FileOperations.SafeCopy(sourceFile, targetFile, overwrite: true);
                                        logger.Msg(3, "Copied MixerThresholdSave.json from persistent to save folder");
                                    }
                                }
                            }
                            catch (Exception copyEx)
                            {
                                logger.Err($"OnSceneWasLoaded: Failed to copy mixer save file: {copyEx.Message}");
                            }
                        }
                    }
                    catch (Exception mainEx)
                    {
                        Main.logger.Err($"OnSceneWasLoaded: Error in Main scene processing: {mainEx.Message}\n{mainEx.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err($"OnSceneWasLoaded: Critical error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static bool _coroutineStarted = false;
        private static void StartLoadCoroutine()
        {
            try
            {
                if (_coroutineStarted) 
                {
                    logger.Msg(3, "StartLoadCoroutine: Already started, skipping");
                    return;
                }
                
                _coroutineStarted = true;
                logger.Msg(3, "StartLoadCoroutine: Starting MixerSaveManager.LoadMixerValuesWhenReady");
                MelonCoroutines.Start(MixerSaveManager.LoadMixerValuesWhenReady());
            }
            catch (Exception ex)
            {
                logger.Err($"StartLoadCoroutine: Error starting coroutine: {ex.Message}\n{ex.StackTrace}");
                _coroutineStarted = false; // Reset flag on error
            }
        }




        // Commented - testing removal - 0 references to this code
        //[Serializable]
        //public class MixerThresholdSaveData
        //{
        //    public Dictionary<int, float> MixerValues = new Dictionary<int, float>();
        //}

    }
} // End of namespace