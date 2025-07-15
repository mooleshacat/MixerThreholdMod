using HarmonyLib;
using System;
using UnityEngine;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Bridge for intercepting game's logging system and redirecting to our custom logger
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and designed for concurrent access
    /// ⚠️ .NET 4.8.1 Compatible: Uses compatible syntax and exception handling patterns
    /// ⚠️ MAIN THREAD WARNING: Logging operations are non-blocking and thread-safe
    /// 
    /// Crash Prevention Features:
    /// - Intercepts game exceptions through logging system
    /// - Identifies mod-caused vs game-caused exceptions
    /// - Prevents logging failures from crashing the game
    /// - Maintains original game logging while adding enhanced monitoring
    /// </summary>
    public static class GameLoggerBridge
    {
        private static bool _isInitialized = false;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Initialize Harmony patches for game logging interception
        /// </summary>
        public static void InitializeLoggingBridge()
        {
            lock (_initLock)
            {
                if (_isInitialized)
                {
                    Main.logger?.Msg(2, "[BRIDGE] GameLoggerBridge already initialized");
                    return;
                }

                Exception initError = null;
                try
                {
                    Main.logger?.Msg(2, "[BRIDGE] Initializing GameLoggerBridge patches");

                    // Apply Harmony patches to intercept game logging
                    var harmony = Main.Instance.HarmonyInstance;

                    // dnSpy Verified: ScheduleOne.Console class and method signatures verified via comprehensive dnSpy analysis
                    // Patch ScheduleOne.Console.Log
                    var logMethod = AccessTools.Method(typeof(ScheduleOne.Console), "Log", new Type[] { typeof(object), typeof(UnityEngine.Object) });
                    if (logMethod != null)
                    {
                        harmony.Patch(logMethod, prefix: new HarmonyMethod(typeof(GameLoggerBridge), nameof(LogPrefix)));
                        Main.logger?.Msg(2, "[BRIDGE] Successfully patched ScheduleOne.Console.Log");
                    }
                    else
                    {
                        Main.logger?.Warn(1, "[BRIDGE] Could not find ScheduleOne.Console.Log method for patching");
                    }

                    // dnSpy Verified: ScheduleOne.Console.LogWarning method signature verified
                    // Patch ScheduleOne.Console.LogWarning
                    var logWarningMethod = AccessTools.Method(typeof(ScheduleOne.Console), "LogWarning", new Type[] { typeof(object), typeof(UnityEngine.Object) });
                    if (logWarningMethod != null)
                    {
                        harmony.Patch(logWarningMethod, prefix: new HarmonyMethod(typeof(GameLoggerBridge), nameof(LogWarningPrefix)));
                        Main.logger?.Msg(2, "[BRIDGE] Successfully patched ScheduleOne.Console.LogWarning");
                    }
                    else
                    {
                        Main.logger?.Warn(1, "[BRIDGE] Could not find ScheduleOne.Console.LogWarning method for patching");
                    }

                    // dnSpy Verified: ScheduleOne.Console.LogError method signature verified  
                    // Patch ScheduleOne.Console.LogError
                    var logErrorMethod = AccessTools.Method(typeof(ScheduleOne.Console), "LogError", new Type[] { typeof(object), typeof(UnityEngine.Object) });
                    if (logErrorMethod != null)
                    {
                        harmony.Patch(logErrorMethod, prefix: new HarmonyMethod(typeof(GameLoggerBridge), nameof(LogErrorPrefix)));
                        Main.logger?.Msg(2, "[BRIDGE] Successfully patched ScheduleOne.Console.LogError");
                    }
                    else
                    {
                        Main.logger?.Warn(1, "[BRIDGE] Could not find ScheduleOne.Console.LogError method for patching");
                    }

                    _isInitialized = true;
                    Main.logger?.Msg(1, "[BRIDGE] GameLoggerBridge initialization completed successfully");
                }
                catch (Exception ex)
                {
                    initError = ex;
                }

                if (initError != null)
                {
                    Main.logger?.Err(string.Format("[BRIDGE] GameLoggerBridge initialization FAILED: {0}\nStackTrace: {1}",
                        initError.Message, initError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Harmony prefix patch for ScheduleOne.Console.Log
        /// Redirects to Logger.Msg() while preserving original functionality
        /// </summary>
        [HarmonyPrefix]
        public static bool LogPrefix(object message, UnityEngine.Object context)
        {
            Exception bridgeError = null;
            try
            {
                // Forward to our custom logger
                string messageStr = message?.ToString() ?? NULL_COMMAND_FALLBACK;
                Main.logger?.Msg(2, string.Format("[GAME] {0}", messageStr));

                // Check if this is potentially mod-related
                AnalyzeForModRelatedContent(messageStr, "INFO");
            }
            catch (Exception ex)
            {
                bridgeError = ex;
            }

            if (bridgeError != null)
            {
                // Log bridge error without causing recursion
                try
                {
                    Main.logger?.Err(string.Format("[BRIDGE] LogPrefix error: {0}", bridgeError.Message));
                }
                catch
                {
                    // Prevent infinite recursion if logger fails
                }
            }

            // Continue with original method execution
            return true;
        }

        /// <summary>
        /// Harmony prefix patch for ScheduleOne.Console.LogWarning
        /// Redirects to Logger.Warn() while preserving original functionality
        /// </summary>
        [HarmonyPrefix]
        public static bool LogWarningPrefix(object message, UnityEngine.Object context)
        {
            Exception bridgeError = null;
            try
            {
                // Forward to our custom logger
                string messageStr = message?.ToString() ?? NULL_COMMAND_FALLBACK;
                Main.logger?.Warn(1, string.Format("[GAME WARNING] {0}", messageStr));

                // Check if this is potentially mod-related
                AnalyzeForModRelatedContent(messageStr, "WARNING");
            }
            catch (Exception ex)
            {
                bridgeError = ex;
            }

            if (bridgeError != null)
            {
                // Log bridge error without causing recursion
                try
                {
                    Main.logger?.Err(string.Format("[BRIDGE] LogWarningPrefix error: {0}", bridgeError.Message));
                }
                catch
                {
                    // Prevent infinite recursion if logger fails
                }
            }

            // Continue with original method execution
            return true;
        }

        /// <summary>
        /// Harmony prefix patch for ScheduleOne.Console.LogError
        /// Redirects to Logger.Err() while preserving original functionality
        /// Performs exception monitoring and mod attribution
        /// </summary>
        [HarmonyPrefix]
        public static bool LogErrorPrefix(object message, UnityEngine.Object context)
        {
            Exception bridgeError = null;
            try
            {
                // Forward to our custom logger with enhanced error handling
                string messageStr = message?.ToString() ?? NULL_COMMAND_FALLBACK;
                Main.logger?.Err(string.Format("[GAME ERROR] {0}", messageStr));

                // Enhanced analysis for errors - more comprehensive than warnings
                AnalyzeForModRelatedContent(messageStr, "ERROR");
                AnalyzeForExceptionContent(messageStr);
            }
            catch (Exception ex)
            {
                bridgeError = ex;
            }

            if (bridgeError != null)
            {
                // Log bridge error without causing recursion
                try
                {
                    Main.logger?.Err(string.Format("[BRIDGE] LogErrorPrefix error: {0}", bridgeError.Message));
                }
                catch
                {
                    // Prevent infinite recursion if logger fails
                }
            }

            // Continue with original method execution
            return true;
        }

        /// <summary>
        /// Analyze log content for mod-related issues
        /// </summary>
        private static void AnalyzeForModRelatedContent(string message, string logLevel)
        {
            if (string.IsNullOrEmpty(message)) return;

            Exception analysisError = null;
            try
            {
                string lowerMessage = message.ToLower();

                // Check for mod-related keywords
                string[] modIndicators = {
                    "mixerthreholdmod", "mixerthresholdmod", "mixer threshold", "mixer_threshold",
                    "melonloader", "harmony", "modding", "patch", "hook"
                };

                foreach (string indicator in modIndicators)
                {
                    if (lowerMessage.Contains(indicator))
                    {
                        Main.logger?.Warn(1, string.Format("[BRIDGE] MOD-RELATED {0} DETECTED: {1}", logLevel, message));
                        break;
                    }
                }

                // Check for save-related issues (our primary concern)
                string[] saveIndicators = {
                    "save", "load", "file", "json", "backup", "corruption", "permission"
                };

                bool isSaveRelated = false;
                foreach (string indicator in saveIndicators)
                {
                    if (lowerMessage.Contains(indicator))
                    {
                        isSaveRelated = true;
                        break;
                    }
                }

                if (isSaveRelated)
                {
                    Main.logger?.Warn(1, string.Format("[BRIDGE] SAVE-RELATED {0}: {1}", logLevel, message));
                }
            }
            catch (Exception ex)
            {
                analysisError = ex;
            }

            if (analysisError != null)
            {
                // Don't let analysis errors affect logging
                try
                {
                    Main.logger?.Err(string.Format("[BRIDGE] Content analysis error: {0}", analysisError.Message));
                }
                catch
                {
                    // Final safety net
                }
            }
        }

        /// <summary>
        /// Analyze error messages for exception patterns and stack traces
        /// </summary>
        private static void AnalyzeForExceptionContent(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            Exception analysisError = null;
            try
            {
                // Check for exception keywords
                string[] exceptionIndicators = {
                    "exception", "stacktrace", "stack trace", "at ", "   at ",
                    "nullreferenceexception", "argumentexception", "ioexception",
                    "unauthorizedaccessexception", "filenotfoundexception"
                };

                string lowerMessage = message.ToLower();
                bool hasExceptionContent = false;

                foreach (string indicator in exceptionIndicators)
                {
                    if (lowerMessage.Contains(indicator))
                    {
                        hasExceptionContent = true;
                        break;
                    }
                }

                if (hasExceptionContent)
                {
                    Main.logger?.Err(string.Format("[BRIDGE] EXCEPTION DETECTED IN GAME LOG: {0}", message));

                    // Check specifically for our mod in stack traces
                    if (lowerMessage.Contains("mixerthreholdmod") || lowerMessage.Contains("mixerthresholdmod"))
                    {
                        Main.logger?.Err("[BRIDGE] ⚠️ MOD-CAUSED EXCEPTION DETECTED ⚠️");
                        Main.logger?.Err(string.Format("[BRIDGE] Our mod appears to be involved in this exception: {0}", message));
                    }
                }
            }
            catch (Exception ex)
            {
                analysisError = ex;
            }

            if (analysisError != null)
            {
                // Don't let analysis errors affect error logging
                try
                {
                    Main.logger?.Err(string.Format("[BRIDGE] Exception analysis error: {0}", analysisError.Message));
                }
                catch
                {
                    // Final safety net
                }
            }
        }
    }
}