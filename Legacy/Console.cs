

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

﻿using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_0_0_1
{
    public class Console
    {
        // --- Console and Debug Support ---
        public class MixerConsoleHook : MonoBehaviour
        {
            private static MixerConsoleHook _instance;
            private static readonly object _instanceLock = new object();

            public static MixerConsoleHook Instance
            {
                get
                {
                    try
                    {
                        lock (_instanceLock)
                        {
                            return _instance;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err(string.Format("MixerConsoleHook.Instance getter: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                        return null;
                    }
                }
                private set
                {
                    try
                    {
                        lock (_instanceLock)
                        {
                            _instance = value;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err(string.Format("MixerConsoleHook.Instance setter: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                    }
                }
            }

            private void Awake()
            {
                try
                {
                    lock (_instanceLock)
                    {
                        if (_instance != null && _instance != this)
                        {
                            Destroy(this);
                            return;
                        }
                        _instance = this;
                        DontDestroyOnLoad(gameObject);
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("MixerConsoleHook.Awake: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                    Main.logger?.Err(string.Format("MixerConsoleHook.Update: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }

            public void OnConsoleCommand(string command)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        Main.logger?.Warn(2, "OnConsoleCommand: Received null or empty command");
                        return;
                    }

                    // Convert to lowercase for case-insensitive comparison
                    string lowerCommand = command.ToLowerInvariant().Trim();

                    // Run command processing on thread pool to avoid blocking Unity main thread
                    Task.Run(() => ProcessCommand(lowerCommand));
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("OnConsoleCommand: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }

            private void ProcessCommand(string lowerCommand)
            {
                try
                {
                    switch (lowerCommand)
                    {
                        case COMMAND_RESET_MIXER_VALUES:
                            ResetMixerValues();
                            break;

                        case "printsavepath":
                            PrintSavePath();
                            break;

                        default:
                            // Handle commands that start with specific prefixes
                            if (lowerCommand.StartsWith(COMMAND_RESET_MIXER_VALUES))
                            {
                                ResetMixerValues();
                            }
                            else
                            {
                                Main.logger?.Warn(1, string.Format("Unknown console command: {0}", lowerCommand));
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("ProcessCommand: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }

            private void ResetMixerValues()
            {
                try
                {
                    // Note: Main.userSetValues doesn't exist in the provided Main.cs, but keeping for compatibility
                    // if (Main.userSetValues != null)
                    //     Main.userSetValues.Clear();

                    if (Main.savedMixerValues != null)
                        Main.savedMixerValues.Clear();

                    // Reset MixerIDManager - no namespace needed since it's in the same namespace
                    MixerIDManager.ResetStableIDCounter();

                    // Optional: Reset the saved JSON file
                    string path = Path.Combine(MelonEnvironment.UserDataDirectory, MIXER_SAVE_FILENAME).Replace('/', '\\');
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    // msgLogLevel = 1 because user initiated something, user wants to see results
                    Main.logger?.Msg(1, "MixerThreholdMod: All mixer values reset and ID counter restarted.");
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("ResetMixerValues: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }

            private void PrintSavePath()
            {
                try
                {
                    // msgLogLevel = 1 because user initiated something, user wants to see results
                    string currentPath = Main.CurrentSavePath ?? NULL_COMMAND_FALLBACK;
                    Main.logger?.Msg(1, string.Format("Current Save Path: {0}", currentPath));
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("PrintSavePath: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
            }
        }

        public static void RegisterConsoleCommandViaReflection()
        {
            try
            {
                if (MixerConsoleHook.Instance != null)
                {
                    Main.logger?.Warn(2, "Console command hook already initialized");
                    return;
                }

                GameObject hookObj = new GameObject("MixerConsoleHook");
                if (hookObj == null)
                {
                    Main.logger?.Err("Failed to create MixerConsoleHook GameObject");
                    return;
                }

                MixerConsoleHook hook = hookObj.AddComponent<MixerConsoleHook>();
                if (hook == null)
                {
                    Main.logger?.Err("Failed to add MixerConsoleHook component");
                    if (hookObj != null) GameObject.Destroy(hookObj);
                    return;
                }

                Application.logMessageReceived += (condition, stackTrace, type) =>
                {
                    try
                    {
                        if (type == LogType.Log && !string.IsNullOrEmpty(condition) && condition.StartsWith("Exec: "))
                        {
                            string command = condition.Substring(6); // Strip off "Exec: "
                            hook?.OnConsoleCommand(command);
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err(string.Format("Error in console log message handler: {0}", ex.Message));
                    }
                };

                Main.logger?.Msg(2, "Console command hook initialized.");
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("RegisterConsoleCommandViaReflection: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}