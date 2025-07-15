using ScheduleOne.Management;
using System;
using System.Linq;
using HarmonyLib;

namespace MixerThreholdMod_1_0_0.Core
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
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        Exception cleanupError = null;
                        try
                        {
                            // Remove from tracked mixers safely
                            bool removed = await TrackedMixers.RemoveAsync(mixerConfig);
                            if (removed)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking");
                            }

                            // Remove from ID manager
                            bool idRemoved = Core.MixerIDManager.RemoveMixerID(mixerConfig);
                            if (idRemoved)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager");
                            }
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

                Main.logger.Msg(3, "[PATCH] EntityConfiguration.Destroy() called - checking for mixer cleanup");

                // Check if this is a mixer configuration that needs cleanup
                var mixerConfig = __instance as MixingStationConfiguration;
                if (mixerConfig != null)
                {
                    // Safe cleanup using background task to not block destruction
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        Exception cleanupError = null;
                        try
                        {
                            // Remove from tracked mixers safely
                            bool removed = await TrackedMixers.RemoveAsync(mixerConfig);
                            if (removed)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking");
                            }

                            // Remove from ID manager
                            bool idRemoved = Core.MixerIDManager.RemoveMixerID(mixerConfig);
                            if (idRemoved)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager");
                            }
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

                Main.logger.Msg(2, $"EntityConfiguration.Destroy() called for mixer");

                // Check if this is a mixer configuration that needs cleanup
                var mixerConfig = __instance as MixingStationConfiguration;
                if (mixerConfig != null)
                {
                    // Safe cleanup using background task to not block destruction
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        // Get a snapshot of tracked mixers
                        var trackedMixers = await TrackedMixers.ToListAsync();
                        if (trackedMixers == null)
                        {
                            // Remove from tracked mixers safely
                            bool removed = await TrackedMixers.RemoveAsync(mixerConfig);
                            if (removed)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking");
                            }

                            // Remove from ID manager
                            bool idRemoved = Core.MixerIDManager.RemoveMixerID(mixerConfig);
                            if (idRemoved)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager");
                            }
                        }
                        catch (Exception ex)
                        {
                            cleanupError = ex;
                        }
                Main.logger.Msg(2, $"EntityConfiguration.Destroy() called mixer");

                // Use async helper to properly handle the removal without blocking
                Task.Run(async () =>
                {
                    // Safe cleanup using background task to not block destruction
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        // Get a snapshot of tracked mixers
                        var trackedMixers = await TrackedMixers.ToListAsync().ConfigureAwait(false);

                        if (cleanupError != null)
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
                            Main.logger.Err(string.Format("[PATCH] CRASH PREVENTION: Cleanup error: {0}", cleanupError.Message));
                            // Don't re-throw - let cleanup fail gracefully
                        }
                    });
                }
                        Exception cleanupError = null;
                        try
                        {
                            // Remove from tracked mixers safely
                            bool removed = await TrackedMixers.RemoveAsync(mixerConfig);
                            if (removed)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking");
                            }

                            // Remove from ID manager
                            bool idRemoved = Core.MixerIDManager.RemoveMixerID(mixerConfig);
                            if (idRemoved)
                            {
                                Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager");
                            }
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