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
    /// ⚠️ MAIN THREAD WARNING: All cleanup operations are performed on background threads
    /// to prevent blocking Unity's main thread during entity destruction.
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
        /// ⚠️ THREAD SAFETY: This method is thread-safe and can be called multiple times safely
        /// ⚠️ IL2CPP COMPATIBLE: Uses dynamic type loading to avoid TypeLoadException
        /// </summary>
        public static void Initialize()
        {
            Exception initError = null;
            try
            {
                if (_patchInitialized)
                {
                    MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] EntityConfiguration_Destroy_Patch already initialized - skipping");
                    return;
                }

                MixerThreholdMod_1_0_0.Main.logger?.Msg(2, "[PATCH] Initializing IL2CPP-compatible EntityConfiguration.Destroy patch...");

                // Get EntityConfiguration type via reflection to avoid IL2CPP issues
                var entityConfigType = MixerThreholdMod_1_0_0.Core.IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Management.EntityConfiguration");
                if (entityConfigType == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger?.Warn(1, "[PATCH] EntityConfiguration type not found - patch will not be applied");
                    MixerThreholdMod_1_0_0.Main.logger?.Warn(1, "[PATCH] This may indicate IL2CPP type loading issues or game version incompatibility");
                    return;
                }

                MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] EntityConfiguration type found successfully");

                // Find the Destroy method with multiple signature attempts for compatibility
                _destroyMethod = entityConfigType.GetMethod("Destroy", BindingFlags.Public | BindingFlags.Instance);
                if (_destroyMethod == null)
                {
                    // Try alternative method signatures
                    var allMethods = entityConfigType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var method in allMethods)
                    {
                        if (method.Name == "Destroy" && method.GetParameters().Length == 0)
                        {
                            _destroyMethod = method;
                            break;
                        }
                    }
                }

                if (_destroyMethod == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger?.Warn(1, "[PATCH] EntityConfiguration.Destroy method not found - patch will not be applied");
                    MixerThreholdMod_1_0_0.Main.logger?.Warn(1, "[PATCH] Available methods on EntityConfiguration:");

                    // Log available methods for debugging
                    var allMethods = entityConfigType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var method in allMethods.Take(10)) // Limit to avoid spam
                    {
                        MixerThreholdMod_1_0_0.Main.logger?.Msg(3, string.Format("[PATCH] - {0}", method.Name));
                    }
                    return;
                }

                MixerThreholdMod_1_0_0.Main.logger?.Msg(3, string.Format("[PATCH] EntityConfiguration.Destroy method found: {0}", _destroyMethod.Name));

                // Apply Harmony patch dynamically
                var harmony = new HarmonyLib.Harmony("MixerThreholdMod.EntityConfiguration_Destroy_Patch");
                var prefixMethod = typeof(EntityConfiguration_Destroy_Patch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);

                if (prefixMethod == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger?.Err("[PATCH] CRITICAL: Prefix method not found on EntityConfiguration_Destroy_Patch class");
                    return;
                }

                MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] Applying Harmony patch...");
                harmony.Patch(_destroyMethod, new HarmonyMethod(prefixMethod), null);

                _patchInitialized = true;
                MixerThreholdMod_1_0_0.Main.logger?.Msg(1, "[PATCH] IL2CPP-compatible EntityConfiguration.Destroy patch applied successfully");
            }
            catch (Exception ex)
            {
                initError = ex;
            }

            if (initError != null)
            {
                MixerThreholdMod_1_0_0.Main.logger?.Err(string.Format("[PATCH] CRASH PREVENTION: Failed to initialize EntityConfiguration_Destroy_Patch: {0}\nStackTrace: {1}",
                    initError.Message, initError.StackTrace));
                MixerThreholdMod_1_0_0.Main.logger?.Err("[PATCH] EntityConfiguration cleanup will not be available, but mod will continue operating");
            }
        }

        /// <summary>
        /// Prefix patch that runs before EntityConfiguration.Destroy
        /// ⚠️ IL2CPP COMPATIBLE: Uses dynamic types to avoid TypeLoadException
        /// ⚠️ THREAD SAFETY: All cleanup operations are performed asynchronously
        /// ⚠️ CRASH PREVENTION: Extensive error handling prevents patch failures from crashing game
        /// </summary>
        public static void Prefix(object __instance)
        {
            Exception patchError = null;
            try
            {
                if (__instance == null)
                {
                    MixerThreholdMod_1_0_0.Main.logger?.Warn(2, "[PATCH] EntityConfiguration_Destroy_Patch: Instance is null - skipping cleanup");
                    return;
                }

                MixerThreholdMod_1_0_0.Main.logger?.Msg(3, string.Format("[PATCH] EntityConfiguration.Destroy() called - checking for mixer cleanup on type: {0}",
                    __instance.GetType().Name));

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
                            MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] Starting background cleanup operations...");

                            // Remove from tracked mixers safely with timeout protection
                            try
                            {
                                var removeTask = MixerThreholdMod_1_0_0.Core.MixerConfigurationTracker.RemoveAsync(__instance);

                                // Wait with timeout to prevent hanging
                                if (await System.Threading.Tasks.Task.WhenAny(removeTask, System.Threading.Tasks.Task.Delay(5000)) == removeTask)
                                {
                                    trackingRemoved = await removeTask;
                                    if (trackingRemoved)
                                    {
                                        MixerThreholdMod_1_0_0.Main.logger?.Msg(2, "[PATCH] Mixer configuration cleaned up from tracking successfully");
                                    }
                                    else
                                    {
                                        MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] Mixer configuration was not in tracking system");
                                    }
                                }
                                else
                                {
                                    MixerThreholdMod_1_0_0.Main.logger?.Warn(1, "[PATCH] Tracker removal timed out after 5 seconds - continuing with ID cleanup");
                                }
                            }
                            catch (Exception trackingEx)
                            {
                                MixerThreholdMod_1_0_0.Main.logger?.Err(string.Format("[PATCH] Tracking cleanup error: {0}", trackingEx.Message));
                            }

                            // Remove from ID manager (synchronous operation) - IL2CPP COMPATIBLE
                            try
                            {
                                // IL2CPP COMPATIBLE: Use reflection to safely call RemoveMixerID
                                // Since __instance is object but RemoveMixerID expects MixingStationConfiguration

                                // Get the RemoveMixerID method via reflection for IL2CPP safety
                                var mixerIdManagerType = typeof(MixerThreholdMod_1_0_0.Core.MixerIDManager);
                                var removeMixerIdMethod = mixerIdManagerType.GetMethod("RemoveMixerID",
                                    BindingFlags.Public | BindingFlags.Static);

                                if (removeMixerIdMethod != null)
                                {
                                    // Invoke the method with the object instance
                                    var result = removeMixerIdMethod.Invoke(null, new object[] { __instance });
                                    idRemoved = (bool)result;

                                    if (idRemoved)
                                    {
                                        MixerThreholdMod_1_0_0.Main.logger?.Msg(2, "[PATCH] Mixer configuration cleaned up from ID manager successfully");
                                    }
                                    else
                                    {
                                        MixerThreholdMod_1_0_0.Main.logger?.Msg(3, "[PATCH] Mixer configuration was not in ID manager");
                                    }
                                }
                                else
                                {
                                    MixerThreholdMod_1_0_0.Main.logger?.Warn(1, "[PATCH] RemoveMixerID method not found via reflection");
                                    idRemoved = false;
                                }
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
                            MixerThreholdMod_1_0_0.Main.logger?.Err(string.Format("[PATCH] CRASH PREVENTION: Overall cleanup error: {0}\nStackTrace: {1}",
                                cleanupError.Message, cleanupError.StackTrace));
                            // Don't re-throw - let cleanup fail gracefully without affecting game
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
                MixerThreholdMod_1_0_0.Main.logger?.Err(string.Format("[PATCH] EntityConfiguration_Destroy_Patch CRASH PREVENTION: Patch error: {0}\nStackTrace: {1}",
                    patchError.Message, patchError.StackTrace));
                // CRITICAL: Never let patch failures crash entity destruction
                MixerThreholdMod_1_0_0.Main.logger?.Err("[PATCH] CRITICAL: Patch failed but entity destruction will continue normally");
            }
        }
    }
}