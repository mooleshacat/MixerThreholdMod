using MelonLoader;
<<<<<<< HEAD

namespace MixerThreholdMod_0_0_1.Core
=======
using System;

namespace MixerThreholdMod_1_0_0.Core
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
{
    /// <summary>
    /// Centralized logging system with configurable verbosity levels.
    /// Thread-safe logging for .NET 4.8.1 compatibility.
    /// 
    /// ⚠️ THREAD SAFETY: This class is thread-safe and can be called from any thread.
    /// All methods are designed to prevent exceptions and provide fallback behavior.
    /// 
<<<<<<< HEAD
=======
    /// ⚠️ MODIFICATION WARNING: This logging helper is critical for crash prevention and debugging.
    /// Be extremely careful when modifying this class as it provides fallback logging when other
    /// systems fail. Recent issues occurred when Copilot accidentally inverted the logging logic,
    /// causing no output. Always test logging functionality after modifications.
    /// 
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible with framework's threading model
    /// - Proper exception handling for crash prevention
    /// </summary>
    public class Logger
    {
<<<<<<< HEAD
        public bool IsDebugEnabled = true;
        public int CurrentMsgLogLevel = 3; // 1, 2, or 3
        public int CurrentWarnLogLevel = 2; // 1 or 2
        public bool ShowLogLevelCalc = false; // Simplified default

        /// <summary>
        /// Info (Msg) logging with level filtering
        /// </summary>
        public void Msg(int logLevel, string message)
        {
            try
            {
                if (!IsDebugEnabled || CurrentMsgLogLevel < logLevel) return;
                
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentMsgLogLevel, logLevel) : "";
                MelonLogger.Msg(string.Format("[Info]{0} {1}", calcAdd, message ?? "[null message]"));
            }
            catch
            {
                // Fallback logging - never let logging crash the mod
                try { MelonLogger.Msg("[ERROR] Logging failure in Logger.Msg"); }
=======
#if DEBUG
        public bool IsDebugEnabled = true; // disable for production builds (disables all logging except errors)
#else
        public bool IsDebugEnabled = false; // disable for production builds (disables all logging except errors)
#endif
        public int CurrentMsgLogLevel = 3; // current dev desired msg log level (1, 2, or 3)
        public int CurrentWarnLogLevel = 2; // current dev desired warn log level (1 or 2)
        public bool ShowLogLevelCalc = true; // Simplified default (leave as true for clarity, for human)

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
                if (msgLogLevel < 1 || msgLogLevel > 3)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case, use helper)
                    this.Err(string.Format("[ERROR] Invalid log level {0} for Msg method. Must be 1, 2, or 3.", msgLogLevel));
                    return;
                }
                // Check if debug mode is enabled and if the CurrentMsgLogLevel allows (greater than or equal to this message's submitted msgLogLevel)
                if (!IsDebugEnabled || CurrentMsgLogLevel < msgLogLevel) 
                    return;

                // ShowLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentMsgLogLevel, msgLogLevel) : "";
                MelonLogger.Msg(string.Format("[Info]{0} {1}", calcAdd, message ?? "[null message]"));
            }
            // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
            // Fallback when exception occurs during Logger.Msg call uses System.Console.WriteLine directly
            catch (Exception ex)
            {
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Msg exception while logging message: {0} Exception: {1}\nStack Trace: {2}", message ?? "null", ex.Message, ex.StackTrace)); }
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
<<<<<<< HEAD
        /// Warning (Warn) logging with level filtering
        /// </summary>
        public void Warn(int logLevel, string message)
        {
            try
            {
                if (!IsDebugEnabled || CurrentWarnLogLevel < logLevel) return;
                
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentWarnLogLevel, logLevel) : "";
                MelonLogger.Warning(string.Format("[WARN]{0} {1}", calcAdd, message ?? "[null message]"));
            }
            catch
            {
                try { MelonLogger.Warning("[ERROR] Logging failure in Logger.Warn"); }
=======
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
                if (warnLogLevel < 1 || warnLogLevel > 2)
                {
                    // Fallback for invalid log levels (should never hit this, but just in case, use helper)
                    this.Warn(1, string.Format("[ERROR] Invalid log level {0} for Warn method. Must be 1 or 2.", warnLogLevel));
                    return;
                }
                // Check if debug mode is enabled and if the CurrentWarnLogLevel allows (greater than or equal to this message's submitted warnLogLevel)
                if (!IsDebugEnabled || CurrentWarnLogLevel < warnLogLevel) return;
                // ShowLogLevelCalc is used to show the current log level calculation for clarity in logs
                var calcAdd = ShowLogLevelCalc ? string.Format("[{0}]>=[{1}]", CurrentWarnLogLevel, warnLogLevel) : "";
                MelonLogger.Warning(string.Format("[WARN]{0} {1}", calcAdd, warningMessage ?? "[null message]"));
            }
            catch (Exception ex)
            {
                // Fallback for caught exception (can we rely on custom logging here? It may be broken!)
                // Always log exceptions AND stack traces to ensure we capture critical failures (even in logger)
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Warn exception while logging warning: {0} Exception: {1}\nStack Trace: {2}", warningMessage ?? "null", ex.Message, ex.StackTrace)); }
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
<<<<<<< HEAD
        /// Error logging (always active, highest priority)
        /// </summary>
        public void Err(string message)
        {
            try
            {
                MelonLogger.Error(string.Format("[ERROR] {0}", message ?? "[null error message]"));
            }
            catch
            {
                // Last resort - try basic console output
                try { System.Console.WriteLine("[CRITICAL] Logger.Err failed: " + (message ?? "null")); }
=======
        /// Error logging (always active, highest priority).
        /// Logs error messages regardless of debug mode or log level settings.
        /// </summary>
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
                try { System.Console.WriteLine(string.Format("[CRITICAL] Logger.Err exception while logging error: {0} Exception: {1}\nStack Trace: {2}", errorMessage ?? "null", ex.Message, ex.StackTrace)); }
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                catch { /* Ultimate fallback - do nothing */ }
            }
        }
    }
}