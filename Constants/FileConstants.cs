using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// File operation constants including paths, extensions, operations, and validations
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class FileConstants
    {
        #region File Extensions
        /// <summary>JSON file extension</summary>
        public const string JSON_EXTENSION = ".json";

        /// <summary>Backup file extension</summary>
        public const string BACKUP_EXTENSION = ".bak";

        /// <summary>Temporary file extension</summary>
        public const string TEMP_EXTENSION = ".tmp";

        /// <summary>Log file extension</summary>
        public const string LOG_EXTENSION = ".log";

        /// <summary>Configuration file extension</summary>
        public const string CONFIG_EXTENSION = ".config";

        /// <summary>Settings file extension</summary>
        public const string SETTINGS_EXTENSION = ".settings";

        /// <summary>Data file extension</summary>
        public const string DATA_EXTENSION = ".data";

        /// <summary>XML file extension</summary>
        public const string XML_EXTENSION = ".xml";

        /// <summary>Text file extension</summary>
        public const string TXT_EXTENSION = ".txt";

        /// <summary>Binary file extension</summary>
        public const string BIN_EXTENSION = ".bin";

        /// <summary>DLL file extension</summary>
        public const string DLL_EXTENSION = ".dll";

        /// <summary>EXE file extension</summary>
        public const string EXE_EXTENSION = ".exe";

        /// <summary>Archive file extension</summary>
        public const string ZIP_EXTENSION = ".zip";

        /// <summary>Compressed file extension</summary>
        public const string RAR_EXTENSION = ".rar";

        /// <summary>Image file extension PNG</summary>
        public const string PNG_EXTENSION = ".png";

        /// <summary>Image file extension JPG</summary>
        public const string JPG_EXTENSION = ".jpg";

        /// <summary>Image file extension JPEG</summary>
        public const string JPEG_EXTENSION = ".jpeg";

        /// <summary>Audio file extension WAV</summary>
        public const string WAV_EXTENSION = ".wav";

        /// <summary>Audio file extension MP3</summary>
        public const string MP3_EXTENSION = ".mp3";

        /// <summary>Video file extension MP4</summary>
        public const string MP4_EXTENSION = ".mp4";

        /// <summary>Video file extension AVI</summary>
        public const string AVI_EXTENSION = ".avi";
        #endregion

        #region Directory Names
        /// <summary>Backup directory name</summary>
        public const string BACKUP_DIRECTORY = "Backups";

        /// <summary>Logs directory name</summary>
        public const string LOGS_DIRECTORY = "Logs";

        /// <summary>Config directory name</summary>
        public const string CONFIG_DIRECTORY = "Config";

        /// <summary>Data directory name</summary>
        public const string DATA_DIRECTORY = "Data";

        /// <summary>Temp directory name</summary>
        public const string TEMP_DIRECTORY = "Temp";

        /// <summary>Cache directory name</summary>
        public const string CACHE_DIRECTORY = "Cache";

        /// <summary>Plugins directory name</summary>
        public const string PLUGINS_DIRECTORY = "Plugins";

        /// <summary>Mods directory name</summary>
        public const string MODS_DIRECTORY = "Mods";

        /// <summary>Settings directory name</summary>
        public const string SETTINGS_DIRECTORY = "Settings";

        /// <summary>Presets directory name</summary>
        public const string PRESETS_DIRECTORY = "Presets";
        #endregion

        #region File Operations
        /// <summary>File operation: Read</summary>
        public const string FILE_OP_READ = "Read";

        /// <summary>File operation: Write</summary>
        public const string FILE_OP_WRITE = "Write";

        /// <summary>File operation: Append</summary>
        public const string FILE_OP_APPEND = "Append";

        /// <summary>File operation: Delete</summary>
        public const string FILE_OP_DELETE = "Delete";

        /// <summary>File operation: Copy</summary>
        public const string FILE_OP_COPY = "Copy";

        /// <summary>File operation: Move</summary>
        public const string FILE_OP_MOVE = "Move";

        /// <summary>File operation: Create</summary>
        public const string FILE_OP_CREATE = "Create";

        /// <summary>File operation: Backup</summary>
        public const string FILE_OP_BACKUP = "Backup";

        /// <summary>File operation: Restore</summary>
        public const string FILE_OP_RESTORE = "Restore";

        /// <summary>File operation: Validate</summary>
        public const string FILE_OP_VALIDATE = "Validate";
        #endregion

        #region File Patterns
        /// <summary>All files pattern</summary>
        public const string ALL_FILES_PATTERN = "*.*";

        /// <summary>JSON files pattern</summary>
        public const string JSON_FILES_PATTERN = "*.json";

        /// <summary>Log files pattern</summary>
        public const string LOG_FILES_PATTERN = "*.log";

        /// <summary>Backup files pattern</summary>
        public const string BACKUP_FILES_PATTERN = "*.bak";

        /// <summary>Temp files pattern</summary>
        public const string TEMP_FILES_PATTERN = "*.tmp";

        /// <summary>Config files pattern</summary>
        public const string CONFIG_FILES_PATTERN = "*.config";

        /// <summary>DLL files pattern</summary>
        public const string DLL_FILES_PATTERN = "*.dll";

        /// <summary>Executable files pattern</summary>
        public const string EXE_FILES_PATTERN = "*.exe";
        #endregion

        #region File Validation
        /// <summary>Maximum file size in bytes (100MB)</summary>
        public const long MAX_FILE_SIZE_BYTES = 104857600;

        /// <summary>Minimum file size in bytes (0 bytes)</summary>
        public const long MIN_FILE_SIZE_BYTES = 0;

        /// <summary>Maximum filename length</summary>
        public const int MAX_FILENAME_LENGTH = 255;

        /// <summary>Maximum path length</summary>
        public const int MAX_PATH_LENGTH = 260;

        /// <summary>Minimum filename length</summary>
        public const int MIN_FILENAME_LENGTH = 1;

        /// <summary>File buffer size for reading (4KB)</summary>
        public const int FILE_BUFFER_SIZE = 4096;

        /// <summary>Large file buffer size (64KB)</summary>
        public const int LARGE_FILE_BUFFER_SIZE = 65536;
        #endregion

        #region Path Separators
        /// <summary>Windows path separator</summary>
        public const string WINDOWS_PATH_SEPARATOR = "\\";

        /// <summary>Unix path separator</summary>
        public const string UNIX_PATH_SEPARATOR = "/";

        /// <summary>Path separator character</summary>
        public const char PATH_SEPARATOR_CHAR = '\\';

        /// <summary>Alternative path separator character</summary>
        public const char ALT_PATH_SEPARATOR_CHAR = '/';

        /// <summary>Volume separator character</summary>
        public const char VOLUME_SEPARATOR_CHAR = ':';

        /// <summary>Directory separator string</summary>
        public const string DIRECTORY_SEPARATOR = "\\";

        /// <summary>Current directory indicator</summary>
        public const string CURRENT_DIRECTORY = ".";

        /// <summary>Parent directory indicator</summary>
        public const string PARENT_DIRECTORY = "..";
        #endregion

        #region File Access Modes
        /// <summary>Read-only file access</summary>
        public const string FILE_ACCESS_READ = "Read";

        /// <summary>Write-only file access</summary>
        public const string FILE_ACCESS_WRITE = "Write";

        /// <summary>Read-write file access</summary>
        public const string FILE_ACCESS_READ_WRITE = "ReadWrite";

        /// <summary>Create file mode</summary>
        public const string FILE_MODE_CREATE = "Create";

        /// <summary>Create new file mode</summary>
        public const string FILE_MODE_CREATE_NEW = "CreateNew";

        /// <summary>Open file mode</summary>
        public const string FILE_MODE_OPEN = "Open";

        /// <summary>Open or create file mode</summary>
        public const string FILE_MODE_OPEN_OR_CREATE = "OpenOrCreate";

        /// <summary>Truncate file mode</summary>
        public const string FILE_MODE_TRUNCATE = "Truncate";

        /// <summary>Append file mode</summary>
        public const string FILE_MODE_APPEND = "Append";
        #endregion
    }
}