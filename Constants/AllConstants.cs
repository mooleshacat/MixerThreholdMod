using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Comprehensive constants index providing access to all domain-specific constants
    /// This class serves as a centralized entry point for all constants across the application
    ///  IL2CPP COMPATIBLE: All imported constants are compile-time safe for AOT compilation
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    ///  THREAD SAFETY: All constants are immutable and thread-safe
    /// 
    /// DOMAIN COVERAGE:
    /// - LoggingConstants: 120+ logging-related constants
    /// - PerformanceConstants: 80+ performance and timing constants
    /// - MixerConstants: 70+ mixer-specific configuration constants
    /// - FileConstants: 130+ file operation and path constants
    /// - ThreadingConstants: 90+ threading and synchronization constants
    /// - ErrorConstants: 140+ error handling and recovery constants
    /// - SystemConstants: 100+ system and platform constants
    /// - GameConstants: 180+ game-specific UI, audio, graphics constants
    /// - ValidationConstants: 160+ validation and data integrity constants
    /// - NetworkConstants: 250+ network and communication constants
    /// - UtilityConstants: 250+ utility and formatting constants
    /// 
    /// TOTAL: 1570+ unique constants across 11 domain-specific files
    /// </summary>
    public static class AllConstants
    {
        #region Logging Constants Access
        /// <summary>Access all logging-related constants</summary>
        public static class Logging
        {
            // Log Levels
            public const int LOG_LEVEL_CRITICAL = LoggingConstants.LOG_LEVEL_CRITICAL;
            public const int LOG_LEVEL_IMPORTANT = LoggingConstants.LOG_LEVEL_IMPORTANT;
            public const int LOG_LEVEL_VERBOSE = LoggingConstants.LOG_LEVEL_VERBOSE;
            public const int WARN_LEVEL_CRITICAL = LoggingConstants.WARN_LEVEL_CRITICAL;
            public const int WARN_LEVEL_VERBOSE = LoggingConstants.WARN_LEVEL_VERBOSE;

            // Prefixes
            public const string SAVE_MANAGER_PREFIX = LoggingConstants.SAVE_MANAGER_PREFIX;
            public const string PERSISTENCE_PREFIX = LoggingConstants.PERSISTENCE_PREFIX;
            public const string DIRECTORY_RESOLVER_PREFIX = LoggingConstants.DIRECTORY_RESOLVER_PREFIX;
            public const string LOGGER_PREFIX = LoggingConstants.LOGGER_PREFIX;
            public const string MIXER_VALIDATION_PREFIX = LoggingConstants.MIXER_VALIDATION_PREFIX;

            // Messages
            public const string SAVE_SUCCESS_MESSAGE = LoggingConstants.SAVE_SUCCESS_MESSAGE;
            public const string SAVE_FAILURE_MESSAGE = LoggingConstants.SAVE_FAILURE_MESSAGE;
            public const string BACKUP_CREATED_MESSAGE = LoggingConstants.BACKUP_CREATED_MESSAGE;

            // File Names
            public const string MAIN_LOG_FILENAME = LoggingConstants.MAIN_LOG_FILENAME;
            public const string ERROR_LOG_FILENAME = LoggingConstants.ERROR_LOG_FILENAME;
            public const string PERFORMANCE_LOG_FILENAME = LoggingConstants.PERFORMANCE_LOG_FILENAME;
        }
        #endregion

        #region Performance Constants Access
        /// <summary>Access all performance-related constants</summary>
        public static class Performance
        {
            // Timeouts
            public const int OPERATION_TIMEOUT_MS = PerformanceConstants.OPERATION_TIMEOUT_MS;
            public const int CONSOLE_COMMAND_DELAY_MS = PerformanceConstants.CONSOLE_COMMAND_DELAY_MS;
            public const int FILE_OPERATION_TIMEOUT_MS = PerformanceConstants.FILE_OPERATION_TIMEOUT_MS;

            // Thresholds
            public const int PERFORMANCE_WARNING_THRESHOLD_MS = PerformanceConstants.PERFORMANCE_WARNING_THRESHOLD_MS;
            public const int PERFORMANCE_SLOW_THRESHOLD_MS = PerformanceConstants.PERFORMANCE_SLOW_THRESHOLD_MS;
            public const int PERFORMANCE_CRITICAL_THRESHOLD_MS = PerformanceConstants.PERFORMANCE_CRITICAL_THRESHOLD_MS;

            // Wait Times
            public const int RETRY_DELAY_MS = PerformanceConstants.RETRY_DELAY_MS;
            public const int SHORT_WAIT_MS = PerformanceConstants.SHORT_WAIT_MS;
            public const int MEDIUM_WAIT_MS = PerformanceConstants.MEDIUM_WAIT_MS;
        }
        #endregion

        #region Mixer Constants Access
        /// <summary>Access all mixer-related constants</summary>
        public static class Mixer
        {
            // Configuration
            public const int MIXER_CHANNEL_COUNT = MixerConstants.MIXER_CHANNEL_COUNT;
            public const int DEFAULT_MIXER_CHANNEL = MixerConstants.DEFAULT_MIXER_CHANNEL;
            public const float MAX_MIXER_VOLUME = MixerConstants.MAX_MIXER_VOLUME;
            public const float MIN_MIXER_VOLUME = MixerConstants.MIN_MIXER_VOLUME;
            public const float DEFAULT_MIXER_VOLUME = MixerConstants.DEFAULT_MIXER_VOLUME;

            // Keys
            public const string MIXER_VALUES_KEY = MixerConstants.MIXER_VALUES_KEY;
            public const string MIXER_VOLUME_KEY = MixerConstants.MIXER_VOLUME_KEY;
            public const string MIXER_GAIN_KEY = MixerConstants.MIXER_GAIN_KEY;

            // Files
            public const string MIXER_SAVE_FILENAME = MixerConstants.MIXER_SAVE_FILENAME;
            public const string MIXER_BACKUP_FILENAME = MixerConstants.MIXER_BACKUP_FILENAME;
            public const string MIXER_CONFIG_FILENAME = MixerConstants.MIXER_CONFIG_FILENAME;
        }
        #endregion

        #region File Constants Access
        /// <summary>Access all file operation constants</summary>
        public static class Files
        {
            // Extensions
            public const string JSON_EXTENSION = FileConstants.JSON_EXTENSION;
            public const string BACKUP_EXTENSION = FileConstants.BACKUP_EXTENSION;
            public const string TEMP_EXTENSION = FileConstants.TEMP_EXTENSION;
            public const string LOG_EXTENSION = FileConstants.LOG_EXTENSION;

            // Directories
            public const string BACKUP_DIRECTORY = FileConstants.BACKUP_DIRECTORY;
            public const string LOGS_DIRECTORY = FileConstants.LOGS_DIRECTORY;
            public const string CONFIG_DIRECTORY = FileConstants.CONFIG_DIRECTORY;

            // Operations
            public const string FILE_OP_READ = FileConstants.FILE_OP_READ;
            public const string FILE_OP_WRITE = FileConstants.FILE_OP_WRITE;
            public const string FILE_OP_BACKUP = FileConstants.FILE_OP_BACKUP;

            // Validation
            public const long MAX_FILE_SIZE_BYTES = FileConstants.MAX_FILE_SIZE_BYTES;
            public const int MAX_FILENAME_LENGTH = FileConstants.MAX_FILENAME_LENGTH;
            public const int MAX_PATH_LENGTH = FileConstants.MAX_PATH_LENGTH;
        }
        #endregion

        #region Threading Constants Access
        /// <summary>Access all threading-related constants</summary>
        public static class Threading
        {
            // Thread Names
            public const string MAIN_THREAD_NAME = ThreadingConstants.MAIN_THREAD_NAME;
            public const string SAVE_THREAD_NAME = ThreadingConstants.SAVE_THREAD_NAME;
            public const string BACKUP_THREAD_NAME = ThreadingConstants.BACKUP_THREAD_NAME;

            // Timeouts
            public const int MUTEX_TIMEOUT_MS = ThreadingConstants.MUTEX_TIMEOUT_MS;
            public const int LOCK_TIMEOUT_MS = ThreadingConstants.LOCK_TIMEOUT_MS;
            public const int DEFAULT_CANCELLATION_TIMEOUT_MS = ThreadingConstants.DEFAULT_CANCELLATION_TIMEOUT_MS;

            // Thread Pool
            public const int MIN_WORKER_THREADS = ThreadingConstants.MIN_WORKER_THREADS;
            public const int MAX_WORKER_THREADS = ThreadingConstants.MAX_WORKER_THREADS;
            public const int MAX_CONCURRENT_OPERATIONS = ThreadingConstants.MAX_CONCURRENT_OPERATIONS;
        }
        #endregion

        #region Error Constants Access
        /// <summary>Access all error handling constants</summary>
        public static class Errors
        {
            // Error Codes
            public const int ERROR_CODE_SUCCESS = ErrorConstants.ERROR_CODE_SUCCESS;
            public const int ERROR_CODE_GENERAL = ErrorConstants.ERROR_CODE_GENERAL;
            public const int ERROR_CODE_FILE_NOT_FOUND = ErrorConstants.ERROR_CODE_FILE_NOT_FOUND;
            public const int ERROR_CODE_TIMEOUT = ErrorConstants.ERROR_CODE_TIMEOUT;

            // Messages
            public const string ERROR_MESSAGE_GENERIC = ErrorConstants.ERROR_MESSAGE_GENERIC;
            public const string ERROR_MESSAGE_FILE_OPERATION = ErrorConstants.ERROR_MESSAGE_FILE_OPERATION;
            public const string ERROR_MESSAGE_TIMEOUT = ErrorConstants.ERROR_MESSAGE_TIMEOUT;

            // Recovery Strategies
            public const string RECOVERY_STRATEGY_RETRY = ErrorConstants.RECOVERY_STRATEGY_RETRY;
            public const string RECOVERY_STRATEGY_FALLBACK = ErrorConstants.RECOVERY_STRATEGY_FALLBACK;
            public const string RECOVERY_STRATEGY_EMERGENCY_SAVE = ErrorConstants.RECOVERY_STRATEGY_EMERGENCY_SAVE;
        }
        #endregion

        #region System Constants Access
        /// <summary>Access all system-related constants</summary>
        public static class System
        {
            // Mod Information
            public const string MOD_NAME = SystemConstants.MOD_NAME;
            public const string MOD_VERSION = SystemConstants.MOD_VERSION;
            public const string MOD_NAMESPACE = SystemConstants.MOD_NAMESPACE;

            // Platform
            public const string PLATFORM_UNITY = SystemConstants.PLATFORM_UNITY;
            public const string BACKEND_IL2CPP = SystemConstants.BACKEND_IL2CPP;
            public const string DOTNET_VERSION = SystemConstants.DOTNET_VERSION;

            // Components
            public const string COMPONENT_SAVE_MANAGER = SystemConstants.COMPONENT_SAVE_MANAGER;
            public const string COMPONENT_LOGGER = SystemConstants.COMPONENT_LOGGER;
            public const string COMPONENT_BACKUP_MANAGER = SystemConstants.COMPONENT_BACKUP_MANAGER;
        }
        #endregion

        #region Game Constants Access
        /// <summary>Access all game-specific constants</summary>
        public static class Game
        {
            // UI
            public const int UI_FONT_SIZE_DEFAULT = GameConstants.UI_FONT_SIZE_DEFAULT;
            public const int UI_BUTTON_WIDTH = GameConstants.UI_BUTTON_WIDTH;
            public const int UI_BUTTON_HEIGHT = GameConstants.UI_BUTTON_HEIGHT;

            // Audio
            public const float AUDIO_MASTER_VOLUME = GameConstants.AUDIO_MASTER_VOLUME;
            public const float AUDIO_DEFAULT_VOLUME = GameConstants.AUDIO_DEFAULT_VOLUME;

            // Graphics
            public const int GRAPHICS_TARGET_FPS = GameConstants.GRAPHICS_TARGET_FPS;
            public const int GRAPHICS_SCREEN_WIDTH = GameConstants.GRAPHICS_SCREEN_WIDTH;
            public const int GRAPHICS_SCREEN_HEIGHT = GameConstants.GRAPHICS_SCREEN_HEIGHT;

            // Physics
            public const float PHYSICS_GRAVITY = GameConstants.PHYSICS_GRAVITY;
            public const float PHYSICS_FIXED_TIMESTEP = GameConstants.PHYSICS_FIXED_TIMESTEP;
        }
        #endregion

        #region Validation Constants Access
        /// <summary>Access all validation-related constants</summary>
        public static class Validation
        {
            // Rules
            public const int MIN_STRING_LENGTH = ValidationConstants.MIN_STRING_LENGTH;
            public const int MAX_STRING_LENGTH = ValidationConstants.MAX_STRING_LENGTH;
            public const int DEFAULT_DECIMAL_PRECISION = ValidationConstants.DEFAULT_DECIMAL_PRECISION;

            // Patterns
            public const string EMAIL_REGEX_PATTERN = ValidationConstants.EMAIL_REGEX_PATTERN;
            public const string URL_REGEX_PATTERN = ValidationConstants.URL_REGEX_PATTERN;
            public const string NUMERIC_REGEX_PATTERN = ValidationConstants.NUMERIC_REGEX_PATTERN;

            // Messages
            public const string VALIDATION_REQUIRED = ValidationConstants.VALIDATION_REQUIRED;
            public const string VALIDATION_INVALID_FORMAT = ValidationConstants.VALIDATION_INVALID_FORMAT;
            public const string VALIDATION_INVALID_EMAIL = ValidationConstants.VALIDATION_INVALID_EMAIL;
        }
        #endregion

        #region Network Constants Access
        /// <summary>Access all network-related constants</summary>
        public static class Network
        {
            // HTTP Methods
            public const string HTTP_METHOD_GET = NetworkConstants.HTTP_METHOD_GET;
            public const string HTTP_METHOD_POST = NetworkConstants.HTTP_METHOD_POST;
            public const string HTTP_METHOD_PUT = NetworkConstants.HTTP_METHOD_PUT;

            // Status Codes
            public const int HTTP_STATUS_OK = NetworkConstants.HTTP_STATUS_OK;
            public const int HTTP_STATUS_NOT_FOUND = NetworkConstants.HTTP_STATUS_NOT_FOUND;
            public const int HTTP_STATUS_INTERNAL_SERVER_ERROR = NetworkConstants.HTTP_STATUS_INTERNAL_SERVER_ERROR;

            // Content Types
            public const string CONTENT_TYPE_JSON = NetworkConstants.CONTENT_TYPE_JSON;
            public const string CONTENT_TYPE_XML = NetworkConstants.CONTENT_TYPE_XML;
            public const string CONTENT_TYPE_TEXT = NetworkConstants.CONTENT_TYPE_TEXT;

            // Timeouts
            public const int HTTP_DEFAULT_TIMEOUT_MS = NetworkConstants.HTTP_DEFAULT_TIMEOUT_MS;
            public const int TCP_CONNECTION_TIMEOUT_MS = NetworkConstants.TCP_CONNECTION_TIMEOUT_MS;
        }
        #endregion

        #region Utility Constants Access
        /// <summary>Access all utility and formatting constants</summary>
        public static class Utility
        {
            // String Manipulation
            public const string EMPTY_STRING = UtilityConstants.EMPTY_STRING;
            public const string SPACE = UtilityConstants.SPACE;
            public const string NEWLINE = UtilityConstants.NEWLINE;
            public const string COMMA_SEPARATOR = UtilityConstants.COMMA_SEPARATOR;

            // Mathematical
            public const double PI = UtilityConstants.PI;
            public const double E = UtilityConstants.E;
            public const double DEGREES_TO_RADIANS = UtilityConstants.DEGREES_TO_RADIANS;

            // Conversion
            public const long BYTES_PER_KB = UtilityConstants.BYTES_PER_KB;
            public const long BYTES_PER_MB = UtilityConstants.BYTES_PER_MB;
            public const int MS_PER_SECOND = UtilityConstants.MS_PER_SECOND;

            // Date/Time Formats
            public const string DATE_FORMAT_ISO8601 = UtilityConstants.DATE_FORMAT_ISO8601;
            public const string TIME_FORMAT_24HOUR = UtilityConstants.TIME_FORMAT_24HOUR;
            public const string TIMESTAMP_FORMAT_MS = UtilityConstants.TIMESTAMP_FORMAT_MS;

            // Cultures
            public const string CULTURE_EN_US = UtilityConstants.CULTURE_EN_US;
            public const string CULTURE_INVARIANT = UtilityConstants.CULTURE_INVARIANT;
        }
        #endregion

        #region Constants Summary
        /// <summary>
        /// Summary of constants organization and usage guidelines
        /// </summary>
        public static class Summary
        {
            /// <summary>Total number of constants across all domains</summary>
            public const int TOTAL_CONSTANTS_COUNT = 1570;

            /// <summary>Number of domain-specific constant files</summary>
            public const int DOMAIN_FILES_COUNT = 11;

            /// <summary>Recommended usage pattern for constants access</summary>
            public const string USAGE_PATTERN = "using static MixerThreholdMod_1_0_0.Constants.[DomainName]Constants;";

            /// <summary>Alternative access through AllConstants class</summary>
            public const string ALTERNATIVE_ACCESS = "AllConstants.[Domain].[CONSTANT_NAME]";

            /// <summary>Constants organization principle</summary>
            public const string ORGANIZATION_PRINCIPLE = "Domain-driven separation of concerns";

            /// <summary>Thread safety guarantee</summary>
            public const string THREAD_SAFETY_GUARANTEE = "All constants are immutable and thread-safe";

            /// <summary>Compatibility assurance</summary>
            public const string COMPATIBILITY_ASSURANCE = ".NET 4.8.1, MONO, IL2CPP compatible";
        }
        #endregion
    }
}