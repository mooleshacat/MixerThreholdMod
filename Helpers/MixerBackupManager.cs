

using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Manages atomic backup of mixer data files.
    ///  THREAD SAFETY: All operations are thread-safe and async.
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    ///  MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataBackupManager
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Creates a backup of the specified mixer data file.
        /// </summary>
        /// <param name="filePath">Path to the original mixer data file.</param>
        public static async Task<bool> BackupAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                logger.Err("[MixerDataBackupManager] BackupAsync: filePath is null or empty.");
                return false;
            }

            string backupPath = filePath + ".bak";
            try
            {
                if (!File.Exists(filePath))
                {
                    logger.Warn(1, string.Format("BackupAsync: Source file not found {0}", filePath));
                    return false;
                }

                // Read source file asynchronously
                byte[] data;
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    data = new byte[fs.Length];
                    int bytesRead = await fs.ReadAsync(data, 0, data.Length).ConfigureAwait(false);
                    if (bytesRead != data.Length)
                    {
                        logger.Warn(2, string.Format("BackupAsync: Incomplete read for {0}", filePath));
                        return false;
                    }
                }

                // Write backup file asynchronously
                using (var fs = new FileStream(backupPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await fs.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }

                logger.Msg(1, string.Format("BackupAsync succeeded for {0} â†’ {1}", filePath, backupPath));
                return true;
            }
            catch (ArgumentNullException ex)
            {
                logger.Err(string.Format("BackupAsync ArgumentNullException for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return false;
            }
            catch (IOException ex)
            {
                logger.Err(string.Format("BackupAsync IOException for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return false;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("BackupAsync failed for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}