using MelonLoader;
using System;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Centralized logging system with configurable verbosity levels.
    /// Thread-safe logging for .NET 4.8.1 compatibility.
    /// 
    /// ⚠️ THREAD SAFETY: This class is thread-safe and can be called from any thread.
    /// All methods are designed to prevent exceptions and provide fallback behavior.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible with framework's threading model
    /// - Proper exception handling for crash prevention
    /// </summary>
    public class Logger
    {
        public bool IsDebugEnabled = true; // disable for production builds (disables all logging except errors)
        public int CurrentMsgLogLevel = 3; // current dev desired msg log level (1, 2, or 3)
        public int CurrentWarnLogLevel = 2; // current dev desired warn log level (1 or 2)
        public bool ShowLogLevelCalc = true; // Simplified default (leave as true for clarity, for human)

        /// <summary>
        /// Info (Msg) logging with level filtering.
        /// Each call requires the log level of submitted message to be specified (1, 2, or 3).
        /// </summary>
        public void Msg(int _msgLogLevel, string _message)
        {
            // model this after the Logger.Warn method & comments, but with Msg (message) specific logic
            // replace warn parameters with msg parameters (will remove these directions in future)
            try
            {
                // Only log if debug is enabled and the _msgLogLevel is within the CurrentMsgLogLevel
                // This allows for dynamic control of message verbosity based on the CurrentMsgLogLevel
                // isDebugEnabled is used to disable all logging except errors in production builds
                // Future will include user's ability to set log levels dynamically in game
                if (_msgLogLevel < 1 || _msgLogLevel > 3)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case, use helper)
                    this.Err(string.Format("[ERROR] Invalid log level {0} for Msg method. Must be 1, 2, or 3.", _msgLogLevel));
                    return;
                }
                // Check if IsDebugEnabled and if the CurrentMsgLogLevel allows (greater than or equal to this message's submitted _msgLogLevel)
                if (IsDebugEnabled && CurrentMsgLogLevel >= _msgLogLevel) 
                    return;

                // Check if IsDebugEnabled and if the CurrentMsgLogLevel allows (greater than or equal to this message's submitted _msgLogLevel)
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentMsgLogLevel, _msgLogLevel) : "";
                // showLogLevelCalc is used to show the current log level calculation for clarity in logs
                MelonLogger.Msg(string.Format("[Info]{0} {1}", calcAdd, _message ?? "[null message]"));
            }
            // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
            // Fallback when exception occurs during Logger.Msg call uses System.Console.WriteLine directly
            catch (Exception ex)
            {
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Msg exception while logging message: {0} Exception: {1}\nStack Trace: {2}", _message ?? "null", ex.Message, ex.StackTrace)); }
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
        /// Warning (Warn) logging with level filtering
        /// Each call requires the log level of submitted warning to be specified (1 or 2).
        /// Only logs if the debug loging mode is enabled and the warning log level is within the current warn log level thresholds.
        /// </summary>
        public void Warn(int _warnLogLevel, string _warningMessage)
        {
            try
            {
                // Only log if debug is enabled and the _warnLogLevel is within the CurrentWarnLogLevel
                // This allows for dynamic control of warning verbosity based on the CurrentWarnLogLevel
                // isDebugEnabled is used to disable all logging except errors in production builds
                // Future will include user's ability to set log levels dynamically in game
                if (_warnLogLevel < 1 || _warnLogLevel > 2)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case, use helper)
                    this.Warn(1, string.Format("[ERROR] Invalid log level {0} for Warn method. Must be 1 or 2.", _warnLogLevel));
                    return;
                }
                // Check if debug mode is enabled and if the CurrentWarnLogLevel allows (greater than or equal to this message's submitted _warnLogLevel)
                if (IsDebugEnabled && CurrentWarnLogLevel >= _warnLogLevel) return;
                // showLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", _warnLogLevel, _warnLogLevel) : "";
                MelonLogger.Warning(string.Format("[WARN]{0} {1}", calcAdd, _warningMessage ?? "[null message]"));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try { System.Console.WriteLine("[CRITICAL] Logger.Warn exception while logging warning: {0} Exception: {1}\nStack Trace: {2}", _warningMessage ?? "null", ex.Message, ex.StackTrace); }
                catch { /* Ultimate fallback - do nothing */ }
            }
        
        }

        /// <summary>
        /// Error logging (always active, highest priority)
        /// </summary>
        public void Err(string _errorMessage)
        {
            try
            {
                // Always log errors regardless of debug mode or log levels
                MelonLogger.Error(string.Format("[ERROR] {0}", _errorMessage ?? "[null error message]"));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try { System.Console.WriteLine("[CRITICAL] Logger.Err exception while logging error: {0} Exception: {1}\nStack Trace: {2}", _errorMessage ?? "null",ex.Message, ex.StackTrace); }
                catch { /* Ultimate fallback - do nothing */ }
            }
        }
    }
}