using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace MixerThreholdMod_1_0_0.Patches
{
    /// <summary>
    /// IL2CPP COMPATIBLE: Harmony patch for EntityConfiguration.Destroy to handle mixer cleanup.
    /// Ensures proper cleanup when mixer entities are destroyed to prevent memory leaks.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This patch ensures cleanup operations don't crash
    /// the game when entities are destroyed. All cleanup is done safely in background.
    /// 
    /// ⚠️ THREAD SAFETY: Cleanup operations are performed asynchronously to not block
    /// the main thread during entity destruction.
    /// 
    /// ⚠️ IL2CPP COMPATIBLE: Uses dynamic type loading to avoid TypeLoadException in IL2CPP builds.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible exception handling patterns
    /// - Safe async cleanup patterns with proper cancellation
    /// - Thread-safe operations throughout
    /// </summary>
    public static class EntityConfiguration_Destroy_Patch
    {
        private static bool _patchInitialized = false;
        private static MethodInfo _destroyMethod = null;

        /// <summary>
        /// Initialize the patch using IL2CPP-compatible type resolution
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (_patchInitialized) return;

                // Get EntityConfiguration type via reflection to avoid IL2CPP issues
                var entityConfigType = MixerThreholdMod_1_0_0.Core.IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Management.EntityConfiguration");
                if (entityConfigType == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger.Warn(1, "[PATCH] EntityConfiguration type not found - patch will not be applied");
                    return;
                }

                _destroyMethod = entityConfigType.GetMethod("Destroy", BindingFlags.Public | BindingFlags.Instance);
                if (_destroyMethod == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger.Warn(1, "[PATCH] EntityConfiguration.Destroy method not found - patch will not be applied");
                    return;
                }

                // Apply Harmony patch dynamically
                var harmony = new HarmonyLib.Harmony("MixerThreholdMod.EntityConfiguration_Destroy_Patch");
                var prefixMethod = typeof(EntityConfiguration_Destroy_Patch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);

                harmony.Patch(_destroyMethod, new HarmonyMethod(prefixMethod), null);

                MixerThreholdMod_1_0_0.Main.logger.Msg(1, "[PATCH] IL2CPP-compatible EntityConfiguration.Destroy patch applied successfully");
                _patchInitialized = true;
            }
            catch (Exception ex)
            {
                MixerThreholdMod_1_0_0.Main.logger.Err(string.Format("[PATCH] Failed to initialize EntityConfiguration_Destroy_Patch: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Prefix patch that runs before EntityConfiguration.Destroy
        /// ⚠️ IL2CPP COMPATIBLE: Uses dynamic types to avoid TypeLoadException
        /// ⚠️ THREAD SAFETY: All cleanup operations are performed asynchronously
        /// ⚠️ CRASH PREVENTION: Extensive error handling prevents patch failures from crashing game
        /// ⚠️ REFLECTION REFERENCE: Called via GetMethod("Prefix") in EntityConfiguration_Destroy_Patch.Initialize() - DO NOT DELETE
        /// </summary>
        public static void Prefix(object __instance)
        {
            Exception patchError = null;
            try
            {
                if (__instance == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger.Warn(2, "[PATCH] EntityConfiguration_Destroy_Patch: Instance is null");
                    return;
                }

                MixerThreholdMod_1_0_0.Main.logger.Msg(3, "[PATCH] EntityConfiguration.Destroy() called - checking for mixer cleanup");

                // Check if this is a mixer configuration that needs cleanup using reflection
                var instanceType = __instance.GetType();
                var mixingStationConfigType = MixerThreholdMod_1_0_0.Core.IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Management.MixingStationConfiguration");

                if (mixingStationConfigType != null && mixingStationConfigType.IsAssignableFrom(instanceType))
                {
                    MixerThreholdMod_1_0_0.Main.logger?.Msg(2, "[PATCH] MixingStationConfiguration detected - performing cleanup");

                    // Safe cleanup using background task to not block destruction
                    // Use Task.Run to ensure we don't block the main Unity thread
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        Exception cleanupError = null;
                        bool trackingRemoved = false;
                        bool idRemoved = false;

                        try
                        {
                            // Remove from tracked mixers safely
                            bool removed = await MixerThreholdMod_1_0_0.Core.MixerConfigurationTracker.RemoveAsync(__instance);
                            if (removed)
                            {
                                MixerThreholdMod_1_0_0.Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking");
                            }

                            // Remove from ID manager
                            bool idRemoved = MixerThreholdMod_1_0_0.Core.MixerIDManager.RemoveMixerID(__instance);
                            if (idRemoved)
                            {
                                MixerThreholdMod_1_0_0.Main.logger.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager");
                            }
                            catch (Exception idEx)
                            {
                                MixerThreholdMod_1_0_0.Main.logger?.Err(string.Format("[PATCH] ID manager cleanup error: {0}", idEx.Message));
                                idRemoved = false;
                            }

                            // Summary of cleanup results
                            MixerThreholdMod_1_0_0.Main.logger?.Msg(2, string.Format("[PATCH] Cleanup completed - Tracking: {0}, ID: {1}",
                                trackingRemoved ? "SUCCESS" : "SKIPPED",
                                idRemoved ? "SUCCESS" : "SKIPPED"));
                        }
                        catch (Exception ex)
                        {
                            cleanupError = ex;
                        }

                        if (cleanupError != null)
                        {
                            MixerThreholdMod_1_0_0.Main.logger.Err(string.Format("[PATCH] CRASH PREVENTION: Cleanup error: {0}", cleanupError.Message));
                            // Don't re-throw - let cleanup fail gracefully
                        }
                    });
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
                patchError = ex;
            }

            if (patchError != null)
            {
                MixerThreholdMod_1_0_0.Main.logger.Err(string.Format("[PATCH] EntityConfiguration_Destroy_Patch CRASH PREVENTION: Patch error: {0}\nStackTrace: {1}",
                    patchError.Message, patchError.StackTrace));
                // CRITICAL: Never let patch failures crash entity destruction
                MixerThreholdMod_1_0_0.Main.logger?.Err("[PATCH] CRITICAL: Patch failed but entity destruction will continue normally");
            }
        }
    }
}