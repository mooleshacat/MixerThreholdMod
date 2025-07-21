using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Centralized logging-related constants for log levels, prefixes, messages, and configurations
    /// âš ï¸ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// âš ï¸ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class LoggingConstants
    {
        #region Log Levels
        /// <summary>Critical/User-facing information - always shown in production logs</summary>
        public const int LOG_LEVEL_CRITICAL = 1;

        /// <summary>Important operational information - shown in detailed logs</summary>
        public const int LOG_LEVEL_IMPORTANT = 2;

        /// <summary>Detailed diagnostic information - shown in verbose logs</summary>
        public const int LOG_LEVEL_VERBOSE = 3;

        /// <summary>High-priority warnings requiring immediate attention</summary>
        public const int WARN_LEVEL_CRITICAL = 1;

        /// <summary>Standard warnings for monitoring and optimization</summary>
        public const int WARN_LEVEL_VERBOSE = 2;
        #endregion

        #region Logging Prefixes
        /// <summary>Logging prefix for SaveManager operations</summary>
        public const string SAVE_MANAGER_PREFIX = "[SAVE-MGR]";

        /// <summary>Logging prefix for MixerDataPersistenceManager operations</summary>
        public const string PERSISTENCE_PREFIX = "[PERSISTENCE]";

        /// <summary>Logging prefix for GameDirectoryResolver operations</summary>
        public const string DIRECTORY_RESOLVER_PREFIX = "[DIR-RESOLVER]";

        /// <summary>Logging prefix for Logger operations</summary>
        public const string LOGGER_PREFIX = "[LOGGER]";

        /// <summary>Logging prefix for MixerValidation operations</summary>
        public const string MIXER_VALIDATION_PREFIX = "[MIXER-VALIDATION]";

        /// <summary>Logging prefix for AtomicFileWriter operations</summary>
        public const string ATOMIC_FILE_WRITER_PREFIX = "[ATOMIC-WRITER]";

        /// <summary>Logging prefix for MixerDataBackupManager operations</summary>
        public const string BACKUP_MANAGER_PREFIX = "[BACKUP-MGR]";

        /// <summary>Logging prefix for EmergencySaveManager operations</summary>
        public const string EMERGENCY_SAVE_PREFIX = "[EMERGENCY-SAVE]";

        /// <summary>Logging prefix for ConsoleCommandHandler operations</summary>
        public const string CONSOLE_COMMAND_PREFIX = "[CONSOLE-CMD]";

        /// <summary>Logging prefix for CancellableIoRunner operations</summary>
        public const string IO_RUNNER_PREFIX = "[IO-RUNNER]";

        /// <summary>Logging prefix for PerformanceOptimizer operations</summary>
        public const string PERFORMANCE_OPTIMIZER_PREFIX = "[PERF-OPT]";

        /// <summary>Logging prefix for SaveManager_Save_Patch operations</summary>
        public const string SAVE_PATCH_PREFIX = "[SAVE-PATCH]";

        /// <summary>Logging prefix for DataIntegrityTracker operations</summary>
        public const string DATA_INTEGRITY_PREFIX = "[DATA-INTEGRITY]";

        /// <summary>Logging prefix for Logger prefix for general operations</summary>
        public const string GENERAL_PREFIX = "[GENERAL]";

        /// <summary>Logging prefix for system operations and monitoring</summary>
        public const string SYSTEM_PREFIX = "[SYSTEM]";

        /// <summary>Logging prefix for error conditions and exceptions</summary>
        public const string ERROR_PREFIX = "[ERROR]";

        /// <summary>Logging prefix for warning conditions</summary>
        public const string WARNING_PREFIX = "[WARNING]";

        /// <summary>Logging prefix for informational messages</summary>
        public const string INFO_PREFIX = "[INFO]";

        /// <summary>Logging prefix for debug messages</summary>
        public const string DEBUG_PREFIX = "[DEBUG]";

        /// <summary>Logging prefix for trace messages</summary>
        public const string TRACE_PREFIX = "[TRACE]";

        /// <summary>Logging prefix for performance monitoring</summary>
        public const string PERF_PREFIX = "[PERF]";

        /// <summary>Logging prefix for memory operations</summary>
        public const string MEMORY_PREFIX = "[MEMORY]";

        /// <summary>Logging prefix for initialization</summary>
        public const string INIT_PREFIX = "[INIT]";

        /// <summary>Logging prefix for cleanup operations</summary>
        public const string CLEANUP_PREFIX = "[CLEANUP]";

        /// <summary>Logging prefix for validation operations</summary>
        public const string VALIDATION_PREFIX = "[VALIDATION]";
        #endregion

        #region Log Messages
        /// <summary>Log message for successful save completion</summary>
        public const string SAVE_SUCCESS_MESSAGE = "Save operation completed successfully";

        /// <summary>Log message for save operation failure</summary>
        public const string SAVE_FAILURE_MESSAGE = "Save operation failed";

        /// <summary>Log message for backup creation</summary>
        public const string BACKUP_CREATED_MESSAGE = "Backup created successfully";

        /// <summary>Log message for backup restoration</summary>
        public const string BACKUP_RESTORED_MESSAGE = "Backup restored successfully";

        /// <summary>Log message for validation success</summary>
        public const string VALIDATION_SUCCESS_MESSAGE = "Validation completed successfully";

        /// <summary>Log message for validation failure</summary>
        public const string VALIDATION_FAILURE_MESSAGE = "Validation failed";

        /// <summary>Log message for operation start</summary>
        public const string OPERATION_START_MESSAGE = "Operation started";

        /// <summary>Log message for operation completion</summary>
        public const string OPERATION_COMPLETE_MESSAGE = "Operation completed";

        /// <summary>Log message for operation timeout</summary>
        public const string OPERATION_TIMEOUT_MESSAGE = "Operation timed out";

        /// <summary>Log message for operation cancellation</summary>
        public const string OPERATION_CANCELLED_MESSAGE = "Operation was cancelled";
        #endregion

        #region Log File Names
        /// <summary>Main log file name</summary>
        public const string MAIN_LOG_FILENAME = "MixerThresholdMod.log";

        /// <summary>Error log file name</summary>
        public const string ERROR_LOG_FILENAME = "MixerThresholdMod_Errors.log";

        /// <summary>Performance log file name</summary>
        public const string PERFORMANCE_LOG_FILENAME = "MixerThresholdMod_Performance.log";

        /// <summary>Debug log file name</summary>
        public const string DEBUG_LOG_FILENAME = "MixerThresholdMod_Debug.log";

        /// <summary>Backup log file name</summary>
        public const string BACKUP_LOG_FILENAME = "MixerThresholdMod_Backup.log";
        #endregion

        #region Log Level Strings
        /// <summary>String representation of Critical log level</summary>
        public const string LOG_LEVEL_CRITICAL_STRING = "CRITICAL";

        /// <summary>String representation of Error log level</summary>
        public const string LOG_LEVEL_ERROR_STRING = "ERROR";

        /// <summary>String representation of Warning log level</summary>
        public const string LOG_LEVEL_WARNING_STRING = "WARNING";

        /// <summary>String representation of Info log level</summary>
        public const string LOG_LEVEL_INFO_STRING = "INFO";

        /// <summary>String representation of Debug log level</summary>
        public const string LOG_LEVEL_DEBUG_STRING = "DEBUG";

        /// <summary>String representation of Trace log level</summary>
        public const string LOG_LEVEL_TRACE_STRING = "TRACE";
        #endregion
    }
}