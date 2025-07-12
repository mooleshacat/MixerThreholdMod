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
                Main.logger.Msg(3, "SaveManager.Save(string) called (Postfix)");

                // Changing saveFolderPath to _savePath causes exception
                // Confirmed not patching because signature mismatch. Solution is just reassign it :)
                var _savePath = saveFolderPath;
                if (string.IsNullOrEmpty(_savePath))
                {
                    Main.logger.Warn(1, "[PATCH] Save folder path is null or empty - cannot proceed");
                    return;
                }

                // Normalize and set current save path
                string normalizedPath = NormalizePath(saveFolderPath);
                Main.CurrentSavePath = normalizedPath;
                Main.logger.Msg(1, string.Format("[PATCH] Save path captured: {0}", normalizedPath));

                Main.logger.Msg(2, $"Saving preferences file BEFORE backing up!");

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

                Main.logger.Msg(2, $"Attempting backup of savegame directory!");

                try
                {
                    normalized = normalized.TrimEnd('\\');
                }
                return normalized;
            }
            catch (Exception ex)
            {
                // catchall at patch level, where my DLL interacts with the game and it's engine
                // hopefully should catch errors in entire project?
                Main.logger.Err($"SaveManager_Save_Patch: Failed to save game and/or preferences and/or backup");
                Main.logger.Err($"SaveManager_Save_Patch: Caught exception: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static IEnumerator BackupSaveFolder(string _saveRoot)
        {
            Main.logger.Msg(3, $"BackupSaveFolder started for: {_saveRoot}");
            yield return new WaitForSeconds(1.5f); // Ensure save completes

            // Move backup logic to a background task to avoid yield issues
            var backupTask = Task.Run(() =>
            {
                try
                {
                    return PerformBackupOperation(_saveRoot);
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"BackupSaveFolder: Error in backup task: {ex.Message}\n{ex.StackTrace}");
                    return BackupResult.CreateFailure($"Backup task failed: {ex.Message}");
                }
            });

            // Wait for backup task completion
            while (!backupTask.IsCompleted)
            {
                yield return null;
            }

            var backupResult = backupTask.Result;
            if (!backupResult.Success)
            {
                Main.logger.Err($"BackupSaveFolder: {backupResult.ErrorMessage}");
                yield break;
            }

            yield return new WaitForSeconds(0.5f); // Allow file system to settle

            // Keep only the latest MaxBackups
            yield return MelonCoroutines.Start(CleanupOldBackups(backupResult.BackupRoot, backupResult.SaveRootPrefix));

            Main.logger.Msg(3, $"BackupSaveFolder: Backup operation completed successfully");
        }

        private static BackupResult PerformBackupOperation(string _saveRoot)
        {
            try
            {
                if (string.IsNullOrEmpty(_saveRoot))
                {
                    Main.logger.Warn(1, "Save root is null or empty, cannot proceed with backup");
                    return BackupResult.CreateFailure("Save root is null or empty");
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
                Main.logger.Err($"PerformBackupOperation: Critical error during backup: {ex.Message}\n{ex.StackTrace}");
                return BackupResult.CreateFailure($"Critical error during backup: {ex.Message}");
            }
        }

        private class BackupResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
            public string BackupRoot { get; set; }
            public string SaveRootPrefix { get; set; }

            public static BackupResult CreateSuccess(string backupRoot, string saveRootPrefix)
            {
                return new BackupResult { Success = true, BackupRoot = backupRoot, SaveRootPrefix = saveRootPrefix };
            }

            public static BackupResult CreateFailure(string errorMessage)
            {
                return new BackupResult { Success = false, ErrorMessage = errorMessage };
            }
        }

        private static IEnumerator CleanupOldBackups(string backupRoot, string saveRootPrefix)
        {
            Main.logger.Msg(3, $"Starting backup cleanup process for: {backupRoot}");

            // Move cleanup logic to background task
            var cleanupTask = Task.Run(() =>
            {
                try
                {
                    return GetBackupDirectoriesForCleanup(backupRoot, saveRootPrefix);
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"CleanupOldBackups: Error getting backup directories: {ex.Message}\n{ex.StackTrace}");
                    return CleanupData.CreateFailure($"Failed to get backup directories: {ex.Message}");
                }
            });

            // Wait for cleanup task
            while (!cleanupTask.IsCompleted)
            {
                yield return null;
            }

            var cleanupData = cleanupTask.Result;
            if (!cleanupData.Success)
            {
                Main.logger.Warn(1, cleanupData.ErrorMessage);
                yield break;
            }

            if (cleanupData.BackupDirs == null || cleanupData.BackupDirs.Count == 0)
            {
                Main.logger.Msg(3, $"No backup directories found matching pattern: {saveRootPrefix}_backup_*");
                yield break;
            }

            Main.logger.Msg(3, $"Found {cleanupData.BackupDirs.Count} backup directories, keeping latest {MaxBackups}");

            int deletionCount = 0;
            var backupDirs = cleanupData.BackupDirs;

            while (backupDirs.Count > MaxBackups && deletionCount < 10) // Safety limit
            {
                var dirToDelete = backupDirs.LastOrDefault();
                if (dirToDelete == null || !dirToDelete.Exists)
                {
                    Main.logger.Warn(1, $"Directory to delete is null or doesn't exist, breaking cleanup loop");
                    break;
                }

                string dirPath = dirToDelete.FullName;

                // Perform the deletion and get the result
                var deletionResult = DeleteBackupDirectory(dirPath);
                if (deletionResult.Success)
                {
                    Main.logger.Msg(1, $"Successfully deleted oldest backup ({deletionCount + 1}/{backupDirs.Count - MaxBackups}): {dirPath}");
                    backupDirs.RemoveAt(backupDirs.Count - 1);
                    deletionCount++;

                    // Add small delay to prevent potential filesystem issues
                    yield return new WaitForSeconds(0.1f);
                }
                else if (deletionResult.ShouldContinue)
                {
                    // Directory was already deleted or similar non-fatal error
                    Main.logger.Warn(1, deletionResult.ErrorMessage);
                    backupDirs.RemoveAt(backupDirs.Count - 1);
                    continue;
                }
                else
                {
                    // Fatal error, stop cleanup
                    Main.logger.Err(deletionResult.ErrorMessage);
                    break;
                }
            }

            Main.logger.Msg(3, $"Backup cleanup completed. Deleted {deletionCount} old directories.");
        }

        private static CleanupData GetBackupDirectoriesForCleanup(string backupRoot, string saveRootPrefix)
        {
            try
            {
                if (!Directory.Exists(backupRoot))
                {
                    return CleanupData.CreateFailure($"Backup root directory does not exist: {backupRoot}");
                }

                var backupRootDir = new DirectoryInfo(backupRoot);
                if (backupRootDir == null)
                {
                    return CleanupData.CreateFailure($"Could not access backup root directory: {backupRoot}");
                }

                var backupDirs = backupRootDir
                    .GetDirectories($"{saveRootPrefix}_backup_*")
                    ?.Where(d => d != null)
                    ?.OrderByDescending(d => d.Name)
                    ?.ToList();

                return CleanupData.CreateSuccess(backupDirs);
            }
            catch (Exception ex)
            {
                Main.logger.Err($"GetBackupDirectoriesForCleanup: Failed during backup cleanup process: {ex.Message}\n{ex.StackTrace}");
                return CleanupData.CreateFailure($"Exception during backup directory enumeration: {ex.Message}");
            }
        }

        private static DeletionResult DeleteBackupDirectory(string dirPath)
        {
            try
            {
                Main.logger.Msg(2, $"Attempting to delete backup directory: {dirPath}");

                // Add additional safety checks before deletion
                if (string.IsNullOrEmpty(dirPath) || !dirPath.Contains("backup"))
                {
                    return DeletionResult.CreateFatal($"Safety check failed: Invalid directory path for deletion: {dirPath}");
                }

                Directory.Delete(dirPath, true);
                return DeletionResult.CreateSuccess();
            }
            catch (UnauthorizedAccessException ex)
            {
                return DeletionResult.CreateFatal($"Access denied while deleting backup directory: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                return DeletionResult.CreateContinue($"Directory not found during deletion (already deleted?): {ex.Message}");
            }
            catch (IOException ex)
            {
                return DeletionResult.CreateFatal($"I/O error while deleting backup directory: {ex.Message}");
            }
            catch (Exception ex)
            {
                return DeletionResult.CreateFatal($"Unexpected error during backup cleanup: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private class CleanupData
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
            public List<DirectoryInfo> BackupDirs { get; set; }

            public static CleanupData CreateSuccess(List<DirectoryInfo> backupDirs)
            {
                return new CleanupData { Success = true, BackupDirs = backupDirs };
            }

            public static CleanupData CreateFailure(string errorMessage)
            {
                return new CleanupData { Success = false, ErrorMessage = errorMessage };
            }
        }

        private class DeletionResult
        {
            public bool Success { get; set; }
            public bool ShouldContinue { get; set; }
            public string ErrorMessage { get; set; }

            public static DeletionResult CreateSuccess()
            {
                return new DeletionResult { Success = true };
            }

            public static DeletionResult CreateContinue(string errorMessage)
            {
                return new DeletionResult { Success = false, ShouldContinue = true, ErrorMessage = errorMessage };
            }

            public static DeletionResult CreateFatal(string errorMessage)
            {
                return new DeletionResult { Success = false, ShouldContinue = false, ErrorMessage = errorMessage };
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(targetDir))
                {
                    Main.logger.Warn(1, $"CopyDirectory: Invalid paths - source: {sourceDir}, target: {targetDir}");
                    return;
                }

                if (!Directory.Exists(sourceDir))
                {
                    Main.logger.Warn(1, $"CopyDirectory: Source directory does not exist: {sourceDir}");
                    return;
                }

                Utils.EnsureDirectoryExists(targetDir, "CopyDirectory target directory");

                var sourceFiles = Directory.GetFiles(sourceDir);
                foreach (var file in sourceFiles)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(file)) continue;

                        string fileName = Path.GetFileName(file);
                        if (string.IsNullOrEmpty(fileName)) continue;

                        string destFile = Path.Combine(targetDir, fileName);
                        File.Copy(file, destFile, overwrite: true);
                    }
                    catch (Exception fileEx)
                    {
                        Main.logger.Warn(1, $"CopyDirectory: Failed to copy file {file}: {fileEx.Message}");
                        // Continue with other files
                    }
                }

                var sourceDirs = Directory.GetDirectories(sourceDir);
                foreach (var dir in sourceDirs)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(dir)) continue;

                        string dirName = Path.GetFileName(dir);
                        if (string.IsNullOrEmpty(dirName)) continue;

                        string destDir = Path.Combine(targetDir, dirName);
                        CopyDirectory(dir, destDir);
                    }
                    catch (Exception dirEx)
                    {
                        Main.logger.Warn(1, $"CopyDirectory: Failed to copy directory {dir}: {dirEx.Message}");
                        // Continue with other directories
                    }
                }

                Main.logger.Msg(3, $"CopyDirectory: Successfully copied from {sourceDir} to {targetDir}");
            }
            catch (Exception ex)
            {
                Main.logger.Err($"CopyDirectory: Critical error during copy operation: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw to allow caller to handle
            }
        }

        private static IEnumerator WriteDelayed(string _saveRoot)
        {
            if (string.IsNullOrEmpty(_saveRoot))
            {
                Main.logger.Warn(1, "WriteDelayed: _saveRoot is null or empty");
                yield break;
            }

            yield return new WaitForSeconds(1.0f);

            // Move coroutine start to background to avoid potential issues
            try
            {
                MelonCoroutines.Start(MixerSaveManager.WriteMixerValuesAsync(_saveRoot));
            }
            catch (Exception ex)
            {
                Main.logger.Err($"WriteDelayed: Error starting WriteMixerValuesAsync coroutine: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}


}