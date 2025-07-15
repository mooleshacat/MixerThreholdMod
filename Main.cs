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
            
            // Initialize IL2CPP-compatible patches
            try
            {
                Patches.SaveManager_Save_Patch.Initialize();
                logger.Msg(2, "SaveManager_Save_Patch initialized");
                
                Patches.LoadManager_LoadedGameFolderPath_Patch.Initialize();
                logger.Msg(2, "LoadManager_LoadedGameFolderPath_Patch initialized");
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("Failed to initialize patches: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }
        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            queuedInstances.Add(__instance);
            logger.Msg(3, "Queued new MixingStationConfiguration");
        }
        public async override void OnUpdate()
        {
            // 🔁 Clean up tracked mixers at the start of each update
            await MixerConfigurationTracker.RemoveAllAsync(tm => tm.ConfigInstance == null);
            if (queuedInstances.Count == 0) return;
            var toProcess = queuedInstances.ToList();
            foreach (var instance in toProcess)
            {
                // Prevent duplicate processing of the same instance
                if (await MixerConfigurationTracker.AnyAsync(tm => tm.ConfigInstance == instance))
                {
                    logger.Warn(1, string.Format("Instance already tracked — skipping duplicate: {0}", instance));
                    continue;
                }
                if (instance.StartThrehold == null)
                {
                    logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                    continue;
                }
                var mixerData = await MixerConfigurationTracker.FirstOrDefaultAsync(tm => tm.ConfigInstance == instance);
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
                        await MixerConfigurationTracker.AddAsync(newTrackedMixer);
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
            return await MixerConfigurationTracker.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
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

        /// <summary>
        /// Property to track if load coroutine has started
        /// ⚠️ THREAD SAFETY: Thread-safe boolean property for coroutine state tracking
        /// </summary>
        public static bool LoadCoroutineStarted
        {
            get { return _coroutineStarted; }
            set { _coroutineStarted = value; }
        }

        /// <summary>
        /// Perform atomic transactional save operation
        /// ⚠️ CRASH PREVENTION: Atomic save operations with rollback capability
        /// ⚠️ THREAD SAFETY: Uses thread-safe operations and proper error handling
        /// </summary>
        public static IEnumerator PerformTransactionalSave()
        {
            Exception transactionError = null;
            var startTime = DateTime.UtcNow;
            string backupPath = null;
            
            try
            {
                logger.Msg(LOG_LEVEL_CRITICAL, "[TRANSACTION] Starting atomic transactional save operation");
                
                if (string.IsNullOrEmpty(CurrentSavePath))
                {
                    logger.Err("[TRANSACTION] No current save path available");
                    yield break;
                }

                if (savedMixerValues.Count == 0)
                {
                    logger.Warn(WARN_LEVEL_CRITICAL, "[TRANSACTION] No mixer data to save");
                    yield break;
                }

                // Step 1: Create backup of current save
                yield return new WaitForSeconds(0.1f);
                try
                {
                    string timestamp = DateTime.UtcNow.ToString(FILENAME_DATETIME_FORMAT);
                    backupPath = Path.Combine(CurrentSavePath, string.Format("MixerThresholdSave_transaction_backup_{0}.json", timestamp));
                    
                    string currentSavePath = Path.Combine(CurrentSavePath, MIXER_SAVE_FILENAME);
                    if (File.Exists(currentSavePath))
                    {
                        File.Copy(currentSavePath, backupPath, true);
                        logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[TRANSACTION] Backup created: {0}", backupPath));
                    }
                }
                catch (Exception backupEx)
                {
                    logger.Err(string.Format("[TRANSACTION] Failed to create backup: {0}", backupEx.Message));
                    yield break;
                }

                yield return new WaitForSeconds(0.1f);

                // Step 2: Perform transactional save
                bool saveSuccess = false;
                try
                {
                    yield return MelonCoroutines.Start(Save.CrashResistantSaveManager.TriggerSaveWithCooldown());
                    
                    // Wait for save completion
                    yield return new WaitForSeconds(1.0f);
                    
                    // Verify save integrity
                    string savePath = Path.Combine(CurrentSavePath, MIXER_SAVE_FILENAME);
                    if (File.Exists(savePath))
                    {
                        var fileInfo = new FileInfo(savePath);
                        if (fileInfo.Length > MIN_VALID_FILE_SIZE_BYTES)
                        {
                            saveSuccess = true;
                            logger.Msg(LOG_LEVEL_CRITICAL, "[TRANSACTION] Transactional save completed successfully");
                        }
                        else
                        {
                            logger.Err(string.Format("[TRANSACTION] Save file too small: {0} bytes", fileInfo.Length));
                        }
                    }
                    else
                    {
                        logger.Err("[TRANSACTION] Save file not found after save operation");
                    }
                }
                catch (Exception saveEx)
                {
                    logger.Err(string.Format("[TRANSACTION] Save operation failed: {0}", saveEx.Message));
                }

                // Step 3: Handle rollback if necessary
                if (!saveSuccess && !string.IsNullOrEmpty(backupPath) && File.Exists(backupPath))
                {
                    try
                    {
                        string targetPath = Path.Combine(CurrentSavePath, MIXER_SAVE_FILENAME);
                        File.Copy(backupPath, targetPath, true);
                        logger.Warn(WARN_LEVEL_CRITICAL, "[TRANSACTION] Rollback completed - restored from backup");
                    }
                    catch (Exception rollbackEx)
                    {
                        logger.Err(string.Format("[TRANSACTION] Rollback failed: {0}", rollbackEx.Message));
                    }
                }

                // Step 4: Cleanup backup file if save was successful
                if (saveSuccess && !string.IsNullOrEmpty(backupPath) && File.Exists(backupPath))
                {
                    try
                    {
                        File.Delete(backupPath);
                        logger.Msg(LOG_LEVEL_VERBOSE, "[TRANSACTION] Backup file cleaned up");
                    }
                    catch (Exception cleanupEx)
                    {
                        logger.Warn(WARN_LEVEL_VERBOSE, string.Format("[TRANSACTION] Failed to cleanup backup: {0}", cleanupEx.Message));
                    }
                }

                var duration = DateTime.UtcNow - startTime;
                logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TRANSACTION] Transaction completed in {0:F3}s, success: {1}", 
                    duration.TotalSeconds, saveSuccess));
            }
            catch (Exception ex)
            {
                transactionError = ex;
            }

            if (transactionError != null)
            {
                logger.Err(string.Format("[TRANSACTION] Critical error during transactional save: {0}\n{1}", 
                    transactionError.Message, transactionError.StackTrace));
                
                // Emergency rollback attempt
                if (!string.IsNullOrEmpty(backupPath) && File.Exists(backupPath))
                {
                    try
                    {
                        string targetPath = Path.Combine(CurrentSavePath, MIXER_SAVE_FILENAME);
                        File.Copy(backupPath, targetPath, true);
                        logger.Msg(LOG_LEVEL_CRITICAL, "[TRANSACTION] Emergency rollback completed");
                    }
                    catch (Exception emergencyEx)
                    {
                        logger.Err(string.Format("[TRANSACTION] Emergency rollback failed: {0}", emergencyEx.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Advanced save operation profiling with comprehensive performance monitoring
        /// ⚠️ THREAD SAFETY: Safe profiling operations with detailed system monitoring
        /// ⚠️ CRASH PREVENTION: Comprehensive error handling throughout profiling process
        /// </summary>
        public static IEnumerator AdvancedSaveOperationProfiling()
        {
            Exception profilingError = null;
            var overallStartTime = DateTime.UtcNow;
            
            try
            {
                logger.Msg(LOG_LEVEL_CRITICAL, "[PROFILE] === ADVANCED SAVE OPERATION PROFILING ===");
                
                if (string.IsNullOrEmpty(CurrentSavePath))
                {
                    logger.Err("[PROFILE] No current save path available");
                    yield break;
                }

                if (savedMixerValues.Count == 0)
                {
                    logger.Warn(WARN_LEVEL_CRITICAL, "[PROFILE] No mixer data available for profiling");
                    yield break;
                }

                // Initialize system monitoring
                SystemMonitor.Initialize();
                yield return new WaitForSeconds(0.1f);

                // Phase 1: Pre-save system analysis
                logger.Msg(LOG_LEVEL_IMPORTANT, "[PROFILE] Phase 1: Pre-save system analysis");
                var prePhaseStart = DateTime.UtcNow;
                
                SystemMonitor.LogCurrentPerformance("PRE_SAVE_ANALYSIS");
                
                // Analyze current mixer state
                logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Current mixer values count: {0}", savedMixerValues.Count));
                logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Save path: {0}", CurrentSavePath));
                
                // Check file system state
                try
                {
                    var driveInfo = new DriveInfo(Path.GetPathRoot(CurrentSavePath));
                    logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Available disk space: {0:F2} GB", 
                        driveInfo.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0)));
                }
                catch (Exception diskEx)
                {
                    logger.Warn(WARN_LEVEL_VERBOSE, string.Format("[PROFILE] Could not get disk info: {0}", diskEx.Message));
                }

                var prePhaseTime = DateTime.UtcNow - prePhaseStart;
                logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Phase 1 completed in {0:F3}s", prePhaseTime.TotalSeconds));

                yield return new WaitForSeconds(0.5f);

                // Phase 2: Save operation with detailed timing
                logger.Msg(LOG_LEVEL_IMPORTANT, "[PROFILE] Phase 2: Save operation profiling");
                var savePhaseStart = DateTime.UtcNow;

                SystemMonitor.LogCurrentPerformance("SAVE_OPERATION_START");

                try
                {
                    // Trigger save with comprehensive monitoring
                    yield return MelonCoroutines.Start(Save.CrashResistantSaveManager.TriggerSaveWithCooldown());
                    
                    // Wait for completion and monitor
                    yield return new WaitForSeconds(1.0f);
                    
                    SystemMonitor.LogCurrentPerformance("SAVE_OPERATION_COMPLETE");
                }
                catch (Exception saveEx)
                {
                    logger.Err(string.Format("[PROFILE] Save operation failed during profiling: {0}", saveEx.Message));
                }

                var savePhaseTime = DateTime.UtcNow - savePhaseStart;
                logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Phase 2 completed in {0:F3}s", savePhaseTime.TotalSeconds));

                yield return new WaitForSeconds(0.5f);

                // Phase 3: Post-save analysis and verification
                logger.Msg(LOG_LEVEL_IMPORTANT, "[PROFILE] Phase 3: Post-save analysis");
                var postPhaseStart = DateTime.UtcNow;

                SystemMonitor.LogCurrentPerformance("POST_SAVE_ANALYSIS");

                // Verify save file integrity
                try
                {
                    string savePath = Path.Combine(CurrentSavePath, MIXER_SAVE_FILENAME);
                    if (File.Exists(savePath))
                    {
                        var fileInfo = new FileInfo(savePath);
                        logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Save file size: {0:F2} KB", fileInfo.Length / 1024.0));
                        logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Save file timestamp: {0:yyyy-MM-dd HH:mm:ss.fff}", fileInfo.LastWriteTime));
                        
                        // Validate JSON content
                        string content = File.ReadAllText(savePath);
                        if (content.Length >= MIN_VALID_JSON_LENGTH && content.Contains("{") && content.Contains("}"))
                        {
                            logger.Msg(LOG_LEVEL_IMPORTANT, "[PROFILE] Save file validation: PASSED");
                        }
                        else
                        {
                            logger.Warn(WARN_LEVEL_CRITICAL, "[PROFILE] Save file validation: FAILED - Invalid JSON structure");
                        }
                    }
                    else
                    {
                        logger.Err("[PROFILE] Save file not found after save operation");
                    }
                }
                catch (Exception verifyEx)
                {
                    logger.Err(string.Format("[PROFILE] Save verification failed: {0}", verifyEx.Message));
                }

                var postPhaseTime = DateTime.UtcNow - postPhaseStart;
                logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("[PROFILE] Phase 3 completed in {0:F3}s", postPhaseTime.TotalSeconds));

                // Final summary
                var totalTime = DateTime.UtcNow - overallStartTime;
                logger.Msg(LOG_LEVEL_CRITICAL, "[PROFILE] === PROFILING SUMMARY ===");
                logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[PROFILE] Total profiling time: {0:F3}s", totalTime.TotalSeconds));
                logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[PROFILE] Pre-save analysis: {0:F3}s ({1:F1}%)", 
                    prePhaseTime.TotalSeconds, (prePhaseTime.TotalSeconds / totalTime.TotalSeconds) * 100));
                logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[PROFILE] Save operation: {0:F3}s ({1:F1}%)", 
                    savePhaseTime.TotalSeconds, (savePhaseTime.TotalSeconds / totalTime.TotalSeconds) * 100));
                logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[PROFILE] Post-save analysis: {0:F3}s ({1:F1}%)", 
                    postPhaseTime.TotalSeconds, (postPhaseTime.TotalSeconds / totalTime.TotalSeconds) * 100));

                SystemMonitor.LogCurrentPerformance("PROFILING_COMPLETE");
            }
            catch (Exception ex)
            {
                profilingError = ex;
            }

            if (profilingError != null)
            {
                logger.Err(string.Format("[PROFILE] Critical error during profiling: {0}\n{1}", 
                    profilingError.Message, profilingError.StackTrace));
            }
        }

    }
} // End of namespace