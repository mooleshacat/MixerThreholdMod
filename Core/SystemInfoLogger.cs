

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

﻿using System;
using System.Diagnostics;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Logs system information for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
internal static class SystemInfoLogger
{
    /// <summary>
    /// Logs basic system information.
    /// </summary>
    public static void LogSystemInfo()
    {
        try
        {
            var process = Process.GetCurrentProcess();
            Main.logger?.Msg(1, string.Format("{0} Process Name: {1}", SYSINFO_PREFIX, process.ProcessName));
            Main.logger?.Msg(1, string.Format("{0} Start Time: {1}", SYSINFO_PREFIX, process.StartTime));
            Main.logger?.Msg(1, string.Format("{0} Machine Name: {1}", SYSINFO_PREFIX, Environment.MachineName));
            Main.logger?.Msg(1, string.Format("{0} OS Version: {1}", SYSINFO_PREFIX, Environment.OSVersion));
            Main.logger?.Msg(1, string.Format("{0} .NET Version: {1}", SYSINFO_PREFIX, Environment.Version));
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error logging system info: {1}", SYSINFO_PREFIX, ex.Message));
        }
    }
}