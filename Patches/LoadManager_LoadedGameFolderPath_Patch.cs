using HarmonyLib;
using ScheduleOne.Persistence;
using System;

namespace MixerThreholdMod_0_0_1.Patches
{
    /// <summary>
    /// Harmony patch for LoadManager.StartGame to capture load operations and initialize save path.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This patch ensures save path is properly captured during game loading
    /// and triggers the load of mixer values when a save is loaded.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are designed to be thread-safe and not block the main thread.
    /// Error handling prevents patch failures from affecting game loading.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible exception handling patterns
    /// - Proper null checking
    /// </summary>
    [HarmonyPatch(typeof(LoadManager), "StartGame")]
    public static class LoadManager_LoadedGameFolderPath_Patch
    {
        private static bool _patchInitialized = false;
        private static MethodInfo _startGameMethod = null;

        /// <summary>
        /// Initialize the patch using IL2CPP-compatible type resolution
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (_patchInitialized) return;

                // Get LoadManager type via reflection to avoid IL2CPP issues
                var loadManagerType = IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Persistence.LoadManager");
                if (loadManagerType == null)
                {
                    Main.logger.Warn(1, "[PATCH] LoadManager type not found - patch will not be applied");
                    return;
                }

                // Get SaveInfo type via reflection
                var saveInfoType = IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Persistence.SaveInfo");
                if (saveInfoType == null)
                {
                    Main.logger.Warn(1, "[PATCH] SaveInfo type not found - patch will not be applied");
                    return;
                }

                _startGameMethod = loadManagerType.GetMethod("StartGame", new[] { saveInfoType, typeof(bool) });
                if (_startGameMethod == null)
                {
                    Main.logger.Warn(1, "[PATCH] LoadManager.StartGame method not found - patch will not be applied");
                    return;
                }

                // Apply Harmony patch dynamically
                var harmony = new HarmonyLib.Harmony("MixerThreholdMod.LoadManager_LoadedGameFolderPath_Patch");
                var postfixMethod = typeof(LoadManager_LoadedGameFolderPath_Patch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
                
                harmony.Patch(_startGameMethod, null, new HarmonyMethod(postfixMethod));
                
                Main.logger.Msg(1, "[PATCH] IL2CPP-compatible LoadManager.StartGame patch applied successfully");
                _patchInitialized = true;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[PATCH] Failed to initialize LoadManager_LoadedGameFolderPath_Patch: {0}", ex.Message));
            }
        }
        /// <summary>
        /// Postfix patch that runs after LoadManager.StartGame completes
        /// </summary>
        public static void Postfix(LoadManager __instance, SaveInfo info, bool allowLoadStacking)
        {
            Exception patchError = null;
            try
            {
                if (info == null || string.IsNullOrEmpty(info.SavePath))
                {
                    Main.logger.Msg(3, "[PATCH] LoadManager postfix: No save info or path");
                    return;
                }

                string savePath = info.SavePath;
                Main.logger.Msg(2, string.Format("[PATCH] LoadManager postfix: Game loading from {0}", savePath));

                // Set current save path for the save system
                Main.CurrentSavePath = NormalizePath(savePath);
                
                // Trigger load of mixer values when game loads
                try
                {
                    MelonCoroutines.Start(Save.CrashResistantSaveManager.LoadMixerValuesWhenReady());
                    Main.logger.Msg(2, "[PATCH] Load mixer values coroutine started");
                }
                catch (Exception loadEx)
                {
                    Main.logger.Err(string.Format("[PATCH] CRASH PREVENTION: Load trigger failed: {0}", loadEx.Message));
                    // Don't re-throw - let the game continue
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }

            if (patchError != null)
            {
                Main.logger.Err(string.Format("[PATCH] LoadManager_LoadedGameFolderPath_Patch CRASH PREVENTION: Patch error: {0}\nStackTrace: {1}", 
                    patchError.Message, patchError.StackTrace));
                // CRITICAL: Never let patch failures crash the game's load process
            }
        }

        /// <summary>
        /// Simple path normalization for .NET 4.8.1 compatibility
        /// </summary>
        private static string NormalizePath(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;
                
                string normalized = path.Replace('/', '\\').Trim();
                if (normalized.EndsWith("\\") && normalized.Length > 1)
                {
                    normalized = normalized.TrimEnd('\\');
                }
                return normalized;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[PATCH] NormalizePath error: {0}", ex.Message));
                return path;
            }
        }
    }
}
            }
        }

        /// <summary>
        /// Simple path normalization for .NET 4.8.1 compatibility
        /// </summary>
        private static string NormalizePath(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;
                
                string normalized = path.Replace('/', '\\').Trim();
                if (normalized.EndsWith("\\") && normalized.Length > 1)
                {
                    normalized = normalized.TrimEnd('\\');
                }
                return normalized;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[PATCH] NormalizePath error: {0}", ex.Message));
                return path;
            }
        }
    }
}
            }
        }

        /// <summary>
        /// Simple path normalization for .NET 4.8.1 compatibility
        /// </summary>
        private static string NormalizePath(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;
                
                string normalized = path.Replace('/', '\\').Trim();
                if (normalized.EndsWith("\\") && normalized.Length > 1)
                {
                    normalized = normalized.TrimEnd('\\');
                }
                return normalized;
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                Main.logger.Err(string.Format("SaveThresholdsCoroutine: Error: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger.Err(string.Format("[PATCH] NormalizePath error: {0}", ex.Message));
                return path;
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }
        }
    }
}