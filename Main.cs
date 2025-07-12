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

// Assembly attributes must come first, before namespace
[assembly: MelonInfo(typeof(MixerThreholdMod_0_0_1.Main), "MixerThreholdMod", "0.0.1", "mooleshacat")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MixerThreholdMod_0_0_1
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
        public static new HarmonyLib.Harmony HarmonyInstance = new HarmonyLib.Harmony("com.mooleshacat.mixerthreholdmod"); // Fixed: added 'new' keyword

        // Thread-safe collections for .NET 4.8.1
        public static readonly ThreadSafeList<MixingStationConfiguration> queuedInstances = new ThreadSafeList<MixingStationConfiguration>();

        // Use ConcurrentDictionary compatible with .NET 4.8.1
        public static readonly System.Collections.Concurrent.ConcurrentDictionary<int, float> savedMixerValues =
            new System.Collections.Concurrent.ConcurrentDictionary<int, float>();

        public static string CurrentSavePath = null;
        private Coroutine mainUpdateCoroutine; // Renamed to avoid any potential conflicts
        private static bool isShuttingDown = false;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            // Add unhandled exception handler for better crash investigation
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            logger.Msg(1, "MixerThreholdMod initializing...");
            logger.Msg(1, string.Format("currentMsgLogLevel: {0}", logger.CurrentMsgLogLevel));
            logger.Msg(1, string.Format("currentWarnLogLevel: {0}", logger.CurrentWarnLogLevel));

            Exception initError = null;
            try
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

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                logger.Err(string.Format("[GLOBAL CRASH] Unhandled exception: {0}\nStackTrace: {1}",
                    exception.Message, exception.StackTrace));

                // Try to save current state before crash - NO YIELD RETURNS here
                Exception saveError = null;
                try
                {
                    Utils.MixerSaveManager.EmergencySave();
                }
                catch (Exception saveEx)
                {
                    saveError = saveEx;
                }

                if (saveError != null)
                {
                    logger.Err(string.Format("Emergency save failed: {0}", saveError.Message));
                }
            }
            else
            {
                logger.Err(string.Format("[GLOBAL CRASH] Non-exception object: {0}", e.ExceptionObject));
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
                    MelonCoroutines.Start(Utils.MixerSaveManager.AttachListenerWhenReady(instance, newTrackedMixer.MixerInstanceID));
                    newTrackedMixer.ListenerAdded = true;
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

            // Restore saved value if exists (thread-safe) - .NET 4.8.1 compatible
            float savedValue;
            if (savedMixerValues.TryGetValue(newTrackedMixer.MixerInstanceID, out savedValue))
            {
                logger.Msg(2, string.Format("Restoring Mixer {0} to {1}", newTrackedMixer.MixerInstanceID, savedValue));

                Exception restoreError = null;
                try
                {
                    instance.StartThrehold.SetValue(savedValue, true);
                }
                catch (Exception ex)
                {
                    restoreError = ex;
                }

                if (restoreError != null)
                {
                    logger.Err(string.Format("Failed to restore value: {0}", restoreError.Message));
                }
            }
        }

        public static bool MixerExists(int mixerInstanceID)
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
                Exception sceneError = null;
                try
                {
                    // Reset mixer state
                    if (MixerIDManager.MixerInstanceMap != null)
                    {
                        MixerIDManager.MixerInstanceMap.Clear();
                    }
                    MixerIDManager.ResetStableIDCounter();
                    savedMixerValues.Clear();

                    logger.Msg(3, "Current Save Path at scene load: " + (CurrentSavePath ?? "[not available yet]"));

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
                            string sourceFile = Path.Combine(persistentPath, "MixerThresholdSave.json");
                            string targetFile = Path.Combine(CurrentSavePath, "MixerThresholdSave.json");

                            if (File.Exists(sourceFile))
                            {
                                FileOperations.SafeCopy(sourceFile, targetFile, overwrite: true);
                                logger.Msg(3, "Copied MixerThresholdSave.json successfully");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    sceneError = ex;
                }

                if (sceneError != null)
                {
                    logger.Err(string.Format("OnSceneWasLoaded error: {0}\nStackTrace: {1}", sceneError.Message, sceneError.StackTrace));
                }
            }
        }

        private static bool _coroutineStarted = false;
        private static void StartLoadCoroutine()
        {
            if (_coroutineStarted || isShuttingDown) return;

            Exception startError = null;
            try
            {
                _coroutineStarted = true;
                MelonCoroutines.Start(Utils.MixerSaveManager.LoadMixerValuesWhenReady());
                logger.Msg(3, "Load coroutine started successfully");
            }
            catch (Exception ex)
            {
                startError = ex;
                _coroutineStarted = false; // Reset on error
            }

            if (startError != null)
            {
                logger.Err(string.Format("StartLoadCoroutine error: {0}\nStackTrace: {1}", startError.Message, startError.StackTrace));
            }
        }

        public override void OnApplicationQuit()
        {
            isShuttingDown = true;

            Exception quitError = null;
            try
            {
                logger.Msg(2, "Application shutting down - cleaning up resources");

                if (mainUpdateCoroutine != null)
                {
                    MelonCoroutines.Stop(mainUpdateCoroutine);
                    mainUpdateCoroutine = null;
                    logger.Msg(2, "Update coroutine stopped");
                }

                // Try to save current state
                if (savedMixerValues.Count > 0)
                {
                    Utils.MixerSaveManager.EmergencySave();
                    logger.Msg(2, "Emergency save completed");
                }
            }
            catch (Exception ex)
            {
                quitError = ex;
            }

            if (quitError != null)
            {
                logger.Err(string.Format("OnApplicationQuit error: {0}", quitError.Message));
            }

            base.OnApplicationQuit();
        }
    }
}