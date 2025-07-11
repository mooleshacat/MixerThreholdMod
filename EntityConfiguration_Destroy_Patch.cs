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
                if (__instance == null)
                {
                    Main.logger.Warn(1, "EntityConfiguration_Destroy_Patch: __instance is null");
                    return;
                }

                Main.logger.Msg(2, $"EntityConfiguration.Destroy() called for mixer");

                // Use fire-and-forget async to avoid blocking the game thread
                Task.Run(async () =>
                {
                    try
                    {
                        // Get a snapshot of tracked mixers
                        var trackedMixers = await TrackedMixers.ToListAsync();
                        if (trackedMixers == null)
                        {
                            Main.logger.Warn(1, "EntityConfiguration_Destroy_Patch: trackedMixers is null");
                            return;
                        }

                        // Find if this instance is tracked
                        var mixerData = trackedMixers.FirstOrDefault(tm => tm != null && tm.ConfigInstance == __instance);
                        if (mixerData != null)
                        {
                            // Remove from shared tracked list
                            await TrackedMixers.RemoveAsync(mixerData.ConfigInstance);
                            Main.logger.Msg(2, $"Removed mixer {mixerData.MixerInstanceID} from tracked list (via EntityConfiguration.Destroy)");
                        }
                        else
                        {
                            Main.logger.Msg(3, "EntityConfiguration_Destroy_Patch: No matching mixer found in tracked list");
                        }
                    }
                    catch (Exception asyncEx)
                    {
                        Main.logger.Err($"EntityConfiguration_Destroy_Patch: Error in async cleanup: {asyncEx.Message}\n{asyncEx.StackTrace}");
                    }
                });
            }
            catch (Exception ex)
            {
                // catchall at patch level, where my DLL interacts with the game and it's engine
                // hopefully should catch errors in entire project?
                Main.logger.Err($"EntityConfiguration_Destroy_Patch: Failed during mixer cleanup");
                Main.logger.Err($"EntityConfiguration_Destroy_Patch: Caught exception: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}