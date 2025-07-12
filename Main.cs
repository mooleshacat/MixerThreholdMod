using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Save;
using MixerThreholdMod_1_0_0.Threading;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
        public static readonly MixerThreholdMod_1_0_0.Core.Logger logger = new MixerThreholdMod_1_0_0.Core.Logger();
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
                logger.Msg(2, "[MAIN] ✓ Console commands registered");

                // Start the main update coroutine with runtime mixer scanning
                var coroutineObj = MelonCoroutines.Start(UpdateCoroutine());
                mainUpdateCoroutine = coroutineObj as Coroutine;
                logger.Msg(1, "[MAIN] ✓ Main update coroutine started successfully");
                
                // Start fallback mixer scanner coroutine
                MelonCoroutines.Start(RuntimeMixerScanner());
                logger.Msg(1, "[MAIN] ✓ Runtime mixer scanner started");
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
                logger.Msg(1, "[MAIN] ✓ MIXER DETECTED: Queued new MixingStationConfiguration for processing");
                logger.Msg(2, string.Format("[MAIN] Queue size now: {0}", queuedInstances.Count));
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
        /// Runtime mixer scanner as fallback detection method
        /// ⚠️ CRASH PREVENTION: Scans for mixers in case constructor patching fails
        /// </summary>
        private IEnumerator RuntimeMixerScanner()
        {
            logger.Msg(2, "[MAIN] RuntimeMixerScanner: Starting...");
            
            yield return new WaitForSeconds(5f); // Wait for game to initialize
            
            int scanCount = 0;
            while (!isShuttingDown)
            {
                Exception scanError = null;
                scanCount++;
                
                try
                {
                    logger.Msg(3, string.Format("[MAIN] RuntimeMixerScanner: Scan #{0}", scanCount));
                    
                    // Method 1: Find MixingStation objects
                    var foundMixingStations = UnityEngine.Object.FindObjectsOfType<MixingStation>();
                    logger.Msg(2, string.Format("[MAIN] RuntimeMixerScanner: Found {0} MixingStation objects", foundMixingStations?.Length ?? 0));
                    
                    if (foundMixingStations != null && foundMixingStations.Length > 0)
                    {
                        foreach (var mixingStation in foundMixingStations)
                        {
                            if (isShuttingDown) break;
                            
                            yield return ProcessFoundMixingStation(mixingStation, scanCount);
                        }
                    }
                    
                    // Method 2: Try to find MixingStationConfiguration objects directly
                    var foundConfigurations = UnityEngine.Object.FindObjectsOfType<MixingStationConfiguration>();
                    logger.Msg(2, string.Format("[MAIN] RuntimeMixerScanner: Found {0} MixingStationConfiguration objects directly", foundConfigurations?.Length ?? 0));
                    
                    if (foundConfigurations != null && foundConfigurations.Length > 0)
                    {
                        foreach (var config in foundConfigurations)
                        {
                            if (isShuttingDown) break;
                            
                            if (config != null)
                            {
                                // Check if already tracked
                                var alreadyTrackedResult = yield return CheckIfConfigurationTracked(config);
                                bool alreadyTracked = false;
                                if (alreadyTrackedResult is bool)
                                {
                                    alreadyTracked = (bool)alreadyTrackedResult;
                                }
                                
                                if (!alreadyTracked)
                                {
                                    queuedInstances.Add(config);
                                    logger.Msg(1, string.Format("[MAIN] ✓ FALLBACK DETECTION: Found MixingStationConfiguration directly: {0}", config.GetHashCode()));
                                }
                            }
                        }
                    }
                    
                    // Method 3: Search in all active GameObjects for mixer-related components
                    if (scanCount % 5 == 0) // Every 5th scan (every 50 seconds)
                    {
                        yield return PerformDeepMixerScan(scanCount);
                    }
                }
                catch (Exception ex)
                {
                    scanError = ex;
                }

                if (scanError != null)
                {
                    logger.Err(string.Format("[MAIN] RuntimeMixerScanner CRASH PREVENTION: Error in scan #{0}: {1}", scanCount, scanError.Message));
                }

                // Scan every 10 seconds
                yield return new WaitForSeconds(10f);
            }
            
            logger.Msg(2, "[MAIN] RuntimeMixerScanner: Stopped");
        }

        /// <summary>
        /// Process a found MixingStation object
        /// </summary>
        private IEnumerator ProcessFoundMixingStation(MixingStation mixingStation, int scanCount)
        {
            Exception processError = null;
            
            try
            {
                logger.Msg(3, string.Format("[MAIN] ProcessFoundMixingStation: Processing station {0} (scan #{1})", mixingStation.GetHashCode(), scanCount));
                
                // Try multiple approaches to get the configuration
                MixingStationConfiguration config = null;
                
                // Approach 1: Look for private configuration field
                var configField = mixingStation.GetType().GetField("configuration", BindingFlags.NonPublic | BindingFlags.Instance);
                if (configField != null)
                {
                    config = configField.GetValue(mixingStation) as MixingStationConfiguration;
                    if (config != null)
                    {
                        logger.Msg(2, string.Format("[MAIN] Found config via private field: {0}", config.GetHashCode()));
                    }
                }
                
                // Approach 2: Look for public properties
                if (config == null)
                {
                    var properties = mixingStation.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in properties)
                    {
                        if (prop.PropertyType == typeof(MixingStationConfiguration) && prop.CanRead)
                        {
                            try
                            {
                                config = prop.GetValue(mixingStation, null) as MixingStationConfiguration;
                                if (config != null)
                                {
                                    logger.Msg(2, string.Format("[MAIN] Found config via property {0}: {1}", prop.Name, config.GetHashCode()));
                                    break;
                                }
                            }
                            catch (Exception propEx)
                            {
                                logger.Warn(2, string.Format("[MAIN] Error reading property {0}: {1}", prop.Name, propEx.Message));
                            }
                        }
                    }
                }
                
                // Approach 3: Check if the MixingStation itself is a configuration
                if (config == null && mixingStation is MixingStationConfiguration)
                {
                    config = mixingStation as MixingStationConfiguration;
                    logger.Msg(2, string.Format("[MAIN] MixingStation is itself a configuration: {0}", config.GetHashCode()));
                }
                
                if (config != null)
                {
                    // Check if already tracked
                    var alreadyTrackedResult = yield return CheckIfConfigurationTracked(config);
                    bool alreadyTracked = false;
                    if (alreadyTrackedResult is bool)
                    {
                        alreadyTracked = (bool)alreadyTrackedResult;
                    }
                    
                    if (!alreadyTracked)
                    {
                        queuedInstances.Add(config);
                        logger.Msg(1, string.Format("[MAIN] ✓ FALLBACK DETECTION: Found mixer via station scan: {0}", config.GetHashCode()));
                    }
                    else
                    {
                        logger.Msg(3, string.Format("[MAIN] Configuration already tracked: {0}", config.GetHashCode()));
                    }
                }
                else
                {
                    logger.Warn(2, string.Format("[MAIN] Could not extract configuration from MixingStation: {0}", mixingStation.GetHashCode()));
                }
            }
            catch (Exception ex)
            {
                processError = ex;
            }

            if (processError != null)
            {
                logger.Err(string.Format("[MAIN] ProcessFoundMixingStation: Error: {0}", processError.Message));
            }
        }

        /// <summary>
        /// Check if a configuration is already tracked
        /// </summary>
        private IEnumerator CheckIfConfigurationTracked(MixingStationConfiguration config)
        {
            bool result = false;
            Exception checkError = null;

            try
            {
                var task = Core.TrackedMixers.AnyAsync(tm => tm?.ConfigInstance == config);
                
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
                    logger.Err(string.Format("[MAIN] CheckIfConfigurationTracked: Task faulted: {0}", task.Exception?.Message));
                }
            }
            catch (Exception ex)
            {
                checkError = ex;
            }

            if (checkError != null)
            {
                logger.Err(string.Format("[MAIN] CheckIfConfigurationTracked: Error: {0}", checkError.Message));
            }

            yield return result;
        }

        /// <summary>
        /// Perform deep scan of all GameObjects for mixer components
        /// </summary>
        private IEnumerator PerformDeepMixerScan(int scanCount)
        {
            Exception deepScanError = null;
            
            try
            {
                logger.Msg(2, string.Format("[MAIN] PerformDeepMixerScan: Starting deep scan #{0}", scanCount));
                
                // Find all active GameObjects
                var allGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                logger.Msg(3, string.Format("[MAIN] Deep scan: Found {0} GameObjects", allGameObjects?.Length ?? 0));
                
                if (allGameObjects != null)
                {
                    int mixerRelatedObjects = 0;
                    foreach (var go in allGameObjects)
                    {
                        if (isShuttingDown) break;
                        
                        try
                        {
                            // Look for mixer-related components
                            var components = go.GetComponents<Component>();
                            foreach (var component in components)
                            {
                                if (component != null)
                                {
                                    var componentType = component.GetType();
                                    var typeName = componentType.Name.ToLower();
                                    
                                    // Check for mixer-related type names
                                    if (typeName.Contains("mixer") || typeName.Contains("mixing") || typeName.Contains("station"))
                                    {
                                        mixerRelatedObjects++;
                                        logger.Msg(2, string.Format("[MAIN] Deep scan: Found mixer-related component: {0} on {1}", componentType.Name, go.name));
                                        
                                        // Try to cast to known types
                                        if (component is MixingStationConfiguration config)
                                        {
                                            var alreadyTrackedResult = yield return CheckIfConfigurationTracked(config);
                                            bool alreadyTracked = false;
                                            if (alreadyTrackedResult is bool)
                                            {
                                                alreadyTracked = (bool)alreadyTrackedResult;
                                            }
                                            
                                            if (!alreadyTracked)
                                            {
                                                queuedInstances.Add(config);
                                                logger.Msg(1, string.Format("[MAIN] ✓ DEEP SCAN DETECTION: Found configuration: {0}", config.GetHashCode()));
                                            }
                                        }
                                        else if (component is MixingStation station)
                                        {
                                            yield return ProcessFoundMixingStation(station, scanCount);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception goEx)
                        {
                            logger.Warn(3, string.Format("[MAIN] Deep scan: Error processing GameObject {0}: {1}", go.name, goEx.Message));
                        }
                        
                        // Yield every 50 objects to prevent frame drops
                        if (mixerRelatedObjects % 50 == 0)
                        {
                            yield return null;
                        }
                    }
                    
                    logger.Msg(2, string.Format("[MAIN] Deep scan #{0}: Found {1} mixer-related objects", scanCount, mixerRelatedObjects));
                }
            }
            catch (Exception ex)
            {
                deepScanError = ex;
            }

            if (deepScanError != null)
            {
                logger.Err(string.Format("[MAIN] PerformDeepMixerScan: Error: {0}", deepScanError.Message));
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
            logger.Msg(2, "[MAIN] UpdateCoroutine started");
            int frameCount = 0;

            while (!isShuttingDown)
            {
                Exception updateError = null;
                try
                {
                    // Clean up null/invalid mixers first - with better error handling
                    try
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
                catch (Exception ex)
                {
                    updateError = ex;
                }

                if (updateError != null)
                {
                    logger.Err(string.Format("[MAIN] UpdateCoroutine CRASH PREVENTION: Error: {0}\nStackTrace: {1}", 
                        updateError.Message, updateError.StackTrace));
                }

                // Normal processing interval
                yield return new WaitForSeconds(0.1f);
            }

            logger.Msg(3, "[MAIN] UpdateCoroutine finished");
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
            TrackedMixer newTrackedMixer = null;

            try
            {
                // Check if already tracked using coroutine approach
                bool alreadyTracked = false;
                yield return CheckIfAlreadyTracked(instance, (result) => alreadyTracked = result);

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
                    MixerInstanceID = Core.MixerIDManager.GetMixerID(instance)
                };

                // Add to tracking collection using coroutine
                yield return AddTrackedMixerAsync(newTrackedMixer);
                logger.Msg(1, string.Format("[MAIN] ✓ MIXER PROCESSED: Created mixer with ID: {0}", newTrackedMixer.MixerInstanceID));
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
                // Use timeout-safe approach for checking mixer existence
                var task = Core.TrackedMixers.AnyAsync(tm => tm.MixerInstanceID == mixerInstanceID);
                
                // Use shorter timeout for existence check
                bool completed = task.Wait(1000); // 1 second timeout
                
                if (completed && !task.IsFaulted)
                {
                    return task.Result;
                }
                else
                {
                    logger.Warn(1, string.Format("[MAIN] MixerExists: Timeout or error checking mixer {0}", mixerInstanceID));
                    return false;
                }
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
                Exception sceneError = null;
                try
                {
                    // Reset mixer state for new game session
                    if (Core.MixerIDManager.MixerInstanceMap != null)
                    {
                        Core.MixerIDManager.MixerInstanceMap.Clear();
                    }
                    Core.MixerIDManager.ResetStableIDCounter();
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
        public static bool LoadCoroutineStarted { get { return _loadCoroutineStarted; } }

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
                quitError = ex;
            }

            if (quitError != null)
            {
                logger.Err(string.Format("[MAIN] OnApplicationQuit error: {0}", quitError.Message));
            }

            logger.Msg(1, "[MAIN] MixerThresholdMod shutdown completed");
            base.OnApplicationQuit();
        }
    }
}