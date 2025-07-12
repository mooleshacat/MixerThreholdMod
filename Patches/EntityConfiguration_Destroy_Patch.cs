using HarmonyLib;
using System;
using System.Linq;
using HarmonyLib;

namespace MixerThreholdMod_1_0_0.Core
{
    [HarmonyPatch(typeof(EntityConfiguration), "Destroy")]
    public static class EntityConfiguration_Destroy_Patch
    {
        public static void Prefix(EntityConfiguration __instance)
        {
            try
            {
                Main.logger.Msg(2, string.Format("EntityConfiguration.Destroy() called mixer"));

                var trackedMixers = TrackedMixers.ToListAsync().Result;

                var mixerData = trackedMixers.FirstOrDefault(tm => tm != null && tm.ConfigInstance == __instance);
                if (mixerData != null)
                {
                    TrackedMixers.RemoveAsync(mixerData.ConfigInstance).Wait();
                    Main.logger.Msg(2, string.Format("Removed mixer from tracked list (via EntityConfiguration.Destroy)"));
                }
                else
                {
                    if (mixingStationConfigType == null)
                    {
                        MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] MixingStationConfiguration type not available - skipping cleanup check");
                    }
                    else
                    {
                        MixerThreholdMod_1_0_0.Main.logger?.Msg(3, string.Format("[PATCH] Instance type {0} is not MixingStationConfiguration - no cleanup needed",
                            instanceType.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("EntityConfiguration_Destroy_Patch: Failed to save game and/or preferences and/or backup"));
                Main.logger.Err(string.Format("EntityConfiguration_Destroy_Patch: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}