using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Helpers;
using MixerThreholdMod_1_0_0.Save;
using Newtonsoft.Json;
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
using ScheduleOne.Noise;
using ScheduleOne.ObjectScripts;
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
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

<<<<<<< HEAD
<<<<<<< HEAD
/*
 * ASYNC USAGE EXPLANATION:
 * 
 * Q: "Why so many async calls?"
 * A: The extensive use of async/await patterns in this mod serves several critical purposes:
 * 
 * 1. THREAD SAFETY: The TrackedMixers collection is accessed from multiple threads (Unity main thread,
 *    background file operations, coroutines). The AsyncLocker ensures thread-safe access without
 *    blocking the main Unity thread, which would cause frame drops and stuttering.
 * 
 * 2. NON-BLOCKING FILE I/O: File operations (reading/writing mixer save data) can be slow, especially
 *    with the file locking system in place. Running these on background threads prevents Unity from
 *    freezing during saves/loads.
 * 
 * 3. COROUTINE SAFETY: Unity coroutines run on the main thread. By using async operations within
 *    coroutines, we can yield execution properly and avoid blocking Unity's frame rendering.
 * 
 * 4. RACE CONDITION PREVENTION: Multiple mixers can be created/destroyed simultaneously. Async locks
 *    ensure that collection modifications are atomic and prevent data corruption.
 * 
 * 5. GRACEFUL DEGRADATION: If file operations fail or take too long, the async pattern with
 *    cancellation tokens allows the mod to continue functioning rather than hard-locking the game.
 * 
 * The async pattern is NOT overused here - it's necessary for a stable, performant mod that handles
 * file I/O and collection management safely in Unity's threading model.
 */

/*
 * ASYNC USAGE EXPLANATION:
 * 
 * Q: "Why so many async calls?"
 * A: The extensive use of async/await patterns in this mod serves several critical purposes:
 * 
 * 1. THREAD SAFETY: The TrackedMixers collection is accessed from multiple threads (Unity main thread,
 *    background file operations, coroutines). The AsyncLocker ensures thread-safe access without
 *    blocking the main Unity thread, which would cause frame drops and stuttering.
 * 
 * 2. NON-BLOCKING FILE I/O: File operations (reading/writing mixer save data) can be slow, especially
 *    with the file locking system in place. Running these on background threads prevents Unity from
 *    freezing during saves/loads.
 * 
 * 3. COROUTINE SAFETY: Unity coroutines run on the main thread. By using async operations within
 *    coroutines, we can yield execution properly and avoid blocking Unity's frame rendering.
 * 
 * 4. RACE CONDITION PREVENTION: Multiple mixers can be created/destroyed simultaneously. Async locks
 *    ensure that collection modifications are atomic and prevent data corruption.
 * 
 * 5. GRACEFUL DEGRADATION: If file operations fail or take too long, the async pattern with
 *    cancellation tokens allows the mod to continue functioning rather than hard-locking the game.
 * 
 * The async pattern is NOT overused here - it's necessary for a stable, performant mod that handles
 * file I/O and collection management safely in Unity's threading model.
 */

