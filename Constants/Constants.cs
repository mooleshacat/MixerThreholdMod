using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Centralized constants for logging levels, system configuration, timeouts, and error messages
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// 
    /// This centralized constants class eliminates magic numbers throughout the codebase
    /// and provides semantic meaning to all numerical and string literals used across
    /// the mod's comprehensive functionality.
    /// </summary>
    public static class ModConstants
    {
        // ===== LOGGING LEVEL CONSTANTS =====
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

        // ===== TIMEOUT AND PERFORMANCE CONSTANTS =====
        /// <summary>Standard operation timeout in milliseconds (2 seconds)</summary>
        public const int OPERATION_TIMEOUT_MS = 2000;

        /// <summary>Console command processing delay in milliseconds (1 second)</summary>
        public const int CONSOLE_COMMAND_DELAY_MS = 1000;

        /// <summary>Performance warning threshold in milliseconds (100ms)</summary>
        public const int PERFORMANCE_WARNING_THRESHOLD_MS = 100;

        /// <summary>Performance slow operation threshold in milliseconds (50ms)</summary>
        public const int PERFORMANCE_SLOW_THRESHOLD_MS = 50;

        /// <summary>System monitoring log interval (every 5th iteration)</summary>
        public const int SYSTEM_MONITORING_LOG_INTERVAL = 5;

        /// <summary>Load operation timeout in seconds (30 seconds)</summary>
        public const float LOAD_TIMEOUT_SECONDS = 30f;

        /// <summary>Attachment timeout in seconds (10 seconds)</summary>
        public const float ATTACH_TIMEOUT_SECONDS = 10f;

        /// <summary>Polling interval for value changes in seconds (200ms)</summary>
        public const float POLL_INTERVAL_SECONDS = 0.2f;

        /// <summary>Save cooldown period in seconds (2 seconds)</summary>
        public const int SAVE_COOLDOWN_SECONDS = 2;

        /// <summary>Backup interval in minutes (5 minutes)</summary>
        public const int BACKUP_INTERVAL_MINUTES = 5;

        // ===== STRESS TESTING CONSTANTS =====
        /// <summary>Maximum iterations warning threshold for game saves</summary>
        public const int GAME_SAVE_MAX_ITERATIONS_WARNING = 20;

        /// <summary>Maximum iterations warning threshold for mixer preferences</summary>
        public const int MIXER_PREF_MAX_ITERATIONS_WARNING = 100;

        /// <summary>Minimum delay recommended for game saves in seconds</summary>
        public const float GAME_SAVE_MIN_DELAY_SECONDS = 3f;

        /// <summary>Maximum delay warning threshold in seconds</summary>
        public const float MAX_DELAY_WARNING_SECONDS = 10f;

        /// <summary>Performance warning threshold for average save time</summary>
        public const float SAVE_PERFORMANCE_WARNING_SECONDS = 1.0f;

        /// <summary>Progress reporting interval for stress tests</summary>
        public const int STRESS_TEST_PROGRESS_INTERVAL = 5;

        /// <summary>Progress reporting interval for mixer preferences tests</summary>
        public const int MIXER_PREF_PROGRESS_INTERVAL = 10;

        // ===== MIXER CONFIGURATION CONSTANTS =====
        /// <summary>Minimum mixer threshold value</summary>
        public const float MIXER_THRESHOLD_MIN = 1f;

        /// <summary>Maximum mixer threshold value (matches game's stack size)</summary>
        public const float MIXER_THRESHOLD_MAX = 20f;

        /// <summary>Default mixer configuration enabled state</summary>
        public const bool MIXER_CONFIG_ENABLED_DEFAULT = true;

        /// <summary>Float comparison tolerance for value changes</summary>
        public const float MIXER_VALUE_TOLERANCE = 0.001f;

        // ===== MEMORY AND CONVERSION CONSTANTS =====
        /// <summary>Bytes to kilobytes conversion factor</summary>
        public const int BYTES_TO_KB = 1024;

        /// <summary>Bytes to megabytes conversion factor</summary>
        public const double BYTES_TO_MB = 1048576.0;

        /// <summary>Milliseconds per second conversion factor</summary>
        public const int MS_PER_SECOND = 1000;

        /// <summary>Seconds per minute conversion factor</summary>
        public const int SECONDS_PER_MINUTE = 60;

        /// <summary>Default delay value for operations (no delay)</summary>
        public const float DEFAULT_OPERATION_DELAY = 0f;

        // ===== ERROR MESSAGE CONSTANTS =====
        /// <summary>Error message template for invalid log levels</summary>
        public const string INVALID_MSG_LEVEL_ERROR = "[ERROR] Invalid log level {0} for Msg method. Must be {1} to {2}.";

        /// <summary>Error message template for invalid warning levels</summary>
        public const string INVALID_WARN_LEVEL_ERROR = "[ERROR] Invalid log level {0} for Warn method. Must be {1} or {2}.";

        /// <summary>Error message template for invalid operation delay</summary>
        public const string INVALID_OPERATION_DELAY_ERROR = "[ERROR] Invalid operation delay {0}. Must be greater than or equal to {1}.";

        /// <summary>Error message for invalid iteration count</summary>
        public const string INVALID_ITERATION_COUNT_ERROR = "[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.";

        /// <summary>Error message for invalid parameter format</summary>
        public const string INVALID_PARAMETER_ERROR = "[CONSOLE] Invalid parameter '{0}'. Must be a delay (number ≥ 0) or bypass flag (true/false).";

        /// <summary>Error message for no save path available</summary>
        public const string NO_SAVE_PATH_ERROR = "[CONSOLE] No current save path available. Load a game first.";

        /// <summary>Error message for no mixer data</summary>
        public const string NO_MIXER_DATA_ERROR = "[CONSOLE] No mixer data to save. Try adjusting some mixer thresholds first.";

        // ===== LOG PREFIX CONSTANTS =====
        /// <summary>Info log prefix</summary>
        public const string LOG_PREFIX_INFO = "[Info]";

        /// <summary>Warning log prefix</summary>
        public const string LOG_PREFIX_WARN = "[WARN]";

        /// <summary>Error log prefix</summary>
        public const string LOG_PREFIX_ERROR = "[ERROR]";

        /// <summary>Critical log prefix</summary>
        public const string LOG_PREFIX_CRITICAL = "[CRITICAL]";

        /// <summary>Console log prefix</summary>
        public const string LOG_PREFIX_CONSOLE = "[CONSOLE]";

        /// <summary>Bridge log prefix</summary>
        public const string LOG_PREFIX_BRIDGE = "[BRIDGE]";

        /// <summary>Save system log prefix</summary>
        public const string LOG_PREFIX_SAVE = "[SAVE]";

        /// <summary>System monitoring log prefix</summary>
        public const string LOG_PREFIX_SYSMON = "[SYSMON]";

        /// <summary>Directory resolver log prefix</summary>
        public const string LOG_PREFIX_DIR_RESOLVER = "[DIR-RESOLVER]";

        /// <summary>Initialization log prefix</summary>
        public const string LOG_PREFIX_INIT = "[INIT]";

        /// <summary>Profile log prefix</summary>
        public const string LOG_PREFIX_PROFILE = "[PROFILE]";

        /// <summary>Monitor log prefix</summary>
        public const string LOG_PREFIX_MONITOR = "[MONITOR]";

        /// <summary>Transaction log prefix</summary>
        public const string LOG_PREFIX_TRANSACTION = "[TRANSACTION]";

        /// <summary>Manual log prefix</summary>
        public const string LOG_PREFIX_MANUAL = "[MANUAL]";

        /// <summary>Game error log prefix</summary>
        public const string LOG_PREFIX_GAME_ERROR = "[GAME ERROR]";

        /// <summary>Game warning log prefix</summary>
        public const string LOG_PREFIX_GAME_WARNING = "[GAME WARNING]";

        /// <summary>Game log prefix</summary>
        public const string LOG_PREFIX_GAME = "[GAME]";

        // ===== NULL MESSAGE FALLBACK CONSTANTS =====
        /// <summary>Null message fallback</summary>
        public const string NULL_MESSAGE_FALLBACK = "[null message]";

        /// <summary>Null error message fallback</summary>
        public const string NULL_ERROR_FALLBACK = "[null error message]";

        /// <summary>Null command fallback</summary>
        public const string NULL_COMMAND_FALLBACK = "[null]";

        /// <summary>Not set path fallback</summary>
        public const string NOT_SET_PATH_FALLBACK = "[not set]";

        /// <summary>Not available path fallback</summary>
        public const string NOT_AVAILABLE_PATH_FALLBACK = "[not available yet]";

        // ===== FILE OPERATION CONSTANTS =====
        /// <summary>JSON file extension</summary>
        public const string JSON_FILE_EXTENSION = ".json";

        /// <summary>Temporary file extension</summary>
        public const string TEMP_FILE_EXTENSION = ".tmp";

        /// <summary>Mixer threshold save filename</summary>
        public const string MIXER_SAVE_FILENAME = "MixerThresholdSave.json";

        /// <summary>Emergency save filename</summary>
        public const string EMERGENCY_SAVE_FILENAME = "MixerThresholdSave_Emergency.json";

        /// <summary>File operation name for copy operations</summary>
        public const string FILE_COPY_OPERATION_NAME = "File Copy Operation";

        /// <summary>Save data version</summary>
        public const string SAVE_DATA_VERSION = "1.0.0";

        /// <summary>Emergency save reason</summary>
        public const string EMERGENCY_SAVE_REASON = "Emergency save before crash/shutdown";

        // ===== CONSOLE COMMAND CONSTANTS =====
        /// <summary>Command recognition result: recognized</summary>
        public const string COMMAND_RECOGNIZED = "RECOGNIZED";

        /// <summary>Command recognition result: unknown</summary>
        public const string COMMAND_UNKNOWN = "UNKNOWN";

        /// <summary>Parameter type: string</summary>
        public const string PARAM_TYPE_STRING = "STRING";

        /// <summary>Parameter type: integer</summary>
        public const string PARAM_TYPE_INTEGER = "INTEGER";

        /// <summary>Parameter type: float</summary>
        public const string PARAM_TYPE_FLOAT = "FLOAT";

        /// <summary>Parameter type: boolean</summary>
        public const string PARAM_TYPE_BOOLEAN = "BOOLEAN";

        // ===== SUCCESS/FAILURE MESSAGE CONSTANTS =====
        /// <summary>Success indicator</summary>
        public const string STATUS_SUCCESS = "SUCCESS";

        /// <summary>Failed indicator</summary>
        public const string STATUS_FAILED = "FAILED";

        /// <summary>Performance category: fast</summary>
        public const string PERFORMANCE_FAST = "FAST";

        /// <summary>Performance category: moderate</summary>
        public const string PERFORMANCE_MODERATE = "MODERATE";

        /// <summary>Performance category: slow</summary>
        public const string PERFORMANCE_SLOW = "SLOW";

        // ===== MOD IDENTIFICATION CONSTANTS =====
        /// <summary>Mod name for Harmony patches</summary>
        public const string HARMONY_MOD_ID = "MixerThreholdMod.Main";

        /// <summary>Console hook GameObject name</summary>
        public const string CONSOLE_HOOK_GAMEOBJECT_NAME = "MixerConsoleHook";

        /// <summary>Mod version for save data</summary>
        public const string MOD_VERSION = "1.0.0";

        /// <summary>Mod name with typo preserved</summary>
        public const string MOD_NAME = "MixerThreholdMod";

        // ===== DATETIME FORMAT CONSTANTS =====
        /// <summary>Standard datetime format for save files</summary>
        public const string SAVE_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>UTC timestamp format for console commands</summary>
        public const string CONSOLE_UTC_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>Directory detection completion format</summary>
        public const string DETECTION_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>Log file modification time format</summary>
        public const string LOG_FILE_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    }
}