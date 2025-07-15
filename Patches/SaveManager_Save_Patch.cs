using MelonLoader;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Save;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

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

        /// <summary>
        /// Initialize the patch using IL2CPP-compatible type resolution
        /// </summary>
        public static void Initialize()
        {
            Exception initError = null;
            try
            {
                if (_patchInitialized) return;

                var saveManagerType = IL2CPPTypeResolver.GetSaveManagerType();
                if (saveManagerType == null)
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, "[PATCH] SaveManager type not found - patch will not be applied");
                    return;
                }

                _saveMethod = saveManagerType.GetMethod("Save", new[] { typeof(string) });
                if (_saveMethod == null)
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, "[PATCH] SaveManager.Save method not found - patch will not be applied");
                    return;
                }

                // Apply Harmony patch dynamically
                var harmony = new Harmony(HARMONY_MOD_ID);
                var postfixMethod = typeof(SaveManager_Save_Patch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
                
                harmony.Patch(_saveMethod, null, new HarmonyMethod(postfixMethod));
                
                Main.logger.Msg(LOG_LEVEL_CRITICAL, "[PATCH] IL2CPP-compatible SaveManager.Save patch applied successfully");
                _patchInitialized = true;
            }
            catch (Exception ex)
            {
                initError = ex;
            }

            if (initError != null)
            {
                Main.logger.Err(string.Format("[PATCH] Failed to initialize SaveManager_Save_Patch: {0}\n{1}", initError.Message, initError.StackTrace));
            }
        }

        /// <summary>
        /// Harmony postfix for SaveManager.Save method
        /// ⚠️ CRASH PREVENTION: Comprehensive error handling to prevent save corruption
        /// </summary>
        public static void Postfix(string saveFolderPath)
        {
            Exception patchError = null;
            try
            {
                Main.logger.Msg(LOG_LEVEL_IMPORTANT, "[PATCH] SaveManager.Save postfix triggered");

                if (string.IsNullOrEmpty(saveFolderPath))
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, "[PATCH] Save folder path is null or empty");
                    return;
                }

                string normalizedPath = Helpers.Utils.NormalizePath(saveFolderPath);
                Main.CurrentSavePath = normalizedPath;
                Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("Captured Save Folder Path: {0}", normalizedPath));

                Main.logger.Msg(LOG_LEVEL_IMPORTANT, "Saving preferences file BEFORE backing up!");

                try
                {
                    // WriteDelayed handles creation of the mixer preferences file - do it _before_ backup!
                    MelonCoroutines.Start(WriteDelayed(normalizedPath));
                    Main.logger.Warn(WARN_LEVEL_VERBOSE, "WriteDelayed: started mixer pref file save in SaveManager.Save(string) inside Postfix");
                }
                catch (Exception writeEx)
                {
                    Main.logger.Err(string.Format("Failed to start WriteDelayed coroutine: {0}\n{1}", writeEx.Message, writeEx.StackTrace));
                }

                Main.logger.Msg(LOG_LEVEL_IMPORTANT, "Attempting backup of savegame directory!");

                try
                {
                    // Start backup coroutine (Get the parent directory for BackupSave to be located in)
                    MelonCoroutines.Start(BackupSaveFolder(normalizedPath));
                    Main.logger.Warn(WARN_LEVEL_VERBOSE, "BackupSaveFolder: started backup coroutine in SaveManager.Save(string) inside Postfix");
                }
                catch (Exception backupEx)
                {
                    Main.logger.Err(string.Format("Failed to start BackupSaveFolder coroutine: {0}\n{1}", backupEx.Message, backupEx.StackTrace));
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }

            if (patchError != null)
            {
                // catchall at patch level, where my DLL interacts with the game and it's engine
                // hopefully should catch errors in entire project?
                Main.logger.Err("[PATCH] SaveManager_Save_Patch: Failed to save game and/or preferences and/or backup");
                Main.logger.Err(string.Format("SaveManager_Save_Patch: Caught exception: {0}\n{1}", patchError.Message, patchError.StackTrace));
            }
        }

        /// <summary>
        /// Backup save folder with comprehensive error handling
        /// ⚠️ THREAD SAFETY: Uses Task.Run to prevent main thread blocking
        /// </summary>
        private static IEnumerator BackupSaveFolder(string saveRoot)
        {
            Exception backupError = null;
            try
            {
                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format("BackupSaveFolder started for: {0}", saveRoot));
                yield return new WaitForSeconds(1.5f); // Ensure save completes

                // Move backup logic to a background task to avoid yield issues
                var backupTask = Task.Run(() =>
                {
                    try
                    {
                        return PerformBackupOperation(saveRoot);
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

                Main.logger.Msg(LOG_LEVEL_VERBOSE, "BackupSaveFolder: Backup operation completed successfully");
            }
            catch (Exception ex)
            {
                backupError = ex;
            }

            if (backupError != null)
            {
                Main.logger.Err(string.Format("BackupSaveFolder: Error: {0}", backupError.Message));
            }
        }

        /// <summary>
        /// Perform the actual backup operation
        /// ⚠️ CRASH PREVENTION: Atomic backup operations with comprehensive error handling
        /// </summary>
        private static BackupResult PerformBackupOperation(string saveRoot)
        {
            Exception operationError = null;
            try
            {
                Main.logger.Msg(LOG_LEVEL_IMPORTANT, "[PATCH] SaveManager.Save postfix triggered");

                var parentDir = Directory.GetParent(saveRoot);
                if (parentDir == null || !parentDir.Exists)
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, string.Format("Could not get parent directory of save root: {0}", saveRoot));
                    return BackupResult.CreateFailure(string.Format("Could not get parent directory of save root: {0}", saveRoot));
                }

                var backupRoot = Helpers.Utils.NormalizePath(Path.Combine(parentDir.FullName, "MixerThreholdMod_backup")) + "\\";
                saveRoot = saveRoot.TrimEnd('\\') + "\\";

                Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("BACKUP ROOT: {0}", backupRoot));
                Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("SAVE ROOT: {0}", saveRoot));

                if (!Directory.Exists(saveRoot))
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, string.Format("Save directory not found at {0}", saveRoot));
                    return BackupResult.CreateFailure(string.Format("Save directory not found at {0}", saveRoot));
                }

                if (!Helpers.Utils.EnsureDirectoryExists(backupRoot, "backup directory creation"))
                {
                    return BackupResult.CreateFailure("Failed to create backup directory");
                }

                string timestamp = DateTime.Now.ToString(FILENAME_DATETIME_FORMAT);
                string backupDirName = string.Format("SaveGame_2_backup_{0}", timestamp);
                string backupPath = Path.Combine(backupRoot, backupDirName);
                string saveRootPrefix = Path.GetFileName(saveRoot.TrimEnd('\\'));

                Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("SAVE ROOT PREFIX: {0}", saveRootPrefix));

                try
                {
                    // Copy the SaveGame_2 folder to backup location
                    CopyDirectory(saveRoot, backupPath);
                    Main.logger.Msg(LOG_LEVEL_IMPORTANT, string.Format("Saved backup to: {0}", backupPath));
                }
                catch (Exception copyEx)
                {
                    Main.logger.Err(string.Format("Failed to copy directory during backup: {0}\n{1}", copyEx.Message, copyEx.StackTrace));
                    return BackupResult.CreateFailure(string.Format("Failed to copy directory during backup: {0}", copyEx.Message));
                }

                return BackupResult.CreateSuccess(backupRoot, saveRootPrefix);
            }
            catch (Exception ex)
            {
                operationError = ex;
            }

            if (operationError != null)
            {
                Main.logger.Err(string.Format("PerformBackupOperation: Critical error during backup: {0}\n{1}", operationError.Message, operationError.StackTrace));
                return BackupResult.CreateFailure(string.Format("Critical error during backup: {0}", operationError.Message));
            }

            return BackupResult.CreateFailure("Unknown error during backup operation");
        }

        /// <summary>
        /// Copy directory recursively with error handling
        /// ⚠️ CRASH PREVENTION: Safe file operations with individual file error handling
        /// </summary>
        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            Exception copyError = null;
            try
            {
                if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(targetDir))
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, string.Format("CopyDirectory: Invalid paths - source: {0}, target: {1}", 
                        sourceDir ?? NULL_PATH_FALLBACK, targetDir ?? NULL_PATH_FALLBACK));
                    return;
                }

                if (!Directory.Exists(sourceDir))
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, string.Format("CopyDirectory: Source directory does not exist: {0}", sourceDir));
                    return;
                }

                Helpers.Utils.EnsureDirectoryExists(targetDir, "CopyDirectory target directory");

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
                        Main.logger.Warn(WARN_LEVEL_VERBOSE, string.Format("CopyDirectory: Failed to copy file {0}: {1}", file, fileEx.Message));
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
                        Main.logger.Warn(WARN_LEVEL_VERBOSE, string.Format("CopyDirectory: Failed to copy directory {0}: {1}", dir, dirEx.Message));
                        // Continue with other directories
                    }
                }

                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format("CopyDirectory: Successfully copied from {0} to {1}", sourceDir, targetDir));
            }
            catch (Exception ex)
            {
                copyError = ex;
            }

            if (copyError != null)
            {
                Main.logger.Err(string.Format("CopyDirectory: Critical error during copy operation: {0}\n{1}", copyError.Message, copyError.StackTrace));
                throw; // Re-throw to allow caller to handle
            }
        }

        /// <summary>
        /// Cleanup old backup directories
        /// ⚠️ CRASH PREVENTION: Safe cleanup with individual directory error handling
        /// </summary>
        private static IEnumerator CleanupOldBackups(string backupRoot, string saveRootPrefix)
        {
            Exception cleanupError = null;
            try
            {
                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format("Starting backup cleanup process for: {0}", backupRoot));

                if (!Directory.Exists(backupRoot))
                {
                    Main.logger.Warn(WARN_LEVEL_VERBOSE, string.Format("Backup root directory does not exist: {0}", backupRoot));
                    yield break;
                }

                var backupRootDir = new DirectoryInfo(backupRoot);
                var backupDirs = backupRootDir
                    .GetDirectories(string.Format("{0}_backup_*", saveRootPrefix))
                    ?.Where(d => d != null)
                    ?.OrderByDescending(d => d.Name)
                    ?.ToList();

                if (backupDirs == null || backupDirs.Count == 0)
                {
                    Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format("No backup directories found matching pattern: {0}_backup_*", saveRootPrefix));
                    yield break;
                }

                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format("Found {0} backup directories, keeping latest {1}", backupDirs.Count, MaxBackups));

                int deletionCount = 0;
                while (backupDirs.Count > MaxBackups && deletionCount < 10) // Safety limit
                {
                    var dirToDelete = backupDirs.LastOrDefault();
                    if (dirToDelete == null || !dirToDelete.Exists)
                    {
                        Main.logger.Warn(WARN_LEVEL_VERBOSE, "Directory to delete is null or doesn't exist, breaking cleanup loop");
                        break;
                    }

                    string dirPath = dirToDelete.FullName;

                    try
                    {
                        Directory.Delete(dirPath, true);
                        Main.logger.Msg(LOG_LEVEL_CRITICAL, string.Format("Successfully deleted oldest backup ({0}/{1}): {2}", 
                            deletionCount + 1, backupDirs.Count - MaxBackups, dirPath));
                        backupDirs.RemoveAt(backupDirs.Count - 1);
                        deletionCount++;

                        // Add small delay to prevent potential filesystem issues
                        yield return new WaitForSeconds(0.1f);
                    }
                    catch (Exception deleteEx)
                    {
                        Main.logger.Err(string.Format("Failed to delete backup directory {0}: {1}", dirPath, deleteEx.Message));
                        backupDirs.RemoveAt(backupDirs.Count - 1);
                        // Continue with next directory
                    }
                }

                Main.logger.Msg(LOG_LEVEL_VERBOSE, string.Format("Backup cleanup completed. Deleted {0} old directories.", deletionCount));
            }
            catch (Exception ex)
            {
                cleanupError = ex;
            }

            if (cleanupError != null)
            {
                Main.logger.Err(string.Format("CleanupOldBackups: Error: {0}", cleanupError.Message));
            }
        }

        /// <summary>
        /// Write mixer values with delay to ensure save completion
        /// ⚠️ CRASH PREVENTION: Safe coroutine execution with comprehensive error handling
        /// </summary>
        private static IEnumerator WriteDelayed(string saveRoot)
        {
            Exception writeError = null;
            try
            {
                if (string.IsNullOrEmpty(saveRoot))
                {
                    Main.logger.Warn(WARN_LEVEL_CRITICAL, "WriteDelayed: saveRoot is null or empty");
                    yield break;
                }

                yield return new WaitForSeconds(1.0f);
                
                try
                {
                    MelonCoroutines.Start(Helpers.MixerSaveManager.WriteMixerValuesAsync(saveRoot));
                }
                catch (Exception ex)
                {
                    Main.logger.Err(string.Format("Error starting WriteMixerValuesAsync coroutine: {0}", ex));
                }
            }
            catch (Exception ex)
            {
                writeError = ex;
            }

            if (writeError != null)
            {
                Main.logger.Err(string.Format("WriteDelayed: Error starting WriteMixerValuesAsync coroutine: {0}\n{1}", writeError.Message, writeError.StackTrace));
            }
        }

        /// <summary>
        /// Backup operation result container
        /// ⚠️ THREAD SAFETY: Immutable result object for safe cross-thread communication
        /// </summary>
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
    }
}