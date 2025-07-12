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
        private object updateCoroutine; // Changed from Coroutine to object

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            // Test log
            Logger logger = new Logger();
            logger.Msg(1, "MixerThreholdMod initializing...");
            logger.Msg(1, string.Format("currentMsgLogLevel: {0}", logger.CurrentMsgLogLevel));
            logger.Msg(1, string.Format("currentWarnLogLevel: {0}", logger.CurrentWarnLogLevel));

            try
            {
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
                logger.Msg(3, "Update coroutine started, caller method continues successfully");
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("OnInitializeMelon error: {0}", ex));
            }
        }

        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            try
            {
                queuedInstances.Add(__instance);
                Main.logger.Msg(3, "Queued new MixingStationConfiguration");
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("QueueInstance error: {0}", ex));
            }
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
            Main.logger.Msg(3, "UpdateCoroutine started (inside coroutine)");
            while (true)
            {
                // Move try-catch inside the loop, but keep yield outside try blocks
                IEnumerator processCoroutine = null;
                Exception coroutineError = null;

                try
                {
                    processCoroutine = ProcessQueuedInstances();
                }
                catch (Exception ex)
                {
                    coroutineError = ex;
                }

                if (coroutineError != null)
                {
                    Main.logger.Err(string.Format("Error creating ProcessQueuedInstances coroutine: {0}", coroutineError));
                    // NO yield in catch clause - move outside
                }

                // Handle error case and normal case outside try-catch
                if (coroutineError != null)
                {
                    yield return new WaitForSeconds(1f); // Wait longer on error
                    continue;
                }

                // Yield outside of try-catch block
                if (processCoroutine != null)
                {
                    yield return processCoroutine;
                }

                yield return new WaitForSeconds(0.1f); // Process every 100ms instead of every frame
            }
            Main.logger.Msg(3, "UpdateCoroutine finished (inside coroutine)");
        }

        private IEnumerator ProcessQueuedInstances()
        {
            Main.logger.Msg(3, "ProcessQueuedInstances started");

            // Handle cleanup with separate error handling - NO yield in try-catch
            Task cleanupTask = null;
            Exception cleanupError = null;
            try
            {
                cleanupTask = CleanupTrackedMixers();
            }
            catch (Exception ex)
            {
                cleanupError = ex;
            }

            if (cleanupError != null)
            {
                Main.logger.Err(string.Format("Error starting cleanup: {0}", cleanupError));
                yield break;
            }

            // Wait for cleanup to complete - yield OUTSIDE try-catch
            while (cleanupTask != null && !cleanupTask.IsCompleted)
            {
                yield return null;
            }

            if (cleanupTask != null && cleanupTask.IsFaulted)
            {
                var baseException = cleanupTask.Exception != null ? cleanupTask.Exception.GetBaseException() : null;
                var message = baseException != null ? baseException.Message : "Unknown error";
                Main.logger.Err(string.Format("Cleanup failed: {0}", message));
                yield break;
            }

            if (queuedInstances.Count == 0)
            {
                Main.logger.Msg(4, "No queued instances to process");
                yield break;
            }

            List<MixingStationConfiguration> toProcess = null;
            Exception processingError = null;
            try
            {
                toProcess = queuedInstances.ToList();
                queuedInstances.Clear(); // Clear early to prevent growth
                Main.logger.Msg(3, string.Format("Processing {0} queued instances", toProcess.Count));
            }
            catch (Exception ex)
            {
                processingError = ex;
            }

            if (processingError != null)
            {
                Main.logger.Err(string.Format("Error preparing instances for processing: {0}", processingError));
                yield break;
            }

            foreach (var instance in toProcess)
            {
                if (instance == null) continue;

                // Process each instance with proper error handling - NO yield in try-catch
                IEnumerator singleInstanceCoroutine = null;
                Exception instanceError = null;
                try
                {
                    singleInstanceCoroutine = ProcessSingleInstance(instance);
                }
                catch (Exception ex)
                {
                    instanceError = ex;
                }

                if (instanceError != null)
                {
                    Main.logger.Err(string.Format("Error creating ProcessSingleInstance coroutine: {0}", instanceError));
                    continue;
                }

                // Yield OUTSIDE try-catch
                if (singleInstanceCoroutine != null)
                {
                    yield return singleInstanceCoroutine;
                }

                // Yield occasionally to prevent frame drops
                yield return null;
            }
            Main.logger.Msg(3, "ProcessQueuedInstances finished");
        }

        private IEnumerator ProcessSingleInstance(MixingStationConfiguration instance)
        {
            Main.logger.Msg(3, "Processing single instance started");

            // Check if already tracked - NO yield in try-catch
            Task<bool> existsTask = null;
            Exception existsError = null;
            try
            {
                existsTask = CheckInstanceExists(instance);
            }
            catch (Exception ex)
            {
                existsError = ex;
            }

            if (existsError != null)
            {
                Main.logger.Err(string.Format("Error starting instance check: {0}", existsError));
                yield break;
            }

            // Wait for task completion - yield OUTSIDE try-catch
            while (existsTask != null && !existsTask.IsCompleted)
            {
                yield return null;
            }

            if (existsTask != null && existsTask.IsFaulted)
            {
                var baseException = existsTask.Exception != null ? existsTask.Exception.GetBaseException() : null;
                var message = baseException != null ? baseException.Message : "Unknown error";
                Main.logger.Err(string.Format("Failed to check instance: {0}", message));
                yield break;
            }

            if (existsTask != null && existsTask.Result)
            {
                Main.logger.Warn(1, string.Format("Instance already tracked — skipping duplicate: {0}", instance));
                yield break;
            }

            if (instance.StartThrehold == null)
            {
                Main.logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                yield break;
            }

            // Process the instance - NO yield in try-catch
            IEnumerator createCoroutine = null;
            Exception createError = null;
            try
            {
                createCoroutine = CreateTrackedMixer(instance);
            }
            catch (Exception ex)
            {
                createError = ex;
            }

            if (createError != null)
            {
                Main.logger.Err(string.Format("Error creating CreateTrackedMixer coroutine: {0}", createError));
                yield break;
            }

            // Yield OUTSIDE try-catch
            if (createCoroutine != null)
            {
                yield return createCoroutine;
            }
            Main.logger.Msg(3, "Processing single instance finished");
        }

        private IEnumerator CreateTrackedMixer(MixingStationConfiguration instance)
        {
            Main.logger.Msg(3, "Creating tracked mixer started");

            TrackedMixer newTrackedMixer = null;
            Exception mixerError = null;
            try
            {
                // Configure threshold on main thread (Unity requirement)
                instance.StartThrehold.Configure(1f, 20f, true);

                newTrackedMixer = new TrackedMixer
                {
                    ConfigInstance = instance,
                    MixerInstanceID = MixerIDManager.GetMixerID(instance)
                };
            }
            catch (Exception ex)
            {
                mixerError = ex;
            }

            if (mixerError != null)
            {
                Main.logger.Err(string.Format("Error configuring mixer: {0}", mixerError));
                yield break;
            }

            // Add to tracked mixers - NO yield in try-catch
            Task addTask = null;
            Exception addError = null;
            try
            {
                addTask = AddTrackedMixer(newTrackedMixer);
            }
            catch (Exception ex)
            {
                addError = ex;
            }

            if (addError != null)
            {
                Main.logger.Err(string.Format("Error starting add tracked mixer: {0}", addError));
                yield break;
            }

            // Wait for task completion - yield OUTSIDE try-catch
            while (addTask != null && !addTask.IsCompleted)
            {
                yield return null;
            }

            if (addTask != null && addTask.IsFaulted)
            {
                var baseException = addTask.Exception != null ? addTask.Exception.GetBaseException() : null;
                var message = baseException != null ? baseException.Message : "Unknown error";
                Main.logger.Err(string.Format("Failed to add tracked mixer: {0}", message));
                yield break;
            }

            Main.logger.Msg(3, string.Format("Created mixer with Stable ID: {0}", newTrackedMixer.MixerInstanceID));

            // Handle listener and restore operations - separate error handling
            Exception listenerError = null;
            try
            {
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
                listenerError = ex;
            }

            if (listenerError != null)
            {
                Main.logger.Err(string.Format("Error setting up mixer listener/restore: {0}", listenerError));
            }
            Main.logger.Msg(3, "Creating tracked mixer finished");
        }

        // Wrapper methods for async operations with proper error handling
        private async Task CleanupTrackedMixers()
        {
            try
            {
                await TrackedMixers.RemoveAllAsync(delegate (TrackedMixer tm) { return tm != null && tm.ConfigInstance == null; });
                Main.logger.Msg(4, "Cleanup tracked mixers completed");
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
                var result = await TrackedMixers.AnyAsync(delegate (TrackedMixer tm) { return tm != null && tm.ConfigInstance == instance; });
                Main.logger.Msg(4, string.Format("Instance exists check result: {0}", result));
                return result;
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
                Main.logger.Msg(3, "Successfully added tracked mixer");
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("Add mixer error: {0}", ex));
                throw; // Re-throw to be handled by caller
            }
        }

        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            try
            {
                return await TrackedMixers.AnyAsync(delegate (TrackedMixer tm) { return tm.MixerInstanceID == mixerInstanceID; });
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("MixerExists error: {0}", ex));
                return false;
            }
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

            try
            {
                MelonCoroutines.Start(MixerSaveManager.LoadMixerValuesWhenReady());
                Main.logger.Msg(3, "Load coroutine started successfully");
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("StartLoadCoroutine error: {0}", ex));
            }
        }

        // Clean shutdown
        public override void OnApplicationQuit()
        {
            try
            {
                if (updateCoroutine != null)
                {
                    MelonCoroutines.Stop(updateCoroutine);
                    Main.logger.Msg(2, "Update coroutine stopped");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("OnApplicationQuit error: {0}", ex));
            }
            finally
            {
                base.OnApplicationQuit();
            }
        }
    }
} // End of namespace