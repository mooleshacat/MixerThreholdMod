using MelonLoader;
using MixerThreholdMod_1_0_0.Save;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Simplified console command processor for handling mod commands
    /// ⚠️ THREAD SAFETY: All operations are thread-safe with proper error handling
    /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe patterns, minimal reflection
    /// ⚠️ CRASH PREVENTION: Comprehensive error handling prevents console failures from affecting gameplay
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible switch statement patterns
    /// - Proper exception handling throughout
    /// </summary>
    public static class ModConsoleCommandProcessor
    {
        /// <summary>
        /// Process console commands with comprehensive error handling
        /// ⚠️ THREAD SAFETY: Safe command processing with detailed logging
        /// </summary>
        public static void ProcessCommand(string command)
        {
            Exception processError = null;
            try
            {
                if (string.IsNullOrEmpty(command))
                {
                    Main.logger?.Warn(WARN_LEVEL_CRITICAL, "[CONSOLE] Empty command received");
                    return;
                }

                var parts = command.ToLower().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                {
                    Main.logger?.Warn(WARN_LEVEL_CRITICAL, "[CONSOLE] No command parts found");
                    return;
                }

                var baseCommand = parts[0];
                Main.logger?.Msg(LOG_LEVEL_IMPORTANT, string.Format("[CONSOLE] Processing command: {0}", baseCommand));

                switch (baseCommand)
                {
                    case COMMAND_MIXER_RESET:
                        HandleResetCommand();
                        break;

                    case COMMAND_MIXER_SAVE:
                        HandleSaveCommand();
                        break;

                    case COMMAND_MIXER_PATH:
                        HandlePathCommand();
                        break;

                    case COMMAND_MIXER_EMERGENCY:
                        HandleEmergencyCommand();
                        break;

                    case COMMAND_SAVE_PREF_STRESS:
                        HandleStressSavePrefCommand(parts);
                        break;

                    case COMMAND_SAVE_GAME_STRESS:
                        HandleStressSaveGameCommand(parts);
                        break;

                    case COMMAND_SAVE_MONITOR:
                        HandleSaveMonitorCommand(parts);
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
                        HandleLogCommand("msg", parts, command);
                        break;

                    case "warn":
                        HandleLogCommand("warn", parts, command);
                        break;

                    case "err":
                        HandleLogCommand("err", parts, command);
                        break;

                    case "detectdirs":
                    case "directories":
                    case "paths":
                        HandleDirectoryDetectionCommand();
                        break;

                    case COMMAND_HELP:
                    case "?":
                        HandleHelpCommand();
                        break;

                    default:
                        Main.logger?.Warn(WARN_LEVEL_CRITICAL, string.Format("[CONSOLE] Unknown command: {0}", baseCommand));
                        Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Type 'help' to see available commands");
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
        /// Handle mixer reset command
        /// ⚠️ CRASH PREVENTION: Safe mixer reset with comprehensive error handling
        /// </summary>
        private static void HandleResetCommand()
        {
            Exception resetError = null;
            try
            {
                if (Main.savedMixerValues != null)
                {
                    Main.savedMixerValues.Clear();
                    Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Mixer values cleared");
                }

                MixerIDManager.ResetStableIDCounter();
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Mixer ID counter reset");

                CrashResistantSaveManager.EmergencySave();
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Empty state saved");
            }
            catch (Exception ex)
            {
                resetError = ex;
            }

            if (resetError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleResetCommand error: {0}", resetError.Message));
            }
        }

        /// <summary>
        /// Handle mixer save command
        /// ⚠️ CRASH PREVENTION: Safe save operation with error handling
        /// </summary>
        private static void HandleSaveCommand()
        {
            Exception saveError = null;
            try
            {
                MelonCoroutines.Start(CrashResistantSaveManager.TriggerSaveWithCooldown());
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Force save triggered");
            }
            catch (Exception ex)
            {
                saveError = ex;
            }

            if (saveError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleSaveCommand error: {0}", saveError.Message));
            }
        }

        /// <summary>
        /// Handle path display command
        /// ⚠️ THREAD SAFETY: Safe path access with null checking
        /// </summary>
        private static void HandlePathCommand()
        {
            Exception pathError = null;
            try
            {
                string currentPath = Main.CurrentSavePath ?? NULL_PATH_FALLBACK;
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Current save path: {0}", currentPath));

                int mixerCount = Main.savedMixerValues?.Count ?? 0;
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Tracked mixer values: {0}", mixerCount));
            }
            catch (Exception ex)
            {
                pathError = ex;
            }

            if (pathError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandlePathCommand error: {0}", pathError.Message));
            }
        }

        /// <summary>
        /// Handle emergency save command
        /// ⚠️ CRASH PREVENTION: Emergency save with comprehensive error handling
        /// </summary>
        private static void HandleEmergencyCommand()
        {
            Exception emergencyError = null;
            try
            {
                CrashResistantSaveManager.EmergencySave();
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Emergency save completed");
            }
            catch (Exception ex)
            {
                emergencyError = ex;
            }

            if (emergencyError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleEmergencyCommand error: {0}", emergencyError.Message));
            }
        }

        /// <summary>
        /// Handle stress test pref save command
        /// ⚠️ THREAD SAFETY: Safe parameter parsing and stress test execution
        /// </summary>
        private static void HandleStressSavePrefCommand(string[] parts)
        {
            Exception stressError = null;
            try
            {
                if (parts.Length < 2)
                {
                    Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Usage: saveprefstress <count> [delay_seconds] [bypass_cooldown]");
                    return;
                }

                if (!int.TryParse(parts[1], out int iterations) || iterations <= 0)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count: {0}", parts[1]));
                    return;
                }

                float delaySeconds = DEFAULT_OPERATION_DELAY;
                bool bypassCooldown = true;

                // Parse optional parameters
                for (int i = 2; i < parts.Length && i < 4; i++)
                {
                    var param = parts[i].Trim();
                    if (bool.TryParse(param, out bool boolValue))
                    {
                        bypassCooldown = boolValue;
                    }
                    else if (float.TryParse(param, out float floatValue) && floatValue >= 0f)
                    {
                        delaySeconds = floatValue;
                    }
                    else
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid parameter: {0}", param));
                        return;
                    }
                }

                Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Starting mixer preferences stress test: {0} iterations, {1:F3}s delay, bypass: {2}", 
                    iterations, delaySeconds, bypassCooldown));
                MelonCoroutines.Start(CrashResistantSaveManager.StressSaveTest(iterations, delaySeconds, bypassCooldown));
            }
            catch (Exception ex)
            {
                stressError = ex;
            }

            if (stressError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleStressSavePrefCommand error: {0}", stressError.Message));
            }
        }

        /// <summary>
        /// Handle stress test game save command
        /// ⚠️ THREAD SAFETY: Safe parameter parsing and stress test execution
        /// </summary>
        private static void HandleStressSaveGameCommand(string[] parts)
        {
            Exception stressError = null;
            try
            {
                if (parts.Length < 2)
                {
                    Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Usage: savegamestress <count> [delay_seconds] [bypass_cooldown]");
                    return;
                }

                if (!int.TryParse(parts[1], out int iterations) || iterations <= 0)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count: {0}", parts[1]));
                    return;
                }

                float delaySeconds = DEFAULT_OPERATION_DELAY;
                bool bypassCooldown = true;

                // Parse optional parameters
                for (int i = 2; i < parts.Length && i < 4; i++)
                {
                    var param = parts[i].Trim();
                    if (bool.TryParse(param, out bool boolValue))
                    {
                        bypassCooldown = boolValue;
                    }
                    else if (float.TryParse(param, out float floatValue) && floatValue >= 0f)
                    {
                        delaySeconds = floatValue;
                    }
                    else
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Invalid parameter: {0}", param));
                        return;
                    }
                }

                Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Starting game save stress test: {0} iterations, {1:F3}s delay, bypass: {2}", 
                    iterations, delaySeconds, bypassCooldown));
                MelonCoroutines.Start(CrashResistantSaveManager.StressGameSaveTest(iterations, delaySeconds, bypassCooldown));
            }
            catch (Exception ex)
            {
                stressError = ex;
            }

            if (stressError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleStressSaveGameCommand error: {0}", stressError.Message));
            }
        }

        /// <summary>
        /// Handle save monitor command
        /// ⚠️ THREAD SAFETY: Safe monitoring command execution
        /// </summary>
        private static void HandleSaveMonitorCommand(string[] parts)
        {
            Exception monitorError = null;
            try
            {
                if (parts.Length < 2)
                {
                    Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Usage: savemonitor <count> [delay_seconds] [bypass_cooldown]");
                    return;
                }

                if (!int.TryParse(parts[1], out int iterations) || iterations <= 0)
                {
                    Main.logger?.Err(string.Format("[CONSOLE] Invalid iteration count: {0}", parts[1]));
                    return;
                }

                if (string.IsNullOrEmpty(Main.CurrentSavePath))
                {
                    Main.logger?.Err(NO_SAVE_PATH_ERROR);
                    return;
                }

                float delaySeconds = DEFAULT_OPERATION_DELAY;
                bool bypassCooldown = true;

                // Parse optional parameters
                for (int i = 2; i < parts.Length; i++)
                {
                    var param = parts[i];
                    if (bool.TryParse(param, out bool boolValue))
                    {
                        bypassCooldown = boolValue;
                    }
                    else if (float.TryParse(param, out float floatValue) && floatValue >= 0f)
                    {
                        delaySeconds = floatValue;
                    }
                    else
                    {
                        Main.logger?.Warn(WARN_LEVEL_VERBOSE, string.Format("[CONSOLE] Unknown parameter ignored: {0}", param));
                    }
                }

                Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}", 
                    iterations, delaySeconds, bypassCooldown));
                MelonCoroutines.Start(Main.StressGameSaveTestWithComprehensiveMonitoring(iterations, delaySeconds, bypassCooldown));
            }
            catch (Exception ex)
            {
                monitorError = ex;
            }

            if (monitorError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleSaveMonitorCommand error: {0}", monitorError.Message));
            }
        }

        /// <summary>
        /// Handle transactional save command
        /// ⚠️ CRASH PREVENTION: Safe transactional save execution
        /// </summary>
        private static void HandleTransactionalSaveCommand()
        {
            Exception transactionError = null;
            try
            {
                if (string.IsNullOrEmpty(Main.CurrentSavePath))
                {
                    Main.logger?.Err(NO_SAVE_PATH_ERROR);
                    return;
                }

                if (Main.savedMixerValues.Count == 0)
                {
                    Main.logger?.Warn(WARN_LEVEL_CRITICAL, NO_MIXER_DATA_ERROR);
                    return;
                }

                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Starting atomic transactional save operation");
                MelonCoroutines.Start(Main.PerformTransactionalSave());
            }
            catch (Exception ex)
            {
                transactionError = ex;
            }

            if (transactionError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleTransactionalSaveCommand error: {0}", transactionError.Message));
            }
        }

        /// <summary>
        /// Handle profile command
        /// ⚠️ CRASH PREVENTION: Safe profiling operation execution
        /// </summary>
        private static void HandleProfileCommand()
        {
            Exception profileError = null;
            try
            {
                if (string.IsNullOrEmpty(Main.CurrentSavePath))
                {
                    Main.logger?.Err(NO_SAVE_PATH_ERROR);
                    return;
                }

                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Starting advanced save operation profiling");
                MelonCoroutines.Start(Main.AdvancedSaveOperationProfiling());
            }
            catch (Exception ex)
            {
                profileError = ex;
            }

            if (profileError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleProfileCommand error: {0}", profileError.Message));
            }
        }

        /// <summary>
        /// Handle logging commands (msg, warn, err)
        /// ⚠️ THREAD SAFETY: Safe message extraction and logging
        /// </summary>
        private static void HandleLogCommand(string logType, string[] parts, string originalCommand)
        {
            Exception logError = null;
            try
            {
                if (parts.Length < 2)
                {
                    Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Usage: {0} <message>", logType));
                    return;
                }

                // Extract message from original command to preserve formatting
                string searchPattern = string.Format("{0} ", parts[0]);
                int messageStartIndex = originalCommand.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase);
                
                if (messageStartIndex >= 0)
                {
                    messageStartIndex += searchPattern.Length;
                    if (messageStartIndex < originalCommand.Length)
                    {
                        string message = originalCommand.Substring(messageStartIndex);
                        
                        switch (logType.ToLower())
                        {
                            case "msg":
                                Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[MANUAL] {0}", message));
                                break;
                            case "warn":
                                Main.logger?.Warn(WARN_LEVEL_CRITICAL, string.Format("[MANUAL] {0}", message));
                                break;
                            case "err":
                                Main.logger?.Err(string.Format("[MANUAL] {0}", message));
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logError = ex;
            }

            if (logError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleLogCommand error: {0}", logError.Message));
            }
        }

        /// <summary>
        /// Handle directory detection command
        /// ⚠️ ASYNC JUSTIFICATION: Directory detection uses game APIs that can block for 100-500ms
        /// </summary>
        private static void HandleDirectoryDetectionCommand()
        {
            Exception detectionError = null;
            try
            {
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] Starting directory detection...");

                Task.Run(async () =>
                {
                    try
                    {
                        var directoryInfo = await Helpers.GameDirectoryResolver.RefreshDirectoryDetectionAsync();
                        Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format("[CONSOLE] Directory detection completed: {0}", directoryInfo.ToString()));
                    }
                    catch (Exception asyncEx)
                    {
                        Main.logger?.Err(string.Format("[CONSOLE] Directory detection failed: {0}", asyncEx.Message));
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

        /// <summary>
        /// Handle help command
        /// ⚠️ THREAD SAFETY: Safe help text display
        /// </summary>
        private static void HandleHelpCommand()
        {
            Exception helpError = null;
            try
            {
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] === AVAILABLE COMMANDS ===");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] mixer_reset - Reset all mixer values");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] mixer_save - Force immediate save");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] mixer_path - Show current save path");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] mixer_emergency - Trigger emergency save");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] saveprefstress <count> - Stress test mixer saves");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] savegamestress <count> - Stress test game saves");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] savemonitor <count> - Comprehensive save monitoring");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] transactionalsave - Atomic transactional save");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] profile - Advanced save profiling");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] detectdirs - Detect game directories");
                Main.logger?.Msg(LOG_LEVEL_CRITICAL, "[CONSOLE] msg/warn/err <message> - Manual logging");
            }
            catch (Exception ex)
            {
                helpError = ex;
            }

            if (helpError != null)
            {
                Main.logger?.Err(string.Format("[CONSOLE] HandleHelpCommand error: {0}", helpError.Message));
            }
        }
    }
}