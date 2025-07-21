

using System;
using System.Diagnostics;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Captures and stores performance metrics for MixerThreholdMod.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
internal class PerformanceSnapshot
{
    public DateTime Timestamp { get; }
    public long MemoryUsageBytes { get; }
    public double CpuUsagePercent { get; }
    public int ThreadCount { get; }

    public PerformanceSnapshot()
    {
        Timestamp = DateTime.UtcNow;
        MemoryUsageBytes = GetCurrentProcessMemory();
        CpuUsagePercent = GetCurrentProcessCpu();
        ThreadCount = GetCurrentProcessThreadCount();
    }

    private long GetCurrentProcessMemory()
    {
        try
        {
            return Process.GetCurrentProcess().WorkingSet64;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error getting memory usage: {1}", PERF_SNAPSHOT_PREFIX, ex.Message));
            return -ONE_LONG;
        }
    }

    private double GetCurrentProcessCpu()
    {
        try
        {
            // Dummy implementation for .NET 4.8.1; replace with actual CPU monitoring if needed
            return ZERO_DOUBLE;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error getting CPU usage: {1}", PERF_SNAPSHOT_PREFIX, ex.Message));
            return -ONE_DOUBLE;
        }
    }

    private int GetCurrentProcessThreadCount()
    {
        try
        {
            return Process.GetCurrentProcess().Threads.Count;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error getting thread count: {1}", PERF_SNAPSHOT_PREFIX, ex.Message));
            return -ONE_INT;
        }
    }
}