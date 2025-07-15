using MelonLoader;
using System;
using MixerThreholdMod_1_0_0.Constants;    // ✅ ESSENTIAL - Keep this! Our constants!

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Centralized logging system with configurable verbosity levels.
    /// Thread-safe logging for .NET 4.8.1 compatibility.
    /// 
    /// ⚠️ THREAD SAFETY: This class is thread-safe and can be called from any thread.
    /// All methods are designed to prevent exceptions and provide fallback behavior.
    /// 
    /// ⚠️ MODIFICATION WARNING: This logging helper is critical for crash prevention and debugging.
    /// Be extremely careful when modifying this class as it provides fallback logging when other
    /// systems fail. Recent issues occurred when Copilot accidentally inverted the logging logic,
    /// causing no output. Always test logging functionality after modifications.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible with framework's threading model
    /// - Proper exception handling for crash prevention
    /// </summary>
    public class Logger
    {
#if DEBUG
        public bool IsDebugEnabled = true; // disable for production builds (disables all logging except errors)
#else
        public bool IsDebugEnabled = false; // disable for production builds (disables all logging except errors)
#endif
        public int CurrentMsgLogLevel = ModConstants.LOG_LEVEL_VERBOSE; // current/default dev desired msg log level (1, 2, or 3) (human changes this only)
        public int CurrentWarnLogLevel = ModConstants.WARN_LEVEL_VERBOSE; // current/default dev desired warn log level (1 or 2) (human changes this only)
        public bool ShowLogLevelCalc = true; // current/default simplified log level calculation (human changes this only)

        /// <summary>
        /// Info (Msg) logging with level filtering.
        /// Each call requires the log level of submitted message to be specified (1, 2, or 3).
        /// Only logs when debug mode is enabled and the message level is within current thresholds.
        /// </summary>
        public void Msg(int msgLogLevel, string message)
        {
            try
            {
                // Only log if debug is enabled and the msgLogLevel is within the CurrentMsgLogLevel
                // This allows for dynamic control of message verbosity based on the CurrentMsgLogLevel
                // IsDebugEnabled is used to disable all logging except errors in production builds
                // Future will include user's ability to set log levels dynamically in game
                if (msgLogLevel < ModConstants.LOG_LEVEL_CRITICAL || msgLogLevel > ModConstants.LOG_LEVEL_VERBOSE)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case, use helper)
                    this.Err(string.Format(ModConstants.INVALID_MSG_LEVEL_ERROR, msgLogLevel, ModConstants.LOG_LEVEL_CRITICAL, ModConstants.LOG_LEVEL_VERBOSE));
                    return;
                }
                // Check if debug mode is enabled and if the CurrentMsgLogLevel allows (greater than or equal to this message's submitted msgLogLevel)
                if (!IsDebugEnabled || CurrentMsgLogLevel < msgLogLevel)
                    return;

                // ShowLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentMsgLogLevel, msgLogLevel) : "";
                MelonLogger.Msg(string.Format("{0}{1} {2}", ModConstants.LOG_PREFIX_INFO, calcAdd, message ?? ModConstants.NULL_MESSAGE_FALLBACK));
            }
            // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
            // Fallback when exception occurs during Logger.Msg call uses System.Console.WriteLine directly
            catch (Exception ex)
            {
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Msg exception while logging message: {0} Exception: {1}\nStack Trace: {2}", message ?? ModConstants.NULL_COMMAND_FALLBACK, ex.Message, ex.StackTrace)); }
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
        /// Warning (Warn) logging with level filtering.
        /// Each call requires the log level of submitted warning to be specified (1 or 2).
        /// Only logs when debug mode is enabled and the warning level is within current thresholds.
        /// </summary>
        public void Warn(int warnLogLevel, string warningMessage)
        {
            try
            {
                // Only log if debug is enabled and the warnLogLevel is within the CurrentWarnLogLevel
                // This allows for dynamic control of warning verbosity based on the CurrentWarnLogLevel
                // IsDebugEnabled is used to disable all logging except errors in production builds
                // Future will include user's ability to set log levels dynamically in game
                if (warnLogLevel < ModConstants.WARN_LEVEL_CRITICAL || warnLogLevel > ModConstants.WARN_LEVEL_VERBOSE)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case, use helper)
                    this.Warn(ModConstants.WARN_LEVEL_CRITICAL, string.Format(ModConstants.INVALID_WARN_LEVEL_ERROR,
                        warnLogLevel, ModConstants.WARN_LEVEL_CRITICAL, ModConstants.WARN_LEVEL_VERBOSE));
                    return;
                }
                // Check if debug mode is enabled and if the CurrentWarnLogLevel allows (greater than or equal to this message's submitted warnLogLevel)
                if (!IsDebugEnabled || CurrentWarnLogLevel < warnLogLevel) return;
                // ShowLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentWarnLogLevel, warnLogLevel) : "";
                MelonLogger.Warning(string.Format("{0}{1} {2}", ModConstants.LOG_PREFIX_WARN, calcAdd, warningMessage ?? ModConstants.NULL_MESSAGE_FALLBACK));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Warn exception while logging warning: {0} Exception: {1}\nStack Trace: {2}", warningMessage ?? ModConstants.NULL_COMMAND_FALLBACK, ex.Message, ex.StackTrace)); }
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
        /// Error logging (always active, highest priority).
        /// Logs error messages regardless of debug mode or log level settings.
        /// </summary>
        public void Err(string errorMessage)
        {
            try
            {
                // Always log errors regardless of debug mode or log levels
                MelonLogger.Error(string.Format("{0} {1}", ModConstants.LOG_PREFIX_ERROR, errorMessage ?? ModConstants.NULL_ERROR_FALLBACK));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Err exception while logging error: {0} Exception: {1}\nStack Trace: {2}", errorMessage ?? ModConstants.NULL_COMMAND_FALLBACK, ex.Message, ex.StackTrace)); }
                catch { /* Ultimate fallback - do nothing */ }
            }
        }
    }
}