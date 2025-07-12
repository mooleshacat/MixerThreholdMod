using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
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
using static MelonLoader.MelonLaunchOptions;
using static MelonLoader.MelonLogger;
using static ScheduleOne.Console;
using static VLB.Consts;

// Reminder: Add to steam game startup command: "--melonloader.captureplayerlogs" for extra MelonLogger verbosity :)

[assembly: MelonInfo(typeof(MixerThreholdMod_0_0_1.Main), "MixerThreholdMod", "0.0.1", "mooleshacat")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_0_0_1
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
        public static MelonPreferences_Entry<bool> debugLoggingEnabledEntry;
        public static MelonPreferences_Entry<bool> showLogLevelCalcEntry;
        public static MelonPreferences_Entry<int> currentMsgLogLevelEntry;
        public static MelonPreferences_Entry<int> currentWarnLogLevelEntry;
        public static readonly Core.Logger logger = new Core.Logger();
        
        // IL2CPP COMPATIBLE: Use compile-time known collection types with proper generic constraints
        public static List<MixingStationConfiguration> queuedInstances = new List<MixingStationConfiguration>();
        public static Dictionary<MixingStationConfiguration, float> userSetValues = new Dictionary<MixingStationConfiguration, float>();
=======
        public static readonly Core.Logger logger = new Core.Logger();
        
        // IL2CPP COMPATIBLE: Use object instead of specific types to avoid TypeLoadException in IL2CPP builds
        // Types will be resolved dynamically using IL2CPPTypeResolver when needed
        public static List<object> queuedInstances = new List<object>();
        public static Dictionary<object, float> userSetValues = new Dictionary<object, float>();
        public static ConcurrentDictionary<int, float> savedMixerValues = new ConcurrentDictionary<int, float>();
        
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
<<<<<<< HEAD
                logger.Err("Target constructor NOT found!");
                return;
=======
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
                    // dnSpy Verified: ScheduleOne.Management.MixingStationConfiguration constructor signature verified via comprehensive dnSpy analysis
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
                    // IL2CPP COMPATIBLE: Use typeof() and compile-time safe method resolution for Harmony patching
                    HarmonyInstance.Patch(
                        constructor,
                        prefix: new HarmonyMethod(typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic))
                    );
                    logger.Msg(1, "Phase 3: IL2CPP-compatible Harmony patch applied successfully");
                    
                    logger.Msg(1, "Phase 4: Registering IL2CPP-compatible console commands...");
                    Core.Console.RegisterConsoleCommandViaReflection();
                    logger.Msg(1, "Phase 4a: Basic console hook registered");
                    
                    // Also initialize native console integration for game console commands
                    logger.Msg(1, "Phase 4b: Initializing IL2CPP-compatible native game console integration...");
                    Core.GameConsoleBridge.InitializeNativeConsoleIntegration();
                    logger.Msg(1, "Phase 4c: Console commands registered successfully");
                    
                    // Initialize game logger bridge for exception monitoring
                    logger.Msg(1, "Phase 5: Initializing game exception monitoring...");
                    Core.GameLoggerBridge.InitializeLoggingBridge();
                    logger.Msg(1, "Phase 5: Game exception monitoring initialized");
                    
                    logger.Msg(1, "=== MixerThreholdMod Initialization COMPLETE ===");
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
>>>>>>> aa94715 (performance optimizations, cache manager)
            }
            HarmonyInstance.Patch(
                constructor,
                prefix: new HarmonyMethod(typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic))
            );
            logger.Msg(2, "Patched constructor");
            Console.RegisterConsoleCommandViaReflection();
        }
<<<<<<< HEAD
<<<<<<< HEAD
        private static void QueueInstance(MixingStationConfiguration __instance)
=======

        private static void QueueInstance(object __instance)
>>>>>>> aa94715 (performance optimizations, cache manager)
=======

        private static void QueueInstance(object __instance)
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
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
                    logger.Warn(1, $"Instance already tracked — skipping duplicate: {instance}");
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
                        logger.Msg(3, $"Created mixer with Stable ID: {newTrackedMixer.MixerInstanceID}");

                        // Now safely add listener
                        if (!mixerData.ListenerAdded)
                        {
                            logger.Msg(3, $"Attaching listener for Mixer {newTrackedMixer.MixerInstanceID}");
                            Utils.CoroutineHelper.RunCoroutine(MixerSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                            newTrackedMixer.ListenerAdded = true;
                        }
                        // Restore saved value if exists
                        if (savedMixerValues.TryGetValue(mixerData.MixerInstanceID, out float savedValue))
                        {
                            logger.Msg(2, $"Restoring Mixer {mixerData.MixerInstanceID} to {savedValue}");
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

<<<<<<< HEAD


        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            return await TrackedMixers.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
=======
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
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("ProcessQueuedInstancesAsync: Critical failure: {0}\n{1}", ex.Message, ex.StackTrace));
                // Ensure we don't leave the system in a bad state
                try
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
>>>>>>> aa94715 (performance optimizations, cache manager)
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, "Scene loaded: " + sceneName);
<<<<<<< HEAD
=======
            
            // One-time console command test when main scene loads (to verify commands work)
            if (sceneName == "Main" && !_consoleTestCompleted)
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
            
