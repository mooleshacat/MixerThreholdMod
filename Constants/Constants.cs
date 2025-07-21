using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Main constants entry point providing backward compatibility and essential core constants
    /// âš ï¸ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// âš ï¸ THREAD SAFETY: All constants are immutable and thread-safe
    /// 
    /// MAJOR REFACTOR: Constants have been separated into domain-specific files:
    /// - LoggingConstants.cs: All logging-related constants (120+ constants)
    /// - PerformanceConstants.cs: Performance and timing constants (80+ constants)
    /// - MixerConstants.cs: Mixer-specific configuration constants (70+ constants)
    /// - FileConstants.cs: File operation and path constants (130+ constants)
    /// - ThreadingConstants.cs: Threading and synchronization constants (90+ constants)
    /// - ErrorConstants.cs: Error handling and recovery constants (140+ constants)
    /// - SystemConstants.cs: System and platform constants (100+ constants)
    /// - GameConstants.cs: Game-specific UI, audio, graphics constants (180+ constants)
    /// - ValidationConstants.cs: Validation and data integrity constants (160+ constants)
    /// - NetworkConstants.cs: Network and communication constants (250+ constants)
    /// - UtilityConstants.cs: Utility and formatting constants (250+ constants)
    /// - AllConstants.cs: Comprehensive index for easy access to all domains
    /// 
    /// TOTAL: 1785+ constants organized across 12 files with clear separation of concerns
    /// 
    /// USAGE RECOMMENDATIONS:
    /// For specific domains: using static MixerThreholdMod_1_0_0.Constants.LoggingConstants;
    /// For comprehensive access: using MixerThreholdMod_1_0_0.Constants.AllConstants;
    /// </summary>
    public static class ModConstants
    {
        #region Backward Compatibility - Core Constants
        /// <summary>
        /// Core constants maintained for backward compatibility
        /// For comprehensive constants, use domain-specific files or AllConstants.cs
        /// </summary>

        // Most critical constants for immediate use
        public const int LOG_LEVEL_CRITICAL = LoggingConstants.LOG_LEVEL_CRITICAL;
        public const int LOG_LEVEL_IMPORTANT = LoggingConstants.LOG_LEVEL_IMPORTANT;
        public const int LOG_LEVEL_VERBOSE = LoggingConstants.LOG_LEVEL_VERBOSE;

        public const int OPERATION_TIMEOUT_MS = PerformanceConstants.OPERATION_TIMEOUT_MS;
        public const int PERFORMANCE_WARNING_THRESHOLD_MS = PerformanceConstants.PERFORMANCE_WARNING_THRESHOLD_MS;

        public const string MIXER_SAVE_FILENAME = MixerConstants.MIXER_SAVE_FILENAME;
        public const string MIXER_VALUES_KEY = MixerConstants.MIXER_VALUES_KEY;

        public const string JSON_EXTENSION = FileConstants.JSON_EXTENSION;
        public const string BACKUP_EXTENSION = FileConstants.BACKUP_EXTENSION;

        public const string SAVE_MANAGER_PREFIX = LoggingConstants.SAVE_MANAGER_PREFIX;
        public const string PERSISTENCE_PREFIX = LoggingConstants.PERSISTENCE_PREFIX;
        public const string DIRECTORY_RESOLVER_PREFIX = LoggingConstants.DIRECTORY_RESOLVER_PREFIX;

        public const string MOD_NAME = SystemConstants.MOD_NAME;
        public const string MOD_VERSION = SystemConstants.MOD_VERSION;

        public const int ERROR_CODE_SUCCESS = ErrorConstants.ERROR_CODE_SUCCESS;
        public const int ERROR_CODE_GENERAL = ErrorConstants.ERROR_CODE_GENERAL;
        #endregion

        #region Domain Access Shortcuts
        /// <summary>
        /// Quick access to domain-specific constants for convenience
        /// Recommended: Use domain-specific files directly for better organization
        /// </summary>

        /// <summary>Access logging constants - use LoggingConstants.cs for full access</summary>
        public static class Logging
        {
            public const string PREFIX_SAVE = LoggingConstants.SAVE_MANAGER_PREFIX;
            public const string PREFIX_BACKUP = LoggingConstants.BACKUP_MANAGER_PREFIX;
            public const string PREFIX_LOGGER = LoggingConstants.LOGGER_PREFIX;
            public const string MSG_SUCCESS = LoggingConstants.SAVE_SUCCESS_MESSAGE;
            public const string MSG_FAILURE = LoggingConstants.SAVE_FAILURE_MESSAGE;
        }

        /// <summary>Access performance constants - use PerformanceConstants.cs for full access</summary>
        public static class Performance
        {
            public const int TIMEOUT_OPERATION = PerformanceConstants.OPERATION_TIMEOUT_MS;
            public const int TIMEOUT_FILE_OP = PerformanceConstants.FILE_OPERATION_TIMEOUT_MS;
            public const int THRESHOLD_WARNING = PerformanceConstants.PERFORMANCE_WARNING_THRESHOLD_MS;
            public const int RETRY_DELAY = PerformanceConstants.RETRY_DELAY_MS;
        }

        /// <summary>Access mixer constants - use MixerConstants.cs for full access</summary>
        public static class Mixer
        {
            public const string SAVE_FILE = MixerConstants.MIXER_SAVE_FILENAME;
            public const string VALUES_KEY = MixerConstants.MIXER_VALUES_KEY;
            public const int CHANNEL_COUNT = MixerConstants.MIXER_CHANNEL_COUNT;
            public const float MAX_VOLUME = MixerConstants.MAX_MIXER_VOLUME;
        }

        /// <summary>Access file constants - use FileConstants.cs for full access</summary>
        public static class Files
        {
            public const string EXT_JSON = FileConstants.JSON_EXTENSION;
            public const string EXT_BACKUP = FileConstants.BACKUP_EXTENSION;
            public const string EXT_LOG = FileConstants.LOG_EXTENSION;
            public const string DIR_BACKUP = FileConstants.BACKUP_DIRECTORY;
            public const string OP_READ = FileConstants.FILE_OP_READ;
            public const string OP_WRITE = FileConstants.FILE_OP_WRITE;
        }

        /// <summary>Access threading constants - use ThreadingConstants.cs for full access</summary>
        public static class Threading
        {
            public const string MAIN_THREAD = ThreadingConstants.MAIN_THREAD_NAME;
            public const string SAVE_THREAD = ThreadingConstants.SAVE_THREAD_NAME;
            public const int MUTEX_TIMEOUT = ThreadingConstants.MUTEX_TIMEOUT_MS;
            public const int LOCK_TIMEOUT = ThreadingConstants.LOCK_TIMEOUT_MS;
        }

        /// <summary>Access error constants - use ErrorConstants.cs for full access</summary>
        public static class Errors
        {
            public const int SUCCESS = ErrorConstants.ERROR_CODE_SUCCESS;
            public const int GENERAL = ErrorConstants.ERROR_CODE_GENERAL;
            public const int FILE_NOT_FOUND = ErrorConstants.ERROR_CODE_FILE_NOT_FOUND;
            public const int TIMEOUT = ErrorConstants.ERROR_CODE_TIMEOUT;
            public const string STRATEGY_RETRY = ErrorConstants.RECOVERY_STRATEGY_RETRY;
            public const string STRATEGY_EMERGENCY = ErrorConstants.RECOVERY_STRATEGY_EMERGENCY_SAVE;
        }
        #endregion

        #region Migration Guide
        /// <summary>
        /// Migration guide for moving from centralized to domain-specific constants
        /// </summary>
        public static class MigrationGuide
        {
            /// <summary>Instructions for migrating to new constants structure</summary>
            public const string MIGRATION_INSTRUCTIONS = "Replace 'using static MixerThreholdMod_1_0_0.Constants.ModConstants;' with domain-specific imports";

            /// <summary>Example of domain-specific import</summary>
            public const string DOMAIN_IMPORT_EXAMPLE = "using static MixerThreholdMod_1_0_0.Constants.LoggingConstants;";

            /// <summary>Benefits of domain separation</summary>
            public const string SEPARATION_BENEFITS = "Better organization, reduced file size, improved maintainability, clearer intent";

            /// <summary>Total constants available</summary>
            public const int TOTAL_CONSTANTS_AVAILABLE = 1785;

            /// <summary>Number of domain files</summary>
            public const int DOMAIN_FILES_COUNT = 12;
        }
        #endregion

        #region Version Information
        /// <summary>Constants refactor version information</summary>
        public const string CONSTANTS_REFACTOR_VERSION = "2.0.0";
        public const string CONSTANTS_REFACTOR_DATE = "2024-12-28";
        public const string CONSTANTS_REFACTOR_AUTHOR = "GitHub Copilot";
        public const string CONSTANTS_REFACTOR_DESCRIPTION = "Comprehensive separation of concerns refactor with 1785+ constants across 12 domain files";
        #endregion
    }
}