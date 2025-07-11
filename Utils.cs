using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixerThreholdMod_0_0_1
{
    public class Utils
    {
        /// <summary>
        /// Normalizes a path by replacing forward slashes with backslashes (Windows-style)
        /// </summary>
        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            
            return path.Replace('/', '\\');
        }

        /// <summary>
        /// Safely executes an action with error logging
        /// </summary>
        public static void SafeExecute(Action action, string operationName = "operation")
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Main.logger.Err($"{operationName}: Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Safely executes an async action with error logging
        /// </summary>
        public static async Task SafeExecuteAsync(Func<Task> asyncAction, string operationName = "async operation")
        {
            try
            {
                if (asyncAction != null)
                    await asyncAction();
            }
            catch (Exception ex)
            {
                Main.logger.Err($"{operationName}: Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Safely gets a value with null checking and error handling
        /// </summary>
        public static T SafeGet<T>(Func<T> getter, T defaultValue = default(T), string operationName = "get operation")
        {
            try
            {
                return getter != null ? getter() : defaultValue;
            }
            catch (Exception ex)
            {
                Main.logger.Err($"{operationName}: Error: {ex.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Validates that a directory exists and creates it if it doesn't
        /// </summary>
        public static bool EnsureDirectoryExists(string path, string operationName = "directory operation")
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    Main.logger.Warn(1, $"{operationName}: Path is null or empty");
                    return false;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Main.logger.Msg(3, $"{operationName}: Created directory: {path}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Main.logger.Err($"{operationName}: Failed to ensure directory exists at {path}: {ex.Message}");
                return false;
            }
        }
        public class CoroutineHelper : MonoBehaviour
        {
            private static CoroutineHelper _instance;
            public static CoroutineHelper Instance
            {
                get
                {
                    try
                    {
                        if (_instance == null)
                        {
                            var go = new GameObject("CoroutineHelper");
<<<<<<< HEAD
                            if (go != null)
                            {
                                _instance = go.AddComponent<CoroutineHelper>();
                                if (_instance != null)
                                {
                                    DontDestroyOnLoad(go);
                                }
                            }
=======
                            _instance = go.AddComponent<CoroutineHelper>();
                            DontDestroyOnLoad(go);
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                        }
                        return _instance;
                    }
                    catch (Exception ex)
                    {
<<<<<<< HEAD
                        Main.logger.Err($"CoroutineHelper.Instance: Error creating instance: {ex.Message}");
=======
                        Main.logger.Err($"Error creating CoroutineHelper instance: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                        return null;
                    }
                }
            }
            // Static wrapper to safely start coroutines from anywhere
            public static void RunCoroutine(System.Collections.IEnumerator routine)
            {
                try
                {
                    if (routine == null)
                    {
<<<<<<< HEAD
                        Main.logger.Warn(1, "CoroutineHelper.RunCoroutine: Routine is null");
=======
                        Main.logger.Warn(1, "RunCoroutine: routine is null");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                        return;
                    }

                    var instance = Instance;
                    if (instance != null)
                    {
<<<<<<< HEAD
                        instance.StartCoroutine(routine);
                    }
                    else
                    {
                        Main.logger.Warn(1, "CoroutineHelper.RunCoroutine: Instance is null, cannot start coroutine");
=======
                        instance.StartCoroutine(SafeCoroutineWrapper(routine));
                    }
                    else
                    {
                        Main.logger.Err("CoroutineHelper instance is null, cannot start coroutine");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                    }
                }
                catch (Exception ex)
                {
<<<<<<< HEAD
                    Main.logger.Err($"CoroutineHelper.RunCoroutine: Error starting coroutine: {ex.Message}");
=======
                    Main.logger.Err($"Error in RunCoroutine: {ex}");
                }
            }

            private static System.Collections.IEnumerator SafeCoroutineWrapper(System.Collections.IEnumerator routine)
            {
                if (routine == null) yield break;

                bool hasMore = true;
                while (hasMore)
                {
                    try
                    {
                        hasMore = routine.MoveNext();
                        if (hasMore)
                        {
                            yield return routine.Current;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Err($"Coroutine continue failure in SafeCoroutineWrapper: {ex}");
                        hasMore = false;
                    }
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
                }
            }
        }
        public static void RunCoroutine(System.Collections.IEnumerator routine)
        {
            try
            {
<<<<<<< HEAD
                if (routine == null)
                {
                    Main.logger.Warn(1, "Utils.RunCoroutine: Routine is null");
                    return;
                }

                CoroutineHelper.Instance?.StartCoroutine(routine);
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Utils.RunCoroutine: Error: {ex.Message}");
=======
                CoroutineHelper.Instance.StartCoroutine(routine);
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Error in Utils.RunCoroutine: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
        }
        public static void PrintFileExistsStatus()
        {
            try
            {
                string saveDir = !string.IsNullOrEmpty(Main.CurrentSavePath)
                    ? Path.GetFullPath(Main.CurrentSavePath)
                    : MelonEnvironment.UserDataDirectory;
                    
                if (string.IsNullOrEmpty(saveDir))
                {
                    Main.logger.Warn(1, "PrintFileExistsStatus: Save directory is null or empty");
                    return;
                }

                string path = NormalizePath(Path.Combine(saveDir, "MixerThresholdSave.json"));
                bool exists = File.Exists(path);
                Main.logger.Msg(3, $"File exists at '{path}': {exists}");
            }
            catch (Exception ex)
            {
                Main.logger.Err($"PrintFileExistsStatus: Error checking file status: {ex.Message}");
            }
        }
        public string GetFullTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        // Appears we are not referencing GetNextID , only ResetIDCounter (from Main())
        // meaning it is never used. We got the ID's to be stable somehow ...

    }
}
