

using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Centralized constants for logging levels, system configuration, timeouts, and error messages
    /// âš ï¸ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// âš ï¸ THREAD SAFETY: All constants are immutable and thread-safe
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

        // ===== REFLECTION AND TYPE CONSTANTS =====
        /// <summary>Exception Message property name for reflection</summary>
        public const string EXCEPTION_MESSAGE_PROPERTY = "Message";

        /// <summary>Exception StackTrace property name for reflection</summary>
        public const string EXCEPTION_STACKTRACE_PROPERTY = "StackTrace";

        /// <summary>GetType method name for reflection</summary>
        public const string GET_TYPE_METHOD_NAME = "GetType";

        /// <summary>GetExecutingAssembly method name for reflection</summary>
        public const string GET_EXECUTING_ASSEMBLY_METHOD = "GetExecutingAssembly";

        /// <summary>Load method name for Assembly loading</summary>
        public const string ASSEMBLY_LOAD_METHOD = "Load";

        /// <summary>Location property name for Assembly</summary>
        public const string ASSEMBLY_LOCATION_PROPERTY = "Location";

        /// <summary>Length property name for arrays and collections</summary>
        public const string LENGTH_PROPERTY_NAME = "Length";

        /// <summary>Count property name for collections</summary>
        public const string COUNT_PROPERTY_NAME = "Count";

        /// <summary>CurrentDomain property name for AppDomain</summary>
        public const string CURRENT_DOMAIN_PROPERTY = "CurrentDomain";

        // ===== BINDING FLAGS CONSTANTS =====
        /// <summary>Public binding flag identifier</summary>
        public const string BINDING_FLAGS_PUBLIC = "Public";

        /// <summary>NonPublic binding flag identifier</summary>
        public const string BINDING_FLAGS_NON_PUBLIC = "NonPublic";

        /// <summary>Static binding flag identifier</summary>
        public const string BINDING_FLAGS_STATIC = "Static";

        /// <summary>Instance binding flag identifier</summary>
        public const string BINDING_FLAGS_INSTANCE = "Instance";

        // ===== FILE OPERATION METHOD NAMES =====
        /// <summary>File.Exists method name</summary>
        public const string FILE_EXISTS_METHOD = "Exists";

        /// <summary>Directory.Exists method name</summary>
        public const string DIRECTORY_EXISTS_METHOD = "Exists";

        /// <summary>File.ReadAllText method name</summary>
        public const string FILE_READ_ALL_TEXT_METHOD = "ReadAllText";

        /// <summary>File.WriteAllText method name</summary>
        public const string FILE_WRITE_ALL_TEXT_METHOD = "WriteAllText";

        /// <summary>File.Delete method name</summary>
        public const string FILE_DELETE_METHOD = "Delete";

        /// <summary>Directory.CreateDirectory method name</summary>
        public const string DIRECTORY_CREATE_METHOD = "CreateDirectory";

        /// <summary>Path.Combine method name</summary>
        public const string PATH_COMBINE_METHOD = "Combine";

        /// <summary>Path.GetDirectoryName method name</summary>
        public const string PATH_GET_DIRECTORY_NAME_METHOD = "GetDirectoryName";

        /// <summary>Path.GetFileName method name</summary>
        public const string PATH_GET_FILENAME_METHOD = "GetFileName";

        // ===== STRING OPERATION CONSTANTS =====
        /// <summary>String.Format method name</summary>
        public const string STRING_FORMAT_METHOD = "Format";

        /// <summary>String.IsNullOrEmpty method name</summary>
        public const string STRING_IS_NULL_OR_EMPTY_METHOD = "IsNullOrEmpty";

        /// <summary>String.StartsWith method name</summary>
        public const string STRING_STARTS_WITH_METHOD = "StartsWith";

        /// <summary>String.Contains method name</summary>
        public const string STRING_CONTAINS_METHOD = "Contains";

        /// <summary>String.Split method name</summary>
        public const string STRING_SPLIT_METHOD = "Split";

        /// <summary>String.Substring method name</summary>
        public const string STRING_SUBSTRING_METHOD = "Substring";

        /// <summary>String.IndexOf method name</summary>
        public const string STRING_INDEX_OF_METHOD = "IndexOf";

        // ===== GUID AND VERSION CONSTANTS =====
        /// <summary>Assembly GUID for project identification</summary>
        public const string ASSEMBLY_GUID = "17e5161c-09cb-40a1-b3ae-2d7e968e8660";

        /// <summary>Assembly version string</summary>
        public const string ASSEMBLY_VERSION = "1.0.0.0";

        /// <summary>Assembly file version string</summary>
        public const string ASSEMBLY_FILE_VERSION = "1.0.0.0";

        /// <summary>Assembly copyright string</summary>
        public const string ASSEMBLY_COPYRIGHT = "Copyright Â©  2025 mooleshacat";

        /// <summary>Assembly title string</summary>
        public const string ASSEMBLY_TITLE = "MixerThreholdMod-1_0_0";

        /// <summary>Assembly product string</summary>
        public const string ASSEMBLY_PRODUCT = "MixerThreholdMod-1_0_0";

        /// <summary>Assembly description string</summary>
        public const string ASSEMBLY_DESCRIPTION = "Schedule 1 MixerThreholdMod";

        // ===== EXTENDED FILE EXTENSIONS =====
        /// <summary>Backup file extension (alternative)</summary>
        public const string BAK_FILE_EXTENSION = ".bak";

        /// <summary>Emergency file extension</summary>
        public const string EMERGENCY_FILE_EXTENSION = ".emergency";

        // ===== PERFORMANCE THRESHOLD CONSTANTS =====
        /// <summary>Framework compatibility version for .NET 4.8</summary>
        public const float FRAMEWORK_VERSION_4_8 = 4.8f;

        /// <summary>Memory optimization threshold in KB</summary>
        public const double MEMORY_OPTIMIZATION_THRESHOLD_KB = 512.0;

        /// <summary>Memory optimization threshold in MB</summary>
        public const double MEMORY_OPTIMIZATION_THRESHOLD_MB = 0.5;

        /// <summary>GC collection threshold</summary>
        public const double GC_COLLECTION_THRESHOLD_MB = 100.0;

        /// <summary>Performance monitoring sample rate</summary>
        public const float PERFORMANCE_SAMPLE_RATE = 0.1f;

        /// <summary>High performance threshold</summary>
        public const float HIGH_PERFORMANCE_THRESHOLD = 0.001f;

        /// <summary>Standard wait time in seconds (alternative)</summary>
        public const float STANDARD_WAIT_TIME_SECONDS = 1.0f;

        /// <summary>Medium wait time in seconds (alternative)</summary>
        public const float MEDIUM_WAIT_TIME_SECONDS = 0.5f;

        /// <summary>Performance critical threshold</summary>
        public const float PERFORMANCE_CRITICAL_THRESHOLD = 2.0f;

        /// <summary>Memory threshold for optimization in bytes</summary>
        public const double MEMORY_THRESHOLD_BYTES = 1048576.0;

        /// <summary>Memory threshold in KB</summary>
        public const double MEMORY_THRESHOLD_KB = 1024.0;

        /// <summary>Default frame rate target</summary>
        public const double DEFAULT_FRAME_RATE = 60.0;

        /// <summary>Frame rate monitoring threshold</summary>
        public const double FRAME_RATE_THRESHOLD = 30.0;

        /// <summary>Performance optimization interval</summary>
        public const double OPTIMIZATION_INTERVAL_SECONDS = 5.0;

        /// <summary>System monitoring threshold</summary>
        public const double SYSTEM_MONITORING_THRESHOLD = 3.0;

        /// <summary>Performance tolerance threshold</summary>
        public const float PERFORMANCE_TOLERANCE = 1.5f;

        // ===== UNIT MULTIPLIER CONSTANTS =====
        /// <summary>Bytes to KB multiplier (alternative)</summary>
        public const int BYTES_TO_KILOBYTES = 1024;

        /// <summary>KB to MB multiplier</summary>
        public const int KB_TO_MB = 1024;

        /// <summary>MB to GB multiplier</summary>
        public const int MB_TO_GB = 1024;

        /// <summary>Milliseconds to seconds divisor</summary>
        public const int MS_TO_SECONDS = 1000;

        /// <summary>Seconds to minutes divisor</summary>
        public const int SECONDS_TO_MINUTES = 60;

        /// <summary>Minutes to hours divisor</summary>
        public const int MINUTES_TO_HOURS = 60;

        // ===== SPECIAL CHARACTER CONSTANTS =====
        /// <summary>Single space character</summary>
        public const string SINGLE_SPACE = " ";

        /// <summary>Comma separator with space</summary>
        public const string COMMA_SEPARATOR = ", ";

        /// <summary>Ellipsis character sequence</summary>
        public const string ELLIPSIS = "...";

        /// <summary>Question mark character</summary>
        public const string QUESTION_MARK = "?";

        /// <summary>Forward slash separator</summary>
        public const string FORWARD_SLASH = "/";

        /// <summary>Backslash separator</summary>
        public const string BACKSLASH = "\\";

        /// <summary>Double backslash escape</summary>
        public const string DOUBLE_BACKSLASH = "\\\\";

        /// <summary>Newline character sequence</summary>
        public const string NEWLINE = "\n";

        /// <summary>Carriage return and newline</summary>
        public const string CRLF = "\r\n";

        /// <summary>Tab character</summary>
        public const string TAB = "\t";

        // ===== ARRAY AND COLLECTION CONSTANTS =====
        /// <summary>Empty string array identifier</summary>
        public const string EMPTY_STRING_ARRAY = "EmptyStringArray";

        /// <summary>Array copy method name</summary>
        public const string ARRAY_COPY_METHOD = "Copy";

        /// <summary>Array empty method name</summary>
        public const string ARRAY_EMPTY_METHOD = "Empty";

        /// <summary>OrderByDescending LINQ method name</summary>
        public const string ORDER_BY_DESCENDING_METHOD = "OrderByDescending";

        /// <summary>Skip LINQ method name</summary>
        public const string SKIP_METHOD = "Skip";

        /// <summary>Take LINQ method name</summary>
        public const string TAKE_METHOD = "Take";

        // ===== ERROR HANDLING RESULT CONSTANTS =====
        /// <summary>CreateFailure method name for result patterns</summary>
        public const string CREATE_FAILURE_METHOD = "CreateFailure";

        /// <summary>CreateSuccess method name for result patterns</summary>
        public const string CREATE_SUCCESS_METHOD = "CreateSuccess";

        /// <summary>Success result indicator</summary>
        public const string SUCCESS_RESULT = "SUCCESS";

        /// <summary>Failure result indicator</summary>
        public const string FAILURE_RESULT = "FAILURE";

        /// <summary>Available status indicator</summary>
        public const string AVAILABLE_STATUS = "AVAILABLE";

        /// <summary>Unavailable status indicator</summary>
        public const string UNAVAILABLE_STATUS = "UNAVAILABLE";

        // ===== CONSOLE AND UI CONSTANTS =====
        /// <summary>Console prefix for user messages</summary>
        public const string CONSOLE_MESSAGE_PREFIX = "[CONSOLE] ";

        /// <summary>Manual operation prefix</summary>
        public const string MANUAL_OPERATION_PREFIX = "[MANUAL] ";

        /// <summary>Patch operation prefix</summary>
        public const string PATCH_OPERATION_PREFIX = "[PATCH] ";

        /// <summary>Error operation prefix</summary>
        public const string ERROR_OPERATION_PREFIX = "[ERROR] ";

        /// <summary>Warning operation prefix</summary>
        public const string WARNING_OPERATION_PREFIX = "[WARNING] ";

        /// <summary>Info operation prefix</summary>
        public const string INFO_OPERATION_PREFIX = "[INFO] ";

        // ===== CONSOLE HELP AND MESSAGES =====
        /// <summary>Console examples header</summary>
        public const string CONSOLE_EXAMPLES_HEADER = "[CONSOLE] Examples:";

        /// <summary>Console required parameters header</summary>
        public const string CONSOLE_REQUIRED_HEADER = "[CONSOLE] Required:";

        /// <summary>Console optional parameters header</summary>
        public const string CONSOLE_OPTIONAL_HEADER = "[CONSOLE] Optional (auto-detected order):";

        /// <summary>Console parameters info message</summary>
        public const string CONSOLE_PARAMETERS_INFO = "[CONSOLE] Parameters can be in any order after count (auto-detected):";

        /// <summary>Console formatting note</summary>
        public const string CONSOLE_FORMATTING_NOTE = "[CONSOLE] Note: Message preserves all spaces and formatting";

        /// <summary>Console missing count parameter error</summary>
        public const string CONSOLE_MISSING_COUNT_ERROR = "[CONSOLE] Missing required parameter: count";

        /// <summary>Console missing message parameter error</summary>
        public const string CONSOLE_MISSING_MESSAGE_ERROR = "[CONSOLE] Missing required parameter: message";

        /// <summary>Console invalid iteration count error template</summary>
        public const string CONSOLE_INVALID_COUNT_ERROR = "[CONSOLE] Invalid iteration count '{0}'. Must be a positive integer.";

        /// <summary>Console delay parameter description</summary>
        public const string CONSOLE_DELAY_PARAM_DESC = "[CONSOLE]   delay_seconds - Delay between saves (number â‰¥ 0, default: 0)";

        /// <summary>Console count parameter description</summary>
        public const string CONSOLE_COUNT_PARAM_DESC = "[CONSOLE]   count - Number of save iterations (positive integer)";

        /// <summary>Console bypass cooldown parameter description</summary>
        public const string CONSOLE_BYPASS_PARAM_DESC = "[CONSOLE]   bypass_cooldown - Skip save cooldown (true/false, default: true)";

        // ===== LOGGING LEVEL STRINGS =====
        /// <summary>Message log level identifier</summary>
        public const string LOG_LEVEL_MSG = "msg";

        /// <summary>Warning log level identifier</summary>
        public const string LOG_LEVEL_WARN = "warn";

        /// <summary>Error log level identifier</summary>
        public const string LOG_LEVEL_ERR = "err";

        // ===== DATA TYPE CONSTANTS =====
        /// <summary>Boolean data type identifier</summary>
        public const string DATA_TYPE_BOOLEAN = "BOOLEAN";

        /// <summary>String data type identifier</summary>
        public const string DATA_TYPE_STRING = "STRING";

        /// <summary>Integer data type identifier</summary>
        public const string DATA_TYPE_INTEGER = "INTEGER";

        /// <summary>Float data type identifier</summary>
        public const string DATA_TYPE_FLOAT = "FLOAT";

        /// <summary>Double data type identifier</summary>
        public const string DATA_TYPE_DOUBLE = "DOUBLE";

        /// <summary>Object data type identifier</summary>
        public const string DATA_TYPE_OBJECT = "OBJECT";

        // ===== PREDICATE AND FUNCTION CONSTANTS =====
        /// <summary>Predicate parameter name</summary>
        public const string PREDICATE_PARAM_NAME = "predicate";

        /// <summary>Action parameter name</summary>
        public const string ACTION_PARAM_NAME = "action";

        /// <summary>Function parameter name</summary>
        public const string FUNCTION_PARAM_NAME = "function";

        /// <summary>Callback parameter name</summary>
        public const string CALLBACK_PARAM_NAME = "callback";

        /// <summary>Delegate parameter name</summary>
        public const string DELEGATE_PARAM_NAME = "delegate";

        // ===== SPECIAL IDENTIFIERS =====
        /// <summary>Pending justification identifier</summary>
        public const string PENDING_JUSTIFICATION = "<Pending>";

        /// <summary>SaveData identifier</summary>
        public const string SAVE_DATA_IDENTIFIER = "saveData";

        /// <summary>Paths identifier</summary>
        public const string PATHS_IDENTIFIER = "paths";

        /// <summary>Directories identifier</summary>
        public const string DIRECTORIES_IDENTIFIER = "directories";

        /// <summary>DetectDirs identifier</summary>
        public const string DETECT_DIRS_IDENTIFIER = "detectdirs";

        // ===== ADDITIONAL LOGGING MESSAGES =====
        /// <summary>Atomic write failure message template</summary>
        public const string ATOMIC_WRITE_FAILED_MSG = "Atomic write failed for {0}: {1}\nStack Trace: {2}";

        /// <summary>Atomic write success message template</summary>
        public const string ATOMIC_WRITE_SUCCESS_MSG = "Atomic write succeeded for {0}";

        /// <summary>File not found warning template</summary>
        public const string FILE_NOT_FOUND_WARNING = "File not found {0}";

        /// <summary>Operation succeeded message template</summary>
        public const string OPERATION_SUCCESS_MSG = "Operation succeeded for {0}";

        /// <summary>Operation failed message template</summary>
        public const string OPERATION_FAILED_MSG = "Operation failed for {0}: {1}\nStack Trace: {2}";

        /// <summary>ArgumentNullException message template</summary>
        public const string ARGUMENT_NULL_EXCEPTION_MSG = "ArgumentNullException for {0}: {1}\nStack Trace: {2}";

        /// <summary>IOException message template</summary>
        public const string IO_EXCEPTION_MSG = "IOException for {0}: {1}\nStack Trace: {2}";

        /// <summary>General exception message template</summary>
        public const string GENERAL_EXCEPTION_MSG = "Exception for {0}: {1}\nStack Trace: {2}";

        // ===== BACKUP OPERATION MESSAGES =====
        /// <summary>Backup succeeded message template</summary>
        public const string BACKUP_SUCCESS_MSG = "BackupAsync succeeded for {0} â†’ {1}";

        /// <summary>Backup failed message template</summary>
        public const string BACKUP_FAILED_MSG = "BackupAsync failed for {0}: {1}\nStack Trace: {2}";

        /// <summary>Backup incomplete read warning</summary>
        public const string BACKUP_INCOMPLETE_READ_WARNING = "BackupAsync: Incomplete read for {0}";

        /// <summary>Backup source not found warning</summary>
        public const string BACKUP_SOURCE_NOT_FOUND_WARNING = "BackupAsync: Source file not found {0}";

        /// <summary>Backup cleanup completed message</summary>
        public const string BACKUP_CLEANUP_COMPLETED_MSG = "Backup cleanup completed. Deleted {0} old directories.";

        /// <summary>Access denied backup delete warning</summary>
        public const string BACKUP_ACCESS_DENIED_WARNING = "Access denied while deleting backup directory: {0}";

        /// <summary>Backup directory not exist warning</summary>
        public const string BACKUP_DIRECTORY_NOT_EXIST_WARNING = "Backup root directory does not exist: {0}";

        /// <summary>Attempting backup message</summary>
        public const string ATTEMPTING_BACKUP_MSG = "Attempting backup of savegame directory!";

        /// <summary>Attempting delete backup message</summary>
        public const string ATTEMPTING_DELETE_BACKUP_MSG = "Attempting to delete backup directory: {0}";

        // ===== MIXER OPERATION MESSAGES =====
        /// <summary>Mixer attach listener started message</summary>
        public const string MIXER_ATTACH_STARTED_MSG = "AttachListenerWhenReady: Started for Mixer {0}";

        /// <summary>Mixer attach listener finished message</summary>
        public const string MIXER_ATTACH_FINISHED_MSG = "AttachListenerWhenReady: Finished for Mixer {0}";

        /// <summary>Mixer start threshold found message</summary>
        public const string MIXER_START_THRESHOLD_FOUND_MSG = "AttachListenerWhenReady: StartThrehold found for Mixer {0}";

        /// <summary>Mixer using polling method message</summary>
        public const string MIXER_USING_POLLING_MSG = "AttachListenerWhenReady: Using polling method for Mixer {0}";

        /// <summary>Mixer attach listener error message</summary>
        public const string MIXER_ATTACH_ERROR_MSG = "AttachListenerWhenReady error for Mixer {0}: {1}\nStackTrace: {2}";

        // ===== SAVE OPERATION MESSAGES =====
        /// <summary>Backup save folder started message</summary>
        public const string BACKUP_SAVE_FOLDER_STARTED_MSG = "BackupSaveFolder started for: {0}";

        /// <summary>Backup save folder in progress message</summary>
        public const string BACKUP_SAVE_FOLDER_IN_PROGRESS_MSG = "BackupSaveFolder: Already in progress, skipping duplicate call";

        /// <summary>Backup save folder completed message</summary>
        public const string BACKUP_SAVE_FOLDER_COMPLETED_MSG = "BackupSaveFolder: Backup operation completed successfully";

        /// <summary>Backup save folder null path message</summary>
        public const string BACKUP_SAVE_FOLDER_NULL_PATH_MSG = "BackupSaveFolder: CurrentSavePath is null/empty, cannot backup";

        /// <summary>Backup save folder error message</summary>
        public const string BACKUP_SAVE_FOLDER_ERROR_MSG = "BackupSaveFolder: Error in backup task: {0}\n{1}";

        /// <summary>Backup save folder finished message</summary>
        public const string BACKUP_SAVE_FOLDER_FINISHED_MSG = "BackupSaveFolder: Finished and cleanup completed";

        /// <summary>Backup save folder generic started message</summary>
        public const string BACKUP_SAVE_FOLDER_GENERIC_STARTED_MSG = "BackupSaveFolder: Started";

        /// <summary>Backup task failed message</summary>
        public const string BACKUP_TASK_FAILED_MSG = "Backup task failed: {0}";

        /// <summary>Backup root message</summary>
        public const string BACKUP_ROOT_MSG = "BACKUP ROOT: {0}";

        /// <summary>Starting backup cleanup message</summary>
        public const string STARTING_BACKUP_CLEANUP_MSG = "Starting backup cleanup process for: {0}";

        // ===== SYSTEM AND REFLECTION MESSAGES =====
        /// <summary>SaveManager type not found message</summary>
        public const string SAVE_MANAGER_TYPE_NOT_FOUND_MSG = "[PATCH] SaveManager type not found - patch will not be applied";

        // ===== DIRECTORY AND PATH CONSTANTS =====
        /// <summary>Unity Application.dataPath reference comment</summary>
        public const string UNITY_DATA_PATH_COMMENT = "Unity Application.dataPath points to \"Schedule I_Data\", parent is install dir";

        /// <summary>Schedule I_Data directory name</summary>
        public const string SCHEDULE_I_DATA_DIR = "Schedule I_Data";

        // ===== LOG FILE NAMES =====
        /// <summary>Latest.log filename</summary>
        public const string LATEST_LOG_FILENAME = "Latest.log";

        /// <summary>Console.log filename</summary>
        public const string CONSOLE_LOG_FILENAME = "Console.log";

        /// <summary>MelonLoader.log filename</summary>
        public const string MELONLOADER_LOG_FILENAME = "MelonLoader.log";

        // ===== EXTENDED NUMERIC CONSTANTS =====
        /// <summary>Zero float value</summary>
        public const float ZERO_FLOAT = 0.0f;

        /// <summary>One float value</summary>
        public const float ONE_FLOAT = 1.0f;

        /// <summary>Two float value</summary>
        public const float TWO_FLOAT = 2.0f;

        /// <summary>Three float value</summary>
        public const float THREE_FLOAT = 3.0f;

        /// <summary>Five float value</summary>
        public const float FIVE_FLOAT = 5.0f;

        /// <summary>Ten float value</summary>
        public const float TEN_FLOAT = 10.0f;

        /// <summary>Twenty float value</summary>
        public const float TWENTY_FLOAT = 20.0f;

        /// <summary>Thirty float value</summary>
        public const float THIRTY_FLOAT = 30.0f;

        /// <summary>Sixty float value</summary>
        public const float SIXTY_FLOAT = 60.0f;

        /// <summary>Hundred float value</summary>
        public const float HUNDRED_FLOAT = 100.0f;

        /// <summary>Thousand float value</summary>
        public const float THOUSAND_FLOAT = 1000.0f;

        /// <summary>Zero integer value</summary>
        public const int ZERO_INT = 0;

        /// <summary>One integer value</summary>
        public const int ONE_INT = 1;

        /// <summary>Two integer value</summary>
        public const int TWO_INT = 2;

        /// <summary>Three integer value</summary>
        public const int THREE_INT = 3;

        /// <summary>Five integer value</summary>
        public const int FIVE_INT = 5;

        /// <summary>Ten integer value</summary>
        public const int TEN_INT = 10;

        /// <summary>Twenty integer value</summary>
        public const int TWENTY_INT = 20;

        /// <summary>Thirty integer value</summary>
        public const int THIRTY_INT = 30;

        /// <summary>Fifty integer value</summary>
        public const int FIFTY_INT = 50;

        /// <summary>Hundred integer value</summary>
        public const int HUNDRED_INT = 100;

        /// <summary>Thousand integer value</summary>
        public const int THOUSAND_INT = 1000;

        // ===== EXTENDED WAIT TIME CONSTANTS =====
        /// <summary>Ultra short wait time (10ms)</summary>
        public const float ULTRA_SHORT_WAIT_SECONDS = 0.01f;

        /// <summary>Tiny wait time (50ms)</summary>
        public const float TINY_WAIT_SECONDS = 0.05f;

        /// <summary>Brief wait time (200ms)</summary>
        public const float BRIEF_WAIT_SECONDS = 0.2f;

        /// <summary>Quick wait time (300ms)</summary>
        public const float QUICK_WAIT_SECONDS = 0.3f;

        /// <summary>Moderate wait time (800ms)</summary>
        public const float MODERATE_WAIT_SECONDS = 0.8f;

        /// <summary>Extended long wait time (3 seconds)</summary>
        public const float EXTENDED_LONG_WAIT_SECONDS = 3.0f;

        /// <summary>Very long wait time (5 seconds)</summary>
        public const float VERY_LONG_WAIT_SECONDS = 5.0f;

        /// <summary>Ultra long wait time (10 seconds)</summary>
        public const float ULTRA_LONG_WAIT_SECONDS = 10.0f;

        // ===== ADDITIONAL PERFORMANCE CONSTANTS =====
        /// <summary>Memory pressure threshold in MB</summary>
        public const double MEMORY_PRESSURE_THRESHOLD_MB = 128.0;

        /// <summary>CPU usage threshold percentage</summary>
        public const double CPU_USAGE_THRESHOLD_PERCENT = 80.0;

        /// <summary>Disk usage threshold percentage</summary>
        public const double DISK_USAGE_THRESHOLD_PERCENT = 90.0;

        /// <summary>Network timeout threshold in seconds</summary>
        public const double NETWORK_TIMEOUT_THRESHOLD_SECONDS = 30.0;

        /// <summary>Database connection timeout in seconds</summary>
        public const double DATABASE_TIMEOUT_THRESHOLD_SECONDS = 15.0;

        /// <summary>Cache eviction threshold</summary>
        public const double CACHE_EVICTION_THRESHOLD = 0.8;

        // ===== CONFIGURATION AND SETTINGS CONSTANTS =====
        /// <summary>Default language setting</summary>
        public const string DEFAULT_LANGUAGE = "en-US";

        /// <summary>Default culture setting</summary>
        public const string DEFAULT_CULTURE = "en-US";

        /// <summary>Default encoding name</summary>
        public const string DEFAULT_ENCODING = "UTF-8";

        /// <summary>Application configuration file name</summary>
        public const string APP_CONFIG_FILENAME = "app.config";

        /// <summary>Settings configuration file name</summary>
        public const string SETTINGS_CONFIG_FILENAME = "settings.json";

        /// <summary>User preferences file name</summary>
        public const string USER_PREFERENCES_FILENAME = "preferences.json";

        /// <summary>Cache directory name</summary>
        public const string CACHE_DIRECTORY_NAME = "Cache";

        /// <summary>Logs directory name</summary>
        public const string LOGS_DIRECTORY_NAME = "Logs";

        /// <summary>Config directory name</summary>
        public const string CONFIG_DIRECTORY_NAME = "Config";

        /// <summary>Data directory name</summary>
        public const string DATA_DIRECTORY_NAME = "Data";

        /// <summary>Temp directory name</summary>
        public const string TEMP_DIRECTORY_NAME = "Temp";

        /// <summary>Backup directory name</summary>
        public const string BACKUP_DIRECTORY_NAME = "Backup";

        // ===== REGISTRY AND SYSTEM CONSTANTS =====
        /// <summary>Windows registry HKEY_CURRENT_USER</summary>
        public const string REGISTRY_HKEY_CURRENT_USER = "HKEY_CURRENT_USER";

        /// <summary>Windows registry HKEY_LOCAL_MACHINE</summary>
        public const string REGISTRY_HKEY_LOCAL_MACHINE = "HKEY_LOCAL_MACHINE";

        /// <summary>System environment variable PATH</summary>
        public const string ENVIRONMENT_PATH = "PATH";

        /// <summary>System environment variable TEMP</summary>
        public const string ENVIRONMENT_TEMP = "TEMP";

        /// <summary>System environment variable USER</summary>
        public const string ENVIRONMENT_USER = "USER";

        /// <summary>System environment variable HOME</summary>
        public const string ENVIRONMENT_HOME = "HOME";

        // ===== THREAD AND SYNCHRONIZATION CONSTANTS =====
        /// <summary>Thread sleep minimum interval</summary>
        public const int THREAD_SLEEP_MIN_MS = 1;

        /// <summary>Thread sleep maximum interval</summary>
        public const int THREAD_SLEEP_MAX_MS = 10000;

        /// <summary>Synchronization context timeout</summary>
        public const int SYNC_CONTEXT_TIMEOUT_MS = 3000;

        /// <summary>Task scheduler timeout</summary>
        public const int TASK_SCHEDULER_TIMEOUT_MS = 5000;

        /// <summary>Thread pool minimum threads</summary>
        public const int THREAD_POOL_MIN_THREADS = 2;

        /// <summary>Thread pool maximum threads</summary>
        public const int THREAD_POOL_MAX_THREADS = 50;

        // ===== HTTP AND NETWORK CONSTANTS =====
        /// <summary>HTTP status OK code</summary>
        public const int HTTP_STATUS_OK = 200;

        /// <summary>HTTP status Not Found code</summary>
        public const int HTTP_STATUS_NOT_FOUND = 404;

        /// <summary>HTTP status Internal Server Error code</summary>
        public const int HTTP_STATUS_INTERNAL_ERROR = 500;

        /// <summary>Default HTTP port</summary>
        public const int HTTP_DEFAULT_PORT = 80;

        /// <summary>Default HTTPS port</summary>
        public const int HTTPS_DEFAULT_PORT = 443;

        /// <summary>localhost address</summary>
        public const string LOCALHOST_ADDRESS = "127.0.0.1";

        /// <summary>localhost hostname</summary>
        public const string LOCALHOST_HOSTNAME = "localhost";

        // ===== DATABASE AND STORAGE CONSTANTS =====
        /// <summary>Default database page size</summary>
        public const int DATABASE_DEFAULT_PAGE_SIZE = 4096;

        /// <summary>Database connection pool size</summary>
        public const int DATABASE_POOL_SIZE = 10;

        /// <summary>SQL SELECT statement</summary>
        public const string SQL_SELECT = "SELECT";

        /// <summary>SQL INSERT statement</summary>
        public const string SQL_INSERT = "INSERT";

        /// <summary>SQL UPDATE statement</summary>
        public const string SQL_UPDATE = "UPDATE";

        /// <summary>SQL DELETE statement</summary>
        public const string SQL_DELETE = "DELETE";

        /// <summary>SQL WHERE clause</summary>
        public const string SQL_WHERE = "WHERE";

        /// <summary>SQL FROM clause</summary>
        public const string SQL_FROM = "FROM";

        // ===== ENCRYPTION AND SECURITY CONSTANTS =====
        /// <summary>AES encryption algorithm name</summary>
        public const string ENCRYPTION_AES = "AES";

        /// <summary>RSA encryption algorithm name</summary>
        public const string ENCRYPTION_RSA = "RSA";

        /// <summary>SHA256 hash algorithm name</summary>
        public const string HASH_SHA256 = "SHA256";

        /// <summary>MD5 hash algorithm name</summary>
        public const string HASH_MD5 = "MD5";

        /// <summary>Default encryption key size</summary>
        public const int ENCRYPTION_KEY_SIZE = 256;

        /// <summary>Default salt size for hashing</summary>
        public const int SALT_SIZE = 16;

        // ===== REGEX AND PATTERN CONSTANTS =====
        /// <summary>Email validation regex pattern</summary>
        public const string EMAIL_REGEX_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        /// <summary>Phone number regex pattern</summary>
        public const string PHONE_REGEX_PATTERN = @"^\+?[\d\s\-\(\)]{10,}$";

        /// <summary>URL validation regex pattern</summary>
        public const string URL_REGEX_PATTERN = @"^https?://[^\s/$.?#].[^\s]*$";

        /// <summary>Version number regex pattern</summary>
        public const string VERSION_REGEX_PATTERN = @"^\d+\.\d+\.\d+(\.\d+)?$";

        /// <summary>GUID regex pattern</summary>
        public const string GUID_REGEX_PATTERN = @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";

        // ===== VALIDATION RULES CONSTANTS =====
        /// <summary>Minimum password length</summary>
        public const int MIN_PASSWORD_LENGTH = 8;

        /// <summary>Maximum password length</summary>
        public const int MAX_PASSWORD_LENGTH = 128;

        /// <summary>Minimum username length</summary>
        public const int MIN_USERNAME_LENGTH = 3;

        /// <summary>Maximum username length</summary>
        public const int MAX_USERNAME_LENGTH = 50;

        /// <summary>Maximum email length</summary>
        public const int MAX_EMAIL_LENGTH = 254;

        /// <summary>Maximum file name length</summary>
        public const int MAX_FILENAME_LENGTH = 255;

        /// <summary>Maximum path length</summary>
        public const int MAX_PATH_LENGTH = 260;

        // ===== FORMATTING AND DISPLAY CONSTANTS =====
        /// <summary>Currency format string</summary>
        public const string CURRENCY_FORMAT = "C";

        /// <summary>Percentage format string</summary>
        public const string PERCENTAGE_FORMAT = "P";

        /// <summary>Number format with 2 decimals</summary>
        public const string NUMBER_FORMAT_2_DECIMALS = "F2";

        /// <summary>Number format with 3 decimals</summary>
        public const string NUMBER_FORMAT_3_DECIMALS = "F3";

        /// <summary>Date format short</summary>
        public const string DATE_FORMAT_SHORT = "MM/dd/yyyy";

        /// <summary>Date format long</summary>
        public const string DATE_FORMAT_LONG = "MMMM dd, yyyy";

        /// <summary>Time format 12 hour</summary>
        public const string TIME_FORMAT_12_HOUR = "hh:mm:ss tt";

        /// <summary>Time format 24 hour</summary>
        public const string TIME_FORMAT_24_HOUR = "HH:mm:ss";

        // ===== UNITY SPECIFIC CONSTANTS =====
        /// <summary>Unity GameObject tag for Player</summary>
        public const string UNITY_TAG_PLAYER = "Player";

        /// <summary>Unity GameObject tag for Enemy</summary>
        public const string UNITY_TAG_ENEMY = "Enemy";

        /// <summary>Unity GameObject tag for MainCamera</summary>
        public const string UNITY_TAG_MAIN_CAMERA = "MainCamera";

        /// <summary>Unity GameObject tag for UI</summary>
        public const string UNITY_TAG_UI = "UI";

        /// <summary>Unity layer name Default</summary>
        public const string UNITY_LAYER_DEFAULT = "Default";

        /// <summary>Unity layer name UI</summary>
        public const string UNITY_LAYER_UI = "UI";

        /// <summary>Unity Input axis Horizontal</summary>
        public const string UNITY_INPUT_HORIZONTAL = "Horizontal";

        /// <summary>Unity Input axis Vertical</summary>
        public const string UNITY_INPUT_VERTICAL = "Vertical";

        /// <summary>Unity Input Mouse X</summary>
        public const string UNITY_INPUT_MOUSE_X = "Mouse X";

        /// <summary>Unity Input Mouse Y</summary>
        public const string UNITY_INPUT_MOUSE_Y = "Mouse Y";

        // ===== IL2CPP AND MELONLOADER CONSTANTS =====
        /// <summary>IL2CPP domain name</summary>
        public const string IL2CPP_DOMAIN = "IL2CPP";

        /// <summary>MelonLoader mod directory</summary>
        public const string MELONLOADER_MODS_DIR = "Mods";

        /// <summary>MelonLoader plugins directory</summary>
        public const string MELONLOADER_PLUGINS_DIR = "Plugins";

        /// <summary>MelonLoader libs directory</summary>
        public const string MELONLOADER_LIBS_DIR = "MelonLoader";

        /// <summary>MelonLoader user libs directory</summary>
        public const string MELONLOADER_USER_LIBS_DIR = "UserLibs";

        /// <summary>MelonLoader dependencies directory</summary>
        public const string MELONLOADER_DEPENDENCIES_DIR = "Dependencies";

        // ===== BUILD AND COMPILATION CONSTANTS =====
        /// <summary>Debug build configuration</summary>
        public const string BUILD_CONFIG_DEBUG = "Debug";

        /// <summary>Release build configuration</summary>
        public const string BUILD_CONFIG_RELEASE = "Release";

        /// <summary>AnyCPU target platform</summary>
        public const string TARGET_PLATFORM_ANY_CPU = "AnyCPU";

        /// <summary>x86 target platform</summary>
        public const string TARGET_PLATFORM_X86 = "x86";

        /// <summary>x64 target platform</summary>
        public const string TARGET_PLATFORM_X64 = "x64";

        /// <summary>.NET Framework target</summary>
        public const string TARGET_FRAMEWORK_NET48 = ".NETFramework,Version=v4.8.1";

        /// <summary>C# language version</summary>
        public const string CSHARP_LANGUAGE_VERSION = "7.3";

        // ===== PERFORMANCE OPTIMIZATION CONSTANTS =====
        /// <summary>High memory usage threshold in MB (512MB)</summary>
        public const double HIGH_MEMORY_THRESHOLD_MB = 512.0;

        /// <summary>High thread count threshold (50 threads)</summary>
        public const int HIGH_THREAD_COUNT_THRESHOLD = 50;

        /// <summary>Maximum performance warnings before cleanup (20 warnings)</summary>
        public const int MAX_PERFORMANCE_WARNINGS = 20;

        /// <summary>Performance warnings cleanup batch size (10 warnings)</summary>
        public const int PERFORMANCE_WARNINGS_CLEANUP_BATCH = 10;

        /// <summary>Memory freed threshold for logging in MB (1MB)</summary>
        public const double MEMORY_FREED_LOG_THRESHOLD_MB = 1.0;

        /// <summary>Optimization interval in seconds (minimum time between optimizations)</summary>
        public const int OPTIMIZATION_INTERVAL_SECONDS = 30;

        /// <summary>Performance metrics history retention count</summary>
        public const int PERFORMANCE_METRICS_HISTORY_COUNT = 100;

        /// <summary>Performance summary log interval (every N cycles)</summary>
        public const int PERFORMANCE_SUMMARY_LOG_INTERVAL = 50;

        // ===== ASSEMBLY INFO CONSTANTS =====
        /// <summary>Assembly configuration empty value</summary>
        public const string ASSEMBLY_CONFIGURATION_EMPTY = "";

        /// <summary>Assembly company empty value</summary>
        public const string ASSEMBLY_COMPANY_EMPTY = "";

        /// <summary>Assembly copyright notice</summary>
        public const string ASSEMBLY_COPYRIGHT = "Copyright Â©  2025 mooleshacat";

        /// <summary>Assembly trademark empty value</summary>
        public const string ASSEMBLY_TRADEMARK_EMPTY = "";

        /// <summary>Assembly culture empty value</summary>
        public const string ASSEMBLY_CULTURE_EMPTY = "";

        /// <summary>Assembly version format pattern</summary>
        public const string ASSEMBLY_VERSION_FORMAT = "1.0.0.0";

        /// <summary>Assembly file version format pattern</summary>
        public const string ASSEMBLY_FILE_VERSION_FORMAT = "1.0.0.0";

        // ===== NUMERIC VALIDATION CONSTANTS =====
        /// <summary>Zero integer value for comparisons</summary>
        public const int ZERO_INT = 0;

        /// <summary>One integer value for comparisons</summary>
        public const int ONE_INT = 1;

        /// <summary>Two integer value for comparisons</summary>
        public const int TWO_INT = 2;

        /// <summary>Three integer value for comparisons</summary>
        public const int THREE_INT = 3;

        /// <summary>Five integer value for comparisons</summary>
        public const int FIVE_INT = 5;

        /// <summary>Ten integer value for comparisons</summary>
        public const int TEN_INT = 10;

        /// <summary>Twenty integer value for comparisons</summary>
        public const int TWENTY_INT = 20;

        /// <summary>Fifty integer value for comparisons</summary>
        public const int FIFTY_INT = 50;

        /// <summary>One hundred integer value for comparisons</summary>
        public const int ONE_HUNDRED_INT = 100;

        /// <summary>One thousand integer value for comparisons</summary>
        public const int ONE_THOUSAND_INT = 1000;

        /// <summary>Zero float value for comparisons</summary>
        public const float ZERO_FLOAT = 0.0f;

        /// <summary>One float value for comparisons</summary>
        public const float ONE_FLOAT = 1.0f;

        /// <summary>Two float value for comparisons</summary>
        public const float TWO_FLOAT = 2.0f;

        /// <summary>Half float value for comparisons (0.5)</summary>
        public const float HALF_FLOAT = 0.5f;

        /// <summary>Quarter float value for comparisons (0.25)</summary>
        public const float QUARTER_FLOAT = 0.25f;

        /// <summary>Three quarters float value for comparisons (0.75)</summary>
        public const float THREE_QUARTERS_FLOAT = 0.75f;

        /// <summary>Zero double value for comparisons</summary>
        public const double ZERO_DOUBLE = 0.0;

        /// <summary>One double value for comparisons</summary>
        public const double ONE_DOUBLE = 1.0;

        /// <summary>Two double value for comparisons</summary>
        public const double TWO_DOUBLE = 2.0;

        /// <summary>Half double value for comparisons (0.5)</summary>
        public const double HALF_DOUBLE = 0.5;

        /// <summary>Zero long value for comparisons</summary>
        public const long ZERO_LONG = 0L;

        /// <summary>One long value for comparisons</summary>
        public const long ONE_LONG = 1L;

        /// <summary>Two long value for comparisons</summary>
        public const long TWO_LONG = 2L;

        // ===== COLLECTION SIZE CONSTANTS =====
        /// <summary>Empty collection count</summary>
        public const int EMPTY_COLLECTION_COUNT = 0;

        /// <summary>Single item collection count</summary>
        public const int SINGLE_ITEM_COUNT = 1;

        /// <summary>Small collection default size</summary>
        public const int SMALL_COLLECTION_SIZE = 10;

        /// <summary>Medium collection default size</summary>
        public const int MEDIUM_COLLECTION_SIZE = 50;

        /// <summary>Large collection default size</summary>
        public const int LARGE_COLLECTION_SIZE = 100;

        /// <summary>Extra large collection default size</summary>
        public const int EXTRA_LARGE_COLLECTION_SIZE = 500;

        /// <summary>Maximum reasonable collection size</summary>
        public const int MAX_REASONABLE_COLLECTION_SIZE = 1000;

        // ===== ERROR HANDLING CONSTANTS =====
        /// <summary>Default retry attempt count</summary>
        public const int DEFAULT_RETRY_ATTEMPTS = 3;

        /// <summary>Maximum retry attempt count</summary>
        public const int MAX_RETRY_ATTEMPTS = 5;

        /// <summary>Retry delay base in milliseconds</summary>
        public const int RETRY_DELAY_BASE_MS = 100;

        /// <summary>Retry delay multiplier for exponential backoff</summary>
        public const int RETRY_DELAY_MULTIPLIER = 2;

        /// <summary>Maximum retry delay in milliseconds</summary>
        public const int MAX_RETRY_DELAY_MS = 5000;

        /// <summary>Critical error escalation threshold</summary>
        public const int CRITICAL_ERROR_THRESHOLD = 10;

        /// <summary>Error recovery timeout in milliseconds</summary>
        public const int ERROR_RECOVERY_TIMEOUT_MS = 10000;

        // ===== MEMORY AND RESOURCE CONSTANTS =====
        /// <summary>Bytes per kilobyte</summary>
        public const int BYTES_PER_KB = 1024;

        /// <summary>Bytes per megabyte</summary>
        public const int BYTES_PER_MB = 1048576; // 1024 * 1024

        /// <summary>Bytes per gigabyte</summary>
        public const long BYTES_PER_GB = 1073741824L; // 1024 * 1024 * 1024

        /// <summary>Memory threshold in bytes for optimization</summary>
        public const long MEMORY_THRESHOLD_BYTES = BYTES_PER_MB;

        /// <summary>Small memory allocation size in bytes</summary>
        public const int SMALL_MEMORY_ALLOCATION_BYTES = 1024;

        /// <summary>Medium memory allocation size in bytes</summary>
        public const int MEDIUM_MEMORY_ALLOCATION_BYTES = 65536; // 64KB

        /// <summary>Large memory allocation size in bytes</summary>
        public const int LARGE_MEMORY_ALLOCATION_BYTES = BYTES_PER_MB;

        /// <summary>Memory cleanup threshold percentage</summary>
        public const double MEMORY_CLEANUP_THRESHOLD_PERCENT = 75.0;

        /// <summary>Memory warning threshold percentage</summary>
        public const double MEMORY_WARNING_THRESHOLD_PERCENT = 85.0;

        /// <summary>Memory critical threshold percentage</summary>
        public const double MEMORY_CRITICAL_THRESHOLD_PERCENT = 95.0;

        // ===== THREADING CONSTANTS =====
        /// <summary>Default thread priority</summary>
        public const string DEFAULT_THREAD_PRIORITY = "Normal";

        /// <summary>Background thread priority</summary>
        public const string BACKGROUND_THREAD_PRIORITY = "BelowNormal";

        /// <summary>High priority thread priority</summary>
        public const string HIGH_THREAD_PRIORITY = "AboveNormal";

        /// <summary>Critical thread priority</summary>
        public const string CRITICAL_THREAD_PRIORITY = "Highest";

        /// <summary>Minimum thread pool size</summary>
        public const int MIN_THREAD_POOL_SIZE = 2;

        /// <summary>Default thread pool size</summary>
        public const int DEFAULT_THREAD_POOL_SIZE = 4;

        /// <summary>Maximum thread pool size</summary>
        public const int MAX_THREAD_POOL_SIZE = 16;

        /// <summary>Thread monitoring interval in milliseconds</summary>
        public const int THREAD_MONITORING_INTERVAL_MS = 5000;

        /// <summary>Thread cleanup timeout in milliseconds</summary>
        public const int THREAD_CLEANUP_TIMEOUT_MS = 30000;

        /// <summary>Thread join timeout in milliseconds</summary>
        public const int THREAD_JOIN_TIMEOUT_MS = 10000;

        // ===== GAME SPECIFIC CONSTANTS =====
        /// <summary>Game minimum frame rate target</summary>
        public const int GAME_MIN_FRAME_RATE = 30;

        /// <summary>Game target frame rate</summary>
        public const int GAME_TARGET_FRAME_RATE = 60;

        /// <summary>Game maximum frame rate</summary>
        public const int GAME_MAX_FRAME_RATE = 120;

        /// <summary>Game low performance threshold FPS</summary>
        public const int GAME_LOW_PERFORMANCE_FPS = 20;

        /// <summary>Game vsync enabled default</summary>
        public const bool GAME_VSYNC_ENABLED_DEFAULT = true;

        /// <summary>Game quality level minimum</summary>
        public const int GAME_QUALITY_LEVEL_MIN = 0;

        /// <summary>Game quality level maximum</summary>
        public const int GAME_QUALITY_LEVEL_MAX = 5;

        /// <summary>Game quality level default</summary>
        public const int GAME_QUALITY_LEVEL_DEFAULT = 3;

        // ===== STRING FORMATTING CONSTANTS =====
        /// <summary>String format with one argument</summary>
        public const string STRING_FORMAT_ONE_ARG = "{0}";

        /// <summary>String format with two arguments</summary>
        public const string STRING_FORMAT_TWO_ARGS = "{0} {1}";

        /// <summary>String format with three arguments</summary>
        public const string STRING_FORMAT_THREE_ARGS = "{0} {1} {2}";

        /// <summary>String format with four arguments</summary>
        public const string STRING_FORMAT_FOUR_ARGS = "{0} {1} {2} {3}";

        /// <summary>String format with five arguments</summary>
        public const string STRING_FORMAT_FIVE_ARGS = "{0} {1} {2} {3} {4}";

        /// <summary>Decimal format with one decimal place</summary>
        public const string DECIMAL_FORMAT_ONE_PLACE = "F1";

        /// <summary>Decimal format with two decimal places</summary>
        public const string DECIMAL_FORMAT_TWO_PLACES = "F2";

        /// <summary>Decimal format with three decimal places</summary>
        public const string DECIMAL_FORMAT_THREE_PLACES = "F3";

        /// <summary>Integer format with no decimal places</summary>
        public const string INTEGER_FORMAT_NO_DECIMALS = "F0";

        /// <summary>Percentage format</summary>
        public const string PERCENTAGE_FORMAT = "P0";

        /// <summary>Percentage format with one decimal</summary>
        public const string PERCENTAGE_FORMAT_ONE_DECIMAL = "P1";

        // ===== VALIDATION CONSTANTS =====
        /// <summary>Minimum valid string length</summary>
        public const int MIN_VALID_STRING_LENGTH = 1;

        /// <summary>Maximum reasonable string length</summary>
        public const int MAX_REASONABLE_STRING_LENGTH = 1000;

        /// <summary>Maximum safe string length</summary>
        public const int MAX_SAFE_STRING_LENGTH = 10000;

        /// <summary>Minimum valid array length</summary>
        public const int MIN_VALID_ARRAY_LENGTH = 1;

        /// <summary>Maximum reasonable array length</summary>
        public const int MAX_REASONABLE_ARRAY_LENGTH = 1000;

        /// <summary>Minimum valid numeric value</summary>
        public const double MIN_VALID_NUMERIC_VALUE = 0.0;

        /// <summary>Maximum safe numeric value</summary>
        public const double MAX_SAFE_NUMERIC_VALUE = double.MaxValue;

        // ===== DIAGNOSTIC CONSTANTS =====
        /// <summary>Diagnostic check interval in seconds</summary>
        public const int DIAGNOSTIC_CHECK_INTERVAL_SECONDS = 60;

        /// <summary>Diagnostic log retention days</summary>
        public const int DIAGNOSTIC_LOG_RETENTION_DAYS = 7;

        /// <summary>Diagnostic metric collection interval in milliseconds</summary>
        public const int DIAGNOSTIC_METRIC_INTERVAL_MS = 1000;

        /// <summary>Diagnostic warning threshold count</summary>
        public const int DIAGNOSTIC_WARNING_THRESHOLD = 10;

        /// <summary>Diagnostic error threshold count</summary>
        public const int DIAGNOSTIC_ERROR_THRESHOLD = 5;

        /// <summary>Diagnostic critical threshold count</summary>
        public const int DIAGNOSTIC_CRITICAL_THRESHOLD = 3;

        // ===== CONFIGURATION CONSTANTS =====
        /// <summary>Configuration file check interval in minutes</summary>
        public const int CONFIG_FILE_CHECK_INTERVAL_MINUTES = 5;

        /// <summary>Configuration backup retention count</summary>
        public const int CONFIG_BACKUP_RETENTION_COUNT = 10;

        /// <summary>Configuration validation timeout in milliseconds</summary>
        public const int CONFIG_VALIDATION_TIMEOUT_MS = 5000;

        /// <summary>Configuration auto-save interval in minutes</summary>
        public const int CONFIG_AUTO_SAVE_INTERVAL_MINUTES = 2;

        /// <summary>Configuration reload delay in milliseconds</summary>
        public const int CONFIG_RELOAD_DELAY_MS = 500;

        // ===== INITIALIZATION CONSTANTS =====
        /// <summary>Initialization timeout in seconds</summary>
        public const int INITIALIZATION_TIMEOUT_SECONDS = 30;

        /// <summary>Initialization retry delay in milliseconds</summary>
        public const int INITIALIZATION_RETRY_DELAY_MS = 1000;

        /// <summary>Initialization max retry attempts</summary>
        public const int INITIALIZATION_MAX_RETRIES = 5;

        /// <summary>Initialization validation timeout in milliseconds</summary>
        public const int INITIALIZATION_VALIDATION_TIMEOUT_MS = 10000;

        /// <summary>Initialization cleanup timeout in milliseconds</summary>
        public const int INITIALIZATION_CLEANUP_TIMEOUT_MS = 15000;

        // ===== CLEANUP AND SHUTDOWN CONSTANTS =====
        /// <summary>Graceful shutdown timeout in seconds</summary>
        public const int GRACEFUL_SHUTDOWN_TIMEOUT_SECONDS = 30;

        /// <summary>Force shutdown timeout in seconds</summary>
        public const int FORCE_SHUTDOWN_TIMEOUT_SECONDS = 10;

        /// <summary>Resource cleanup timeout in milliseconds</summary>
        public const int RESOURCE_CLEANUP_TIMEOUT_MS = 5000;

        /// <summary>Emergency cleanup timeout in milliseconds</summary>
        public const int EMERGENCY_CLEANUP_TIMEOUT_MS = 2000;

        /// <summary>Final cleanup attempts</summary>
        public const int FINAL_CLEANUP_ATTEMPTS = 3;

        // ===== MONITORING CONSTANTS =====
        /// <summary>Health check interval in seconds</summary>
        public const int HEALTH_CHECK_INTERVAL_SECONDS = 30;

        /// <summary>Status report interval in minutes</summary>
        public const int STATUS_REPORT_INTERVAL_MINUTES = 10;

        /// <summary>Watchdog timeout in seconds</summary>
        public const int WATCHDOG_TIMEOUT_SECONDS = 60;

        /// <summary>Heartbeat interval in seconds</summary>
        public const int HEARTBEAT_INTERVAL_SECONDS = 5;

        /// <summary>Monitoring data retention hours</summary>
        public const int MONITORING_DATA_RETENTION_HOURS = 24;

        // ===== NETWORK AND COMMUNICATION CONSTANTS =====
        /// <summary>Network connection timeout in milliseconds</summary>
        public const int NETWORK_CONNECTION_TIMEOUT_MS = 10000;

        /// <summary>Network read timeout in milliseconds</summary>
        public const int NETWORK_READ_TIMEOUT_MS = 5000;

        /// <summary>Network write timeout in milliseconds</summary>
        public const int NETWORK_WRITE_TIMEOUT_MS = 5000;

        /// <summary>Network retry delay in milliseconds</summary>
        public const int NETWORK_RETRY_DELAY_MS = 1000;

        /// <summary>Network max retry attempts</summary>
        public const int NETWORK_MAX_RETRY_ATTEMPTS = 3;

        /// <summary>Network buffer size in bytes</summary>
        public const int NETWORK_BUFFER_SIZE_BYTES = 8192;

        // ===== SECURITY CONSTANTS =====
        /// <summary>Security validation timeout in milliseconds</summary>
        public const int SECURITY_VALIDATION_TIMEOUT_MS = 3000;

        /// <summary>Security token expiry in minutes</summary>
        public const int SECURITY_TOKEN_EXPIRY_MINUTES = 60;

        /// <summary>Security hash salt length</summary>
        public const int SECURITY_HASH_SALT_LENGTH = 32;

        /// <summary>Security max failed attempts</summary>
        public const int SECURITY_MAX_FAILED_ATTEMPTS = 5;

        /// <summary>Security lockout duration in minutes</summary>
        public const int SECURITY_LOCKOUT_DURATION_MINUTES = 15;

        // ===== QUALITY ASSURANCE CONSTANTS =====
        /// <summary>Quality check interval in operations</summary>
        public const int QUALITY_CHECK_INTERVAL_OPERATIONS = 100;

        /// <summary>Quality threshold percentage</summary>
        public const double QUALITY_THRESHOLD_PERCENT = 95.0;

        /// <summary>Quality warning threshold percentage</summary>
        public const double QUALITY_WARNING_THRESHOLD_PERCENT = 90.0;

        /// <summary>Quality critical threshold percentage</summary>
        public const double QUALITY_CRITICAL_THRESHOLD_PERCENT = 80.0;

        /// <summary>Quality metrics history retention count</summary>
        public const int QUALITY_METRICS_HISTORY_COUNT = 1000;

        // ===== MIXER SPECIFIC OPERATIONAL CONSTANTS =====
        /// <summary>Mixer component activation delay in milliseconds</summary>
        public const int MIXER_ACTIVATION_DELAY_MS = 100;

        /// <summary>Mixer component deactivation delay in milliseconds</summary>
        public const int MIXER_DEACTIVATION_DELAY_MS = 50;

        /// <summary>Mixer threshold validation attempts</summary>
        public const int MIXER_THRESHOLD_VALIDATION_ATTEMPTS = 3;

        /// <summary>Mixer data sync interval in seconds</summary>
        public const int MIXER_DATA_SYNC_INTERVAL_SECONDS = 30;

        /// <summary>Mixer component scan interval in seconds</summary>
        public const int MIXER_COMPONENT_SCAN_INTERVAL_SECONDS = 10;

        /// <summary>Mixer batch processing size</summary>
        public const int MIXER_BATCH_PROCESSING_SIZE = 50;

        /// <summary>Mixer priority queue capacity</summary>
        public const int MIXER_PRIORITY_QUEUE_CAPACITY = 100;

        /// <summary>Mixer threshold change sensitivity</summary>
        public const float MIXER_THRESHOLD_CHANGE_SENSITIVITY = 0.01f;

        /// <summary>Mixer auto-save threshold changes</summary>
        public const int MIXER_AUTO_SAVE_THRESHOLD_CHANGES = 5;

        /// <summary>Mixer configuration validation timeout in milliseconds</summary>
        public const int MIXER_CONFIG_VALIDATION_TIMEOUT_MS = 2000;

        // ===== GAME STATE CONSTANTS =====
        /// <summary>Game state check interval in seconds</summary>
        public const int GAME_STATE_CHECK_INTERVAL_SECONDS = 1;

        /// <summary>Game state transition timeout in seconds</summary>
        public const int GAME_STATE_TRANSITION_TIMEOUT_SECONDS = 10;

        /// <summary>Game state persistence interval in minutes</summary>
        public const int GAME_STATE_PERSISTENCE_INTERVAL_MINUTES = 1;

        /// <summary>Game state backup interval in minutes</summary>
        public const int GAME_STATE_BACKUP_INTERVAL_MINUTES = 5;

        /// <summary>Game state validation attempts</summary>
        public const int GAME_STATE_VALIDATION_ATTEMPTS = 3;

        /// <summary>Game state recovery timeout in seconds</summary>
        public const int GAME_STATE_RECOVERY_TIMEOUT_SECONDS = 30;

        /// <summary>Game state emergency save threshold</summary>
        public const int GAME_STATE_EMERGENCY_SAVE_THRESHOLD = 10;

        // ===== USER INTERFACE CONSTANTS =====
        /// <summary>UI update interval in milliseconds</summary>
        public const int UI_UPDATE_INTERVAL_MS = 16; // ~60 FPS

        /// <summary>UI animation duration in milliseconds</summary>
        public const int UI_ANIMATION_DURATION_MS = 300;

        /// <summary>UI element fade duration in milliseconds</summary>
        public const int UI_ELEMENT_FADE_DURATION_MS = 150;

        /// <summary>UI tooltip display delay in milliseconds</summary>
        public const int UI_TOOLTIP_DISPLAY_DELAY_MS = 500;

        /// <summary>UI notification display duration in seconds</summary>
        public const int UI_NOTIFICATION_DISPLAY_DURATION_SECONDS = 3;

        /// <summary>UI popup timeout in seconds</summary>
        public const int UI_POPUP_TIMEOUT_SECONDS = 10;

        /// <summary>UI element minimum width in pixels</summary>
        public const int UI_ELEMENT_MIN_WIDTH_PIXELS = 50;

        /// <summary>UI element minimum height in pixels</summary>
        public const int UI_ELEMENT_MIN_HEIGHT_PIXELS = 20;

        /// <summary>UI panel default width in pixels</summary>
        public const int UI_PANEL_DEFAULT_WIDTH_PIXELS = 300;

        /// <summary>UI panel default height in pixels</summary>
        public const int UI_PANEL_DEFAULT_HEIGHT_PIXELS = 200;

        // ===== AUDIO CONSTANTS =====
        /// <summary>Audio volume minimum (0.0)</summary>
        public const float AUDIO_VOLUME_MIN = 0.0f;

        /// <summary>Audio volume maximum (1.0)</summary>
        public const float AUDIO_VOLUME_MAX = 1.0f;

        /// <summary>Audio volume default (0.7)</summary>
        public const float AUDIO_VOLUME_DEFAULT = 0.7f;

        /// <summary>Audio fade duration in seconds</summary>
        public const float AUDIO_FADE_DURATION_SECONDS = 1.0f;

        /// <summary>Audio sample rate standard (44100 Hz)</summary>
        public const int AUDIO_SAMPLE_RATE_STANDARD = 44100;

        /// <summary>Audio buffer size default</summary>
        public const int AUDIO_BUFFER_SIZE_DEFAULT = 1024;

        /// <summary>Audio channel count mono</summary>
        public const int AUDIO_CHANNELS_MONO = 1;

        /// <summary>Audio channel count stereo</summary>
        public const int AUDIO_CHANNELS_STEREO = 2;

        /// <summary>Audio bit depth standard (16 bits)</summary>
        public const int AUDIO_BIT_DEPTH_STANDARD = 16;

        /// <summary>Audio processing interval in milliseconds</summary>
        public const int AUDIO_PROCESSING_INTERVAL_MS = 10;

        // ===== GRAPHICS CONSTANTS =====
        /// <summary>Graphics resolution minimum width</summary>
        public const int GRAPHICS_MIN_WIDTH = 640;

        /// <summary>Graphics resolution minimum height</summary>
        public const int GRAPHICS_MIN_HEIGHT = 480;

        /// <summary>Graphics resolution default width</summary>
        public const int GRAPHICS_DEFAULT_WIDTH = 1920;

        /// <summary>Graphics resolution default height</summary>
        public const int GRAPHICS_DEFAULT_HEIGHT = 1080;

        /// <summary>Graphics maximum width</summary>
        public const int GRAPHICS_MAX_WIDTH = 3840;

        /// <summary>Graphics maximum height</summary>
        public const int GRAPHICS_MAX_HEIGHT = 2160;

        /// <summary>Graphics texture quality minimum</summary>
        public const int GRAPHICS_TEXTURE_QUALITY_MIN = 0;

        /// <summary>Graphics texture quality maximum</summary>
        public const int GRAPHICS_TEXTURE_QUALITY_MAX = 4;

        /// <summary>Graphics shadow distance minimum</summary>
        public const float GRAPHICS_SHADOW_DISTANCE_MIN = 10.0f;

        /// <summary>Graphics shadow distance maximum</summary>
        public const float GRAPHICS_SHADOW_DISTANCE_MAX = 500.0f;

        /// <summary>Graphics LOD bias minimum</summary>
        public const float GRAPHICS_LOD_BIAS_MIN = 0.1f;

        /// <summary>Graphics LOD bias maximum</summary>
        public const float GRAPHICS_LOD_BIAS_MAX = 2.0f;

        /// <summary>Graphics anisotropic filtering levels</summary>
        public const int GRAPHICS_ANISOTROPIC_LEVELS = 16;

        /// <summary>Graphics anti-aliasing samples minimum</summary>
        public const int GRAPHICS_ANTIALIASING_MIN = 0;

        /// <summary>Graphics anti-aliasing samples maximum</summary>
        public const int GRAPHICS_ANTIALIASING_MAX = 8;

        // ===== INPUT CONSTANTS =====
        /// <summary>Input polling rate in Hz</summary>
        public const int INPUT_POLLING_RATE_HZ = 120;

        /// <summary>Input deadzone threshold</summary>
        public const float INPUT_DEADZONE_THRESHOLD = 0.1f;

        /// <summary>Input sensitivity minimum</summary>
        public const float INPUT_SENSITIVITY_MIN = 0.1f;

        /// <summary>Input sensitivity maximum</summary>
        public const float INPUT_SENSITIVITY_MAX = 10.0f;

        /// <summary>Input sensitivity default</summary>
        public const float INPUT_SENSITIVITY_DEFAULT = 1.0f;

        /// <summary>Input double click interval in milliseconds</summary>
        public const int INPUT_DOUBLE_CLICK_INTERVAL_MS = 300;

        /// <summary>Input long press duration in milliseconds</summary>
        public const int INPUT_LONG_PRESS_DURATION_MS = 1000;

        /// <summary>Input key repeat delay in milliseconds</summary>
        public const int INPUT_KEY_REPEAT_DELAY_MS = 500;

        /// <summary>Input key repeat rate in milliseconds</summary>
        public const int INPUT_KEY_REPEAT_RATE_MS = 30;

        /// <summary>Input mouse wheel sensitivity</summary>
        public const float INPUT_MOUSE_WHEEL_SENSITIVITY = 1.0f;

        // ===== PHYSICS CONSTANTS =====
        /// <summary>Physics gravity default (-9.81)</summary>
        public const float PHYSICS_GRAVITY_DEFAULT = -9.81f;

        /// <summary>Physics time step fixed (1/60)</summary>
        public const float PHYSICS_TIME_STEP_FIXED = 0.016666f;

        /// <summary>Physics collision detection mode discrete</summary>
        public const int PHYSICS_COLLISION_MODE_DISCRETE = 0;

        /// <summary>Physics collision detection mode continuous</summary>
        public const int PHYSICS_COLLISION_MODE_CONTINUOUS = 1;

        /// <summary>Physics solver iterations default</summary>
        public const int PHYSICS_SOLVER_ITERATIONS_DEFAULT = 6;

        /// <summary>Physics velocity threshold</summary>
        public const float PHYSICS_VELOCITY_THRESHOLD = 0.1f;

        /// <summary>Physics angular velocity threshold</summary>
        public const float PHYSICS_ANGULAR_VELOCITY_THRESHOLD = 0.1f;

        /// <summary>Physics bounce threshold</summary>
        public const float PHYSICS_BOUNCE_THRESHOLD = 2.0f;

        /// <summary>Physics friction coefficient default</summary>
        public const float PHYSICS_FRICTION_COEFFICIENT_DEFAULT = 0.6f;

        /// <summary>Physics restitution coefficient default</summary>
        public const float PHYSICS_RESTITUTION_COEFFICIENT_DEFAULT = 0.0f;

        // ===== ANIMATION CONSTANTS =====
        /// <summary>Animation frame rate standard (30 FPS)</summary>
        public const int ANIMATION_FRAME_RATE_STANDARD = 30;

        /// <summary>Animation frame rate high (60 FPS)</summary>
        public const int ANIMATION_FRAME_RATE_HIGH = 60;

        /// <summary>Animation blend time default in seconds</summary>
        public const float ANIMATION_BLEND_TIME_DEFAULT = 0.1f;

        /// <summary>Animation crossfade time default in seconds</summary>
        public const float ANIMATION_CROSSFADE_TIME_DEFAULT = 0.25f;

        /// <summary>Animation playback speed minimum</summary>
        public const float ANIMATION_PLAYBACK_SPEED_MIN = 0.1f;

        /// <summary>Animation playback speed maximum</summary>
        public const float ANIMATION_PLAYBACK_SPEED_MAX = 10.0f;

        /// <summary>Animation playback speed default</summary>
        public const float ANIMATION_PLAYBACK_SPEED_DEFAULT = 1.0f;

        /// <summary>Animation curve tangent mode linear</summary>
        public const int ANIMATION_TANGENT_MODE_LINEAR = 0;

        /// <summary>Animation curve tangent mode smooth</summary>
        public const int ANIMATION_TANGENT_MODE_SMOOTH = 1;

        /// <summary>Animation loop count infinite</summary>
        public const int ANIMATION_LOOP_COUNT_INFINITE = -1;

        // ===== SHADER CONSTANTS =====
        /// <summary>Shader compilation timeout in milliseconds</summary>
        public const int SHADER_COMPILATION_TIMEOUT_MS = 30000;

        /// <summary>Shader property float precision</summary>
        public const int SHADER_FLOAT_PRECISION = 6;

        /// <summary>Shader texture slot maximum</summary>
        public const int SHADER_TEXTURE_SLOT_MAX = 16;

        /// <summary>Shader uniform buffer size maximum</summary>
        public const int SHADER_UNIFORM_BUFFER_SIZE_MAX = 65536;

        /// <summary>Shader vertex attribute maximum</summary>
        public const int SHADER_VERTEX_ATTRIBUTE_MAX = 16;

        /// <summary>Shader pass maximum count</summary>
        public const int SHADER_PASS_MAX_COUNT = 8;

        /// <summary>Shader keyword maximum count</summary>
        public const int SHADER_KEYWORD_MAX_COUNT = 256;

        // ===== LIGHTING CONSTANTS =====
        /// <summary>Lighting intensity minimum</summary>
        public const float LIGHTING_INTENSITY_MIN = 0.0f;

        /// <summary>Lighting intensity maximum</summary>
        public const float LIGHTING_INTENSITY_MAX = 8.0f;

        /// <summary>Lighting intensity default</summary>
        public const float LIGHTING_INTENSITY_DEFAULT = 1.0f;

        /// <summary>Lighting range minimum</summary>
        public const float LIGHTING_RANGE_MIN = 0.1f;

        /// <summary>Lighting range maximum</summary>
        public const float LIGHTING_RANGE_MAX = 1000.0f;

        /// <summary>Lighting spot angle minimum</summary>
        public const float LIGHTING_SPOT_ANGLE_MIN = 1.0f;

        /// <summary>Lighting spot angle maximum</summary>
        public const float LIGHTING_SPOT_ANGLE_MAX = 179.0f;

        /// <summary>Lighting shadow strength minimum</summary>
        public const float LIGHTING_SHADOW_STRENGTH_MIN = 0.0f;

        /// <summary>Lighting shadow strength maximum</summary>
        public const float LIGHTING_SHADOW_STRENGTH_MAX = 1.0f;

        /// <summary>Lighting color temperature minimum (Kelvin)</summary>
        public const float LIGHTING_COLOR_TEMP_MIN = 1000.0f;

        /// <summary>Lighting color temperature maximum (Kelvin)</summary>
        public const float LIGHTING_COLOR_TEMP_MAX = 40000.0f;

        // ===== PARTICLE SYSTEM CONSTANTS =====
        /// <summary>Particle system maximum particles</summary>
        public const int PARTICLE_SYSTEM_MAX_PARTICLES = 10000;

        /// <summary>Particle system emission rate minimum</summary>
        public const float PARTICLE_EMISSION_RATE_MIN = 0.0f;

        /// <summary>Particle system emission rate maximum</summary>
        public const float PARTICLE_EMISSION_RATE_MAX = 1000.0f;

        /// <summary>Particle lifetime minimum in seconds</summary>
        public const float PARTICLE_LIFETIME_MIN = 0.1f;

        /// <summary>Particle lifetime maximum in seconds</summary>
        public const float PARTICLE_LIFETIME_MAX = 100.0f;

        /// <summary>Particle start speed minimum</summary>
        public const float PARTICLE_START_SPEED_MIN = 0.0f;

        /// <summary>Particle start speed maximum</summary>
        public const float PARTICLE_START_SPEED_MAX = 100.0f;

        /// <summary>Particle start size minimum</summary>
        public const float PARTICLE_START_SIZE_MIN = 0.01f;

        /// <summary>Particle start size maximum</summary>
        public const float PARTICLE_START_SIZE_MAX = 10.0f;

        /// <summary>Particle gravity modifier minimum</summary>
        public const float PARTICLE_GRAVITY_MODIFIER_MIN = -2.0f;

        /// <summary>Particle gravity modifier maximum</summary>
        public const float PARTICLE_GRAVITY_MODIFIER_MAX = 2.0f;

        // ===== TERRAIN CONSTANTS =====
        /// <summary>Terrain height map resolution minimum</summary>
        public const int TERRAIN_HEIGHT_MAP_RES_MIN = 33;

        /// <summary>Terrain height map resolution maximum</summary>
        public const int TERRAIN_HEIGHT_MAP_RES_MAX = 4097;

        /// <summary>Terrain detail resolution minimum</summary>
        public const int TERRAIN_DETAIL_RES_MIN = 0;

        /// <summary>Terrain detail resolution maximum</summary>
        public const int TERRAIN_DETAIL_RES_MAX = 4048;

        /// <summary>Terrain control texture resolution minimum</summary>
        public const int TERRAIN_CONTROL_TEX_RES_MIN = 16;

        /// <summary>Terrain control texture resolution maximum</summary>
        public const int TERRAIN_CONTROL_TEX_RES_MAX = 2048;

        /// <summary>Terrain base map distance minimum</summary>
        public const float TERRAIN_BASE_MAP_DISTANCE_MIN = 0.0f;

        /// <summary>Terrain base map distance maximum</summary>
        public const float TERRAIN_BASE_MAP_DISTANCE_MAX = 20000.0f;

        /// <summary>Terrain tree distance minimum</summary>
        public const float TERRAIN_TREE_DISTANCE_MIN = 0.0f;

        /// <summary>Terrain tree distance maximum</summary>
        public const float TERRAIN_TREE_DISTANCE_MAX = 5000.0f;

        /// <summary>Terrain billboard start distance minimum</summary>
        public const float TERRAIN_BILLBOARD_START_MIN = 5.0f;

        /// <summary>Terrain billboard start distance maximum</summary>
        public const float TERRAIN_BILLBOARD_START_MAX = 2000.0f;

        // ===== CAMERA CONSTANTS =====
        /// <summary>Camera field of view minimum</summary>
        public const float CAMERA_FOV_MIN = 1.0f;

        /// <summary>Camera field of view maximum</summary>
        public const float CAMERA_FOV_MAX = 179.0f;

        /// <summary>Camera field of view default</summary>
        public const float CAMERA_FOV_DEFAULT = 60.0f;

        /// <summary>Camera near clipping plane minimum</summary>
        public const float CAMERA_NEAR_CLIP_MIN = 0.01f;

        /// <summary>Camera near clipping plane maximum</summary>
        public const float CAMERA_NEAR_CLIP_MAX = 10.0f;

        /// <summary>Camera near clipping plane default</summary>
        public const float CAMERA_NEAR_CLIP_DEFAULT = 0.3f;

        /// <summary>Camera far clipping plane minimum</summary>
        public const float CAMERA_FAR_CLIP_MIN = 1.0f;

        /// <summary>Camera far clipping plane maximum</summary>
        public const float CAMERA_FAR_CLIP_MAX = 50000.0f;

        /// <summary>Camera far clipping plane default</summary>
        public const float CAMERA_FAR_CLIP_DEFAULT = 1000.0f;

        /// <summary>Camera orthographic size minimum</summary>
        public const float CAMERA_ORTHO_SIZE_MIN = 0.1f;

        /// <summary>Camera orthographic size maximum</summary>
        public const float CAMERA_ORTHO_SIZE_MAX = 1000.0f;

        // ===== MATERIAL CONSTANTS =====
        /// <summary>Material metallic value minimum</summary>
        public const float MATERIAL_METALLIC_MIN = 0.0f;

        /// <summary>Material metallic value maximum</summary>
        public const float MATERIAL_METALLIC_MAX = 1.0f;

        /// <summary>Material smoothness value minimum</summary>
        public const float MATERIAL_SMOOTHNESS_MIN = 0.0f;

        /// <summary>Material smoothness value maximum</summary>
        public const float MATERIAL_SMOOTHNESS_MAX = 1.0f;

        /// <summary>Material normal map scale minimum</summary>
        public const float MATERIAL_NORMAL_SCALE_MIN = 0.0f;

        /// <summary>Material normal map scale maximum</summary>
        public const float MATERIAL_NORMAL_SCALE_MAX = 2.0f;

        /// <summary>Material parallax scale minimum</summary>
        public const float MATERIAL_PARALLAX_SCALE_MIN = 0.0f;

        /// <summary>Material parallax scale maximum</summary>
        public const float MATERIAL_PARALLAX_SCALE_MAX = 0.1f;

        /// <summary>Material emission intensity minimum</summary>
        public const float MATERIAL_EMISSION_INTENSITY_MIN = 0.0f;

        /// <summary>Material emission intensity maximum</summary>
        public const float MATERIAL_EMISSION_INTENSITY_MAX = 10.0f;

        /// <summary>Material alpha cutoff minimum</summary>
        public const float MATERIAL_ALPHA_CUTOFF_MIN = 0.0f;

        /// <summary>Material alpha cutoff maximum</summary>
        public const float MATERIAL_ALPHA_CUTOFF_MAX = 1.0f;

        // ===== TEXTURE CONSTANTS =====
        /// <summary>Texture size minimum (power of 2)</summary>
        public const int TEXTURE_SIZE_MIN = 1;

        /// <summary>Texture size maximum (power of 2)</summary>
        public const int TEXTURE_SIZE_MAX = 16384;

        /// <summary>Texture mip map level maximum</summary>
        public const int TEXTURE_MIP_LEVEL_MAX = 14;

        /// <summary>Texture compression quality minimum</summary>
        public const int TEXTURE_COMPRESSION_QUALITY_MIN = 0;

        /// <summary>Texture compression quality maximum</summary>
        public const int TEXTURE_COMPRESSION_QUALITY_MAX = 100;

        /// <summary>Texture aniso level minimum</summary>
        public const int TEXTURE_ANISO_LEVEL_MIN = 1;

        /// <summary>Texture aniso level maximum</summary>
        public const int TEXTURE_ANISO_LEVEL_MAX = 16;

        /// <summary>Texture filter mode point</summary>
        public const int TEXTURE_FILTER_MODE_POINT = 0;

        /// <summary>Texture filter mode bilinear</summary>
        public const int TEXTURE_FILTER_MODE_BILINEAR = 1;

        /// <summary>Texture filter mode trilinear</summary>
        public const int TEXTURE_FILTER_MODE_TRILINEAR = 2;

        /// <summary>Texture wrap mode repeat</summary>
        public const int TEXTURE_WRAP_MODE_REPEAT = 0;

        /// <summary>Texture wrap mode clamp</summary>
        public const int TEXTURE_WRAP_MODE_CLAMP = 1;

        // ===== ERROR MESSAGE CONSTANTS =====
        /// <summary>Error message null parameter</summary>
        public const string ERROR_MSG_NULL_PARAMETER = "Parameter cannot be null";

        /// <summary>Error message empty string</summary>
        public const string ERROR_MSG_EMPTY_STRING = "String cannot be null or empty";

        /// <summary>Error message invalid range</summary>
        public const string ERROR_MSG_INVALID_RANGE = "Value is outside the valid range";

        /// <summary>Error message file not found</summary>
        public const string ERROR_MSG_FILE_NOT_FOUND = "File not found at specified path";

        /// <summary>Error message access denied</summary>
        public const string ERROR_MSG_ACCESS_DENIED = "Access denied to specified resource";

        /// <summary>Error message timeout occurred</summary>
        public const string ERROR_MSG_TIMEOUT_OCCURRED = "Operation timed out";

        /// <summary>Error message operation cancelled</summary>
        public const string ERROR_MSG_OPERATION_CANCELLED = "Operation was cancelled";

        /// <summary>Error message invalid format</summary>
        public const string ERROR_MSG_INVALID_FORMAT = "Invalid format specified";

        /// <summary>Error message not supported</summary>
        public const string ERROR_MSG_NOT_SUPPORTED = "Operation not supported";

        /// <summary>Error message already initialized</summary>
        public const string ERROR_MSG_ALREADY_INITIALIZED = "Component already initialized";

        /// <summary>Error message not initialized</summary>
        public const string ERROR_MSG_NOT_INITIALIZED = "Component not initialized";

        /// <summary>Error message invalid operation</summary>
        public const string ERROR_MSG_INVALID_OPERATION = "Invalid operation in current state";

        /// <summary>Error message resource exhausted</summary>
        public const string ERROR_MSG_RESOURCE_EXHAUSTED = "System resources exhausted";

        /// <summary>Error message connection failed</summary>
        public const string ERROR_MSG_CONNECTION_FAILED = "Connection failed";

        /// <summary>Error message data corruption</summary>
        public const string ERROR_MSG_DATA_CORRUPTION = "Data corruption detected";

        /// <summary>Error message version mismatch</summary>
        public const string ERROR_MSG_VERSION_MISMATCH = "Version mismatch detected";

        /// <summary>Error message insufficient memory</summary>
        public const string ERROR_MSG_INSUFFICIENT_MEMORY = "Insufficient memory available";

        /// <summary>Error message invalid configuration</summary>
        public const string ERROR_MSG_INVALID_CONFIGURATION = "Invalid configuration detected";

        /// <summary>Error message operation failed</summary>
        public const string ERROR_MSG_OPERATION_FAILED = "Operation failed to complete";

        // ===== SUCCESS MESSAGE CONSTANTS =====
        /// <summary>Success message operation completed</summary>
        public const string SUCCESS_MSG_OPERATION_COMPLETED = "Operation completed successfully";

        /// <summary>Success message file saved</summary>
        public const string SUCCESS_MSG_FILE_SAVED = "File saved successfully";

        /// <summary>Success message file loaded</summary>
        public const string SUCCESS_MSG_FILE_LOADED = "File loaded successfully";

        /// <summary>Success message connection established</summary>
        public const string SUCCESS_MSG_CONNECTION_ESTABLISHED = "Connection established successfully";

        /// <summary>Success message initialization complete</summary>
        public const string SUCCESS_MSG_INITIALIZATION_COMPLETE = "Initialization completed successfully";

        /// <summary>Success message validation passed</summary>
        public const string SUCCESS_MSG_VALIDATION_PASSED = "Validation passed successfully";

        /// <summary>Success message backup created</summary>
        public const string SUCCESS_MSG_BACKUP_CREATED = "Backup created successfully";

        /// <summary>Success message settings applied</summary>
        public const string SUCCESS_MSG_SETTINGS_APPLIED = "Settings applied successfully";

        /// <summary>Success message data synchronized</summary>
        public const string SUCCESS_MSG_DATA_SYNCHRONIZED = "Data synchronized successfully";

        /// <summary>Success message cleanup completed</summary>
        public const string SUCCESS_MSG_CLEANUP_COMPLETED = "Cleanup completed successfully";

        // ===== WARNING MESSAGE CONSTANTS =====
        /// <summary>Warning message performance degraded</summary>
        public const string WARNING_MSG_PERFORMANCE_DEGRADED = "Performance degradation detected";

        /// <summary>Warning message memory usage high</summary>
        public const string WARNING_MSG_MEMORY_USAGE_HIGH = "High memory usage detected";

        /// <summary>Warning message disk space low</summary>
        public const string WARNING_MSG_DISK_SPACE_LOW = "Low disk space warning";

        /// <summary>Warning message deprecated feature</summary>
        public const string WARNING_MSG_DEPRECATED_FEATURE = "Using deprecated feature";

        /// <summary>Warning message unstable operation</summary>
        public const string WARNING_MSG_UNSTABLE_OPERATION = "Operation may be unstable";

        /// <summary>Warning message backup recommended</summary>
        public const string WARNING_MSG_BACKUP_RECOMMENDED = "Backup recommended before proceeding";

        /// <summary>Warning message compatibility issue</summary>
        public const string WARNING_MSG_COMPATIBILITY_ISSUE = "Compatibility issue detected";

        /// <summary>Warning message configuration mismatch</summary>
        public const string WARNING_MSG_CONFIGURATION_MISMATCH = "Configuration mismatch detected";

        /// <summary>Warning message resource limitation</summary>
        public const string WARNING_MSG_RESOURCE_LIMITATION = "Resource limitation encountered";

        /// <summary>Warning message temporary failure</summary>
        public const string WARNING_MSG_TEMPORARY_FAILURE = "Temporary failure occurred";

        // ===== INFORMATION MESSAGE CONSTANTS =====
        /// <summary>Info message operation started</summary>
        public const string INFO_MSG_OPERATION_STARTED = "Operation started";

        /// <summary>Info message progress update</summary>
        public const string INFO_MSG_PROGRESS_UPDATE = "Progress update available";

        /// <summary>Info message status changed</summary>
        public const string INFO_MSG_STATUS_CHANGED = "Status changed";

        /// <summary>Info message new version available</summary>
        public const string INFO_MSG_NEW_VERSION_AVAILABLE = "New version available";

        /// <summary>Info message maintenance scheduled</summary>
        public const string INFO_MSG_MAINTENANCE_SCHEDULED = "Maintenance scheduled";

        /// <summary>Info message feature enabled</summary>
        public const string INFO_MSG_FEATURE_ENABLED = "Feature enabled";

        /// <summary>Info message feature disabled</summary>
        public const string INFO_MSG_FEATURE_DISABLED = "Feature disabled";

        /// <summary>Info message system ready</summary>
        public const string INFO_MSG_SYSTEM_READY = "System ready";

        /// <summary>Info message connection restored</summary>
        public const string INFO_MSG_CONNECTION_RESTORED = "Connection restored";

        /// <summary>Info message update available</summary>
        public const string INFO_MSG_UPDATE_AVAILABLE = "Update available";

        // ===== DELIMITER AND SEPARATOR CONSTANTS =====
        /// <summary>Comma delimiter</summary>
        public const string DELIMITER_COMMA = ",";

        /// <summary>Semicolon delimiter</summary>
        public const string DELIMITER_SEMICOLON = ";";

        /// <summary>Pipe delimiter</summary>
        public const string DELIMITER_PIPE = "|";

        /// <summary>Tab delimiter</summary>
        public const string DELIMITER_TAB = "\t";

        /// <summary>Space delimiter</summary>
        public const string DELIMITER_SPACE = " ";

        /// <summary>Colon delimiter</summary>
        public const string DELIMITER_COLON = ":";

        /// <summary>Equals delimiter</summary>
        public const string DELIMITER_EQUALS = "=";

        /// <summary>Ampersand delimiter</summary>
        public const string DELIMITER_AMPERSAND = "&";

        /// <summary>Question mark delimiter</summary>
        public const string DELIMITER_QUESTION_MARK = "?";

        /// <summary>Hash delimiter</summary>
        public const string DELIMITER_HASH = "#";

        /// <summary>At symbol delimiter</summary>
        public const string DELIMITER_AT_SYMBOL = "@";

        /// <summary>Percent delimiter</summary>
        public const string DELIMITER_PERCENT = "%";

        /// <summary>Dollar delimiter</summary>
        public const string DELIMITER_DOLLAR = "$";

        /// <summary>Caret delimiter</summary>
        public const string DELIMITER_CARET = "^";

        /// <summary>Plus delimiter</summary>
        public const string DELIMITER_PLUS = "+";

        /// <summary>Minus delimiter</summary>
        public const string DELIMITER_MINUS = "-";

        /// <summary>Underscore delimiter</summary>
        public const string DELIMITER_UNDERSCORE = "_";

        /// <summary>Tilde delimiter</summary>
        public const string DELIMITER_TILDE = "~";

        /// <summary>Backtick delimiter</summary>
        public const string DELIMITER_BACKTICK = "`";

        /// <summary>Exclamation delimiter</summary>
        public const string DELIMITER_EXCLAMATION = "!";

        // ===== BRACKET AND PARENTHESIS CONSTANTS =====
        /// <summary>Opening parenthesis</summary>
        public const string BRACKET_OPEN_PAREN = "(";

        /// <summary>Closing parenthesis</summary>
        public const string BRACKET_CLOSE_PAREN = ")";

        /// <summary>Opening square bracket</summary>
        public const string BRACKET_OPEN_SQUARE = "[";

        /// <summary>Closing square bracket</summary>
        public const string BRACKET_CLOSE_SQUARE = "]";

        /// <summary>Opening curly brace</summary>
        public const string BRACKET_OPEN_CURLY = "{";

        /// <summary>Closing curly brace</summary>
        public const string BRACKET_CLOSE_CURLY = "}";

        /// <summary>Opening angle bracket</summary>
        public const string BRACKET_OPEN_ANGLE = "<";

        /// <summary>Closing angle bracket</summary>
        public const string BRACKET_CLOSE_ANGLE = ">";

        // ===== QUOTE AND ESCAPE CONSTANTS =====
        /// <summary>Single quote</summary>
        public const string QUOTE_SINGLE = "'";

        /// <summary>Double quote</summary>
        public const string QUOTE_DOUBLE = "\"";

        /// <summary>Backslash escape</summary>
        public const string ESCAPE_BACKSLASH = "\\";

        /// <summary>Forward slash</summary>
        public const string ESCAPE_FORWARD_SLASH = "/";

        /// <summary>Newline escape</summary>
        public const string ESCAPE_NEWLINE = "\n";

        /// <summary>Carriage return escape</summary>
        public const string ESCAPE_CARRIAGE_RETURN = "\r";

        /// <summary>Tab escape</summary>
        public const string ESCAPE_TAB = "\t";

        /// <summary>Vertical tab escape</summary>
        public const string ESCAPE_VERTICAL_TAB = "\v";

        /// <summary>Form feed escape</summary>
        public const string ESCAPE_FORM_FEED = "\f";

        /// <summary>Backspace escape</summary>
        public const string ESCAPE_BACKSPACE = "\b";

        /// <summary>Alert (bell) escape</summary>
        public const string ESCAPE_ALERT = "\a";

        /// <summary>Null character escape</summary>
        public const string ESCAPE_NULL = "\0";

        // ===== ENCODING CONSTANTS =====
        /// <summary>UTF-8 encoding name</summary>
        public const string ENCODING_UTF8 = "UTF-8";

        /// <summary>UTF-16 encoding name</summary>
        public const string ENCODING_UTF16 = "UTF-16";

        /// <summary>UTF-32 encoding name</summary>
        public const string ENCODING_UTF32 = "UTF-32";

        /// <summary>ASCII encoding name</summary>
        public const string ENCODING_ASCII = "ASCII";

        /// <summary>ISO-8859-1 encoding name</summary>
        public const string ENCODING_ISO_8859_1 = "ISO-8859-1";

        /// <summary>Windows-1252 encoding name</summary>
        public const string ENCODING_WINDOWS_1252 = "Windows-1252";

        /// <summary>Base64 encoding name</summary>
        public const string ENCODING_BASE64 = "Base64";

        /// <summary>Hex encoding name</summary>
        public const string ENCODING_HEX = "Hexadecimal";

        // ===== CULTURE CONSTANTS =====
        /// <summary>Invariant culture name</summary>
        public const string CULTURE_INVARIANT = "";

        /// <summary>English US culture</summary>
        public const string CULTURE_EN_US = "en-US";

        /// <summary>English UK culture</summary>
        public const string CULTURE_EN_UK = "en-GB";

        /// <summary>German culture</summary>
        public const string CULTURE_DE_DE = "de-DE";

        /// <summary>French culture</summary>
        public const string CULTURE_FR_FR = "fr-FR";

        /// <summary>Spanish culture</summary>
        public const string CULTURE_ES_ES = "es-ES";

        /// <summary>Italian culture</summary>
        public const string CULTURE_IT_IT = "it-IT";

        /// <summary>Japanese culture</summary>
        public const string CULTURE_JA_JP = "ja-JP";

        /// <summary>Chinese simplified culture</summary>
        public const string CULTURE_ZH_CN = "zh-CN";

        /// <summary>Chinese traditional culture</summary>
        public const string CULTURE_ZH_TW = "zh-TW";

        /// <summary>Korean culture</summary>
        public const string CULTURE_KO_KR = "ko-KR";

        /// <summary>Russian culture</summary>
        public const string CULTURE_RU_RU = "ru-RU";

        /// <summary>Portuguese Brazil culture</summary>
        public const string CULTURE_PT_BR = "pt-BR";

        /// <summary>Dutch culture</summary>
        public const string CULTURE_NL_NL = "nl-NL";

        /// <summary>Swedish culture</summary>
        public const string CULTURE_SV_SE = "sv-SE";

        /// <summary>Norwegian culture</summary>
        public const string CULTURE_NO_NO = "no-NO";

        /// <summary>Danish culture</summary>
        public const string CULTURE_DA_DK = "da-DK";

        /// <summary>Finnish culture</summary>
        public const string CULTURE_FI_FI = "fi-FI";

        /// <summary>Polish culture</summary>
        public const string CULTURE_PL_PL = "pl-PL";

        /// <summary>Turkish culture</summary>
        public const string CULTURE_TR_TR = "tr-TR";

        /// <summary>Arabic culture</summary>
        public const string CULTURE_AR_SA = "ar-SA";

        /// <summary>Hebrew culture</summary>
        public const string CULTURE_HE_IL = "he-IL";

        /// <summary>Hindi culture</summary>
        public const string CULTURE_HI_IN = "hi-IN";

        /// <summary>Thai culture</summary>
        public const string CULTURE_TH_TH = "th-TH";

        /// <summary>Vietnamese culture</summary>
        public const string CULTURE_VI_VN = "vi-VN";

        /// <summary>Indonesian culture</summary>
        public const string CULTURE_ID_ID = "id-ID";

        /// <summary>Malay culture</summary>
        public const string CULTURE_MS_MY = "ms-MY";

        /// <summary>Ukrainian culture</summary>
        public const string CULTURE_UK_UA = "uk-UA";

        /// <summary>Czech culture</summary>
        public const string CULTURE_CS_CZ = "cs-CZ";

        /// <summary>Hungarian culture</summary>
        public const string CULTURE_HU_HU = "hu-HU";

        /// <summary>Greek culture</summary>
        public const string CULTURE_EL_GR = "el-GR";

        /// <summary>Bulgarian culture</summary>
        public const string CULTURE_BG_BG = "bg-BG";

        /// <summary>Romanian culture</summary>
        public const string CULTURE_RO_RO = "ro-RO";

        /// <summary>Croatian culture</summary>
        public const string CULTURE_HR_HR = "hr-HR";

        /// <summary>Serbian culture</summary>
        public const string CULTURE_SR_RS = "sr-RS";

        /// <summary>Slovenian culture</summary>
        public const string CULTURE_SL_SI = "sl-SI";

        /// <summary>Slovak culture</summary>
        public const string CULTURE_SK_SK = "sk-SK";

        /// <summary>Estonian culture</summary>
        public const string CULTURE_ET_EE = "et-EE";

        /// <summary>Latvian culture</summary>
        public const string CULTURE_LV_LV = "lv-LV";

        /// <summary>Lithuanian culture</summary>
        public const string CULTURE_LT_LT = "lt-LT";
    }
}