using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

<<<<<<< HEAD
namespace MixerThreholdMod_0_0_1
{
    public static class Utils
=======
namespace MixerThreholdMod_1_0_0.Helpers
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
                    if (_instance == null)
                    {
                        var go = new GameObject("CoroutineHelper");
                        _instance = go.AddComponent<CoroutineHelper>();
                        DontDestroyOnLoad(go);
                    }
                    return _instance;
                }
            }
            // Static wrapper to safely start coroutines from anywhere
            public static void RunCoroutine(System.Collections.IEnumerator routine)
            {
                Instance.StartCoroutine(routine);
            }
        }
        public static void RunCoroutine(System.Collections.IEnumerator routine)
        {
            CoroutineHelper.Instance.StartCoroutine(routine);
        }
        public static void PrintFileExistsStatus()
        {
            string saveDir = !string.IsNullOrEmpty(Main.CurrentSavePath)
                ? Path.GetFullPath(Main.CurrentSavePath)
                : MelonEnvironment.UserDataDirectory;
            string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
            bool exists = File.Exists(path);
            Main.logger.Msg(3, string.Format("File exists at '{0}': {1}", path, exists));
        }
        public string GetFullTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }

}