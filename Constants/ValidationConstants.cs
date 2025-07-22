using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Validation and data integrity constants for input validation, data verification, and integrity checks
    ///  IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    ///  THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class ValidationConstants
    {
        #region Validation Rules
        /// <summary>Minimum string length for validation</summary>
        public const int MIN_STRING_LENGTH = 1;

        /// <summary>Maximum string length for validation</summary>
        public const int MAX_STRING_LENGTH = 1000;

        /// <summary>Minimum numeric value for validation</summary>
        public const double MIN_NUMERIC_VALUE = double.MinValue;

        /// <summary>Maximum numeric value for validation</summary>
        public const double MAX_NUMERIC_VALUE = double.MaxValue;

        /// <summary>Default decimal precision for validation</summary>
        public const int DEFAULT_DECIMAL_PRECISION = 2;

        /// <summary>Maximum decimal precision for validation</summary>
        public const int MAX_DECIMAL_PRECISION = 10;

        /// <summary>Minimum array length for validation</summary>
        public const int MIN_ARRAY_LENGTH = 0;

        /// <summary>Maximum array length for validation</summary>
        public const int MAX_ARRAY_LENGTH = 10000;

        /// <summary>Email maximum length for validation</summary>
        public const int EMAIL_MAX_LENGTH = 254;

        /// <summary>URL maximum length for validation</summary>
        public const int URL_MAX_LENGTH = 2048;

        /// <summary>Password minimum length for validation</summary>
        public const int PASSWORD_MIN_LENGTH = 8;

        /// <summary>Password maximum length for validation</summary>
        public const int PASSWORD_MAX_LENGTH = 128;
        #endregion

        #region Regular Expression Patterns
        /// <summary>Email validation regex pattern</summary>
        public const string EMAIL_REGEX_PATTERN = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        /// <summary>URL validation regex pattern</summary>
        public const string URL_REGEX_PATTERN = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";

        /// <summary>Phone number validation regex pattern</summary>
        public const string PHONE_REGEX_PATTERN = @"^\+?[1-9]\d{1,14}$";

        /// <summary>Alphanumeric only regex pattern</summary>
        public const string ALPHANUMERIC_REGEX_PATTERN = @"^[a-zA-Z0-9]+$";

        /// <summary>Numeric only regex pattern</summary>
        public const string NUMERIC_REGEX_PATTERN = @"^[0-9]+$";

        /// <summary>Alphabetic only regex pattern</summary>
        public const string ALPHABETIC_REGEX_PATTERN = @"^[a-zA-Z]+$";

        /// <summary>Strong password regex pattern</summary>
        public const string STRONG_PASSWORD_REGEX_PATTERN = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        /// <summary>IPv4 address regex pattern</summary>
        public const string IPV4_REGEX_PATTERN = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

        /// <summary>IPv6 address regex pattern</summary>
        public const string IPV6_REGEX_PATTERN = @"^(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$";

        /// <summary>Hexadecimal color code regex pattern</summary>
        public const string HEX_COLOR_REGEX_PATTERN = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

        /// <summary>GUID regex pattern</summary>
        public const string GUID_REGEX_PATTERN = @"^[{(]?[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}[)}]?$";

        /// <summary>Version number regex pattern</summary>
        public const string VERSION_REGEX_PATTERN = @"^\d+\.\d+(\.\d+)?(\.\d+)?$";

        /// <summary>File name validation regex pattern</summary>
        public const string FILENAME_REGEX_PATTERN = @"^[^<>:""/\\|?*]+$";

        /// <summary>Windows path validation regex pattern</summary>
        public const string WINDOWS_PATH_REGEX_PATTERN = @"^[a-zA-Z]:\\(?:[^<>:""/\\|?*]+\\)*[^<>:""/\\|?*]*$";

        /// <summary>Unix path validation regex pattern</summary>
        public const string UNIX_PATH_REGEX_PATTERN = @"^\/(?:[^\/\0]+\/)*[^\/\0]*$";

        /// <summary>Credit card number regex pattern</summary>
        public const string CREDIT_CARD_REGEX_PATTERN = @"^\d{4}[\s-]?\d{4}[\s-]?\d{4}[\s-]?\d{4}$";
        #endregion

        #region Validation Messages
        /// <summary>Required field validation message</summary>
        public const string VALIDATION_REQUIRED = "This field is required";

        /// <summary>Invalid format validation message</summary>
        public const string VALIDATION_INVALID_FORMAT = "Invalid format";

        /// <summary>Length validation message template</summary>
        public const string VALIDATION_LENGTH_TEMPLATE = "Length must be between {0} and {1} characters";

        /// <summary>Range validation message template</summary>
        public const string VALIDATION_RANGE_TEMPLATE = "Value must be between {0} and {1}";

        /// <summary>Email validation message</summary>
        public const string VALIDATION_INVALID_EMAIL = "Invalid email address";

        /// <summary>URL validation message</summary>
        public const string VALIDATION_INVALID_URL = "Invalid URL format";

        /// <summary>Phone validation message</summary>
        public const string VALIDATION_INVALID_PHONE = "Invalid phone number";

        /// <summary>Password strength validation message</summary>
        public const string VALIDATION_WEAK_PASSWORD = "Password must contain uppercase, lowercase, number, and special character";

        /// <summary>Date validation message</summary>
        public const string VALIDATION_INVALID_DATE = "Invalid date format";

        /// <summary>Numeric validation message</summary>
        public const string VALIDATION_NOT_NUMERIC = "Value must be numeric";

        /// <summary>File size validation message</summary>
        public const string VALIDATION_FILE_SIZE = "File size exceeds maximum allowed";

        /// <summary>File type validation message</summary>
        public const string VALIDATION_FILE_TYPE = "Invalid file type";

        /// <summary>Duplicate value validation message</summary>
        public const string VALIDATION_DUPLICATE = "Value already exists";

        /// <summary>Custom validation message</summary>
        public const string VALIDATION_CUSTOM = "Custom validation failed";
        #endregion

        #region Data Integrity Constants
        /// <summary>Checksum validation enabled</summary>
        public const bool CHECKSUM_VALIDATION_ENABLED = true;

        /// <summary>Hash algorithm for integrity checking</summary>
        public const string INTEGRITY_HASH_ALGORITHM = "SHA256";

        /// <summary>Backup integrity check enabled</summary>
        public const bool BACKUP_INTEGRITY_CHECK_ENABLED = true;

        /// <summary>Data corruption detection enabled</summary>
        public const bool CORRUPTION_DETECTION_ENABLED = true;

        /// <summary>Maximum validation attempts</summary>
        public const int MAX_VALIDATION_ATTEMPTS = 3;

        /// <summary>Validation timeout in milliseconds</summary>
        public const int VALIDATION_TIMEOUT_MS = 5000;

        /// <summary>Integrity check interval in milliseconds</summary>
        public const int INTEGRITY_CHECK_INTERVAL_MS = 30000;
        #endregion

        #region Validation Types
        /// <summary>String validation type</summary>
        public const string VALIDATION_TYPE_STRING = "String";

        /// <summary>Numeric validation type</summary>
        public const string VALIDATION_TYPE_NUMERIC = "Numeric";

        /// <summary>Boolean validation type</summary>
        public const string VALIDATION_TYPE_BOOLEAN = "Boolean";

        /// <summary>Date validation type</summary>
        public const string VALIDATION_TYPE_DATE = "Date";

        /// <summary>Email validation type</summary>
        public const string VALIDATION_TYPE_EMAIL = "Email";

        /// <summary>URL validation type</summary>
        public const string VALIDATION_TYPE_URL = "URL";

        /// <summary>Phone validation type</summary>
        public const string VALIDATION_TYPE_PHONE = "Phone";

        /// <summary>Password validation type</summary>
        public const string VALIDATION_TYPE_PASSWORD = "Password";

        /// <summary>File validation type</summary>
        public const string VALIDATION_TYPE_FILE = "File";

        /// <summary>JSON validation type</summary>
        public const string VALIDATION_TYPE_JSON = "JSON";

        /// <summary>XML validation type</summary>
        public const string VALIDATION_TYPE_XML = "XML";

        /// <summary>Custom validation type</summary>
        public const string VALIDATION_TYPE_CUSTOM = "Custom";
        #endregion

        #region Validation Results
        /// <summary>Validation success result</summary>
        public const string VALIDATION_RESULT_SUCCESS = "Success";

        /// <summary>Validation failure result</summary>
        public const string VALIDATION_RESULT_FAILURE = "Failure";

        /// <summary>Validation error result</summary>
        public const string VALIDATION_RESULT_ERROR = "Error";

        /// <summary>Validation warning result</summary>
        public const string VALIDATION_RESULT_WARNING = "Warning";

        /// <summary>Validation skipped result</summary>
        public const string VALIDATION_RESULT_SKIPPED = "Skipped";

        /// <summary>Validation pending result</summary>
        public const string VALIDATION_RESULT_PENDING = "Pending";

        /// <summary>Validation timeout result</summary>
        public const string VALIDATION_RESULT_TIMEOUT = "Timeout";

        /// <summary>Validation cancelled result</summary>
        public const string VALIDATION_RESULT_CANCELLED = "Cancelled";
        #endregion
    }
}