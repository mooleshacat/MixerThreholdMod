using HarmonyLib;
using MelonLoader;
using MixerThreholdMod_1_0_0;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Helpers;
using MixerThreholdMod_1_0_0.Save;
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Patches
{
    /// <summary>
    /// Harmony patch for LoadManager.LoadedGameFolderPath getter and StartGame to capture load operations and initialize save path.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This patch ensures save path is properly captured during game loading
    /// and triggers the load of mixer values when a save is loaded.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are designed to be thread-safe and not block the main thread.
    /// Error handling prevents patch failures from affecting game loading.
    /// 
    /// ⚠️ IL2CPP COMPATIBLE: Uses dynamic type loading to avoid TypeLoadException in IL2CPP builds.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible exception handling patterns
    /// - Proper null checking and async patterns
    /// </summary>
    public static class LoadManager_LoadedGameFolderPath_Patch
    {
        private static bool _patchInitialized = false;
        private static MethodInfo _startGameMethod = null;
        private static bool _isHandlingGetter = false;

        /// <summary>
        /// Initialize the patch using IL2CPP-compatible type resolution
        /// </summary>
        public static void Initialize()
        {
            Exception initError = null;
            try
            {
                if (_patchInitialized) return;

                Main.logger?.Msg(2, "[PATCH] Initializing LoadManager_LoadedGameFolderPath_Patch");

                // Get LoadManager type via reflection to avoid IL2CPP issues
                var loadManagerType = IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Persistence.LoadManager");
                if (loadManagerType == null)
                {
                    Main.logger?.Warn(1, "[PATCH] LoadManager type not found - patch will not be applied");
                    return;
                }

                // Get SaveInfo type via reflection
                var saveInfoType = IL2CPPTypeResolver.GetTypeByName("ScheduleOne.Persistence.SaveInfo");
                if (saveInfoType == null)
                {
                    Main.logger?.Warn(1, "[PATCH] SaveInfo type not found - patch will not be applied");
                    return;
                }

                _startGameMethod = loadManagerType.GetMethod("StartGame", new[] { saveInfoType, typeof(bool) });
                if (_startGameMethod == null)
                {
                    Main.logger?.Warn(1, "[PATCH] LoadManager.StartGame method not found - patch will not be applied");
                    return;
                }

                // Apply Harmony patch dynamically
                var harmony = Main.Instance.HarmonyInstance;
                var postfixMethod = typeof(LoadManager_LoadedGameFolderPath_Patch).GetMethod("StartGamePostfix", BindingFlags.Static | BindingFlags.Public);

                harmony.Patch(_startGameMethod, null, new HarmonyMethod(postfixMethod));

                Main.logger?.Msg(1, "[PATCH] IL2CPP-compatible LoadManager.StartGame patch applied successfully");
                _patchInitialized = true;
            }
            catch (Exception ex)
            {
                initError = ex;
            }

            if (initError != null)
            {
                Main.logger?.Err(string.Format("[PATCH] Failed to initialize LoadManager_LoadedGameFolderPath_Patch: {0}\nStackTrace: {1}",
                    initError.Message, initError.StackTrace));
            }
        }

        /// <summary>
        /// Postfix patch for LoadedGameFolderPath getter to capture save path changes
        /// </summary>
        public static void Postfix(ref string __result)
        {
            if (_isHandlingGetter) return;

            Exception patchError = null;
            try
            {
                _isHandlingGetter = true;

                Main.logger?.Msg(3, string.Format("[PATCH] LoadManager_LoadedGameFolderPath_Patch: Postfix called with result: {0}", __result ?? "null"));

                if (!string.IsNullOrEmpty(__result))
                {
                    Main.CurrentSavePath = __result;

                    string normalizedPath = Utils.NormalizePath(__result);
                    string mixerSavePath = Path.Combine(normalizedPath, MIXER_SAVE_FILENAME);

                    int mixerCount = 0;
                    try
                    {
                        mixerCount = MixerConfigurationTracker.Count(tm => tm != null);
                    }
                    catch (Exception countEx)
                    {
                        Main.logger?.Err(string.Format("[PATCH] Error counting mixers: {0}", countEx.Message));
                    }

                    if (mixerCount == 0)
                    {
                        // Use the same flag, so when one triggers it suppresses the other
                        if (!MixerSaveManager._hasLoggedZeroMixers)
                        {
                            Main.logger?.Msg(2, "[PATCH] No mixers tracked (maybe you have none?) — skipping mixer threshold prefs save/create.");
                            Main.logger?.Warn(1, "[PATCH] No mixers tracked (maybe you have none?) — suppressing future logs until reload save.");
                            MixerSaveManager._hasLoggedZeroMixers = true;
                        }
                        return;
                    }

                    if (!File.Exists(mixerSavePath))
                    {
                        try
                        {
                            Main.logger?.Warn(1, "[PATCH] MixerThresholdSave.json missing on load — creating it now.");
                            MelonCoroutines.Start(SaveThresholdsCoroutine(normalizedPath));
                        }
                        catch (Exception coroutineEx)
                        {
                            Main.logger?.Err(string.Format("[PATCH] Error starting save coroutine: {0}", coroutineEx.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }
            finally
            {
                _isHandlingGetter = false;
            }

            if (patchError != null)
            {
                Main.logger?.Err(string.Format("[PATCH] LoadManager_LoadedGameFolderPath_Patch getter CRASH PREVENTION: {0}\nStackTrace: {1}",
                    patchError.Message, patchError.StackTrace));
            }
        }

        /// <summary>
        /// Postfix patch that runs after LoadManager.StartGame completes
        /// IL2CPP COMPATIBLE: Uses dynamic types to avoid TypeLoadException
        /// </summary>
        public static void StartGamePostfix(object __instance, object info, bool allowLoadStacking)
        {
            Exception patchError = null;
            try
            {
                if (info == null)
                {
                    Main.logger?.Msg(3, "[PATCH] LoadManager StartGame postfix: No save info");
                    return;
                }

                // Use reflection to get SavePath property from SaveInfo object
                var savePathProperty = info.GetType().GetProperty("SavePath");
                if (savePathProperty == null)
                {
                    Main.logger?.Warn(1, "[PATCH] LoadManager StartGame postfix: SavePath property not found on SaveInfo");
                    return;
                }

                var savePath = savePathProperty.GetValue(info, null) as string;
                if (string.IsNullOrEmpty(savePath))
                {
                    Main.logger?.Msg(3, "[PATCH] LoadManager StartGame postfix: Save path is empty");
                    return;
                }

                Main.logger?.Msg(2, string.Format("[PATCH] LoadManager StartGame postfix: Game loading from {0}", savePath));

                // Set current save path for the save system
                Main.CurrentSavePath = Utils.NormalizePath(savePath);

                // Trigger load of mixer values when game loads
                try
                {
                    MelonCoroutines.Start(CrashResistantSaveManager.LoadMixerValuesWhenReady());
                    Main.logger?.Msg(2, "[PATCH] Load mixer values coroutine started from StartGame");
                }
                catch (Exception loadEx)
                {
                    Main.logger?.Err(string.Format("[PATCH] CRASH PREVENTION: Load trigger failed: {0}", loadEx.Message));
                    // Don't re-throw - let the game continue
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }

            if (patchError != null)
            {
                Main.logger?.Err(string.Format("[PATCH] LoadManager_LoadedGameFolderPath_Patch StartGame CRASH PREVENTION: {0}\nStackTrace: {1}",
                    patchError.Message, patchError.StackTrace));
                // CRITICAL: Never let patch failures crash the game's load process
            }
        }

        /// <summary>
        /// Coroutine to save mixer thresholds when missing on load
        /// ⚠️ THREAD SAFETY: Runs on Unity coroutine thread, uses async file operations
        /// </summary>
        private static IEnumerator SaveThresholdsCoroutine(string savePath)
        {
            Exception saveError = null;
            try
            {
                Main.logger?.Msg(2, string.Format("[PATCH] SaveThresholdsCoroutine: Starting save to {0}", savePath));

                // Wait a frame to ensure we're not blocking
                yield return null;

                // Use the crash-resistant save manager to create the file
                yield return MelonCoroutines.Start(CrashResistantSaveManager.PerformCrashResistantSave(savePath, 0f, true));

                Main.logger?.Msg(2, "[PATCH] SaveThresholdsCoroutine: Save completed successfully");
            }
            catch (Exception ex)
            {
                saveError = ex;
            }

            if (saveError != null)
            {
                Main.logger?.Err(string.Format("[PATCH] SaveThresholdsCoroutine CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                    saveError.Message, saveError.StackTrace));
            }
        }
    }
}