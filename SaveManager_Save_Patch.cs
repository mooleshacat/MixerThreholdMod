// SaveManagerSavePatch.cs
using MelonLoader;
using ScheduleOne.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace MixerThreholdMod_0_0_1
{
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Save), new[] { typeof(string) })]
    public static class SaveManager_Save_Patch
    {
        private const int MaxBackups = 5;

        public static void Postfix(string saveFolderPath)
        {
            try
            {
                Main.logger.Msg(3, "SaveManager.Save(string) called (Postfix)");
                
                // Changing saveFolderPath to _savePath causes exception
                // Confirmed not patching because signature mismatch. Solution is just reassign it :)
                var _savePath = saveFolderPath;
                if (string.IsNullOrEmpty(_savePath))
                {
                    Main.logger.Warn(1, "Save folder path is null or empty — cannot set CurrentSavePath.");
                    return;
                }

                string normalizedPath = _savePath.Replace('/', '\\');
                Main.CurrentSavePath = normalizedPath;
                Main.logger.Msg(2, $"Captured Save Folder Path: {normalizedPath}");

                Main.logger.Msg(2, $"Saving preferences file BEFORE backing up!");
                
                try
                {
                    // WriteDelayed handles creation of the mixer preferences file - do it _before_ backup!
                    MelonCoroutines.Start(WriteDelayed(normalizedPath));
                    Main.logger.Warn(2, $"WriteDelayed: started mixer pref file save in SaveManager.Save(string) inside Postfix");
                }
                catch (Exception writeEx)
                {
                    Main.logger.Err($"Failed to start WriteDelayed coroutine: {writeEx.Message}\n{writeEx.StackTrace}");
                }

                Main.logger.Msg(2, $"Attempting backup of savegame directory!");
                
                try
                {
                    // Start backup coroutine (Get the parent directory for BackupSave to be located in)
                    MelonCoroutines.Start(BackupSaveFolder(normalizedPath));
                    Main.logger.Warn(2, $"BackupSaveFolder: started backup coroutine in SaveManager.Save(string) inside Postfix");
                }
                catch (Exception backupEx)
                {
                    Main.logger.Err($"Failed to start BackupSaveFolder coroutine: {backupEx.Message}\n{backupEx.StackTrace}");
                }
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
            try
            {
                Main.logger.Msg(3, $"BackupSaveFolder started for: {_saveRoot}");
                yield return new WaitForSeconds(1.5f); // Ensure save completes

                if (string.IsNullOrEmpty(_saveRoot))
                {
                    Main.logger.Warn(1, "Save root is null or empty, cannot proceed with backup");
                    yield break;
                }

                var parentDir = Directory.GetParent(_saveRoot);
                if (parentDir == null)
                {
                    Main.logger.Warn(1, $"Could not get parent directory of save root: {_saveRoot}");
                    yield break;
                }

                var _backupRoot = Path.Combine(parentDir.FullName, "MixerThreholdMod_backup").Replace('/', '\\') + "\\";
                _saveRoot = _saveRoot.TrimEnd('\\') + "\\";
                    
                Main.logger.Msg(2, $"BACKUP ROOT: {_backupRoot}");
                Main.logger.Msg(2, $"SAVE ROOT: {_saveRoot}");

                if (!Directory.Exists(_saveRoot))
                {
                    Main.logger.Warn(1, $"Save directory not found at {_saveRoot}");
                    yield break;
                }

                if (!Directory.Exists(_backupRoot))
                {
                    try
                    {
                        Directory.CreateDirectory(_backupRoot);
                        Main.logger.Msg(2, $"Created backup directory: {_backupRoot}");
                    }
                    catch (Exception createEx)
                    {
                        Main.logger.Err($"Failed to create backup directory {_backupRoot}: {createEx.Message}");
                        yield break;
                    }
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
                    yield break;
                }

                yield return new WaitForSeconds(0.5f); // Allow file system to settle

                // Keep only the latest MaxBackups
                yield return MelonCoroutines.Start(CleanupOldBackups(_backupRoot, _saveRootPrefix));
                
                Main.logger.Msg(3, $"BackupSaveFolder: Backup operation completed successfully");
            }
            catch (Exception ex)
            {
                Main.logger.Err($"BackupSaveFolder: Critical error during backup: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                Main.logger.Msg(3, $"BackupSaveFolder: Method completed for {_saveRoot}");
            }
        }

        private static IEnumerator CleanupOldBackups(string backupRoot, string saveRootPrefix)
        {
            try
            {
                Main.logger.Msg(3, $"Starting backup cleanup process for: {backupRoot}");
                
                if (!Directory.Exists(backupRoot))
                {
                    Main.logger.Warn(1, $"Backup root directory does not exist: {backupRoot}");
                    yield break;
                }

                var backupRootDir = new DirectoryInfo(backupRoot);
                if (backupRootDir == null)
                {
                    Main.logger.Warn(1, $"Could not access backup root directory: {backupRoot}");
                    yield break;
                }

                var backupDirs = backupRootDir
                    .GetDirectories($"{saveRootPrefix}_backup_*")
                    ?.Where(d => d != null)
                    ?.OrderByDescending(d => d.Name)
                    ?.ToList();

                if (backupDirs == null || backupDirs.Count == 0)
                {
                    Main.logger.Msg(3, $"No backup directories found matching pattern: {saveRootPrefix}_backup_*");
                    yield break;
                }

                Main.logger.Msg(3, $"Found {backupDirs.Count} backup directories, keeping latest {MaxBackups}");
                
                int deletionCount = 0;
                while (backupDirs.Count > MaxBackups && deletionCount < 10) // Safety limit
                {
                    var dirToDelete = backupDirs.LastOrDefault();
                    if (dirToDelete == null || !dirToDelete.Exists)
                    {
                        Main.logger.Warn(1, $"Directory to delete is null or doesn't exist, breaking cleanup loop");
                        break;
                    }

                    string dirPath = dirToDelete.FullName;
                    Main.logger.Msg(2, $"Attempting to delete backup directory: {dirPath}");
                    
                    // Add additional safety checks before deletion
                    if (string.IsNullOrEmpty(dirPath) || !dirPath.Contains("backup"))
                    {
                        Main.logger.Err($"Safety check failed: Invalid directory path for deletion: {dirPath}");
                        break;
                    }

                    try
                    {
                        Directory.Delete(dirPath, true);
                        Main.logger.Msg(1, $"Successfully deleted oldest backup ({deletionCount + 1}/{backupDirs.Count - MaxBackups}): {dirPath}");
                        
                        backupDirs.RemoveAt(backupDirs.Count - 1);
                        deletionCount++;
                        
                        // Add small delay to prevent potential filesystem issues
                        yield return new WaitForSeconds(0.1f);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Main.logger.Err($"Access denied while deleting backup directory: {ex.Message}");
                        break;
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Main.logger.Warn(1, $"Directory not found during deletion (already deleted?): {ex.Message}");
                        backupDirs.RemoveAt(backupDirs.Count - 1);
                        continue;
                    }
                    catch (IOException ex)
                    {
                        Main.logger.Err($"I/O error while deleting backup directory: {ex.Message}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Err($"Unexpected error during backup cleanup: {ex.Message}\n{ex.StackTrace}");
                        break;
                    }
                }
                Main.logger.Msg(3, $"Backup cleanup completed. Deleted {deletionCount} old directories.");
            }
            catch (Exception ex)
            {
                // catchall at backup code, where the suspected crash is
                // hopefully should catch errors
                Main.logger.Err($"CleanupOldBackups: Failed during backup cleanup process");
                Main.logger.Err($"CleanupOldBackups: Caught exception: {ex.Message}\n{ex.StackTrace}");
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

                Directory.CreateDirectory(targetDir);
                Main.logger.Msg(3, $"CopyDirectory: Created target directory: {targetDir}");

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
            yield return new WaitForSeconds(1.0f);
            MelonCoroutines.Start(MixerSaveManager.WriteMixerValuesAsync(_saveRoot));
        }
    }
}