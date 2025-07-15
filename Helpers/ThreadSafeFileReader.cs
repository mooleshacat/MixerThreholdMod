using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Provides thread-safe file reading operations for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class ThreadSafeFileReader
{
    private static readonly object _fileLock = new object();

    /// <summary>
    /// Reads all text from a file asynchronously in a thread-safe manner.
    /// </summary>
    public static async Task<string> ReadAllTextAsync(string filePath, CancellationToken ct = default(CancellationToken))
    {
        try
        {
            return await Task.Run(() =>
            {
                lock (_fileLock)
                {
                    if (!File.Exists(filePath))
                    {
                        Main.logger?.Warn(2, string.Format("{0} File does not exist: {1}", THREADSAFE_READ_PREFIX, filePath));
                        return string.Empty;
                    }
                    return File.ReadAllText(filePath);
                }
            }, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error reading file: {1}\n{2}", THREADSAFE_READ_PREFIX, filePath, ex));
            return string.Empty;
        }
    }
}