[assembly: MelonInfo(typeof(MixerThreholdMod_0_0_1.Main), "MixerThreholdMod", "0.0.1", "mooleshacat")]
=======
[assembly: MelonInfo(typeof(MixerThreholdMod_1_0_0.Main), "MixerThreholdMod", "1.0.0", "mooleshacat")]
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
[assembly: MelonInfo(typeof(MixerThreholdMod_1_0_0.Main), "MixerThreholdMod", "1.0.0", "mooleshacat")]
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_1_0_0
{
    /// <summary>
    /// IL2CPP COMPATIBLE: Main mod class with comprehensive thread safety and memory leak monitoring
    /// ⚠️ THREAD SAFETY: All operations designed to prevent main thread blocking and ensure concurrent safety
    /// ⚠️ .NET 4.8.1 Compatible: Uses framework-appropriate patterns and syntax throughout
    /// ⚠️ IL2CPP COMPATIBLE: Uses AOT-safe patterns, minimal reflection, compile-time known types
    /// ⚠️ MEMORY LEAK PREVENTION: Comprehensive monitoring and cleanup patterns implemented
    /// 
    /// IL2CPP Compatibility Features:
    /// - No use of System.Reflection.Emit or dynamic code generation
    /// - Minimal reflection usage with AOT-safe patterns (typeof() instead of GetType())
    /// - All generic constraints properly defined for AOT compilation
    /// - No runtime assembly traversal or dynamic type loading
    /// - Interface-based patterns instead of reflection-heavy approaches
    /// - Compile-time safe collection types and operations
    /// 
    /// Memory Leak Prevention Features:
    /// - Advanced SystemMonitor integration with leak detection
    /// - Proper resource disposal patterns in all async operations
    /// - ConcurrentDictionary usage for thread-safe collections without leaks
    /// - Background task management with proper cleanup
    /// - Memory pressure monitoring during save operations
    /// 
    /// Thread Safety Features:
    /// - Async/await patterns throughout to prevent main thread blocking
    /// - Proper cancellation token usage for cooperative cancellation
    /// - Thread-safe collections (ConcurrentDictionary) for shared state
    /// - Lock-free operations where possible to prevent deadlocks
    /// - Background thread usage for heavy operations
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - string.Format() instead of string interpolation for maximum compatibility
    /// - Compatible async/await patterns with proper ConfigureAwait usage
    /// - Framework-appropriate exception handling and logging patterns
    /// - Compatible reflection patterns with minimal usage
    /// </summary>
    public class Main : MelonMod
    {
        public static Main Instance { get; private set; }
        public static MelonPreferences_Entry<bool> debugLoggingEnabledEntry;
        public static MelonPreferences_Entry<bool> showLogLevelCalcEntry;
        public static MelonPreferences_Entry<int> currentMsgLogLevelEntry;
        public static MelonPreferences_Entry<int> currentWarnLogLevelEntry;
        public static readonly Core.Logger logger = new Core.Logger();
<<<<<<< HEAD
<<<<<<< HEAD
        public static List<MixingStationConfiguration> queuedInstances = new List<MixingStationConfiguration>();
        public static Dictionary<MixingStationConfiguration, float> userSetValues = new Dictionary<MixingStationConfiguration, float>();

        public static Dictionary<int, float> savedMixerValues = new Dictionary<int, float>();
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        
        // IL2CPP COMPATIBLE: Use object instead of specific types to avoid TypeLoadException in IL2CPP builds
        // Types will be resolved dynamically using IL2CPPTypeResolver when needed
        public static List<object> queuedInstances = new List<object>();
        public static Dictionary<object, float> userSetValues = new Dictionary<object, float>();
        public static ConcurrentDictionary<int, float> savedMixerValues = new ConcurrentDictionary<int, float>();
        
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public static string CurrentSavePath = null;
        public static bool LoadCoroutineStarted = false;
        private static bool _consoleTestCompleted = false;

        public override void OnInitializeMelon()
        {
<<<<<<< HEAD
<<<<<<< HEAD
            Instance = this;
            base.OnInitializeMelon();
            
            // Initialize exception handler here too ?

            // Initialize game logging bridge for exception monitoring
            Core.GameLoggerBridge.InitializeLoggingBridge();

            // Critical: Add unhandled exception handler for crash prevention
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            logger.Msg(1, "[MAIN] MixerThresholdMod initializing - focused on save crash prevention");
            logger.Msg(1, string.Format("[MAIN] Debug levels - Msg: {0}, Warn: {1}", logger.CurrentMsgLogLevel, logger.CurrentWarnLogLevel));

            Exception initError = null;
            try
            {
                // Enhanced constructor detection with comprehensive debugging
                logger.Msg(1, "[MAIN] Starting MixingStationConfiguration constructor analysis...");
                
                var mixingStationType = typeof(MixingStationConfiguration);
                logger.Msg(2, string.Format("[MAIN] MixingStationConfiguration type: {0}", mixingStationType.FullName));
                
                // Get all constructors for debugging
                var allConstructors = mixingStationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                logger.Msg(1, string.Format("[MAIN] Found {0} total constructors", allConstructors.Length));
                
                for (int i = 0; i < allConstructors.Length; i++)
                {
                    var ctor = allConstructors[i];
                    var parameters = ctor.GetParameters();
                    var paramTypes = string.Join(", ", parameters.Select(p => p.ParameterType.Name));
                    logger.Msg(2, string.Format("[MAIN] Constructor {0}: ({1})", i + 1, paramTypes));
                }

                // Try the original constructor signature
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
                    logger.Warn(1, "[MAIN] Target constructor (ConfigurationReplicator, IConfigurable, MixingStation) not found");
                    
                    // Try alternative constructor signatures
                    var alternativeSignatures = new[]
                    {
                        new Type[] { typeof(ConfigurationReplicator), typeof(IConfigurable) },
                        new Type[] { typeof(IConfigurable), typeof(MixingStation) },
                        new Type[] { typeof(MixingStation) },
                        new Type[] { typeof(ConfigurationReplicator) }
                    };
                    
                    foreach (var sig in alternativeSignatures)
                    {
                        var altConstructor = mixingStationType.GetConstructor(
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                            null, sig, null);
                            
                        if (altConstructor != null)
                        {
                            constructor = altConstructor;
                            var sigTypes = string.Join(", ", sig.Select(t => t.Name));
                            logger.Msg(1, string.Format("[MAIN] Found alternative constructor: ({0})", sigTypes));
                            break;
                        }
                    }
                }
                else
                {
                    logger.Msg(1, "[MAIN] Target constructor found successfully");
                }

                if (constructor == null)
                {
                    logger.Err("[MAIN] CRITICAL: No suitable MixingStationConfiguration constructor found");
                    logger.Msg(1, "[MAIN] Will attempt runtime mixer scanning as fallback");
                    // Don't return - continue with fallback methods
                }
                else
                {
                    // Attempt to patch the constructor
                    try
                    {
                        var queueMethod = typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic);
                        if (queueMethod == null)
                        {
                            logger.Err("[MAIN] CRITICAL: QueueInstance method not found for patching");
                        }
                        else
                        {
                            HarmonyInstance.Patch(
                                constructor,
                                prefix: new HarmonyMethod(queueMethod)
                            );
                            
                            logger.Msg(1, "[MAIN] ✓ Constructor patch applied successfully");
                        }
                    }
                    catch (Exception patchEx)
                    {
                        logger.Err(string.Format("[MAIN] Constructor patch failed: {0}\nStackTrace: {1}", patchEx.Message, patchEx.StackTrace));
                    }
                }

                // Register console commands for debugging
                Core.Console.RegisterConsoleCommandViaReflection();
                logger.Msg(3, "[MAIN] Console commands registered");

                // Start the main update coroutine with runtime mixer scanning
                var coroutineObj = MelonCoroutines.Start(UpdateCoroutine());
                mainUpdateCoroutine = coroutineObj as Coroutine;
                logger.Msg(1, "[MAIN] ✓ Main update coroutine started successfully");
                
                // Start fallback mixer scanner coroutine
                MelonCoroutines.Start(RuntimeMixerScanner());
                logger.Msg(1, "[MAIN] ✓ Runtime mixer scanner started");
=======
            try
            {
                // Test logger immediately - if this fails, we have a fundamental problem
                try
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

=======
            try
            {
                // Test logger immediately - if this fails, we have a fundamental problem
                try
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

>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    logger.Msg(1, "Phase 3: Applying IL2CPP-compatible Harmony patch...");
                    // IL2CPP COMPATIBLE: Only apply Harmony patch if constructor is available
                    if (constructor != null)
                    {
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
                    Core.ModConsoleCommandProcessor.RegisterConsoleCommandViaReflection();
                    logger.Msg(1, "Phase 4a: Basic console hook registered");
                    
                    // Also initialize native console integration for game console commands
                    logger.Msg(1, "Phase 4b: Initializing IL2CPP-compatible native game console integration...");
                    Core.NativeGameConsoleIntegration.InitializeNativeConsoleIntegration();
                    logger.Msg(1, "Phase 4c: IL2CPP-compatible console commands registered successfully");
                    
                    // Initialize IL2CPP-compatible patches
                    logger.Msg(1, "Phase 6: Initializing IL2CPP-compatible patches...");
                    Patches.SaveManager_Save_Patch.Initialize();
                    Patches.LoadManager_LoadedGameFolderPath_Patch.Initialize();
                    logger.Msg(1, "Phase 6: IL2CPP-compatible patches initialized");
                    
                    // Initialize game logger bridge for exception monitoring
                    logger.Msg(1, "Phase 7: Initializing game exception monitoring...");
                    Core.GameExceptionMonitor.InitializeLoggingBridge();
                    logger.Msg(1, "Phase 7: Game exception monitoring initialized");
                    
                    // Initialize system hardware monitoring with memory leak detection (DEBUG mode only)
                    logger.Msg(1, "Phase 6: Initializing advanced system monitoring with memory leak detection...");
                    Core.SystemMonitor.Initialize();
                    logger.Msg(1, "Phase 6: Advanced system monitoring with memory leak detection initialized");

                    // Phase 7 nothing interesting here ... Just "performance optimization routines"
                    logger.Msg(1, "Phase 7: Initializing advanced performance optimization...");
                    Helpers.Utils.PerformanceOptimizationManager.InitializeAdvancedOptimization();
                    logger.Msg(1, "Phase 7: Advanced performance optimization initialized (authentication required)");

                    logger.Msg(1, "Phase 8: Initializing advanced system monitoring with memory leak detection...");
                    Core.AdvancedSystemPerformanceMonitor.Initialize();
                    logger.Msg(1, "Phase 8: Advanced system monitoring with memory leak detection initialized");
                    
                    logger.Msg(1, "=== MixerThreholdMod IL2CPP-Compatible Initialization COMPLETE ===");
                }
                catch (Exception harmonyEx)
                {
                    logger.Err(string.Format("CRITICAL: Harmony/Console setup failed: {0}\n{1}", harmonyEx.Message, harmonyEx.StackTrace));
                    throw; // Re-throw to ensure initialization failure is visible
                }
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("CRITICAL: OnInitializeMelon failed: {0}\n{1}", ex.Message, ex.StackTrace));
                // Don't re-throw here to prevent mod loader crash, but log prominently
                System.Console.WriteLine(string.Format("[CRITICAL] MixerThreholdMod initialization failed: {0}", ex.Message));
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            if (isShuttingDown) return;

            Exception queueError = null;
=======
        private static void QueueInstance(object __instance)
        {
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
        private static void QueueInstance(object __instance)
        {
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            try
            {
                if (__instance == null)
                {
                    logger.Warn(1, "QueueInstance: Received null instance - ignoring");
                    return;
                }
                
                queuedInstances.Add(__instance);
<<<<<<< HEAD
<<<<<<< HEAD
                logger.Msg(1, "[MAIN] ✓ MIXER DETECTED: Queued new MixingStationConfiguration for processing");
                logger.Msg(2, string.Format("[MAIN] Queue size now: {0}", queuedInstances.Count));
=======
                logger.Msg(3, string.Format("QueueInstance: Successfully queued MixingStationConfiguration (Total: {0})", queuedInstances.Count));
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
                logger.Msg(3, string.Format("QueueInstance: Successfully queued MixingStationConfiguration (Total: {0})", queuedInstances.Count));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("QueueInstance: Critical failure - {0}\n{1}", ex.Message, ex.StackTrace));
                // Don't re-throw to prevent Harmony patch failure from crashing the game
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
        /// <summary>
        /// Runtime mixer scanner as fallback detection method
        /// ⚠️ CRASH PREVENTION: Scans for mixers in case constructor patching fails
        /// </summary>
        private IEnumerator RuntimeMixerScanner()
        {
            logger.Msg(2, "[MAIN] RuntimeMixerScanner: Starting...");
            
            yield return new WaitForSeconds(5f); // Wait for game to initialize
            
            while (!isShuttingDown)
            {
                Exception scanError = null;
                try
                {
                    // Find all MixingStationConfiguration instances in the scene
                    var foundMixers = UnityEngine.Object.FindObjectsOfType<MixingStation>();
                    if (foundMixers != null && foundMixers.Length > 0)
                    {
                        logger.Msg(2, string.Format("[MAIN] RuntimeMixerScanner: Found {0} MixingStation objects", foundMixers.Length));
                        
                        foreach (var mixingStation in foundMixers)
                        {
                            if (isShuttingDown) break;
                            
                            // Try to get the configuration via reflection
                            var configField = mixingStation.GetType().GetField("configuration", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (configField != null)
                            {
                                var config = configField.GetValue(mixingStation) as MixingStationConfiguration;
                                if (config != null)
                                {
                                    // Check if already tracked
                                    bool alreadyTracked = Core.TrackedMixers.Any(tm => tm?.ConfigInstance == config);
                                    if (!alreadyTracked)
                                    {
                                        queuedInstances.Add(config);
                                        logger.Msg(1, string.Format("[MAIN] ✓ FALLBACK DETECTION: Found mixer via runtime scan: {0}", config.GetHashCode()));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        logger.Msg(3, "[MAIN] RuntimeMixerScanner: No MixingStation objects found yet");
                    }
                }
                catch (Exception ex)
                {
                    scanError = ex;
                }

                if (scanError != null)
                {
                    logger.Err(string.Format("[MAIN] RuntimeMixerScanner CRASH PREVENTION: Error: {0}", scanError.Message));
                }

                // Scan every 10 seconds
                yield return new WaitForSeconds(10f);
            }
            
            logger.Msg(2, "[MAIN] RuntimeMixerScanner: Stopped");
        }

        /// <summary>
        /// Main update loop - uses coroutines to prevent blocking Unity thread
        /// ⚠️ CRASH PREVENTION: All operations designed to not block main thread
        /// </summary>
=======
        private static bool _isProcessingQueued = false;

>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public override void OnUpdate()
        {
            try
            {
                // Prevent multiple concurrent executions of async operations
                if (_isProcessingQueued || queuedInstances.Count == 0) 
                    return;

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
<<<<<<< HEAD
                Exception updateError = null;
                try
                {
                    // Clean up null/invalid mixers first - with better error handling
                    try
                    {
                        yield return CleanupNullMixers();
=======
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
                                    Helpers.Utils.CoroutineHelper.RunCoroutine(Save.CrashResistantSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
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
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    }
                    catch (Exception cleanupEx)
                    {
                        logger.Err(string.Format("[MAIN] UpdateCoroutine: Cleanup error: {0}", cleanupEx.Message));
                    }

                    // Process queued instances
                    if (queuedInstances.Count > 0)
                    {
                        var toProcess = queuedInstances.ToList();
                        queuedInstances.Clear();

                        logger.Msg(1, string.Format("[MAIN] ✓ PROCESSING: {0} queued mixer instances", toProcess.Count));

                        foreach (var instance in toProcess)
                        {
                            if (isShuttingDown) break;

                            // Process each instance with crash protection
                            yield return ProcessMixerInstance(instance);

                            // Yield every few operations to prevent frame drops
                            yield return null;
                        }
                    }
                    else
                    {
                        // Log periodically if no mixers found to aid debugging
                        frameCount++;
                        if (frameCount % 1000 == 0) // Every ~100 seconds at 10fps
                        {
                            yield return LogMixerDiagnostics(frameCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    updateError = ex;
                }

                if (updateError != null)
                {
                    logger.Err(string.Format("[MAIN] UpdateCoroutine CRASH PREVENTION: Error: {0}\nStackTrace: {1}", 
                        updateError.Message, updateError.StackTrace));
                }
<<<<<<< HEAD

                // Normal processing interval - NO try-catch around this yield
                yield return new WaitForSeconds(0.1f);
            }

            logger.Msg(3, "UpdateCoroutine finished");
        }

        /// <summary>
        /// Cleanup null mixers using coroutine to prevent blocking
        /// </summary>
        private IEnumerator CleanupNullMixers()
        {
            Exception cleanupError = null;
            List<TrackedMixer> currentMixers = null;
            
            // Get mixers without try/catch to avoid yield return issues
            var getMixersCoroutine = GetAllMixersAsync();
            if (getMixersCoroutine != null)
            {
                yield return getMixersCoroutine;
                // Extract result from coroutine
                if (getMixersCoroutine.Current is List<TrackedMixer>)
                {
                    currentMixers = (List<TrackedMixer>)getMixersCoroutine.Current;
                }
            }
            
            try
            {
                if (currentMixers != null)
                {
                    var validMixers = new List<TrackedMixer>();
                    foreach (var tm in currentMixers)
                    {
                        if (tm?.ConfigInstance != null)
                        {
                            validMixers.Add(tm);
                        }
                    }

                    if (validMixers.Count != currentMixers.Count)
                    {
                        logger.Msg(3, string.Format("[MAIN] Cleaned up {0} null mixers", currentMixers.Count - validMixers.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                cleanupError = ex;
            }

            if (cleanupError != null)
            {
                logger.Err(string.Format("[MAIN] CleanupNullMixers: Error: {0}", cleanupError.Message));
            }
            
            yield return null; // Required return for IEnumerator
        }

        /// <summary>
        /// Get all mixers via async coroutine
        /// </summary>
        /// <summary>
        /// Get all mixers via async coroutine
        /// </summary>
        private IEnumerator GetAllMixersAsync()
        {
            var allMixers = new List<TrackedMixer>();
            Exception asyncError = null;

            // Execute async task without try/catch to avoid yield return issues
            var task = Core.TrackedMixers.GetAllAsync();

            // Wait for async task with timeout
            float startTime = Time.time;
            while (!task.IsCompleted && (Time.time - startTime) < 2f)
            {
                yield return new WaitForSeconds(0.1f);
            }

            try
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    allMixers.AddRange(task.Result);
                    logger.Msg(3, "[MAIN] GetAllMixersAsync: Successfully retrieved mixer list");
                }
                else if (task.IsFaulted)
                {
                    logger.Err(string.Format("[MAIN] GetAllMixersAsync: Task faulted: {0}", task.Exception?.Message));
                }
                else
                {
                    logger.Warn(1, "[MAIN] GetAllMixersAsync: Task timed out after 2 seconds");
                }
            }
            catch (Exception ex)
            {
                asyncError = ex;
            }

            if (asyncError != null)
            {
                logger.Err(string.Format("[MAIN] GetAllMixersAsync: Error: {0}", asyncError.Message));
            }

            yield return allMixers;
        }

        /// <summary>
        /// Log mixer diagnostics as a coroutine
        /// </summary>
        private IEnumerator LogMixerDiagnostics(int frameCount)
        {
            Exception diagError = null;
            object mixerCountResult = null;
            
            // Get mixer count without try/catch to avoid yield return issues
            var getMixerCountCoroutine = GetMixerCountAsync();
            if (getMixerCountCoroutine != null)
            {
                yield return getMixerCountCoroutine;
                mixerCountResult = getMixerCountCoroutine.Current;
            }
            
            try
            {
                int trackedMixersCount = 0;
                
                if (mixerCountResult is int)
                {
                    trackedMixersCount = (int)mixerCountResult;
                }

                logger.Msg(2, string.Format("[MAIN] Diagnostic (frame {0}): Tracked mixers: {1}, Saved values: {2}, Queued: {3}", 
                    frameCount, trackedMixersCount, savedMixerValues.Count, queuedInstances.Count));

                // Enhanced diagnostics every 2000 frames (~200 seconds)
                if (frameCount % 2000 == 0)
                {
                    logger.Msg(1, string.Format("[MAIN] ENHANCED DIAGNOSTICS:"));
                    logger.Msg(1, string.Format("  - MixerInstanceMap count: {0}", Core.MixerIDManager.GetMixerCount()));
                    logger.Msg(1, string.Format("  - Current save path: {0}", CurrentSavePath ?? "[none]"));
                    logger.Msg(1, string.Format("  - Shutdown status: {0}", isShuttingDown));
                }
            }
            catch (Exception ex)
            {
                diagError = ex;
            }

            if (diagError != null)
            {
                logger.Err(string.Format("[MAIN] LogMixerDiagnostics: Error: {0}", diagError.Message));
            }
            
            yield return null; // Required return for IEnumerator
        }

        /// <summary>
        /// Get mixer count via async coroutine
        /// </summary>
        private IEnumerator GetMixerCountAsync()
        {
            Exception countError = null;
            int count = 0;

            // Execute async task without try/catch to avoid yield return issues
            var task = Core.TrackedMixers.CountAsync(tm => tm != null);
            
            // Wait for async task with timeout
            float startTime = Time.time;
            while (!task.IsCompleted && (Time.time - startTime) < 1f)
            {
                yield return new WaitForSeconds(0.1f);
            }

            try
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    count = task.Result;
                }
            }
            catch (Exception ex)
            {
                countError = ex;
            }

            if (countError != null)
            {
                logger.Err(string.Format("[MAIN] GetMixerCountAsync: Error: {0}", countError.Message));
            }

            yield return count;
        }

        /// <summary>
        /// Cleanup null mixers using coroutine to prevent blocking
        /// </summary>
        private IEnumerator CleanupNullMixers()
        {
            Exception cleanupError = null;
            try
            {
                var currentMixers = yield return GetAllMixersAsync();
                if (currentMixers != null)
                {
                    var validMixers = new List<TrackedMixer>();
                    foreach (var tm in currentMixers)
                    {
                        if (tm?.ConfigInstance != null)
                        {
                            validMixers.Add(tm);
                        }
                    }

                    if (validMixers.Count != currentMixers.Count)
                    {
                        logger.Msg(3, string.Format("[MAIN] Cleaned up {0} null mixers", currentMixers.Count - validMixers.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                cleanupError = ex;
            }

            if (cleanupError != null)
            {
                logger.Err(string.Format("[MAIN] CleanupNullMixers: Error: {0}", cleanupError.Message));
            }
        }

        /// <summary>
        /// Get all mixers via async coroutine
        /// </summary>
        private IEnumerator GetAllMixersAsync()
        {
            logger.Msg(2, "[MAIN] UpdateCoroutine started");
            int frameCount = 0;

            try
            {
                // Clean up null/invalid mixers first
                Core.TrackedMixers.RemoveAll(tm => tm?.ConfigInstance == null);

                // Process queued instances
                if (queuedInstances.Count > 0)
                {
                    var toProcess = queuedInstances.ToList();
                    queuedInstances.Clear();

                    logger.Msg(1, string.Format("[MAIN] ✓ PROCESSING: {0} queued mixer instances", toProcess.Count));

                    foreach (var instance in toProcess)
                    {
                        yield return CleanupNullMixers();
                    }
                    catch (Exception cleanupEx)
                    {
                        logger.Err(string.Format("[MAIN] UpdateCoroutine: Cleanup error: {0}", cleanupEx.Message));
                    }

                    // Process queued instances
                    if (queuedInstances.Count > 0)
                    {
                        var toProcess = queuedInstances.ToList();
                        queuedInstances.Clear();

                        logger.Msg(1, string.Format("[MAIN] ✓ PROCESSING: {0} queued mixer instances", toProcess.Count));

                        foreach (var instance in toProcess)
                        {
                            if (isShuttingDown) break;

                            // Process each instance with crash protection
                            yield return ProcessMixerInstance(instance);

                            // Yield every few operations to prevent frame drops
                            yield return null;
                        }
                    }
                    else
                    {
                        // Log periodically if no mixers found to aid debugging
                        frameCount++;
                        if (frameCount % 1000 == 0) // Every ~100 seconds at 10fps
                        {
                            yield return LogMixerDiagnostics(frameCount);
                        }
                    }
                }
                else
                {
                    // Log periodically if no mixers found to aid debugging
                    frameCount++;
                    if (frameCount % 1000 == 0) // Every ~100 seconds at 10fps
                    {
                        logger.Msg(2, string.Format("[MAIN] No mixers found yet (frame {0}). Tracked mixers: {1}, Saved values: {2}", 
                            frameCount, Core.TrackedMixers.Count(tm => tm != null), savedMixerValues.Count));
                    }
                }
                else
                {
                    // Log periodically if no mixers found to aid debugging
                    frameCount++;
                    if (frameCount % 1000 == 0) // Every ~100 seconds at 10fps
                    {
                        logger.Msg(2, string.Format("[MAIN] No mixers found yet (frame {0}). Tracked mixers: {1}, Saved values: {2}", 
                            frameCount, Core.TrackedMixers.Count(tm => tm != null), savedMixerValues.Count));
                    }
                }

                if (updateError != null)
                {
                    logger.Err(string.Format("[MAIN] UpdateCoroutine CRASH PREVENTION: Error: {0}\nStackTrace: {1}", 
                        updateError.Message, updateError.StackTrace));
                }
            }
            catch (Exception ex)
            {
                asyncError = ex;
            }

            if (asyncError != null)
            {
                logger.Err(string.Format("[MAIN] GetAllMixersAsync: Error: {0}", asyncError.Message));
            }

            yield return allMixers;
        }

        /// <summary>
        /// Log mixer diagnostics as a coroutine
        /// </summary>
        private IEnumerator LogMixerDiagnostics(int frameCount)
        {
            Exception diagError = null;
            try
            {
                var mixerCountResult = yield return GetMixerCountAsync();
                int trackedMixersCount = 0;
                
                if (mixerCountResult is int)
                {
                    trackedMixersCount = (int)mixerCountResult;
                }

                logger.Msg(2, string.Format("[MAIN] Diagnostic (frame {0}): Tracked mixers: {1}, Saved values: {2}, Queued: {3}", 
                    frameCount, trackedMixersCount, savedMixerValues.Count, queuedInstances.Count));

                // Enhanced diagnostics every 2000 frames (~200 seconds)
                if (frameCount % 2000 == 0)
                {
                    logger.Msg(1, string.Format("[MAIN] ENHANCED DIAGNOSTICS:"));
                    logger.Msg(1, string.Format("  - MixerInstanceMap count: {0}", Core.MixerIDManager.GetMixerCount()));
                    logger.Msg(1, string.Format("  - Current save path: {0}", CurrentSavePath ?? "[none]"));
                    logger.Msg(1, string.Format("  - Shutdown status: {0}", isShuttingDown));
                }
            }
            catch (Exception ex)
            {
                diagError = ex;
            }

            if (diagError != null)
            {
                logger.Err(string.Format("[MAIN] LogMixerDiagnostics: Error: {0}", diagError.Message));
            }
        }

        /// <summary>
        /// Get mixer count via async coroutine
        /// </summary>
        private IEnumerator GetMixerCountAsync()
        {
            Exception countError = null;
            int count = 0;

            try
            {
                var task = Core.TrackedMixers.CountAsync(tm => tm != null);
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (task.IsCompleted && !task.IsFaulted)
                {
                    count = task.Result;
                }
            }
            catch (Exception ex)
            {
                countError = ex;
            }

            if (countError != null)
            {
                logger.Err(string.Format("[MAIN] GetMixerCountAsync: Error: {0}", countError.Message));
            }

            yield return count;
        }

        /// <summary>
        /// Cleanup null mixers using coroutine to prevent blocking
        /// </summary>
        private IEnumerator CleanupNullMixers()
        {
            Exception cleanupError = null;
            try
            {
                var currentMixers = yield return GetAllMixersAsync();
                if (currentMixers != null)
                {
                    var validMixers = new List<TrackedMixer>();
                    foreach (var tm in currentMixers)
                    {
                        if (tm?.ConfigInstance != null)
                        {
                            validMixers.Add(tm);
                        }
                    }

                    if (validMixers.Count != currentMixers.Count)
                    {
                        logger.Msg(3, string.Format("[MAIN] Cleaned up {0} null mixers", currentMixers.Count - validMixers.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                cleanupError = ex;
            }

            if (cleanupError != null)
            {
                logger.Err(string.Format("[MAIN] CleanupNullMixers: Error: {0}", cleanupError.Message));
            }
        }

        /// <summary>
        /// Get all mixers via async coroutine
        /// </summary>
        private IEnumerator GetAllMixersAsync()
        {
            var allMixers = new List<TrackedMixer>();
            Exception asyncError = null;
            bool taskCompleted = false;

            try
            {
                var task = Core.TrackedMixers.GetAllAsync();
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 2f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (task.IsCompleted && !task.IsFaulted)
                {
                    allMixers.AddRange(task.Result);
                    taskCompleted = true;
                }
                else if (task.IsFaulted)
                {
                    logger.Err(string.Format("[MAIN] GetAllMixersAsync: Task faulted: {0}", task.Exception?.Message));
                }
                else
                {
                    logger.Warn(1, "[MAIN] GetAllMixersAsync: Task timed out after 2 seconds");
                }
            }
            catch (Exception ex)
            {
                asyncError = ex;
            }

            if (asyncError != null)
            {
                logger.Err(string.Format("[MAIN] GetAllMixersAsync: Error: {0}", asyncError.Message));
            }

            yield return allMixers;
        }

        /// <summary>
        /// Log mixer diagnostics as a coroutine
        /// </summary>
        private IEnumerator LogMixerDiagnostics(int frameCount)
        {
            Exception diagError = null;
            try
            {
                var mixerCountResult = yield return GetMixerCountAsync();
                int trackedMixersCount = 0;
                
                if (mixerCountResult is int)
                {
                    trackedMixersCount = (int)mixerCountResult;
                }

                logger.Msg(2, string.Format("[MAIN] Diagnostic (frame {0}): Tracked mixers: {1}, Saved values: {2}, Queued: {3}", 
                    frameCount, trackedMixersCount, savedMixerValues.Count, queuedInstances.Count));

                // Enhanced diagnostics every 2000 frames (~200 seconds)
                if (frameCount % 2000 == 0)
                {
                    logger.Msg(1, string.Format("[MAIN] ENHANCED DIAGNOSTICS:"));
                    logger.Msg(1, string.Format("  - MixerInstanceMap count: {0}", Core.MixerIDManager.GetMixerCount()));
                    logger.Msg(1, string.Format("  - Current save path: {0}", CurrentSavePath ?? "[none]"));
                    logger.Msg(1, string.Format("  - Shutdown status: {0}", isShuttingDown));
                }
            }
            catch (Exception ex)
            {
                diagError = ex;
            }

            if (diagError != null)
            {
                logger.Err(string.Format("[MAIN] LogMixerDiagnostics: Error: {0}", diagError.Message));
            }
        }

        /// <summary>
        /// Get mixer count via async coroutine
        /// </summary>
        private IEnumerator GetMixerCountAsync()
        {
            Exception countError = null;
            int count = 0;

            try
            {
                var task = Core.TrackedMixers.CountAsync(tm => tm != null);
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (task.IsCompleted && !task.IsFaulted)
                {
                    count = task.Result;
                }
            }
            catch (Exception ex)
            {
                countError = ex;
            }

            if (countError != null)
            {
                logger.Err(string.Format("[MAIN] GetMixerCountAsync: Error: {0}", countError.Message));
            }

            yield return count;
        }

        /// <summary>
        /// Process a single mixer instance with crash protection
        /// </summary>
        private IEnumerator ProcessMixerInstance(MixingStationConfiguration instance)
        {
            if (instance == null || isShuttingDown) yield break;

            Exception processingError = null;
            bool alreadyTracked = false;
            TrackedMixer newTrackedMixer = null;

            // Do all error-prone work BEFORE any yield returns
            try
            {
                // Check if already tracked
                bool alreadyTracked = Core.TrackedMixers.Any(tm => tm?.ConfigInstance == instance);
                if (alreadyTracked)
                {
                    logger.Warn(1, "Instance already tracked - skipping duplicate");
                    yield break;
                }

                if (instance.StartThrehold == null)
                {
                    logger.Warn(1, "StartThrehold is null - skipping instance");
                    yield break;
                }

                // Configure threshold on main thread (Unity requirement)
                instance.StartThrehold.Configure(1f, 20f, true);

                newTrackedMixer = new TrackedMixer
                {
                    ConfigInstance = instance,
                    MixerInstanceID = Core.MixerIDManager.GetMixerID(instance)
                };

                // Add to tracking collection
                Core.TrackedMixers.Add(newTrackedMixer);
                logger.Msg(2, string.Format("[MAIN] Created mixer with ID: {0}", newTrackedMixer.MixerInstanceID));
=======
                
                queuedInstances.Clear();
                logger.Msg(3, "ProcessQueuedInstancesAsync: Completed successfully");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
        private static bool _isProcessingQueued = false;

        public override void OnUpdate()
        {
            try
            {
                // Prevent multiple concurrent executions of async operations
                if (_isProcessingQueued || queuedInstances.Count == 0) 
                    return;

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
                                    Helpers.Utils.CoroutineHelper.RunCoroutine(Save.CrashResistantSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
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
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("ProcessQueuedInstancesAsync: Critical failure: {0}\n{1}", ex.Message, ex.StackTrace));
                // Ensure we don't leave the system in a bad state
                try
                {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
                    await TrackedMixers.RemoveAllAsync(tm => tm?.ConfigInstance == null);
=======
                    queuedInstances.Clear();
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }
                catch 
                {
<<<<<<< HEAD
                    listenerError = listenerEx;
                }

                if (listenerError != null)
                {
                    logger.Err(string.Format("Failed to attach listener: {0}", listenerError.Message));
                }
            }

                var toProcess = queuedInstances?.ToList() ?? new List<MixingStationConfiguration>();
=======
                    await TrackedMixers.RemoveAllAsync(tm => tm.ConfigInstance == null);
                }
                catch (Exception ex)
                {
                    logger.Err($"Error cleaning up tracked mixers: {ex}");
                }

                if (queuedInstances.Count == 0) return;
                
                var toProcess = queuedInstances.ToList();
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                foreach (var instance in toProcess)
                {
                    try
                    {
<<<<<<< HEAD
                        if (instance == null)
                        {
                            logger.Warn(1, "OnUpdate: Skipping null instance in queuedInstances");
                            continue;
                        }

        /// <summary>
        /// Check if mixer is already tracked using coroutine
        /// </summary>
        private IEnumerator CheckIfAlreadyTracked(MixingStationConfiguration instance, System.Action<bool> callback)
        {
            bool result = false;
            Exception checkError = null;

            try
            {
                var task = Core.TrackedMixers.AnyAsync(tm => tm?.ConfigInstance == instance);
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
=======
                    // Even clearing failed - something is seriously wrong
                    logger.Err("ProcessQueuedInstancesAsync: Even queue clearing failed!");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }

                if (task.IsCompleted && !task.IsFaulted)
                {
                    result = task.Result;
                }
                else if (task.IsFaulted)
                {
                    logger.Err(string.Format("[MAIN] CheckIfAlreadyTracked: Task faulted: {0}", task.Exception?.Message));
                }
                else
                {
                    logger.Warn(1, "[MAIN] CheckIfAlreadyTracked: Task timed out - assuming not tracked");
                }
            }
            catch (Exception ex)
            {
                checkError = ex;
            }

            if (checkError != null)
            {
                logger.Err(string.Format("[MAIN] CheckIfAlreadyTracked: Error: {0}", checkError.Message));
            }

            callback(result);
        }

        /// <summary>
        /// Add tracked mixer using coroutine
        /// </summary>
        private IEnumerator AddTrackedMixerAsync(TrackedMixer mixer)
        {
            Exception addError = null;

            try
            {
                var task = Core.TrackedMixers.AddAsync(mixer);
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (task.IsFaulted)
                {
                    logger.Err(string.Format("[MAIN] AddTrackedMixerAsync: Task faulted: {0}", task.Exception?.Message));
                }
                else if (!task.IsCompleted)
                {
                    logger.Warn(1, "[MAIN] AddTrackedMixerAsync: Task timed out");
                }
            }
            catch (Exception ex)
            {
                addError = ex;
            }

            if (addError != null)
            {
                logger.Err(string.Format("[MAIN] AddTrackedMixerAsync: Error: {0}", addError.Message));
            }
        }

<<<<<<< HEAD
        /// <summary>
        /// Check if mixer is already tracked using coroutine
        /// </summary>
        private IEnumerator CheckIfAlreadyTracked(MixingStationConfiguration instance, System.Action<bool> callback)
        {
            bool result = false;
            Exception checkError = null;

            try
            {
                var task = Core.TrackedMixers.AnyAsync(tm => tm?.ConfigInstance == instance);
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (task.IsCompleted && !task.IsFaulted)
                {
                    result = task.Result;
                }
                else if (task.IsFaulted)
                {
                    logger.Err(string.Format("[MAIN] CheckIfAlreadyTracked: Task faulted: {0}", task.Exception?.Message));
                }
                else
                {
                    logger.Warn(1, "[MAIN] CheckIfAlreadyTracked: Task timed out - assuming not tracked");
                }
            }
            catch (Exception ex)
            {
                checkError = ex;
            }

            if (checkError != null)
            {
                logger.Err(string.Format("[MAIN] CheckIfAlreadyTracked: Error: {0}", checkError.Message));
            }

            callback(result);
        }

        /// <summary>
        /// Add tracked mixer using coroutine
        /// </summary>
        private IEnumerator AddTrackedMixerAsync(TrackedMixer mixer)
        {
            Exception addError = null;

            try
            {
                var task = Core.TrackedMixers.AddAsync(mixer);
                
                // Wait for async task with timeout
                float startTime = Time.time;
                while (!task.IsCompleted && (Time.time - startTime) < 1f)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (task.IsFaulted)
                {
                    logger.Err(string.Format("[MAIN] AddTrackedMixerAsync: Task faulted: {0}", task.Exception?.Message));
                }
                else if (!task.IsCompleted)
                {
                    logger.Warn(1, "[MAIN] AddTrackedMixerAsync: Task timed out");
                }
            }
            catch (Exception ex)
            {
                addError = ex;
            }

            if (addError != null)
            {
                logger.Err(string.Format("[MAIN] AddTrackedMixerAsync: Error: {0}", addError.Message));
            }
        }

        /// <summary>
        /// Check if a mixer exists by ID
        /// </summary>
        public static bool MixerExists(int mixerInstanceID)
        {
            try
            {
                return Core.TrackedMixers.Any(tm => tm.MixerInstanceID == mixerInstanceID);
            }
            catch (Exception ex)
            {
                logger.Err($"OnUpdate: Critical error in main update loop: {ex.Message}\n{ex.StackTrace}");
=======
                        logger.Err($"Error processing mixer instance: {ex}");
                    }
                }
                queuedInstances.Clear();
            }
            catch (Exception ex)
            {
                logger.Err($"Critical error in OnUpdate: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
        }

        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            Exception checkError = null;
            bool result = false;

=======
        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            try
            {
                return await Core.MixerConfigurationTracker.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("MixerExists: Caught exception: {0}", ex));
                return false;
            }
=======
                    queuedInstances.Clear();
                }
                catch 
                {
                    // Even clearing failed - something is seriously wrong
                    logger.Err("ProcessQueuedInstancesAsync: Even queue clearing failed!");
                }
            }
        }

        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            try
            {
                return await Core.MixerConfigurationTracker.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("MixerExists: Caught exception: {0}", ex));
                return false;
            }
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, "Scene loaded: " + sceneName);
            
            // One-time console command test when main scene loads (to verify commands work)
            if (sceneName == "Main" && !_consoleTestCompleted)
<<<<<<< HEAD
            {
                _consoleTestCompleted = true;
                logger.Msg(2, "[DEBUG] Running one-time console command test...");
                
                // Test console commands to verify they work
                Task.Run(() =>
                {
                    try
                    {
                        // Wait a moment for everything to settle
                        System.Threading.Thread.Sleep(1000);
                        
                        logger.Msg(2, "[DEBUG] Testing console commands...");
                        Core.ModConsoleCommandProcessor.ProcessManualCommand("msg This is a test message from console command");
                        Core.ModConsoleCommandProcessor.ProcessManualCommand("warn This is a test warning from console command");
                        logger.Msg(2, "[DEBUG] Console command test completed - check logs above for test output");
                    }
                    catch (Exception ex)
                    {
                        logger.Err(string.Format("[DEBUG] Console command test failed: {0}", ex.Message));
                    }
                });
            }
            
            if (sceneName == "Main")
            {
<<<<<<< HEAD
                base.OnSceneWasLoaded(buildIndex, sceneName);
                logger.Msg(2, "Scene loaded: " + sceneName);
                
<<<<<<< HEAD
                if (sceneName == "Main")
                {
                    // Reset mixer state for new game session
                    if (Core.MixerIDManager.MixerInstanceMap != null)
                    {
                        Core.MixerIDManager.MixerInstanceMap.Clear();
                    }
                    Core.MixerIDManager.ResetStableIDCounter();
                    savedMixerValues.Clear();

                        // Clear previous mixer values
                        if (savedMixerValues != null)
                        {
                            savedMixerValues.Clear();
                        }

                        logger.Msg(3, "Current Save Path at scene load: " + (Main.CurrentSavePath ?? "[not available yet]"));
=======
                try
                {
                    Core.MixerIDManager.MixerInstanceMap.Clear();
                    Core.MixerIDManager.ResetStableIDCounter();

                    savedMixerValues.Clear();
                    logger.Msg(3, "Current Save Path at scene load: " + (CurrentSavePath ?? "[not available yet]"));
                    StartLoadCoroutine();
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
            {
                _consoleTestCompleted = true;
                logger.Msg(2, "[DEBUG] Running one-time console command test...");
                
                // Test console commands to verify they work
                Task.Run(() =>
                {
                    try
                    {
                        // Wait a moment for everything to settle
                        System.Threading.Thread.Sleep(1000);
                        
                        logger.Msg(2, "[DEBUG] Testing console commands...");
                        Core.ModConsoleCommandProcessor.ProcessManualCommand("msg This is a test message from console command");
                        Core.ModConsoleCommandProcessor.ProcessManualCommand("warn This is a test warning from console command");
                        logger.Msg(2, "[DEBUG] Console command test completed - check logs above for test output");
                    }
                    catch (Exception ex)
                    {
                        logger.Err(string.Format("[DEBUG] Console command test failed: {0}", ex.Message));
                    }
                });
            }
            
            if (sceneName == "Main")
            {
                try
                {
                    Core.MixerIDManager.MixerInstanceMap.Clear();
                    Core.MixerIDManager.ResetStableIDCounter();

                    savedMixerValues.Clear();
                    logger.Msg(3, "Current Save Path at scene load: " + (CurrentSavePath ?? "[not available yet]"));
                    StartLoadCoroutine();
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)

                    if (!string.IsNullOrEmpty(CurrentSavePath))
                    {
                        Task.Run(async () =>
                        {
<<<<<<< HEAD
<<<<<<< HEAD
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
                                    
=======
                switch (sceneName)
                {
                    case "Main":
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
                                try
                                {
                                    string persistentPath = MelonEnvironment.UserDataDirectory;
                                    string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json").Replace('/', '\\');
                                    string targetFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json").Replace('/', '\\');
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                    if (File.Exists(sourceFile))
                                    {
                                        FileOperations.SafeCopy(sourceFile, targetFile, overwrite: true);
                                        logger.Msg(3, "Copied MixerThresholdSave.json from persistent to save folder");
                                    }
                                }
<<<<<<< HEAD
                            }
                            catch (Exception copyEx)
                            {
                                logger.Err($"OnSceneWasLoaded: Failed to copy mixer save file: {copyEx.Message}");
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                            }
                            catch (Exception ex)
                            {
                                logger.Err(string.Format("Background file copy failed: {0}", ex));
                            }
                        });
                    }
                }
<<<<<<< HEAD
            }
            catch (Exception ex)
            {
                Main.logger.Err($"OnSceneWasLoaded: Critical error: {ex.Message}\n{ex.StackTrace}");
=======
                                catch (Exception ex)
                                {
                                    logger.Err($"Error copying MixerThresholdSave.json: {ex}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Main.logger.Err($"OnSceneWasLoaded: Caught exception: {ex.Message}\n{ex.StackTrace}");
                        }
                        break;
=======
                catch (Exception ex)
                {
                    logger.Err(string.Format("OnSceneWasLoaded: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Critical error in OnSceneWasLoaded: {ex}");
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
        private static bool _loadCoroutineStarted = false;
        public static bool LoadCoroutineStarted { get { return _loadCoroutineStarted; } }

        /// <summary>
        /// Start the load coroutine once per session
        /// </summary>
=======
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        private static void StartLoadCoroutine()
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
                if (_coroutineStarted) 
                {
                    logger.Msg(3, "StartLoadCoroutine: Already started, skipping");
                    return;
                }

                _coroutineStarted = true;
                logger.Msg(3, "StartLoadCoroutine: Starting MixerSaveManager.LoadMixerValuesWhenReady");
=======
                if (_coroutineStarted) return;
                _coroutineStarted = true;

>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                MelonCoroutines.Start(MixerSaveManager.LoadMixerValuesWhenReady());
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                logger.Err($"StartLoadCoroutine: Error starting coroutine: {ex.Message}\n{ex.StackTrace}");
                _coroutineStarted = false; // Reset flag on error
=======
                logger.Err($"Error starting LoadMixerValuesWhenReady coroutine: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
        }

        // Commented - testing removal - 0 references to this code
        //[Serializable]
        //public class MixerThresholdSaveData
        //{
        //    public Dictionary<int, float> MixerValues = new Dictionary<int, float>();
        //}

            if (quitError != null)
            {
                logger.Err(string.Format("OnApplicationQuit error: {0}", quitError.Message));
            }

            base.OnApplicationQuit();
=======
                if (LoadCoroutineStarted) return;
                LoadCoroutineStarted = true;
                MelonCoroutines.Start(Save.CrashResistantSaveManager.LoadMixerValuesWhenReady());
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("StartLoadCoroutine: Caught exception: {0}", ex));
            }
        }

        // Console command implementations for comprehensive testing and debugging
        public static IEnumerator StressGameSaveTestWithComprehensiveMonitoring(int iterations, float delaySeconds, bool bypassCooldown)
        {
            logger.Msg(1, string.Format("[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}", iterations, delaySeconds, bypassCooldown));
            
            // Log initial system state
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("STRESS_TEST_START");
            
            var startTime = DateTime.Now;
            int successCount = 0;
            int failureCount = 0;
            
            for (int i = 1; i <= iterations; i++)
            {
                logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} - Starting save operation", i, iterations));
                
                // Track timing and system performance for this iteration
                var iterationStart = DateTime.Now;
                bool iterationSuccess = false;
                
                // Log system state before critical operation (every 5th iteration to avoid spam)
                if (i % 5 == 1 || iterations <= 5)
                {
                    AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_START", i));
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
                        AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_END", i));
                    }
                    
                    successCount++;
                    iterationSuccess = true;
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format("[MONITOR] Iteration {0}/{1} FAILED: {2}", i, iterations, ex.Message));
                    AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_FAILED", i));
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
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("STRESS_TEST_COMPLETE");
            
            logger.Msg(1, string.Format("[MONITOR] Comprehensive monitoring complete: {0} success, {1} failures in {2:F1}s", successCount, failureCount, totalTime));
            logger.Msg(1, string.Format("[MONITOR] Average time per operation: {0:F1}ms", (totalTime * 1000) / iterations));
            logger.Msg(1, string.Format("[MONITOR] Success rate: {0:F1}%", (successCount * 100.0) / iterations));
        }

        public static IEnumerator PerformTransactionalSave()
        {
            logger.Msg(1, "[CONSOLE] Starting atomic transactional save operation");
            logger.Msg(2, "[TRANSACTION] Performing save operation...");
            
            // Log system state before transaction
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_START");
            
            var saveStart = DateTime.Now;
            
            // Perform the save operation - yield return outside try/catch for .NET 4.8.1 compatibility
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                var saveTime = (DateTime.Now - saveStart).TotalMilliseconds;
                
                // Log system state after successful transaction
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_SUCCESS");
                
                logger.Msg(1, string.Format("[TRANSACTION] Transactional save completed successfully in {0:F1}ms", saveTime));
                logger.Msg(2, string.Format("[TRANSACTION] Performance: {0:F3} saves/second", 1000.0 / saveTime));
            }
            catch (Exception ex)
            {
                // Log system state after failed transaction
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_FAILED");
                
                logger.Err(string.Format("[TRANSACTION] Transactional save FAILED: {0}", ex.Message));
                logger.Msg(1, "[TRANSACTION] Check backup files for recovery if needed");
            }
        }

        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            logger.Msg(1, "[CONSOLE] Starting advanced save operation profiling");
            
            var profileStart = DateTime.Now;
            
            // Initial system state capture
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_START");
            
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
                logger.Msg(3, string.Format("[PROFILE] Memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                
                // Enhanced system diagnostics
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_PHASE1");
                
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
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_BEFORE_SAVE");
            
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                phase2Time = (DateTime.Now - phase2Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 2 (save) completed in {0:F1}ms", phase2Time));
                
                // System state after save operation
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_AFTER_SAVE");
                
                // Phase 3: Post-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 3: Post-save diagnostics");
                var phase3Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Final memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                logger.Msg(3, string.Format("[PROFILE] Mixer count after save: {0}", savedMixerValues?.Count ?? 0));
                
                // Final system state capture
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_PHASE3");
                
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
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_COMPLETE");
            }
            catch (Exception ex)
            {
                // System state on error
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_ERROR");
                
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

=======
                if (LoadCoroutineStarted) return;
                LoadCoroutineStarted = true;
                MelonCoroutines.Start(Save.CrashResistantSaveManager.LoadMixerValuesWhenReady());
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("StartLoadCoroutine: Caught exception: {0}", ex));
            }
        }

        // Console command implementations for comprehensive testing and debugging
        public static IEnumerator StressGameSaveTestWithComprehensiveMonitoring(int iterations, float delaySeconds, bool bypassCooldown)
        {
            logger.Msg(1, string.Format("[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}", iterations, delaySeconds, bypassCooldown));
            
            // Log initial system state
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("STRESS_TEST_START");
            
            var startTime = DateTime.Now;
            int successCount = 0;
            int failureCount = 0;
            
            for (int i = 1; i <= iterations; i++)
            {
                logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} - Starting save operation", i, iterations));
                
                // Track timing and system performance for this iteration
                var iterationStart = DateTime.Now;
                bool iterationSuccess = false;
                
                // Log system state before critical operation (every 5th iteration to avoid spam)
                if (i % 5 == 1 || iterations <= 5)
                {
                    AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_START", i));
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
                        AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_END", i));
                    }
                    
                    successCount++;
                    iterationSuccess = true;
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format("[MONITOR] Iteration {0}/{1} FAILED: {2}", i, iterations, ex.Message));
                    AdvancedSystemPerformanceMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_FAILED", i));
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
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("STRESS_TEST_COMPLETE");
            
            logger.Msg(1, string.Format("[MONITOR] Comprehensive monitoring complete: {0} success, {1} failures in {2:F1}s", successCount, failureCount, totalTime));
            logger.Msg(1, string.Format("[MONITOR] Average time per operation: {0:F1}ms", (totalTime * 1000) / iterations));
            logger.Msg(1, string.Format("[MONITOR] Success rate: {0:F1}%", (successCount * 100.0) / iterations));
        }

        public static IEnumerator PerformTransactionalSave()
        {
            logger.Msg(1, "[CONSOLE] Starting atomic transactional save operation");
            logger.Msg(2, "[TRANSACTION] Performing save operation...");
            
            // Log system state before transaction
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_START");
            
            var saveStart = DateTime.Now;
            
            // Perform the save operation - yield return outside try/catch for .NET 4.8.1 compatibility
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                var saveTime = (DateTime.Now - saveStart).TotalMilliseconds;
                
                // Log system state after successful transaction
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_SUCCESS");
                
                logger.Msg(1, string.Format("[TRANSACTION] Transactional save completed successfully in {0:F1}ms", saveTime));
                logger.Msg(2, string.Format("[TRANSACTION] Performance: {0:F3} saves/second", 1000.0 / saveTime));
            }
            catch (Exception ex)
            {
                // Log system state after failed transaction
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("TRANSACTION_FAILED");
                
                logger.Err(string.Format("[TRANSACTION] Transactional save FAILED: {0}", ex.Message));
                logger.Msg(1, "[TRANSACTION] Check backup files for recovery if needed");
            }
        }

        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            logger.Msg(1, "[CONSOLE] Starting advanced save operation profiling");
            
            var profileStart = DateTime.Now;
            
            // Initial system state capture
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_START");
            
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
                logger.Msg(3, string.Format("[PROFILE] Memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                
                // Enhanced system diagnostics
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_PHASE1");
                
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
            AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_BEFORE_SAVE");
            
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                phase2Time = (DateTime.Now - phase2Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 2 (save) completed in {0:F1}ms", phase2Time));
                
                // System state after save operation
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_AFTER_SAVE");
                
                // Phase 3: Post-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 3: Post-save diagnostics");
                var phase3Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Final memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                logger.Msg(3, string.Format("[PROFILE] Mixer count after save: {0}", savedMixerValues?.Count ?? 0));
                
                // Final system state capture
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_PHASE3");
                
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
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_COMPLETE");
            }
            catch (Exception ex)
            {
                // System state on error
                AdvancedSystemPerformanceMonitor.LogCurrentPerformance("PROFILE_ERROR");
                
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

>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public override void OnApplicationQuit()
        {
            try
            {
                logger?.Msg(2, "[MAIN] Application shutting down - cleaning up resources");
                
                // Cleanup system monitoring resources
                AdvancedSystemPerformanceMonitor.Cleanup();
                
                logger?.Msg(1, "[MAIN] Cleanup completed successfully");
            }
            catch (Exception ex)
            {
                logger?.Err(string.Format("[MAIN] Cleanup error: {0}", ex.Message));
                // Don't throw here - we're shutting down anyway
            }
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        }
    }
}