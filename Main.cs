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
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

// Reminder: Add to steam game startup command: "--melonloader.captureplayerlogs" for extra MelonLogger verbosity :)

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
        public static readonly Logger logger = new Logger();
        private static List<MixingStationConfiguration> queuedInstances = new List<MixingStationConfiguration>();
        public static Dictionary<MixingStationConfiguration, float> userSetValues = new Dictionary<MixingStationConfiguration, float>();
        public static Main Instance { get; private set; }
        public static Harmony HarmonyInstance { get; private set; }
        public static Dictionary<int, float> savedMixerValues = new Dictionary<int, float>();
        public static string CurrentSavePath = null;
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            Instance = this; // ADD THIS LINE
            HarmonyInstance = new Harmony("MixerThreholdMod_1_0_0");
            // Test log
            Logger logger = new Logger();
            logger.Msg(1, "MixerThreholdMod initializing...");
            logger.Msg(1, string.Format("currentMsgLogLevel: {0}", logger.CurrentMsgLogLevel));
            logger.Msg(1, string.Format("currentWarnLogLevel: {0}", logger.CurrentWarnLogLevel));
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
            queuedInstances.Add(__instance);
            logger.Msg(3, "Queued new MixingStationConfiguration");
        }
        public async override void OnUpdate()
        {
            // 🔁 Clean up tracked mixers at the start of each update
            await TrackedMixers.RemoveAllAsync(tm => tm.ConfigInstance == null);
            if (queuedInstances.Count == 0) return;
            var toProcess = queuedInstances.ToList();
            foreach (var instance in toProcess)
            {
                // Prevent duplicate processing of the same instance
                if (await TrackedMixers.AnyAsync(tm => tm.ConfigInstance == instance))
                {
                    logger.Warn(1, string.Format("Instance already tracked — skipping duplicate: {0}", instance));
                    continue;
                }
                if (instance.StartThrehold == null)
                {
                    logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                    continue;
                }
                var mixerData = await TrackedMixers.FirstOrDefaultAsync(tm => tm.ConfigInstance == instance);
                if (mixerData == null)
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
                        logger.Msg(3, string.Format("Created mixer with Stable ID: {0}", newTrackedMixer.MixerInstanceID));

                        // Now safely add listener
                        if (!mixerData.ListenerAdded)
                        {
                            logger.Msg(3, string.Format("Attaching listener for Mixer {0}", newTrackedMixer.MixerInstanceID));
                            Utils.CoroutineHelper.RunCoroutine(MixerSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                            newTrackedMixer.ListenerAdded = true;
                        }
                        // Restore saved value if exists
                        if (savedMixerValues.TryGetValue(mixerData.MixerInstanceID, out float savedValue))
                        {
                            logger.Msg(2, string.Format("Restoring Mixer {0} to {1}", mixerData.MixerInstanceID, savedValue));
                            instance.StartThrehold.SetValue(savedValue, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Err("Error configuring mixer: " + ex);
                    }
                }
            }
            queuedInstances.Clear();
        }



        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            return await TrackedMixers.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, "Scene loaded: " + sceneName);
            if (sceneName == "Main")
            {
                try
                {
                    // Reset mixer IDs
                    MixerIDManager.MixerInstanceMap.Clear();
                    MixerIDManager.ResetStableIDCounter();

                    // Clear previous mixer values
                    savedMixerValues.Clear();
                    logger.Msg(3, "Current Save Path at scene load: " + (Main.CurrentSavePath ?? "[not available yet]"));

                    // Start coroutine to wait for save path
                    StartLoadCoroutine();
                    // Force-refresh file copy to save folder
                    if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        string persistentPath = MelonEnvironment.UserDataDirectory;
                        string sourceFile = Path.Combine(persistentPath, MIXER_SAVE_FILENAME).Replace('/', '\\');
                        string targetFile = Path.Combine(Main.CurrentSavePath, MIXER_SAVE_FILENAME).Replace('/', '\\');
                        if (File.Exists(sourceFile))
                        {
                            FileOperations.SafeCopy(sourceFile, targetFile, overwrite: true);
                            logger.Msg(3, "Copied MixerThresholdSave.json from persistent to save folder");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.logger.Err(string.Format("OnSceneWasLoaded: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }
        }

        private static bool _coroutineStarted = false;
        private static void StartLoadCoroutine()
        {
            if (_coroutineStarted) return;
            _coroutineStarted = true;

            MelonCoroutines.Start(MixerSaveManager.LoadMixerValuesWhenReady());
        }

        /// <summary>
        /// Comprehensive stress test with monitoring integration for dnSpy verification
        /// ⚠️ ASYNC JUSTIFICATION: Stress testing with delays can take several minutes
        /// ⚠️ THREAD SAFETY: All operations use thread-safe collections and async patterns
        /// </summary>
        public static IEnumerator StressGameSaveTestWithComprehensiveMonitoring(int iterations, float delaySeconds, bool bypassCooldown)
        {
            Exception stressError = null;
            int successCount = 0;
            int errorCount = 0;
            var startTime = DateTime.Now;

            try
            {
                logger.Msg(1, string.Format("[STRESS] Starting comprehensive game save stress test: {0} iterations, {1:F3}s delay, bypass: {2}",
                    iterations, delaySeconds, bypassCooldown));

                for (int i = 1; i <= iterations; i++)
                {
                    var iterationStart = DateTime.Now;
                    Exception iterationError = null;

                    try
                    {
                        logger.Msg(2, string.Format("[STRESS] === ITERATION {0}/{1} ===", i, iterations));

                        // Take system snapshot before save
                        Core.AdvancedSystemPerformanceMonitor.TakeMemorySnapshot(string.Format("PRE_SAVE_{0}", i));

                        // Attempt save using crash-resistant save manager
                        yield return MelonCoroutines.Start(Save.CrashResistantSaveManager.StressGameSaveTest(i, bypassCooldown ? 0f : delaySeconds));

                        // Take system snapshot after save
                        Core.AdvancedSystemPerformanceMonitor.TakeMemorySnapshot(string.Format("POST_SAVE_{0}", i));

                        successCount++;

                        var iterationTime = (DateTime.Now - iterationStart).TotalMilliseconds;
                        logger.Msg(2, string.Format("[STRESS] Iteration {0} completed in {1:F1}ms", i, iterationTime));

                        // Apply delay if specified
                        if (delaySeconds > 0f)
                        {
                            yield return new WaitForSeconds(delaySeconds);
                        }
                    }
                    catch (Exception ex)
                    {
                        iterationError = ex;
                        errorCount++;
                    }

                    if (iterationError != null)
                    {
                        logger.Err(string.Format("[STRESS] Iteration {0} failed: {1}", i, iterationError.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                stressError = ex;
            }
            finally
            {
                var totalTime = DateTime.Now - startTime;
                logger.Msg(1, string.Format("[STRESS] === COMPREHENSIVE STRESS TEST COMPLETE ==="));
                logger.Msg(1, string.Format("[STRESS] Total time: {0:F1}s", totalTime.TotalSeconds));
                logger.Msg(1, string.Format("[STRESS] Success: {0}/{1} ({2:F1}%)", successCount, iterations, (successCount * 100.0) / iterations));
                logger.Msg(1, string.Format("[STRESS] Errors: {0}/{1} ({2:F1}%)", errorCount, iterations, (errorCount * 100.0) / iterations));

                // Generate final memory analysis
                Core.AdvancedSystemPerformanceMonitor.GenerateMemoryLeakReport();

                if (stressError != null)
                {
                    logger.Err(string.Format("[STRESS] Overall test error: {0}", stressError.Message));
                }
            }
        }

    }
} // End of namespace