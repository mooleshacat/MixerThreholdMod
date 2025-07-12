using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using MixerThreholdMod_0_0_1.Utils;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

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
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_1_0_0
{
    // Thread-safe list implementation for .NET 4.8.1 - only this class stays in Main.cs
    public class ThreadSafeList<T>
    {
        private readonly List<T> _list = new List<T>();
        private readonly object _lock = new object();

        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        public List<T> ToList()
        {
            lock (_lock)
            {
                return new List<T>(_list);
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        public void RemoveAll(Predicate<T> match)
        {
            lock (_lock)
            {
                _list.RemoveAll(match);
            }
        }

        public bool Any(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                return _list.Any(predicate);
            }
        }
    }

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

        public static Dictionary<int, float> savedMixerValues = new Dictionary<int, float>();
        public static string CurrentSavePath = null;
        private Coroutine mainUpdateCoroutine; // Renamed to avoid any potential conflicts
        private static bool isShuttingDown = false;

        public override void OnInitializeMelon()
        {
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
                logger.Err(string.Format("OnInitializeMelon error: {0}\nStackTrace: {1}", initError.Message, initError.StackTrace));
            }
        }

        private static void QueueInstance(MixingStationConfiguration __instance)
        {
            if (isShuttingDown) return;

            Exception queueError = null;
            try
            {
                if (__instance == null)
                {
                    logger.Warn(1, "QueueInstance: Attempting to queue null instance");
                    return;
                }

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
                logger.Err(string.Format("QueueInstance error: {0}\nStackTrace: {1}", queueError.Message, queueError.StackTrace));
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
        public override void OnUpdate()
        {
            // Keep OnUpdate minimal for .NET 4.8.1 compatibility
            if (isShuttingDown && mainUpdateCoroutine != null)
            {
                MelonCoroutines.Stop(mainUpdateCoroutine);
                mainUpdateCoroutine = null;
            }
        }

        private IEnumerator UpdateCoroutine()
        {
            logger.Msg(3, "UpdateCoroutine started");

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
                Exception updateError = null;
                try
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
                // Check if already tracked using coroutine approach
                bool alreadyTracked = false;
                yield return CheckIfAlreadyTracked(instance, (result) => alreadyTracked = result);

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
                    MixerInstanceID = MixerIDManager.GetMixerID(instance)
                };

                // Add to tracking collection
                Core.TrackedMixers.Add(newTrackedMixer);
                logger.Msg(1, string.Format("[MAIN] ✓ MIXER PROCESSED: Created mixer with ID: {0}", newTrackedMixer.MixerInstanceID));
            }
            catch (Exception ex)
            {
                processingError = ex;
            }

            if (processingError != null)
            {
                logger.Err(string.Format("ProcessSingleInstance error: {0}\nStackTrace: {1}", processingError.Message, processingError.StackTrace));
                yield break;
            }

            if (newTrackedMixer == null) yield break;

            // Attach listener safely - NO try-catch around yield
            if (!newTrackedMixer.ListenerAdded)
            {
                logger.Msg(3, string.Format("Attaching listener for Mixer {0}", newTrackedMixer.MixerInstanceID));

                Exception listenerError = null;
                try
                {
<<<<<<< HEAD
                    await TrackedMixers.RemoveAllAsync(tm => tm?.ConfigInstance == null);
                }
                catch (Exception listenerEx)
                {
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

            try
            {
                result = TrackedMixers.Any(tm => tm.MixerInstanceID == mixerInstanceID);
            }
            catch (Exception ex)
            {
                checkError = ex;
            }

            if (checkError != null)
            {
                logger.Err(string.Format("MixerExists error: {0}", checkError.Message));
                return false;
            }

            return result;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            logger.Msg(2, "Scene loaded: " + sceneName);

            if (sceneName == "Main" && !isShuttingDown)
            {
                base.OnSceneWasLoaded(buildIndex, sceneName);
                logger.Msg(2, "Scene loaded: " + sceneName);
                
<<<<<<< HEAD
                if (sceneName == "Main")
                {
                    // Reset mixer state
                    if (MixerIDManager.MixerInstanceMap != null)
                    {
                        MixerIDManager.MixerInstanceMap.Clear();
                    }
                    MixerIDManager.ResetStableIDCounter();
                    savedMixerValues.Clear();

                        // Clear previous mixer values
                        if (savedMixerValues != null)
                        {
                            savedMixerValues.Clear();
                        }

                        logger.Msg(3, "Current Save Path at scene load: " + (Main.CurrentSavePath ?? "[not available yet]"));

                    // Start load coroutine
                    if (!_coroutineStarted)
                    {
                        StartLoadCoroutine();
                    }

                    // Handle file copying safely
                    if (!string.IsNullOrEmpty(CurrentSavePath))
                    {
                        string persistentPath = MelonEnvironment.UserDataDirectory;
                        if (!string.IsNullOrEmpty(persistentPath))
                        {
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
                            }
                        }
                    }
                }
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
                }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Critical error in OnSceneWasLoaded: {ex}");
            }
        }

        private static bool _loadCoroutineStarted = false;
        public static bool LoadCoroutineStarted { get { return _loadCoroutineStarted; } }

        /// <summary>
        /// Start the load coroutine once per session
        /// </summary>
        private static void StartLoadCoroutine()
        {
            if (_coroutineStarted || isShuttingDown) return;

            Exception startError = null;
            try
            {
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
        }
    }
}