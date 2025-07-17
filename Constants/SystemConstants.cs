using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// System and configuration constants for Unity, IL2CPP, MelonLoader, and platform-specific settings
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class SystemConstants
    {
        #region Mod Information
        /// <summary>Mod name identifier</summary>
        public const string MOD_NAME = "MixerThreholdMod";

        /// <summary>Mod version</summary>
        public const string MOD_VERSION = "1.0.0";

        /// <summary>Mod author</summary>
        public const string MOD_AUTHOR = "MixerThresholdMod Team";

        /// <summary>Mod description</summary>
        public const string MOD_DESCRIPTION = "Advanced mixer threshold management mod";

        /// <summary>Mod namespace</summary>
        public const string MOD_NAMESPACE = "MixerThreholdMod_1_0_0";

        /// <summary>Mod assembly name</summary>
        public const string MOD_ASSEMBLY_NAME = "MixerThreholdMod-1_0_0";

        /// <summary>Mod GUID for MelonLoader</summary>
        public const string MOD_GUID = "com.mixerthresholdmod.1_0_0";
        #endregion

        #region Platform Constants
        /// <summary>Windows platform identifier</summary>
        public const string PLATFORM_WINDOWS = "Windows";

        /// <summary>Linux platform identifier</summary>
        public const string PLATFORM_LINUX = "Linux";

        /// <summary>macOS platform identifier</summary>
        public const string PLATFORM_MACOS = "macOS";

        /// <summary>Unity platform identifier</summary>
        public const string PLATFORM_UNITY = "Unity";

        /// <summary>IL2CPP backend identifier</summary>
        public const string BACKEND_IL2CPP = "IL2CPP";

        /// <summary>Mono backend identifier</summary>
        public const string BACKEND_MONO = "Mono";

        /// <summary>.NET Framework version</summary>
        public const string DOTNET_VERSION = "4.8.1";
        #endregion

        #region Assembly Information
        /// <summary>MelonLoader assembly name</summary>
        public const string MELONLOADER_ASSEMBLY = "MelonLoader";

        /// <summary>Unity Engine assembly name</summary>
        public const string UNITY_ENGINE_ASSEMBLY = "UnityEngine";

        /// <summary>Unity Core assembly name</summary>
        public const string UNITY_CORE_ASSEMBLY = "UnityEngine.CoreModule";

        /// <summary>System assembly name</summary>
        public const string SYSTEM_ASSEMBLY = "System";

        /// <summary>System Core assembly name</summary>
        public const string SYSTEM_CORE_ASSEMBLY = "System.Core";

        /// <summary>Newtonsoft JSON assembly name</summary>
        public const string NEWTONSOFT_JSON_ASSEMBLY = "Newtonsoft.Json";
        #endregion

        #region Version Information
        /// <summary>Minimum Unity version required</summary>
        public const string MIN_UNITY_VERSION = "2019.4.0";

        /// <summary>Minimum MelonLoader version required</summary>
        public const string MIN_MELONLOADER_VERSION = "0.5.7";

        /// <summary>Configuration version</summary>
        public const string CONFIG_VERSION = "1.0";

        /// <summary>Save format version</summary>
        public const string SAVE_FORMAT_VERSION = "1.0";

        /// <summary>API version</summary>
        public const string API_VERSION = "1.0.0";
        #endregion

        #region Environment Settings
        /// <summary>Development environment identifier</summary>
        public const string ENVIRONMENT_DEVELOPMENT = "Development";

        /// <summary>Production environment identifier</summary>
        public const string ENVIRONMENT_PRODUCTION = "Production";

        /// <summary>Testing environment identifier</summary>
        public const string ENVIRONMENT_TESTING = "Testing";

        /// <summary>Debug build configuration</summary>
        public const string BUILD_CONFIG_DEBUG = "Debug";

        /// <summary>Release build configuration</summary>
        public const string BUILD_CONFIG_RELEASE = "Release";
        #endregion

        #region Component Names
        /// <summary>SaveManager component name</summary>
        public const string COMPONENT_SAVE_MANAGER = "SaveManager";

        /// <summary>Logger component name</summary>
        public const string COMPONENT_LOGGER = "Logger";

        /// <summary>MixerValidation component name</summary>
        public const string COMPONENT_MIXER_VALIDATION = "MixerValidation";

        /// <summary>AtomicFileWriter component name</summary>
        public const string COMPONENT_ATOMIC_FILE_WRITER = "AtomicFileWriter";

        /// <summary>MixerDataBackupManager component name</summary>
        public const string COMPONENT_BACKUP_MANAGER = "MixerDataBackupManager";

        /// <summary>EmergencySaveManager component name</summary>
        public const string COMPONENT_EMERGENCY_SAVE = "EmergencySaveManager";

        /// <summary>ConsoleCommandHandler component name</summary>
        public const string COMPONENT_CONSOLE_COMMAND = "ConsoleCommandHandler";

        /// <summary>CancellableIoRunner component name</summary>
        public const string COMPONENT_IO_RUNNER = "CancellableIoRunner";

        /// <summary>PerformanceOptimizer component name</summary>
        public const string COMPONENT_PERFORMANCE_OPTIMIZER = "PerformanceOptimizer";

        /// <summary>DataIntegrityTracker component name</summary>
        public const string COMPONENT_DATA_INTEGRITY = "DataIntegrityTracker";
        #endregion

        #region Registry Keys
        /// <summary>Unity registry key</summary>
        public const string UNITY_REGISTRY_KEY = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Unity Technologies";

        /// <summary>Game registry key base</summary>
        public const string GAME_REGISTRY_KEY_BASE = "HKEY_CURRENT_USER\\SOFTWARE";

        /// <summary>Mod settings registry key</summary>
        public const string MOD_REGISTRY_KEY = "HKEY_CURRENT_USER\\SOFTWARE\\MixerThresholdMod";
        #endregion

        #region System Paths
        /// <summary>Unity installation path environment variable</summary>
        public const string UNITY_PATH_ENV_VAR = "UNITY_PATH";

        /// <summary>Game data path environment variable</summary>
        public const string GAME_DATA_PATH_ENV_VAR = "GAME_DATA_PATH";

        /// <summary>User profile path environment variable</summary>
        public const string USER_PROFILE_ENV_VAR = "USERPROFILE";

        /// <summary>AppData path environment variable</summary>
        public const string APPDATA_ENV_VAR = "APPDATA";

        /// <summary>LocalAppData path environment variable</summary>
        public const string LOCALAPPDATA_ENV_VAR = "LOCALAPPDATA";

        /// <summary>Program Files path environment variable</summary>
        public const string PROGRAM_FILES_ENV_VAR = "PROGRAMFILES";

        /// <summary>Program Files x86 path environment variable</summary>
        public const string PROGRAM_FILES_X86_ENV_VAR = "PROGRAMFILES(X86)";
        #endregion

        #region Configuration Keys
        /// <summary>Debug mode configuration key</summary>
        public const string CONFIG_DEBUG_MODE = "DebugMode";

        /// <summary>Verbose logging configuration key</summary>
        public const string CONFIG_VERBOSE_LOGGING = "VerboseLogging";

        /// <summary>Performance monitoring configuration key</summary>
        public const string CONFIG_PERFORMANCE_MONITORING = "PerformanceMonitoring";

        /// <summary>Auto backup configuration key</summary>
        public const string CONFIG_AUTO_BACKUP = "AutoBackup";

        /// <summary>Emergency save configuration key</summary>
        public const string CONFIG_EMERGENCY_SAVE = "EmergencySave";

        /// <summary>Validation enabled configuration key</summary>
        public const string CONFIG_VALIDATION_ENABLED = "ValidationEnabled";

        /// <summary>Thread safety mode configuration key</summary>
        public const string CONFIG_THREAD_SAFETY_MODE = "ThreadSafetyMode";

        /// <summary>Memory optimization configuration key</summary>
        public const string CONFIG_MEMORY_OPTIMIZATION = "MemoryOptimization";
        #endregion
    }
}