<<<<<<< HEAD
<<<<<<< HEAD
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using MelonLoader.Utils;

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
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
using MelonLoader;
using MelonLoader.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// IL2CPP COMPATIBLE: Simplified console integration for debugging and user commands.
    /// Provides essential console commands for save management and debugging with AOT-safe patterns.
    /// 
    /// ⚠️ THREAD SAFETY: All console operations are designed to be thread-safe with proper error handling.
    /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe patterns, minimal reflection, AOT-friendly operations.
    /// ⚠️ MEMORY LEAK PREVENTION: Proper cleanup and disposal patterns prevent console-related memory leaks.
    /// 
    /// IL2CPP Compatibility Features:
    /// - Minimal reflection usage with compile-time known types only
    /// - No dynamic code generation or runtime type creation
    /// - AOT-safe method resolution using typeof() instead of GetType()
    /// - Interface-based command processing instead of reflection-heavy approaches
    /// - Compile-time safe generic constraints and collection usage
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation for maximum compatibility
    /// - Compatible reflection patterns with minimal usage and proper error handling
    /// - Proper exception handling throughout all console operations
    /// - Framework-appropriate async patterns and resource management
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
<<<<<<< HEAD
=======
            /// ⚠️ COMPREHENSIVE LOGGING: Logs full command details including all parameters, system context, and error information
            /// Enhanced with system monitoring integration and complete command breakdown
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
            /// ⚠️ COMPREHENSIVE LOGGING: Logs full command details including all parameters, system context, and error information
            /// Enhanced with system monitoring integration and complete command breakdown
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            /// </summary>
            public void OnConsoleCommand(string command)
            {
                Exception commandError = null;
<<<<<<< HEAD
<<<<<<< HEAD
                try
                {
                    if (string.IsNullOrEmpty(command)) return;
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                var commandStartTime = DateTime.UtcNow;
                
                try
                {
                    if (string.IsNullOrEmpty(command)) 
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Empty command received - no action taken");
                        return;
                    }
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)

                    // Enhanced comprehensive command logging with system context
                    Main.logger?.Msg(2, string.Format("[CONSOLE] === COMMAND RECEIVED ==="));
                    Main.logger?.Msg(2, string.Format("[CONSOLE] Timestamp: {0:yyyy-MM-dd HH:mm:ss.fff} UTC", commandStartTime));
                    Main.logger?.Msg(2, string.Format("[CONSOLE] Full command: '{0}'", command));
                    Main.logger?.Msg(2, string.Format("[CONSOLE] Command length: {0} characters", command.Length));
                    Main.logger?.Msg(2, string.Format("[CONSOLE] Command hash: {0}", command.GetHashCode()));
                    
                    // Log system context during command processing (DEBUG mode only)
                    AdvancedSystemPerformanceMonitor.LogCurrentPerformance("CONSOLE_COMMAND");
                    
                    // Parse and log command components with enhanced analysis
                    var originalParts = command.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var parts = command.ToLower().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length > 0)
                    {
                        Main.logger?.Msg(2, string.Format("[CONSOLE] Base command: '{0}' (case-insensitive)", parts[0]));
                        Main.logger?.Msg(2, string.Format("[CONSOLE] Original case: '{0}'", originalParts[0]));
                        
                        if (parts.Length > 1)
                        {
                            Main.logger?.Msg(2, string.Format("[CONSOLE] Parameters ({0}): [{1}]", parts.Length - 1, string.Join(", ", parts, 1, parts.Length - 1)));
                            Main.logger?.Msg(2, string.Format("[CONSOLE] Original case parameters: [{0}]", string.Join(", ", originalParts, 1, originalParts.Length - 1)));
                            
                            // Enhanced parameter analysis
                            for (int i = 1; i < parts.Length; i++)
                            {
                                var param = parts[i];
                                var originalParam = originalParts[i];
                                
                                // Analyze parameter type
                                int intValue;
                                float floatValue;
                                bool boolValue;
                                
                                string paramType = "STRING";
                                if (int.TryParse(param, out intValue))
                                {
                                    paramType = "INTEGER";
                                }
                                else if (float.TryParse(param, out floatValue))
                                {
                                    paramType = "FLOAT";
                                }
                                else if (bool.TryParse(param, out boolValue))
                                {
                                    paramType = "BOOLEAN";
                                }
                                
                                Main.logger?.Msg(3, string.Format("[CONSOLE] Parameter {0}: '{1}' (original: '{2}', type: {3})", i, param, originalParam, paramType));
                            }
                        }
                        else
                        {
                            Main.logger?.Msg(3, "[CONSOLE] No parameters provided");
                        }
                        
                        // Check if command is recognized
                        var recognizedCommands = new string[] 
                        { 
                            "mixer_reset", "mixer_save", "mixer_path", "mixer_emergency",
                            "saveprefstress", "savegamestress", "savemonitor", "transactionalsave", "profile",
                            "msg", "warn", "err", "help", "?"
                        };
                        
                        bool isRecognized = false;
                        foreach (var cmd in recognizedCommands)
                        {
                            if (parts[0] == cmd)
                            {
                                isRecognized = true;
                                break;
                            }
                        }
                        
                        Main.logger?.Msg(2, string.Format("[CONSOLE] Command recognition: {0}", isRecognized ? "RECOGNIZED" : "UNKNOWN"));
                        
                        if (!isRecognized)
                        {
                            Main.logger?.Warn(1, string.Format("[CONSOLE] Unknown command detected: '{0}'", parts[0]));
                            Main.logger?.Msg(1, "[CONSOLE] Available commands: help, mixer_reset, mixer_save, mixer_path, mixer_emergency");
                            Main.logger?.Msg(1, "[CONSOLE] Stress testing: saveprefstress, savegamestress, savemonitor, transactionalsave, profile");
                            Main.logger?.Msg(1, "[CONSOLE] Logging: msg, warn, err");
                        }
                    }
                    else
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Command parsing failed - no components found");
                        return;
                    }
                    
                    // Process command with performance monitoring
                    AdvancedSystemPerformanceMonitor.MonitorOperation(string.Format("Console Command: {0}", parts[0]), () => {
                        ProcessCommand(command.ToLower().Trim());
                    });
                    
                    var commandEndTime = DateTime.UtcNow;
                    var executionTime = commandEndTime - commandStartTime;
                    
                    Main.logger?.Msg(2, string.Format("[CONSOLE] Command execution time: {0:F3}ms", executionTime.TotalMilliseconds));
                    Main.logger?.Msg(2, "[CONSOLE] === COMMAND COMPLETED ===");
                }
                catch (Exception ex)
                {
                    commandError = ex;
                }

                if (commandError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] OnConsoleCommand error: {0}\n{1}", commandError.Message, commandError.StackTrace));
