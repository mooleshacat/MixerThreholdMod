using ScheduleOne.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace MixerThreholdMod_0_0_1
{
    [HarmonyPatch(typeof(EntityConfiguration), "Destroy")]
    public static class EntityConfiguration_Destroy_Patch
    {
        public static void Prefix(EntityConfiguration __instance)
        {
            try
            {
                Main.logger.Msg(2, $"EntityConfiguration.Destroy() called mixer");

                // Get a snapshot of tracked mixers
                var trackedMixers = TrackedMixers.ToListAsync().Result;

                // Find if this instance is tracked
                var mixerData = trackedMixers.FirstOrDefault(tm => tm != null && tm.ConfigInstance == __instance);
                if (mixerData != null)
                {
                    // Remove from shared tracked list
                    TrackedMixers.RemoveAsync(mixerData.ConfigInstance).Wait();

                    Main.logger.Msg(2, $"Removed mixer from tracked list (via EntityConfiguration.Destroy)");
                }
            }
            catch (Exception ex)
            {
                // catchall at patch level, where my DLL interacts with the game and it's engine
                // hopefully should catch errors in entire project?
                Main.logger.Err($"EntityConfiguration_Destroy_Patch: Failed to save game and/or preferences and/or backup");
                Main.logger.Err($"EntityConfiguration_Destroy_Patch: Caught exception: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}