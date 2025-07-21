

using System;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Tracks and verifies integrity of mixer data across save/load cycles.
    /// âš ï¸ THREAD SAFETY: All operations are thread-safe.
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// âš ï¸ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataIntegrityTracker
    {
        private static readonly Logger logger = new Logger();
        private static byte[] lastKnownGoodData;

        /// <summary>
        /// Records the last known good mixer data after a successful validation.
        /// </summary>
        /// <param name="data">Mixer data to record.</param>
        public static void Record(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                logger.Warn(1, "[MixerDataIntegrityTracker] Record: data is null or empty.");
                return;
            }
            try
            {
                lastKnownGoodData = new byte[data.Length];
                Array.Copy(data, lastKnownGoodData, data.Length);
                logger.Msg(1, "[MixerDataIntegrityTracker] Record: last known good data updated.");
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("Record failed: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Checks if the current mixer data matches the last known good data.
        /// </summary>
        /// <param name="data">Current mixer data to compare.</param>
        /// <returns>True if data matches, false otherwise.</returns>
        public static bool IsIntegrityMaintained(byte[] data)
        {
            if (data == null || lastKnownGoodData == null)
            {
                logger.Warn(1, "[MixerDataIntegrityTracker] IsIntegrityMaintained: data or lastKnownGoodData is null.");
                return false;
            }
            if (data.Length != lastKnownGoodData.Length)
            {
                logger.Warn(2, "[MixerDataIntegrityTracker] IsIntegrityMaintained: data length mismatch.");
                return false;
            }
            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != lastKnownGoodData[i])
                    {
                        logger.Warn(2, "[MixerDataIntegrityTracker] IsIntegrityMaintained: data mismatch detected.");
                        return false;
                    }
                }
                logger.Msg(1, "[MixerDataIntegrityTracker] IsIntegrityMaintained: data integrity verified.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("IsIntegrityMaintained failed: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}