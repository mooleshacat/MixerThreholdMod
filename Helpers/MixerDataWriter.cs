

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

﻿using System;
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
                logger.Err(string.Format("{0} WriteAsync: filePath is null or empty.", MIXER_DATA_READER_PREFIX));
                return false;
            }
            if (data == null)
            {
                logger.Err(string.Format("{0} WriteAsync: data is null for {1}.", MIXER_DATA_READER_PREFIX, filePath));
                return false;
            }

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await fs.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }
                logger.Msg(LOG_LEVEL_CRITICAL, string.Format(OPERATION_SUCCESS_MSG, filePath));
                return true;
            }
            catch (ArgumentNullException ex)
            {
                logger.Err(string.Format(ARGUMENT_NULL_EXCEPTION_MSG, filePath, ex.Message, ex.StackTrace));
                return false;
            }
            catch (IOException ex)
            {
                logger.Err(string.Format(IO_EXCEPTION_MSG, filePath, ex.Message, ex.StackTrace));
                return false;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format(GENERAL_EXCEPTION_MSG, filePath, ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}