

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

﻿using System;
using System.Diagnostics;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Monitors memory usage for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
internal static class MemoryMonitor
{
    /// <summary>
    /// Gets the current process memory usage in bytes.
    /// </summary>
    public static long GetCurrentMemoryUsage()
    {
        try
        {
            return Process.GetCurrentProcess().WorkingSet64;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error getting memory usage: {1}", MEMORY_MONITOR_PREFIX, ex.Message));
            return -1;
        }
    }

    /// <summary>
    /// Logs the current memory usage.
    /// </summary>
    public static void LogCurrentMemoryUsage()
    {
        long memoryUsage = GetCurrentMemoryUsage();
        Main.logger?.Msg(1, string.Format("{0} Current memory usage: {1:N0} bytes", MEMORY_MONITOR_PREFIX, memoryUsage));
    }
}