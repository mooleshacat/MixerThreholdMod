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
                try
                {
                    if (Instance != null)
                    {
                        Destroy(this);
                        return;
                    }
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"MixerConsoleHook.Awake: Error: {ex.Message}");
                }
            }
            private void Update()
            {
                try
                {
                    // Optional: handle key input for console if needed
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"MixerConsoleHook.Update: Error: {ex.Message}");
                }
            }
            public void OnConsoleCommand(string command)
            {
                try
                {
                    if (string.IsNullOrEmpty(command))
                        return;
                        
                    if (command.StartsWith("resetmixervalues", System.StringComparison.OrdinalIgnoreCase))
                    {
                        if (Main.userSetValues != null)
                        {
                            Main.userSetValues.Clear();
                        }
                        
                        if (Main.savedMixerValues != null)
                        {
                            Main.savedMixerValues.Clear();
                        }
                        
                        // Optional: Reset the saved JSON file
                        try
                        {
                            string path = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');
                            if (File.Exists(path))
                                File.Delete(path);
                        }
                        catch (Exception fileEx)
                        {
                            Main.logger.Err($"OnConsoleCommand: Error deleting save file: {fileEx.Message}");
                        }
                        
                        // msgLogLevel = 1 because user initiated something, user wants to see results
                        Main.logger.Msg(1, "MixerThreholdMod: All mixer values reset and ID counter restarted.");
                    }
                    else if (command == "printsavepath")
                    {
                        // msgLogLevel = 1 because user initiated something, user wants to see results
                        Main.logger.Msg(1, "Current Save Path: " + (Main.CurrentSavePath ?? "[null]"));
                    }
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"OnConsoleCommand: Error processing command '{command}': {ex.Message}");
                }
            }
        }
        public static void RegisterConsoleCommandViaReflection()
        {
            try
            {
                Main.logger.Msg(3, "RegisterConsoleCommandViaReflection: Starting console hook initialization");
                
                GameObject hookObj = new GameObject("MixerConsoleHook");
                if (hookObj == null)
                {
                    Main.logger.Err("RegisterConsoleCommandViaReflection: Failed to create GameObject");
                    return;
                }

                MixerConsoleHook hook = hookObj.AddComponent<MixerConsoleHook>();
                if (hook == null)
                {
                    Main.logger.Err("RegisterConsoleCommandViaReflection: Failed to add MixerConsoleHook component");
                    return;
                }

                Application.logMessageReceived += (condition, stackTrace, type) =>
                {
                    try
                    {
                        if (type == LogType.Log && !string.IsNullOrEmpty(condition) && condition.StartsWith("Exec: "))
                        {
                            string command = condition.Substring(6); // Strip off "Exec: "
                            if (hook != null)
                            {
                                hook.OnConsoleCommand(command);
                            }
                        }
                    }
                    catch (Exception logEx)
                    {
                        Main.logger.Err($"RegisterConsoleCommandViaReflection: Error in log message handler: {logEx.Message}");
                    }
                };
                
                Main.logger.Msg(2, "Console command hook initialized.");
            }
            catch (System.Exception ex)
            {
                Main.logger.Err("Failed to register console hook: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
