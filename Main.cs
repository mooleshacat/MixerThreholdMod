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
        public static Main Instance { get; private set; }
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
                    // dnSpy Verified: ScheduleOne.Management.MixingStationConfiguration constructor signature verified via comprehensive dnSpy analysis
                    logger.Msg(1, "Phase 2: Looking up MixingStationConfiguration constructor...");
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
                        logger.Err("CRITICAL: Target constructor NOT found! Mod will not function.");
                        return;
                    }
                    logger.Msg(1, "Phase 2: Constructor found successfully");

                    logger.Msg(1, "Phase 3: Applying Harmony patch...");
                    HarmonyInstance.Patch(
                        constructor,
                        prefix: new HarmonyMethod(typeof(Main).GetMethod("QueueInstance", BindingFlags.Static | BindingFlags.NonPublic))
                    );
                    logger.Msg(1, "Phase 3: Harmony patch applied successfully");
                    
                    logger.Msg(1, "Phase 4: Registering console commands...");
                    Core.Console.RegisterConsoleCommandViaReflection();
                    logger.Msg(1, "Phase 4a: Basic console hook registered");
                    
                    // Also initialize native console integration for game console commands
                    logger.Msg(1, "Phase 4b: Initializing native game console integration...");
                    Core.GameConsoleBridge.InitializeNativeConsoleIntegration();
                    logger.Msg(1, "Phase 4c: Console commands registered successfully");
                    
                    // Initialize game logger bridge for exception monitoring
                    logger.Msg(1, "Phase 5: Initializing game exception monitoring...");
                    Core.GameLoggerBridge.InitializeLoggingBridge();
                    logger.Msg(1, "Phase 5: Game exception monitoring initialized");
                    
                    // Initialize system hardware monitoring for debugging (DEBUG mode only)
                    logger.Msg(1, "Phase 6: Initializing system monitoring...");
                    Core.SystemMonitor.Initialize();
                    logger.Msg(1, "Phase 6: System monitoring initialized");
                    
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
            }
        }

        private static void QueueInstance(MixingStationConfiguration __instance)
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
                await Core.TrackedMixers.RemoveAllAsync(tm => tm.ConfigInstance == null);
                
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
                                logger.Msg(3, "ProcessQueuedInstancesAsync: Configuring new mixer...");
                                instance.StartThrehold.Configure(1f, 20f, true);
                                logger.Msg(3, "ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)");

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
                        Core.Console.ProcessManualCommand("msg This is a test message from console command");
                        Core.Console.ProcessManualCommand("warn This is a test warning from console command");
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
                
                // Track timing and system performance for this iteration
                var iterationStart = DateTime.Now;
                bool iterationSuccess = false;
                
                // Log system state before critical operation (every 5th iteration to avoid spam)
                if (i % 5 == 1 || iterations <= 5)
                {
                    SystemMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_START", i));
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
                        SystemMonitor.LogCurrentPerformance(string.Format("ITERATION_{0}_END", i));
                    }
                    
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
            
            // Log system state before transaction
            SystemMonitor.LogCurrentPerformance("TRANSACTION_START");
            
            var saveStart = DateTime.Now;
            
            // Perform the save operation - yield return outside try/catch for .NET 4.8.1 compatibility
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                var saveTime = (DateTime.Now - saveStart).TotalMilliseconds;
                
                // Log system state after successful transaction
                SystemMonitor.LogCurrentPerformance("TRANSACTION_SUCCESS");
                
                logger.Msg(1, string.Format("[TRANSACTION] Transactional save completed successfully in {0:F1}ms", saveTime));
                logger.Msg(2, string.Format("[TRANSACTION] Performance: {0:F3} saves/second", 1000.0 / saveTime));
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
            
            // Initial system state capture
            SystemMonitor.LogCurrentPerformance("PROFILE_START");
            
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
                SystemMonitor.LogCurrentPerformance("PROFILE_PHASE1");
                
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
            SystemMonitor.LogCurrentPerformance("PROFILE_BEFORE_SAVE");
            
            yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
            
            try
            {
                phase2Time = (DateTime.Now - phase2Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 2 (save) completed in {0:F1}ms", phase2Time));
                
                // System state after save operation
                SystemMonitor.LogCurrentPerformance("PROFILE_AFTER_SAVE");
                
                // Phase 3: Post-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 3: Post-save diagnostics");
                var phase3Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Final memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                logger.Msg(3, string.Format("[PROFILE] Mixer count after save: {0}", savedMixerValues?.Count ?? 0));
                
                // Final system state capture
                SystemMonitor.LogCurrentPerformance("PROFILE_PHASE3");
                
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
                // System state on error
                SystemMonitor.LogCurrentPerformance("PROFILE_ERROR");
                
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
                SystemMonitor.Cleanup();
                
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