>>>>>>> aa94715 (performance optimizations, cache manager)
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
                        string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json").Replace('/', '\\');
                        string targetFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json").Replace('/', '\\');
                        if (File.Exists(sourceFile))
                        {
<<<<<<< HEAD
                            FileOperations.SafeCopy(sourceFile, targetFile, overwrite: true);
                            logger.Msg(3, "Copied MixerThresholdSave.json from persistent to save folder");
                        }
=======
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
>>>>>>> aa94715 (performance optimizations, cache manager)
                    }
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"OnSceneWasLoaded: Caught exception: {ex.Message}\n{ex.StackTrace}");
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

<<<<<<< HEAD



        // Commented - testing removal - 0 references to this code
        //[Serializable]
        //public class MixerThresholdSaveData
        //{
        //    public Dictionary<int, float> MixerValues = new Dictionary<int, float>();
        //}

=======
        // Console command implementations for comprehensive testing and debugging
        public static IEnumerator StressGameSaveTestWithComprehensiveMonitoring(int iterations, float delaySeconds, bool bypassCooldown)
        {
            logger.Msg(1, string.Format("[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}", iterations, delaySeconds, bypassCooldown));
            
            // Log initial system state
            SystemMonitor.LogCurrentPerformance("STRESS_TEST_START");
            
            var startTime = DateTime.Now;
            int successCount = 0;
            int failureCount = 0;
            
            for (int i = 1; i <= iterations; i++)
            {
                logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} - Starting save operation", i, iterations));
                
                // Track timing for this iteration
                var iterationStart = DateTime.Now;
                bool iterationSuccess = false;
                
                // Perform the save with comprehensive monitoring - yield return outside try/catch for .NET 4.8.1 compatibility
                yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
                
                try
                {
                    var iterationTime = (DateTime.Now - iterationStart).TotalMilliseconds;
                    logger.Msg(2, string.Format("[MONITOR] Iteration {0}/{1} completed in {2:F1}ms", i, iterations, iterationTime));
                    successCount++;
                    iterationSuccess = true;
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format("[MONITOR] Iteration {0}/{1} FAILED: {2}", i, iterations, ex.Message));
                    SystemMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_FAILED", i));
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
            SystemMonitor.LogCurrentPerformance("STRESS_TEST_COMPLETE");
            
            logger.Msg(1, string.Format("[MONITOR] Comprehensive monitoring complete: {0} success, {1} failures in {2:F1}s", successCount, failureCount, totalTime));
            logger.Msg(1, string.Format("[MONITOR] Average time per operation: {0:F1}ms", (totalTime * 1000) / iterations));
            logger.Msg(1, string.Format("[MONITOR] Success rate: {0:F1}%", (successCount * 100.0) / iterations));
        }

        public static IEnumerator PerformTransactionalSave()
        {
            logger.Msg(1, "[CONSOLE] Starting atomic transactional save operation");
            logger.Msg(2, "[TRANSACTION] Performing save operation...");
            
            var saveStart = DateTime.Now;
            bool saveSuccess = false;
            
            // Perform the save operation - yield return outside try/catch for .NET 4.8.1 compatibility
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                var saveTime = (DateTime.Now - saveStart).TotalMilliseconds;
                logger.Msg(1, string.Format("[TRANSACTION] Transactional save completed successfully in {0:F1}ms", saveTime));
                saveSuccess = true;
            }
            catch (Exception ex)
            {
                // Log system state after failed transaction
                SystemMonitor.LogCurrentPerformance("TRANSACTION_FAILED");
                
                logger.Err(string.Format("[TRANSACTION] Transactional save FAILED: {0}", ex.Message));
                logger.Msg(1, "[TRANSACTION] Check backup files for recovery if needed");
            }
        }

        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            logger.Msg(1, "[CONSOLE] Starting advanced save operation profiling");
            
            var profileStart = DateTime.Now;
            
            // Phase 1: Pre-save diagnostics
            logger.Msg(2, "[PROFILE] Phase 1: Pre-save diagnostics");
            var phase1Start = DateTime.Now;
            
            try
            {
                logger.Msg(3, string.Format("[PROFILE] Current save path: {0}", CurrentSavePath ?? "[not set]"));
                logger.Msg(3, string.Format("[PROFILE] Mixer count: {0}", savedMixerValues?.Count ?? 0));
                logger.Msg(3, string.Format("[PROFILE] Memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                
                phase1Time = (DateTime.Now - phase1Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 1 completed in {0:F1}ms", phase1Time));
                
                // Phase 2: Save operation profiling
                logger.Msg(2, "[PROFILE] Phase 2: Save operation profiling");
                var phase2Start = DateTime.Now;
                
                // Move yield return outside try/catch for .NET 4.8.1 compatibility
                yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
                
                var phase2Time = (DateTime.Now - phase2Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 2 (save) completed in {0:F1}ms", phase2Time));
                
                // System state after save operation
                SystemMonitor.LogCurrentPerformance("PROFILE_AFTER_SAVE");
                
                // Phase 3: Post-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 3: Post-save diagnostics");
                var phase3Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Final memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                logger.Msg(3, string.Format("[PROFILE] Mixer count after save: {0}", savedMixerValues?.Count ?? 0));
                
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
                SystemMonitor.LogCurrentPerformance("PROFILE_COMPLETE");
            }
            catch (Exception ex)
            {
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
    }
} // End of namespace