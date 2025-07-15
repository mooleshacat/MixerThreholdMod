using MelonLoader;
using System;
using System.Collections;
using MixerThreholdMod_1_0_0.Save;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Legacy save manager for backward compatibility
    /// ⚠️ DEPRECATED: Use CrashResistantSaveManager for new code
    /// ⚠️ THREAD SAFETY: All operations are thread-safe
    /// .NET 4.8.1 Compatible
    /// </summary>
    public static class MixerSaveManager
    {
        public static bool _hasLoggedZeroMixers = false;

        /// <summary>
        /// Load mixer values when ready - delegates to CrashResistantSaveManager
        /// </summary>
        public static IEnumerator LoadMixerValuesWhenReady()
        {
            Main.logger.Msg(2, "[LEGACY] MixerSaveManager.LoadMixerValuesWhenReady - delegating to CrashResistantSaveManager");
            yield return CrashResistantSaveManager.LoadMixerValuesWhenReady();
        }

        /// <summary>
        /// Attach listener when ready - delegates to CrashResistantSaveManager
        /// </summary>
        public static IEnumerator AttachListenerWhenReady(object config, int mixerID)
        {
            Main.logger.Msg(2, string.Format("[LEGACY] MixerSaveManager.AttachListenerWhenReady - delegating to CrashResistantSaveManager for Mixer {0}", mixerID));

            // Convert object to MixingStationConfiguration
            var mixerConfig = config as ScheduleOne.Management.MixingStationConfiguration;
            if (mixerConfig == null)
            {
                Main.logger.Err(string.Format("[LEGACY] Invalid config type for Mixer {0}", mixerID));
                yield break;
            }

            yield return CrashResistantSaveManager.AttachListenerWhenReady(mixerConfig, mixerID);
        }
    }
}