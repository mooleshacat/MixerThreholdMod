using HarmonyLib;
using MelonLoader;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Save;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
using ScheduleOne.Persistence;
=======
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Persistence;  // REMOVED: Use IL2CPPTypeResolver for safe type loading
>>>>>>> 2bf7ffe (performance optimizations, cache manager)
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

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
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======

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
                var harmony = new Harmony("MixerThreholdMod.SaveManager_Save_Patch");
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
>>>>>>> 2bf7ffe (performance optimizations, cache manager)

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
                var harmony = new Harmony("MixerThreholdMod.SaveManager_Save_Patch");
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
>>>>>>> aa94715 (performance optimizations, cache manager)

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

                string normalizedPath = Utils.NormalizePath(_savePath);
                Main.CurrentSavePath = normalizedPath;
                Main.logger.Msg(2, string.Format("Captured Save Folder Path: {0}", normalizedPath));

                Main.logger.Msg(2, "Saving preferences file BEFORE backing up!");

                try
                {
                    // WriteDelayed handles creation of the mixer preferences file - do it _before_ backup!
                    MelonCoroutines.Start(WriteDelayed(normalizedPath));
                    Main.logger.Warn(2, "WriteDelayed: started mixer pref file save in SaveManager.Save(string) inside Postfix");
                }
                catch (Exception writeEx)
                {
                    Main.logger.Err(string.Format("Failed to start WriteDelayed coroutine: {0}\n{1}", writeEx.Message, writeEx.StackTrace));
                }

                Main.logger.Msg(2, "Attempting backup of savegame directory!");

                try
                {
                    // Start backup coroutine (Get the parent directory for BackupSave to be located in)
                    MelonCoroutines.Start(BackupSaveFolder(normalizedPath));
                    Main.logger.Warn(2, "BackupSaveFolder: started backup coroutine in SaveManager.Save(string) inside Postfix");
                }
                catch (Exception backupEx)
                {
                    Main.logger.Err(string.Format("Failed to start BackupSaveFolder coroutine: {0}\n{1}", backupEx.Message, backupEx.StackTrace));
                }
            }
            }
            }
            catch (Exception ex)
            {
                // catchall at patch level, where my DLL interacts with the game and it's engine
                // hopefully should catch errors in entire project?
                Main.logger.Err("SaveManager_Save_Patch: Failed to save game and/or preferences and/or backup");
                Main.logger.Err(string.Format("SaveManager_Save_Patch: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private static IEnumerator BackupSaveFolder(string _saveRoot)
        {
            Main.logger.Msg(3, string.Format("BackupSaveFolder started for: {0}", _saveRoot));
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
                    Main.logger.Err(string.Format("BackupSaveFolder: Error in backup task: {0}\n{1}", ex.Message, ex.StackTrace));
                    return BackupResult.CreateFailure(string.Format("Backup task failed: {0}", ex.Message));
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
                Main.logger.Err(string.Format("BackupSaveFolder: {0}", backupResult.ErrorMessage));
                yield break;
            }

            yield return new WaitForSeconds(0.5f); // Allow file system to settle

            // Keep only the latest MaxBackups
            yield return MelonCoroutines.Start(CleanupOldBackups(backupResult.BackupRoot, backupResult.SaveRootPrefix));

            Main.logger.Msg(3, "BackupSaveFolder: Backup operation completed successfully");
        }

        private static BackupResult PerformBackupOperation(string _saveRoot)
        {
            Exception patchError = null;
            try
            {
                Main.logger.Msg(2, "[PATCH] SaveManager.Save postfix triggered");

                if (string.IsNullOrEmpty(saveFolderPath))
                {
                    Main.logger.Warn(1, string.Format("Could not get parent directory of save root: {0}", _saveRoot));
                    return BackupResult.CreateFailure(string.Format("Could not get parent directory of save root: {0}", _saveRoot));
                }

                var _backupRoot = Utils.NormalizePath(Path.Combine(parentDir.FullName, "MixerThreholdMod_backup")) + "\\";
                _saveRoot = _saveRoot.TrimEnd('\\') + "\\";

                Main.logger.Msg(2, string.Format("BACKUP ROOT: {0}", _backupRoot));
                Main.logger.Msg(2, string.Format("SAVE ROOT: {0}", _saveRoot));

                if (string.IsNullOrEmpty(_saveRoot))
                {
                    Main.logger.Warn(1, string.Format("Save directory not found at {0}", _saveRoot));
                    return BackupResult.CreateFailure(string.Format("Save directory not found at {0}", _saveRoot));
                }

                if (!Utils.EnsureDirectoryExists(_backupRoot, "backup directory creation"))
                {
                    return BackupResult.CreateFailure("Failed to create backup directory");
                }

                string _timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string _backupDirName = string.Format("SaveGame_2_backup_{0}", _timestamp);
                string _backupPath = Path.Combine(_backupRoot, _backupDirName);
                string _saveRootPrefix = Path.GetFileName(_saveRoot.TrimEnd('\\'));

                Main.logger.Msg(2, string.Format("SAVE ROOT PREFIX: {0}", _saveRootPrefix));

                try
                {
                    // Copy the SaveGame_2 folder to backup location
                    CopyDirectory(_saveRoot, _backupPath);
                    Main.logger.Msg(2, string.Format("Saved backup to: {0}", _backupPath));
                }
                catch (Exception copyEx)
                {
                    Main.logger.Err(string.Format("Failed to copy directory during backup: {0}\n{1}", copyEx.Message, copyEx.StackTrace));
                    return BackupResult.CreateFailure(string.Format("Failed to copy directory during backup: {0}", copyEx.Message));
                }

                return BackupResult.CreateSuccess(_backupRoot, _saveRootPrefix);
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("PerformBackupOperation: Critical error during backup: {0}\n{1}", ex.Message, ex.StackTrace));
                return BackupResult.CreateFailure(string.Format("Critical error during backup: {0}", ex.Message));
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
            Main.logger.Msg(3, string.Format("Starting backup cleanup process for: {0}", backupRoot));

            // Move cleanup logic to background task
            var cleanupTask = Task.Run(() =>
            {
=======
            Main.logger.Msg(3, string.Format("Starting backup cleanup process for: {0}", backupRoot));
                // Trigger crash-resistant save immediately after game save
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
            Main.logger.Msg(3, string.Format("Starting backup cleanup process for: {0}", backupRoot));
                // Trigger crash-resistant save immediately after game save
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                try
                {
                    return GetBackupDirectoriesForCleanup(backupRoot, saveRootPrefix);
                }
                catch (Exception ex)
                {
                    Main.logger.Err(string.Format("CleanupOldBackups: Error getting backup directories: {0}\n{1}", ex.Message, ex.StackTrace));
                    return CleanupData.CreateFailure(string.Format("Failed to get backup directories: {0}", ex.Message));
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
                Main.logger.Msg(3, string.Format("No backup directories found matching pattern: {0}_backup_*", saveRootPrefix));
                yield break;
            }

            Main.logger.Msg(3, string.Format("Found {0} backup directories, keeping latest {1}", cleanupData.BackupDirs.Count, MaxBackups));

            int deletionCount = 0;
            var backupDirs = cleanupData.BackupDirs;

            while (backupDirs.Count > MaxBackups && deletionCount < 10) // Safety limit
            {
                var dirToDelete = backupDirs.LastOrDefault();
                if (dirToDelete == null || !dirToDelete.Exists)
                {
                    Main.logger.Warn(1, "Directory to delete is null or doesn't exist, breaking cleanup loop");
                    break;
                }

                string dirPath = dirToDelete.FullName;

                // Perform the deletion and get the result
                var deletionResult = DeleteBackupDirectory(dirPath);
                if (deletionResult.Success)
                {
                    Main.logger.Msg(1, string.Format("Successfully deleted oldest backup ({0}/{1}): {2}", deletionCount + 1, backupDirs.Count - MaxBackups, dirPath));
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
                catch (Exception saveEx)
                {
                    Main.logger.Err(string.Format("[PATCH] CRASH PREVENTION: Save trigger failed: {0}", saveEx.Message));
                    // Don't re-throw - let the game continue
                }
            }
<<<<<<< HEAD
<<<<<<< HEAD

            Main.logger.Msg(3, string.Format("Backup cleanup completed. Deleted {0} old directories.", deletionCount));
        }

        private static CleanupData GetBackupDirectoriesForCleanup(string backupRoot, string saveRootPrefix)
        {
            try
            {
                if (!Directory.Exists(backupRoot))
                {
                    return CleanupData.CreateFailure(string.Format("Backup root directory does not exist: {0}", backupRoot));
                }

                var backupRootDir = new DirectoryInfo(backupRoot);
                if (backupRootDir == null)
                {
                    return CleanupData.CreateFailure(string.Format("Could not access backup root directory: {0}", backupRoot));
                }

                var backupDirs = backupRootDir
                    .GetDirectories(string.Format("{0}_backup_*", saveRootPrefix))
                    ?.Where(d => d != null)
                    ?.OrderByDescending(d => d.Name)
                    ?.ToList();

                return CleanupData.CreateSuccess(backupDirs);
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("GetBackupDirectoriesForCleanup: Failed during backup cleanup process: {0}\n{1}", ex.Message, ex.StackTrace));
                return CleanupData.CreateFailure(string.Format("Exception during backup directory enumeration: {0}", ex.Message));
            }


        private static DeletionResult DeleteBackupDirectory(string dirPath)
        {
            try
            {
                Main.logger.Msg(2, string.Format("Attempting to delete backup directory: {0}", dirPath));

                // Add additional safety checks before deletion
                if (string.IsNullOrEmpty(dirPath) || !dirPath.Contains("backup"))
                {
                    return DeletionResult.CreateFatal(string.Format("Safety check failed: Invalid directory path for deletion: {0}", dirPath));
                }

                Directory.Delete(dirPath, true);
                return DeletionResult.CreateSuccess();
            }
            catch (UnauthorizedAccessException ex)
            {
                return DeletionResult.CreateFatal(string.Format("Access denied while deleting backup directory: {0}", ex.Message));
            }
            catch (DirectoryNotFoundException ex)
            {
                return DeletionResult.CreateContinue(string.Format("Directory not found during deletion (already deleted?): {0}", ex.Message));
            }
            catch (IOException ex)
            {
                return DeletionResult.CreateFatal(string.Format("I/O error while deleting backup directory: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                return DeletionResult.CreateFatal(string.Format("Unexpected error during backup cleanup: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
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
=======
                Main.logger.Err($"Coroutine continue failure in BackupSaveFolder: {ex}");
>>>>>>> 63ef1db (Add comprehensive coroutine exception handling and fix crash-prone backup operations)
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
=======
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        /// <summary>
        /// Simple path normalization for .NET 4.8.1 compatibility
        /// </summary>
        private static string NormalizePath(string path)
<<<<<<< HEAD
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(targetDir))
                {
                    Main.logger.Warn(1, string.Format("CopyDirectory: Invalid paths - source: {0}, target: {1}", sourceDir, targetDir));
                    return;
                }

                if (!Directory.Exists(sourceDir))
                {
                    Main.logger.Warn(1, string.Format("CopyDirectory: Source directory does not exist: {0}", sourceDir));
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
                        Main.logger.Warn(1, string.Format("CopyDirectory: Failed to copy file {0}: {1}", file, fileEx.Message));
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
                        Main.logger.Warn(1, string.Format("CopyDirectory: Failed to copy directory {0}: {1}", dir, dirEx.Message));
                        // Continue with other directories
                    }
                }

                Main.logger.Msg(3, string.Format("CopyDirectory: Successfully copied from {0} to {1}", sourceDir, targetDir));
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("CopyDirectory: Critical error during copy operation: {0}\n{1}", ex.Message, ex.StackTrace));
                throw; // Re-throw to allow caller to handle
            }
        }

        private static IEnumerator WriteDelayed(string _saveRoot)
        {
            try
            {
                if (string.IsNullOrEmpty(_saveRoot))
                {
                    Main.logger.Warn(1, "WriteDelayed: _saveRoot is null or empty");
                    yield break;
                }

                yield return new WaitForSeconds(1.0f);
                
                try
                {
                    MelonCoroutines.Start(MixerSaveManager.WriteMixerValuesAsync(_saveRoot));
                }
                catch (Exception ex)
                {
                    Main.logger.Err($"Error starting WriteMixerValuesAsync coroutine: {ex}");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("WriteDelayed: Error starting WriteMixerValuesAsync coroutine: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}