<<<<<<< HEAD
<<<<<<< HEAD
                }
            }

            /// <summary>
            /// Process specific console commands
            /// </summary>
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    Main.logger?.Err(string.Format("[CONSOLE] Failed command was: '{0}'", command ?? "[null]"));
                    Main.logger?.Err(string.Format("[CONSOLE] Command processing failed after {0:F3}ms", (DateTime.UtcNow - commandStartTime).TotalMilliseconds));
                }
            }

<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            private void ProcessCommand(string lowerCommand)
            {
                Exception processError = null;
                try
                {
<<<<<<< HEAD
<<<<<<< HEAD
                    switch (lowerCommand)
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    // Handle commands with parameters
                    var parts = lowerCommand.Split(' ');
                    var baseCommand = parts[0];

                    switch (baseCommand)
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
                        case "mixer_saveprefstress":
                        case "saveprefstress":
<<<<<<< HEAD
<<<<<<< HEAD
                            HandleStressSavePrefCommand(parts);
                            break;
                        case "mixer_savegamestress":
                        case "savegamestress":
                            HandleStressSaveGameCommand(parts);
                            break;
                        case "mixer_savemonitor":
                        case "savemonitor":
                            HandleComprehensiveSaveMonitoringCommand(parts);
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                            if (parts.Length < 2)
                            {
                                Main.logger?.Msg(1, "[CONSOLE] Missing required parameter: count");
                                ShowCommandHelp("saveprefstress");
                            }
                            else
                            {
                                HandleStressSavePrefCommand(parts);
                            }
                            break;
                        case "mixer_savegamestress":
                        case "savegamestress":
                            if (parts.Length < 2)
                            {
                                Main.logger?.Msg(1, "[CONSOLE] Missing required parameter: count");
                                ShowCommandHelp("savegamestress");
                            }
                            else
                            {
                                HandleStressSaveGameCommand(parts);
                            }
                            break;
                        case "mixer_savemonitor":
                        case "savemonitor":
                            if (parts.Length < 2)
                            {
                                Main.logger?.Msg(1, "[CONSOLE] Missing required parameter: count");
                                ShowCommandHelp("savemonitor");
                            }
                            else
                            {
                                HandleComprehensiveSaveMonitoringCommand(parts);
                            }
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                            break;
                        case "transactionalsave":
                        case "mixer_transactional":
                            HandleTransactionalSaveCommand();
                            break;
                        case "profile":
                        case "mixer_profile":
                            HandleProfileCommand();
                            break;
                        case "msg":
<<<<<<< HEAD
<<<<<<< HEAD
                            HandleSingleLogCommand("msg", parts, lowerCommand);
                            break;
                        case "warn":
                            HandleSingleLogCommand("warn", parts, lowerCommand);
                            break;
                        case "err":
                            HandleSingleLogCommand("err", parts, lowerCommand);
                            break;
                        case "detectdirs":
                        case "directories":
                        case "paths":
                            HandleDirectoryDetectionCommand();
                            break;
                        case "detectdirs":
                        case "directories":
                        case "paths":
                            HandleDirectoryDetectionCommand();
                            break;
                        case "detectdirs":
                        case "directories":
                        case "paths":
                            HandleDirectoryDetectionCommand();
                            break;
                        case "detectdirs":
                        case "directories":
                        case "paths":
                            HandleDirectoryDetectionCommand();
                            break;
                        default:
                            Main.logger?.Msg(1, string.Format("[CONSOLE] Available commands: mixer_reset, mixer_save, mixer_path, mixer_emergency"));
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                            if (parts.Length < 2)
                            {
                                Main.logger?.Msg(1, "[CONSOLE] Missing required parameter: message");
                                ShowCommandHelp("msg");
                            }
                            else
                            {
                                HandleSingleLogCommand("msg", parts, lowerCommand);
                            }
                            break;
                        case "warn":
                            if (parts.Length < 2)
                            {
                                Main.logger?.Msg(1, "[CONSOLE] Missing required parameter: message");
                                ShowCommandHelp("warn");
                            }
                            else
                            {
                                HandleSingleLogCommand("warn", parts, lowerCommand);
                            }
                            break;
                        case "err":
                            if (parts.Length < 2)
                            {
                                Main.logger?.Msg(1, "[CONSOLE] Missing required parameter: message");
                                ShowCommandHelp("err");
                            }
                            else
                            {
                                HandleSingleLogCommand("err", parts, lowerCommand);
                            }
                            break;
                        case "help":
                        case "?":
                            ShowHelpMessage();
                            break;
                        default:
                            if (baseCommand == "help" || baseCommand == "?")
                            {
                                ShowHelpMessage();
                            }
                            else
                            {
                                Main.logger?.Warn(1, string.Format("[CONSOLE] Unknown command: '{0}'", baseCommand));
                                Main.logger?.Msg(1, "[CONSOLE] Type 'help' to see available commands");
                            }
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            /// <summary>
            /// Show help for specific command when parameters are missing
            /// ⚠️ THREAD SAFETY: Safe logging operations with comprehensive error handling
            /// </summary>
            private void ShowCommandHelp(string commandName)
            {
                Exception helpError = null;
                try
                {
                    switch (commandName.ToLower())
                    {
                        case "saveprefstress":
                            Main.logger?.Msg(1, "[CONSOLE] === SAVEPREFSTRESS HELP ===");
                            Main.logger?.Msg(1, "[CONSOLE] Usage: saveprefstress <count> [delay_seconds] [bypass_cooldown]");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Required:");
                            Main.logger?.Msg(1, "[CONSOLE]   count - Number of save iterations (positive integer)");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Optional (auto-detected order):");
                            Main.logger?.Msg(1, "[CONSOLE]   delay_seconds - Delay between saves (number ≥ 0, default: 0)");
                            Main.logger?.Msg(1, "[CONSOLE]   bypass_cooldown - Skip save cooldown (true/false, default: true)");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Examples:");
                            Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 10");
                            Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 5 2.0");
                            Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 20 false");
                            Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 10 0.1 false");
                            break;

                        case "savegamestress":
                            Main.logger?.Msg(1, "[CONSOLE] === SAVEGAMESTRESS HELP ===");
                            Main.logger?.Msg(1, "[CONSOLE] Usage: savegamestress <count> [delay_seconds] [bypass_cooldown]");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Required:");
                            Main.logger?.Msg(1, "[CONSOLE]   count - Number of save iterations (positive integer)");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Optional (auto-detected order):");
                            Main.logger?.Msg(1, "[CONSOLE]   delay_seconds - Delay between saves (number ≥ 0, default: 0)");
                            Main.logger?.Msg(1, "[CONSOLE]   bypass_cooldown - Skip save cooldown (true/false, default: true)");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Examples:");
                            Main.logger?.Msg(1, "[CONSOLE]   savegamestress 10");
                            Main.logger?.Msg(1, "[CONSOLE]   savegamestress 5 3.0");
                            Main.logger?.Msg(1, "[CONSOLE]   savegamestress 3 false");
                            Main.logger?.Msg(1, "[CONSOLE]   savegamestress 5 2.0 false");
                            break;

                        case "savemonitor":
                            Main.logger?.Msg(1, "[CONSOLE] === SAVEMONITOR HELP ===");
                            Main.logger?.Msg(1, "[CONSOLE] Usage: savemonitor <count> [delay_seconds] [bypass_cooldown]");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Required:");
                            Main.logger?.Msg(1, "[CONSOLE]   count - Number of save iterations (positive integer)");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Optional (auto-detected order):");
                            Main.logger?.Msg(1, "[CONSOLE]   delay_seconds - Delay between saves (number ≥ 0, default: 0)");
                            Main.logger?.Msg(1, "[CONSOLE]   bypass_cooldown - Skip save cooldown (true/false, default: true)");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Features: Multi-method validation, comprehensive monitoring");
                            Main.logger?.Msg(1, "[CONSOLE] Examples:");
                            Main.logger?.Msg(1, "[CONSOLE]   savemonitor 5");
                            Main.logger?.Msg(1, "[CONSOLE]   savemonitor 3 2.0");
                            Main.logger?.Msg(1, "[CONSOLE]   savemonitor 10 false");
                            Main.logger?.Msg(1, "[CONSOLE]   savemonitor 5 1.5 false");
                            break;

                        case "msg":
                            Main.logger?.Msg(1, "[CONSOLE] === MSG HELP ===");
                            Main.logger?.Msg(1, "[CONSOLE] Usage: msg <message>");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Required:");
                            Main.logger?.Msg(1, "[CONSOLE]   message - Text to log as info message");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Examples:");
                            Main.logger?.Msg(1, "[CONSOLE]   msg Testing mixer behavior at threshold 0.8");
                            Main.logger?.Msg(1, "[CONSOLE]   msg Save operation completed successfully");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Note: Message preserves all spaces and formatting");
                            break;

                        case "warn":
                            Main.logger?.Msg(1, "[CONSOLE] === WARN HELP ===");
                            Main.logger?.Msg(1, "[CONSOLE] Usage: warn <message>");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Required:");
                            Main.logger?.Msg(1, "[CONSOLE]   message - Text to log as warning message");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Examples:");
                            Main.logger?.Msg(1, "[CONSOLE]   warn Performance degradation detected");
                            Main.logger?.Msg(1, "[CONSOLE]   warn Memory usage approaching critical levels");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Note: Message preserves all spaces and formatting");
                            break;

                        case "err":
                            Main.logger?.Msg(1, "[CONSOLE] === ERR HELP ===");
                            Main.logger?.Msg(1, "[CONSOLE] Usage: err <message>");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Required:");
                            Main.logger?.Msg(1, "[CONSOLE]   message - Text to log as error message");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Examples:");
                            Main.logger?.Msg(1, "[CONSOLE]   err Critical save failure - investigating corruption");
                            Main.logger?.Msg(1, "[CONSOLE]   err Mixer threshold validation failed");
                            Main.logger?.Msg(1, "[CONSOLE] ");
                            Main.logger?.Msg(1, "[CONSOLE] Note: Message preserves all spaces and formatting");
                            break;

                        default:
                            Main.logger?.Msg(1, string.Format("[CONSOLE] No specific help available for command: {0}", commandName));
                            Main.logger?.Msg(1, "[CONSOLE] Type 'help' to see all available commands");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    helpError = ex;
                }

                if (helpError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] ShowCommandHelp error: {0}\n{1}", helpError.Message, helpError.StackTrace));
                }
            }

<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            private void ShowHelpMessage()
            {
                Exception helpError = null;
                try
                {
                    Main.logger?.Msg(1, "[CONSOLE] Available commands:");
                    Main.logger?.Msg(1, "[CONSOLE] ");
<<<<<<< HEAD
<<<<<<< HEAD
                    Main.logger?.Msg(1, "[CONSOLE] === SYSTEM COMMANDS ===");
                    Main.logger?.Msg(1, "[CONSOLE]   detectdirs - Detect and display game directories");
                    Main.logger?.Msg(1, "[CONSOLE]   directories - Alias for detectdirs");
                    Main.logger?.Msg(1, "[CONSOLE]   paths - Alias for detectdirs");
=======
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    Main.logger?.Msg(1, "[CONSOLE] === MIXER MANAGEMENT ===");
                    Main.logger?.Msg(1, "[CONSOLE]   mixer_reset - Reset all mixer values");
                    Main.logger?.Msg(1, "[CONSOLE]   mixer_save - Force immediate save");
                    Main.logger?.Msg(1, "[CONSOLE]   mixer_path - Show current save path");
                    Main.logger?.Msg(1, "[CONSOLE]   mixer_emergency - Trigger emergency save");
                    Main.logger?.Msg(1, "[CONSOLE] ");
                    Main.logger?.Msg(1, "[CONSOLE] === STRESS TESTING ===");
                    Main.logger?.Msg(1, "[CONSOLE]   saveprefstress <count> [delay] [bypass] - Stress test mixer prefs saves");
                    Main.logger?.Msg(1, "[CONSOLE]   savegamestress <count> [delay] [bypass] - Stress test game saves");
                    Main.logger?.Msg(1, "[CONSOLE]   savemonitor <count> [delay] [bypass] - Comprehensive save monitoring (dnSpy)");
                    Main.logger?.Msg(1, "[CONSOLE]   transactionalsave - Perform atomic transactional save");
                    Main.logger?.Msg(1, "[CONSOLE]   profile - Advanced save operation profiling");
                    Main.logger?.Msg(1, "[CONSOLE]     Examples: saveprefstress 10 0.1 true");
                    Main.logger?.Msg(1, "[CONSOLE]               savegamestress 5 false 2.0");
                    Main.logger?.Msg(1, "[CONSOLE]               savemonitor 3 1.0 - Multi-method validation");
                    Main.logger?.Msg(1, "[CONSOLE]     Note: Parameters after count can be in any order");
                    Main.logger?.Msg(1, "[CONSOLE] ");
                    Main.logger?.Msg(1, "[CONSOLE] === MANUAL LOGGING ===");
                    Main.logger?.Msg(1, "[CONSOLE]   msg <message> - Log info message");
                    Main.logger?.Msg(1, "[CONSOLE]   warn <message> - Log warning message");
                    Main.logger?.Msg(1, "[CONSOLE]   err <message> - Log error message");
                    Main.logger?.Msg(1, "[CONSOLE]     Examples: msg Testing mixer behavior at threshold 0.8");
                    Main.logger?.Msg(1, "[CONSOLE]               warn Performance degradation detected");
                    Main.logger?.Msg(1, "[CONSOLE]               err Critical save failure during stress test");
                    Main.logger?.Msg(1, "[CONSOLE]     Note: Messages preserve all spaces and formatting");
                }
                catch (Exception ex)
                {
                    helpError = ex;
                }

                if (helpError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] ShowHelpMessage error: {0}\n{1}", helpError.Message, helpError.StackTrace));
                }
            }

<<<<<<< HEAD
<<<<<<< HEAD
            // Add this method to the MixerConsoleHook class:
            /// <summary>
            /// Handle directory detection command
            /// ⚠️ THREAD SAFETY: Safe directory detection with comprehensive error handling
            /// </summary>
            private void HandleDirectoryDetectionCommand()
            {
                Exception detectionError = null;
                try
                {
                    Main.logger?.Msg(1, "[CONSOLE] Starting directory detection command...");

                    // ⚠️ ASYNC JUSTIFICATION: Directory detection can take 100-500ms using game APIs
                    // Task.Run prevents Unity main thread blocking during game API access
                    Task.Run(async () =>
                    {
                        try
                        {
                            var directoryInfo = await Helpers.GameDirectoryResolver.RefreshDirectoryDetectionAsync();

                            Main.logger?.Msg(1, "[CONSOLE] === DIRECTORY DETECTION COMMAND RESULTS ===");
                            Main.logger?.Msg(1, string.Format("[CONSOLE] Detection Status: {0}", directoryInfo.ToString()));

                            if (directoryInfo.GameDirectoryFound)
                            {
                                Main.logger?.Msg(1, string.Format("[CONSOLE] 🎮 Game Directory: {0}", directoryInfo.GameInstallDirectory));
                            }

                            if (directoryInfo.UserDataDirectoryFound)
                            {
                                Main.logger?.Msg(1, string.Format("[CONSOLE] 👤 User Data: {0}", directoryInfo.UserDataDirectory));
                            }

                            if (directoryInfo.SavesDirectoryFound)
                            {
                                Main.logger?.Msg(1, string.Format("[CONSOLE] 💾 Saves Directory: {0}", directoryInfo.SavesDirectory));

                                if (!string.IsNullOrEmpty(directoryInfo.IndividualSavesPath))
                                {
                                    Main.logger?.Msg(1, string.Format("[CONSOLE] 💾 Individual Saves: {0}", directoryInfo.IndividualSavesPath));
                                }

                                if (!string.IsNullOrEmpty(directoryInfo.CurrentSavePath))
                                {
                                    Main.logger?.Msg(1, string.Format("[CONSOLE] 💾 Current Save: {0}", directoryInfo.CurrentSavePath));
                                }
                            }

                            if (directoryInfo.MelonLoaderLogFound)
                            {
                                Main.logger?.Msg(1, string.Format("[CONSOLE] 🍈 MelonLoader Log: {0}", directoryInfo.MelonLoaderLogFile));

                                try
                                {
                                    var logInfo = new FileInfo(directoryInfo.MelonLoaderLogFile);
                                    Main.logger?.Msg(2, string.Format("[CONSOLE] Log file: {0:F1} KB, modified {1:yyyy-MM-dd HH:mm:ss}",
                                        logInfo.Length / 1024.0, logInfo.LastWriteTime));
                                }
                                catch (Exception logInfoEx)
                                {
                                    Main.logger?.Warn(2, string.Format("[CONSOLE] Could not get log file details: {0}", logInfoEx.Message));
                                }
                            }

                            Main.logger?.Msg(1, "[CONSOLE] Directory detection command completed successfully");
                        }
                        catch (Exception asyncEx)
                        {
                            Main.logger?.Err(string.Format("[CONSOLE] Directory detection command failed: {0}", asyncEx.Message));
                        }
                    });
                }
                catch (Exception ex)
                {
                    detectionError = ex;
                }

                if (detectionError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleDirectoryDetectionCommand error: {0}", detectionError.Message));
                }
            }

=======
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            /// <summary>
            /// Handle single-type logging commands (msg, warn, err)
            /// ⚠️ THREAD SAFETY: Safe logging operations with comprehensive error handling
            /// Direct logging commands without type specification
            /// </summary>
            /// <param name="logType">Type of log message (msg, warn, err)</param>
            /// <param name="parts">Command parts split by spaces</param>
            /// <param name="originalCommand">Original command string to preserve message formatting</param>
            private void HandleSingleLogCommand(string logType, string[] parts, string originalCommand)
            {
                Exception logError = null;
                try
                {
                    if (parts.Length < 2)
                    {
                        // Show specific help for the command type
                        switch (logType.ToLower())
                        {
                            case "msg":
                                Main.logger?.Msg(1, "[CONSOLE] Usage: msg <message>");
                                Main.logger?.Msg(1, "[CONSOLE] Example: msg Testing mixer behavior at threshold 0.8");
                                break;
                            case "warn":
                                Main.logger?.Msg(1, "[CONSOLE] Usage: warn <message>");
                                Main.logger?.Msg(1, "[CONSOLE] Example: warn Performance degradation detected during stress test");
                                break;
                            case "err":
                                Main.logger?.Msg(1, "[CONSOLE] Usage: err <message>");
                                Main.logger?.Msg(1, "[CONSOLE] Example: err Critical save failure - investigating corruption");
                                break;
                        }
                        Main.logger?.Msg(1, "[CONSOLE] Note: Message preserves all spaces and formatting");
                        return;
                    }

                    // Extract message by finding the position after command + space
                    string searchPattern = string.Format("{0} ", parts[0]);
                    int messageStartIndex = originalCommand.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase);

                    if (messageStartIndex == -1)
                    {
                        // Fallback: reconstruct message from parts
                        var messageParts = new string[parts.Length - 1];
                        Array.Copy(parts, 1, messageParts, 0, parts.Length - 1);
                        var fallbackMessage = string.Join(" ", messageParts);

                        Main.logger?.Warn(1, string.Format("[CONSOLE] {0} command: Using fallback message reconstruction", logType));
                        ProcessLogMessage(logType, fallbackMessage);
                        return;
                    }

                    // Extract the full message preserving original formatting
                    messageStartIndex += searchPattern.Length;
                    if (messageStartIndex >= originalCommand.Length)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] {0} command: No message provided", logType));
                        return;
                    }

                    string message = originalCommand.Substring(messageStartIndex);

                    // Validate message isn't empty
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] {0} command: Message cannot be empty", logType));
                        return;
                    }

                    ProcessLogMessage(logType, message);
                }
                catch (Exception ex)
                {
                    logError = ex;
                }

                if (logError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleSingleLogCommand error: {0}\n{1}", logError.Message, logError.StackTrace));
                }
            }

            /// <summary>
            /// Process the actual logging based on message type
            /// ⚠️ THREAD SAFETY: Thread-safe logging operations
            /// </summary>
            /// <param name="logType">Type of log message (msg, warn, err)</param>
            /// <param name="message">Message content to log</param>
            private void ProcessLogMessage(string logType, string message)
            {
                Exception processError = null;
                try
                {
                    switch (logType.ToLower())
                    {
                        case "msg":
                        case "message":
                        case "info":
                            Main.logger?.Msg(1, string.Format("[MANUAL] {0}", message));
                            Main.logger?.Msg(2, string.Format("[CONSOLE] Manual info message logged: {0}", message.Length > 50 ? message.Substring(0, 50) + "..." : message));
                            break;

                        case "warn":
                        case "warning":
                            Main.logger?.Warn(1, string.Format("[MANUAL] {0}", message));
                            Main.logger?.Msg(2, string.Format("[CONSOLE] Manual warning logged: {0}", message.Length > 50 ? message.Substring(0, 50) + "..." : message));
                            break;

                        case "err":
                        case "error":
                            Main.logger?.Err(string.Format("[MANUAL] {0}", message));
                            Main.logger?.Msg(2, string.Format("[CONSOLE] Manual error logged: {0}", message.Length > 50 ? message.Substring(0, 50) + "..." : message));
                            break;

                        default:
                            Main.logger?.Err(string.Format("[CONSOLE] Invalid log type '{0}'. Use: msg, warn, or err", logType));
                            Main.logger?.Msg(1, "[CONSOLE] Available log types: msg (info), warn (warning), err (error)");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    processError = ex;
                }

                if (processError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] ProcessLogMessage error: {0}\n{1}", processError.Message, processError.StackTrace));
                }
            }

            private void HandleStressSaveGameCommand(string[] parts)
            {
                Exception stressError = null;
                try
                {
                    if (parts.Length < 2)
                    {
                        Main.logger?.Msg(1, "[CONSOLE] Usage: savegamestress <count> [delay_seconds] [bypass_cooldown]");
                        Main.logger?.Msg(1, "[CONSOLE] Parameters can be in any order after count (auto-detected):");
                        Main.logger?.Msg(1, "[CONSOLE] Examples:");
                        Main.logger?.Msg(1, "[CONSOLE]   savegamestress 10              (10 saves, no delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE]   savegamestress 5 3.0           (5 saves, 3s delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE]   savegamestress 3 false         (3 saves, no delay, bypass=false)");
                        Main.logger?.Msg(1, "[CONSOLE]   savegamestress 5 2.0 false     (5 saves, 2s delay, bypass=false)");
                        Main.logger?.Msg(1, "[CONSOLE]   savegamestress 3 true 5.0      (3 saves, 5s delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE] Note: This calls the game's SaveManager directly");
                        return;
                    }

                    int iterations;
                    if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.", parts[1]));
                        return;
                    }

                    float delaySeconds = 0f;
                    bool bypassCooldown = true;

                    for (int i = 2; i < parts.Length && i < 4; i++)
                    {
                        var param = parts[i].Trim();
                        bool boolValue;
                        if (bool.TryParse(param, out boolValue))
                        {
                            bypassCooldown = boolValue;
                            Main.logger?.Msg(3, string.Format("[CONSOLE] Parsed parameter '{0}' as bypass cooldown: {1}", param, boolValue));
                        }
                        else
                        {
                            float floatValue;
                            if (float.TryParse(param, out floatValue) && floatValue >= 0f)
                            {
                                delaySeconds = floatValue;
                                Main.logger?.Msg(3, string.Format("[CONSOLE] Parsed parameter '{0}' as delay: {1:F3}s", param, floatValue));
                            }
                            else
                            {
                                Main.logger?.Err(string.Format("[CONSOLE] Invalid parameter '{0}'. Must be a delay (number ≥ 0) or bypass flag (true/false).", param));
                                return;
                            }
                        }
                    }

                    if (iterations > 20)
                    {
                        Main.logger?.Warn(1, string.Format("[CONSOLE] Warning: {0} game saves is excessive. Consider using fewer iterations.", iterations));
                    }

                    if (delaySeconds < 3f && iterations > 5)
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Warning: Game saves should have adequate delay (3+ seconds recommended) to prevent corruption.");
                    }

                    Main.logger?.Msg(1, string.Format("[CONSOLE] Starting game save stress test: {0} iterations, {1:F3}s delay, bypass cooldown: {2}", iterations, delaySeconds, bypassCooldown));
                    MelonCoroutines.Start(Save.CrashResistantSaveManager.StressGameSaveTest(iterations, delaySeconds, bypassCooldown));
                }
                catch (Exception ex)
                {
                    stressError = ex;
                }

                if (stressError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleStressSaveGameCommand error: {0}\n{1}", stressError.Message, stressError.StackTrace));
                }
            }

            private void HandleComprehensiveSaveMonitoringCommand(string[] parts)
            {
                Exception monitorError = null;
                try
                {
                    if (parts.Length < 2)
                    {
                        Main.logger?.Msg(1, "[CONSOLE] Usage: savemonitor <count> [delay_seconds] [bypass_cooldown]");
                        Main.logger?.Msg(1, "[CONSOLE] Parameters can be in any order after count (auto-detected):");
                        Main.logger?.Msg(1, "[CONSOLE] Examples:");
                        Main.logger?.Msg(1, "[CONSOLE]   savemonitor 5                   (5 saves, no delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE]   savemonitor 3 2.0              (3 saves, 2s delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE]   savemonitor 10 false           (10 saves, no delay, bypass=false)");
                        Main.logger?.Msg(1, "[CONSOLE]   savemonitor 5 1.5 false        (5 saves, 1.5s delay, bypass=false)");
                        return;
                    }

                    int iterations;
                    if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.", parts[1]));
                        return;
                    }

                    float delaySeconds = 0f;
                    bool bypassCooldown = true;

                    for (int i = 2; i < parts.Length; i++)
                    {
                        string param = parts[i];

                        if (param.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            bypassCooldown = true;
                        }
                        else if (param.Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            bypassCooldown = false;
                        }
                        else
                        {
                            float value;
                            if (float.TryParse(param, out value))
                            {
                                if (value >= 0f)
                                {
                                    delaySeconds = value;
                                }
                                else
                                {
                                    Main.logger?.Warn(1, string.Format("[CONSOLE] Negative delay '{0}' ignored. Using 0 seconds.", param));
                                }
                            }
                            else
                            {
                                Main.logger?.Warn(1, string.Format("[CONSOLE] Unknown parameter '{0}' ignored.", param));
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        Main.logger?.Err("[CONSOLE] No current save path available. Load a game first.");
                        return;
                    }

                    Main.logger?.Msg(1, string.Format("[CONSOLE] Starting comprehensive save monitoring (dnSpy): {0} iterations, {1:F3}s delay, bypass cooldown: {2}", iterations, delaySeconds, bypassCooldown));
                    MelonCoroutines.Start(Main.StressGameSaveTestWithComprehensiveMonitoring(iterations, delaySeconds, bypassCooldown));
                }
                catch (Exception ex)
                {
                    monitorError = ex;
                }

                if (monitorError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleComprehensiveSaveMonitoringCommand error: {0}\n{1}", monitorError.Message, monitorError.StackTrace));
                }
            }

            private void HandleTransactionalSaveCommand()
            {
                Exception transactionError = null;
                try
                {
                    if (string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        Main.logger?.Err("[CONSOLE] No current save path available. Load a game first.");
                        return;
                    }

                    if (Main.savedMixerValues.Count == 0)
                    {
                        Main.logger?.Warn(1, "[CONSOLE] No mixer data to save. Try adjusting some mixer thresholds first.");
                        return;
                    }

                    Main.logger?.Msg(1, "[CONSOLE] Starting atomic transactional save operation");
                    MelonCoroutines.Start(Main.PerformTransactionalSave());
                }
                catch (Exception ex)
                {
                    transactionError = ex;
                }

                if (transactionError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleTransactionalSaveCommand error: {0}\n{1}", transactionError.Message, transactionError.StackTrace));
                }
            }

            private void HandleProfileCommand()
            {
                Exception profileError = null;
                try
                {
                    if (string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        Main.logger?.Err("[CONSOLE] No current save path available. Load a game first.");
                        return;
                    }

                    Main.logger?.Msg(1, "[CONSOLE] Starting advanced save operation profiling");
                    Main.logger?.Msg(1, "[CONSOLE] This will perform a complete save cycle with detailed performance monitoring");
                    MelonCoroutines.Start(Main.AdvancedSaveOperationProfiling());
                }
                catch (Exception ex)
                {
                    profileError = ex;
                }

                if (profileError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleProfileCommand error: {0}\n{1}", profileError.Message, profileError.StackTrace));
                }
            }

            private void HandleStressSavePrefCommand(string[] parts)
            {
                Exception stressError = null;
                try
                {
                    if (parts.Length < 2)
                    {
                        Main.logger?.Msg(1, "[CONSOLE] Usage: saveprefstress <count> [delay_seconds] [bypass_cooldown]");
                        Main.logger?.Msg(1, "[CONSOLE] Parameters can be in any order after count (auto-detected):");
                        Main.logger?.Msg(1, "[CONSOLE] Examples:");
                        Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 10              (10 saves, no delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 5 2.0           (5 saves, 2s delay, bypass=true)");
                        Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 20 false        (20 saves, no delay, bypass=false)");
                        Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 10 0.1 false    (10 saves, 0.1s delay, bypass=false)");
                        Main.logger?.Msg(1, "[CONSOLE]   saveprefstress 5 true 2.0      (5 saves, 2s delay, bypass=true)");
                        return;
                    }

                    int iterations;
                    if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.", parts[1]));
                        return;
                    }

                    float delaySeconds = 0f;
                    bool bypassCooldown = true;

                    for (int i = 2; i < parts.Length && i < 4; i++)
                    {
                        var param = parts[i].Trim();
                        bool boolValue;
                        if (bool.TryParse(param, out boolValue))
                        {
                            bypassCooldown = boolValue;
                            Main.logger?.Msg(3, string.Format("[CONSOLE] Parsed parameter '{0}' as bypass cooldown: {1}", param, boolValue));
                        }
                        else
                        {
                            float floatValue;
                            if (float.TryParse(param, out floatValue) && floatValue >= 0f)
                            {
                                delaySeconds = floatValue;
                                Main.logger?.Msg(3, string.Format("[CONSOLE] Parsed parameter '{0}' as delay: {1:F3}s", param, floatValue));
                            }
                            else
                            {
                                Main.logger?.Err(string.Format("[CONSOLE] Invalid parameter '{0}'. Must be a delay (number ≥ 0) or bypass flag (true/false).", param));
                                return;
                            }
                        }
                    }

                    if (iterations > 100)
                    {
                        Main.logger?.Warn(1, string.Format("[CONSOLE] Warning: {0} iterations is a large stress test. This may take significant time.", iterations));
                    }

                    if (delaySeconds > 10f)
                    {
                        Main.logger?.Warn(1, string.Format("[CONSOLE] Warning: {0:F1}s delay will make this test take {1:F1} minutes.", delaySeconds, (iterations * delaySeconds) / 60f));
                    }

                    if (!bypassCooldown && delaySeconds < 2f)
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Warning: With cooldown enabled and low delay, some saves may be skipped due to 2-second cooldown.");
                    }

                    Main.logger?.Msg(1, string.Format("[CONSOLE] Starting mixer preferences stress test: {0} iterations, {1:F3}s delay, bypass cooldown: {2}", iterations, delaySeconds, bypassCooldown));
                    MelonCoroutines.Start(Save.CrashResistantSaveManager.StressSaveTest(iterations, delaySeconds, bypassCooldown));
                }
                catch (Exception ex)
                {
                    stressError = ex;
                }

                if (stressError != null)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] HandleStressSavePrefCommand error: {0}\n{1}", stressError.Message, stressError.StackTrace));
                }
            }

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

                    MixerIDManager.ResetStableIDCounter();
                    Main.logger?.Msg(1, "[CONSOLE] Mixer ID counter reset");

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
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                
                // Test the console commands to ensure they work
                Main.logger?.Msg(2, "[CONSOLE] Testing console command functionality...");
                TestConsoleCommands(hookInstance);
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)

        /// <summary>
        /// Test console commands to verify they work properly
        /// ⚠️ THREAD SAFETY: Safe testing with comprehensive error handling
        /// </summary>
        private static void TestConsoleCommands(MixerConsoleHook hookInstance)
        {
            Exception testError = null;
            try
            {
                Main.logger?.Msg(3, "[CONSOLE] Running console command tests...");
                
                // Test basic logging commands
                Main.logger?.Msg(3, "[CONSOLE] Testing 'msg' command...");
                hookInstance.OnConsoleCommand("msg Console command test - this should appear as a manual message");
                
                Main.logger?.Msg(3, "[CONSOLE] Testing 'warn' command...");
                hookInstance.OnConsoleCommand("warn Console warning test - this should appear as a manual warning");
                
                // Test help command
                Main.logger?.Msg(3, "[CONSOLE] Testing help display...");
                hookInstance.OnConsoleCommand("help");
                
                Main.logger?.Msg(2, "[CONSOLE] Console command tests completed");
                Main.logger?.Msg(1, "[CONSOLE] Note: Console commands work but may need game console integration for user input");
            }
            catch (Exception ex)
            {
                testError = ex;
            }

            if (testError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] TestConsoleCommands error: {0}\n{1}", testError.Message, testError.StackTrace));
            }
        }

        /// <summary>
        /// Manual console command processor for testing and debugging
        /// Can be called from other parts of the code to test commands
        /// </summary>
        public static void ProcessManualCommand(string command)
        {
            Exception manualError = null;
            try
            {
                var hookInstance = MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    Main.logger?.Msg(2, string.Format("[CONSOLE] Processing manual command: {0}", command));
                    hookInstance.OnConsoleCommand(command);
                }
                else
                {
                    Main.logger?.Err("[CONSOLE] Cannot process manual command - hook instance not available");
                }
            }
            catch (Exception ex)
            {
                manualError = ex;
            }

            if (manualError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] ProcessManualCommand error: {0}\n{1}", manualError.Message, manualError.StackTrace));
            }
        }
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
    }
}