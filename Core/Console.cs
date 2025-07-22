using System;

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Thread-safe console wrapper for MixerThreholdMod.
///  THREAD SAFETY: All operations are thread-safe using lock objects.
///  .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
internal static class Console
{
    private static readonly object _lock = new object();

    /// <summary>
    /// Writes a message to the console.
    /// </summary>
    public static void WriteLine(string message)
    {
        lock (_lock)
        {
            System.Console.WriteLine(string.Format("{0} {1}", CONSOLE_PREFIX, message));
        }
    }

    /// <summary>
    /// Reads a line from the console.
    /// </summary>
    public static string ReadLine()
    {
        lock (_lock)
        {
            return System.Console.ReadLine();
        }
    }
}