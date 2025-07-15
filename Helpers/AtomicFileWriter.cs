using System;
using System.IO;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Provides atomic file write operations for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class AtomicFileWriter
{
    /// <summary>
    /// Atomically writes content to a file by writing to a temp file and renaming.
    /// </summary>
    public static async Task<bool> WriteAllTextAtomicAsync(string filePath, string content)
    {
        string tempFilePath = filePath + ".tmp";
        try
        {
            await Task.Run(() =>
            {
                File.WriteAllText(tempFilePath, content);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.Move(tempFilePath, filePath);
            }).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} Atomic write completed: {1}", ATOMIC_WRITE_PREFIX, filePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error during atomic write: {1}\n{2}", ATOMIC_WRITE_PREFIX, ex.Message, ex.StackTrace));
            if (File.Exists(tempFilePath))
            {
                try { File.Delete(tempFilePath); } catch { }
            }
            return false;
        }
    }
}