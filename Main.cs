using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using MelonLoader;
using MelonLoader.Utils;
using HarmonyLib;
using MixerThreholdMod_1_0_0.Core;    // ✅ ESSENTIAL - Keep this! IDE false positive
using MixerThreholdMod_1_0_0.Helpers; // ✅ NEEDED
using MixerThreholdMod_1_0_0.Save;    // ✅ NEEDED
// Suspect these two may be IL2CPP compatibility problems same as below
//using ScheduleOne.Noise;
//using ScheduleOne.ObjectScripts;
// COMMENTED FOR TESTING COMPILE TIME ERRORS? IT APPEARS OK ... MAYBE NOT NEEDED.

// We fixed these and used type resolver, do we need to do same for the above?
// NOTE: I have seen MixerIDManager.cs access ScheduleOne.Manager and not use type resolver.
// It's using declaration was even removed once and commented to use IL2CPPTypeResolver but
// somehow it got added back. We need to fix it. This makes me think the above is not ok either.

// IL2CPP COMPATIBILITY: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading

// We fixed these and used type resolver, do we need to do same for the above?
// NOTE: I have seen MixerIDManager.cs access ScheduleOne.Manager and not use type resolver.
// It's using declaration was even removed once and commented to use IL2CPPTypeResolver but
// somehow it got added back. We need to fix it. This makes me think the above is not ok either.

