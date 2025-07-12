using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using MixerThreholdMod_0_0_1.Core;
using MixerThreholdMod_0_0_1.Save;
using MixerThreholdMod_0_0_1.Threading;
using ScheduleOne.Management;
using ScheduleOne.Noise;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using static MelonLoader.MelonLaunchOptions;
using static MelonLoader.MelonLogger;
using static ScheduleOne.Console;
using static VLB.Consts;

// Reminder: Add to steam game startup command: "--melonloader.captureplayerlogs" for extra MelonLogger verbosity :)

// Assembly attributes must come first, before namespace
[assembly: MelonInfo(typeof(MixerThreholdMod_1_0_0.Main), "MixerThreholdMod", "1.0.0", "mooleshacat")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_1_0_0
{
    /// <summary>
    /// MixerThresholdMod - Main class for mixer threshold management and save crash prevention.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This mod is specifically designed to prevent the crashes
    /// identified in PR #12 during save operations, especially with repeated saves and extended gameplay.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are designed to be thread-safe and not block Unity's main thread.
    /// File operations use proper locking and async patterns to prevent UI freezes and deadlocks.
    /// 
    /// ⚠️ MAIN THREAD WARNING: This mod uses coroutines and background tasks to prevent blocking
    /// Unity's main thread during file I/O operations.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses compatible async/await patterns
    /// - String.Format instead of string interpolation
    /// - Compatible exception handling throughout
    /// - Proper ConcurrentDictionary usage
    /// 
    /// Key Features:
    /// - Crash-resistant save system with atomic operations
    /// - Emergency save on application quit/crash
    /// - Thread-safe mixer value tracking
    /// - Automatic backup system with cleanup
    /// - Comprehensive error handling and logging
    /// </summary>
    public class Main : MelonMod
    {
        // Core components
        public static readonly Logger logger = new Logger();
        public static new HarmonyLib.Harmony HarmonyInstance = new HarmonyLib.Harmony("com.mooleshacat.mixerthreholdmod");

        // Thread-safe collections for .NET 4.8.1 compatibility
        public static readonly ThreadSafeList<MixingStationConfiguration> queuedInstances = new ThreadSafeList<MixingStationConfiguration>();
        public static readonly ConcurrentDictionary<int, float> savedMixerValues = new ConcurrentDictionary<int, float>();

        // State management
        public static string CurrentSavePath = null;
        private Coroutine mainUpdateCoroutine;
        private static bool isShuttingDown = false;

        /// <summary>
        /// Initialize the mod with crash prevention mechanisms
        /// </summary>
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            // Critical: Add unhandled exception handler for crash prevention
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            logger.Msg(1, "[MAIN] MixerThresholdMod initializing - focused on save crash prevention");
            logger.Msg(1, string.Format("[MAIN] Debug levels - Msg: {0}, Warn: {1}", logger.CurrentMsgLogLevel, logger.CurrentWarnLogLevel));

            Exception initError = null;
            try
            {
                // Patch the MixingStationConfiguration constructor to queue instances
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
                    logger.Err("[MAIN] CRITICAL: MixingStationConfiguration constructor not found - mod cannot function");
                    return;
                }

                // Register console commands for debugging
                Core.Console.RegisterConsoleCommandViaReflection();
                logger.Msg(2, "[MAIN] ✓ Console commands registered");

                logger.Msg(2, "[MAIN] Patched MixingStationConfiguration constructor successfully");

                // Register console commands for debugging
                Console.RegisterConsoleCommandViaReflection();
                logger.Msg(3, "[MAIN] Console commands registered");

                // Start the main update coroutine
                var coroutineObj = MelonCoroutines.Start(UpdateCoroutine());
                mainUpdateCoroutine = coroutineObj as Coroutine;
                logger.Msg(2, "[MAIN] Main update coroutine started successfully");
            }
            catch (Exception ex)
            {
                initError = ex;
            }

            if (initError != null)
            {
                logger.Err(string.Format("[MAIN] CRITICAL: Initialization failed: {0}\nStackTrace: {1}", initError.Message, initError.StackTrace));
            }
            else
            {
                logger.Msg(1, "[MAIN] MixerThresholdMod initialization completed successfully");
            }
            HarmonyInstance.Patch(
                constructor,
                prefix: new HarmonyMethod(typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic))
            );
            logger.Msg(2, "Patched constructor");
            Console.RegisterConsoleCommandViaReflection();
        }

        /// <summary>
        /// Global exception handler for crash prevention and emergency saves
        /// </summary>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                logger.Err(string.Format("[MAIN] CRITICAL CRASH DETECTED: {0}\nStackTrace: {1}", 
                    exception.Message, exception.StackTrace));

                // Emergency save attempt - NO YIELD RETURNS in crash handler
                Exception emergencyError = null;
                try
                {
                    CrashResistantSaveManager.EmergencySave();
                    logger.Msg(1, "[MAIN] Emergency save completed before crash");
                }
                catch (Exception saveEx)
                {
                    emergencyError = saveEx;
                }

                if (emergencyError != null)
                {
                    logger.Err(string.Format("[MAIN] Emergency save failed: {0}", emergencyError.Message));
                }
            }
            else
            {
                logger.Err(string.Format("[MAIN] CRITICAL: Non-exception crash object: {0}", e.ExceptionObject));
            }
        }

        /// <summary>
        /// Queue mixer instance for processing (Harmony prefix patch)
        /// </summary>
        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            if (isShuttingDown || __instance == null) return;

            Exception queueError = null;
            try
            {
                queuedInstances.Add(__instance);
                logger.Msg(3, "[MAIN] Queued new MixingStationConfiguration for processing");
            }
            catch (Exception ex)
            {
                queueError = ex;
            }

            if (queueError != null)
            {
                logger.Err(string.Format("[MAIN] QueueInstance error: {0}\nStackTrace: {1}", queueError.Message, queueError.StackTrace));
            }
        }

        /// <summary>
        /// Main update loop - uses coroutines to prevent blocking Unity thread
        /// ⚠️ CRASH PREVENTION: All operations designed to not block main thread
        /// </summary>
        public override void OnUpdate()
        {
            // Keep OnUpdate minimal for .NET 4.8.1 compatibility
            if (isShuttingDown && mainUpdateCoroutine != null)
            {
                MelonCoroutines.Stop(mainUpdateCoroutine);
                mainUpdateCoroutine = null;
                logger.Msg(2, "[MAIN] Main update coroutine stopped due to shutdown");
            }
        }

        /// <summary>
        /// Main update coroutine for processing queued mixer instances
        /// </summary>
        private IEnumerator UpdateCoroutine()
        {
            logger.Msg(3, "[MAIN] UpdateCoroutine started");

            while (!isShuttingDown)
            {
                // Clean up null/invalid mixers first
                TrackedMixers.RemoveAll(tm => tm?.ConfigInstance == null);

                // Process queued instances
                if (queuedInstances.Count > 0)
                {
                    queuedInstances.Clear();
                }
                catch 
                {
                    // Even clearing failed - something is seriously wrong
                    logger.Err("ProcessQueuedInstancesAsync: Even queue clearing failed!");
                }
            }
        }

                    logger.Msg(3, string.Format("[MAIN] Processing {0} queued mixer instances", toProcess.Count));

                    foreach (var instance in toProcess)
                    {
                        if (instance == null)
                        {
                            logger.Warn(1, "ProcessQueuedInstancesAsync: Skipping null instance");
                            continue;
                        }

                        // Process each instance with crash protection
                        yield return ProcessMixerInstance(instance);

                                var newTrackedMixer = new Core.TrackedMixer
                                {
                                    ConfigInstance = instance,
                                    MixerInstanceID = Core.MixerIDManager.GetMixerID(instance)
                                };
                                await Core.TrackedMixers.AddAsync(newTrackedMixer);
                                logger.Msg(2, string.Format("Created mixer with Stable ID: {0}", newTrackedMixer.MixerInstanceID));

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
                            catch (Exception mixerEx)
                            {
                                logger.Err(string.Format("Error configuring individual mixer: {0}\n{1}", mixerEx.Message, mixerEx.StackTrace));
                                // Continue processing other mixers despite this failure
                            }
                        }
                    }
                    catch (Exception instanceEx)
                    {
                        logger.Err(string.Format("Error processing individual instance: {0}\n{1}", instanceEx.Message, instanceEx.StackTrace));
                        // Continue processing other instances despite this failure
                    }
                }

                // Normal processing interval
                yield return new WaitForSeconds(0.1f);
            }

            logger.Msg(3, "[MAIN] UpdateCoroutine finished");
        }

        /// <summary>
        /// Process a single mixer instance with crash protection
        /// </summary>
        private IEnumerator ProcessMixerInstance(MixingStationConfiguration instance)
        {
            if (instance == null || isShuttingDown) yield break;

            Exception processingError = null;
            TrackedMixer newTrackedMixer = null;

            try
            {
                // Check if already tracked
                bool alreadyTracked = TrackedMixers.Any(tm => tm?.ConfigInstance == instance);
                if (alreadyTracked)
                {
                    logger.Warn(2, "[MAIN] Instance already tracked - skipping duplicate");
                    yield break;
                }

                if (instance.StartThrehold == null)
                {
                    logger.Warn(2, "[MAIN] StartThreshold is null - skipping instance");
                    yield break;
                }

                // Configure threshold on main thread (Unity requirement)
                instance.StartThrehold.Configure(1f, 20f, true);

                // Create tracked mixer
                newTrackedMixer = new TrackedMixer
                {
                    ConfigInstance = instance,
                    MixerInstanceID = MixerIDManager.GetMixerID(instance)
                };

                // Add to tracking collection
                TrackedMixers.Add(newTrackedMixer);
                logger.Msg(2, string.Format("[MAIN] Created mixer with ID: {0}", newTrackedMixer.MixerInstanceID));
            }
            catch (Exception ex)
            {
                processingError = ex;
            }

            if (processingError != null)
            {
                logger.Err(string.Format("[MAIN] ProcessMixerInstance CRASH PREVENTION: Error: {0}\nStackTrace: {1}", 
                    processingError.Message, processingError.StackTrace));
                yield break;
            }

            if (newTrackedMixer == null) yield break;

            // Attach value change listener
            if (!newTrackedMixer.ListenerAdded)
            {
                logger.Msg(3, string.Format("[MAIN] Attaching listener for Mixer {0}", newTrackedMixer.MixerInstanceID));

                Exception listenerError = null;
                try
                {
                    MelonCoroutines.Start(CrashResistantSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                    newTrackedMixer.ListenerAdded = true;
                    logger.Msg(2, string.Format("[MAIN] Listener attached successfully for Mixer {0}", newTrackedMixer.MixerInstanceID));
                }
                catch 
                {
                    listenerError = listenerEx;
                }

                if (listenerError != null)
                {
                    logger.Err(string.Format("[MAIN] Failed to attach listener for Mixer {0}: {1}", newTrackedMixer.MixerInstanceID, listenerError.Message));
                }
            }

            // Restore saved value if exists
            float savedValue;
            if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out savedValue))
            {
                logger.Msg(2, string.Format("[MAIN] Restoring Mixer {0} to saved value: {1}", newTrackedMixer.MixerInstanceID, savedValue));

                Exception restoreError = null;
                try
                {
                    instance.StartThrehold.SetValue(savedValue, true);
                    logger.Msg(2, string.Format("[MAIN] Successfully restored Mixer {0}", newTrackedMixer.MixerInstanceID));
                }
                catch (Exception ex)
                {
                    restoreError = ex;
                }

                if (restoreError != null)
                {
                    logger.Err(string.Format("[MAIN] Failed to restore value for Mixer {0}: {1}", newTrackedMixer.MixerInstanceID, restoreError.Message));
                }
            }
        }

        /// <summary>
        /// Check if a mixer exists by ID
        /// </summary>
        public static bool MixerExists(int mixerInstanceID)
        {
            try
            {
                return TrackedMixers.Any(tm => tm.MixerInstanceID == mixerInstanceID);
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[MAIN] MixerExists error: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Handle scene loading with crash prevention
        /// </summary>
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, string.Format("[MAIN] Scene loaded: {0}", sceneName));

            if (sceneName == "Main" && !isShuttingDown)
            {
                try
                {
                    // Reset mixer state for new game session
                    if (MixerIDManager.MixerInstanceMap != null)
                    {
                        MixerIDManager.MixerInstanceMap.Clear();
                    }
                    MixerIDManager.ResetStableIDCounter();

                    // Clear previous mixer values
                    savedMixerValues.Clear();
                    logger.Msg(3, "Current Save Path at scene load: " + (Main.CurrentSavePath ?? "[not available yet]"));

                    logger.Msg(2, string.Format("[MAIN] Main scene loaded - save path: {0}", CurrentSavePath ?? "[not available yet]"));

                    // Start load coroutine once per session
                    if (!_loadCoroutineStarted)
                    {
                        StartLoadCoroutine();
                    }

                    // Copy emergency save to current save path if available
                    if (!string.IsNullOrEmpty(CurrentSavePath))
                    {
                        CopyEmergencySaveIfExists();
                    }
                }
                catch (Exception ex)
                {
                    sceneError = ex;
                }

                if (sceneError != null)
                {
                    logger.Err(string.Format("[MAIN] OnSceneWasLoaded CRASH PREVENTION: Error: {0}\nStackTrace: {1}", 
                        sceneError.Message, sceneError.StackTrace));
                }
            }
        }

        private static bool _loadCoroutineStarted = false;

        /// <summary>
        /// Start the load coroutine once per session
        /// </summary>
        private static void StartLoadCoroutine()
        {
            if (_loadCoroutineStarted || isShuttingDown) return;

            MelonCoroutines.Start(MixerSaveManager.LoadMixerValuesWhenReady());
        }

        // Console command implementations for comprehensive testing and debugging
        public static IEnumerator StressGameSaveTestWithComprehensiveMonitoring(int iterations, float delaySeconds, bool bypassCooldown)
        {
            logger.Msg(1, string.Format("[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}", iterations, delaySeconds, bypassCooldown));
            
            var startTime = DateTime.Now;
            int successCount = 0;
            int failureCount = 0;
            
            for (int i = 1; i <= iterations; i++)
            {
                try
                {
                    logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} - Starting save operation", i, iterations));
                    
                    // Track timing for this iteration
                    var iterationStart = DateTime.Now;
                    
                    // Perform the save with comprehensive monitoring
                    yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
                    
                    var iterationTime = (DateTime.Now - iterationStart).TotalMilliseconds;
                    logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} completed in {2:F1}ms", i, iterations, iterationTime));
                    successCount++;
                    
                    // Add delay if specified
                    if (delaySeconds > 0f)
                    {
                        logger.Msg(3, string.Format("[MONITOR] Waiting {0:F3}s before next iteration...", delaySeconds));
                        yield return new WaitForSeconds(delaySeconds);
                    }
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format("[MONITOR] Iteration {0}/{1} FAILED: {2}", i, iterations, ex.Message));
                    failureCount++;
                }
            }
            
            var totalTime = (DateTime.Now - startTime).TotalSeconds;
            logger.Msg(1, string.Format("[MONITOR] Comprehensive monitoring complete: {0} success, {1} failures in {2:F1}s", successCount, failureCount, totalTime));
        }

        public static IEnumerator PerformTransactionalSave()
        {
            logger.Msg(1, "[CONSOLE] Starting atomic transactional save operation");
            
            try
            {
                _loadCoroutineStarted = true;
                MelonCoroutines.Start(CrashResistantSaveManager.LoadMixerValuesWhenReady());
                logger.Msg(2, "[MAIN] Load coroutine started successfully");
            }
            catch (Exception ex)
            {
                startError = ex;
                _loadCoroutineStarted = false; // Reset on error
            }

            if (startError != null)
            {
                logger.Err(string.Format("[MAIN] StartLoadCoroutine error: {0}\nStackTrace: {1}", startError.Message, startError.StackTrace));
            }
        }

        /// <summary>
        /// Copy emergency save file to current save path if it exists
        /// </summary>
        private static void CopyEmergencySaveIfExists()
        {
            try
            {
                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (string.IsNullOrEmpty(persistentPath) || string.IsNullOrEmpty(CurrentSavePath)) return;

                string emergencyFile = Path.Combine(persistentPath, "MixerThresholdSave_Emergency.json");
                string targetFile = Path.Combine(CurrentSavePath, "MixerThresholdSave.json");

                if (File.Exists(emergencyFile) && !File.Exists(targetFile))
                {
                    File.Copy(emergencyFile, targetFile, overwrite: false);
                    logger.Msg(2, "[MAIN] Copied emergency save to current save location");
                }
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[MAIN] CopyEmergencySaveIfExists error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Application quit handler with emergency save
        /// </summary>
        public override void OnApplicationQuit()
        {
            isShuttingDown = true;
            logger.Msg(1, "[MAIN] Application shutting down - performing cleanup");

            Exception quitError = null;
            try
            {
                // Stop main coroutine
                if (mainUpdateCoroutine != null)
                {
                    MelonCoroutines.Stop(mainUpdateCoroutine);
                    mainUpdateCoroutine = null;
                    logger.Msg(2, "[MAIN] Main update coroutine stopped");
                }

                // Emergency save if we have data
                if (savedMixerValues.Count > 0)
                {
                    CrashResistantSaveManager.EmergencySave();
                    logger.Msg(1, "[MAIN] Emergency save completed on quit");
                }
                else
                {
                    logger.Msg(2, "[MAIN] No mixer data to save on quit");
                }
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[PROFILE] Advanced profiling FAILED: {0}", ex.Message));
            }

            if (quitError != null)
            {
                logger.Err(string.Format("[MAIN] OnApplicationQuit error: {0}", quitError.Message));
            }

            logger.Msg(1, "[MAIN] MixerThresholdMod shutdown completed");
            base.OnApplicationQuit();
        }
    }
} // End of namespace