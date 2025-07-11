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
                // Changing saveFolderPath to _savePath causes exception
                // Confirmed not patching because signature mismatch. Solution is just reassign it :)
                var _savePath = saveFolderPath;
                if (string.IsNullOrEmpty(_savePath))
                {
                    Main.logger.Warn(1, "Save folder path is null or empty — cannot set CurrentSavePath.");
                    return;
                }

                Main.logger.Msg(3, "SaveManager.Save(string) called (Postfix)");
                string normalizedPath = _savePath.Replace('/', '\\');
                Main.CurrentSavePath = normalizedPath;
                Main.logger.Msg(2, $"Captured Save Folder Path: {normalizedPath}");

                Main.logger.Msg(2, $"Saving preferences file BEFORE backing up!");
                // WriteDelayed handles creation of the mixer preferences file - do it _before_ backup!
                MelonCoroutines.Start(WriteDelayed(normalizedPath));

                Main.logger.Warn(2, $"WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix (Main() not locked)");
                Main.logger.Warn(2, $"WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix (Main() not locked)");

                Main.logger.Msg(2, $"Attempting backup of savegame directory!");
                // Start backup coroutine (Get the parent directory for BackupSave to be located in)
                MelonCoroutines.Start(BackupSaveFolder(normalizedPath));
                // BELOW COMMENT NEVER EXECUTES RELATED TO SAVE CRASH! SUSPECT: MelonCoroutines.Start(BackupSaveFolder())

                Main.logger.Warn(2, $"BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix (Main() not locked)");
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
            yield return new WaitForSeconds(1.5f); // Ensure save completes

            try
            {
                var _backupRoot = Directory.GetParent(_saveRoot).FullName + "\\MixerThreholdMod_backup\\";
                _saveRoot = _saveRoot + "\\";
                Main.logger.Msg(1, $"BACKUP ROOT: {_backupRoot}");
                Main.logger.Msg(1, $"SAVE ROOT: {_saveRoot}");

                if (!Directory.Exists(_saveRoot))
                {
                    Main.logger.Warn(1, $"SaveGame_2 directory not found at {_saveRoot}");
                    yield break;
                }

                if (!Directory.Exists(_backupRoot))
                {
                    Directory.CreateDirectory(_backupRoot);
                    Main.logger.Msg(1, $"Created backup directory: {_backupRoot}");
                }

                string _timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string _backupDirName = $"SaveGame_2_backup_{_timestamp}";
                string _backupPath = Path.Combine(_backupRoot, _backupDirName);
                string _saveRootPrefix = Path.GetFileName(_saveRoot); // directory name

                Main.logger.Msg(3, $"SAVE ROOT PREFIX: {_saveRootPrefix}");

                // Copy the SaveGame_2 folder to backup location
                CopyDirectory(_saveRoot, _backupPath);
                Main.logger.Msg(1, $"Saved backup to: {_backupPath}");
                try
                {
                    // Keep only the latest MaxBackups
                    var _backupDirs = new DirectoryInfo(_backupRoot)
                        .GetDirectories($"{_saveRootPrefix}_backup_*")
                        .OrderByDescending(d => d.Name)
                        .ToList();

                    Main.logger.Msg(3, $"Filtering to keep latest {MaxBackups} backups from: {_backupRoot}");
                    while (_backupDirs.Count > MaxBackups)
                    {
                            // CONTEXT: CRASH ON SAVE: WE ARE NOT GETTING HERE, LAST LOG LINE IS ^^^ AND NEXT ONE NEVER SHOWS
                            // NO EXCEPTIONS BEING THROWN! CONFUSING!
                            // ISSUE WITH LOOP CONDITION OR DIRECTORY.DELETE - ONE OR OTHER - DOESN'T MAKE IT TO COMMENT AFTER
                            // ONLY HAPPENS AFTER WAITING THEN SAVING. SAVING IMMEDIATELY AFTER LOAD IS FINE!
                            string dirToDelete = _backupDirs.Last().FullName;
                            Directory.Delete(dirToDelete, true);
                            Main.logger.Msg(1, $"Deleted oldest of {MaxBackups} backups: {dirToDelete}");
                            _backupDirs.RemoveAt(_backupDirs.Count - 1);
                    
                    }
                }
                catch (Exception ex)
                {
                    // catchall at backup code, where the suspected crash is
                    // hopefully should catch errors
                    Main.logger.Err($"BackupSaveFolder: Failed to delete oldest backup");
                    Main.logger.Err($"BackupSaveFolder: Caught exception: {ex.Message}\n{ex.StackTrace}");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err($"BackupSaveFolder: Error during backup: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite: true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string destDir = Path.Combine(targetDir, Path.GetFileName(dir));
                CopyDirectory(dir, destDir);
            }
        }

        private static IEnumerator WriteDelayed(string _saveRoot)
        {
            yield return new WaitForSeconds(1.0f);
            MelonCoroutines.Start(MixerSaveManager.WriteMixerValuesAsync(_saveRoot));
        }
    }
}