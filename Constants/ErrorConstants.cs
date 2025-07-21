using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Error handling constants for exception management, error codes, and recovery strategies
    /// âš ï¸ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// âš ï¸ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class ErrorConstants
    {
        #region Error Codes
        /// <summary>Success error code</summary>
        public const int ERROR_CODE_SUCCESS = 0;

        /// <summary>General error code</summary>
        public const int ERROR_CODE_GENERAL = 1;

        /// <summary>File not found error code</summary>
        public const int ERROR_CODE_FILE_NOT_FOUND = 2;

        /// <summary>Access denied error code</summary>
        public const int ERROR_CODE_ACCESS_DENIED = 3;

        /// <summary>Invalid operation error code</summary>
        public const int ERROR_CODE_INVALID_OPERATION = 4;

        /// <summary>Timeout error code</summary>
        public const int ERROR_CODE_TIMEOUT = 5;

        /// <summary>Out of memory error code</summary>
        public const int ERROR_CODE_OUT_OF_MEMORY = 6;

        /// <summary>Invalid argument error code</summary>
        public const int ERROR_CODE_INVALID_ARGUMENT = 7;

        /// <summary>Operation cancelled error code</summary>
        public const int ERROR_CODE_OPERATION_CANCELLED = 8;

        /// <summary>Network error code</summary>
        public const int ERROR_CODE_NETWORK = 9;

        /// <summary>Serialization error code</summary>
        public const int ERROR_CODE_SERIALIZATION = 10;

        /// <summary>Validation error code</summary>
        public const int ERROR_CODE_VALIDATION = 11;

        /// <summary>Configuration error code</summary>
        public const int ERROR_CODE_CONFIGURATION = 12;

        /// <summary>Permission error code</summary>
        public const int ERROR_CODE_PERMISSION = 13;

        /// <summary>Data corruption error code</summary>
        public const int ERROR_CODE_DATA_CORRUPTION = 14;

        /// <summary>Version mismatch error code</summary>
        public const int ERROR_CODE_VERSION_MISMATCH = 15;
        #endregion

        #region Error Messages
        /// <summary>Generic error message</summary>
        public const string ERROR_MESSAGE_GENERIC = "An error occurred during operation";

        /// <summary>File operation error message</summary>
        public const string ERROR_MESSAGE_FILE_OPERATION = "File operation failed";

        /// <summary>Save operation error message</summary>
        public const string ERROR_MESSAGE_SAVE_OPERATION = "Save operation failed";

        /// <summary>Load operation error message</summary>
        public const string ERROR_MESSAGE_LOAD_OPERATION = "Load operation failed";

        /// <summary>Validation error message</summary>
        public const string ERROR_MESSAGE_VALIDATION = "Validation failed";

        /// <summary>Configuration error message</summary>
        public const string ERROR_MESSAGE_CONFIGURATION = "Configuration error";

        /// <summary>Network error message</summary>
        public const string ERROR_MESSAGE_NETWORK = "Network operation failed";

        /// <summary>Memory error message</summary>
        public const string ERROR_MESSAGE_MEMORY = "Memory allocation failed";

        /// <summary>Timeout error message</summary>
        public const string ERROR_MESSAGE_TIMEOUT = "Operation timed out";

        /// <summary>Cancellation error message</summary>
        public const string ERROR_MESSAGE_CANCELLATION = "Operation was cancelled";

        /// <summary>Permission error message</summary>
        public const string ERROR_MESSAGE_PERMISSION = "Permission denied";

        /// <summary>Data corruption error message</summary>
        public const string ERROR_MESSAGE_DATA_CORRUPTION = "Data corruption detected";

        /// <summary>Version mismatch error message</summary>
        public const string ERROR_MESSAGE_VERSION_MISMATCH = "Version mismatch detected";

        /// <summary>Resource unavailable error message</summary>
        public const string ERROR_MESSAGE_RESOURCE_UNAVAILABLE = "Resource is unavailable";

        /// <summary>Operation not supported error message</summary>
        public const string ERROR_MESSAGE_NOT_SUPPORTED = "Operation is not supported";
        #endregion

        #region Exception Types
        /// <summary>System exception type name</summary>
        public const string EXCEPTION_TYPE_SYSTEM = "SystemException";

        /// <summary>Argument exception type name</summary>
        public const string EXCEPTION_TYPE_ARGUMENT = "ArgumentException";

        /// <summary>Null reference exception type name</summary>
        public const string EXCEPTION_TYPE_NULL_REFERENCE = "NullReferenceException";

        /// <summary>Invalid operation exception type name</summary>
        public const string EXCEPTION_TYPE_INVALID_OPERATION = "InvalidOperationException";

        /// <summary>File not found exception type name</summary>
        public const string EXCEPTION_TYPE_FILE_NOT_FOUND = "FileNotFoundException";

        /// <summary>Unauthorized access exception type name</summary>
        public const string EXCEPTION_TYPE_UNAUTHORIZED_ACCESS = "UnauthorizedAccessException";

        /// <summary>Timeout exception type name</summary>
        public const string EXCEPTION_TYPE_TIMEOUT = "TimeoutException";

        /// <summary>Out of memory exception type name</summary>
        public const string EXCEPTION_TYPE_OUT_OF_MEMORY = "OutOfMemoryException";

        /// <summary>Operation cancelled exception type name</summary>
        public const string EXCEPTION_TYPE_OPERATION_CANCELLED = "OperationCanceledException";

        /// <summary>Not supported exception type name</summary>
        public const string EXCEPTION_TYPE_NOT_SUPPORTED = "NotSupportedException";
        #endregion

        #region Recovery Strategies
        /// <summary>Retry recovery strategy</summary>
        public const string RECOVERY_STRATEGY_RETRY = "Retry";

        /// <summary>Fallback recovery strategy</summary>
        public const string RECOVERY_STRATEGY_FALLBACK = "Fallback";

        /// <summary>Skip recovery strategy</summary>
        public const string RECOVERY_STRATEGY_SKIP = "Skip";

        /// <summary>Abort recovery strategy</summary>
        public const string RECOVERY_STRATEGY_ABORT = "Abort";

        /// <summary>Default recovery strategy</summary>
        public const string RECOVERY_STRATEGY_DEFAULT = "Default";

        /// <summary>Emergency save recovery strategy</summary>
        public const string RECOVERY_STRATEGY_EMERGENCY_SAVE = "EmergencySave";

        /// <summary>Backup restore recovery strategy</summary>
        public const string RECOVERY_STRATEGY_BACKUP_RESTORE = "BackupRestore";

        /// <summary>Reset to defaults recovery strategy</summary>
        public const string RECOVERY_STRATEGY_RESET_DEFAULTS = "ResetDefaults";
        #endregion

        #region Error Severity Levels
        /// <summary>Critical error severity</summary>
        public const string ERROR_SEVERITY_CRITICAL = "Critical";

        /// <summary>High error severity</summary>
        public const string ERROR_SEVERITY_HIGH = "High";

        /// <summary>Medium error severity</summary>
        public const string ERROR_SEVERITY_MEDIUM = "Medium";

        /// <summary>Low error severity</summary>
        public const string ERROR_SEVERITY_LOW = "Low";

        /// <summary>Warning severity</summary>
        public const string ERROR_SEVERITY_WARNING = "Warning";

        /// <summary>Information severity</summary>
        public const string ERROR_SEVERITY_INFO = "Information";
        #endregion

        #region Error Context
        /// <summary>Save context identifier</summary>
        public const string ERROR_CONTEXT_SAVE = "Save";

        /// <summary>Load context identifier</summary>
        public const string ERROR_CONTEXT_LOAD = "Load";

        /// <summary>Validation context identifier</summary>
        public const string ERROR_CONTEXT_VALIDATION = "Validation";

        /// <summary>Backup context identifier</summary>
        public const string ERROR_CONTEXT_BACKUP = "Backup";

        /// <summary>Initialization context identifier</summary>
        public const string ERROR_CONTEXT_INITIALIZATION = "Initialization";

        /// <summary>Configuration context identifier</summary>
        public const string ERROR_CONTEXT_CONFIGURATION = "Configuration";

        /// <summary>Network context identifier</summary>
        public const string ERROR_CONTEXT_NETWORK = "Network";

        /// <summary>Threading context identifier</summary>
        public const string ERROR_CONTEXT_THREADING = "Threading";

        /// <summary>Performance context identifier</summary>
        public const string ERROR_CONTEXT_PERFORMANCE = "Performance";

        /// <summary>Security context identifier</summary>
        public const string ERROR_CONTEXT_SECURITY = "Security";
        #endregion
    }
}