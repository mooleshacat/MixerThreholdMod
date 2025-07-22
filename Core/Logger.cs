using System;

using MelonLoader;

using MixerThreholdMod_1_0_0.Core;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Centralized logging system with configurable verbosity levels.
    /// Thread-safe logging for .NET 4.8.1 compatibility.
    /// 
    ///  THREAD SAFETY: This class is thread-safe and can be called from any thread.
    /// All methods are designed to prevent exceptions and provide fallback behavior.
    /// 
    ///  MODIFICATION WARNING: This logging helper is critical for crash prevention and debugging.
    /// Be extremely careful when modifying this class as it provides fallback logging when other
    /// systems fail. Recent issues occurred when Copilot accidentally inverted the logging logic,
    /// causing no output. Always test logging functionality after modifications.
    /// 
    ///  COMPILER DIRECTIVE WARNING: Logging behavior is controlled by #if DEBUG. Changing build configuration
    /// will affect IsDebugEnabled and all non-error logging. Review this before modifying or deploying.
    /// 
    ///  .NET 4.8.1 COMPATIBLE: Uses string.Format instead of string interpolation
    /// - Uses string.Format instead of string interpolation
    /// - Compatible with framework's threading model
    /// - Proper exception handling for crash prevention
    /// </summary>
    public class Logger
    {
#if DEBUG
        public bool IsDebugEnabled = true; // Enable for debug builds
#else
        public bool IsDebugEnabled = false; // Disable for production builds (disables all logging except errors)
#endif
        public int CurrentMsgLogLevel = LOG_LEVEL_VERBOSE;
        public int CurrentWarnLogLevel = LOG_LEVEL_VERBOSE;
        public bool ShowLogLevelCalc = true;

        /// <summary>
        /// Info (Msg) logging with level filtering.
        /// Each call requires the log level of submitted message to be specified (1, 2, or 3).
        /// Only logs when debug mode is enabled and the message level is within current thresholds.
        ///  THREAD SAFETY: Thread-safe method with comprehensive exception handling
        /// </summary>
        /// <param name="msgLogLevel">Log level for this message (1-3, where 1 is highest priority)</param>
        /// <param name="message">Message to log</param>
        public void Msg(int msgLogLevel, string message)
        {
            try
            {
                // Validate log level range
                if (msgLogLevel < LOG_LEVEL_CRITICAL || msgLogLevel > LOG_LEVEL_VERBOSE)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case)
                    this.Err(string.Format(INVALID_MSG_LEVEL_ERROR, msgLogLevel, LOG_LEVEL_CRITICAL, LOG_LEVEL_VERBOSE));
                    return;
                }

                // Check if debug mode is enabled and if the CurrentMsgLogLevel allows this message
                // CurrentMsgLogLevel >= msgLogLevel means we should log this message
                if (!IsDebugEnabled || CurrentMsgLogLevel < msgLogLevel)
                {
                    return;
                }

                // ShowLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentMsgLogLevel, msgLogLevel) : "";
                MelonLogger.Msg(string.Format("[Info]{0} {1}", calcAdd, message ?? NULL_MESSAGE_FALLBACK));
            }
            catch (Exception ex)
            {
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                // Fallback when exception occurs during Logger.Msg call uses System.Console.WriteLine directly
                try
                {
                    System.Console.WriteLine(string.Format("[CRITICAL] Logger.Msg exception while logging message: {0} Exception: {1}\nStack Trace: {2}",
                        message ?? "null", ex.Message, ex.StackTrace));
                }
                catch
                {
                    /* Ultimate fallback - do nothing */
                }
            }
        }

        /// <summary>
        /// Warning (Warn) logging with level filtering.
        /// Each call requires the log level of submitted warning to be specified (1 or 2).
        /// Only logs when debug mode is enabled and the warning level is within current thresholds.
        ///  THREAD SAFETY: Thread-safe method with comprehensive exception handling
        /// </summary>
        /// <param name="warnLogLevel">Warning level for this message (1-2, where 1 is highest priority)</param>
        /// <param name="warningMessage">Warning message to log</param>
        public void Warn(int warnLogLevel, string warningMessage)
        {
            try
            {
                // Validate warning level range
                if (warnLogLevel < WARN_LEVEL_CRITICAL || warnLogLevel > WARN_LEVEL_VERBOSE)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case)
                    this.Err(string.Format(INVALID_WARN_LEVEL_ERROR, warnLogLevel, WARN_LEVEL_CRITICAL, WARN_LEVEL_VERBOSE));
                    return;
                }

                // Check if debug mode is enabled and if the CurrentWarnLogLevel allows this warning
                // CurrentWarnLogLevel >= warnLogLevel means we should log this warning
                if (!IsDebugEnabled || CurrentWarnLogLevel < warnLogLevel)
                {
                    return;
                }

                // ShowLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentWarnLogLevel, warnLogLevel) : "";
                MelonLogger.Warning(string.Format("[WARN]{0} {1}", calcAdd, warningMessage ?? NULL_MESSAGE_FALLBACK));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try
                {
                    System.Console.WriteLine(string.Format("[CRITICAL] Logger.Warn exception while logging warning: {0} Exception: {1}\nStack Trace: {2}",
                        warningMessage ?? "null", ex.Message, ex.StackTrace));
                }
                catch
                {
                    /* Ultimate fallback - do nothing */
                }
            }
        }

        /// <summary>
        /// Error logging (always active, highest priority).
        /// Logs error messages regardless of debug mode or log level settings.
        ///  THREAD SAFETY: Thread-safe method with comprehensive exception handling
        /// </summary>
        /// <param name="errorMessage">Error message to log</param>
        public void Err(string errorMessage)
        {
            try
            {
                // Always log errors regardless of debug mode or log levels
                MelonLogger.Error(string.Format("[ERROR] {0}", errorMessage ?? "[null error message]"));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try
                {
                    System.Console.WriteLine(string.Format("[CRITICAL] Logger.Err exception while logging error: {0} Exception: {1}\nStack Trace: {2}",
                        errorMessage ?? "null", ex.Message, ex.StackTrace));
                }
                catch
                {
                    /* Ultimate fallback - do nothing */
                }
            }
        }
    }
}