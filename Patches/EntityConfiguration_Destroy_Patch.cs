using ScheduleOne.Management;
using System;
using System.Threading.Tasks;
using HarmonyLib;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Patches
{
    /// <summary>
    /// Harmony patch for EntityConfiguration.Destroy to handle mixer cleanup.
    /// Ensures proper cleanup when mixer entities are destroyed to prevent memory leaks.
    /// 
    /// ⚠️ THREAD SAFETY WARNING: This patch executes cleanup operations asynchronously
    /// to prevent blocking Unity's main thread during entity destruction. All async 
    /// operations use ConfigureAwait(false) to prevent deadlocks.
    /// 
    /// ⚠️ .NET 4.8.1 COMPATIBILITY: Uses string.Format() instead of string interpolation,
    /// explicit type declarations, and IL2CPP-compatible async patterns.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: All cleanup operations include comprehensive error
    /// handling and never throw exceptions that could crash entity destruction.
    /// </summary>
    [HarmonyPatch(typeof(EntityConfiguration), "Destroy")]
    public static class EntityConfiguration_Destroy_Patch
    {
        /// <summary>
        /// Prefix patch that runs before EntityConfiguration.Destroy
        /// ⚠️ THREAD SAFETY: Uses async cleanup to prevent main thread blocking
        /// </summary>
        /// <param name="__instance">The EntityConfiguration instance being destroyed</param>
        public static void Prefix(EntityConfiguration __instance)
        {
            Exception patchError = null;
            try
            {
                if (__instance == null)
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, string.Format(
                        "[PATCH] {0}: Instance is null", 
                        PATCH_ENTITY_DESTROY_NAME));
                    return;
                }

                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format(
                    "[PATCH] {0}: EntityConfiguration.Destroy() called - checking for mixer cleanup", 
                    PATCH_ENTITY_DESTROY_NAME));

                // Check if this is a mixer configuration that needs cleanup
                var mixerConfig = __instance as MixingStationConfiguration;
                if (mixerConfig != null)
                {
                    // Perform cleanup asynchronously to not block entity destruction
                    // ⚠️ THREAD SAFETY: ConfigureAwait(false) prevents deadlocks
                    Task.Run(async () =>
                    {
                        Exception cleanupError = null;
                        try
                        {
                            Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format(
                                "[PATCH] {0}: Starting cleanup for mixer configuration", 
                                PATCH_ENTITY_DESTROY_NAME));

                            // Remove from tracked mixers safely
                            bool removed = await Core.MixerConfigurationTracker.RemoveAsync(mixerConfig)
                                .ConfigureAwait(false);
                            
                            if (removed)
                            {
                                Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format(
                                    "[PATCH] {0}: Mixer configuration removed from tracking", 
                                    PATCH_ENTITY_DESTROY_NAME));
                            }
                            else
                            {
                                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format(
                                    "[PATCH] {0}: Mixer configuration was not in tracking list", 
                                    PATCH_ENTITY_DESTROY_NAME));
                            }

                            // Remove from ID manager if it exists
                            try
                            {
                                bool idRemoved = Core.MixerIDManager.RemoveMixerID(mixerConfig);
                                if (idRemoved)
                                {
                                    Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format(
                                        "[PATCH] {0}: Mixer configuration removed from ID manager", 
                                        PATCH_ENTITY_DESTROY_NAME));
                                }
                            }
                            catch (Exception idEx)
                            {
                                Main.logger.Warn(WARN_LEVEL_VERBOSE, string.Format(
                                    "[PATCH] {0}: Warning during ID manager cleanup: {1}", 
                                    PATCH_ENTITY_DESTROY_NAME, idEx.Message));
                                // Continue with cleanup - ID manager errors are not critical
                            }
                        }
                        catch (Exception ex)
                        {
                            cleanupError = ex;
                        }

                        // ⚠️ CRASH PREVENTION: Never let cleanup errors crash entity destruction
                        if (cleanupError != null)
                        {
                            Main.logger.Err(string.Format(
                                "[PATCH] {0} CRASH PREVENTION: Cleanup error: {1}\nStackTrace: {2}", 
                                PATCH_ENTITY_DESTROY_NAME, 
                                cleanupError.Message, 
                                cleanupError.StackTrace ?? NULL_MESSAGE_FALLBACK));
                            // Graceful degradation - don't re-throw
                        }
                    }).ConfigureAwait(false);
                }
                else
                {
                    Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format(
                        "[PATCH] {0}: Not a mixer configuration, no cleanup needed", 
                        PATCH_ENTITY_DESTROY_NAME));
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }

            // ⚠️ CRITICAL CRASH PREVENTION: Never let patch failures crash entity destruction
            if (patchError != null)
            {
                Main.logger.Err(string.Format(
                    "[PATCH] {0} CRASH PREVENTION: Patch error: {1}\nStackTrace: {2}", 
                    PATCH_ENTITY_DESTROY_NAME, 
                    patchError.Message, 
                    patchError.StackTrace ?? NULL_MESSAGE_FALLBACK));
                // NEVER re-throw patch errors - entity destruction must continue
            }
        }
    }
}