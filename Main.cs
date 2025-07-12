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
        private Coroutine updateCoroutine;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
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

            // Start the update coroutine instead of using async OnUpdate
            updateCoroutine = MelonCoroutines.Start(UpdateCoroutine());
        }

        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            queuedInstances.Add(__instance);
            Main.logger.Msg(3, "Queued new MixingStationConfiguration");
        }

        // Remove the async override and replace with synchronous version
        public override void OnUpdate()
        {
            // Keep this minimal - just essential frame-by-frame checks
            // Heavy lifting is now done in the coroutine
        }

        // New coroutine-based update system
        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                yield return ProcessQueuedInstances();
                yield return new WaitForSeconds(0.1f); // Process every 100ms instead of every frame
            }
        }

        private IEnumerator ProcessQueuedInstances()
        {
            try
            {
                // Clean up tracked mixers - .NET 4.8.1 compatible async handling
                var cleanupTask = CleanupTrackedMixers();
                while (!cleanupTask.IsCompleted)
                {
                    yield return null;
                }

                if (cleanupTask.IsFaulted)
                {
                    var baseException = cleanupTask.Exception != null ? cleanupTask.Exception.GetBaseException() : null;
                    var message = baseException != null ? baseException.Message : "Unknown error";
                    Main.logger.Err(string.Format("Cleanup failed: {0}", message));
                    yield break;
                }

                if (queuedInstances.Count == 0) yield break;

                var toProcess = queuedInstances.ToList();
                queuedInstances.Clear(); // Clear early to prevent growth

                foreach (var instance in toProcess)
                {
                    if (instance == null) continue;

                    // Process each instance with proper error handling
                    yield return ProcessSingleInstance(instance);

                    // Yield occasionally to prevent frame drops
                    yield return null;
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Error in ProcessQueuedInstances: {0}", ex));
            }
        }

        private IEnumerator ProcessSingleInstance(MixingStationConfiguration instance)
        {
            try
            {
                // Check if already tracked - .NET 4.8.1 compatible async handling
                var existsTask = CheckInstanceExists(instance);
                while (!existsTask.IsCompleted)
                {
                    yield return null;
                }

                if (existsTask.IsFaulted)
                {
                    var baseException = existsTask.Exception != null ? existsTask.Exception.GetBaseException() : null;
                    var message = baseException != null ? baseException.Message : "Unknown error";
                    Main.logger.Err(string.Format("Failed to check instance: {0}", message));
                    yield break;
                }

                if (existsTask.Result)
                {
                    Main.logger.Warn(1, string.Format("Instance already tracked — skipping duplicate: {0}", instance));
                    yield break;
                }

                if (instance.StartThrehold == null)
                {
                    Main.logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                    yield break;
                }

                // Process the instance
                yield return CreateTrackedMixer(instance);
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Error processing instance: {0}", ex));
            }
        }

        private IEnumerator CreateTrackedMixer(MixingStationConfiguration instance)
        {
            try
            {
                // Configure threshold on main thread (Unity requirement)
                instance.StartThrehold.Configure(1f, 20f, true);

                var newTrackedMixer = new TrackedMixer
                {
                    ConfigInstance = instance,
                    MixerInstanceID = MixerIDManager.GetMixerID(instance)
                };

                // Add to tracked mixers - .NET 4.8.1 compatible async handling
                var addTask = AddTrackedMixer(newTrackedMixer);
                while (!addTask.IsCompleted)
                {
                    yield return null;
                }

                if (addTask.IsFaulted)
                {
                    var baseException = addTask.Exception != null ? addTask.Exception.GetBaseException() : null;
                    var message = baseException != null ? baseException.Message : "Unknown error";
                    Main.logger.Err(string.Format("Failed to add tracked mixer: {0}", message));
                    yield break;
                }

                Main.logger.Msg(3, string.Format("Created mixer with Stable ID: {0}", newTrackedMixer.MixerInstanceID));

                // Add listener safely
                if (!newTrackedMixer.ListenerAdded)
                {
                    Main.logger.Msg(3, string.Format("Attaching listener for Mixer {0}", newTrackedMixer.MixerInstanceID));
                    Utils.CoroutineHelper.RunCoroutine(MixerSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                    newTrackedMixer.ListenerAdded = true;
                }

                // Restore saved value if exists
                float savedValue;
                if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out savedValue))
                {
                    Main.logger.Msg(2, string.Format("Restoring Mixer {0} to {1}", newTrackedMixer.MixerInstanceID, savedValue));
                    instance.StartThrehold.SetValue(savedValue, true);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Error creating tracked mixer: {0}", ex));
            }
        }

        // Wrapper methods for async operations with proper error handling
        private async Task CleanupTrackedMixers()
        {
            try
            {
                await TrackedMixers.RemoveAllAsync(delegate (TrackedMixer tm) { return tm != null && tm.ConfigInstance == null; });
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Cleanup error: {0}", ex));
                throw; // Re-throw to be handled by caller
            }
        }

        private async Task<bool> CheckInstanceExists(MixingStationConfiguration instance)
        {
            try
            {
                return await TrackedMixers.AnyAsync(delegate (TrackedMixer tm) { return tm != null && tm.ConfigInstance == instance; });
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Check instance error: {0}", ex));
                return false; // Assume doesn't exist on error
            }
        }

        private async Task AddTrackedMixer(TrackedMixer mixer)
        {
            try
            {
                await TrackedMixers.AddAsync(mixer);
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Add mixer error: {0}", ex));
                throw; // Re-throw to be handled by caller
            }
        }

        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            return await TrackedMixers.AnyAsync(delegate (TrackedMixer tm) { return tm.MixerInstanceID == mixerInstanceID; });
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            Main.logger.Msg(2, "Scene loaded: " + sceneName);
            if (sceneName == "Main")
            {
                try
                {
                    // Reset mixer IDs
                    MixerIDManager.MixerInstanceMap.Clear();
                    MixerIDManager.ResetStableIDCounter();

                    // Clear previous mixer values
                    savedMixerValues.Clear();
                    Main.logger.Msg(3, "Current Save Path at scene load: " + (Main.CurrentSavePath ?? "[not available yet]"));

                    // Start coroutine to wait for save path
                    StartLoadCoroutine();
                    // Force-refresh file copy to save folder
                    if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        string persistentPath = MelonEnvironment.UserDataDirectory;
                        string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json").Replace('/', '\\');
                        string targetFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json").Replace('/', '\\');
                        if (File.Exists(sourceFile))
                        {
                            FileOperations.SafeCopy(sourceFile, targetFile, overwrite: true);
                            Main.logger.Msg(3, "Copied MixerThresholdSave.json from persistent to save folder");
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

        // Clean shutdown
        public override void OnApplicationQuit()
        {
            if (updateCoroutine != null)
            {
                MelonCoroutines.Stop(updateCoroutine);
            }
            base.OnApplicationQuit();
        }
    }
} // End of namespace