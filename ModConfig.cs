using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MixerThreholdMod_0_0_1
{
    public static class ModConfig
    {
        public static void Load()
        {
            var cat = MelonPreferences.GetCategory("MixerThreholdMod");

            Logger.IsDebugEnabled = cat.GetEntry<bool>("EnableDebugLogging").Value;
            Logger.CurrentMsgLogLevel = cat.GetEntry<int>("CurrentMsgLogLevel").Value;
            Logger.CurrentWarnLogLevel = cat.GetEntry<int>("CurrentWarnLogLevel").Value;
            Logger.ShowLogLevelCalc = cat.GetEntry<bool>("ShowLogLevelCalc").Value;
        }

        public static void HookRealtimeUpdates()
        {
            var cat = MelonPreferences.GetCategory("MixerThreholdMod");

            var enableDebug = cat.GetEntry<bool>("EnableDebugLogging");
            var msgLogLevel = cat.GetEntry<int>("CurrentMsgLogLevel");
            var warnLogLevel = cat.GetEntry<int>("CurrentWarnLogLevel");
            var showCalc = cat.GetEntry<bool>("ShowLogLevelCalc");

            HookObsoleteEvent(enableDebug, (oldVal, newVal) =>
            {
                Logger.IsDebugEnabled = newVal;
            });

            HookObsoleteEvent(msgLogLevel, (oldVal, newVal) =>
            {
                Logger.CurrentMsgLogLevel = newVal;
            });

            HookObsoleteEvent(warnLogLevel, (oldVal, newVal) =>
            {
                Logger.CurrentWarnLogLevel = newVal;
            });

            HookObsoleteEvent(showCalc, (oldVal, newVal) =>
            {
                Logger.ShowLogLevelCalc = newVal;
            });
        }

        private static void HookObsoleteEvent<T>(MelonPreferences_Entry<T> entry, Action<T, T> handler)
        {
            var eventInfo = typeof(MelonPreferences_Entry<T>).GetEvent("OnValueChanged", BindingFlags.Public | BindingFlags.Instance);
            if (eventInfo == null) return;

            var method = handler.Method;
            var del = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, method);
            eventInfo.AddEventHandler(entry, del);
        }
    }
}