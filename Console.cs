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
    public class Console
    {
        // --- Console and Debug Support ---
        public class MixerConsoleHook : MonoBehaviour
        {
            public static MixerConsoleHook Instance { get; private set; }
            private void Awake()
            {
                if (Instance != null)
                {
                    Destroy(this);
                    return;
                }
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            private void Update()
            {
                // Optional: handle key input for console if needed
            }
            public void OnConsoleCommand(string command)
            {
                if (string.IsNullOrEmpty(command))
                    return;
                if (command.StartsWith("resetmixervalues", System.StringComparison.OrdinalIgnoreCase))
                {
                    Main.userSetValues.Clear();
                    Main.savedMixerValues.Clear();
                    // Optional: Reset the saved JSON file
                    string path = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');
                    if (File.Exists(path))
                        File.Delete(path);
                    // msgLogLevel = 1 because user initiated something, user wants to see results
                    Main.logger.Msg(1, "MixerThreholdMod: All mixer values reset and ID counter restarted.");
                }
                else if (command == "printsavepath")
                {
                    // msgLogLevel = 1 because user initiated something, user wants to see results
                    Main.logger.Msg(1, "Current Save Path: " + (Main.CurrentSavePath ?? "[null]"));
                }
            }
        }
        public static void RegisterConsoleCommandViaReflection()
        {
            try
            {
                GameObject hookObj = new GameObject("MixerConsoleHook");
                MixerConsoleHook hook = hookObj.AddComponent<MixerConsoleHook>();
                Application.logMessageReceived += (condition, stackTrace, type) =>
                {
                    if (type == LogType.Log && condition.StartsWith("Exec: "))
                    {
                        string command = condition.Substring(6); // Strip off "Exec: "
                        hook.OnConsoleCommand(command);
                    }
                };
                Main.logger.Msg(2, "Console command hook initialized.");
            }
            catch (System.Exception ex)
            {
                Main.logger.Err("Failed to register console hook: " + ex);
            }
        }
    }
}
