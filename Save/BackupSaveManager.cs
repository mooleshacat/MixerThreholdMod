using System;
using System.IO;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Manages save backups for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class BackupSaveManager
{
    /// <summary>
    /// Creates a backup of the specified save file.
    /// </summary>
    public static async Task<bool> CreateSaveBackupAsync(string saveFilePath, string backupDirectory)
    {
        try
        {
            if (!File.Exists(saveFilePath))
            {
                Main.logger?.Warn(1, string.Format("{0} Save file does not exist: {1}", SAVE_BACKUP_PREFIX, saveFilePath));
                return false;
            }

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            string backupFileName = string.Format("SaveBackup_{0:yyyyMMdd_HHmmss}.json", DateTime.UtcNow);
            string backupFilePath = Path.Combine(backupDirectory, backupFileName);

            await Task.Run(() => File.Copy(saveFilePath, backupFilePath, true)).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} Save backup created: {1}", SAVE_BACKUP_PREFIX, backupFilePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error creating save backup: {1}\n{2}", SAVE_BACKUP_PREFIX, ex.Message, ex.StackTrace));
            return false;
        }
    }

    /// <summary>
    /// Restores a save file from a backup.
    /// </summary>
    public static async Task<bool> RestoreSaveBackupAsync(string backupFilePath, string targetSaveFilePath)
    {
        try
        {
            if (!File.Exists(backupFilePath))
            {
                Main.logger?.Warn(1, string.Format("{0} Backup file does not exist: {1}", SAVE_BACKUP_PREFIX, backupFilePath));
                return false;
            }

            await Task.Run(() => File.Copy(backupFilePath, targetSaveFilePath, true)).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} Save backup restored to: {1}", SAVE_BACKUP_PREFIX, targetSaveFilePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error restoring save backup: {1}\n{2}", SAVE_BACKUP_PREFIX, ex.Message, ex.StackTrace));
            return false;
        }
    }
}