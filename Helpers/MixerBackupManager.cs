using System;
using System.IO;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Handles backup and restore operations for mixer data.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class MixerBackupManager
{
    /// <summary>
    /// Creates a backup of the specified mixer data file.
    /// </summary>
    public static async Task<bool> CreateBackupAsync(string sourceFilePath, string backupDirectory)
    {
        try
        {
            if (!File.Exists(sourceFilePath))
            {
                Main.logger?.Warn(1, string.Format("{0} Source file does not exist: {1}", BACKUP_PREFIX, sourceFilePath));
                return false;
            }

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            string backupFileName = string.Format("MixerBackup_{0:yyyyMMdd_HHmmss}.json", DateTime.UtcNow);
            string backupFilePath = Path.Combine(backupDirectory, backupFileName);

            await Task.Run(() => File.Copy(sourceFilePath, backupFilePath, true)).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} Backup created: {1}", BACKUP_PREFIX, backupFilePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error creating backup: {1}\n{2}", BACKUP_PREFIX, ex.Message, ex.StackTrace));
            return false;
        }
    }

    /// <summary>
    /// Restores mixer data from a backup file.
    /// </summary>
    public static async Task<bool> RestoreBackupAsync(string backupFilePath, string targetFilePath)
    {
        try
        {
            if (!File.Exists(backupFilePath))
            {
                Main.logger?.Warn(1, string.Format("{0} Backup file does not exist: {1}", BACKUP_PREFIX, backupFilePath));
                return false;
            }

            await Task.Run(() => File.Copy(backupFilePath, targetFilePath, true)).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} Backup restored to: {1}", BACKUP_PREFIX, targetFilePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error restoring backup: {1}\n{2}", BACKUP_PREFIX, ex.Message, ex.StackTrace));
            return false;
        }
    }
}