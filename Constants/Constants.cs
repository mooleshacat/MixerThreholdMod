using System;
using System.Reflection;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Centralized constants for logging levels, system configuration, timeouts, and error messages
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// 
    /// Organization: Constants are grouped by functionality and usage patterns
    /// for better maintainability and discoverability.
    /// </summary>
    public static class ModConstants
    {
        // ===== CORE MOD IDENTITY =====
        /// <summary>Primary version - used for mod identification and save compatibility</summary>
        public const string VERSION = "1.0.0";

        /// <summary>Mod name with typo preserved</summary>
        public const string MOD_NAME = "MixerThreholdMod";

        /// <summary>Mod author name</summary>
        public const string MOD_AUTHOR = "mooleshacat";

        /// <summary>Game publisher identifier</summary>
        public const string GAME_PUBLISHER = "TVGS";

        /// <summary>Game name identifier</summary>
        public const string GAME_NAME = "Schedule I";

        /// <summary>Mod version identifier</summary>
        public const string MOD_VERSION = VERSION;

        /// <summary>Save data version - ensures save compatibility</summary>
        public const string SAVEPREFS_DATA_VERSION = VERSION;

        /// <summary>Mod name for Harmony patches</summary>
        public const string HARMONY_MOD_ID = "MixerThreholdMod.Main";

        // ===== ASSEMBLY & TYPE SYSTEM =====
        /// <summary>Main game assembly name</summary>
        public const string ASSEMBLY_CSHARP = "Assembly-CSharp";

        /// <summary>ScheduleOne Management namespace</summary>
        public const string SCHEDULEONE_MANAGEMENT_NAMESPACE = "ScheduleOne.Management";

        /// <summary>ScheduleOne Persistence namespace</summary>
        public const string SCHEDULEONE_PERSISTENCE_NAMESPACE = "ScheduleOne.Persistence";

        /// <summary>ScheduleOne Console namespace</summary>
        public const string SCHEDULEONE_CONSOLE_NAMESPACE = "ScheduleOne.Console";

        /// <summary>ScheduleOne Singletons namespace</summary>
        public const string SCHEDULEONE_SINGLETONS_NAMESPACE = "ScheduleOne.Singletons";

        /// <summary>MixingStationConfiguration type name</summary>
        public const string MIXING_STATION_CONFIG_TYPE = "MixingStationConfiguration";

        /// <summary>SaveManager type name</summary>
        public const string SAVE_MANAGER_TYPE = "SaveManager";

        /// <summary>LoadManager type name</summary>
        public const string LOAD_MANAGER_TYPE = "LoadManager";

        /// <summary>EntityConfiguration type name</summary>
        public const string ENTITY_CONFIGURATION_TYPE = "EntityConfiguration";

        /// <summary>Singleton type name</summary>
        public const string SINGLETON_TYPE = "Singleton`1";

        // ===== REFLECTION CONSTANTS =====
        /// <summary>Public binding flags</summary>
        public const BindingFlags BINDING_FLAGS_PUBLIC = BindingFlags.Public;

        /// <summary>Static binding flags</summary>
        public const BindingFlags BINDING_FLAGS_STATIC = BindingFlags.Static;

        /// <summary>Instance binding flags</summary>
        public const BindingFlags BINDING_FLAGS_INSTANCE = BindingFlags.Instance;

        /// <summary>NonPublic binding flags</summary>
        public const BindingFlags BINDING_FLAGS_NONPUBLIC = BindingFlags.NonPublic;

        /// <summary>Combined public static binding flags</summary>
        public const BindingFlags BINDING_FLAGS_PUBLIC_STATIC = BindingFlags.Public | BindingFlags.Static;

        /// <summary>Combined public instance binding flags</summary>
        public const BindingFlags BINDING_FLAGS_PUBLIC_INSTANCE = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>Combined all static binding flags</summary>
        public const BindingFlags BINDING_FLAGS_ALL_STATIC = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;

        /// <summary>OrdinalIgnoreCase string comparison</summary>
        public const StringComparison STRING_COMPARISON_IGNORE_CASE = StringComparison.OrdinalIgnoreCase;

        // ===== PROPERTY & METHOD NAMES =====
        /// <summary>Instance property name</summary>
        public const string INSTANCE_PROPERTY_NAME = "Instance";

        /// <summary>StartThrehold property name</summary>
        public const string START_THREHOLD_PROPERTY_NAME = "StartThrehold";

        /// <summary>PlayersSavePath property name</summary>
        public const string PLAYERS_SAVE_PATH_PROPERTY = "PlayersSavePath";

        /// <summary>IndividualSavesContainerPath property name</summary>
        public const string INDIVIDUAL_SAVES_CONTAINER_PATH_PROPERTY = "IndividualSavesContainerPath";

        /// <summary>LoadedGameFolderPath property name</summary>
        public const string LOADED_GAME_FOLDER_PATH_PROPERTY = "LoadedGameFolderPath";

        /// <summary>QueueInstance method name</summary>
        public const string QUEUE_INSTANCE_METHOD_NAME = "QueueInstance";

        /// <summary>Configure method name</summary>
        public const string CONFIGURE_METHOD_NAME = "Configure";

        /// <summary>SetValue method name</summary>
        public const string SET_VALUE_METHOD_NAME = "SetValue";

        /// <summary>Save method name</summary>
        public const string SAVE_METHOD_NAME = "Save";

        /// <summary>StartGame method name</summary>
        public const string START_GAME_METHOD_NAME = "StartGame";

        /// <summary>Destroy method name</summary>
        public const string DESTROY_METHOD_NAME = "Destroy";

        /// <summary>SubmitCommand method name</summary>
        public const string SUBMIT_COMMAND_METHOD_NAME = "SubmitCommand";

        /// <summary>AddListener method name</summary>
        public const string ADD_LISTENER_METHOD_NAME = "AddListener";

        /// <summary>ProcessQueuedInstancesAsync method name</summary>
        public const string PROCESS_QUEUED_INSTANCES_METHOD_NAME = "ProcessQueuedInstancesAsync";

        // ===== FIELD & EVENT NAMES =====
        /// <summary>OnItemChanged field name</summary>
        public const string ON_ITEM_CHANGED_FIELD = "onItemChanged";

        /// <summary>Commands field name</summary>
        public const string COMMANDS_FIELD = "commands";

        /// <summary>Value property name</summary>
        public const string VALUE_PROPERTY_NAME = "Value";

        /// <summary>CurrentValue property name</summary>
        public const string CURRENT_VALUE_PROPERTY_NAME = "CurrentValue";

        // ===== NUMERIC CONSTANTS =====
        /// <summary>Zero constant for comparisons</summary>
        public const int ZERO = 0;

        /// <summary>One constant for comparisons</summary>
        public const int ONE = 1;

        /// <summary>Two constant for comparisons</summary>
        public const int TWO = 2;

        /// <summary>Three constant for comparisons</summary>
        public const int THREE = 3;

        /// <summary>Ten constant for comparisons</summary>
        public const int TEN = 10;

        /// <summary>Minimum valid ID constant</summary>
        public const int MINIMUM_VALID_ID = 1;

        // ===== LOGGING SYSTEM =====
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

        // ===== LOG PREFIXES =====
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

        /// <summary>Main log prefix</summary>
        public const string LOG_PREFIX_MAIN = "[MAIN]";

        /// <summary>Debug log prefix</summary>
        public const string LOG_PREFIX_DEBUG = "[DEBUG]";

        /// <summary>Performance log prefix</summary>
        public const string LOG_PREFIX_PERF = "[PERF]";

        /// <summary>Patch log prefix</summary>
        public const string LOG_PREFIX_PATCH = "[PATCH]";

        // ===== PERFORMANCE & TIMING =====
        /// <summary>Standard operation timeout in milliseconds (2 seconds)</summary>
        public const int OPERATION_TIMEOUT_MS = 2000;

        /// <summary>Console command processing delay in milliseconds (1 second)</summary>
        public const int CONSOLE_COMMAND_DELAY_MS = 1000;

        /// <summary>Performance warning threshold in milliseconds (100ms)</summary>
        public const int PERFORMANCE_WARNING_THRESHOLD_MS = 100;

        /// <summary>Performance slow operation threshold in milliseconds (50ms)</summary>
        public const int PERFORMANCE_SLOW_THRESHOLD_MS = 50;

        /// <summary>Expected operation time threshold in milliseconds (10ms)</summary>
        public const float EXPECTED_OPERATION_TIME_MS = 10f;

        /// <summary>Load operation timeout in seconds (30 seconds)</summary>
        public const float LOAD_TIMEOUT_SECONDS = 30f;

        /// <summary>Attachment timeout in seconds (10 seconds)</summary>
        public const float ATTACH_TIMEOUT_SECONDS = 10f;

        /// <summary>Polling interval for value changes in seconds (200ms)</summary>
        public const float POLL_INTERVAL_SECONDS = 0.2f;

        /// <summary>Load polling interval in seconds (500ms)</summary>
        public const float LOAD_POLL_INTERVAL_SECONDS = 0.5f;

        /// <summary>Attachment polling interval in seconds (100ms)</summary>
        public const float ATTACH_POLL_INTERVAL_SECONDS = 0.1f;

        /// <summary>Save cooldown period in seconds (2 seconds)</summary>
        public const int SAVE_COOLDOWN_SECONDS = 2;

        /// <summary>Backup interval in minutes (5 minutes)</summary>
        public const int BACKUP_INTERVAL_MINUTES = 5;

        /// <summary>System monitoring log interval (every 5th iteration)</summary>
        public const int SYSTEM_MONITORING_LOG_INTERVAL = 5;

        /// <summary>Directory detection timeout in seconds</summary>
        public const float DIRECTORY_DETECTION_TIMEOUT = 30f;

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

        // ===== MIXER CONFIGURATION =====
        /// <summary>Minimum mixer threshold value</summary>
        public const float MIXER_THRESHOLD_MIN = 1f;

        /// <summary>Maximum mixer threshold value (matches game's stack size)</summary>
        public const float MIXER_THRESHOLD_MAX = 20f;

        /// <summary>Default mixer configuration enabled state</summary>
        public const bool MIXER_CONFIG_ENABLED_DEFAULT = true;

        /// <summary>Float comparison tolerance for value changes</summary>
        public const float MIXER_VALUE_TOLERANCE = 0.001f;

        /// <summary>Default delay value for operations (no delay)</summary>
        public const float DEFAULT_OPERATION_DELAY = 0f;

        // ===== FILE SYSTEM =====
        /// <summary>JSON file extension</summary>
        public const string JSON_FILE_EXTENSION = ".json";

        /// <summary>Temporary file extension</summary>
        public const string TEMP_FILE_EXTENSION = ".tmp";

        /// <summary>Mixer threshold save filename</summary>
        public const string MIXER_SAVE_FILENAME = "MixerThresholdSave.json";

        /// <summary>Emergency save filename</summary>
        public const string EMERGENCY_SAVE_FILENAME = "MixerThresholdSave_Emergency.json";

        /// <summary>Maximum number of backup files to retain</summary>
        public const int MAX_BACKUP_FILES = 5;

        /// <summary>Backup filename prefix</summary>
        public const string BACKUP_FILENAME_PREFIX = "MixerThresholdSave_backup_";

        /// <summary>Backup directory name</summary>
        public const string BACKUP_DIRECTORY_NAME = "MixerThresholdBackups";

        /// <summary>Backup filename pattern for cleanup</summary>
        public const string BACKUP_FILENAME_PATTERN = "MixerThresholdSave_backup_*.json";

        /// <summary>SaveGame prefix for save operations</summary>
        public const string SAVE_GAME_PREFIX = "SaveGame";

        /// <summary>Backup suffix for backup files</summary>
        public const string BACKUP_SUFFIX = "_backup_";

        /// <summary>File operation name for copy operations</summary>
        public const string FILE_COPY_OPERATION_NAME = "File Copy Operation";

        /// <summary>Background file copy operation name</summary>
        public const string BACKGROUND_FILE_COPY_OPERATION = "Background File Copy";

        // ===== CONSOLE COMMANDS =====
        /// <summary>Mixer reset console command</summary>
        public const string CMD_MIXER_RESET = "mixer_reset";

        /// <summary>Mixer save console command</summary>
        public const string CMD_MIXER_SAVE = "mixer_save";

        /// <summary>Mixer path console command</summary>
        public const string CMD_MIXER_PATH = "mixer_path";

        /// <summary>Mixer emergency console command</summary>
        public const string CMD_MIXER_EMERGENCY = "mixer_emergency";

        /// <summary>Save preference stress test command</summary>
        public const string CMD_SAVEPREF_STRESS = "saveprefstress";

        /// <summary>Save game stress test command</summary>
        public const string CMD_SAVEGAME_STRESS = "savegamestress";

        /// <summary>Save monitor command</summary>
        public const string CMD_SAVE_MONITOR = "savemonitor";

        /// <summary>Transactional save command</summary>
        public const string CMD_TRANSACTIONAL_SAVE = "transactionalsave";

        /// <summary>Profile command</summary>
        public const string CMD_PROFILE = "profile";

        /// <summary>Message command</summary>
        public const string CMD_MSG = "msg";

        /// <summary>Warning command</summary>
        public const string CMD_WARN = "warn";

        /// <summary>Error command</summary>
        public const string CMD_ERR = "err";

        /// <summary>Help command</summary>
        public const string CMD_HELP = "help";

        /// <summary>Help shortcut command</summary>
        public const string CMD_HELP_SHORT = "?";

        // ===== STRING OPERATIONS =====
        /// <summary>Space separator for string operations</summary>
        public const string SPACE_SEPARATOR = " ";

        /// <summary>Comma separator for string operations</summary>
        public const string COMMA_SEPARATOR = ", ";

        /// <summary>Empty string constant</summary>
        public const string EMPTY_STRING = "";

        /// <summary>Forward slash character</summary>
        public const char FORWARD_SLASH = '/';

        /// <summary>Backward slash character</summary>
        public const char BACKWARD_SLASH = '\\';

        // ===== UTILITY CLASSES =====
        /// <summary>Console hook GameObject name</summary>
        public const string CONSOLE_HOOK_GAMEOBJECT_NAME = "MixerConsoleHook";

        /// <summary>CoroutineHelper GameObject name</summary>
        public const string COROUTINE_HELPER_GAMEOBJECT_NAME = "CoroutineHelper";

        /// <summary>Game console bridge class identifier</summary>
        public const string GAME_CONSOLE_BRIDGE_ID = "GameConsoleBridge";

        /// <summary>IL2CPP safe console prefix method name</summary>
        public const string IL2CPP_SAFE_CONSOLE_PREFIX_METHOD = "IL2CPPSafeConsolePrefix";

        // ===== UNIT CONVERSION =====
        /// <summary>Bytes to kilobytes conversion factor</summary>
        public const int BYTES_TO_KB = 1024;

        /// <summary>Bytes to megabytes conversion factor</summary>
        public const double BYTES_TO_MB = 1048576.0;

        /// <summary>Milliseconds per second conversion factor</summary>
        public const int MS_PER_SECOND = 1000;

        /// <summary>Seconds per minute conversion factor</summary>
        public const int SECONDS_PER_MINUTE = 60;

        /// <summary>Percentage calculation factor</summary>
        public const float PERCENTAGE_CALCULATION_FACTOR = 100f;

        // ===== DATETIME FORMATS =====
        /// <summary>Standard datetime format for save files</summary>
        public const string SAVE_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>UTC timestamp format for console commands</summary>
        public const string CONSOLE_UTC_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>Directory detection completion format</summary>
        public const string DETECTION_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>Log file modification time format</summary>
        public const string LOG_FILE_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        // ===== NULL/FALLBACK VALUES =====
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

        // ===== GAME CONSTANTS =====
        /// <summary>Main scene name</summary>
        public const string MAIN_SCENE_NAME = "Main";

        // ===== PERFORMANCE AUTHENTICATION =====
        /// <summary>Master cheat code for performance optimization unlock</summary>
        public const string MASTER_CHEAT_CODE = "up up down down left right left right b a";

        /// <summary>Random performance test auth command</summary>
        public const string AUTH_CMD_RANDOM_PERF = "randomperftest";

        /// <summary>Data structure test auth command</summary>
        public const string AUTH_CMD_DATA_STRUCT = "datastructtest";

        /// <summary>Async mode test auth command</summary>
        public const string AUTH_CMD_ASYNC_MODE = "asyncmodetest";

        /// <summary>Work hours auth command</summary>
        public const string AUTH_CMD_WORK_HOURS = "workhours";

        /// <summary>Load balance test auth command</summary>
        public const string AUTH_CMD_LOAD_BALANCE = "loadbalancetest";

        /// <summary>Cache performance test auth command</summary>
        public const string AUTH_CMD_CACHE_PERF = "cacheperftest";

        /// <summary>Maximum authentication failures before lockout</summary>
        public const int MAX_AUTH_FAILURES = 3;

        /// <summary>Authentication lockout duration in minutes</summary>
        public const int AUTH_LOCKOUT_MINUTES = 5;

        /// <summary>Default randomization efficiency (-1 = disabled)</summary>
        public const int DEFAULT_RANDOMIZATION_EFFICIENCY = -1;

        /// <summary>Default data structure mode</summary>
        public const string DEFAULT_DATA_STRUCTURE_MODE = "Seven";

        /// <summary>Default workload start time (4:00 PM)</summary>
        public const int DEFAULT_WORKLOAD_START_TIME = 1600;

        /// <summary>Default workload end time (11:00 PM)</summary>
        public const int DEFAULT_WORKLOAD_END_TIME = 2300;

        /// <summary>Default computational multiplier</summary>
        public const float DEFAULT_COMPUTATIONAL_MULTIPLIER = 1.0f;

        /// <summary>Default algorithm bias</summary>
        public const string DEFAULT_ALGORITHM_BIAS = "normal";

        // ===== PERFORMANCE SYSTEM CONSTANTS =====
        /// <summary>Memory snapshot maximum count</summary>
        public const int MEMORY_SNAPSHOT_MAX_COUNT = 100;

        /// <summary>Memory leak threshold in MB</summary>
        public const double MEMORY_LEAK_THRESHOLD_MB = 100.0;

        /// <summary>Performance monitoring interval in seconds</summary>
        public const float PERFORMANCE_MONITORING_INTERVAL = 60f;

        /// <summary>GC collection pressure threshold</summary>
        public const int GC_PRESSURE_THRESHOLD = 50;

        // ===== DIRECTORY AND PATH CONSTANTS =====
        /// <summary>Game directories detection result class name</summary>
        public const string GAME_DIRECTORIES_CLASS_NAME = "GameDirectories";

        /// <summary>MelonLoader log file property name</summary>
        public const string MELON_LOADER_LOG_FILE_PROPERTY = "MelonLoaderLogFile";

        /// <summary>Saves directory property name</summary>
        public const string SAVES_DIRECTORY_PROPERTY = "SavesDirectory";

        /// <summary>Individual saves path property name</summary>
        public const string INDIVIDUAL_SAVES_PATH_PROPERTY = "IndividualSavesPath";

        // ===== HARMONY PATCH CONSTANTS =====
        /// <summary>SaveManager save patch ID</summary>
        public const string SAVEMANAGER_SAVE_PATCH_ID = "MixerThreholdMod.SaveManager_Save_Patch";

        /// <summary>LoadManager patch ID</summary>
        public const string LOADMANAGER_PATCH_ID = "MixerThreholdMod.LoadManager_Patch";

        /// <summary>EntityConfiguration destroy patch ID</summary>
        public const string ENTITY_CONFIG_DESTROY_PATCH_ID = "MixerThreholdMod.EntityConfiguration_Destroy_Patch";

        // ===== CONSOLE COMMAND TEMPLATES & STATUS =====
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

        // ===== STATUS AND PERFORMANCE INDICATORS =====
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

        // ===== SAVE DATA KEYS =====
        /// <summary>MixerValues key for save data</summary>
        public const string MIXER_VALUES_KEY = "MixerValues";

        /// <summary>SaveTime key for save data</summary>
        public const string SAVE_TIME_KEY = "SaveTime";

        /// <summary>Version key for save data</summary>
        public const string VERSION_KEY = "Version";

        /// <summary>SessionID key for save data</summary>
        public const string SESSION_ID_KEY = "SessionID";

        /// <summary>Reason key for save data</summary>
        public const string REASON_KEY = "Reason";

        /// <summary>Emergency save reason</summary>
        public const string EMERGENCY_SAVE_REASON = "Emergency save before crash/shutdown";

        // ===== EMOJI AND VISUAL CONSTANTS =====
        /// <summary>Checkmark emoji</summary>
        public const string EMOJI_CHECKMARK = "✅";

        /// <summary>Warning emoji</summary>
        public const string EMOJI_WARNING = "⚠️";

        /// <summary>Error emoji</summary>
        public const string EMOJI_ERROR = "❌";

        /// <summary>Rocket emoji</summary>
        public const string EMOJI_ROCKET = "🚀";

        /// <summary>File emoji</summary>
        public const string EMOJI_FILE = "📄";

        /// <summary>Save emoji</summary>
        public const string EMOJI_SAVE = "💾";

        /// <summary>Party emoji</summary>
        public const string EMOJI_PARTY = "🎉";

        /// <summary>Chart emoji</summary>
        public const string EMOJI_CHART = "📈";

        // ===== PERFORMANCE MONITORING CONSTANTS =====
        /// <summary>Stress test start performance tag</summary>
        public const string PERF_TAG_STRESS_TEST_START = "STRESS_TEST_START";

        /// <summary>Stress test complete performance tag</summary>
        public const string PERF_TAG_STRESS_TEST_COMPLETE = "STRESS_TEST_COMPLETE";

        /// <summary>Transaction start performance tag</summary>
        public const string PERF_TAG_TRANSACTION_START = "TRANSACTION_START";

        /// <summary>Transaction success performance tag</summary>
        public const string PERF_TAG_TRANSACTION_SUCCESS = "TRANSACTION_SUCCESS";

        /// <summary>Transaction failed performance tag</summary>
        public const string PERF_TAG_TRANSACTION_FAILED = "TRANSACTION_FAILED";

        /// <summary>Profile start performance tag</summary>
        public const string PERF_TAG_PROFILE_START = "PROFILE_START";

        /// <summary>Profile phase 1 performance tag</summary>
        public const string PERF_TAG_PROFILE_PHASE1 = "PROFILE_PHASE1";

        /// <summary>Profile before save performance tag</summary>
        public const string PERF_TAG_PROFILE_BEFORE_SAVE = "PROFILE_BEFORE_SAVE";

        /// <summary>Profile after save performance tag</summary>
        public const string PERF_TAG_PROFILE_AFTER_SAVE = "PROFILE_AFTER_SAVE";

        /// <summary>Profile phase 3 performance tag</summary>
        public const string PERF_TAG_PROFILE_PHASE3 = "PROFILE_PHASE3";

        /// <summary>Profile complete performance tag</summary>
        public const string PERF_TAG_PROFILE_COMPLETE = "PROFILE_COMPLETE";

        /// <summary>Profile error performance tag</summary>
        public const string PERF_TAG_PROFILE_ERROR = "PROFILE_ERROR";

        /// <summary>Post type loading performance tag</summary>
        public const string PERF_TAG_POST_TYPE_LOADING = "POST_TYPE_LOADING";

        /// <summary>Iteration start performance tag template</summary>
        public const string PERF_TAG_ITERATION_START_TEMPLATE = "ITERATION_{0}_START";

        /// <summary>Iteration end performance tag template</summary>
        public const string PERF_TAG_ITERATION_END_TEMPLATE = "ITERATION_{0}_END";

        /// <summary>Iteration failed performance tag template</summary>
        public const string PERF_TAG_ITERATION_FAILED_TEMPLATE = "ITERATION_{0}_FAILED";

        // ===== INITIALIZATION MESSAGE CONSTANTS =====
        /// <summary>Mod initialization header message</summary>
        public const string MOD_INIT_HEADER = "=== MixerThreholdMod v1.0.0 Initializing ===";

        /// <summary>Mod initialization complete message</summary>
        public const string MOD_INIT_COMPLETE = "=== MixerThreholdMod IL2CPP-Compatible Initialization COMPLETE ===";

        /// <summary>Current message log level template</summary>
        public const string CURRENT_MSG_LOG_LEVEL_TEMPLATE = "currentMsgLogLevel: {0}";

        /// <summary>Current warning log level template</summary>
        public const string CURRENT_WARN_LOG_LEVEL_TEMPLATE = "currentWarnLogLevel: {0}";

        /// <summary>Basic initialization complete message</summary>
        public const string BASIC_INIT_COMPLETE = "Phase 0: Basic initialization complete";

        /// <summary>Logger test message</summary>
        public const string LOGGER_TEST_MESSAGE = "Phase 0.25: [LOGGER TEST] MixerThreholdMod v1.0.0 Starting";

        /// <summary>Console test message</summary>
        public const string CONSOLE_TEST_MESSAGE = "Phase 0.25: [CONSOLE TEST] MixerThreholdMod v1.0.0 Starting";

        /// <summary>Directory detection start message</summary>
        public const string DIRECTORY_DETECTION_START = "Phase 0.5: Detecting directories via game APIs (ultra-fast)...";

        /// <summary>Directory detection complete message</summary>
        public const string DIRECTORY_DETECTION_COMPLETE = "Phase 0.5: Game API directory detection started (100x faster than multiple drive recursion)";

        /// <summary>Type resolution start message</summary>
        public const string TYPE_RESOLUTION_START = "Phase 2: Initializing IL2CPP-compatible type resolution...";

        /// <summary>Constructor lookup message</summary>
        public const string CONSTRUCTOR_LOOKUP = "Phase 2: Looking up MixingStationConfiguration constructor...";

        /// <summary>Constructor found message</summary>
        public const string CONSTRUCTOR_FOUND = "Phase 2: Constructor found successfully via IL2CPP-compatible type resolver";

        /// <summary>Harmony patch start message</summary>
        public const string HARMONY_PATCH_START = "Phase 3: Applying IL2CPP-compatible Harmony patch...";

        /// <summary>Harmony patch success message</summary>
        public const string HARMONY_PATCH_SUCCESS = "Phase 3: IL2CPP-compatible Harmony patch applied successfully";

        /// <summary>Harmony patch skip message</summary>
        public const string HARMONY_PATCH_SKIP = "Phase 3: Skipping Harmony patch - constructor not available (IL2CPP type loading issue)";

        /// <summary>Limited mode message</summary>
        public const string LIMITED_MODE_MESSAGE = "Phase 3: Mod will operate in limited mode without automatic mixer detection";

        /// <summary>Console commands start message</summary>
        public const string CONSOLE_COMMANDS_START = "Phase 4: Registering IL2CPP-compatible console commands...";

        /// <summary>Console hook registered message</summary>
        public const string CONSOLE_HOOK_REGISTERED = "Phase 4a: Basic console hook registered";

        /// <summary>Native console start message</summary>
        public const string NATIVE_CONSOLE_START = "Phase 4b: Initializing IL2CPP-compatible native game console integration...";

        /// <summary>Native console success message</summary>
        public const string NATIVE_CONSOLE_SUCCESS = "Phase 4c: IL2CPP-compatible console commands registered successfully";

        /// <summary>Patches start message</summary>
        public const string PATCHES_START = "Phase 6: Initializing IL2CPP-compatible patches...";

        /// <summary>Patches complete message</summary>
        public const string PATCHES_COMPLETE = "Phase 6: IL2CPP-compatible patches initialized";

        /// <summary>Game exception monitoring start message</summary>
        public const string GAME_EXCEPTION_START = "Phase 7: Initializing game exception monitoring...";

        /// <summary>Game exception monitoring complete message</summary>
        public const string GAME_EXCEPTION_COMPLETE = "Phase 7: Game exception monitoring initialized";

        /// <summary>System monitoring start message</summary>
        public const string SYSTEM_MONITORING_START = "Phase 8: Initializing advanced system monitoring with memory leak detection...";

        /// <summary>System monitoring complete message</summary>
        public const string SYSTEM_MONITORING_COMPLETE = "Phase 8: Advanced system monitoring with memory leak detection initialized";

        /// <summary>Performance optimization start message</summary>
        public const string PERFORMANCE_OPT_START = "Phase 9: Initializing advanced performance optimization...";

        /// <summary>Performance optimization complete message</summary>
        public const string PERFORMANCE_OPT_COMPLETE = "Phase 9: Advanced performance optimization initialized (authentication required)";

        // ===== ERROR MESSAGE TEMPLATES =====
        /// <summary>Critical logger failure template</summary>
        public const string CRITICAL_LOGGER_FAILURE_TEMPLATE = "[CRITICAL] Logger failed during startup: {0}";

        /// <summary>Critical initialization failure template</summary>
        public const string CRITICAL_INIT_FAILURE_TEMPLATE = "[CRITICAL] MixerThreholdMod initialization failed: {0}";

        /// <summary>Harmony console setup failure template</summary>
        public const string HARMONY_CONSOLE_FAILURE_TEMPLATE = "CRITICAL: Harmony/Console setup failed: {0}\n{1}";

        /// <summary>OnInitializeMelon failure template</summary>
        public const string ON_INIT_MELON_FAILURE_TEMPLATE = "CRITICAL: OnInitializeMelon failed: {0}\n{1}";

        /// <summary>Directory detection failure template</summary>
        public const string DIRECTORY_DETECTION_FAILURE_TEMPLATE = "[INIT] Game-based directory detection failed: {0}";

        /// <summary>Constructor not found error</summary>
        public const string CONSTRUCTOR_NOT_FOUND_ERROR = "CRITICAL: Target constructor NOT found! This may be due to IL2CPP type loading issues.";

        /// <summary>Limited functionality warning</summary>
        public const string LIMITED_FUNCTIONALITY_WARNING = "CRITICAL: Mod functionality will be limited but initialization will continue.";

        /// <summary>Queue instance null warning</summary>
        public const string QUEUE_INSTANCE_NULL_WARNING = "QueueInstance: Received null instance - ignoring";

        /// <summary>Queue instance failure template</summary>
        public const string QUEUE_INSTANCE_FAILURE_TEMPLATE = "QueueInstance: Critical failure - {0}\n{1}";

        /// <summary>Background processing failure template</summary>
        public const string BACKGROUND_PROCESSING_FAILURE_TEMPLATE = "OnUpdate background processing: Caught exception: {0}\n{1}";

        /// <summary>OnUpdate failure template</summary>
        public const string ON_UPDATE_FAILURE_TEMPLATE = "OnUpdate: Caught exception: {0}\n{1}";

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

        // ===== PROCESSING MESSAGE TEMPLATES =====
        /// <summary>Queue instance success template</summary>
        public const string QUEUE_INSTANCE_SUCCESS_TEMPLATE = "QueueInstance: Successfully queued MixingStationConfiguration (Total: {0})";

        /// <summary>Process cleanup start message</summary>
        public const string PROCESS_CLEANUP_START = "ProcessQueuedInstancesAsync: Starting cleanup and processing";

        /// <summary>Process instances template</summary>
        public const string PROCESS_INSTANCES_TEMPLATE = "ProcessQueuedInstancesAsync: Processing {0} queued instances";

        /// <summary>Process null instance skip message</summary>
        public const string PROCESS_NULL_INSTANCE_SKIP = "ProcessQueuedInstancesAsync: Skipping null instance";

        /// <summary>Instance already tracked template</summary>
        public const string INSTANCE_ALREADY_TRACKED_TEMPLATE = "Instance already tracked — skipping duplicate: {0}";

        /// <summary>StartThrehold property not found message</summary>
        public const string START_THREHOLD_NOT_FOUND = "StartThrehold property not found for instance. Skipping.";

        /// <summary>StartThrehold null message</summary>
        public const string START_THREHOLD_NULL = "StartThrehold is null for instance. Skipping for now.";

        /// <summary>New mixer configuration message</summary>
        public const string NEW_MIXER_CONFIG = "ProcessQueuedInstancesAsync: Configuring new mixer...";

        /// <summary>Mixer configured success message</summary>
        public const string MIXER_CONFIGURED_SUCCESS = "ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)";

        /// <summary>Configure method not found message</summary>
        public const string CONFIGURE_METHOD_NOT_FOUND = "Configure method not found on StartThrehold. Skipping configuration.";

        /// <summary>Mixer created template</summary>
        public const string MIXER_CREATED_TEMPLATE = "Created mixer with Stable ID: {0}";

        /// <summary>Attaching listener template</summary>
        public const string ATTACHING_LISTENER_TEMPLATE = "Attaching listener for Mixer {0}";

        /// <summary>Restoring mixer template</summary>
        public const string RESTORING_MIXER_TEMPLATE = "Restoring Mixer {0} to {1}";

        /// <summary>SetValue method not found message</summary>
        public const string SET_VALUE_METHOD_NOT_FOUND = "SetValue method not found on StartThrehold. Cannot restore saved value.";

        /// <summary>Process complete success message</summary>
        public const string PROCESS_COMPLETE_SUCCESS = "ProcessQueuedInstancesAsync: Completed successfully";

        /// <summary>Process critical failure template</summary>
        public const string PROCESS_CRITICAL_FAILURE_TEMPLATE = "ProcessQueuedInstancesAsync: Critical failure: {0}\n{1}";

        /// <summary>Queue clearing failed message</summary>
        public const string QUEUE_CLEARING_FAILED = "ProcessQueuedInstancesAsync: Even queue clearing failed!";

        /// <summary>Individual mixer error template</summary>
        public const string INDIVIDUAL_MIXER_ERROR_TEMPLATE = "Error configuring individual mixer: {0}\n{1}";

        /// <summary>Individual instance error template</summary>
        public const string INDIVIDUAL_INSTANCE_ERROR_TEMPLATE = "Error processing individual instance: {0}\n{1}";

        // ===== MIXER EXISTS MESSAGE TEMPLATES =====
        /// <summary>Mixer exists check template</summary>
        public const string MIXER_EXISTS_CHECK_TEMPLATE = "MixerExists: Checking for mixer ID {0}";

        /// <summary>Invalid mixer ID template</summary>
        public const string INVALID_MIXER_ID_TEMPLATE = "MixerExists: Invalid mixer ID {0} - must be positive integer";

        /// <summary>Mixer exists result template</summary>
        public const string MIXER_EXISTS_RESULT_TEMPLATE = "MixerExists: Mixer ID {0} exists: {1} (checked in {2:F1}ms)";

        /// <summary>Mixer exists timeout template</summary>
        public const string MIXER_EXISTS_TIMEOUT_TEMPLATE = "MixerExists: Timeout after 2 seconds checking mixer ID {0}";

        /// <summary>Mixer exists crash prevention template</summary>
        public const string MIXER_EXISTS_CRASH_PREVENTION_TEMPLATE = "MixerExists: CRASH PREVENTION - Operation failed for mixer ID {0}: {1} (took {2:F1}ms)";

        /// <summary>Mixer exists performance warning template</summary>
        public const string MIXER_EXISTS_PERFORMANCE_WARNING_TEMPLATE = "MixerExists: PERFORMANCE WARNING - Operation took {0:F1}ms for mixer ID {1} (expected <10ms)";

        /// <summary>Mixer exists slow operation template</summary>
        public const string MIXER_EXISTS_SLOW_OPERATION_TEMPLATE = "MixerExists: Slow operation - {0:F1}ms for mixer ID {1}";

        /// <summary>Mixer exists inner operation failure template</summary>
        public const string MIXER_EXISTS_INNER_FAILURE_TEMPLATE = "MixerExists: Inner operation failed: {0}";

        // ===== SCENE AND CONSOLE MESSAGE TEMPLATES =====
        /// <summary>Scene loaded template</summary>
        public const string SCENE_LOADED_TEMPLATE = "Scene loaded: {0}";

        /// <summary>Console command test start message</summary>
        public const string CONSOLE_COMMAND_TEST_START = "[DEBUG] Running one-time console command test...";

        /// <summary>Console command testing message</summary>
        public const string CONSOLE_COMMAND_TESTING = "[DEBUG] Testing console commands...";

        /// <summary>Console command test complete message</summary>
        public const string CONSOLE_COMMAND_TEST_COMPLETE = "[DEBUG] Console command test completed - check logs above for test output";

        /// <summary>Console command test failure template</summary>
        public const string CONSOLE_COMMAND_TEST_FAILURE_TEMPLATE = "[DEBUG] Console command test failed: {0}";

        /// <summary>Console test command message</summary>
        public const string CONSOLE_TEST_MSG_COMMAND = "msg This is a test message from console command";

        /// <summary>Console test warning command</summary>
        public const string CONSOLE_TEST_WARN_COMMAND = "warn This is a test warning from console command";

        /// <summary>Current save path template</summary>
        public const string CURRENT_SAVE_PATH_TEMPLATE = "Current Save Path at scene load: {0}";

        /// <summary>Background file copy failure template</summary>
        public const string BACKGROUND_FILE_COPY_FAILURE_TEMPLATE = "Background file copy failed: {0}";

        /// <summary>OnSceneWasLoaded failure template</summary>
        public const string ON_SCENE_LOADED_FAILURE_TEMPLATE = "OnSceneWasLoaded: Caught exception: {0}\n{1}";

        /// <summary>StartLoadCoroutine failure template</summary>
        public const string START_LOAD_COROUTINE_FAILURE_TEMPLATE = "StartLoadCoroutine: Caught exception: {0}";

        // ===== MONITORING AND PROFILING MESSAGE TEMPLATES =====
        /// <summary>Comprehensive monitoring start template</summary>
        public const string COMPREHENSIVE_MONITORING_START_TEMPLATE = "[CONSOLE] Starting comprehensive save monitoring: {0} iterations, {1:F3}s delay, bypass: {2}";

        /// <summary>Monitor iteration start template</summary>
        public const string MONITOR_ITERATION_START_TEMPLATE = "[MONITOR] Iteration {0}/{1} - Starting save operation";

        /// <summary>Monitor iteration complete template</summary>
        public const string MONITOR_ITERATION_COMPLETE_TEMPLATE = "[MONITOR] Iteration {0}/{1} completed in {2:F1}ms";

        /// <summary>Monitor iteration failed template</summary>
        public const string MONITOR_ITERATION_FAILED_TEMPLATE = "[MONITOR] Iteration {0}/{1} FAILED: {2}";

        /// <summary>Monitor waiting template</summary>
        public const string MONITOR_WAITING_TEMPLATE = "[MONITOR] Waiting {0:F3}s before next iteration...";

        /// <summary>Monitor complete template</summary>
        public const string MONITOR_COMPLETE_TEMPLATE = "[MONITOR] Comprehensive monitoring complete: {0} success, {1} failures in {2:F1}s";

        /// <summary>Monitor average time template</summary>
        public const string MONITOR_AVERAGE_TIME_TEMPLATE = "[MONITOR] Average time per operation: {0:F1}ms";

        /// <summary>Monitor success rate template</summary>
        public const string MONITOR_SUCCESS_RATE_TEMPLATE = "[MONITOR] Success rate: {0:F1}%";

        /// <summary>Transaction start message</summary>
        public const string TRANSACTION_START_MESSAGE = "[CONSOLE] Starting atomic transactional save operation";

        /// <summary>Transaction performing message</summary>
        public const string TRANSACTION_PERFORMING_MESSAGE = "[TRANSACTION] Performing save operation...";

        /// <summary>Transaction success template</summary>
        public const string TRANSACTION_SUCCESS_TEMPLATE = "[TRANSACTION] Transactional save completed successfully in {0:F1}ms";

        /// <summary>Transaction performance template</summary>
        public const string TRANSACTION_PERFORMANCE_TEMPLATE = "[TRANSACTION] Performance: {0:F3} saves/second";

        /// <summary>Transaction failed template</summary>
        public const string TRANSACTION_FAILED_TEMPLATE = "[TRANSACTION] Transactional save FAILED: {0}";

        /// <summary>Transaction backup check message</summary>
        public const string TRANSACTION_BACKUP_CHECK = "[TRANSACTION] Check backup files for recovery if needed";

        /// <summary>Profile start message</summary>
        public const string PROFILE_START_MESSAGE = "[CONSOLE] Starting advanced save operation profiling";

        /// <summary>Profile phase 1 message</summary>
        public const string PROFILE_PHASE1_MESSAGE = "[PROFILE] Phase 1: Pre-save diagnostics";

        /// <summary>Profile phase 2 message</summary>
        public const string PROFILE_PHASE2_MESSAGE = "[PROFILE] Phase 2: Save operation profiling";

        /// <summary>Profile phase 3 message</summary>
        public const string PROFILE_PHASE3_MESSAGE = "[PROFILE] Phase 3: Post-save diagnostics";

        /// <summary>Profile save path template</summary>
        public const string PROFILE_SAVE_PATH_TEMPLATE = "[PROFILE] Current save path: {0}";

        /// <summary>Profile mixer count template</summary>
        public const string PROFILE_MIXER_COUNT_TEMPLATE = "[PROFILE] Mixer count: {0}";

        /// <summary>Profile memory usage template</summary>
        public const string PROFILE_MEMORY_USAGE_TEMPLATE = "[PROFILE] Memory usage: {0} KB";

        /// <summary>Profile final memory template</summary>
        public const string PROFILE_FINAL_MEMORY_TEMPLATE = "[PROFILE] Final memory usage: {0} KB";

        /// <summary>Profile mixer count after save template</summary>
        public const string PROFILE_MIXER_COUNT_AFTER_TEMPLATE = "[PROFILE] Mixer count after save: {0}";

        /// <summary>Profile phase complete template</summary>
        public const string PROFILE_PHASE_COMPLETE_TEMPLATE = "[PROFILE] Phase {0} completed in {1:F1}ms";

        /// <summary>Profile complete template</summary>
        public const string PROFILE_COMPLETE_TEMPLATE = "[PROFILE] Advanced profiling complete. Total time: {0:F1}ms";

        /// <summary>Profile breakdown template</summary>
        public const string PROFILE_BREAKDOWN_TEMPLATE = "[PROFILE] Breakdown: PreSave={0:F1}ms, Save={1:F1}ms, PostSave={2:F1}ms";

        /// <summary>Profile performance template</summary>
        public const string PROFILE_PERFORMANCE_TEMPLATE = "[PROFILE] Performance: {0:F3} saves/second";

        /// <summary>Profile overhead template</summary>
        public const string PROFILE_OVERHEAD_TEMPLATE = "[PROFILE] Overhead ratio: {0:F1}% (diagnostics vs save time)";

        /// <summary>Profile phase 1 failed template</summary>
        public const string PROFILE_PHASE1_FAILED_TEMPLATE = "[PROFILE] Advanced profiling FAILED in Phase 1: {0}";

        /// <summary>Profile phase 2/3 failed template</summary>
        public const string PROFILE_PHASE23_FAILED_TEMPLATE = "[PROFILE] Advanced profiling FAILED in Phase 2/3: {0}";

        /// <summary>Profile phase 1 error template</summary>
        public const string PROFILE_PHASE1_ERROR_TEMPLATE = "[PROFILE] Advanced profiling had Phase 1 error: {0}";

        // ===== APPLICATION LIFECYCLE MESSAGE TEMPLATES =====
        /// <summary>Application shutdown message</summary>
        public const string APPLICATION_SHUTDOWN_MESSAGE = "[MAIN] Application shutting down - cleaning up resources";

        /// <summary>Cleanup complete message</summary>
        public const string CLEANUP_COMPLETE_MESSAGE = "[MAIN] Cleanup completed successfully";

        /// <summary>Cleanup error template</summary>
        public const string CLEANUP_ERROR_TEMPLATE = "[MAIN] Cleanup error: {0}";

        // ===== INITIALIZATION DETAIL MESSAGE TEMPLATES =====
        /// <summary>Game directory detection template</summary>
        public const string GAME_DIRECTORY_DETECTION_TEMPLATE = "[INIT] 🚀 Game-based directory detection: {0}";

        /// <summary>MelonLoader log ready template</summary>
        public const string MELON_LOADER_LOG_READY_TEMPLATE = "[INIT] 📄 MelonLoader log ready for management: {0}";

        /// <summary>Game saves directory template</summary>
        public const string GAME_SAVES_DIRECTORY_TEMPLATE = "[INIT] 💾 Game saves directory: {0}";

        // ===== FILE OPERATION MESSAGE TEMPLATES =====
        /// <summary>File copy success template</summary>
        public const string FILE_COPY_SUCCESS_TEMPLATE = "Copied {0} from persistent to save folder (Read: {1:F1}ms, Write: {2:F1}ms, Total: {3:F1}ms)";

        // ===== SAVE LOADING CONSTANTS =====
        /// <summary>Save load starting log template</summary>
        public const string SAVE_LOAD_STARTING_TEMPLATE = "[SAVE] LoadMixerValuesWhenReady: Starting load process";

        /// <summary>Save path available template</summary>
        public const string SAVE_PATH_AVAILABLE_TEMPLATE = "[SAVE] LoadMixerValuesWhenReady: Save path available: {0}";

        /// <summary>Loading from file template</summary>
        public const string LOADING_FROM_FILE_TEMPLATE = "[SAVE] LoadMixerValuesWhenReady: Loading from {0}";

        /// <summary>Successfully loaded values template</summary>
        public const string SUCCESSFULLY_LOADED_VALUES_TEMPLATE = "[SAVE] LoadMixerValuesWhenReady: Successfully loaded {0} mixer values";

        /// <summary>No save files found message</summary>
        public const string NO_SAVE_FILES_FOUND = "[SAVE] LoadMixerValuesWhenReady: No save files found - starting fresh";

        /// <summary>Load timeout waiting message</summary>
        public const string LOAD_TIMEOUT_WAITING = "[SAVE] LoadMixerValuesWhenReady: Timeout waiting for save path - using emergency defaults";

        /// <summary>Load completed message</summary>
        public const string LOAD_COMPLETED = "[SAVE] LoadMixerValuesWhenReady: Completed";

        /// <summary>Load crash prevention template</summary>
        public const string LOAD_CRASH_PREVENTION_TEMPLATE = "[SAVE] LoadMixerValuesWhenReady CRASH PREVENTION: Load failed but continuing: {0}";

        // ===== LISTENER ATTACHMENT CONSTANTS =====
        /// <summary>Listener starting template</summary>
        public const string LISTENER_STARTING_TEMPLATE = "[SAVE] AttachListenerWhenReady: Starting for Mixer {0}";

        /// <summary>Listener timeout template</summary>
        public const string LISTENER_TIMEOUT_TEMPLATE = "[SAVE] AttachListenerWhenReady: Timeout - StartThreshold not available for Mixer {0}";

        /// <summary>StartThrehold access error template</summary>
        public const string START_THREHOLD_ACCESS_ERROR_TEMPLATE = "[SAVE] AttachListenerWhenReady: Error accessing StartThrehold: {0}";
    }
}