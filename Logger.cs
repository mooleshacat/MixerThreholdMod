using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScheduleOne.Console;

namespace MixerThreholdMod_1_0_0
{
    public class Logger
    {
        // Configurable via settings or mod prefs
        public bool IsDebugEnabled = true;
        public int CurrentMsgLogLevel = 3; // 1, 2, or 3
        public int CurrentWarnLogLevel = 2; // 1 or 2
        public bool ShowLogLevelCalc = true;
        // Info (Msg) logging
        public void Msg(int logLevel, string message)
        {
            var _calcAdd = "";
            if (ShowLogLevelCalc)
                _calcAdd = $"[{CurrentMsgLogLevel}]>=[{logLevel}]";
            if (IsDebugEnabled && CurrentMsgLogLevel >= logLevel)
                MelonLogger.Msg($"[Info]" + _calcAdd + $" {message}");
        }
        // Warning (Warn) logging
        public void Warn(int logLevel, string message)
        {
            var _calcAdd = "";
            if (ShowLogLevelCalc)
                _calcAdd = $"[{CurrentWarnLogLevel}]>=[{logLevel}]";
            if (IsDebugEnabled && CurrentWarnLogLevel >= logLevel)
                MelonLogger.Warning($"[WARN]" + _calcAdd + $" {message}");
        }
        // Error logging (always active)
        public void Err(string message)
        {
            MelonLogger.Error($"[ERROR] {message}");
        }
    }
}