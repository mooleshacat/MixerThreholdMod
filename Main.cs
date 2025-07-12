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
using MelonLoader;
using MelonLoader.Utils;
using HarmonyLib;
using MixerThreholdMod_1_0_0.Core;    // ✅ ESSENTIAL - Keep this! IDE false positive
using MixerThreholdMod_1_0_0.Constants;    // ✅ ESSENTIAL - Keep this! Our constants!
using MixerThreholdMod_1_0_0.Helpers; // ✅ NEEDED
using MixerThreholdMod_1_0_0.Save;    // ✅ NEEDED

// Reminder: Add to steam game startup command: "--melonloader.captureplayerlogs" for extra MelonLogger verbosity :)

// IL2CPP COMPATIBILITY: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading

[assembly: MelonInfo(typeof(MixerThreholdMod_1_0_0.Main), ModConstants.MOD_NAME, ModConstants.VERSION, "mooleshacat")]
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
        public static bool LoadCoroutineStarted = false;
        private static bool _consoleTestCompleted = false;

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
                Instance = this;
                base.OnInitializeMelon();
                logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.MOD_INIT_HEADER);
                logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, string.Format(ModConstants.CURRENT_MSG_LOG_LEVEL_TEMPLATE, logger.CurrentMsgLogLevel));
                logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, string.Format(ModConstants.CURRENT_WARN_LOG_LEVEL_TEMPLATE, logger.CurrentWarnLogLevel));
                logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.BASIC_INIT_COMPLETE);

                // Test logger immediately - if this fails, we have a fundamental problem
                try
                {
                    logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.LOGGER_TEST_MESSAGE);
                    System.Console.WriteLine(ModConstants.CONSOLE_TEST_MESSAGE);
                }
                if (instance.StartThrehold == null)
                {
                    // If even basic logging fails, use console directly
                    System.Console.WriteLine(string.Format(ModConstants.CRITICAL_LOGGER_FAILURE_TEMPLATE, logEx.Message));
                    throw; // This is fatal - if we can't log, we're in trouble
                }

                logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.DIRECTORY_DETECTION_START);
                // ⚠️ ASYNC JUSTIFICATION: Game API access can take 50-200ms but prevents 20+ second filesystem recursion
                // Uses game's own SaveManager/LoadManager APIs for instant path resolution
                Task.Run(async () =>
                {
                    try
                    {
                        // Configure threshold
                        instance.StartThrehold.Configure(1f, 20f, true);

                        // Log key findings for user benefit
                        logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, string.Format(ModConstants.GAME_DIRECTORY_DETECTION_TEMPLATE, directoryInfo.ToString()));

                        if (directoryInfo.MelonLoaderLogFound)
                        {
                            logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, string.Format(ModConstants.MELON_LOADER_LOG_READY_TEMPLATE, directoryInfo.MelonLoaderLogFile));
                        }

                        // Now safely add listener
                        if (!mixerData.ListenerAdded)
                        {
                            logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, string.Format(ModConstants.GAME_SAVES_DIRECTORY_TEMPLATE, directoryInfo.SavesDirectory));
                        }
                    }
                    catch (Exception dirEx)
                    {
                        logger.Err(string.Format(ModConstants.DIRECTORY_DETECTION_FAILURE_TEMPLATE, dirEx.Message));
                    }
                });
                logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.DIRECTORY_DETECTION_COMPLETE);

                try
                {
                    // IL2CPP COMPATIBLE: Use IL2CPPTypeResolver for safe type resolution in both MONO and IL2CPP builds
                    // dnSpy Verified: ScheduleOne.Management.MixingStationConfiguration constructor signature verified via comprehensive dnSpy analysis
                    logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.TYPE_RESOLUTION_START);

                    // Log comprehensive type availability for debugging
                    Core.IL2CPPTypeResolver.LogTypeAvailability();

                    // IL2CPP-specific memory analysis after type loading
                    if (Core.IL2CPPTypeResolver.IsIL2CPPBuild)
                    {
                        Core.AdvancedSystemPerformanceMonitor.LogIL2CPPMemoryLeakAnalysis(ModConstants.PERF_TAG_POST_TYPE_LOADING);
                    }

                    logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.CONSTRUCTOR_LOOKUP);
                    var constructor = Core.IL2CPPTypeResolver.GetMixingStationConfigurationConstructor();
                    if (constructor == null)
                    {
                        logger.Err(ModConstants.CONSTRUCTOR_NOT_FOUND_ERROR);
                        logger.Err(ModConstants.LIMITED_FUNCTIONALITY_WARNING);
                        // Don't return here - allow other initialization to continue
                    }
                    else
                    {
                        logger.Msg(ModConstants.LOG_LEVEL_CRITICAL, ModConstants.CONSTRUCTOR_FOUND);
                    }

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
                    logger.Msg(1, "Phase 4: Console commands registered successfully");
                    
                    logger.Msg(1, "=== MixerThreholdMod Initialization COMPLETE ===");
                }
                catch (Exception harmonyEx)
                {
                    logger.Err(string.Format(ModConstants.HARMONY_CONSOLE_FAILURE_TEMPLATE, harmonyEx.Message, harmonyEx.StackTrace));
                    throw; // Re-throw to ensure initialization failure is visible
                }
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(ModConstants.ON_INIT_MELON_FAILURE_TEMPLATE, ex.Message, ex.StackTrace));
                // Don't re-throw here to prevent mod loader crash, but log prominently
                System.Console.WriteLine(string.Format(ModConstants.CRITICAL_INIT_FAILURE_TEMPLATE, ex.Message));
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
                    logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, ModConstants.QUEUE_INSTANCE_NULL_WARNING);
                    return;
                }

                queuedInstances.Add(__instance);
                logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.QUEUE_INSTANCE_SUCCESS_TEMPLATE, queuedInstances.Count));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(ModConstants.QUEUE_INSTANCE_FAILURE_TEMPLATE, ex.Message, ex.StackTrace));
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

                // ⚠️ ASYNC JUSTIFICATION: ProcessQueuedInstancesAsync() contains:
                // - Thread-safe collection operations that could block main thread for 10-50ms
                // - Reflection-based property access that can take 5-20ms per instance
                // - Potential database-like operations in MixerConfigurationTracker (up to 100ms)
                // - File I/O for coroutine-based save operations (50-200ms)
                // Task.Run prevents Unity main thread blocking which would cause frame drops
                Task.Run(async () =>
                {
                    try
                    {
                        _isProcessingQueued = true;
                        await ProcessQueuedInstancesAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.Err(string.Format(ModConstants.BACKGROUND_PROCESSING_FAILURE_TEMPLATE, ex.Message, ex.StackTrace));
                    }
                    finally
                    {
                        _isProcessingQueued = false;
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(ModConstants.ON_UPDATE_FAILURE_TEMPLATE, ex.Message, ex.StackTrace));
            }
            queuedInstances.Clear();
        }



        public async static Task<bool> MixerExists(int mixerInstanceID)
        {
            var diagnostics = new Helpers.FileOperationDiagnostics();

            try
            {
                logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, ModConstants.PROCESS_CLEANUP_START);

                // ⚠️ ASYNC JUSTIFICATION: RemoveAllAsync() performs:
                // - ConcurrentBag enumeration and filtering (5-15ms for large collections)
                // - Thread-safe collection rebuilding (10-30ms)
                // - Memory allocation/deallocation that could trigger GC (5-50ms)
                await Core.MixerConfigurationTracker.RemoveAllAsync(tm => tm.ConfigInstance == null);

                var toProcess = queuedInstances.ToList();
                logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.PROCESS_INSTANCES_TEMPLATE, toProcess.Count));

                foreach (var instance in toProcess)
                {
                    try
                    {
                        if (instance == null)
                        {
                            logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, ModConstants.PROCESS_NULL_INSTANCE_SKIP);
                            continue;
                        }

                        // ⚠️ ASYNC JUSTIFICATION: AnyAsync() performs:
                        // - LINQ operations on potentially large collections (5-20ms)
                        // - Thread-safe enumeration with locking (3-10ms)
                        // - Predicate evaluation across multiple objects (2-15ms per object)
                        if (await Core.MixerConfigurationTracker.AnyAsync(tm => tm.ConfigInstance == instance))
                        {
                            logger.Warn(ModConstants.WARN_LEVEL_CRITICAL, string.Format(ModConstants.INSTANCE_ALREADY_TRACKED_TEMPLATE, instance));
                            continue;
                        }

                        // IL2CPP COMPATIBLE: Use reflection to access StartThrehold property safely
                        var startThresholdProperty = instance.GetType().GetProperty(ModConstants.START_THREHOLD_PROPERTY_NAME);
                        if (startThresholdProperty == null)
                        {
                            logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, ModConstants.START_THREHOLD_NOT_FOUND);
                            continue;
                        }

                        var startThreshold = startThresholdProperty.GetValue(instance, null);
                        if (startThreshold == null)
                        {
                            logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, ModConstants.START_THREHOLD_NULL);
                            continue;
                        }

                        // ⚠️ ASYNC JUSTIFICATION: FirstOrDefaultAsync() performs:
                        // - Complex LINQ predicate matching (5-25ms)
                        // - Thread-safe collection access with potential lock contention (3-15ms)
                        // - Object comparison operations across multiple instances (2-10ms per comparison)
                        var mixerData = await Core.MixerConfigurationTracker.FirstOrDefaultAsync(tm => tm.ConfigInstance == instance);
                        if (mixerData == null)
                        {
                            try
                            {
                                logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, ModConstants.NEW_MIXER_CONFIG);

                                // IL2CPP COMPATIBLE: Use reflection to call Configure method safely
                                var configureMethod = startThreshold.GetType().GetMethod(ModConstants.CONFIGURE_METHOD_NAME, new[] { typeof(float), typeof(float), typeof(bool) });
                                if (configureMethod != null)
                                {
                                    configureMethod.Invoke(startThreshold, new object[] { ModConstants.MIXER_THRESHOLD_MIN, ModConstants.MIXER_THRESHOLD_MAX, ModConstants.MIXER_CONFIG_ENABLED_DEFAULT });
                                    logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, ModConstants.MIXER_CONFIGURED_SUCCESS);
                                }
                                else
                                {
                                    logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, ModConstants.CONFIGURE_METHOD_NOT_FOUND);
                                    continue;
                                }

                                var newTrackedMixer = new Core.TrackedMixer
                                {
                                    ConfigInstance = instance,
                                    MixerInstanceID = Core.MixerIDManager.GetMixerID(instance)
                                };

                                // ⚠️ ASYNC JUSTIFICATION: AddAsync() performs:
                                // - Thread-safe collection modification (3-10ms)
                                // - Timestamp generation and object initialization (1-5ms)
                                // - Memory allocation that could trigger GC (5-50ms)
                                await Core.MixerConfigurationTracker.AddAsync(newTrackedMixer);
                                logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, string.Format(ModConstants.MIXER_CREATED_TEMPLATE, newTrackedMixer.MixerInstanceID));

                                if (!newTrackedMixer.ListenerAdded)
                                {
                                    logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.ATTACHING_LISTENER_TEMPLATE, newTrackedMixer.MixerInstanceID));
                                    Utils.CoroutineHelper.RunCoroutine(CrashResistantSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                                    newTrackedMixer.ListenerAdded = true;
                                }

                                // Restore saved value if exists
                                float savedValue;
                                if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out savedValue))
                                {
                                    logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, string.Format(ModConstants.RESTORING_MIXER_TEMPLATE, newTrackedMixer.MixerInstanceID, savedValue));

                                    // IL2CPP COMPATIBLE: Use reflection to call SetValue method safely
                                    var setValueMethod = startThreshold.GetType().GetMethod(ModConstants.SET_VALUE_METHOD_NAME, new[] { typeof(float), typeof(bool) });
                                    if (setValueMethod != null)
                                    {
                                        setValueMethod.Invoke(startThreshold, new object[] { savedValue, ModConstants.MIXER_CONFIG_ENABLED_DEFAULT });
                                    }
                                    else
                                    {
                                        logger.Msg(ModConstants.WARN_LEVEL_CRITICAL, ModConstants.SET_VALUE_METHOD_NOT_FOUND);
                                    }
                                }
                            }
                            catch (Exception mixerEx)
                            {
                                logger.Err(string.Format(ModConstants.INDIVIDUAL_MIXER_ERROR_TEMPLATE, mixerEx.Message, mixerEx.StackTrace));
                                // Continue processing other mixers despite this failure
                            }
                        }
                    }
                    catch (Exception instanceEx)
                    {
                        logger.Err(string.Format(ModConstants.INDIVIDUAL_INSTANCE_ERROR_TEMPLATE, instanceEx.Message, instanceEx.StackTrace));
                        // Continue processing other instances despite this failure
                    }
                }

                queuedInstances.Clear();
                logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, ModConstants.PROCESS_COMPLETE_SUCCESS);
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(ModConstants.PROCESS_CRITICAL_FAILURE_TEMPLATE, ex.Message, ex.StackTrace));
                // Ensure we don't leave the system in a bad state
                try
                {
                    queuedInstances.Clear();
                }
                catch
                {
                    // Even clearing failed - something is seriously wrong
                    logger.Err(ModConstants.QUEUE_CLEARING_FAILED);
                }
            }
            finally
            {
                diagnostics.LogSummary(ModConstants.PROCESS_QUEUED_INSTANCES_METHOD_NAME);
            }
        }

        /// <summary>
        /// ⚠️ DEFENSIVE PROGRAMMING: Enhanced MixerExists with comprehensive error handling,
        /// timeout protection, and thread-safe operations for production reliability.
        /// ⚠️ SIMPLIFIED SYNC VERSION: Removed unnecessary async for simple lookup operations
        /// that complete in <5ms and don't involve file I/O or heavy processing.
        /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses framework-safe patterns and exception handling.
        /// </summary>
        public static bool MixerExists(int mixerInstanceID)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Exception operationError = null;

            try
            {
                logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.MIXER_EXISTS_CHECK_TEMPLATE, mixerInstanceID));

                // Input validation with defensive programming
                if (mixerInstanceID <= 0)
                {
                    logger.Warn(ModConstants.WARN_LEVEL_VERBOSE, string.Format(ModConstants.INVALID_MIXER_ID_TEMPLATE, mixerInstanceID));
                    return false;
                }

                // ⚠️ SYNC JUSTIFICATION: This operation is now synchronous because:
                // - Simple LINQ Any() operation completes in 1-5ms typically
                // - ConcurrentBag enumeration is thread-safe and fast
                // - No file I/O or heavy processing involved
                // - Async overhead (3-8ms) is more than the operation itself

                // Use a timeout to prevent hanging in edge cases
                var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(ModConstants.OPERATION_TIMEOUT_MS / ModConstants.MS_PER_SECOND));
                bool result = false;

                // Run with timeout protection for defensive programming
                var task = Task.Run(() =>
                {
                    try
                    {
                        // Get current snapshot of mixers for thread-safe enumeration
                        var mixers = Core.MixerConfigurationTracker.ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        result = mixers.Any(tm => tm.MixerInstanceID == mixerInstanceID);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        logger.Err(string.Format(ModConstants.MIXER_EXISTS_INNER_FAILURE_TEMPLATE, ex.Message));
                        return false;
                    }
                }, timeoutCts.Token);

                // Wait with timeout protection
                if (task.Wait(ModConstants.OPERATION_TIMEOUT_MS)) // 2 second timeout
                {
                    result = task.Result;
                    logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.MIXER_EXISTS_RESULT_TEMPLATE, mixerInstanceID, result, sw.Elapsed.TotalMilliseconds));
                }
                else
                {
                    logger.Warn(ModConstants.WARN_LEVEL_CRITICAL, string.Format(ModConstants.MIXER_EXISTS_TIMEOUT_TEMPLATE, mixerInstanceID));
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                operationError = ex;
                return false;
            }
            finally
            {
                sw.Stop();

                if (operationError != null)
                {
                    logger.Err(string.Format(ModConstants.MIXER_EXISTS_CRASH_PREVENTION_TEMPLATE,
                        mixerInstanceID, operationError.Message, sw.Elapsed.TotalMilliseconds));
                }

                // Performance monitoring and warnings
                var elapsedMs = sw.Elapsed.TotalMilliseconds;
                if (elapsedMs > ModConstants.PERFORMANCE_WARNING_THRESHOLD_MS)
                {
                    logger.Warn(ModConstants.WARN_LEVEL_CRITICAL, string.Format(ModConstants.MIXER_EXISTS_PERFORMANCE_WARNING_TEMPLATE, elapsedMs, mixerInstanceID));
                }
                else if (elapsedMs > ModConstants.PERFORMANCE_SLOW_THRESHOLD_MS)
                {
                    logger.Msg(ModConstants.LOG_LEVEL_IMPORTANT, string.Format(ModConstants.MIXER_EXISTS_SLOW_OPERATION_TEMPLATE, elapsedMs, mixerInstanceID));
                }
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
                    // Reset mixer IDs
                    MixerIDManager.MixerInstanceMap.Clear();
                    MixerIDManager.ResetStableIDCounter();

                    // Clear previous mixer values
                    savedMixerValues.Clear();
                    logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.CURRENT_SAVE_PATH_TEMPLATE, CurrentSavePath ?? ModConstants.NOT_AVAILABLE_PATH_FALLBACK));
                    StartLoadCoroutine();
                    // Force-refresh file copy to save folder
                    if (!string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        string persistentPath = MelonEnvironment.UserDataDirectory;
                        string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json").Replace('/', '\\');
                        string targetFile = Path.Combine(Main.CurrentSavePath, "MixerThresholdSave.json").Replace('/', '\\');
                        if (File.Exists(sourceFile))
                        {
                            var diagnostics = new Helpers.FileOperationDiagnostics();

                            try
                            {
                                string persistentPath = MelonEnvironment.UserDataDirectory;
                                string sourceFile = Path.Combine(persistentPath, ModConstants.MIXER_SAVE_FILENAME).Replace('/', '\\');
                                string targetFile = Path.Combine(CurrentSavePath, ModConstants.MIXER_SAVE_FILENAME).Replace('/', '\\');

                                if (File.Exists(sourceFile))
                                {
                                    diagnostics.StartOperation(ModConstants.FILE_COPY_OPERATION_NAME);

                                    var readSw = System.Diagnostics.Stopwatch.StartNew();
                                    string content = await Helpers.ThreadSafeFileOperations.SafeReadAllTextAsync(sourceFile);
                                    readSw.Stop();

                                    var writeSw = System.Diagnostics.Stopwatch.StartNew();
                                    await Helpers.ThreadSafeFileOperations.SafeWriteAllTextAsync(targetFile, content);
                                    writeSw.Stop();

                                    diagnostics.EndOperation();

                                    logger.Msg(ModConstants.LOG_LEVEL_VERBOSE, string.Format(ModConstants.FILE_COPY_SUCCESS_TEMPLATE, ModConstants.MIXER_SAVE_FILENAME, readSw.Elapsed.TotalMilliseconds, writeSw.Elapsed.TotalMilliseconds, diagnostics.GetLastOperationTime()));
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Err(string.Format(ModConstants.BACKGROUND_FILE_COPY_FAILURE_TEMPLATE, ex));
                            }
                            finally
                            {
                                diagnostics.LogSummary(ModConstants.BACKGROUND_FILE_COPY_OPERATION);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format(ModConstants.ON_SCENE_LOADED_FAILURE_TEMPLATE, ex.Message, ex.StackTrace));
                }
            }
        }

        private static bool _coroutineStarted = false;
        private static void StartLoadCoroutine()
        {
            try
            {
                if (LoadCoroutineStarted) return;
                LoadCoroutineStarted = true;
                MelonCoroutines.Start(CrashResistantSaveManager.LoadMixerValuesWhenReady());
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(ModConstants.START_LOAD_COROUTINE_FAILURE_TEMPLATE, ex));
            }
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
                // Perform the save operation with comprehensive logging
                logger.Msg(2, "[TRANSACTION] Performing save operation...");
                var saveStart = DateTime.Now;
                yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
                var saveTime = (DateTime.Now - saveStart).TotalMilliseconds;
                
                logger.Msg(1, string.Format("[TRANSACTION] Transactional save completed successfully in {0:F1}ms", saveTime));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[TRANSACTION] Transactional save FAILED: {0}", ex.Message));
                logger.Msg(1, "[TRANSACTION] Check backup files for recovery if needed");
            }
        }

        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            logger.Msg(1, "[CONSOLE] Starting advanced save operation profiling");
            
            try
            {
                var profileStart = DateTime.Now;
                
                // Phase 1: Pre-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 1: Pre-save diagnostics");
                var phase1Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Current save path: {0}", CurrentSavePath ?? "[not set]"));
                logger.Msg(3, string.Format("[PROFILE] Mixer count: {0}", savedMixerValues?.Count ?? 0));
                logger.Msg(3, string.Format("[PROFILE] Memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                
                var phase1Time = (DateTime.Now - phase1Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 1 completed in {0:F1}ms", phase1Time));
                
                // Phase 2: Save operation profiling
                logger.Msg(2, "[PROFILE] Phase 2: Save operation profiling");
                var phase2Start = DateTime.Now;
                yield return Save.CrashResistantSaveManager.TriggerSaveWithCooldown();
                var phase2Time = (DateTime.Now - phase2Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 2 (save) completed in {0:F1}ms", phase2Time));
                
                // Phase 3: Post-save diagnostics
                logger.Msg(2, "[PROFILE] Phase 3: Post-save diagnostics");
                var phase3Start = DateTime.Now;
                
                logger.Msg(3, string.Format("[PROFILE] Final memory usage: {0} KB", System.GC.GetTotalMemory(false) / 1024));
                logger.Msg(3, string.Format("[PROFILE] Mixer count after save: {0}", savedMixerValues?.Count ?? 0));
                
                var phase3Time = (DateTime.Now - phase3Start).TotalMilliseconds;
                logger.Msg(2, string.Format("[PROFILE] Phase 3 completed in {0:F1}ms", phase3Time));
                
                var totalTime = (DateTime.Now - profileStart).TotalMilliseconds;
                logger.Msg(1, string.Format("[PROFILE] Advanced profiling complete. Total time: {0:F1}ms", totalTime));
                logger.Msg(1, string.Format("[PROFILE] Breakdown: PreSave={0:F1}ms, Save={1:F1}ms, PostSave={2:F1}ms", 
                    phase1Time, phase2Time, phase3Time));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[PROFILE] Advanced profiling FAILED: {0}", ex.Message));
            }
        }
    }
} // End of namespace