using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Performance-related constants including timeouts, thresholds, and monitoring intervals
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class PerformanceConstants
    {
        #region Timeout Constants
        /// <summary>Standard operation timeout in milliseconds (2 seconds)</summary>
        public const int OPERATION_TIMEOUT_MS = 2000;

        /// <summary>Console command processing delay in milliseconds (1 second)</summary>
        public const int CONSOLE_COMMAND_DELAY_MS = 1000;

        /// <summary>File operation timeout in milliseconds (5 seconds)</summary>
        public const int FILE_OPERATION_TIMEOUT_MS = 5000;

        /// <summary>Save operation timeout in milliseconds (10 seconds)</summary>
        public const int SAVE_OPERATION_TIMEOUT_MS = 10000;

        /// <summary>Backup operation timeout in milliseconds (30 seconds)</summary>
        public const int BACKUP_OPERATION_TIMEOUT_MS = 30000;

        /// <summary>Emergency save timeout in milliseconds (2 seconds)</summary>
        public const int EMERGENCY_SAVE_TIMEOUT_MS = 2000;

        /// <summary>Network request timeout in milliseconds (30 seconds)</summary>
        public const int NETWORK_TIMEOUT_MS = 30000;

        /// <summary>Database operation timeout in milliseconds (15 seconds)</summary>
        public const int DATABASE_TIMEOUT_MS = 15000;
        #endregion

        #region Performance Thresholds
        /// <summary>Performance warning threshold in milliseconds (100ms)</summary>
        public const int PERFORMANCE_WARNING_THRESHOLD_MS = 100;

        /// <summary>Performance slow operation threshold in milliseconds (50ms)</summary>
        public const int PERFORMANCE_SLOW_THRESHOLD_MS = 50;

        /// <summary>Critical performance threshold in milliseconds (500ms)</summary>
        public const int PERFORMANCE_CRITICAL_THRESHOLD_MS = 500;

        /// <summary>Memory warning threshold in bytes (100MB)</summary>
        public const long MEMORY_WARNING_THRESHOLD_BYTES = 104857600;

        /// <summary>Memory critical threshold in bytes (500MB)</summary>
        public const long MEMORY_CRITICAL_THRESHOLD_BYTES = 524288000;

        /// <summary>CPU usage warning threshold (75%)</summary>
        public const int CPU_WARNING_THRESHOLD_PERCENT = 75;

        /// <summary>CPU usage critical threshold (90%)</summary>
        public const int CPU_CRITICAL_THRESHOLD_PERCENT = 90;
        #endregion

        #region Monitoring Intervals
        /// <summary>System monitoring log interval (every 5th iteration)</summary>
        public const int SYSTEM_MONITORING_LOG_INTERVAL = 5;

        /// <summary>Performance monitoring interval in milliseconds (1 minute)</summary>
        public const int PERFORMANCE_MONITORING_INTERVAL_MS = 60000;

        /// <summary>Memory monitoring interval in milliseconds (30 seconds)</summary>
        public const int MEMORY_MONITORING_INTERVAL_MS = 30000;

        /// <summary>CPU monitoring interval in milliseconds (15 seconds)</summary>
        public const int CPU_MONITORING_INTERVAL_MS = 15000;

        /// <summary>I/O monitoring interval in milliseconds (10 seconds)</summary>
        public const int IO_MONITORING_INTERVAL_MS = 10000;
        #endregion

        #region Wait Time Constants
        /// <summary>Standard retry delay in milliseconds (500ms)</summary>
        public const int RETRY_DELAY_MS = 500;

        /// <summary>Short wait time in milliseconds (100ms)</summary>
        public const int SHORT_WAIT_MS = 100;

        /// <summary>Medium wait time in milliseconds (250ms)</summary>
        public const int MEDIUM_WAIT_MS = 250;

        /// <summary>Long wait time in milliseconds (1000ms)</summary>
        public const int LONG_WAIT_MS = 1000;

        /// <summary>Very long wait time in milliseconds (5000ms)</summary>
        public const int VERY_LONG_WAIT_MS = 5000;

        /// <summary>Polling interval in milliseconds (100ms)</summary>
        public const int POLLING_INTERVAL_MS = 100;

        /// <summary>Background task interval in milliseconds (5000ms)</summary>
        public const int BACKGROUND_TASK_INTERVAL_MS = 5000;
        #endregion

        #region Buffer and Cache Sizes
        /// <summary>Standard buffer size (4KB)</summary>
        public const int BUFFER_SIZE_STANDARD = 4096;

        /// <summary>Large buffer size (64KB)</summary>
        public const int BUFFER_SIZE_LARGE = 65536;

        /// <summary>Small buffer size (1KB)</summary>
        public const int BUFFER_SIZE_SMALL = 1024;

        /// <summary>Maximum cache size (100 items)</summary>
        public const int MAX_CACHE_SIZE = 100;

        /// <summary>Default cache expiry in milliseconds (5 minutes)</summary>
        public const int CACHE_EXPIRY_MS = 300000;
        #endregion

        #region Optimization Constants
        /// <summary>GC collection threshold (50MB)</summary>
        public const long GC_COLLECTION_THRESHOLD_BYTES = 52428800;

        /// <summary>Thread pool minimum threads</summary>
        public const int THREAD_POOL_MIN_THREADS = 2;

        /// <summary>Thread pool maximum threads</summary>
        public const int THREAD_POOL_MAX_THREADS = 16;

        /// <summary>Async operation queue limit</summary>
        public const int ASYNC_QUEUE_LIMIT = 50;

        /// <summary>Batch operation size</summary>
        public const int BATCH_OPERATION_SIZE = 10;
        #endregion
    }
}