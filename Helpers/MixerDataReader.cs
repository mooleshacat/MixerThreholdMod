

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Robust, thread-safe reader for mixer data files.
    /// âš ï¸ THREAD SAFETY: All operations are thread-safe and async.
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// âš ï¸ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataReader
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Reads mixer data from the specified file path asynchronously.
        /// </summary>
        /// <param name="filePath">Path to the mixer data file.</param>
        /// <returns>Byte array of mixer data, or null if read fails.</returns>
        public static async Task<byte[]> ReadAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                logger.Err(string.Format("{0} ReadAsync: filePath is null or empty.", MIXER_DATA_READER_PREFIX));
                return null;
            }

            try
            {
                if (!File.Exists(filePath))
                {
                logger.Warn(LOG_LEVEL_CRITICAL, string.Format(FILE_NOT_FOUND_WARNING, filePath));
                    return null;
                }

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var buffer = new byte[fs.Length];
                    int bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (bytesRead != buffer.Length)
                    {
                        logger.Warn(LOG_LEVEL_VERBOSE, string.Format(BACKUP_INCOMPLETE_READ_WARNING, filePath));
                    }
                    logger.Msg(LOG_LEVEL_CRITICAL, string.Format(OPERATION_SUCCESS_MSG, filePath));
                    return buffer;
                }
            }
            catch (ArgumentNullException ex)
            {
                logger.Err(string.Format(ARGUMENT_NULL_EXCEPTION_MSG, filePath, ex.Message, ex.StackTrace));
                return null;
            }
            catch (IOException ex)
            {
                logger.Err(string.Format(IO_EXCEPTION_MSG, filePath, ex.Message, ex.StackTrace));
                return null;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(GENERAL_EXCEPTION_MSG, filePath, ex.Message, ex.StackTrace));
                return null;
            }
        }
    }
}