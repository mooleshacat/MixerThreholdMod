

using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;
using MixerThreholdMod_1_0_0.Core;
using MixerThreholdMod_1_0_0.Helpers;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Manages persistence of mixer data.
    /// Delegates read, write, backup, and validation to dedicated helpers.
    /// âš ï¸ THREAD SAFETY: All operations are thread-safe and async.
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// âš ï¸ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataPersistenceManager
    {
        private static readonly Logger logger = new Logger();

        public static async Task<bool> SaveMixerDataAsync(string path, byte[] data)
        {
            try
            {
                // Delegate atomic write
                return await AtomicFileWriter.WriteAsync(path, data).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                logger.Err(string.Format("SaveMixerDataAsync failed for {0}: {1}\nStack Trace: {2}", path, ex.Message, ex.StackTrace));
                return false;
            }
        }

        public static async Task<byte[]> LoadMixerDataAsync(string path)
        {
            try
            {
                // Delegate thread-safe read
                return await ThreadSafeFileReader.ReadAsync(path).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                logger.Err(string.Format("LoadMixerDataAsync failed for {0}: {1}\nStack Trace: {2}", path, ex.Message, ex.StackTrace));
                return null;
            }
        }

        public static async Task<bool> BackupMixerDataAsync(string path)
        {
            try
            {
                // Delegate backup logic
                return await MixerDataBackupManager.BackupAsync(path).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                logger.Err(string.Format("BackupMixerDataAsync failed for {0}: {1}\nStack Trace: {2}", path, ex.Message, ex.StackTrace));
                return false;
            }
        }

        public static bool ValidateMixerData(byte[] data)
        {
            try
            {
                // Delegate validation
                return MixerDataValidator.Validate(data);
            }
            catch (System.Exception ex)
            {
                logger.Err(string.Format("ValidateMixerData failed: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}