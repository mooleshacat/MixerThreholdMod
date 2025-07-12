using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixerThreholdMod_1_0_0.Helpers
{
    public static class UtilityHelpers
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
                        Main.logger?.Err(string.Format("CoroutineHelper.Instance: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                    Main.logger?.Err(string.Format("CoroutineHelper.RunCoroutine: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                        Main.logger?.Err(string.Format("SafeCoroutineWrapper: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                    Main.logger?.Warn(1, "UtilityHelpers.RunCoroutine: routine is null");
                    return;
                }

                var instance = CoroutineHelper.Instance;
                if (instance != null)
                {
                    instance.StartCoroutine(routine);
                }
                else
                {
                    Main.logger?.Err("CoroutineHelper.Instance is null in UtilityHelpers.RunCoroutine");
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("UtilityHelpers.RunCoroutine: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Normalizes a file path by replacing forward slashes with backslashes and ensuring proper formatting
        /// </summary>
        /// <param name="path">The path to normalize</param>
        /// <returns>Normalized path with backslashes</returns>
        public static string NormalizePath(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    Main.logger?.Warn(1, "NormalizePath: Input path is null or empty");
                    return string.Empty;
                }

                // Replace forward slashes with backslashes
                string normalized = path.Replace('/', '\\');

                // Remove any double backslashes
                while (normalized.Contains("\\\\"))
                {
                    normalized = normalized.Replace("\\\\", "\\");
                }

                // Ensure we don't end with a backslash unless it's a root drive
                if (normalized.Length > 3 && normalized.EndsWith("\\"))
                {
                    normalized = normalized.TrimEnd('\\');
                }

                Main.logger?.Msg(3, string.Format("NormalizePath: '{0}' -> '{1}'", path, normalized));
                return normalized;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("NormalizePath: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                return path; // Return original path if normalization fails
            }
        }

        /// <summary>
        /// Ensures a directory exists, creating it if necessary
        /// </summary>
        /// <param name="directoryPath">The directory path to ensure exists</param>
        /// <param name="operationDescription">Description of the operation for logging purposes</param>
        /// <returns>True if directory exists or was created successfully, false otherwise</returns>
        public static bool EnsureDirectoryExists(string directoryPath, string operationDescription = "directory creation")
        {
            try
            {
                if (string.IsNullOrEmpty(directoryPath))
                {
                    Main.logger?.Warn(1, string.Format("EnsureDirectoryExists: Directory path is null or empty for operation: {0}", operationDescription));
                    return false;
                }

                if (Directory.Exists(directoryPath))
                {
                    Main.logger?.Msg(3, string.Format("EnsureDirectoryExists: Directory already exists for {0}: {1}", operationDescription, directoryPath));
                    return true;
                }

                try
                {
                    Directory.CreateDirectory(directoryPath);
                    Main.logger?.Msg(2, string.Format("EnsureDirectoryExists: Created directory for {0}: {1}", operationDescription, directoryPath));
                    return true;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Main.logger?.Err(string.Format("EnsureDirectoryExists: Access denied creating directory for {0} [{1}]: {2}", operationDescription, directoryPath, ex.Message));
                    return false;
                }
                catch (IOException ex)
                {
                    Main.logger?.Err(string.Format("EnsureDirectoryExists: IO error creating directory for {0} [{1}]: {2}", operationDescription, directoryPath, ex.Message));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("EnsureDirectoryExists: Caught exception for {0} [{1}]: {2}\n{3}", operationDescription, directoryPath, ex.Message, ex.StackTrace));
                return false;
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
                Main.logger?.Msg(3, string.Format("File exists at '{0}': {1}", path, exists));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("PrintFileExistsStatus: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                Main.logger?.Err(string.Format("GetFullTimestamp: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                Main.logger?.Err(string.Format("SafeFileExists: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                Main.logger?.Err(string.Format("SafeDirectoryExists: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}