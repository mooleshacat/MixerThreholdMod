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
    }
}