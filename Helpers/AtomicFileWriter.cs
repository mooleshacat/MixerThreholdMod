using System;
using System.IO;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Atomic file writer for safe, crash-resistant save operations.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and async.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class AtomicFileWriter
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Writes data to a file atomically. Uses a temp file and renames on success.
        /// </summary>
        /// <param name="filePath">Target file path</param>
        /// <param name="data">Data to write</param>
        public static async Task<bool> WriteAsync(string filePath, byte[] data)
        {
            string tempPath = filePath + ".tmp";
            try
            {
                // Write to temp file first
                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await fs.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }

                // Replace original file atomically
                if (File.Exists(filePath))
                {
                    File.Replace(tempPath, filePath, null);
                }
                else
                {
                    File.Move(tempPath, filePath);
                }

                logger.Msg(1, string.Format("Atomic write succeeded for {0}", filePath));
                return true;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("Atomic write failed for {0}: {1}\nStack Trace: {2}", filePath, ex.Message, ex.StackTrace));
                // Emergency fallback: try to clean up temp file
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { /* ignore */ }
                return false;
            }
        }
    }
}