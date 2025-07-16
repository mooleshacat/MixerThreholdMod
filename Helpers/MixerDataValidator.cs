

﻿using System;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Validates mixer data for integrity before save/load operations.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataValidator
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Validates the provided mixer data.
        /// </summary>
        /// <param name="data">Mixer data to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool Validate(byte[] data)
        {
            if (data == null)
            {
                logger.Err("[MixerDataValidator] Validate: data is null.");
                return false;
            }
            if (data.Length == 0)
            {
                logger.Warn(1, "[MixerDataValidator] Validate: data is empty.");
                return false;
            }

            try
            {
                // Example validation: check for expected header bytes (customize as needed)
                // For demonstration, assume first byte must be non-zero
                if (data[0] == 0)
                {
                    logger.Warn(2, "[MixerDataValidator] Validate: data header invalid (first byte is zero).");
                    return false;
                }

                // Add further validation logic as needed...

                logger.Msg(1, "[MixerDataValidator] Validate: data is valid.");
                return true;
            }
            catch (IndexOutOfRangeException ex)
            {
                logger.Err(string.Format("Validate IndexOutOfRangeException: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));
                return false;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("Validate failed: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}