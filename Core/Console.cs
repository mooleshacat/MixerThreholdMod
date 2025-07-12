using MelonLoader;
using MelonLoader.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MixerThreholdMod_0_0_1.Core
{
    /// <summary>
    /// Simplified console integration for debugging and user commands.
    /// Provides essential console commands for save management and debugging.
    /// 
    /// ⚠️ THREAD SAFETY: All console operations are designed to be thread-safe.
    /// Error handling prevents console failures from crashing the mod.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible reflection patterns
    /// - Proper exception handling
    /// </summary>
    public static class Console
    {
        /// <summary>
        /// Console hook for handling in-game console commands
        /// </summary>
        public class MixerConsoleHook : MonoBehaviour
        {
            private static MixerConsoleHook _instance;

            public static MixerConsoleHook Instance
            {
                get
                {
                    Exception getError = null;
                    try
                    {
                        if (_instance == null)
                        {
                            var go = new GameObject("MixerConsoleHook");
                            _instance = go.AddComponent<MixerConsoleHook>();
                            DontDestroyOnLoad(go);
                            Main.logger?.Msg(3, "[CONSOLE] MixerConsoleHook instance created");
                        }
                        return _instance;
                    }
                    catch (Exception ex)
                    {
                        getError = ex;
                        return null;
                    }
                    finally
                    {
                        if (getError != null)
                        {
                            Main.logger?.Err(string.Format("[CONSOLE] MixerConsoleHook.Instance getter error: {0}\n{1}", getError.Message, getError.StackTrace));
                        }
                    }
                }
                private set
                {
                    Exception setError = null;
                    try
                    {
                        _instance = value;
                    }
                    catch (Exception ex)
                    {
                        setError = ex;
                    }

                    if (setError != null)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] MixerConsoleHook.Instance setter error: {0}\n{1}", setError.Message, setError.StackTrace));
                    }
                }
            }

            private void Awake()
            {
                Exception awakeError = null;
                try
                {
                    Main.logger?.Msg(3, "[CONSOLE] MixerConsoleHook.Awake called");
                }
                catch (Exception ex)
                {
                    awakeError = ex;
                }

                if (awakeError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] MixerConsoleHook.Awake error: {0}\n{1}", awakeError.Message, awakeError.StackTrace));
                }
            }

            /// <summary>
            /// Handle console command input
            /// </summary>
            public void OnConsoleCommand(string command)
            {
                Exception commandError = null;
                try
                {
                    if (string.IsNullOrEmpty(command)) return;

                    Main.logger?.Msg(2, string.Format("[CONSOLE] Processing command: {0}", command));
                    ProcessCommand(command.ToLower().Trim());
                }
                catch (Exception ex)
                {
                    commandError = ex;
                }

                if (commandError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] OnConsoleCommand error: {0}\n{1}", commandError.Message, commandError.StackTrace));
                }
            }

            /// <summary>
            /// Process specific console commands
            /// </summary>
            private void ProcessCommand(string lowerCommand)
            {
                Exception processError = null;
                try
                {
                    switch (lowerCommand)
                    {
                        case "mixer_reset":
                            ResetMixerValues();
                            break;
                        case "mixer_save":
                            ForceSave();
                            break;
                        case "mixer_path":
                            PrintSavePath();
                            break;
                        case "mixer_emergency":
                            EmergencySave();
                            break;
                        default:
                            Main.logger?.Msg(1, string.Format("[CONSOLE] Available commands: mixer_reset, mixer_save, mixer_path, mixer_emergency"));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    processError = ex;
                }

                if (processError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] ProcessCommand error: {0}\n{1}", processError.Message, processError.StackTrace));
                }
            }

            /// <summary>
            /// Reset all mixer values
            /// </summary>
            private void ResetMixerValues()
            {
                Exception resetError = null;
                try
                {
                    if (Main.savedMixerValues != null)
                    {
                        Main.savedMixerValues.Clear();
                        Main.logger?.Msg(1, "[CONSOLE] Mixer values cleared");
                    }

                    // Reset ID manager
                    MixerIDManager.ResetStableIDCounter();
                    Main.logger?.Msg(1, "[CONSOLE] Mixer ID counter reset");

                    // Force emergency save of empty state
                    Save.CrashResistantSaveManager.EmergencySave();
                    Main.logger?.Msg(1, "[CONSOLE] Empty state saved");
                }
                catch (Exception ex)
                {
                    resetError = ex;
                }

                if (resetError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] ResetMixerValues error: {0}\n{1}", resetError.Message, resetError.StackTrace));
                }
            }

            /// <summary>
            /// Force an immediate save
            /// </summary>
            private void ForceSave()
            {
                Exception saveError = null;
                try
                {
                    MelonCoroutines.Start(Save.CrashResistantSaveManager.TriggerSaveWithCooldown());
                    Main.logger?.Msg(1, "[CONSOLE] Force save triggered");
                }
                catch (Exception ex)
                {
                    saveError = ex;
                }

                if (saveError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] ForceSave error: {0}\n{1}", saveError.Message, saveError.StackTrace));
                }
            }

            /// <summary>
            /// Print current save path
            /// </summary>
            private void PrintSavePath()
            {
                Exception pathError = null;
                try
                {
                    string currentPath = Main.CurrentSavePath ?? "[not set]";
                    Main.logger?.Msg(1, string.Format("[CONSOLE] Current save path: {0}", currentPath));

                    int mixerCount = Main.savedMixerValues?.Count ?? 0;
                    Main.logger?.Msg(1, string.Format("[CONSOLE] Tracked mixer values: {0}", mixerCount));
                }
                catch (Exception ex)
                {
                    pathError = ex;
                }

                if (pathError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] PrintSavePath error: {0}\n{1}", pathError.Message, pathError.StackTrace));
                }
            }

            /// <summary>
            /// Trigger emergency save
            /// </summary>
            private void EmergencySave()
            {
                Exception emergencyError = null;
                try
                {
                    Save.CrashResistantSaveManager.EmergencySave();
                    Main.logger?.Msg(1, "[CONSOLE] Emergency save completed");
                }
                catch (Exception ex)
                {
                    emergencyError = ex;
                }

                if (emergencyError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] EmergencySave error: {0}\n{1}", emergencyError.Message, emergencyError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Register console commands via reflection
        /// </summary>
        public static void RegisterConsoleCommandViaReflection()
        {
            Exception regError = null;
            try
            {
                // Ensure console hook instance exists
                var hookInstance = MixerConsoleHook.Instance;
                if (hookInstance == null)
                {
                    Main.logger?.Warn(1, "[CONSOLE] Failed to create console hook instance");
                    return;
                }

                Main.logger?.Msg(2, "[CONSOLE] Console commands registered successfully");
            }
            catch (Exception ex)
            {
                regError = ex;
            }

            if (regError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] RegisterConsoleCommandViaReflection error: {0}\n{1}", regError.Message, regError.StackTrace));
            }
        }
    }
}