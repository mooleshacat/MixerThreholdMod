using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Helpers;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Save
{
    /// <summary>
    /// Handles emergency save operations for mixer data to prevent data loss during crashes.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and async.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class EmergencySaveManager
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Performs an emergency save of mixer data to a dedicated emergency file.
        /// </summary>
        /// <param name="filePath">Original mixer data file path.</param>
        /// <param name="data">Mixer data to save.</param>
        public static async Task<bool> EmergencySaveAsync(string filePath, byte[] data)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                logger.Err(string.Format("{0} EmergencySaveAsync: filePath is null or empty.", EMERGENCY_SAVE_PREFIX));
                return false;
            }
            if (data == null)
            {
                logger.Err(string.Format("{0} EmergencySaveAsync: data is null for {1}.", EMERGENCY_SAVE_PREFIX, filePath));
                return false;
            }

            string emergencyPath = filePath + ".emergency";
            try
            {
                // Use atomic file writer for emergency save
                bool result = await AtomicFileWriter.WriteAsync(emergencyPath, data).ConfigureAwait(false);
                if (!result)
                {
                    logger.Err(string.Format("EmergencySaveAsync: Atomic write failed for {0}", emergencyPath));
                    return false;
                }
                logger.Msg(1, string.Format("EmergencySaveAsync succeeded for {0}", emergencyPath));
                return true;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("EmergencySaveAsync failed for {0}: {1}\nStack Trace: {2}", emergencyPath, ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}