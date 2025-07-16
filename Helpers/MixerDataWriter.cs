using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Robust, thread-safe writer for mixer data files.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and async.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataWriter
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Writes mixer data to the specified file path asynchronously.
        /// </summary>
        /// <param name="filePath">Path to the mixer data file.</param>
        /// <param name="data">Byte array of mixer data to write.</param>
        /// <returns>True if write succeeds, false otherwise.</returns>
        public static async Task<bool> WriteAsync(string filePath, byte[] data)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                logger.Err("[MixerDataWriter] WriteAsync: filePath is null or empty.");
                return false;
            }
            if (data == null)
            {
                logger.Err(string.Format("[MixerDataWriter] WriteAsync: data is null for {0}.", filePath));
                return false;
            }

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await fs.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }
                logger.Msg(1, string.Format("[MixerDataWriter] WriteAsync succeeded for {0}", filePath));
                return true;
            }
            catch (ArgumentNullException ex)
            {
                logger.Err(string.Format("WriteAsync ArgumentNullException for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return false;
            }
            catch (IOException ex)
            {
                logger.Err(string.Format("WriteAsync IOException for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return false;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("WriteAsync failed for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}