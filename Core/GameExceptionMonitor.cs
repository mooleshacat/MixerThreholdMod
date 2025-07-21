

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Monitors and logs game exceptions for MixerThreholdMod.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
internal static class GameExceptionMonitor
{
    private static readonly object _lock = new object();

    /// <summary>
    /// Logs an exception with detailed context.
    /// </summary>
    public static void LogException(Exception ex, string context = null)
    {
        lock (_lock)
        {
            string message = string.Format("{0} Exception in {1}: {2}\nStack Trace: {3}",
                GAME_EXCEPTION_PREFIX,
                context ?? "unknown context",
                ex?.Message ?? "null",
                ex?.StackTrace ?? "no stack trace");
            Main.logger?.Err(message);
        }
    }

    /// <summary>
    /// Logs a warning for a recoverable exception.
    /// </summary>
    public static void LogRecoverable(Exception ex, string context = null)
    {
        lock (_lock)
        {
            string message = string.Format("{0} Recoverable exception in {1}: {2}\nStack Trace: {3}",
                GAME_EXCEPTION_PREFIX,
                context ?? "unknown context",
                ex?.Message ?? "null",
                ex?.StackTrace ?? "no stack trace");
            Main.logger?.Warn(1, message);
        }
    }
}