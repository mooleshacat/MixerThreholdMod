using MelonLoader;
using ScheduleOne.Persistence;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace MixerThreholdMod_1_0_0.Patches
{
    /// <summary>
    /// Harmony patch for SaveManager.Save to capture save folder path and trigger mixer data persistence.
    /// This patch is critical for the save crash prevention system.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This patch intercepts save operations to ensure mixer data 
    /// is properly saved before the game saves, preventing data loss during crashes.
    /// 
    /// ⚠️ THREAD SAFETY: All operations use thread-safe methods and don't block the main thread.
    /// Error handling prevents patch failures from crashing the save process.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible exception handling patterns
    /// - Proper async coroutine usage
    /// </summary>
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Save), new[] { typeof(string) })]
    public static class SaveManager_Save_Patch
    {
        private const int MaxBackups = 5;

        /// <summary>
        /// Postfix patch that runs after SaveManager.Save completes.
        /// ⚠️ CRASH PREVENTION: This is the critical entry point for preventing save crashes.
        /// </summary>
        public static void Postfix(string saveFolderPath)
        {
            Exception patchError = null;
            try
            {
                Main.logger.Msg(2, "[PATCH] SaveManager.Save postfix triggered");

                if (string.IsNullOrEmpty(saveFolderPath))
                {
                    Main.logger.Warn(1, "[PATCH] Save folder path is null or empty - cannot proceed");
                    return;
                }

                // Normalize and set current save path
                string normalizedPath = NormalizePath(saveFolderPath);
                Main.CurrentSavePath = normalizedPath;
                Main.logger.Msg(1, string.Format("[PATCH] Save path captured: {0}", normalizedPath));

                // Trigger crash-resistant save immediately after game save
                try
                {
                    MelonCoroutines.Start(Save.CrashResistantSaveManager.TriggerSaveWithCooldown());
                    Main.logger.Msg(2, "[PATCH] Crash-resistant save triggered successfully");
                }
                catch (Exception saveEx)
                {
                    Main.logger.Err(string.Format("[PATCH] CRASH PREVENTION: Save trigger failed: {0}", saveEx.Message));
                    // Don't re-throw - let the game continue
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }

            if (patchError != null)
            {
                Main.logger.Err(string.Format("[PATCH] SaveManager_Save_Patch CRASH PREVENTION: Patch error: {0}\nStackTrace: {1}", patchError.Message, patchError.StackTrace));
                // CRITICAL: Never let patch failures crash the game's save process
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
                
                // Basic normalization - convert forward slashes and trim
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
                return path; // Return original on error
            }
        }
    }
}