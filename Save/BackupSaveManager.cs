

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Helpers;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Save
{
    /// <summary>
    /// Orchestrates periodic and emergency backups of all mixer save files.
    /// âš ï¸ THREAD SAFETY: All operations are thread-safe and async.
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// âš ï¸ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class BackupSaveManager
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Backs up all specified mixer save files.
        /// </summary>
        /// <param name="filePaths">Array of file paths to backup.</param>
        public static async Task<bool> BackupAllAsync(string[] filePaths)
        {
            if (filePaths == null || filePaths.Length == 0)
            {
                logger.Err(string.Format("{0} BackupAllAsync: filePaths is null or empty.", BACKUP_SAVE_PREFIX));
                return false;
            }

            bool allSucceeded = true;
            foreach (var path in filePaths)
            {
                try
                {
                    bool result = await MixerDataBackupManager.BackupAsync(path).ConfigureAwait(false);
                    if (!result)
                    {
                        logger.Warn(LOG_LEVEL_CRITICAL, string.Format(BACKUP_FAILED_MSG, path, "", ""));
                        allSucceeded = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Err(string.Format(GENERAL_EXCEPTION_MSG, path, ex.Message, ex.StackTrace));
                    allSucceeded = false;
                }
            }
            return allSucceeded;
        }
    }
}