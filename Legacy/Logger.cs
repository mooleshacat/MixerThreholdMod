

using MelonLoader;

using MixerThreholdMod_1_0_0.Core;

﻿using MelonLoader;
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
                _calcAdd = string.Format("[{0}]>=[{1}]", CurrentMsgLogLevel, logLevel);
            if (IsDebugEnabled && CurrentMsgLogLevel >= logLevel)
                MelonLogger.Msg(string.Format("[Info]{0} {1}", _calcAdd, message));
        }

        // Warning (Warn) logging
        public void Warn(int logLevel, string message)
        {
            var _calcAdd = "";
            if (ShowLogLevelCalc)
                _calcAdd = string.Format("[{0}]>=[{1}]", CurrentWarnLogLevel, logLevel);
            if (IsDebugEnabled && CurrentWarnLogLevel >= logLevel)
                MelonLogger.Warning(string.Format("[WARN]{0} {1}", _calcAdd, message));
        }

        // Error logging (always active)
        public void Err(string message)
        {
            MelonLogger.Error(string.Format("[ERROR] {0}", message));
        }
    }
}