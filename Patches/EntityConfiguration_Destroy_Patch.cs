using ScheduleOne.Management;
using System;
using HarmonyLib;

namespace MixerThreholdMod_0_0_1.Patches
{
    /// <summary>
    /// Harmony patch for EntityConfiguration.Destroy to handle mixer cleanup.
    /// Ensures proper cleanup when mixer entities are destroyed to prevent memory leaks.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This patch ensures cleanup operations don't crash
    /// the game when entities are destroyed. All cleanup is done safely in background.
    /// 
    /// ⚠️ THREAD SAFETY: Cleanup operations are performed asynchronously to not block
    /// the main thread during entity destruction.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible exception handling patterns
    /// - Safe async cleanup patterns
    /// </summary>
    [HarmonyPatch(typeof(EntityConfiguration), "Destroy")]
    public static class EntityConfiguration_Destroy_Patch
    {
        /// <summary>
        /// Prefix patch that runs before EntityConfiguration.Destroy
        /// </summary>
        public static void Prefix(EntityConfiguration __instance)
        {
            Exception patchError = null;
            try
            {
                if (__instance == null)
                {
                    Main.logger.Warn(2, "[PATCH] EntityConfiguration_Destroy_Patch: Instance is null");
                    return;
                }

                Main.logger.Msg(3, "[PATCH] EntityConfiguration.Destroy() called - checking for mixer cleanup");

                // Check if this is a mixer configuration that needs cleanup
                var mixerConfig = __instance as MixingStationConfiguration;
                if (mixerConfig != null)
                {
                    // Safe cleanup using background task to not block destruction
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        Exception cleanupError = null;
                        try
                        {
                            // Remove from ID manager
                            bool removed = Core.MixerIDManager.RemoveMixerID(mixerConfig);
                            if (removed)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager");
                            }

                            // Remove from tracked mixers
                            Core.TrackedMixers.RemoveAll(tm => tm?.ConfigInstance == mixerConfig);
                            Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking");
                        }
                        catch (Exception ex)
                        {
                            cleanupError = ex;
                        }

                        if (cleanupError != null)
                        {
                            Main.logger.Err(string.Format("[PATCH] CRASH PREVENTION: Cleanup error: {0}", cleanupError.Message));
                            // Don't re-throw - let cleanup fail gracefully
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }

            if (patchError != null)
            {
                Main.logger.Err(string.Format("[PATCH] EntityConfiguration_Destroy_Patch CRASH PREVENTION: Patch error: {0}\nStackTrace: {1}", 
                    patchError.Message, patchError.StackTrace));
                // CRITICAL: Never let patch failures crash entity destruction
            }
        }
    }
}