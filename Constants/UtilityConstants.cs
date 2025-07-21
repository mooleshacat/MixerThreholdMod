using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Utility constants for common operations, string manipulations, mathematical calculations, and formatting
    /// âš ï¸ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// âš ï¸ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class UtilityConstants
    {
        #region String Manipulation Constants
        /// <summary>Empty string constant</summary>
        public const string EMPTY_STRING = "";

        /// <summary>Space character</summary>
        public const string SPACE = " ";

        /// <summary>Tab character</summary>
        public const string TAB = "\t";

        /// <summary>New line character</summary>
        public const string NEWLINE = "\n";

        /// <summary>Carriage return character</summary>
        public const string CARRIAGE_RETURN = "\r";

        /// <summary>Windows line ending</summary>
        public const string WINDOWS_LINE_ENDING = "\r\n";

        /// <summary>Unix line ending</summary>
        public const string UNIX_LINE_ENDING = "\n";

        /// <summary>Comma separator</summary>
        public const string COMMA_SEPARATOR = ",";

        /// <summary>Semicolon separator</summary>
        public const string SEMICOLON_SEPARATOR = ";";

        /// <summary>Pipe separator</summary>
        public const string PIPE_SEPARATOR = "|";

        /// <summary>Underscore separator</summary>
        public const string UNDERSCORE_SEPARATOR = "_";

        /// <summary>Dash separator</summary>
        public const string DASH_SEPARATOR = "-";

        /// <summary>Dot separator</summary>
        public const string DOT_SEPARATOR = ".";

        /// <summary>Colon separator</summary>
        public const string COLON_SEPARATOR = ":";

        /// <summary>Default string trimming characters</summary>
        public static readonly char[] DEFAULT_TRIM_CHARS = { ' ', '\t', '\n', '\r' };

        /// <summary>Whitespace characters array</summary>
        public static readonly char[] WHITESPACE_CHARS = { ' ', '\t', '\n', '\r', '\f', '\v' };
        #endregion

        #region Mathematical Constants
        /// <summary>Pi mathematical constant</summary>
        public const double PI = 3.14159265358979323846;

        /// <summary>E mathematical constant</summary>
        public const double E = 2.71828182845904523536;

        /// <summary>Golden ratio mathematical constant</summary>
        public const double GOLDEN_RATIO = 1.61803398874989484820;

        /// <summary>Square root of 2</summary>
        public const double SQRT_2 = 1.41421356237309504880;

        /// <summary>Square root of 3</summary>
        public const double SQRT_3 = 1.73205080756887729352;

        /// <summary>Natural logarithm of 2</summary>
        public const double LN_2 = 0.69314718055994530942;

        /// <summary>Natural logarithm of 10</summary>
        public const double LN_10 = 2.30258509299404568402;

        /// <summary>Degrees to radians conversion factor</summary>
        public const double DEGREES_TO_RADIANS = PI / 180.0;

        /// <summary>Radians to degrees conversion factor</summary>
        public const double RADIANS_TO_DEGREES = 180.0 / PI;

        /// <summary>Gravitational acceleration (m/sÂ²)</summary>
        public const double GRAVITY_ACCELERATION = 9.80665;

        /// <summary>Speed of light in vacuum (m/s)</summary>
        public const double SPEED_OF_LIGHT = 299792458.0;

        /// <summary>Planck constant (Jâ‹…s)</summary>
        public const double PLANCK_CONSTANT = 6.62607015e-34;
        #endregion

        #region Conversion Constants
        /// <summary>Bytes per kilobyte</summary>
        public const long BYTES_PER_KB = 1024;

        /// <summary>Bytes per megabyte</summary>
        public const long BYTES_PER_MB = 1024 * 1024;

        /// <summary>Bytes per gigabyte</summary>
        public const long BYTES_PER_GB = 1024 * 1024 * 1024;

        /// <summary>Bytes per terabyte</summary>
        public const long BYTES_PER_TB = 1024L * 1024L * 1024L * 1024L;

        /// <summary>Milliseconds per second</summary>
        public const int MS_PER_SECOND = 1000;

        /// <summary>Seconds per minute</summary>
        public const int SECONDS_PER_MINUTE = 60;

        /// <summary>Minutes per hour</summary>
        public const int MINUTES_PER_HOUR = 60;

        /// <summary>Hours per day</summary>
        public const int HOURS_PER_DAY = 24;

        /// <summary>Days per week</summary>
        public const int DAYS_PER_WEEK = 7;

        /// <summary>Months per year</summary>
        public const int MONTHS_PER_YEAR = 12;

        /// <summary>Inches per foot</summary>
        public const double INCHES_PER_FOOT = 12.0;

        /// <summary>Feet per yard</summary>
        public const double FEET_PER_YARD = 3.0;

        /// <summary>Centimeters per meter</summary>
        public const double CM_PER_METER = 100.0;

        /// <summary>Meters per kilometer</summary>
        public const double METERS_PER_KM = 1000.0;

        /// <summary>Celsius to Fahrenheit conversion offset</summary>
        public const double CELSIUS_TO_FAHRENHEIT_OFFSET = 32.0;

        /// <summary>Celsius to Fahrenheit conversion multiplier</summary>
        public const double CELSIUS_TO_FAHRENHEIT_MULTIPLIER = 1.8;
        #endregion

        #region Date and Time Constants
        /// <summary>Standard date format (ISO 8601)</summary>
        public const string DATE_FORMAT_ISO8601 = "yyyy-MM-ddTHH:mm:ss.fffZ";

        /// <summary>Short date format</summary>
        public const string DATE_FORMAT_SHORT = "yyyy-MM-dd";

        /// <summary>Long date format</summary>
        public const string DATE_FORMAT_LONG = "yyyy-MM-dd HH:mm:ss";

        /// <summary>Time only format</summary>
        public const string TIME_FORMAT_ONLY = "HH:mm:ss";

        /// <summary>12-hour time format</summary>
        public const string TIME_FORMAT_12HOUR = "hh:mm:ss tt";

        /// <summary>24-hour time format</summary>
        public const string TIME_FORMAT_24HOUR = "HH:mm:ss";

        /// <summary>Timestamp format with milliseconds</summary>
        public const string TIMESTAMP_FORMAT_MS = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>Filename safe timestamp format</summary>
        public const string FILENAME_TIMESTAMP_FORMAT = "yyyyMMdd_HHmmss";

        /// <summary>Log timestamp format</summary>
        public const string LOG_TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>UTC timestamp format</summary>
        public const string UTC_TIMESTAMP_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffZ";
        #endregion

        #region Numeric Formatting Constants
        /// <summary>Currency format string</summary>
        public const string CURRENCY_FORMAT = "C";

        /// <summary>Percentage format string</summary>
        public const string PERCENTAGE_FORMAT = "P";

        /// <summary>Number format with 2 decimal places</summary>
        public const string NUMBER_FORMAT_2_DECIMAL = "F2";

        /// <summary>Number format with 4 decimal places</summary>
        public const string NUMBER_FORMAT_4_DECIMAL = "F4";

        /// <summary>Integer format with leading zeros</summary>
        public const string INTEGER_FORMAT_LEADING_ZEROS = "D4";

        /// <summary>Exponential format</summary>
        public const string EXPONENTIAL_FORMAT = "E";

        /// <summary>Hexadecimal format uppercase</summary>
        public const string HEX_FORMAT_UPPER = "X";

        /// <summary>Hexadecimal format lowercase</summary>
        public const string HEX_FORMAT_LOWER = "x";

        /// <summary>Binary format prefix</summary>
        public const string BINARY_FORMAT_PREFIX = "0b";

        /// <summary>Hexadecimal format prefix</summary>
        public const string HEX_FORMAT_PREFIX = "0x";
        #endregion

        #region Culture Constants
        /// <summary>Invariant culture name</summary>
        public const string CULTURE_INVARIANT = "";

        /// <summary>English (US) culture name</summary>
        public const string CULTURE_EN_US = "en-US";

        /// <summary>English (UK) culture name</summary>
        public const string CULTURE_EN_GB = "en-GB";

        /// <summary>German culture name</summary>
        public const string CULTURE_DE_DE = "de-DE";

        /// <summary>French culture name</summary>
        public const string CULTURE_FR_FR = "fr-FR";

        /// <summary>Spanish culture name</summary>
        public const string CULTURE_ES_ES = "es-ES";

        /// <summary>Italian culture name</summary>
        public const string CULTURE_IT_IT = "it-IT";

        /// <summary>Japanese culture name</summary>
        public const string CULTURE_JA_JP = "ja-JP";

        /// <summary>Chinese (Simplified) culture name</summary>
        public const string CULTURE_ZH_CN = "zh-CN";

        /// <summary>Chinese (Traditional) culture name</summary>
        public const string CULTURE_ZH_TW = "zh-TW";

        /// <summary>Russian culture name</summary>
        public const string CULTURE_RU_RU = "ru-RU";

        /// <summary>Korean culture name</summary>
        public const string CULTURE_KO_KR = "ko-KR";

        /// <summary>Portuguese culture name</summary>
        public const string CULTURE_PT_PT = "pt-PT";

        /// <summary>Portuguese (Brazil) culture name</summary>
        public const string CULTURE_PT_BR = "pt-BR";
        #endregion

        #region Array and Collection Constants
        /// <summary>Empty array size</summary>
        public const int EMPTY_ARRAY_SIZE = 0;

        /// <summary>Default collection capacity</summary>
        public const int DEFAULT_COLLECTION_CAPACITY = 10;

        /// <summary>Large collection capacity</summary>
        public const int LARGE_COLLECTION_CAPACITY = 1000;

        /// <summary>Maximum recommended collection size</summary>
        public const int MAX_RECOMMENDED_COLLECTION_SIZE = 10000;

        /// <summary>Default dictionary capacity</summary>
        public const int DEFAULT_DICTIONARY_CAPACITY = 16;

        /// <summary>Default hash set capacity</summary>
        public const int DEFAULT_HASHSET_CAPACITY = 16;

        /// <summary>Array growth factor</summary>
        public const double ARRAY_GROWTH_FACTOR = 1.5;

        /// <summary>Collection resize threshold</summary>
        public const double COLLECTION_RESIZE_THRESHOLD = 0.75;
        #endregion

        #region Boolean Constants
        /// <summary>True string representation</summary>
        public const string TRUE_STRING = "True";

        /// <summary>False string representation</summary>
        public const string FALSE_STRING = "False";

        /// <summary>Yes string representation</summary>
        public const string YES_STRING = "Yes";

        /// <summary>No string representation</summary>
        public const string NO_STRING = "No";

        /// <summary>On string representation</summary>
        public const string ON_STRING = "On";

        /// <summary>Off string representation</summary>
        public const string OFF_STRING = "Off";

        /// <summary>Enabled string representation</summary>
        public const string ENABLED_STRING = "Enabled";

        /// <summary>Disabled string representation</summary>
        public const string DISABLED_STRING = "Disabled";

        /// <summary>Active string representation</summary>
        public const string ACTIVE_STRING = "Active";

        /// <summary>Inactive string representation</summary>
        public const string INACTIVE_STRING = "Inactive";
        #endregion

        #region Special Character Constants
        /// <summary>Null character</summary>
        public const char NULL_CHAR = '\0';

        /// <summary>Bell character</summary>
        public const char BELL_CHAR = '\a';

        /// <summary>Backspace character</summary>
        public const char BACKSPACE_CHAR = '\b';

        /// <summary>Form feed character</summary>
        public const char FORM_FEED_CHAR = '\f';

        /// <summary>Vertical tab character</summary>
        public const char VERTICAL_TAB_CHAR = '\v';

        /// <summary>Escape character</summary>
        public const char ESCAPE_CHAR = '\x1B';

        /// <summary>Delete character</summary>
        public const char DELETE_CHAR = '\x7F';

        /// <summary>Quotation mark character</summary>
        public const char QUOTE_CHAR = '"';

        /// <summary>Apostrophe character</summary>
        public const char APOSTROPHE_CHAR = '\'';

        /// <summary>Backslash character</summary>
        public const char BACKSLASH_CHAR = '\\';

        /// <summary>Forward slash character</summary>
        public const char FORWARD_SLASH_CHAR = '/';

        /// <summary>At symbol character</summary>
        public const char AT_SYMBOL_CHAR = '@';

        /// <summary>Hash symbol character</summary>
        public const char HASH_SYMBOL_CHAR = '#';

        /// <summary>Dollar symbol character</summary>
        public const char DOLLAR_SYMBOL_CHAR = '$';

        /// <summary>Percent symbol character</summary>
        public const char PERCENT_SYMBOL_CHAR = '%';

        /// <summary>Ampersand character</summary>
        public const char AMPERSAND_CHAR = '&';

        /// <summary>Asterisk character</summary>
        public const char ASTERISK_CHAR = '*';

        /// <summary>Plus character</summary>
        public const char PLUS_CHAR = '+';

        /// <summary>Minus character</summary>
        public const char MINUS_CHAR = '-';

        /// <summary>Equals character</summary>
        public const char EQUALS_CHAR = '=';

        /// <summary>Question mark character</summary>
        public const char QUESTION_MARK_CHAR = '?';

        /// <summary>Exclamation mark character</summary>
        public const char EXCLAMATION_MARK_CHAR = '!';

        /// <summary>Tilde character</summary>
        public const char TILDE_CHAR = '~';

        /// <summary>Grave accent character</summary>
        public const char GRAVE_ACCENT_CHAR = '`';

        /// <summary>Caret character</summary>
        public const char CARET_CHAR = '^';

        /// <summary>Left parenthesis character</summary>
        public const char LEFT_PAREN_CHAR = '(';

        /// <summary>Right parenthesis character</summary>
        public const char RIGHT_PAREN_CHAR = ')';

        /// <summary>Left bracket character</summary>
        public const char LEFT_BRACKET_CHAR = '[';

        /// <summary>Right bracket character</summary>
        public const char RIGHT_BRACKET_CHAR = ']';

        /// <summary>Left brace character</summary>
        public const char LEFT_BRACE_CHAR = '{';

        /// <summary>Right brace character</summary>
        public const char RIGHT_BRACE_CHAR = '}';

        /// <summary>Less than character</summary>
        public const char LESS_THAN_CHAR = '<';

        /// <summary>Greater than character</summary>
        public const char GREATER_THAN_CHAR = '>';
        #endregion
    }
}