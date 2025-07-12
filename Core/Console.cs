using MelonLoader;
using MelonLoader.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;

namespace MixerThreholdMod_1_0_0.Core
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

            private void ProcessCommand(string lowerCommand)
            {
                Exception processError = null;
                try
                {
                    // Handle commands with parameters
                    var parts = lowerCommand.Split(' ');
                    var baseCommand = parts[0];

                    switch (baseCommand)
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
                            HandleStressSavePrefCommand(parts);
                            break;
                        case "mixer_savegamestress":
                        case "savegamestress":
                            HandleStressSaveGameCommand(parts);
                            break;
                        case "mixer_savemonitor":
                        case "savemonitor":
                            HandleComprehensiveSaveMonitoringCommand(parts);
                            break;
                        case "transactionalsave":
                        case "mixer_transactional":
                            HandleTransactionalSaveCommand();
                            break;
                        case "profile":
                        case "mixer_profile":
                            HandleProfileCommand();
                            break;
                        // REMOVED: case "mixer_log": and case "log":
                        case "msg":
                            HandleSingleLogCommand("msg", parts, lowerCommand);
                            break;
                        case "warn":
                            HandleSingleLogCommand("warn", parts, lowerCommand);
                            break;
                        case "err":
                            HandleSingleLogCommand("err", parts, lowerCommand);
                            break;
                        default:
                            ShowHelpMessage();
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

            private void ShowHelpMessage()
            {
                Exception helpError = null;
                try
                {
                    Main.logger?.Msg(1, "[CONSOLE] Available commands:");
                    Main.logger?.Msg(1, "[CONSOLE] ");
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

            /// <summary>
            /// Handle stress save game command with parameters
            /// ⚠️ THREAD SAFETY: Validates parameters and starts game save stress test safely
            /// Supports flexible parameter ordering: <count> [delay] [bypassCooldown]
            /// </summary>
            /// <param name="parts">Command parts: [0]=command, [1-3]=flexible parameters</param>
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

                    // Parse iteration count (always first parameter)
                    int iterations;
                    if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.", parts[1]));
                        return;
                    }

                    // Parse remaining parameters flexibly
                    float delaySeconds = 0f;
                    bool bypassCooldown = true; // Default to true

                    // Analyze remaining parameters (parts[2] and parts[3] if they exist)
                    for (int i = 2; i < parts.Length && i < 4; i++)
                    {
                        var param = parts[i].Trim();

                        // Try to parse as boolean first
                        bool boolValue;
                        if (bool.TryParse(param, out boolValue))
                        {
                            bypassCooldown = boolValue;
                            Main.logger?.Msg(3, string.Format("[CONSOLE] Parsed parameter '{0}' as bypass cooldown: {1}", param, boolValue));
                        }
                        // Try to parse as float
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

                    // Game save validation warnings (more conservative)
                    if (iterations > 20)
                    {
                        Main.logger?.Warn(1, string.Format("[CONSOLE] Warning: {0} game saves is excessive. Consider using fewer iterations.", iterations));
                    }

                    if (delaySeconds < 3f && iterations > 5)
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Warning: Game saves should have adequate delay (3+ seconds recommended) to prevent corruption.");
                    }

                    if (!bypassCooldown)
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Warning: Game save cooldown behavior depends on game's internal cooldown system (not controlled by this mod).");
                    }

                    // Start the game save stress test using direct SaveManager access
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

            /// <summary>
            /// Handle comprehensive save monitoring command with dnSpy multi-method validation.
            /// ⚠️ THREAD SAFETY: Validates parameters and starts comprehensive monitoring safely.
            /// Supports flexible parameter ordering: <count> [delay] [bypassCooldown]
            /// </summary>
            /// <param name="parts">Command parts: [0]=command, [1-3]=flexible parameters</param>
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
                        Main.logger?.Msg(1, "[CONSOLE] ");
                        Main.logger?.Msg(1, "[CONSOLE] Features (dnSpy integration):");
                        Main.logger?.Msg(1, "[CONSOLE]   • Multi-method save validation (timestamp, size, backup, permissions)");
                        Main.logger?.Msg(1, "[CONSOLE]   • Enhanced permission testing (4-layer validation)");
                        Main.logger?.Msg(1, "[CONSOLE]   • Comprehensive failure categorization");
                        Main.logger?.Msg(1, "[CONSOLE]   • File system baseline monitoring");
                        return;
                    }

                    // Parse iteration count (always first parameter)
                    int iterations;
                    if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.", parts[1]));
                        return;
                    }

                    // Parse remaining parameters flexibly
                    float delaySeconds = 0f;
                    bool bypassCooldown = true; // Default to true

                    // Process parameters after count in any order
                    for (int i = 2; i < parts.Length; i++)
                    {
                        string param = parts[i];
                        
                        // Check if it's a boolean parameter
                        if (param.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            bypassCooldown = true;
                        }
                        else if (param.Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            bypassCooldown = false;
                        }
                        // Check if it's a numeric parameter (delay)
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

                    // Validate current save path
                    if (string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        Main.logger?.Err("[CONSOLE] No current save path available. Load a game first.");
                        return;
                    }

                    // Warn about cooldown behavior
                    if (!bypassCooldown)
                    {
                        Main.logger?.Warn(1, "[CONSOLE] Warning: Comprehensive monitoring respects game save cooldown behavior.");
                    }

                    // Start the comprehensive save monitoring test
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

            /// <summary>
            /// Handle transactional save command.
            /// ⚠️ THREAD SAFETY: Starts atomic save operation safely.
            /// </summary>
            private void HandleTransactionalSaveCommand()
            {
                Exception transactionError = null;
                try
                {
                    // Validate current save path
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

                    // Start transactional save
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

            /// <summary>
            /// Handle advanced profiling command.
            /// ⚠️ THREAD SAFETY: Starts performance profiling operation safely.
            /// </summary>
            private void HandleProfileCommand()
            {
                Exception profileError = null;
                try
                {
                    // Validate current save path
                    if (string.IsNullOrEmpty(Main.CurrentSavePath))
                    {
                        Main.logger?.Err("[CONSOLE] No current save path available. Load a game first.");
                        return;
                    }

                    // Start advanced profiling
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

            /// <summary>
            /// Handle stress save mixer preferences command with parameters
            /// ⚠️ THREAD SAFETY: Validates parameters and starts coroutine safely
            /// Supports flexible parameter ordering: <count> [delay] [bypassCooldown]
            /// </summary>
            /// <param name="parts">Command parts: [0]=command, [1-3]=flexible parameters</param>
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

                    // Parse iteration count (always first parameter)
                    int iterations;
                    if (!int.TryParse(parts[1], out iterations) || iterations <= 0)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.", parts[1]));
                        return;
                    }

                    // Parse remaining parameters flexibly
                    float delaySeconds = 0f;
                    bool bypassCooldown = true; // Default to true

                    // Analyze remaining parameters (parts[2] and parts[3] if they exist)
                    for (int i = 2; i < parts.Length && i < 4; i++)
                    {
                        var param = parts[i].Trim();

                        // Try to parse as boolean first
                        bool boolValue;
                        if (bool.TryParse(param, out boolValue))
                        {
                            bypassCooldown = boolValue;
                            Main.logger?.Msg(3, string.Format("[CONSOLE] Parsed parameter '{0}' as bypass cooldown: {1}", param, boolValue));
                        }
                        // Try to parse as float
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

                    // Validation warnings
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

                    // Start the mixer preferences stress test
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