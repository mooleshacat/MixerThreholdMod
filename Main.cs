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
        public static readonly Logger logger = new Logger();
        private static List<MixingStationConfiguration> queuedInstances = new List<MixingStationConfiguration>();
        public static Dictionary<MixingStationConfiguration, float> userSetValues = new Dictionary<MixingStationConfiguration, float>();

        public static Dictionary<int, float> savedMixerValues = new Dictionary<int, float>();
        public static string CurrentSavePath = null;
        private Coroutine mainUpdateCoroutine; // Renamed to avoid any potential conflicts
        private static bool isShuttingDown = false;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            // Global unhandled exception handler
            UnhandledExceptionEventHandler value = (sender, args) =>
            {
                Exception ex = args.ExceptionObject as Exception;
                if (ex != null)
                {
                    Main.logger.Err($"[GLOBAL] Unhandled exception: {ex.Message}\n{ex.StackTrace}");
                }
                else
                {
                    Main.logger.Err($"[GLOBAL] Unhandled exception: {args.ExceptionObject}");
                }
            };
            AppDomain.CurrentDomain.UnhandledException += value;

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

                // Start the update coroutine - explicit cast to Coroutine
                var coroutineObj = MelonCoroutines.Start(UpdateCoroutine());
                mainUpdateCoroutine = coroutineObj as Coroutine;
                logger.Msg(3, "Update coroutine started successfully");
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
                logger.Msg(3, "Queued new MixingStationConfiguration");
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

        public async override void OnUpdate()
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
                // NO TRY-CATCH around yield returns for .NET 4.8.1 compatibility

                // Clean up null/invalid mixers first - use synchronous version
                TrackedMixers.RemoveAll(tm => tm?.ConfigInstance == null);

                if (queuedInstances.Count > 0)
                {
                    var toProcess = queuedInstances.ToList();
                    queuedInstances.Clear();

                    logger.Msg(3, string.Format("Processing {0} queued instances", toProcess.Count));

                    foreach (var instance in toProcess)
                    {
                        if (isShuttingDown) break;

                        // Process without try-catch around yield
                        yield return ProcessSingleInstanceCoroutine(instance);

                        // Yield every few operations to prevent frame drops
                        yield return null;
                    }
                }

                // Normal processing interval - NO try-catch around this yield
                yield return new WaitForSeconds(0.1f);
            }

            logger.Msg(3, "UpdateCoroutine finished");
        }

        private IEnumerator ProcessSingleInstanceCoroutine(MixingStationConfiguration instance)
        {
            if (instance == null || isShuttingDown) yield break;

            Exception processingError = null;
            bool alreadyTracked = false;
            TrackedMixer newTrackedMixer = null;

            // Do all error-prone work BEFORE any yield returns
            try
            {
                // Check if already tracked (thread-safe) - use synchronous version
                alreadyTracked = TrackedMixers.Any(tm => tm?.ConfigInstance == instance);
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

                // Add to collection (thread-safe) - use synchronous version
                TrackedMixers.Add(newTrackedMixer);
                logger.Msg(3, string.Format("Created mixer with ID: {0}", newTrackedMixer.MixerInstanceID));
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

                        // Prevent duplicate processing of the same instance
                        bool alreadyTracked = await TrackedMixers.AnyAsync(tm => tm?.ConfigInstance == instance);
                        if (alreadyTracked)
                        {
                            logger.Warn(1, $"Instance already tracked — skipping duplicate: {instance}");
                            continue;
                        }

                        if (instance.StartThrehold == null)
                        {
                            logger.Warn(1, "StartThrehold is null for instance. Skipping for now.");
                            continue;
                        }

                        var existingMixerData = await TrackedMixers.FirstOrDefaultAsync(tm => tm?.ConfigInstance == instance);
                        if (existingMixerData == null)
=======
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
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
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
<<<<<<< HEAD
                                
=======
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                await TrackedMixers.AddAsync(newTrackedMixer);
                                logger.Msg(3, $"Created mixer with Stable ID: {newTrackedMixer.MixerInstanceID}");

                                // Now safely add listener
                                if (!newTrackedMixer.ListenerAdded)
                                {
                                    logger.Msg(3, $"Attaching listener for Mixer {newTrackedMixer.MixerInstanceID}");
                                    try
                                    {
                                        Utils.CoroutineHelper.RunCoroutine(MixerSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                                        newTrackedMixer.ListenerAdded = true;
                                    }
<<<<<<< HEAD
                                    catch (Exception listenerEx)
                                    {
                                        logger.Err($"OnUpdate: Failed to start listener attachment coroutine: {listenerEx.Message}");
=======
                                    catch (Exception ex)
                                    {
                                        logger.Err($"Error starting AttachListenerWhenReady coroutine for Mixer {newTrackedMixer.MixerInstanceID}: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                    }
                                }

                                // Restore saved value if exists
<<<<<<< HEAD
                                if (savedMixerValues != null && savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out float savedValue))
=======
                                if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out float savedValue))
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                                {
                                    try
                                    {
                                        logger.Msg(2, $"Restoring Mixer {newTrackedMixer.MixerInstanceID} to {savedValue}");
                                        instance.StartThrehold.SetValue(savedValue, true);
                                    }
<<<<<<< HEAD
                                    catch (Exception restoreEx)
                                    {
                                        logger.Err($"OnUpdate: Failed to restore saved value for Mixer {newTrackedMixer.MixerInstanceID}: {restoreEx.Message}");
                                    }
                                }
                            }
                            catch (Exception configEx)
                            {
                                Main.logger.Err("Error configuring mixer: " + configEx.Message + "\n" + configEx.StackTrace);
=======
                                    catch (Exception ex)
                                    {
                                        logger.Err($"Error restoring saved value for Mixer {newTrackedMixer.MixerInstanceID}: {ex}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Main.logger.Err("Error configuring mixer: " + ex);
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                            }
                        }
                    }
                    catch (Exception instanceEx)
                    {
<<<<<<< HEAD
                        logger.Err($"OnUpdate: Error processing mixer instance: {instanceEx.Message}\n{instanceEx.StackTrace}");
                    }
                }

                queuedInstances?.Clear();
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