using System;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Tracks and logs performance metrics for mixer data persistence operations.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class MixerDataPerformanceMetrics
{
    private static readonly object _lock = new object();
    private static long _totalPersistenceOperations = 0;
    private static long _totalLoadOperations = 0;
    private static long _totalValidationOperations = 0;
    private static TimeSpan _totalPersistenceTime = TimeSpan.Zero;
    private static TimeSpan _totalLoadTime = TimeSpan.Zero;

    public static void RecordPersistence(TimeSpan duration)
    {
        lock (_lock)
        {
            _totalPersistenceOperations++;
            _totalPersistenceTime = _totalPersistenceTime.Add(duration);
        }
    }

    public static void RecordLoad(TimeSpan duration)
    {
        lock (_lock)
        {
            _totalLoadOperations++;
            _totalLoadTime = _totalLoadTime.Add(duration);
        }
    }

    public static void RecordValidation()
    {
        lock (_lock)
        {
            _totalValidationOperations++;
        }
    }

    public static void LogMetrics()
    {
        lock (_lock)
        {
            Main.logger?.Msg(1, PERSISTENCE_PREFIX + " ===== PERSISTENCE PERFORMANCE METRICS =====");
            Main.logger?.Msg(1, string.Format(PERSISTENCE_PREFIX + " Total persistence operations: {0}", _totalPersistenceOperations));
            Main.logger?.Msg(1, string.Format(PERSISTENCE_PREFIX + " Total load operations: {0}", _totalLoadOperations));
            Main.logger?.Msg(1, string.Format(PERSISTENCE_PREFIX + " Total validation operations: {0}", _totalValidationOperations));

            if (_totalPersistenceOperations > 0)
            {
                double avgPersistenceTime = _totalPersistenceTime.TotalSeconds / _totalPersistenceOperations;
                Main.logger?.Msg(1, string.Format(PERSISTENCE_PREFIX + " Average persistence time: {0:F3}s", avgPersistenceTime));
            }

            if (_totalLoadOperations > 0)
            {
                double avgLoadTime = _totalLoadTime.TotalSeconds / _totalLoadOperations;
                Main.logger?.Msg(1, string.Format(PERSISTENCE_PREFIX + " Average load time: {0:F3}s", avgLoadTime));
            }

            Main.logger?.Msg(1, PERSISTENCE_PREFIX + " ==========================================");
        }
    }
}