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

        // ===== MIXER CONFIGURATION CONSTANTS =====
        /// <summary>Minimum mixer threshold value</summary>
        public const float MIXER_THRESHOLD_MIN = 1f;

        /// <summary>Maximum mixer threshold value (matches game's stack size)</summary>
        public const float MIXER_THRESHOLD_MAX = 20f;

        /// <summary>Default mixer configuration enabled state</summary>
        public const bool MIXER_CONFIG_ENABLED_DEFAULT = true;

        /// <summary>Float comparison tolerance for value changes</summary>
        public const float MIXER_VALUE_TOLERANCE = 0.001f;

        // ===== FILE OPERATION CONSTANTS =====
        /// <summary>JSON file extension</summary>
        public const string JSON_FILE_EXTENSION = ".json";

        /// <summary>Temporary file extension</summary>
        public const string TEMP_FILE_EXTENSION = ".tmp";

        /// <summary>Lock file extension</summary>
        public const string LOCK_FILE_EXTENSION = ".lock";

        /// <summary>Mixer threshold save filename</summary>
        public const string MIXER_SAVE_FILENAME = "MixerThresholdSave.json";

        /// <summary>Emergency save filename</summary>
        public const string EMERGENCY_SAVE_FILENAME = "MixerThresholdSave_Emergency.json";

        /// <summary>Backup filename pattern</summary>
        public const string BACKUP_FILENAME_PATTERN = "MixerThresholdSave_backup_{0}.json";

        /// <summary>Backup filename wildcard pattern for file searches</summary>
        public const string BACKUP_FILENAME_WILDCARD = "MixerThresholdSave_backup_*.json";

        // ===== MOD IDENTIFICATION CONSTANTS =====
        /// <summary>Mod name for Harmony patches</summary>
        public const string HARMONY_MOD_ID = "MixerThreholdMod.Main";

        /// <summary>Mod version for save data</summary>
        public const string MOD_VERSION = "1.0.0";

        /// <summary>Mod name with typo preserved</summary>
        public const string MOD_NAME = "MixerThreholdMod";

        /// <summary>Patch name for EntityConfiguration Destroy patch</summary>
        public const string PATCH_ENTITY_DESTROY_NAME = "EntityConfiguration_Destroy_Patch";

        // ===== COMPONENT NAME CONSTANTS =====
        /// <summary>Performance optimizer component name</summary>
        public const string PERFORMANCE_OPTIMIZER_NAME = "PerformanceOptimizer";

        // ===== LOGGING PREFIX CONSTANTS =====
        /// <summary>Logging prefix for advanced save operations</summary>
        public const string ADVANCED_SAVE_OPERATION_PREFIX = "[ADVANCED_SAVE]";

        /// <summary>Logging prefix for stress test operations</summary>
        public const string STRESS_TEST_PREFIX = "[STRESS_TEST]";

        /// <summary>Logging prefix for backup operations</summary>
        public const string BACKUP_OPERATION_PREFIX = "[BACKUP]";

        /// <summary>Logging prefix for directory resolver operations</summary>
        public const string DIRECTORY_RESOLVER_PREFIX = "[DIR-RESOLVER]";

        /// <summary>Logging prefix for file lock operations</summary>
        public const string FILE_LOCK_PREFIX = "[FILE_LOCK]";

        /// <summary>Logging prefix for IO runner operations</summary>
        public const string IO_RUNNER_PREFIX = "[IO_RUNNER]";

        /// <summary>Logging prefix for diagnostics operations</summary>
        public const string DIAGNOSTICS_PREFIX = "[DIAGNOSTICS]";

        /// <summary>Logging prefix for persistence operations</summary>
        public const string PERSISTENCE_PREFIX = "[PERSISTENCE]";

        /// <summary>Logging prefix for utility operations</summary>
        public const string UTILS_PREFIX = "[UTILS]";

        /// <summary>Logging prefix for emergency save manager operations</summary>
        public const string EMERGENCY_SAVE_PREFIX = "[EmergencySaveManager]";

        /// <summary>Logging prefix for backup save manager operations</summary>
        public const string BACKUP_SAVE_PREFIX = "[BackupSaveManager]";

        /// <summary>Logging prefix for mixer data reader operations</summary>
        public const string MIXER_DATA_READER_PREFIX = "[MixerDataReader]";

        /// <summary>Logging prefix for mixer data performance metrics operations</summary>
        public const string PERFORMANCE_METRICS_PREFIX = "[MixerDataPerformanceMetrics]";

        /// <summary>Logging prefix for save manager patch operations</summary>
        public const string SAVE_MANAGER_PATCH_PREFIX = "[SaveManager_Save_Patch]";

        // ===== ERROR MESSAGE CONSTANTS =====
        /// <summary>Error message template for invalid log levels</summary>
        public const string INVALID_MSG_LEVEL_ERROR = "[ERROR] Invalid log level {0} for Msg method. Must be {1} to {2}.";

        /// <summary>Error message template for invalid warning levels</summary>
        public const string INVALID_WARN_LEVEL_ERROR = "[ERROR] Invalid log level {0} for Warn method. Must be {1} or {2}.";

        /// <summary>Error message for no save path available</summary>
        public const string NO_SAVE_PATH_ERROR = "[CONSOLE] No current save path available. Load a game first.";

        /// <summary>Error message for no mixer data</summary>
        public const string NO_MIXER_DATA_ERROR = "[CONSOLE] No mixer data to save. Try adjusting some mixer thresholds first.";

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

        // ===== COMMAND NAME CONSTANTS =====
        /// <summary>Console command for resetting mixer values</summary>
        public const string COMMAND_RESET_MIXER_VALUES = "mixer_reset";

        /// <summary>Console command for saving mixer values</summary>
        public const string COMMAND_MIXER_SAVE = "mixer_save";

        /// <summary>Console command for displaying mixer path</summary>
        public const string COMMAND_MIXER_PATH = "mixer_path";

        /// <summary>Console command for emergency save</summary>
        public const string COMMAND_MIXER_EMERGENCY = "mixer_emergency";

        /// <summary>Console command for stress testing mixer preferences saves</summary>
        public const string COMMAND_SAVE_PREF_STRESS = "saveprefstress";

        /// <summary>Console command for stress testing game saves</summary>
        public const string COMMAND_SAVE_GAME_STRESS = "savegamestress";

        /// <summary>Console command for comprehensive save monitoring</summary>
        public const string COMMAND_SAVE_MONITOR = "savemonitor";

        /// <summary>Console command for help information</summary>
        public const string COMMAND_HELP = "help";

        /// <summary>Command name for resetting mixer values (alternative)</summary>
        public const string COMMAND_MIXER_RESET = "mixer_reset";

        // ===== PARAMETER NAME CONSTANTS =====
        /// <summary>Parameter name for timeout in milliseconds</summary>
        public const string TIMEOUT_MS_PARAM = "timeoutMs";

        /// <summary>Parameter name for cancellation token</summary>
        public const string CANCELLATION_TOKEN_PARAM = "cancellationToken";

        /// <summary>Parameter name for operation context</summary>
        public const string OPERATION_CONTEXT_PARAM = "operationContext";

        // ===== KEY NAME CONSTANTS =====
        /// <summary>JSON key for mixer values in save data</summary>
        public const string MIXER_VALUES_KEY = "MixerValues";

        /// <summary>JSON key for save timestamp</summary>
        public const string SAVE_TIME_KEY = "SaveTime";

        /// <summary>JSON key for mod version in save data</summary>
        public const string VERSION_KEY = "Version";

        /// <summary>JSON key for mixer ID in save data</summary>
        public const string MIXER_ID_KEY = "MixerID";

        /// <summary>JSON key for threshold value in save data</summary>
        public const string THRESHOLD_VALUE_KEY = "ThresholdValue";

        // ===== DATETIME FORMAT CONSTANTS =====
        /// <summary>Standard datetime format for save files and logging</summary>
        public const string STANDARD_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>UTC datetime format with milliseconds for precise timestamps</summary>
        public const string UTC_DATETIME_FORMAT_WITH_MS = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>Filename-safe datetime format for backup files</summary>
        public const string FILENAME_DATETIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";

        /// <summary>ISO 8601 datetime format for compatibility</summary>
        public const string ISO_DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffZ";

        // ===== FALLBACK AND DEFAULT VALUES =====
        /// <summary>Fallback value for null or empty commands</summary>
        public const string NULL_COMMAND_FALLBACK = "[null_command]";

        /// <summary>Fallback value for null or empty paths</summary>
        public const string NULL_PATH_FALLBACK = "[null_path]";

        /// <summary>Fallback value for null or empty strings</summary>
        public const string NULL_STRING_FALLBACK = "[null_string]";

        /// <summary>Fallback value for null or empty log messages</summary>
        public const string NULL_MESSAGE_FALLBACK = "[null_message]";

        /// <summary>Default mixer ID when none is assigned</summary>
        public const int DEFAULT_MIXER_ID = -1;

        /// <summary>Invalid mixer ID indicator</summary>
        public const int INVALID_MIXER_ID = -999;

        // ===== ERROR HANDLING CONSTANTS =====
        /// <summary>Maximum retry attempts for file operations</summary>
        public const int MAX_RETRY_ATTEMPTS = 3;

        /// <summary>Retry delay in milliseconds</summary>
        public const int RETRY_DELAY_MS = 500;

        /// <summary>Maximum error message length for logging</summary>
        public const int MAX_ERROR_MESSAGE_LENGTH = 500;

        // ===== SYSTEM MONITORING CONSTANTS =====
        /// <summary>Memory leak detection threshold in MB</summary>
        public const double MEMORY_LEAK_THRESHOLD_MB = 100.0;

        /// <summary>Performance monitoring sample interval</summary>
        public const int PERFORMANCE_SAMPLE_INTERVAL_MS = 100;

        /// <summary>System health check interval in seconds</summary>
        public const int SYSTEM_HEALTH_CHECK_INTERVAL_SECONDS = 30;

        // ===== FILE OPERATION CONSTANTS =====
        /// <summary>Default file buffer size for read/write operations</summary>
        public const int DEFAULT_FILE_BUFFER_SIZE = 4096;

        /// <summary>Maximum concurrent file operations allowed</summary>
        public const int MAX_CONCURRENT_OPERATIONS = 10;

        /// <summary>File operation timeout in seconds</summary>
        public const float FILE_OPERATION_TIMEOUT_SECONDS = 30f;

        // ===== VALIDATION CONSTANTS =====
        /// <summary>Minimum valid file size in bytes</summary>
        public const int MIN_VALID_FILE_SIZE_BYTES = 10;

        /// <summary>Maximum expected save file size in bytes (1MB)</summary>
        public const int MAX_EXPECTED_SAVE_FILE_SIZE_BYTES = 1048576;

        /// <summary>Minimum valid JSON string length</summary>
        public const int MIN_VALID_JSON_LENGTH = 2;

        // ===== THREAD SAFETY CONSTANTS =====
        /// <summary>Thread safety lock timeout in milliseconds</summary>
        public const int THREAD_LOCK_TIMEOUT_MS = 5000;

        /// <summary>Async operation cancellation timeout in seconds</summary>
        public const int ASYNC_CANCELLATION_TIMEOUT_SECONDS = 10;

        /// <summary>Task completion timeout in milliseconds</summary>
        public const int TASK_COMPLETION_TIMEOUT_MS = 15000;

        // ===== WAIT TIME CONSTANTS =====
        /// <summary>Short wait time in seconds (100ms)</summary>
        public const float SHORT_WAIT_SECONDS = 0.1f;

        /// <summary>Medium wait time in seconds (500ms)</summary>
        public const float MEDIUM_WAIT_SECONDS = 0.5f;

        /// <summary>Standard wait time in seconds (1 second)</summary>
        public const float STANDARD_WAIT_SECONDS = 1.0f;

        /// <summary>Long wait time in seconds (1.5 seconds)</summary>
        public const float LONG_WAIT_SECONDS = 1.5f;

        /// <summary>Extended wait time in seconds (2 seconds)</summary>
        public const float EXTENDED_WAIT_SECONDS = 2.0f;

        // ===== FILE EXTENSION CONSTANTS =====
        /// <summary>Backup file extension</summary>
        public const string BACKUP_FILE_EXTENSION = ".backup";

        /// <summary>Log file extension</summary>
        public const string LOG_FILE_EXTENSION = ".log";

        /// <summary>Text file extension</summary>
        public const string TEXT_FILE_EXTENSION = ".txt";

        // ===== ASSEMBLY AND VERSION CONSTANTS =====
        /// <summary>Assembly name for IL2CPP type loading</summary>
        public const string ASSEMBLY_CSHARP = "Assembly-CSharp";

        /// <summary>Unity Assembly name for type loading</summary>
        public const string UNITY_ENGINE_ASSEMBLY = "UnityEngine";

        /// <summary>MelonLoader Assembly name</summary>
        public const string MELONLOADER_ASSEMBLY = "MelonLoader";

        // ===== NUMERIC PRECISION CONSTANTS =====
        /// <summary>Default decimal precision for logging</summary>
        public const int DEFAULT_DECIMAL_PRECISION = 3;

        /// <summary>High precision decimal places</summary>
        public const int HIGH_PRECISION_DECIMALS = 6;

        /// <summary>Memory size precision for display</summary>
        public const int MEMORY_SIZE_PRECISION = 2;

        // ===== SAFETY AND LIMITS CONSTANTS =====
        /// <summary>Maximum safe loop iterations</summary>
        public const int MAX_SAFE_LOOP_ITERATIONS = 1000;

        /// <summary>Maximum safe collection size</summary>
        public const int MAX_SAFE_COLLECTION_SIZE = 10000;

        /// <summary>Maximum safe string length for logging</summary>
        public const int MAX_SAFE_STRING_LENGTH = 1000;

        // ===== DEBUG AND CONDITIONAL COMPILATION =====
        /// <summary>Debug mode identifier string</summary>
        public const string DEBUG_MODE = "DEBUG";

        /// <summary>Release mode identifier string</summary>
        public const string RELEASE_MODE = "RELEASE";

        /// <summary>IL2CPP build identifier</summary>
        public const string IL2CPP_BUILD = "IL2CPP";

        /// <summary>MONO build identifier</summary>
        public const string MONO_BUILD = "MONO";
    }
}