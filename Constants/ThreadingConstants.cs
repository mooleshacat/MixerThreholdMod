using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Threading and synchronization constants for async operations and thread safety
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// ⚠️ MAIN THREAD WARNING: These constants help prevent blocking Unity main thread
    /// </summary>
    public static class ThreadingConstants
    {
        #region Thread Names
        /// <summary>Main thread name identifier</summary>
        public const string MAIN_THREAD_NAME = "MainThread";

        /// <summary>Background worker thread name prefix</summary>
        public const string BACKGROUND_THREAD_PREFIX = "Background_";

        /// <summary>I/O thread name prefix</summary>
        public const string IO_THREAD_PREFIX = "IO_";

        /// <summary>Save thread name</summary>
        public const string SAVE_THREAD_NAME = "SaveThread";

        /// <summary>Backup thread name</summary>
        public const string BACKUP_THREAD_NAME = "BackupThread";

        /// <summary>Monitoring thread name</summary>
        public const string MONITORING_THREAD_NAME = "MonitoringThread";

        /// <summary>Validation thread name</summary>
        public const string VALIDATION_THREAD_NAME = "ValidationThread";

        /// <summary>Performance thread name</summary>
        public const string PERFORMANCE_THREAD_NAME = "PerformanceThread";
        #endregion

        #region Thread Pool Settings
        /// <summary>Minimum worker threads in thread pool</summary>
        public const int MIN_WORKER_THREADS = 2;

        /// <summary>Maximum worker threads in thread pool</summary>
        public const int MAX_WORKER_THREADS = 16;

        /// <summary>Minimum completion port threads</summary>
        public const int MIN_COMPLETION_PORT_THREADS = 2;

        /// <summary>Maximum completion port threads</summary>
        public const int MAX_COMPLETION_PORT_THREADS = 16;

        /// <summary>Thread pool queue limit</summary>
        public const int THREAD_POOL_QUEUE_LIMIT = 100;

        /// <summary>Thread idle timeout in milliseconds (30 seconds)</summary>
        public const int THREAD_IDLE_TIMEOUT_MS = 30000;
        #endregion

        #region Synchronization Timeouts
        /// <summary>Mutex timeout in milliseconds (5 seconds)</summary>
        public const int MUTEX_TIMEOUT_MS = 5000;

        /// <summary>Semaphore timeout in milliseconds (10 seconds)</summary>
        public const int SEMAPHORE_TIMEOUT_MS = 10000;

        /// <summary>Lock timeout in milliseconds (2 seconds)</summary>
        public const int LOCK_TIMEOUT_MS = 2000;

        /// <summary>Critical section timeout in milliseconds (1 second)</summary>
        public const int CRITICAL_SECTION_TIMEOUT_MS = 1000;

        /// <summary>Reader-writer lock timeout in milliseconds (5 seconds)</summary>
        public const int RW_LOCK_TIMEOUT_MS = 5000;

        /// <summary>Event wait timeout in milliseconds (30 seconds)</summary>
        public const int EVENT_WAIT_TIMEOUT_MS = 30000;
        #endregion

        #region Thread Priorities
        /// <summary>High priority thread setting</summary>
        public const string THREAD_PRIORITY_HIGH = "AboveNormal";

        /// <summary>Normal priority thread setting</summary>
        public const string THREAD_PRIORITY_NORMAL = "Normal";

        /// <summary>Low priority thread setting</summary>
        public const string THREAD_PRIORITY_LOW = "BelowNormal";

        /// <summary>Background priority thread setting</summary>
        public const string THREAD_PRIORITY_BACKGROUND = "Lowest";

        /// <summary>Critical priority thread setting</summary>
        public const string THREAD_PRIORITY_CRITICAL = "Highest";
        #endregion

        #region Cancellation Tokens
        /// <summary>Default cancellation timeout in milliseconds (30 seconds)</summary>
        public const int DEFAULT_CANCELLATION_TIMEOUT_MS = 30000;

        /// <summary>Quick cancellation timeout in milliseconds (5 seconds)</summary>
        public const int QUICK_CANCELLATION_TIMEOUT_MS = 5000;

        /// <summary>Extended cancellation timeout in milliseconds (2 minutes)</summary>
        public const int EXTENDED_CANCELLATION_TIMEOUT_MS = 120000;

        /// <summary>Cancellation check interval in milliseconds (100ms)</summary>
        public const int CANCELLATION_CHECK_INTERVAL_MS = 100;
        #endregion

        #region Async Operation Limits
        /// <summary>Maximum concurrent async operations</summary>
        public const int MAX_CONCURRENT_OPERATIONS = 10;

        /// <summary>Maximum async queue size</summary>
        public const int MAX_ASYNC_QUEUE_SIZE = 50;

        /// <summary>Async operation retry count</summary>
        public const int ASYNC_RETRY_COUNT = 3;

        /// <summary>Async operation retry delay in milliseconds (1 second)</summary>
        public const int ASYNC_RETRY_DELAY_MS = 1000;

        /// <summary>Background task interval in milliseconds (5 seconds)</summary>
        public const int BACKGROUND_TASK_INTERVAL_MS = 5000;
        #endregion

        #region Thread State Messages
        /// <summary>Thread started message</summary>
        public const string THREAD_STARTED_MESSAGE = "Thread started successfully";

        /// <summary>Thread stopped message</summary>
        public const string THREAD_STOPPED_MESSAGE = "Thread stopped successfully";

        /// <summary>Thread aborted message</summary>
        public const string THREAD_ABORTED_MESSAGE = "Thread was aborted";

        /// <summary>Thread timeout message</summary>
        public const string THREAD_TIMEOUT_MESSAGE = "Thread operation timed out";

        /// <summary>Thread exception message</summary>
        public const string THREAD_EXCEPTION_MESSAGE = "Thread encountered an exception";

        /// <summary>Main thread blocked warning</summary>
        public const string MAIN_THREAD_BLOCKED_WARNING = "WARNING: Operation may block main thread";

        /// <summary>Deadlock detected message</summary>
        public const string DEADLOCK_DETECTED_MESSAGE = "Potential deadlock detected";

        /// <summary>Resource contention message</summary>
        public const string RESOURCE_CONTENTION_MESSAGE = "Resource contention detected";
        #endregion

        #region Synchronization Object Names
        /// <summary>Save operation mutex name</summary>
        public const string SAVE_MUTEX_NAME = "MixerThresholdMod_SaveMutex";

        /// <summary>Backup operation mutex name</summary>
        public const string BACKUP_MUTEX_NAME = "MixerThresholdMod_BackupMutex";

        /// <summary>File access semaphore name</summary>
        public const string FILE_ACCESS_SEMAPHORE_NAME = "MixerThresholdMod_FileAccess";

        /// <summary>Performance monitoring semaphore name</summary>
        public const string PERFORMANCE_SEMAPHORE_NAME = "MixerThresholdMod_Performance";

        /// <summary>Validation lock name</summary>
        public const string VALIDATION_LOCK_NAME = "MixerThresholdMod_Validation";

        /// <summary>Global operation event name</summary>
        public const string GLOBAL_OPERATION_EVENT_NAME = "MixerThresholdMod_GlobalOperation";
        #endregion
    }
}