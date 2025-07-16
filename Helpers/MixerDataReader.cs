using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Robust, thread-safe reader for mixer data files.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and async.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
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
                logger.Err("[MixerDataReader] ReadAsync: filePath is null or empty.");
                return null;
            }

            try
            {
                if (!File.Exists(filePath))
                {
                    logger.Warn(1, string.Format("ReadAsync: File not found {0}", filePath));
                    return null;
                }

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var buffer = new byte[fs.Length];
                    int bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (bytesRead != buffer.Length)
                    {
                        logger.Warn(2, string.Format("ReadAsync: Incomplete read for {0}", filePath));
                    }
                    logger.Msg(1, string.Format("[MixerDataReader] ReadAsync succeeded for {0}", filePath));
                    return buffer;
                }
            }
            catch (ArgumentNullException ex)
            {
                logger.Err(string.Format("ReadAsync ArgumentNullException for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return null;
            }
            catch (IOException ex)
            {
                logger.Err(string.Format("ReadAsync IOException for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return null;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("ReadAsync failed for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return null;
            }
        }
    }
}