using MelonLoader;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Save;
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using MixerThreholdMod_1_0_0.Constants;    // ✅ ESSENTIAL - Keep this! Our constants!

namespace MixerThreholdMod_0_0_1.Patches
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
    /// ⚠️ IL2CPP COMPATIBLE: Uses dynamic type loading to avoid TypeLoadException in IL2CPP builds.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible exception handling patterns
    /// - Proper async coroutine usage
    /// </summary>
    public static class SaveManager_Save_Patch
    {
        private const int MaxBackups = 5;
        private static bool _patchInitialized = false;
        private static MethodInfo _saveMethod = null;

        /// <summary>
        /// Initialize the patch using IL2CPP-compatible type resolution
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (_patchInitialized) return;

                var saveManagerType = IL2CPPTypeResolver.GetSaveManagerType();
                if (saveManagerType == null)
                {
                    Main.logger.Warn(1, "[PATCH] SaveManager type not found - patch will not be applied");
                    return;
                }

                _saveMethod = saveManagerType.GetMethod("Save", new[] { typeof(string) });
                if (_saveMethod == null)
                {
                    Main.logger.Warn(1, "[PATCH] SaveManager.Save method not found - patch will not be applied");
                    return;
                }

                // Apply Harmony patch dynamically
                // FIX: Use correct HarmonyLib v2 syntax
                var harmony = new HarmonyLib.Harmony("MixerThreholdMod.SaveManager_Save_Patch");
                var postfixMethod = typeof(SaveManager_Save_Patch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);

                harmony.Patch(_saveMethod, null, new HarmonyMethod(postfixMethod));
                
                Main.logger.Msg(1, "[PATCH] IL2CPP-compatible SaveManager.Save patch applied successfully");
                _patchInitialized = true;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[PATCH] Failed to initialize SaveManager_Save_Patch: {0}", ex.Message));
            }
        }

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

                var parentDir = Directory.GetParent(_saveRoot);
                if (parentDir == null)
                {
                    Main.logger.Warn(1, $"Could not get parent directory of save root: {_saveRoot}");
                    return BackupResult.CreateFailure($"Could not get parent directory of save root: {_saveRoot}");
                }

                var _backupRoot = Utils.NormalizePath(Path.Combine(parentDir.FullName, "MixerThreholdMod_backup")) + "\\";
                _saveRoot = _saveRoot.TrimEnd('\\') + "\\";

                Main.logger.Msg(2, $"BACKUP ROOT: {_backupRoot}");
                Main.logger.Msg(2, $"SAVE ROOT: {_saveRoot}");

                if (!Directory.Exists(_saveRoot))
                {
                    Main.logger.Warn(1, $"Save directory not found at {_saveRoot}");
                    return BackupResult.CreateFailure($"Save directory not found at {_saveRoot}");
                }

                if (!Utils.EnsureDirectoryExists(_backupRoot, "backup directory creation"))
                {
                    return BackupResult.CreateFailure("Failed to create backup directory");
                }

                string _timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string _backupDirName = $"SaveGame_2_backup_{_timestamp}";
                string _backupPath = Path.Combine(_backupRoot, _backupDirName);
                string _saveRootPrefix = Path.GetFileName(_saveRoot.TrimEnd('\\'));

                Main.logger.Msg(2, $"SAVE ROOT PREFIX: {_saveRootPrefix}");

                try
                {
                    // Copy the SaveGame_2 folder to backup location
                    CopyDirectory(_saveRoot, _backupPath);
                    Main.logger.Msg(2, $"Saved backup to: {_backupPath}");
                }
                catch (Exception copyEx)
                {
                    Main.logger.Err($"Failed to copy directory during backup: {copyEx.Message}\n{copyEx.StackTrace}");
                    return BackupResult.CreateFailure($"Failed to copy directory during backup: {copyEx.Message}");
                }

                return BackupResult.CreateSuccess(_backupRoot, _saveRootPrefix);
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("[PATCH] NormalizePath error: {0}", ex.Message));
                return path; // Return original on error
            }
        }
    }
}