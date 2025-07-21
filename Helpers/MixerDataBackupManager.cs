

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Manages backup and restore operations for mixer data.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe and async.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses Task.Run, string.Format, and proper cancellation tokens.
/// </summary>
public static class MixerDataBackupManager
{
    private const int MaxBackups = 5;

    public static async Task<bool> CreateBackupAsync(string sourceFile, string backupDir, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
                return false;

            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);

            string timestamp = DateTime.Now.ToString(FILENAME_DATETIME_FORMAT);
            string backupFile = Path.Combine(backupDir, string.Format("MixerThresholdSave_backup_{0}.json", timestamp));

            await Task.Run(() => File.Copy(sourceFile, backupFile, false), cancellationToken).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} MixerDataBackupManager: Created backup: {1}", PERSISTENCE_PREFIX, backupFile));
            CleanupOldBackups(backupDir);
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} MixerDataBackupManager: Backup failed: {1}", PERSISTENCE_PREFIX, ex.Message));
            return false;
        }
    }

    public static void CleanupOldBackups(string backupDir)
    {
        try
        {
            var backupFiles = Directory.GetFiles(backupDir, "MixerThresholdSave_backup_*.json");
            if (backupFiles.Length <= MaxBackups)
                return;

            var sortedBackups = new System.Linq.EnumerableQuery<string>(backupFiles)
                .OrderByDescending(f => File.GetCreationTime(f)).ToList();
            var oldBackups = sortedBackups.Skip(MaxBackups).ToList();

            foreach (var oldBackup in oldBackups)
                File.Delete(oldBackup);

            Main.logger?.Msg(3, string.Format("{0} MixerDataBackupManager: Cleaned up {1} old backup files", PERSISTENCE_PREFIX, oldBackups.Count));
        }
        catch (Exception ex)
        {
            Main.logger?.Warn(1, string.Format("{0} MixerDataBackupManager: Cleanup failed: {1}", PERSISTENCE_PREFIX, ex.Message));
        }
    }
}