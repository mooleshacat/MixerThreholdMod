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
                            if (go != null)
                            {
                                _instance = go.AddComponent<CoroutineHelper>();
                                if (_instance != null)
                                {
                                    DontDestroyOnLoad(go);
                                }
                            }
                        }
                        return _instance;
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Err($"CoroutineHelper.Instance: Error creating instance: {ex.Message}");
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
                        Main.logger.Warn(1, "CoroutineHelper.RunCoroutine: Routine is null");
                        return;
                    }

                    var instance = Instance;
                    if (instance != null)
                    {
                        instance.StartCoroutine(routine);
                    }
                    else
                    {
                        Main.logger.Warn(1, "CoroutineHelper.RunCoroutine: Instance is null, cannot start coroutine");
                    }
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"CoroutineHelper.RunCoroutine: Error starting coroutine: {ex.Message}");
                }
            }
        }
        public static void RunCoroutine(System.Collections.IEnumerator routine)
        {
            try
            {
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

                string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
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
