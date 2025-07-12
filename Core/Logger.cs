using MelonLoader;

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
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
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
                catch { /* Ultimate fallback - do nothing */ }
            }
        }

        /// <summary>
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
                catch { /* Ultimate fallback - do nothing */ }
            }
        }
    }
}