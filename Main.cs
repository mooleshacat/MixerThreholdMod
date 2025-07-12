using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Helpers;
using MixerThreholdMod_1_0_0.Save;
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

[assembly: MelonInfo(typeof(MixerThreholdMod_1_0_0.Main), "MixerThreholdMod", "1.0.0", "mooleshacat")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_1_0_0
{
    public class Main : MelonMod
    {
        public static MelonPreferences_Entry<bool> debugLoggingEnabledEntry;
        public static MelonPreferences_Entry<bool> showLogLevelCalcEntry;
        public static MelonPreferences_Entry<int> currentMsgLogLevelEntry;
        public static MelonPreferences_Entry<int> currentWarnLogLevelEntry;
        public static readonly Core.Logger logger = new Core.Logger();
        public static List<MixingStationConfiguration> queuedInstances = new List<MixingStationConfiguration>();
        public static Dictionary<MixingStationConfiguration, float> userSetValues = new Dictionary<MixingStationConfiguration, float>();
        public static ConcurrentDictionary<int, float> savedMixerValues = new ConcurrentDictionary<int, float>();
        public static string CurrentSavePath = null;
        public static bool LoadCoroutineStarted = false;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            logger.Msg(1, "MixerThreholdMod initializing...");
            logger.Msg(1, string.Format("currentMsgLogLevel: {0}", logger.CurrentMsgLogLevel));
            logger.Msg(1, string.Format("currentWarnLogLevel: {0}", logger.CurrentWarnLogLevel));

            try
            {
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
                Core.Console.RegisterConsoleCommandViaReflection();
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("OnInitializeMelon: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            try
            {
                queuedInstances.Add(__instance);
                logger.Msg(3, "Queued new MixingStationConfiguration");
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("QueueInstance: Caught exception: {0}", ex));
            }
        }

        public async override void OnUpdate()
        {
            try
            {
                await Core.TrackedMixers.RemoveAllAsync(tm => tm.ConfigInstance == null);
                if (queuedInstances.Count == 0) return;
                var toProcess = queuedInstances.ToList();
                foreach (var instance in toProcess)
                {
                    if (await Core.TrackedMixers.AnyAsync(tm => tm.ConfigInstance == instance))
                    {
                        logger.Warn(1, string.Format("Instance already tracked — skipping duplicate: {0}", instance));
                        continue;
                    }
                    if (instance.StartThrehold == null)
                    {
                        logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                        continue;
                    }
                    var mixerData = await Core.TrackedMixers.FirstOrDefaultAsync(tm => tm.ConfigInstance == instance);
                    if (mixerData == null)
                    {
                        try
                        {
                            instance.StartThrehold.Configure(1f, 20f, true);

                            var newTrackedMixer = new Core.TrackedMixer
                            {
                                ConfigInstance = instance,
                                MixerInstanceID = Core.MixerIDManager.GetMixerID(instance)
                            };
                            await Core.TrackedMixers.AddAsync(newTrackedMixer);
                            logger.Msg(3, string.Format("Created mixer with Stable ID: {0}", newTrackedMixer.MixerInstanceID));

                            if (!newTrackedMixer.ListenerAdded)
                            {
                                logger.Msg(3, string.Format("Attaching listener for Mixer {0}", newTrackedMixer.MixerInstanceID));
                                Helpers.Utils.CoroutineHelper.RunCoroutine(Save.CrashResistantSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                                newTrackedMixer.ListenerAdded = true;
                            }
                            // Restore saved value if exists
                            float savedValue;
                            if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out savedValue))
                            {
                                logger.Msg(2, string.Format("Restoring Mixer {0} to {1}", newTrackedMixer.MixerInstanceID, savedValue));
                                instance.StartThrehold.SetValue(savedValue, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Err("Error configuring mixer: " + ex);
                        }
                    }
                }
                queuedInstances.Clear();
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("OnUpdate: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            try
            {
                return await Core.TrackedMixers.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("MixerExists: Caught exception: {0}", ex));
                return false;
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, "Scene loaded: " + sceneName);
            if (sceneName == "Main")
            {
                try
                {
                    Core.MixerIDManager.MixerInstanceMap.Clear();
                    Core.MixerIDManager.ResetStableIDCounter();

                    savedMixerValues.Clear();
                    logger.Msg(3, "Current Save Path at scene load: " + (CurrentSavePath ?? "[not available yet]"));
                    StartLoadCoroutine();

                    if (!string.IsNullOrEmpty(CurrentSavePath))
                    {
                        Task.Run(async () =>
                        {
                            try
                            {
                                string persistentPath = MelonEnvironment.UserDataDirectory;
                                string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json").Replace('/', '\\');
                                string targetFile = Path.Combine(CurrentSavePath, "MixerThresholdSave.json").Replace('/', '\\');
                                if (File.Exists(sourceFile))
                                {
                                    await Helpers.FileOperations.SafeWriteAllTextAsync(targetFile, await Helpers.FileOperations.SafeReadAllTextAsync(sourceFile));
                                    logger.Msg(3, "Copied MixerThresholdSave.json from persistent to save folder");
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Err(string.Format("Background file copy failed: {0}", ex));
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format("OnSceneWasLoaded: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }
        }

        private static void StartLoadCoroutine()
        {
            try
            {
                if (LoadCoroutineStarted) return;
                LoadCoroutineStarted = true;
                MelonCoroutines.Start(Save.CrashResistantSaveManager.LoadMixerValuesWhenReady());
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("StartLoadCoroutine: Caught exception: {0}", ex));
            }
        }

        // Placeholder methods referenced by Console.cs - can be implemented later
        public static IEnumerator StressGameSaveTestWithComprehensiveMonitoring(int iterations, float delaySeconds, bool bypassCooldown)
        {
            logger.Warn(1, "StressGameSaveTestWithComprehensiveMonitoring: Method not yet implemented");
            yield break;
        }

        public static IEnumerator PerformTransactionalSave()
        {
            logger.Warn(1, "PerformTransactionalSave: Method not yet implemented");
            yield break;
        }

        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            logger.Warn(1, "AdvancedSaveOperationProfiling: Method not yet implemented");
            yield break;
        }
    }
}