// IL2CPP COMPATIBILITY: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading

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
        public static Main Instance { get; private set; }
        public static MelonPreferences_Entry<bool> debugLoggingEnabledEntry;
        public static MelonPreferences_Entry<bool> showLogLevelCalcEntry;
        public static MelonPreferences_Entry<int> currentMsgLogLevelEntry;
        public static MelonPreferences_Entry<int> currentWarnLogLevelEntry;
        public static readonly Core.Logger logger = new Core.Logger();

        public static new HarmonyLib.Harmony HarmonyInstance { get; private set; }

        // IL2CPP COMPATIBLE: Use object instead of specific types to avoid TypeLoadException in IL2CPP builds
        // Types will be resolved dynamically using IL2CPPTypeResolver when needed
        public static List<object> queuedInstances = new List<object>();
        public static Dictionary<object, float> userSetValues = new Dictionary<object, float>();
        public static ConcurrentDictionary<int, float> savedMixerValues = new ConcurrentDictionary<int, float>();
        
        public static string CurrentSavePath = null;
        public static bool LoadCoroutineStarted = false;
        private static bool _consoleTestCompleted = false;

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
                    logger.Msg(1, "=== LOGGER TEST: MixerThreholdMod v1.0.0 Starting ===");
                    System.Console.WriteLine("[CONSOLE TEST] MixerThreholdMod v1.0.0 Starting");
                }
                catch (Exception logEx)
                {
                    // If even basic logging fails, use console directly
                    System.Console.WriteLine(string.Format("[CRITICAL] Logger failed during startup: {0}", logEx.Message));
                    throw; // This is fatal - if we can't log, we're in trouble
                }

                Instance = this;
                base.OnInitializeMelon();
                logger.Msg(1, "=== MixerThreholdMod v1.0.0 Initializing ===");
                logger.Msg(1, string.Format("currentMsgLogLevel: {0}", logger.CurrentMsgLogLevel));
                logger.Msg(1, string.Format("currentWarnLogLevel: {0}", logger.CurrentWarnLogLevel));
                logger.Msg(1, "Phase 1: Basic initialization complete");

                try
                {
                    // IL2CPP COMPATIBLE: Use IL2CPPTypeResolver for safe type resolution in both MONO and IL2CPP builds
                    // dnSpy Verified: ScheduleOne.Management.MixingStationConfiguration constructor signature verified via comprehensive dnSpy analysis
                    logger.Msg(1, "Phase 2: Initializing IL2CPP-compatible type resolution...");

                    // Log comprehensive type availability for debugging
                    Core.IL2CPPTypeResolver.LogTypeAvailability();

                    // IL2CPP-specific memory analysis after type loading
                    if (Core.IL2CPPTypeResolver.IsIL2CPPBuild)
                    {
                        Core.AdvancedSystemPerformanceMonitor.LogIL2CPPMemoryLeakAnalysis("POST_TYPE_LOADING");
                    }
                    
                    logger.Msg(1, "Phase 2: Looking up MixingStationConfiguration constructor...");
                    var constructor = Core.IL2CPPTypeResolver.GetMixingStationConfigurationConstructor();
                    if (constructor == null)
                    {
                        logger.Err("CRITICAL: Target constructor NOT found! This may be due to IL2CPP type loading issues.");
                        logger.Err("CRITICAL: Mod functionality will be limited but initialization will continue.");
                        // Don't return here - allow other initialization to continue
                    }
                    else
                    {
                        logger.Msg(1, "Phase 2: Constructor found successfully via IL2CPP-compatible type resolver");
                    }

                    logger.Msg(1, "Phase 3: Applying IL2CPP-compatible Harmony patch...");
                    // IL2CPP COMPATIBLE: Only apply Harmony patch if constructor is available
                    if (constructor != null)
                    {
                        HarmonyInstance = new HarmonyLib.Harmony("MixerThreholdMod.Main");

                        // IL2CPP COMPATIBLE: Use typeof() and compile-time safe method resolution for Harmony patching
                        HarmonyInstance.Patch(
                            constructor,
                            prefix: new HarmonyMethod(typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic))
                        );
                        logger.Msg(1, "Phase 3: IL2CPP-compatible Harmony patch applied successfully");
                    }
                    else
                    {
                        logger.Warn(1, "Phase 3: Skipping Harmony patch - constructor not available (IL2CPP type loading issue)");
                        logger.Warn(1, "Phase 3: Mod will operate in limited mode without automatic mixer detection");
                    }
                    
                    logger.Msg(1, "Phase 4: Registering IL2CPP-compatible console commands...");
                    MixerThreholdMod_1_0_0.Core.Console.RegisterConsoleCommandViaReflection();
                    logger.Msg(1, "Phase 4a: Basic console hook registered");
                    
                    // Also initialize native console integration for game console commands
                    logger.Msg(1, "Phase 4b: Initializing IL2CPP-compatible native game console integration...");
                    Core.GameConsoleBridge.InitializeNativeConsoleIntegration();
                    logger.Msg(1, "Phase 4c: IL2CPP-compatible console commands registered successfully");
                    
                    // Initialize IL2CPP-compatible patches
                    logger.Msg(1, "Phase 6: Initializing IL2CPP-compatible patches...");
                    Patches.SaveManager_Save_Patch.Initialize();
                    Patches.LoadManager_LoadedGameFolderPath_Patch.Initialize();
                    logger.Msg(1, "Phase 6: IL2CPP-compatible patches initialized");
                    
                    // Initialize game logger bridge for exception monitoring
                    logger.Msg(1, "Phase 7: Initializing game exception monitoring...");
                    Core.GameLoggerBridge.InitializeLoggingBridge();
                    logger.Msg(1, "Phase 7: Game exception monitoring initialized");
                    
                    // Initialize system hardware monitoring with memory leak detection (DEBUG mode only)
                    logger.Msg(1, "Phase 6: Initializing advanced system monitoring with memory leak detection...");
                    Core.AdvancedSystemPerformanceMonitor.Initialize();
                    logger.Msg(1, "Phase 6: Advanced system monitoring with memory leak detection initialized");

                    // Phase 7 nothing interesting here ... Just "performance optimization routines"
                    logger.Msg(1, "Phase 7: Initializing advanced performance optimization...");
                    Utils.PerformanceOptimizationManager.InitializeAdvancedOptimization();
                    logger.Msg(1, "Phase 7: Advanced performance optimization initialized (authentication required)");
                                        
                    logger.Msg(1, "=== MixerThreholdMod IL2CPP-Compatible Initialization COMPLETE ===");
                }
                catch (Exception harmonyEx)
                {
                    logger.Err(string.Format("CRITICAL: Harmony/Console setup failed: {0}\n{1}", harmonyEx.Message, harmonyEx.StackTrace));
                    throw; // Re-throw to ensure initialization failure is visible
                }
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("CRITICAL: OnInitializeMelon failed: {0}\n{1}", ex.Message, ex.StackTrace));
                // Don't re-throw here to prevent mod loader crash, but log prominently
                System.Console.WriteLine(string.Format("[CRITICAL] MixerThreholdMod initialization failed: {0}", ex.Message));
            }
        }

        //⚠️ REFLECTION REFERENCE: Called via typeof(Main).GetMethod("QueueInstance") in 
        //⚠️ OnInitializeMelon() - DO NOT DELETE
        private static void QueueInstance(object __instance)
        {
            try
            {
                if (__instance == null)
                {
                    logger.Warn(1, "QueueInstance: Received null instance - ignoring");
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

                // Process queued instances on background thread to avoid blocking main thread
                Task.Run(async () =>
                {
                    try
                    {
                        _isProcessingQueued = true;
                        await ProcessQueuedInstancesAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.Err(string.Format("OnUpdate background processing: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                    }
                    finally
                    {
                        _isProcessingQueued = false;
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("OnUpdate: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private static async Task ProcessQueuedInstancesAsync()
        {
            try
            {
                logger.Msg(3, "ProcessQueuedInstancesAsync: Starting cleanup and processing");
                
                // Clean up null instances
                await Core.MixerConfigurationTracker.RemoveAllAsync(tm => tm.ConfigInstance == null);
                
                var toProcess = queuedInstances.ToList();
                logger.Msg(3, string.Format("ProcessQueuedInstancesAsync: Processing {0} queued instances", toProcess.Count));
                
                foreach (var instance in toProcess)
                {
                    try
                    {
                        if (instance == null)
                        {
                            logger.Warn(1, "ProcessQueuedInstancesAsync: Skipping null instance");
                            continue;
                        }

                        if (await Core.MixerConfigurationTracker.AnyAsync(tm => tm.ConfigInstance == instance))
                        {
                            logger.Warn(1, string.Format("Instance already tracked — skipping duplicate: {0}", instance));
                            continue;
                        }
                        
                        // IL2CPP COMPATIBLE: Use reflection to access StartThrehold property safely
                        var startThresholdProperty = instance.GetType().GetProperty("StartThrehold");
                        if (startThresholdProperty == null)
                        {
                            logger.Warn(1, "StartThrehold property not found for instance. Skipping.");
                            continue;
                        }

                        var startThreshold = startThresholdProperty.GetValue(instance, null);
                        if (startThreshold == null)
                        {
                            logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                            continue;
                        }
                        
                        var mixerData = await Core.MixerConfigurationTracker.FirstOrDefaultAsync(tm => tm.ConfigInstance == instance);
                        if (mixerData == null)
                        {
                            try
                            {
                                logger.Msg(3, "ProcessQueuedInstancesAsync: Configuring new mixer...");
                                
                                // IL2CPP COMPATIBLE: Use reflection to call Configure method safely
                                var configureMethod = startThreshold.GetType().GetMethod("Configure", new[] { typeof(float), typeof(float), typeof(bool) });
                                if (configureMethod != null)
                                {
                                    configureMethod.Invoke(startThreshold, new object[] { 1f, 20f, true });
                                    logger.Msg(3, "ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)");
                                }
                                else
                                {
                                    logger.Warn(1, "Configure method not found on StartThrehold. Skipping configuration.");
                                    continue;
                                }

                                var newTrackedMixer = new Core.TrackedMixer
                                {
                                    ConfigInstance = instance,
                                    MixerInstanceID = Core.MixerIDManager.GetMixerID(instance)
                                };
                                await Core.MixerConfigurationTracker.AddAsync(newTrackedMixer);
                                logger.Msg(2, string.Format("Created mixer with Stable ID: {0}", newTrackedMixer.MixerInstanceID));

                                if (!newTrackedMixer.ListenerAdded)
                                {
                                    logger.Msg(3, string.Format("Attaching listener for Mixer {0}", newTrackedMixer.MixerInstanceID));
                                    Utils.CoroutineHelper.RunCoroutine(CrashResistantSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                                    newTrackedMixer.ListenerAdded = true;
                                }
                                
                                // Restore saved value if exists
                                float savedValue;
                                if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out savedValue))
                                {
                                    logger.Msg(2, string.Format("Restoring Mixer {0} to {1}", newTrackedMixer.MixerInstanceID, savedValue));
                                    
                                    // IL2CPP COMPATIBLE: Use reflection to call SetValue method safely
                                    var setValueMethod = startThreshold.GetType().GetMethod("SetValue", new[] { typeof(float), typeof(bool) });
                                    if (setValueMethod != null)
                                    {
                                        setValueMethod.Invoke(startThreshold, new object[] { savedValue, true });
                                    }
                                    else
                                    {
                                        logger.Warn(1, "SetValue method not found on StartThrehold. Cannot restore saved value.");
                                    }
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
                
                queuedInstances.Clear();
                logger.Msg(3, "ProcessQueuedInstancesAsync: Completed successfully");
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("ProcessQueuedInstancesAsync: Critical failure: {0}\n{1}", ex.Message, ex.StackTrace));
                // Ensure we don't leave the system in a bad state
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

                    logger.Msg(3, string.Format("[MAIN] Processing {0} queued mixer instances", toProcess.Count));

                    foreach (var instance in toProcess)
                    {
                        if (isShuttingDown) break;

                        // Process each instance with crash protection
                        yield return ProcessMixerInstance(instance);

                        // Yield every few operations to prevent frame drops
                        yield return null;
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
                catch (Exception listenerEx)
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
                return await Core.MixerConfigurationTracker.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
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
                    Core.MixerIDManager.MixerInstanceMap.Clear();
                    Core.MixerIDManager.ResetStableIDCounter();

                    // Clear previous mixer values
                    savedMixerValues.Clear();

                    logger.Msg(2, string.Format("[MAIN] Main scene loaded - save path: {0}", CurrentSavePath ?? "[not available yet]"));

                    // Start load coroutine once per session
                    if (!_loadCoroutineStarted)
                    {
                        StartLoadCoroutine();
                    }

                    // Copy emergency save to current save path if available
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
                                    await Helpers.ThreadSafeFileOperations.SafeWriteAllTextAsync(targetFile, await Helpers.ThreadSafeFileOperations.SafeReadAllTextAsync(sourceFile));
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

            Exception startError = null;
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
            logger.Msg(1, string.Format("[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}", iterations, delaySeconds, bypassCooldown));

            // Log initial system state
            Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("STRESS_TEST_START");
            
            var startTime = DateTime.Now;
            int successCount = 0;
            int failureCount = 0;
            
            for (int i = 1; i <= iterations; i++)
            {
                string persistentPath = MelonEnvironment.UserDataDirectory;
                if (string.IsNullOrEmpty(persistentPath) || string.IsNullOrEmpty(CurrentSavePath)) return;

                string emergencyFile = Path.Combine(persistentPath, "MixerThresholdSave_Emergency.json");
                string targetFile = Path.Combine(CurrentSavePath, "MixerThresholdSave.json");

                if (File.Exists(emergencyFile) && !File.Exists(targetFile))
                {
                    Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_START", i));
                }

                // Perform the save with comprehensive monitoring - yield return outside try/catch for .NET 4.8.1 compatibility
                yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();

                try
                {
                    var iterationTime = (DateTime.Now - iterationStart).TotalMilliseconds;
                    logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} completed in {2:F1}ms", i, iterations, iterationTime));
                    
                    // Log system state after critical operation (every 5th iteration)
                    if (i % 5 == 0 || i == iterations || iterations <= 5)
                    {
                        Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_END", i));
                    }
                    
                    successCount++;
                    iterationSuccess = true;
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format("[MONITOR] Iteration {0}/{1} FAILED: {2}", i, iterations, ex.Message));
                    Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_FAILED", i));
                    failureCount++;
                }
                
                // Add delay if specified - yield return outside try/catch for .NET 4.8.1 compatibility
                if (delaySeconds > 0f && iterationSuccess)
                {
                    logger.Msg(3, string.Format("[MONITOR] Waiting {0:F3}s before next iteration...", delaySeconds));
                    yield return new WaitForSeconds(delaySeconds);
                }
            }
            
            var totalTime = (DateTime.Now - startTime).TotalSeconds;

            // Log final system state after stress test
            Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("STRESS_TEST_COMPLETE");
            
            logger.Msg(1, string.Format("[MONITOR] Comprehensive monitoring complete: {0} success, {1} failures in {2:F1}s", successCount, failureCount, totalTime));
            logger.Msg(1, string.Format("[MONITOR] Average time per operation: {0:F1}ms", (totalTime * 1000) / iterations));
            logger.Msg(1, string.Format("[MONITOR] Success rate: {0:F1}%", (successCount * 100.0) / iterations));
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

        public static IEnumerator PerformTransactionalSave()
        {
            logger.Msg(1, "[CONSOLE] Starting atomic transactional save operation");
            logger.Msg(2, "[TRANSACTION] Performing save operation...");

            // Log system state before transaction
            Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_START");
            
            var saveStart = DateTime.Now;

            // Perform the save operation - yield return outside try/catch for .NET 4.8.1 compatibility
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();

            try
            {
                var saveTime = (DateTime.Now - saveStart).TotalMilliseconds;

                // Log system state after successful transaction
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_SUCCESS");
                
                logger.Msg(1, string.Format("[TRANSACTION] Transactional save completed successfully in {0:F1}ms", saveTime));
                logger.Msg(2, string.Format("[TRANSACTION] Performance: {0:F3} saves/second", 1000.0 / saveTime));
            }
            catch (Exception ex)
            {
                // Log system state after failed transaction
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_FAILED");
                
                logger.Err(string.Format("[TRANSACTION] Transactional save FAILED: {0}", ex.Message));
                logger.Msg(1, "[TRANSACTION] Check backup files for recovery if needed");
            }
        }

        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            logger.Msg(1, "[CONSOLE] Starting advanced save operation profiling");
            
            var profileStart = DateTime.Now;

            // Initial system state capture
            Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_START");
            
            // Phase 1: Pre-save diagnostics
            logger.Msg(2, "[PROFILE] Phase 1: Pre-save diagnostics");
            var phase1Start = DateTime.Now;
            
            double phase1Time = 0;
            double phase2Time = 0;
            double phase3Time = 0;
            Exception profileError = null;
            
            try
            {
                logger.Msg(3, string.Format("[PROFILE] Current save path: {0}", CurrentSavePath ?? "[not set]"));
                logger.Msg(3, string.Format("[PROFILE] Mixer count: {0}", savedMixerValues?.Count ?? 0));
                logger.Msg(3, string.Format("[PROFILE] Memory usage: {0} KB", GC.GetTotalMemory(false) / 1024));

                // Enhanced system diagnostics
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_PHASE1");
                
                phase1Time = (DateTime.Now - phase1Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 1 completed in {0:F1}ms", phase1Time));
            }
            catch (Exception ex)
            {
                profileError = ex;
            }
            
            // Phase 2: Save operation profiling - yield return outside try/catch for .NET 4.8.1 compatibility
            logger.Msg(2, "[PROFILE] Phase 2: Save operation profiling");
            var phase2Start = DateTime.Now;

            // System state before save operation
            Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_BEFORE_SAVE");

            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();

            try
            {
                phase2Time = (DateTime.Now - phase2Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 2 (save) completed in {0:F1}ms", phase2Time));

                // System state after save operation
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_AFTER_SAVE");
                
                // Phase 3: Post-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 3: Post-save diagnostics");
                var phase3Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Final memory usage: {0} KB", GC.GetTotalMemory(false) / 1024));
                logger.Msg(3, string.Format("[PROFILE] Mixer count after save: {0}", savedMixerValues?.Count ?? 0));

                // Final system state capture
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_PHASE3");
                
                phase3Time = (DateTime.Now - phase3Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 3 completed in {0:F1}ms", phase3Time));
                
                var totalTime = (DateTime.Now - profileStart).TotalMilliseconds;
                
                // Comprehensive performance summary
                logger.Msg(1, string.Format("[PROFILE] Advanced profiling complete. Total time: {0:F1}ms", totalTime));
                logger.Msg(1, string.Format("[PROFILE] Breakdown: PreSave={0:F1}ms, Save={1:F1}ms, PostSave={2:F1}ms", 
                    phase1Time, phase2Time, phase3Time));
                logger.Msg(1, string.Format("[PROFILE] Performance: {0:F3} saves/second", 1000.0 / phase2Time));
                logger.Msg(1, string.Format("[PROFILE] Overhead ratio: {0:F1}% (diagnostics vs save time)", 
                    ((phase1Time + phase3Time) / phase2Time) * 100));

                // Final system state
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_COMPLETE");
            }
            catch (Exception ex)
            {
                // System state on error
                Core.AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_ERROR");
                
                if (profileError != null)
                {
                    logger.Err(string.Format("[PROFILE] Advanced profiling FAILED in Phase 1: {0}", profileError.Message));
                }
                logger.Err(string.Format("[PROFILE] Advanced profiling FAILED in Phase 2/3: {0}", ex.Message));
            }
            
            // Log phase 1 error if no other errors occurred
            if (profileError != null)
            {
                logger.Err(string.Format("[PROFILE] Advanced profiling had Phase 1 error: {0}", profileError.Message));
            }
        }

        public override void OnApplicationQuit()
        {
            try
            {
                logger?.Msg(2, "[MAIN] Application shutting down - cleaning up resources");

                // Cleanup system monitoring resources
                Core.AdvancedSystemPerformanceMonitor.Cleanup();
                
                logger?.Msg(1, "[MAIN] Cleanup completed successfully");
            }
            catch (Exception ex)
            {
                logger?.Err(string.Format("[MAIN] Cleanup error: {0}", ex.Message));
                // Don't throw here - we're shutting down anyway
            }
        }
    }
} // End of namespace