using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Provides thread-safe file writing operations for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class ThreadSafeFileWriter
{
    private static readonly object _fileLock = new object();

    /// <summary>
    /// Writes all text to a file asynchronously in a thread-safe manner.
    /// </summary>
    public static async Task<bool> WriteAllTextAsync(string filePath, string content, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            await Task.Run(() =>
            {
                lock (_fileLock)
                {
                    File.WriteAllText(filePath, content);
                }
            }, ct).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} File write completed: {1}", THREADSAFE_WRITE_PREFIX, filePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error writing file: {1}\n{2}", THREADSAFE_WRITE_PREFIX, filePath, ex));
            return false;
        }
    }
}