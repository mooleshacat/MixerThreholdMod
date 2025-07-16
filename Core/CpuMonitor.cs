

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

﻿using System;
using System.Diagnostics;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Monitors CPU usage for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
internal static class CpuMonitor
{
    /// <summary>
    /// Gets the current process CPU usage percentage.
    /// </summary>
    public static double GetCurrentCpuUsage()
    {
        try
        {
            // Dummy implementation for .NET 4.8.1; replace with actual CPU monitoring if needed
            return 0.0;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error getting CPU usage: {1}", CPU_MONITOR_PREFIX, ex.Message));
            return -1;
        }
    }

    /// <summary>
    /// Logs the current CPU usage.
    /// </summary>
    public static void LogCurrentCpuUsage()
    {
        double cpuUsage = GetCurrentCpuUsage();
        Main.logger?.Msg(1, string.Format("{0} Current CPU usage: {1:F2}%", CPU_MONITOR_PREFIX, cpuUsage));
    }
}