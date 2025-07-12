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
    public static class Utils
    {
        public class CoroutineHelper : MonoBehaviour
        {
            private static CoroutineHelper _instance;
            private static readonly object _instanceLock = new object();

            public static CoroutineHelper Instance
            {
                get
                {
                    try
                    {
                        lock (_instanceLock)
                        {
                            if (_instance == null)
                            {
                                var go = new GameObject("CoroutineHelper");
                                if (go != null)
                                {
                                    _instance = go.AddComponent<CoroutineHelper>();
                                    if (_instance != null)
                                    {
                                        DontDestroyOnLoad(go);
                                    }
                                    else
                                    {
                                        Main.logger?.Err("Failed to add CoroutineHelper component");
                                        if (go != null) Destroy(go);
                                    }
                                }
                                else
                                {
                                    Main.logger?.Err("Failed to create CoroutineHelper GameObject");
                                }
                            }
                            return _instance;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err($"CoroutineHelper.Instance: Caught exception: {ex.Message}\n{ex.StackTrace}");
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
                        Main.logger?.Warn(1, "RunCoroutine: routine is null");
                        return;
                    }

                    var instance = Instance;
                    if (instance != null)
                    {
                        instance.StartCoroutine(SafeCoroutineWrapper(routine));
                    }
                    else
                    {
                        Main.logger?.Err("CoroutineHelper instance is null, cannot start coroutine");
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Err($"CoroutineHelper.RunCoroutine: Caught exception: {ex.Message}\n{ex.StackTrace}");
                }
            }

            private static System.Collections.IEnumerator SafeCoroutineWrapper(System.Collections.IEnumerator routine)
            {
                if (routine == null)
                {
                    Main.logger?.Warn(1, "SafeCoroutineWrapper: routine is null");
                    yield break;
                }

                bool hasMore = true;
                while (hasMore)
                {
                    object current = null;
                    try
                    {
                        hasMore = routine.MoveNext();
                        if (hasMore)
                        {
                            current = routine.Current;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err($"SafeCoroutineWrapper: Caught exception: {ex.Message}\n{ex.StackTrace}");
                        hasMore = false;
                    }
                    if (hasMore)
                    {
                        yield return current;
                    }
                }
            }
        }

        public static void RunCoroutine(System.Collections.IEnumerator routine)
        {
            try
            {
                if (routine == null)
                {
                    Main.logger?.Warn(1, "Utils.RunCoroutine: routine is null");
                    return;
                }

                var instance = CoroutineHelper.Instance;
                if (instance != null)
                {
                    instance.StartCoroutine(routine);
                }
                else
                {
                    Main.logger?.Err("CoroutineHelper.Instance is null in Utils.RunCoroutine");
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"Utils.RunCoroutine: Caught exception: {ex.Message}\n{ex.StackTrace}");
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
                    Main.logger?.Warn(1, "PrintFileExistsStatus: Unable to determine save directory");
                    return;
                }

                string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
                bool exists = File.Exists(path);
                Main.logger?.Msg(3, $"File exists at '{path}': {exists}");
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"PrintFileExistsStatus: Caught exception: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static string GetFullTimestamp()
        {
            try
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"GetFullTimestamp: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return "[TIMESTAMP_ERROR]";
            }
        }

        // Thread-safe file existence check
        public static bool SafeFileExists(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                return File.Exists(path);
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"SafeFileExists: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        // Thread-safe directory existence check
        public static bool SafeDirectoryExists(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                return Directory.Exists(path);
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"SafeDirectoryExists: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }
    }
}