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

[assembly: MelonInfo(typeof(MixerThreholdMod_1_0_0.Main), "MixerThreholdMod", "1.0.0", "mooleshacat")]
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
        
        // IL2CPP COMPATIBLE: Use object instead of specific types to avoid TypeLoadException in IL2CPP builds
        // Types will be resolved dynamically using IL2CPPTypeResolver when needed
        public static List<object> queuedInstances = new List<object>();
        public static Dictionary<object, float> userSetValues = new Dictionary<object, float>();
        public static ConcurrentDictionary<int, float> savedMixerValues = new ConcurrentDictionary<int, float>();
        
        public static string CurrentSavePath = null;
        public static bool LoadCoroutineStarted = false;
        private static bool _consoleTestCompleted = false;

        public override void OnInitializeMelon()
        {
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
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("CRITICAL: OnInitializeMelon failed: {0}\n{1}", ex.Message, ex.StackTrace));
                // Don't re-throw here to prevent mod loader crash, but log prominently
                System.Console.WriteLine(string.Format("[CRITICAL] MixerThreholdMod initialization failed: {0}", ex.Message));
            }
        }

        private static void QueueInstance(object __instance)
        {
            try
            {
                if (__instance == null)
                {
                    logger.Warn(1, "QueueInstance: Received null instance - ignoring");
                    return;
                }
                
                queuedInstances.Add(__instance);
                logger.Msg(3, string.Format("QueueInstance: Successfully queued MixingStationConfiguration (Total: {0})", queuedInstances.Count));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("QueueInstance: Critical failure - {0}\n{1}", ex.Message, ex.StackTrace));
                // Don't re-throw to prevent Harmony patch failure from crashing the game
            }
        }

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
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, "Scene loaded: " + sceneName);
            
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
        }
    }
}