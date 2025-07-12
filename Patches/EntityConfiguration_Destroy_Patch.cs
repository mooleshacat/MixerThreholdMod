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
                if (__instance == null)
                {
                    TrackedMixers.RemoveAsync(mixerData.ConfigInstance).Wait();
                    Main.logger.Msg(2, string.Format("Removed mixer from tracked list (via EntityConfiguration.Destroy)"));
                }

                Main.logger.Msg(2, $"EntityConfiguration.Destroy() called for mixer");

                // Use fire-and-forget async to avoid blocking the game thread
                Task.Run(async () =>
                {
                    try
                    {
                        // Get a snapshot of tracked mixers
                        var trackedMixers = await TrackedMixers.ToListAsync().ConfigureAwait(false);
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
                            await TrackedMixers.RemoveAsync(mixerData.ConfigInstance).ConfigureAwait(false);
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
                Main.logger.Err(string.Format("EntityConfiguration_Destroy_Patch: Failed to save game and/or preferences and/or backup"));
                Main.logger.Err(string.Format("EntityConfiguration_Destroy_Patch: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}