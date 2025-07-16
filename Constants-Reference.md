# MixerThreholdMod Constants Reference

This document provides a comprehensive reference for all constants defined in the MixerThreholdMod project.

**Generated**: 2025-07-16 16:55:48  
**Total Constants**: 1839  
**Files Scanned**: 15

## Table of Contents
- [All Constants](#all-constants) (129 constants)
- [Core Constants](#core-constants) (54 constants)
- [Error Constants](#error-constants) (65 constants)
- [File Constants](#file-constants) (73 constants)
- [Game Constants](#game-constants) (78 constants)
- [General Constants](#general-constants) (918 constants)
- [Logging Constants](#logging-constants) (51 constants)
- [Mixer Constants](#mixer-constants) (42 constants)
- [Network Constants](#network-constants) (90 constants)
- [Performance Constants](#performance-constants) (37 constants)
- [System Constants](#system-constants) (58 constants)
- [Threading Constants](#threading-constants) (48 constants)
- [Utility Constants](#utility-constants) (127 constants)
- [Validation Constants](#validation-constants) (69 constants)

---

## All Constants

### AllConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ALTERNATIVE_ACCESS` | string | `"AllConstants.[Domain].[CONSTANT_NAME]"` | _No description_ |
| `AUDIO_DEFAULT_VOLUME` | float | `GameConstants.AUDIO_DEFAULT_VOLUME` | _No description_ |
| `AUDIO_MASTER_VOLUME` | float | `GameConstants.AUDIO_MASTER_VOLUME` | Audio |
| `BACKEND_IL2CPP` | string | `SystemConstants.BACKEND_IL2CPP` | _No description_ |
| `BACKUP_CREATED_MESSAGE` | string | `LoggingConstants.BACKUP_CREATED_MESSAGE` | _No description_ |
| `BACKUP_DIRECTORY` | string | `FileConstants.BACKUP_DIRECTORY` | Directories |
| `BACKUP_EXTENSION` | string | `FileConstants.BACKUP_EXTENSION` | _No description_ |
| `BACKUP_THREAD_NAME` | string | `ThreadingConstants.BACKUP_THREAD_NAME` | _No description_ |
| `BYTES_PER_KB` | long | `UtilityConstants.BYTES_PER_KB` | Conversion |
| `BYTES_PER_MB` | long | `UtilityConstants.BYTES_PER_MB` | _No description_ |
| `COMMA_SEPARATOR` | string | `UtilityConstants.COMMA_SEPARATOR` | _No description_ |
| `COMPATIBILITY_ASSURANCE` | string | `".NET 4.8.1, MONO, IL2CPP compatible"` | _No description_ |
| `COMPONENT_BACKUP_MANAGER` | string | `SystemConstants.COMPONENT_BACKUP_MANAGER` | _No description_ |
| `COMPONENT_LOGGER` | string | `SystemConstants.COMPONENT_LOGGER` | _No description_ |
| `COMPONENT_SAVE_MANAGER` | string | `SystemConstants.COMPONENT_SAVE_MANAGER` | Components |
| `CONFIG_DIRECTORY` | string | `FileConstants.CONFIG_DIRECTORY` | _No description_ |
| `CONSOLE_COMMAND_DELAY_MS` | int | `PerformanceConstants.CONSOLE_COMMAND_DELAY_MS` | _No description_ |
| `CONTENT_TYPE_JSON` | string | `NetworkConstants.CONTENT_TYPE_JSON` | Content Types |
| `CONTENT_TYPE_TEXT` | string | `NetworkConstants.CONTENT_TYPE_TEXT` | _No description_ |
| `CONTENT_TYPE_XML` | string | `NetworkConstants.CONTENT_TYPE_XML` | _No description_ |
| `CULTURE_EN_US` | string | `UtilityConstants.CULTURE_EN_US` | Cultures |
| `CULTURE_INVARIANT` | string | `UtilityConstants.CULTURE_INVARIANT` | _No description_ |
| `DATE_FORMAT_ISO8601` | string | `UtilityConstants.DATE_FORMAT_ISO8601` | Date/Time Formats |
| `DEFAULT_CANCELLATION_TIMEOUT_MS` | int | `ThreadingConstants.DEFAULT_CANCELLATION_TIMEOUT_MS` | _No description_ |
| `DEFAULT_DECIMAL_PRECISION` | int | `ValidationConstants.DEFAULT_DECIMAL_PRECISION` | _No description_ |
| `DEFAULT_MIXER_CHANNEL` | int | `MixerConstants.DEFAULT_MIXER_CHANNEL` | _No description_ |
| `DEFAULT_MIXER_VOLUME` | float | `MixerConstants.DEFAULT_MIXER_VOLUME` | _No description_ |
| `DEGREES_TO_RADIANS` | double | `UtilityConstants.DEGREES_TO_RADIANS` | _No description_ |
| `DIRECTORY_RESOLVER_PREFIX` | string | `LoggingConstants.DIRECTORY_RESOLVER_PREFIX` | _No description_ |
| `DOMAIN_FILES_COUNT` | int | `11` | _No description_ |
| `DOTNET_VERSION` | string | `SystemConstants.DOTNET_VERSION` | _No description_ |
| `E` | double | `UtilityConstants.E` | _No description_ |
| `EMAIL_REGEX_PATTERN` | string | `ValidationConstants.EMAIL_REGEX_PATTERN` | Patterns |
| `EMPTY_STRING` | string | `UtilityConstants.EMPTY_STRING` | String Manipulation |
| `ERROR_CODE_FILE_NOT_FOUND` | int | `ErrorConstants.ERROR_CODE_FILE_NOT_FOUND` | _No description_ |
| `ERROR_CODE_GENERAL` | int | `ErrorConstants.ERROR_CODE_GENERAL` | _No description_ |
| `ERROR_CODE_SUCCESS` | int | `ErrorConstants.ERROR_CODE_SUCCESS` | Error Codes |
| `ERROR_CODE_TIMEOUT` | int | `ErrorConstants.ERROR_CODE_TIMEOUT` | _No description_ |
| `ERROR_LOG_FILENAME` | string | `LoggingConstants.ERROR_LOG_FILENAME` | _No description_ |
| `ERROR_MESSAGE_FILE_OPERATION` | string | `ErrorConstants.ERROR_MESSAGE_FILE_OPERATION` | _No description_ |
| `ERROR_MESSAGE_GENERIC` | string | `ErrorConstants.ERROR_MESSAGE_GENERIC` | Messages |
| `ERROR_MESSAGE_TIMEOUT` | string | `ErrorConstants.ERROR_MESSAGE_TIMEOUT` | _No description_ |
| `FILE_OP_BACKUP` | string | `FileConstants.FILE_OP_BACKUP` | _No description_ |
| `FILE_OP_READ` | string | `FileConstants.FILE_OP_READ` | Operations |
| `FILE_OP_WRITE` | string | `FileConstants.FILE_OP_WRITE` | _No description_ |
| `FILE_OPERATION_TIMEOUT_MS` | int | `PerformanceConstants.FILE_OPERATION_TIMEOUT_MS` | _No description_ |
| `GRAPHICS_SCREEN_HEIGHT` | int | `GameConstants.GRAPHICS_SCREEN_HEIGHT` | _No description_ |
| `GRAPHICS_SCREEN_WIDTH` | int | `GameConstants.GRAPHICS_SCREEN_WIDTH` | _No description_ |
| `GRAPHICS_TARGET_FPS` | int | `GameConstants.GRAPHICS_TARGET_FPS` | Graphics |
| `HTTP_DEFAULT_TIMEOUT_MS` | int | `NetworkConstants.HTTP_DEFAULT_TIMEOUT_MS` | Timeouts |
| `HTTP_METHOD_GET` | string | `NetworkConstants.HTTP_METHOD_GET` | HTTP Methods |
| `HTTP_METHOD_POST` | string | `NetworkConstants.HTTP_METHOD_POST` | _No description_ |
| `HTTP_METHOD_PUT` | string | `NetworkConstants.HTTP_METHOD_PUT` | _No description_ |
| `HTTP_STATUS_INTERNAL_SERVER_ERROR` | int | `NetworkConstants.HTTP_STATUS_INTERNAL_SERVER_ERROR` | _No description_ |
| `HTTP_STATUS_NOT_FOUND` | int | `NetworkConstants.HTTP_STATUS_NOT_FOUND` | _No description_ |
| `HTTP_STATUS_OK` | int | `NetworkConstants.HTTP_STATUS_OK` | Status Codes |
| `JSON_EXTENSION` | string | `FileConstants.JSON_EXTENSION` | Extensions |
| `LOCK_TIMEOUT_MS` | int | `ThreadingConstants.LOCK_TIMEOUT_MS` | _No description_ |
| `LOG_EXTENSION` | string | `FileConstants.LOG_EXTENSION` | _No description_ |
| `LOG_LEVEL_CRITICAL` | int | `LoggingConstants.LOG_LEVEL_CRITICAL` | Log Levels |
| `LOG_LEVEL_IMPORTANT` | int | `LoggingConstants.LOG_LEVEL_IMPORTANT` | _No description_ |
| `LOG_LEVEL_VERBOSE` | int | `LoggingConstants.LOG_LEVEL_VERBOSE` | _No description_ |
| `LOGGER_PREFIX` | string | `LoggingConstants.LOGGER_PREFIX` | _No description_ |
| `LOGS_DIRECTORY` | string | `FileConstants.LOGS_DIRECTORY` | _No description_ |
| `MAIN_LOG_FILENAME` | string | `LoggingConstants.MAIN_LOG_FILENAME` | File Names |
| `MAIN_THREAD_NAME` | string | `ThreadingConstants.MAIN_THREAD_NAME` | Thread Names |
| `MAX_CONCURRENT_OPERATIONS` | int | `ThreadingConstants.MAX_CONCURRENT_OPERATIONS` | _No description_ |
| `MAX_FILE_SIZE_BYTES` | long | `FileConstants.MAX_FILE_SIZE_BYTES` | Validation |
| `MAX_FILENAME_LENGTH` | int | `FileConstants.MAX_FILENAME_LENGTH` | _No description_ |
| `MAX_MIXER_VOLUME` | float | `MixerConstants.MAX_MIXER_VOLUME` | _No description_ |
| `MAX_PATH_LENGTH` | int | `FileConstants.MAX_PATH_LENGTH` | _No description_ |
| `MAX_STRING_LENGTH` | int | `ValidationConstants.MAX_STRING_LENGTH` | _No description_ |
| `MAX_WORKER_THREADS` | int | `ThreadingConstants.MAX_WORKER_THREADS` | _No description_ |
| `MEDIUM_WAIT_MS` | int | `PerformanceConstants.MEDIUM_WAIT_MS` | _No description_ |
| `MIN_MIXER_VOLUME` | float | `MixerConstants.MIN_MIXER_VOLUME` | _No description_ |
| `MIN_STRING_LENGTH` | int | `ValidationConstants.MIN_STRING_LENGTH` | Rules |
| `MIN_WORKER_THREADS` | int | `ThreadingConstants.MIN_WORKER_THREADS` | Thread Pool |
| `MIXER_BACKUP_FILENAME` | string | `MixerConstants.MIXER_BACKUP_FILENAME` | _No description_ |
| `MIXER_CHANNEL_COUNT` | int | `MixerConstants.MIXER_CHANNEL_COUNT` | Configuration |
| `MIXER_CONFIG_FILENAME` | string | `MixerConstants.MIXER_CONFIG_FILENAME` | _No description_ |
| `MIXER_GAIN_KEY` | string | `MixerConstants.MIXER_GAIN_KEY` | _No description_ |
| `MIXER_SAVE_FILENAME` | string | `MixerConstants.MIXER_SAVE_FILENAME` | Files |
| `MIXER_VALIDATION_PREFIX` | string | `LoggingConstants.MIXER_VALIDATION_PREFIX` | _No description_ |
| `MIXER_VALUES_KEY` | string | `MixerConstants.MIXER_VALUES_KEY` | Keys |
| `MIXER_VOLUME_KEY` | string | `MixerConstants.MIXER_VOLUME_KEY` | _No description_ |
| `MOD_NAME` | string | `SystemConstants.MOD_NAME` | Mod Information |
| `MOD_NAMESPACE` | string | `SystemConstants.MOD_NAMESPACE` | _No description_ |
| `MOD_VERSION` | string | `SystemConstants.MOD_VERSION` | _No description_ |
| `MS_PER_SECOND` | int | `UtilityConstants.MS_PER_SECOND` | _No description_ |
| `MUTEX_TIMEOUT_MS` | int | `ThreadingConstants.MUTEX_TIMEOUT_MS` | Timeouts |
| `NEWLINE` | string | `UtilityConstants.NEWLINE` | _No description_ |
| `NUMERIC_REGEX_PATTERN` | string | `ValidationConstants.NUMERIC_REGEX_PATTERN` | _No description_ |
| `OPERATION_TIMEOUT_MS` | int | `PerformanceConstants.OPERATION_TIMEOUT_MS` | Timeouts |
| `ORGANIZATION_PRINCIPLE` | string | `"Domain-driven separation of concerns"` | _No description_ |
| `PERFORMANCE_CRITICAL_THRESHOLD_MS` | int | `PerformanceConstants.PERFORMANCE_CRITICAL_THRES...` | _No description_ |
| `PERFORMANCE_LOG_FILENAME` | string | `LoggingConstants.PERFORMANCE_LOG_FILENAME` | _No description_ |
| `PERFORMANCE_SLOW_THRESHOLD_MS` | int | `PerformanceConstants.PERFORMANCE_SLOW_THRESHOLD_MS` | _No description_ |
| `PERFORMANCE_WARNING_THRESHOLD_MS` | int | `PerformanceConstants.PERFORMANCE_WARNING_THRESH...` | Thresholds |
| `PERSISTENCE_PREFIX` | string | `LoggingConstants.PERSISTENCE_PREFIX` | _No description_ |
| `PHYSICS_FIXED_TIMESTEP` | float | `GameConstants.PHYSICS_FIXED_TIMESTEP` | _No description_ |
| `PHYSICS_GRAVITY` | float | `GameConstants.PHYSICS_GRAVITY` | Physics |
| `PI` | double | `UtilityConstants.PI` | Mathematical |
| `PLATFORM_UNITY` | string | `SystemConstants.PLATFORM_UNITY` | Platform |
| `RECOVERY_STRATEGY_EMERGENCY_SAVE` | string | `ErrorConstants.RECOVERY_STRATEGY_EMERGENCY_SAVE` | _No description_ |
| `RECOVERY_STRATEGY_FALLBACK` | string | `ErrorConstants.RECOVERY_STRATEGY_FALLBACK` | _No description_ |
| `RECOVERY_STRATEGY_RETRY` | string | `ErrorConstants.RECOVERY_STRATEGY_RETRY` | Recovery Strategies |
| `RETRY_DELAY_MS` | int | `PerformanceConstants.RETRY_DELAY_MS` | Wait Times |
| `SAVE_FAILURE_MESSAGE` | string | `LoggingConstants.SAVE_FAILURE_MESSAGE` | _No description_ |
| `SAVE_MANAGER_PREFIX` | string | `LoggingConstants.SAVE_MANAGER_PREFIX` | Prefixes |
| `SAVE_SUCCESS_MESSAGE` | string | `LoggingConstants.SAVE_SUCCESS_MESSAGE` | Messages |
| `SAVE_THREAD_NAME` | string | `ThreadingConstants.SAVE_THREAD_NAME` | _No description_ |
| `SHORT_WAIT_MS` | int | `PerformanceConstants.SHORT_WAIT_MS` | _No description_ |
| `SPACE` | string | `UtilityConstants.SPACE` | _No description_ |
| `TCP_CONNECTION_TIMEOUT_MS` | int | `NetworkConstants.TCP_CONNECTION_TIMEOUT_MS` | _No description_ |
| `TEMP_EXTENSION` | string | `FileConstants.TEMP_EXTENSION` | _No description_ |
| `THREAD_SAFETY_GUARANTEE` | string | `"All constants are immutable and thread-safe"` | _No description_ |
| `TIME_FORMAT_24HOUR` | string | `UtilityConstants.TIME_FORMAT_24HOUR` | _No description_ |
| `TIMESTAMP_FORMAT_MS` | string | `UtilityConstants.TIMESTAMP_FORMAT_MS` | _No description_ |
| `TOTAL_CONSTANTS_COUNT` | int | `1570` | _No description_ |
| `UI_BUTTON_HEIGHT` | int | `GameConstants.UI_BUTTON_HEIGHT` | _No description_ |
| `UI_BUTTON_WIDTH` | int | `GameConstants.UI_BUTTON_WIDTH` | _No description_ |
| `UI_FONT_SIZE_DEFAULT` | int | `GameConstants.UI_FONT_SIZE_DEFAULT` | UI |
| `URL_REGEX_PATTERN` | string | `ValidationConstants.URL_REGEX_PATTERN` | _No description_ |
| `USAGE_PATTERN` | string | `"using static MixerThreholdMod_1_0_0.Constants....` | _No description_ |
| `VALIDATION_INVALID_EMAIL` | string | `ValidationConstants.VALIDATION_INVALID_EMAIL` | _No description_ |
| `VALIDATION_INVALID_FORMAT` | string | `ValidationConstants.VALIDATION_INVALID_FORMAT` | _No description_ |
| `VALIDATION_REQUIRED` | string | `ValidationConstants.VALIDATION_REQUIRED` | Messages |
| `WARN_LEVEL_CRITICAL` | int | `LoggingConstants.WARN_LEVEL_CRITICAL` | _No description_ |
| `WARN_LEVEL_VERBOSE` | int | `LoggingConstants.WARN_LEVEL_VERBOSE` | _No description_ |


## Core Constants

### Constants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `BACKUP_EXTENSION` | string | `FileConstants.BACKUP_EXTENSION` | _No description_ |
| `CHANNEL_COUNT` | int | `MixerConstants.MIXER_CHANNEL_COUNT` | _No description_ |
| `CONSTANTS_REFACTOR_AUTHOR` | string | `"GitHub Copilot"` | _No description_ |
| `CONSTANTS_REFACTOR_DATE` | string | `"2024-12-28"` | _No description_ |
| `CONSTANTS_REFACTOR_DESCRIPTION` | string | `"Comprehensive separation of concerns refactor ...` | _No description_ |
| `CONSTANTS_REFACTOR_VERSION` | string | `"2.0.0"` | _No description_ |
| `DIR_BACKUP` | string | `FileConstants.BACKUP_DIRECTORY` | _No description_ |
| `DIRECTORY_RESOLVER_PREFIX` | string | `LoggingConstants.DIRECTORY_RESOLVER_PREFIX` | _No description_ |
| `DOMAIN_FILES_COUNT` | int | `12` | _No description_ |
| `DOMAIN_IMPORT_EXAMPLE` | string | `"using static MixerThreholdMod_1_0_0.Constants....` | _No description_ |
| `ERROR_CODE_GENERAL` | int | `ErrorConstants.ERROR_CODE_GENERAL` | _No description_ |
| `ERROR_CODE_SUCCESS` | int | `ErrorConstants.ERROR_CODE_SUCCESS` | _No description_ |
| `EXT_BACKUP` | string | `FileConstants.BACKUP_EXTENSION` | _No description_ |
| `EXT_JSON` | string | `FileConstants.JSON_EXTENSION` | _No description_ |
| `EXT_LOG` | string | `FileConstants.LOG_EXTENSION` | _No description_ |
| `FILE_NOT_FOUND` | int | `ErrorConstants.ERROR_CODE_FILE_NOT_FOUND` | _No description_ |
| `GENERAL` | int | `ErrorConstants.ERROR_CODE_GENERAL` | _No description_ |
| `JSON_EXTENSION` | string | `FileConstants.JSON_EXTENSION` | _No description_ |
| `LOCK_TIMEOUT` | int | `ThreadingConstants.LOCK_TIMEOUT_MS` | _No description_ |
| `LOG_LEVEL_CRITICAL` | int | `LoggingConstants.LOG_LEVEL_CRITICAL` | For comprehensive constants, use domain-specific files or AllConstants.cs Most critical constants for immediate use |
| `LOG_LEVEL_IMPORTANT` | int | `LoggingConstants.LOG_LEVEL_IMPORTANT` | _No description_ |
| `LOG_LEVEL_VERBOSE` | int | `LoggingConstants.LOG_LEVEL_VERBOSE` | _No description_ |
| `MAIN_THREAD` | string | `ThreadingConstants.MAIN_THREAD_NAME` | _No description_ |
| `MAX_VOLUME` | float | `MixerConstants.MAX_MIXER_VOLUME` | _No description_ |
| `MIGRATION_INSTRUCTIONS` | string | `"Replace 'using static MixerThreholdMod_1_0_0.C...` | _No description_ |
| `MIXER_SAVE_FILENAME` | string | `MixerConstants.MIXER_SAVE_FILENAME` | _No description_ |
| `MIXER_VALUES_KEY` | string | `MixerConstants.MIXER_VALUES_KEY` | _No description_ |
| `MOD_NAME` | string | `SystemConstants.MOD_NAME` | _No description_ |
| `MOD_VERSION` | string | `SystemConstants.MOD_VERSION` | _No description_ |
| `MSG_FAILURE` | string | `LoggingConstants.SAVE_FAILURE_MESSAGE` | _No description_ |
| `MSG_SUCCESS` | string | `LoggingConstants.SAVE_SUCCESS_MESSAGE` | _No description_ |
| `MUTEX_TIMEOUT` | int | `ThreadingConstants.MUTEX_TIMEOUT_MS` | _No description_ |
| `OP_READ` | string | `FileConstants.FILE_OP_READ` | _No description_ |
| `OP_WRITE` | string | `FileConstants.FILE_OP_WRITE` | _No description_ |
| `OPERATION_TIMEOUT_MS` | int | `PerformanceConstants.OPERATION_TIMEOUT_MS` | _No description_ |
| `PERFORMANCE_WARNING_THRESHOLD_MS` | int | `PerformanceConstants.PERFORMANCE_WARNING_THRESH...` | _No description_ |
| `PERSISTENCE_PREFIX` | string | `LoggingConstants.PERSISTENCE_PREFIX` | _No description_ |
| `PREFIX_BACKUP` | string | `LoggingConstants.BACKUP_MANAGER_PREFIX` | _No description_ |
| `PREFIX_LOGGER` | string | `LoggingConstants.LOGGER_PREFIX` | _No description_ |
| `PREFIX_SAVE` | string | `LoggingConstants.SAVE_MANAGER_PREFIX` | _No description_ |
| `RETRY_DELAY` | int | `PerformanceConstants.RETRY_DELAY_MS` | _No description_ |
| `SAVE_FILE` | string | `MixerConstants.MIXER_SAVE_FILENAME` | _No description_ |
| `SAVE_MANAGER_PREFIX` | string | `LoggingConstants.SAVE_MANAGER_PREFIX` | _No description_ |
| `SAVE_THREAD` | string | `ThreadingConstants.SAVE_THREAD_NAME` | _No description_ |
| `SEPARATION_BENEFITS` | string | `"Better organization, reduced file size, improv...` | _No description_ |
| `STRATEGY_EMERGENCY` | string | `ErrorConstants.RECOVERY_STRATEGY_EMERGENCY_SAVE` | _No description_ |
| `STRATEGY_RETRY` | string | `ErrorConstants.RECOVERY_STRATEGY_RETRY` | _No description_ |
| `SUCCESS` | int | `ErrorConstants.ERROR_CODE_SUCCESS` | _No description_ |
| `THRESHOLD_WARNING` | int | `PerformanceConstants.PERFORMANCE_WARNING_THRESH...` | _No description_ |
| `TIMEOUT` | int | `ErrorConstants.ERROR_CODE_TIMEOUT` | _No description_ |
| `TIMEOUT_FILE_OP` | int | `PerformanceConstants.FILE_OPERATION_TIMEOUT_MS` | _No description_ |
| `TIMEOUT_OPERATION` | int | `PerformanceConstants.OPERATION_TIMEOUT_MS` | _No description_ |
| `TOTAL_CONSTANTS_AVAILABLE` | int | `1785` | _No description_ |
| `VALUES_KEY` | string | `MixerConstants.MIXER_VALUES_KEY` | _No description_ |


## Error Constants

### ErrorConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ERROR_CODE_ACCESS_DENIED` | int | `3` | _No description_ |
| `ERROR_CODE_CONFIGURATION` | int | `12` | _No description_ |
| `ERROR_CODE_DATA_CORRUPTION` | int | `14` | _No description_ |
| `ERROR_CODE_FILE_NOT_FOUND` | int | `2` | _No description_ |
| `ERROR_CODE_GENERAL` | int | `1` | _No description_ |
| `ERROR_CODE_INVALID_ARGUMENT` | int | `7` | _No description_ |
| `ERROR_CODE_INVALID_OPERATION` | int | `4` | _No description_ |
| `ERROR_CODE_NETWORK` | int | `9` | _No description_ |
| `ERROR_CODE_OPERATION_CANCELLED` | int | `8` | _No description_ |
| `ERROR_CODE_OUT_OF_MEMORY` | int | `6` | _No description_ |
| `ERROR_CODE_PERMISSION` | int | `13` | _No description_ |
| `ERROR_CODE_SERIALIZATION` | int | `10` | _No description_ |
| `ERROR_CODE_SUCCESS` | int | `0` | _No description_ |
| `ERROR_CODE_TIMEOUT` | int | `5` | _No description_ |
| `ERROR_CODE_VALIDATION` | int | `11` | _No description_ |
| `ERROR_CODE_VERSION_MISMATCH` | int | `15` | _No description_ |
| `ERROR_CONTEXT_BACKUP` | string | `"Backup"` | _No description_ |
| `ERROR_CONTEXT_CONFIGURATION` | string | `"Configuration"` | _No description_ |
| `ERROR_CONTEXT_INITIALIZATION` | string | `"Initialization"` | _No description_ |
| `ERROR_CONTEXT_LOAD` | string | `"Load"` | _No description_ |
| `ERROR_CONTEXT_NETWORK` | string | `"Network"` | _No description_ |
| `ERROR_CONTEXT_PERFORMANCE` | string | `"Performance"` | _No description_ |
| `ERROR_CONTEXT_SAVE` | string | `"Save"` | _No description_ |
| `ERROR_CONTEXT_SECURITY` | string | `"Security"` | _No description_ |
| `ERROR_CONTEXT_THREADING` | string | `"Threading"` | _No description_ |
| `ERROR_CONTEXT_VALIDATION` | string | `"Validation"` | _No description_ |
| `ERROR_MESSAGE_CANCELLATION` | string | `"Operation was cancelled"` | _No description_ |
| `ERROR_MESSAGE_CONFIGURATION` | string | `"Configuration error"` | _No description_ |
| `ERROR_MESSAGE_DATA_CORRUPTION` | string | `"Data corruption detected"` | _No description_ |
| `ERROR_MESSAGE_FILE_OPERATION` | string | `"File operation failed"` | _No description_ |
| `ERROR_MESSAGE_GENERIC` | string | `"An error occurred during operation"` | _No description_ |
| `ERROR_MESSAGE_LOAD_OPERATION` | string | `"Load operation failed"` | _No description_ |
| `ERROR_MESSAGE_MEMORY` | string | `"Memory allocation failed"` | _No description_ |
| `ERROR_MESSAGE_NETWORK` | string | `"Network operation failed"` | _No description_ |
| `ERROR_MESSAGE_NOT_SUPPORTED` | string | `"Operation is not supported"` | _No description_ |
| `ERROR_MESSAGE_PERMISSION` | string | `"Permission denied"` | _No description_ |
| `ERROR_MESSAGE_RESOURCE_UNAVAILABLE` | string | `"Resource is unavailable"` | _No description_ |
| `ERROR_MESSAGE_SAVE_OPERATION` | string | `"Save operation failed"` | _No description_ |
| `ERROR_MESSAGE_TIMEOUT` | string | `"Operation timed out"` | _No description_ |
| `ERROR_MESSAGE_VALIDATION` | string | `"Validation failed"` | _No description_ |
| `ERROR_MESSAGE_VERSION_MISMATCH` | string | `"Version mismatch detected"` | _No description_ |
| `ERROR_SEVERITY_CRITICAL` | string | `"Critical"` | _No description_ |
| `ERROR_SEVERITY_HIGH` | string | `"High"` | _No description_ |
| `ERROR_SEVERITY_INFO` | string | `"Information"` | _No description_ |
| `ERROR_SEVERITY_LOW` | string | `"Low"` | _No description_ |
| `ERROR_SEVERITY_MEDIUM` | string | `"Medium"` | _No description_ |
| `ERROR_SEVERITY_WARNING` | string | `"Warning"` | _No description_ |
| `EXCEPTION_TYPE_ARGUMENT` | string | `"ArgumentException"` | _No description_ |
| `EXCEPTION_TYPE_FILE_NOT_FOUND` | string | `"FileNotFoundException"` | _No description_ |
| `EXCEPTION_TYPE_INVALID_OPERATION` | string | `"InvalidOperationException"` | _No description_ |
| `EXCEPTION_TYPE_NOT_SUPPORTED` | string | `"NotSupportedException"` | _No description_ |
| `EXCEPTION_TYPE_NULL_REFERENCE` | string | `"NullReferenceException"` | _No description_ |
| `EXCEPTION_TYPE_OPERATION_CANCELLED` | string | `"OperationCanceledException"` | _No description_ |
| `EXCEPTION_TYPE_OUT_OF_MEMORY` | string | `"OutOfMemoryException"` | _No description_ |
| `EXCEPTION_TYPE_SYSTEM` | string | `"SystemException"` | _No description_ |
| `EXCEPTION_TYPE_TIMEOUT` | string | `"TimeoutException"` | _No description_ |
| `EXCEPTION_TYPE_UNAUTHORIZED_ACCESS` | string | `"UnauthorizedAccessException"` | _No description_ |
| `RECOVERY_STRATEGY_ABORT` | string | `"Abort"` | _No description_ |
| `RECOVERY_STRATEGY_BACKUP_RESTORE` | string | `"BackupRestore"` | _No description_ |
| `RECOVERY_STRATEGY_DEFAULT` | string | `"Default"` | _No description_ |
| `RECOVERY_STRATEGY_EMERGENCY_SAVE` | string | `"EmergencySave"` | _No description_ |
| `RECOVERY_STRATEGY_FALLBACK` | string | `"Fallback"` | _No description_ |
| `RECOVERY_STRATEGY_RESET_DEFAULTS` | string | `"ResetDefaults"` | _No description_ |
| `RECOVERY_STRATEGY_RETRY` | string | `"Retry"` | _No description_ |
| `RECOVERY_STRATEGY_SKIP` | string | `"Skip"` | _No description_ |


## File Constants

### FileConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ALL_FILES_PATTERN` | string | `"*.*"` | _No description_ |
| `ALT_PATH_SEPARATOR_CHAR` | char | `'/'` | _No description_ |
| `AVI_EXTENSION` | string | `".avi"` | _No description_ |
| `BACKUP_DIRECTORY` | string | `"Backups"` | _No description_ |
| `BACKUP_EXTENSION` | string | `".bak"` | _No description_ |
| `BACKUP_FILES_PATTERN` | string | `"*.bak"` | _No description_ |
| `BIN_EXTENSION` | string | `".bin"` | _No description_ |
| `CACHE_DIRECTORY` | string | `"Cache"` | _No description_ |
| `CONFIG_DIRECTORY` | string | `"Config"` | _No description_ |
| `CONFIG_EXTENSION` | string | `".config"` | _No description_ |
| `CONFIG_FILES_PATTERN` | string | `"*.config"` | _No description_ |
| `CURRENT_DIRECTORY` | string | `"."` | _No description_ |
| `DATA_DIRECTORY` | string | `"Data"` | _No description_ |
| `DATA_EXTENSION` | string | `".data"` | _No description_ |
| `DIRECTORY_SEPARATOR` | string | `"\\"` | _No description_ |
| `DLL_EXTENSION` | string | `".dll"` | _No description_ |
| `DLL_FILES_PATTERN` | string | `"*.dll"` | _No description_ |
| `EXE_EXTENSION` | string | `".exe"` | _No description_ |
| `EXE_FILES_PATTERN` | string | `"*.exe"` | _No description_ |
| `FILE_ACCESS_READ` | string | `"Read"` | _No description_ |
| `FILE_ACCESS_READ_WRITE` | string | `"ReadWrite"` | _No description_ |
| `FILE_ACCESS_WRITE` | string | `"Write"` | _No description_ |
| `FILE_BUFFER_SIZE` | int | `4096` | _No description_ |
| `FILE_MODE_APPEND` | string | `"Append"` | _No description_ |
| `FILE_MODE_CREATE` | string | `"Create"` | _No description_ |
| `FILE_MODE_CREATE_NEW` | string | `"CreateNew"` | _No description_ |
| `FILE_MODE_OPEN` | string | `"Open"` | _No description_ |
| `FILE_MODE_OPEN_OR_CREATE` | string | `"OpenOrCreate"` | _No description_ |
| `FILE_MODE_TRUNCATE` | string | `"Truncate"` | _No description_ |
| `FILE_OP_APPEND` | string | `"Append"` | _No description_ |
| `FILE_OP_BACKUP` | string | `"Backup"` | _No description_ |
| `FILE_OP_COPY` | string | `"Copy"` | _No description_ |
| `FILE_OP_CREATE` | string | `"Create"` | _No description_ |
| `FILE_OP_DELETE` | string | `"Delete"` | _No description_ |
| `FILE_OP_MOVE` | string | `"Move"` | _No description_ |
| `FILE_OP_READ` | string | `"Read"` | _No description_ |
| `FILE_OP_RESTORE` | string | `"Restore"` | _No description_ |
| `FILE_OP_VALIDATE` | string | `"Validate"` | _No description_ |
| `FILE_OP_WRITE` | string | `"Write"` | _No description_ |
| `JPEG_EXTENSION` | string | `".jpeg"` | _No description_ |
| `JPG_EXTENSION` | string | `".jpg"` | _No description_ |
| `JSON_EXTENSION` | string | `".json"` | _No description_ |
| `JSON_FILES_PATTERN` | string | `"*.json"` | _No description_ |
| `LARGE_FILE_BUFFER_SIZE` | int | `65536` | _No description_ |
| `LOG_EXTENSION` | string | `".log"` | _No description_ |
| `LOG_FILES_PATTERN` | string | `"*.log"` | _No description_ |
| `LOGS_DIRECTORY` | string | `"Logs"` | _No description_ |
| `MAX_FILE_SIZE_BYTES` | long | `104857600` | _No description_ |
| `MAX_FILENAME_LENGTH` | int | `255` | _No description_ |
| `MAX_PATH_LENGTH` | int | `260` | _No description_ |
| `MIN_FILE_SIZE_BYTES` | long | `0` | _No description_ |
| `MIN_FILENAME_LENGTH` | int | `1` | _No description_ |
| `MODS_DIRECTORY` | string | `"Mods"` | _No description_ |
| `MP3_EXTENSION` | string | `".mp3"` | _No description_ |
| `MP4_EXTENSION` | string | `".mp4"` | _No description_ |
| `PARENT_DIRECTORY` | string | `".."` | _No description_ |
| `PATH_SEPARATOR_CHAR` | char | `'\\'` | _No description_ |
| `PLUGINS_DIRECTORY` | string | `"Plugins"` | _No description_ |
| `PNG_EXTENSION` | string | `".png"` | _No description_ |
| `PRESETS_DIRECTORY` | string | `"Presets"` | _No description_ |
| `RAR_EXTENSION` | string | `".rar"` | _No description_ |
| `SETTINGS_DIRECTORY` | string | `"Settings"` | _No description_ |
| `SETTINGS_EXTENSION` | string | `".settings"` | _No description_ |
| `TEMP_DIRECTORY` | string | `"Temp"` | _No description_ |
| `TEMP_EXTENSION` | string | `".tmp"` | _No description_ |
| `TEMP_FILES_PATTERN` | string | `"*.tmp"` | _No description_ |
| `TXT_EXTENSION` | string | `".txt"` | _No description_ |
| `UNIX_PATH_SEPARATOR` | string | `"/"` | _No description_ |
| `VOLUME_SEPARATOR_CHAR` | char | `':'` | _No description_ |
| `WAV_EXTENSION` | string | `".wav"` | _No description_ |
| `WINDOWS_PATH_SEPARATOR` | string | `"\\"` | _No description_ |
| `XML_EXTENSION` | string | `".xml"` | _No description_ |
| `ZIP_EXTENSION` | string | `".zip"` | _No description_ |


## Game Constants

### GameConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ANIMATION_BLEND_TIME` | float | `0.2f` | _No description_ |
| `ANIMATION_FADE_IN_TIME` | float | `0.3f` | _No description_ |
| `ANIMATION_FADE_OUT_TIME` | float | `0.2f` | _No description_ |
| `ANIMATION_LOOP_MODE` | string | `"Loop"` | _No description_ |
| `ANIMATION_PLAY_MODE` | string | `"StopSameLayer"` | _No description_ |
| `ANIMATION_SPEED_DEFAULT` | float | `1.0f` | _No description_ |
| `ANIMATION_SPEED_FAST` | float | `2.0f` | _No description_ |
| `ANIMATION_SPEED_SLOW` | float | `0.5f` | _No description_ |
| `AUDIO_BIT_DEPTH` | int | `16` | _No description_ |
| `AUDIO_BUFFER_SIZE` | int | `1024` | _No description_ |
| `AUDIO_CHANNELS` | int | `2` | _No description_ |
| `AUDIO_DEFAULT_VOLUME` | float | `0.8f` | _No description_ |
| `AUDIO_FADE_TIME` | float | `0.5f` | _No description_ |
| `AUDIO_MASTER_VOLUME` | float | `1.0f` | _No description_ |
| `AUDIO_MAX_VOLUME` | float | `1.0f` | _No description_ |
| `AUDIO_MIN_VOLUME` | float | `0.0f` | _No description_ |
| `AUDIO_SAMPLE_RATE` | int | `44100` | _No description_ |
| `GAME_STATE_CREDITS` | string | `"Credits"` | _No description_ |
| `GAME_STATE_GAME_OVER` | string | `"GameOver"` | _No description_ |
| `GAME_STATE_LOADING` | string | `"Loading"` | _No description_ |
| `GAME_STATE_MAIN_MENU` | string | `"MainMenu"` | _No description_ |
| `GAME_STATE_PAUSED` | string | `"Paused"` | _No description_ |
| `GAME_STATE_PLAYING` | string | `"Playing"` | _No description_ |
| `GAME_STATE_SETTINGS` | string | `"Settings"` | _No description_ |
| `GAMEPLAY_DEFAULT_HEALTH` | float | `100.0f` | _No description_ |
| `GAMEPLAY_DEFAULT_JUMP_FORCE` | float | `10.0f` | _No description_ |
| `GAMEPLAY_DEFAULT_SPEED` | float | `5.0f` | _No description_ |
| `GAMEPLAY_INVINCIBILITY_TIME` | float | `2.0f` | _No description_ |
| `GAMEPLAY_MAX_LEVEL` | int | `100` | _No description_ |
| `GAMEPLAY_PLAYER_LIVES` | int | `3` | _No description_ |
| `GAMEPLAY_RESPAWN_TIME` | float | `3.0f` | _No description_ |
| `GAMEPLAY_START_LEVEL` | int | `1` | _No description_ |
| `GAMEPLAY_XP_PER_LEVEL` | int | `1000` | _No description_ |
| `GRAPHICS_AA_SAMPLES` | int | `4` | _No description_ |
| `GRAPHICS_ANISO_FILTERING` | int | `16` | _No description_ |
| `GRAPHICS_MIN_FPS` | int | `30` | _No description_ |
| `GRAPHICS_RENDER_DISTANCE` | float | `1000.0f` | _No description_ |
| `GRAPHICS_SCREEN_HEIGHT` | int | `1080` | _No description_ |
| `GRAPHICS_SCREEN_WIDTH` | int | `1920` | _No description_ |
| `GRAPHICS_SHADOW_QUALITY` | string | `"High"` | _No description_ |
| `GRAPHICS_TARGET_FPS` | int | `60` | _No description_ |
| `GRAPHICS_TEXTURE_QUALITY` | string | `"Full Res"` | _No description_ |
| `GRAPHICS_VSYNC_ENABLED` | bool | `true` | _No description_ |
| `INPUT_BUFFER_SIZE` | int | `10` | _No description_ |
| `INPUT_CONTROLLER_DEADZONE` | float | `0.2f` | _No description_ |
| `INPUT_DOUBLE_CLICK_TIME_MS` | int | `300` | _No description_ |
| `INPUT_LONG_PRESS_TIME_MS` | int | `500` | _No description_ |
| `INPUT_MAX_LAG_COMPENSATION_MS` | int | `100` | _No description_ |
| `INPUT_MOUSE_SENSITIVITY` | float | `2.0f` | _No description_ |
| `PHYSICS_COLLISION_DETECTION` | string | `"Continuous"` | _No description_ |
| `PHYSICS_DEFAULT_BOUNCE` | float | `0.3f` | _No description_ |
| `PHYSICS_DEFAULT_FRICTION` | float | `0.6f` | _No description_ |
| `PHYSICS_FIXED_TIMESTEP` | float | `0.02f` | _No description_ |
| `PHYSICS_GRAVITY` | float | `-9.81f` | _No description_ |
| `PHYSICS_MAX_TIMESTEP` | float | `0.333f` | _No description_ |
| `PHYSICS_SLEEP_THRESHOLD` | float | `0.005f` | _No description_ |
| `PHYSICS_SOLVER_ITERATIONS` | int | `6` | _No description_ |
| `PHYSICS_VELOCITY_ITERATIONS` | int | `1` | _No description_ |
| `UI_BUTTON_HEIGHT` | int | `30` | _No description_ |
| `UI_BUTTON_WIDTH` | int | `100` | _No description_ |
| `UI_COLOR_BACKGROUND` | string | `"#303030"` | _No description_ |
| `UI_COLOR_DISABLED` | string | `"#757575"` | _No description_ |
| `UI_COLOR_ERROR` | string | `"#F44336"` | _No description_ |
| `UI_COLOR_PRIMARY` | string | `"#2196F3"` | _No description_ |
| `UI_COLOR_SECONDARY` | string | `"#FF9800"` | _No description_ |
| `UI_COLOR_SUCCESS` | string | `"#4CAF50"` | _No description_ |
| `UI_COLOR_TEXT` | string | `"#FFFFFF"` | _No description_ |
| `UI_COLOR_WARNING` | string | `"#FF5722"` | _No description_ |
| `UI_ELEMENT_SPACING` | int | `5` | _No description_ |
| `UI_FADE_DURATION_MS` | int | `250` | _No description_ |
| `UI_FONT_SIZE_DEFAULT` | int | `12` | _No description_ |
| `UI_FONT_SIZE_LARGE` | int | `16` | _No description_ |
| `UI_FONT_SIZE_SMALL` | int | `10` | _No description_ |
| `UI_PANEL_PADDING` | int | `10` | _No description_ |
| `UI_SCROLLBAR_WIDTH` | int | `16` | _No description_ |
| `UI_TOOLTIP_DELAY_MS` | int | `500` | _No description_ |
| `UI_WINDOW_MIN_HEIGHT` | int | `150` | _No description_ |
| `UI_WINDOW_MIN_WIDTH` | int | `200` | _No description_ |


## General Constants

### Constants_LargeOriginal.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ACTION_PARAM_NAME` | string | `"action"` | _No description_ |
| `ADVANCED_SAVE_OPERATION_PREFIX` | string | `"[ADVANCED_SAVE]"` | ===== LOGGING PREFIX CONSTANTS ===== |
| `ANIMATION_BLEND_TIME_DEFAULT` | float | `0.1f` | _No description_ |
| `ANIMATION_CROSSFADE_TIME_DEFAULT` | float | `0.25f` | _No description_ |
| `ANIMATION_FRAME_RATE_HIGH` | int | `60` | _No description_ |
| `ANIMATION_FRAME_RATE_STANDARD` | int | `30` | ===== ANIMATION CONSTANTS ===== |
| `ANIMATION_LOOP_COUNT_INFINITE` | int | `-1` | _No description_ |
| `ANIMATION_PLAYBACK_SPEED_DEFAULT` | float | `1.0f` | _No description_ |
| `ANIMATION_PLAYBACK_SPEED_MAX` | float | `10.0f` | _No description_ |
| `ANIMATION_PLAYBACK_SPEED_MIN` | float | `0.1f` | _No description_ |
| `ANIMATION_TANGENT_MODE_LINEAR` | int | `0` | _No description_ |
| `ANIMATION_TANGENT_MODE_SMOOTH` | int | `1` | _No description_ |
| `APP_CONFIG_FILENAME` | string | `"app.config"` | _No description_ |
| `ARGUMENT_NULL_EXCEPTION_MSG` | string | `"ArgumentNullException for {0}: {1}\nStack Trac...` | _No description_ |
| `ARRAY_COPY_METHOD` | string | `"Copy"` | _No description_ |
| `ARRAY_EMPTY_METHOD` | string | `"Empty"` | _No description_ |
| `ASSEMBLY_COMPANY_EMPTY` | string | `""` | _No description_ |
| `ASSEMBLY_CONFIGURATION_EMPTY` | string | `""` | ===== ASSEMBLY INFO CONSTANTS ===== |
| `ASSEMBLY_COPYRIGHT` | string | `"Copyright ©  2025 mooleshacat"` | _No description_ |
| `ASSEMBLY_COPYRIGHT` | string | `"Copyright ©  2025 mooleshacat"` | _No description_ |
| `ASSEMBLY_CSHARP` | string | `"Assembly-CSharp"` | ===== ASSEMBLY AND VERSION CONSTANTS ===== |
| `ASSEMBLY_CULTURE_EMPTY` | string | `""` | _No description_ |
| `ASSEMBLY_DESCRIPTION` | string | `"Schedule 1 MixerThreholdMod"` | _No description_ |
| `ASSEMBLY_FILE_VERSION` | string | `"1.0.0.0"` | _No description_ |
| `ASSEMBLY_FILE_VERSION_FORMAT` | string | `"1.0.0.0"` | _No description_ |
| `ASSEMBLY_GUID` | string | `"17e5161c-09cb-40a1-b3ae-2d7e968e8660"` | ===== GUID AND VERSION CONSTANTS ===== |
| `ASSEMBLY_LOAD_METHOD` | string | `"Load"` | _No description_ |
| `ASSEMBLY_LOCATION_PROPERTY` | string | `"Location"` | _No description_ |
| `ASSEMBLY_PRODUCT` | string | `"MixerThreholdMod-1_0_0"` | _No description_ |
| `ASSEMBLY_TITLE` | string | `"MixerThreholdMod-1_0_0"` | _No description_ |
| `ASSEMBLY_TRADEMARK_EMPTY` | string | `""` | _No description_ |
| `ASSEMBLY_VERSION` | string | `"1.0.0.0"` | _No description_ |
| `ASSEMBLY_VERSION_FORMAT` | string | `"1.0.0.0"` | _No description_ |
| `ASYNC_CANCELLATION_TIMEOUT_SECONDS` | int | `10` | _No description_ |
| `ATOMIC_WRITE_FAILED_MSG` | string | `"Atomic write failed for {0}: {1}\nStack Trace:...` | ===== ADDITIONAL LOGGING MESSAGES ===== |
| `ATOMIC_WRITE_SUCCESS_MSG` | string | `"Atomic write succeeded for {0}"` | _No description_ |
| `ATTACH_TIMEOUT_SECONDS` | float | `10f` | _No description_ |
| `ATTEMPTING_BACKUP_MSG` | string | `"Attempting backup of savegame directory!"` | _No description_ |
| `ATTEMPTING_DELETE_BACKUP_MSG` | string | `"Attempting to delete backup directory: {0}"` | _No description_ |
| `AUDIO_BIT_DEPTH_STANDARD` | int | `16` | _No description_ |
| `AUDIO_BUFFER_SIZE_DEFAULT` | int | `1024` | _No description_ |
| `AUDIO_CHANNELS_MONO` | int | `1` | _No description_ |
| `AUDIO_CHANNELS_STEREO` | int | `2` | _No description_ |
| `AUDIO_FADE_DURATION_SECONDS` | float | `1.0f` | _No description_ |
| `AUDIO_PROCESSING_INTERVAL_MS` | int | `10` | _No description_ |
| `AUDIO_SAMPLE_RATE_STANDARD` | int | `44100` | _No description_ |
| `AUDIO_VOLUME_DEFAULT` | float | `0.7f` | _No description_ |
| `AUDIO_VOLUME_MAX` | float | `1.0f` | _No description_ |
| `AUDIO_VOLUME_MIN` | float | `0.0f` | ===== AUDIO CONSTANTS ===== |
| `AVAILABLE_STATUS` | string | `"AVAILABLE"` | _No description_ |
| `BACKGROUND_THREAD_PRIORITY` | string | `"BelowNormal"` | _No description_ |
| `BACKSLASH` | string | `"\\"` | _No description_ |
| `BACKUP_ACCESS_DENIED_WARNING` | string | `"Access denied while deleting backup directory:...` | _No description_ |
| `BACKUP_CLEANUP_COMPLETED_MSG` | string | `"Backup cleanup completed. Deleted {0} old dire...` | _No description_ |
| `BACKUP_DIRECTORY_NAME` | string | `"Backup"` | _No description_ |
| `BACKUP_DIRECTORY_NOT_EXIST_WARNING` | string | `"Backup root directory does not exist: {0}"` | _No description_ |
| `BACKUP_FAILED_MSG` | string | `"BackupAsync failed for {0}: {1}\nStack Trace: ...` | _No description_ |
| `BACKUP_FILE_EXTENSION` | string | `".backup"` | ===== FILE EXTENSION CONSTANTS ===== |
| `BACKUP_FILENAME_PATTERN` | string | `"MixerThresholdSave_backup_{0}.json"` | _No description_ |
| `BACKUP_FILENAME_WILDCARD` | string | `"MixerThresholdSave_backup_*.json"` | _No description_ |
| `BACKUP_INCOMPLETE_READ_WARNING` | string | `"BackupAsync: Incomplete read for {0}"` | _No description_ |
| `BACKUP_INTERVAL_MINUTES` | int | `5` | _No description_ |
| `BACKUP_OPERATION_PREFIX` | string | `"[BACKUP]"` | _No description_ |
| `BACKUP_ROOT_MSG` | string | `"BACKUP ROOT: {0}"` | _No description_ |
| `BACKUP_SAVE_FOLDER_COMPLETED_MSG` | string | `"BackupSaveFolder: Backup operation completed s...` | _No description_ |
| `BACKUP_SAVE_FOLDER_ERROR_MSG` | string | `"BackupSaveFolder: Error in backup task: {0}\n{1}"` | _No description_ |
| `BACKUP_SAVE_FOLDER_FINISHED_MSG` | string | `"BackupSaveFolder: Finished and cleanup completed"` | _No description_ |
| `BACKUP_SAVE_FOLDER_GENERIC_STARTED_MSG` | string | `"BackupSaveFolder: Started"` | _No description_ |
| `BACKUP_SAVE_FOLDER_IN_PROGRESS_MSG` | string | `"BackupSaveFolder: Already in progress, skippin...` | _No description_ |
| `BACKUP_SAVE_FOLDER_NULL_PATH_MSG` | string | `"BackupSaveFolder: CurrentSavePath is null/empt...` | _No description_ |
| `BACKUP_SAVE_FOLDER_STARTED_MSG` | string | `"BackupSaveFolder started for: {0}"` | ===== SAVE OPERATION MESSAGES ===== |
| `BACKUP_SAVE_PREFIX` | string | `"[BackupSaveManager]"` | _No description_ |
| `BACKUP_SOURCE_NOT_FOUND_WARNING` | string | `"BackupAsync: Source file not found {0}"` | _No description_ |
| `BACKUP_SUCCESS_MSG` | string | `"BackupAsync succeeded for {0} → {1}"` | ===== BACKUP OPERATION MESSAGES ===== |
| `BACKUP_TASK_FAILED_MSG` | string | `"Backup task failed: {0}"` | _No description_ |
| `BAK_FILE_EXTENSION` | string | `".bak"` | ===== EXTENDED FILE EXTENSIONS ===== |
| `BINDING_FLAGS_INSTANCE` | string | `"Instance"` | _No description_ |
| `BINDING_FLAGS_NON_PUBLIC` | string | `"NonPublic"` | _No description_ |
| `BINDING_FLAGS_PUBLIC` | string | `"Public"` | ===== BINDING FLAGS CONSTANTS ===== |
| `BINDING_FLAGS_STATIC` | string | `"Static"` | _No description_ |
| `BRACKET_CLOSE_ANGLE` | string | `"&gt;"` | _No description_ |
| `BRACKET_CLOSE_CURLY` | string | `"}"` | _No description_ |
| `BRACKET_CLOSE_PAREN` | string | `")"` | _No description_ |
| `BRACKET_CLOSE_SQUARE` | string | `"]"` | _No description_ |
| `BRACKET_OPEN_ANGLE` | string | `"&lt;"` | _No description_ |
| `BRACKET_OPEN_CURLY` | string | `"{"` | _No description_ |
| `BRACKET_OPEN_PAREN` | string | `"("` | ===== BRACKET AND PARENTHESIS CONSTANTS ===== |
| `BRACKET_OPEN_SQUARE` | string | `"["` | _No description_ |
| `BRIEF_WAIT_SECONDS` | float | `0.2f` | _No description_ |
| `BUILD_CONFIG_DEBUG` | string | `"Debug"` | ===== BUILD AND COMPILATION CONSTANTS ===== |
| `BUILD_CONFIG_RELEASE` | string | `"Release"` | _No description_ |
| `BYTES_PER_GB` | long | `1073741824L` | _No description_ |
| `BYTES_PER_KB` | int | `1024` | ===== MEMORY AND RESOURCE CONSTANTS ===== |
| `BYTES_PER_MB` | int | `1048576` | _No description_ |
| `BYTES_TO_KB` | int | `1024` | ===== MEMORY AND CONVERSION CONSTANTS ===== |
| `BYTES_TO_KILOBYTES` | int | `1024` | ===== UNIT MULTIPLIER CONSTANTS ===== |
| `BYTES_TO_MB` | double | `1048576.0` | _No description_ |
| `CACHE_DIRECTORY_NAME` | string | `"Cache"` | _No description_ |
| `CACHE_EVICTION_THRESHOLD` | double | `0.8` | _No description_ |
| `CALLBACK_PARAM_NAME` | string | `"callback"` | _No description_ |
| `CAMERA_FAR_CLIP_DEFAULT` | float | `1000.0f` | _No description_ |
| `CAMERA_FAR_CLIP_MAX` | float | `50000.0f` | _No description_ |
| `CAMERA_FAR_CLIP_MIN` | float | `1.0f` | _No description_ |
| `CAMERA_FOV_DEFAULT` | float | `60.0f` | _No description_ |
| `CAMERA_FOV_MAX` | float | `179.0f` | _No description_ |
| `CAMERA_FOV_MIN` | float | `1.0f` | ===== CAMERA CONSTANTS ===== |
| `CAMERA_NEAR_CLIP_DEFAULT` | float | `0.3f` | _No description_ |
| `CAMERA_NEAR_CLIP_MAX` | float | `10.0f` | _No description_ |
| `CAMERA_NEAR_CLIP_MIN` | float | `0.01f` | _No description_ |
| `CAMERA_ORTHO_SIZE_MAX` | float | `1000.0f` | _No description_ |
| `CAMERA_ORTHO_SIZE_MIN` | float | `0.1f` | _No description_ |
| `CANCELLATION_TOKEN_PARAM` | string | `"cancellationToken"` | _No description_ |
| `COMMA_SEPARATOR` | string | `", "` | _No description_ |
| `COMMAND_HELP` | string | `"help"` | _No description_ |
| `COMMAND_MIXER_EMERGENCY` | string | `"mixer_emergency"` | _No description_ |
| `COMMAND_MIXER_PATH` | string | `"mixer_path"` | _No description_ |
| `COMMAND_MIXER_RESET` | string | `"mixer_reset"` | _No description_ |
| `COMMAND_MIXER_SAVE` | string | `"mixer_save"` | _No description_ |
| `COMMAND_RESET_MIXER_VALUES` | string | `"mixer_reset"` | ===== COMMAND NAME CONSTANTS ===== |
| `COMMAND_SAVE_GAME_STRESS` | string | `"savegamestress"` | _No description_ |
| `COMMAND_SAVE_MONITOR` | string | `"savemonitor"` | _No description_ |
| `COMMAND_SAVE_PREF_STRESS` | string | `"saveprefstress"` | _No description_ |
| `CONFIG_AUTO_SAVE_INTERVAL_MINUTES` | int | `2` | _No description_ |
| `CONFIG_BACKUP_RETENTION_COUNT` | int | `10` | _No description_ |
| `CONFIG_DIRECTORY_NAME` | string | `"Config"` | _No description_ |
| `CONFIG_FILE_CHECK_INTERVAL_MINUTES` | int | `5` | ===== CONFIGURATION CONSTANTS ===== |
| `CONFIG_RELOAD_DELAY_MS` | int | `500` | _No description_ |
| `CONFIG_VALIDATION_TIMEOUT_MS` | int | `5000` | _No description_ |
| `CONSOLE_BYPASS_PARAM_DESC` | string | `"[CONSOLE]   bypass_cooldown - Skip save cooldo...` | _No description_ |
| `CONSOLE_COMMAND_DELAY_MS` | int | `1000` | _No description_ |
| `CONSOLE_COUNT_PARAM_DESC` | string | `"[CONSOLE]   count - Number of save iterations ...` | _No description_ |
| `CONSOLE_DELAY_PARAM_DESC` | string | `"[CONSOLE]   delay_seconds - Delay between save...` | _No description_ |
| `CONSOLE_EXAMPLES_HEADER` | string | `"[CONSOLE] Examples:"` | ===== CONSOLE HELP AND MESSAGES ===== |
| `CONSOLE_FORMATTING_NOTE` | string | `"[CONSOLE] Note: Message preserves all spaces a...` | _No description_ |
| `CONSOLE_INVALID_COUNT_ERROR` | string | `"[CONSOLE] Invalid iteration count '{0}'. Must ...` | _No description_ |
| `CONSOLE_LOG_FILENAME` | string | `"Console.log"` | _No description_ |
| `CONSOLE_MESSAGE_PREFIX` | string | `"[CONSOLE] "` | ===== CONSOLE AND UI CONSTANTS ===== |
| `CONSOLE_MISSING_COUNT_ERROR` | string | `"[CONSOLE] Missing required parameter: count"` | _No description_ |
| `CONSOLE_MISSING_MESSAGE_ERROR` | string | `"[CONSOLE] Missing required parameter: message"` | _No description_ |
| `CONSOLE_OPTIONAL_HEADER` | string | `"[CONSOLE] Optional (auto-detected order):"` | _No description_ |
| `CONSOLE_PARAMETERS_INFO` | string | `"[CONSOLE] Parameters can be in any order after...` | _No description_ |
| `CONSOLE_REQUIRED_HEADER` | string | `"[CONSOLE] Required:"` | _No description_ |
| `COUNT_PROPERTY_NAME` | string | `"Count"` | _No description_ |
| `CPU_USAGE_THRESHOLD_PERCENT` | double | `80.0` | _No description_ |
| `CREATE_FAILURE_METHOD` | string | `"CreateFailure"` | ===== ERROR HANDLING RESULT CONSTANTS ===== |
| `CREATE_SUCCESS_METHOD` | string | `"CreateSuccess"` | _No description_ |
| `CRITICAL_ERROR_THRESHOLD` | int | `10` | _No description_ |
| `CRITICAL_THREAD_PRIORITY` | string | `"Highest"` | _No description_ |
| `CRLF` | string | `"\r\n"` | _No description_ |
| `CSHARP_LANGUAGE_VERSION` | string | `"7.3"` | _No description_ |
| `CULTURE_AR_SA` | string | `"ar-SA"` | _No description_ |
| `CULTURE_BG_BG` | string | `"bg-BG"` | _No description_ |
| `CULTURE_CS_CZ` | string | `"cs-CZ"` | _No description_ |
| `CULTURE_DA_DK` | string | `"da-DK"` | _No description_ |
| `CULTURE_DE_DE` | string | `"de-DE"` | _No description_ |
| `CULTURE_EL_GR` | string | `"el-GR"` | _No description_ |
| `CULTURE_EN_UK` | string | `"en-GB"` | _No description_ |
| `CULTURE_EN_US` | string | `"en-US"` | _No description_ |
| `CULTURE_ES_ES` | string | `"es-ES"` | _No description_ |
| `CULTURE_ET_EE` | string | `"et-EE"` | _No description_ |
| `CULTURE_FI_FI` | string | `"fi-FI"` | _No description_ |
| `CULTURE_FR_FR` | string | `"fr-FR"` | _No description_ |
| `CULTURE_HE_IL` | string | `"he-IL"` | _No description_ |
| `CULTURE_HI_IN` | string | `"hi-IN"` | _No description_ |
| `CULTURE_HR_HR` | string | `"hr-HR"` | _No description_ |
| `CULTURE_HU_HU` | string | `"hu-HU"` | _No description_ |
| `CULTURE_ID_ID` | string | `"id-ID"` | _No description_ |
| `CULTURE_INVARIANT` | string | `""` | ===== CULTURE CONSTANTS ===== |
| `CULTURE_IT_IT` | string | `"it-IT"` | _No description_ |
| `CULTURE_JA_JP` | string | `"ja-JP"` | _No description_ |
| `CULTURE_KO_KR` | string | `"ko-KR"` | _No description_ |
| `CULTURE_LT_LT` | string | `"lt-LT"` | _No description_ |
| `CULTURE_LV_LV` | string | `"lv-LV"` | _No description_ |
| `CULTURE_MS_MY` | string | `"ms-MY"` | _No description_ |
| `CULTURE_NL_NL` | string | `"nl-NL"` | _No description_ |
| `CULTURE_NO_NO` | string | `"no-NO"` | _No description_ |
| `CULTURE_PL_PL` | string | `"pl-PL"` | _No description_ |
| `CULTURE_PT_BR` | string | `"pt-BR"` | _No description_ |
| `CULTURE_RO_RO` | string | `"ro-RO"` | _No description_ |
| `CULTURE_RU_RU` | string | `"ru-RU"` | _No description_ |
| `CULTURE_SK_SK` | string | `"sk-SK"` | _No description_ |
| `CULTURE_SL_SI` | string | `"sl-SI"` | _No description_ |
| `CULTURE_SR_RS` | string | `"sr-RS"` | _No description_ |
| `CULTURE_SV_SE` | string | `"sv-SE"` | _No description_ |
| `CULTURE_TH_TH` | string | `"th-TH"` | _No description_ |
| `CULTURE_TR_TR` | string | `"tr-TR"` | _No description_ |
| `CULTURE_UK_UA` | string | `"uk-UA"` | _No description_ |
| `CULTURE_VI_VN` | string | `"vi-VN"` | _No description_ |
| `CULTURE_ZH_CN` | string | `"zh-CN"` | _No description_ |
| `CULTURE_ZH_TW` | string | `"zh-TW"` | _No description_ |
| `CURRENCY_FORMAT` | string | `"C"` | ===== FORMATTING AND DISPLAY CONSTANTS ===== |
| `CURRENT_DOMAIN_PROPERTY` | string | `"CurrentDomain"` | _No description_ |
| `DATA_DIRECTORY_NAME` | string | `"Data"` | _No description_ |
| `DATA_TYPE_BOOLEAN` | string | `"BOOLEAN"` | ===== DATA TYPE CONSTANTS ===== |
| `DATA_TYPE_DOUBLE` | string | `"DOUBLE"` | _No description_ |
| `DATA_TYPE_FLOAT` | string | `"FLOAT"` | _No description_ |
| `DATA_TYPE_INTEGER` | string | `"INTEGER"` | _No description_ |
| `DATA_TYPE_OBJECT` | string | `"OBJECT"` | _No description_ |
| `DATA_TYPE_STRING` | string | `"STRING"` | _No description_ |
| `DATABASE_DEFAULT_PAGE_SIZE` | int | `4096` | ===== DATABASE AND STORAGE CONSTANTS ===== |
| `DATABASE_POOL_SIZE` | int | `10` | _No description_ |
| `DATABASE_TIMEOUT_THRESHOLD_SECONDS` | double | `15.0` | _No description_ |
| `DATE_FORMAT_LONG` | string | `"MMMM dd, yyyy"` | _No description_ |
| `DATE_FORMAT_SHORT` | string | `"MM/dd/yyyy"` | _No description_ |
| `DEBUG_MODE` | string | `"DEBUG"` | ===== DEBUG AND CONDITIONAL COMPILATION ===== |
| `DECIMAL_FORMAT_ONE_PLACE` | string | `"F1"` | _No description_ |
| `DECIMAL_FORMAT_THREE_PLACES` | string | `"F3"` | _No description_ |
| `DECIMAL_FORMAT_TWO_PLACES` | string | `"F2"` | _No description_ |
| `DEFAULT_CULTURE` | string | `"en-US"` | _No description_ |
| `DEFAULT_DECIMAL_PRECISION` | int | `3` | ===== NUMERIC PRECISION CONSTANTS ===== |
| `DEFAULT_ENCODING` | string | `"UTF-8"` | _No description_ |
| `DEFAULT_FILE_BUFFER_SIZE` | int | `4096` | ===== FILE OPERATION CONSTANTS ===== |
| `DEFAULT_FRAME_RATE` | double | `60.0` | _No description_ |
| `DEFAULT_LANGUAGE` | string | `"en-US"` | ===== CONFIGURATION AND SETTINGS CONSTANTS ===== |
| `DEFAULT_MIXER_ID` | int | `-1` | _No description_ |
| `DEFAULT_OPERATION_DELAY` | float | `0f` | _No description_ |
| `DEFAULT_RETRY_ATTEMPTS` | int | `3` | ===== ERROR HANDLING CONSTANTS ===== |
| `DEFAULT_THREAD_POOL_SIZE` | int | `4` | _No description_ |
| `DEFAULT_THREAD_PRIORITY` | string | `"Normal"` | ===== THREADING CONSTANTS ===== |
| `DELEGATE_PARAM_NAME` | string | `"delegate"` | _No description_ |
| `DELIMITER_AMPERSAND` | string | `"&"` | _No description_ |
| `DELIMITER_AT_SYMBOL` | string | `"@"` | _No description_ |
| `DELIMITER_BACKTICK` | string | `"`"` | _No description_ |
| `DELIMITER_CARET` | string | `"^"` | _No description_ |
| `DELIMITER_COLON` | string | `":"` | _No description_ |
| `DELIMITER_COMMA` | string | `","` | ===== DELIMITER AND SEPARATOR CONSTANTS ===== |
| `DELIMITER_DOLLAR` | string | `"$"` | _No description_ |
| `DELIMITER_EQUALS` | string | `"="` | _No description_ |
| `DELIMITER_EXCLAMATION` | string | `"!"` | _No description_ |
| `DELIMITER_HASH` | string | `"#"` | _No description_ |
| `DELIMITER_MINUS` | string | `"-"` | _No description_ |
| `DELIMITER_PERCENT` | string | `"%"` | _No description_ |
| `DELIMITER_PIPE` | string | `"\|"` | _No description_ |
| `DELIMITER_PLUS` | string | `"+"` | _No description_ |
| `DELIMITER_QUESTION_MARK` | string | `"?"` | _No description_ |
| `DELIMITER_SEMICOLON` | string | `";"` | _No description_ |
| `DELIMITER_SPACE` | string | `" "` | _No description_ |
| `DELIMITER_TAB` | string | `"\t"` | _No description_ |
| `DELIMITER_TILDE` | string | `"~"` | _No description_ |
| `DELIMITER_UNDERSCORE` | string | `"_"` | _No description_ |
| `DETECT_DIRS_IDENTIFIER` | string | `"detectdirs"` | _No description_ |
| `DIAGNOSTIC_CHECK_INTERVAL_SECONDS` | int | `60` | ===== DIAGNOSTIC CONSTANTS ===== |
| `DIAGNOSTIC_CRITICAL_THRESHOLD` | int | `3` | _No description_ |
| `DIAGNOSTIC_ERROR_THRESHOLD` | int | `5` | _No description_ |
| `DIAGNOSTIC_LOG_RETENTION_DAYS` | int | `7` | _No description_ |
| `DIAGNOSTIC_METRIC_INTERVAL_MS` | int | `1000` | _No description_ |
| `DIAGNOSTIC_WARNING_THRESHOLD` | int | `10` | _No description_ |
| `DIAGNOSTICS_PREFIX` | string | `"[DIAGNOSTICS]"` | _No description_ |
| `DIRECTORIES_IDENTIFIER` | string | `"directories"` | _No description_ |
| `DIRECTORY_CREATE_METHOD` | string | `"CreateDirectory"` | _No description_ |
| `DIRECTORY_EXISTS_METHOD` | string | `"Exists"` | _No description_ |
| `DIRECTORY_RESOLVER_PREFIX` | string | `"[DIR-RESOLVER]"` | _No description_ |
| `DISK_USAGE_THRESHOLD_PERCENT` | double | `90.0` | _No description_ |
| `DOUBLE_BACKSLASH` | string | `"\\\\"` | _No description_ |
| `ELLIPSIS` | string | `"..."` | _No description_ |
| `EMAIL_REGEX_PATTERN` | string | `@"^[^@\s]+@[^@\s]+\.[^@\s]+$"` | ===== REGEX AND PATTERN CONSTANTS ===== |
| `EMERGENCY_CLEANUP_TIMEOUT_MS` | int | `2000` | _No description_ |
| `EMERGENCY_FILE_EXTENSION` | string | `".emergency"` | _No description_ |
| `EMERGENCY_SAVE_FILENAME` | string | `"MixerThresholdSave_Emergency.json"` | _No description_ |
| `EMERGENCY_SAVE_PREFIX` | string | `"[EmergencySaveManager]"` | _No description_ |
| `EMPTY_COLLECTION_COUNT` | int | `0` | ===== COLLECTION SIZE CONSTANTS ===== |
| `EMPTY_STRING_ARRAY` | string | `"EmptyStringArray"` | ===== ARRAY AND COLLECTION CONSTANTS ===== |
| `ENCODING_ASCII` | string | `"ASCII"` | _No description_ |
| `ENCODING_BASE64` | string | `"Base64"` | _No description_ |
| `ENCODING_HEX` | string | `"Hexadecimal"` | _No description_ |
| `ENCODING_ISO_8859_1` | string | `"ISO-8859-1"` | _No description_ |
| `ENCODING_UTF16` | string | `"UTF-16"` | _No description_ |
| `ENCODING_UTF32` | string | `"UTF-32"` | _No description_ |
| `ENCODING_UTF8` | string | `"UTF-8"` | ===== ENCODING CONSTANTS ===== |
| `ENCODING_WINDOWS_1252` | string | `"Windows-1252"` | _No description_ |
| `ENCRYPTION_AES` | string | `"AES"` | ===== ENCRYPTION AND SECURITY CONSTANTS ===== |
| `ENCRYPTION_KEY_SIZE` | int | `256` | _No description_ |
| `ENCRYPTION_RSA` | string | `"RSA"` | _No description_ |
| `ENVIRONMENT_HOME` | string | `"HOME"` | _No description_ |
| `ENVIRONMENT_PATH` | string | `"PATH"` | _No description_ |
| `ENVIRONMENT_TEMP` | string | `"TEMP"` | _No description_ |
| `ENVIRONMENT_USER` | string | `"USER"` | _No description_ |
| `ERROR_MSG_ACCESS_DENIED` | string | `"Access denied to specified resource"` | _No description_ |
| `ERROR_MSG_ALREADY_INITIALIZED` | string | `"Component already initialized"` | _No description_ |
| `ERROR_MSG_CONNECTION_FAILED` | string | `"Connection failed"` | _No description_ |
| `ERROR_MSG_DATA_CORRUPTION` | string | `"Data corruption detected"` | _No description_ |
| `ERROR_MSG_EMPTY_STRING` | string | `"String cannot be null or empty"` | _No description_ |
| `ERROR_MSG_FILE_NOT_FOUND` | string | `"File not found at specified path"` | _No description_ |
| `ERROR_MSG_INSUFFICIENT_MEMORY` | string | `"Insufficient memory available"` | _No description_ |
| `ERROR_MSG_INVALID_CONFIGURATION` | string | `"Invalid configuration detected"` | _No description_ |
| `ERROR_MSG_INVALID_FORMAT` | string | `"Invalid format specified"` | _No description_ |
| `ERROR_MSG_INVALID_OPERATION` | string | `"Invalid operation in current state"` | _No description_ |
| `ERROR_MSG_INVALID_RANGE` | string | `"Value is outside the valid range"` | _No description_ |
| `ERROR_MSG_NOT_INITIALIZED` | string | `"Component not initialized"` | _No description_ |
| `ERROR_MSG_NOT_SUPPORTED` | string | `"Operation not supported"` | _No description_ |
| `ERROR_MSG_NULL_PARAMETER` | string | `"Parameter cannot be null"` | ===== ERROR MESSAGE CONSTANTS ===== |
| `ERROR_MSG_OPERATION_CANCELLED` | string | `"Operation was cancelled"` | _No description_ |
| `ERROR_MSG_OPERATION_FAILED` | string | `"Operation failed to complete"` | _No description_ |
| `ERROR_MSG_RESOURCE_EXHAUSTED` | string | `"System resources exhausted"` | _No description_ |
| `ERROR_MSG_TIMEOUT_OCCURRED` | string | `"Operation timed out"` | _No description_ |
| `ERROR_MSG_VERSION_MISMATCH` | string | `"Version mismatch detected"` | _No description_ |
| `ERROR_OPERATION_PREFIX` | string | `"[ERROR] "` | _No description_ |
| `ERROR_RECOVERY_TIMEOUT_MS` | int | `10000` | _No description_ |
| `ESCAPE_ALERT` | string | `"\a"` | _No description_ |
| `ESCAPE_BACKSLASH` | string | `"\\"` | _No description_ |
| `ESCAPE_BACKSPACE` | string | `"\b"` | _No description_ |
| `ESCAPE_CARRIAGE_RETURN` | string | `"\r"` | _No description_ |
| `ESCAPE_FORM_FEED` | string | `"\f"` | _No description_ |
| `ESCAPE_FORWARD_SLASH` | string | `"/"` | _No description_ |
| `ESCAPE_NEWLINE` | string | `"\n"` | _No description_ |
| `ESCAPE_NULL` | string | `"\0"` | _No description_ |
| `ESCAPE_TAB` | string | `"\t"` | _No description_ |
| `ESCAPE_VERTICAL_TAB` | string | `"\v"` | _No description_ |
| `EXCEPTION_MESSAGE_PROPERTY` | string | `"Message"` | ===== REFLECTION AND TYPE CONSTANTS ===== |
| `EXCEPTION_STACKTRACE_PROPERTY` | string | `"StackTrace"` | _No description_ |
| `EXTENDED_LONG_WAIT_SECONDS` | float | `3.0f` | _No description_ |
| `EXTENDED_WAIT_SECONDS` | float | `2.0f` | _No description_ |
| `EXTRA_LARGE_COLLECTION_SIZE` | int | `500` | _No description_ |
| `FAILURE_RESULT` | string | `"FAILURE"` | _No description_ |
| `FIFTY_INT` | int | `50` | _No description_ |
| `FIFTY_INT` | int | `50` | _No description_ |
| `FILE_DELETE_METHOD` | string | `"Delete"` | _No description_ |
| `FILE_EXISTS_METHOD` | string | `"Exists"` | ===== FILE OPERATION METHOD NAMES ===== |
| `FILE_LOCK_PREFIX` | string | `"[FILE_LOCK]"` | _No description_ |
| `FILE_NOT_FOUND_WARNING` | string | `"File not found {0}"` | _No description_ |
| `FILE_OPERATION_TIMEOUT_SECONDS` | float | `30f` | _No description_ |
| `FILE_READ_ALL_TEXT_METHOD` | string | `"ReadAllText"` | _No description_ |
| `FILE_WRITE_ALL_TEXT_METHOD` | string | `"WriteAllText"` | _No description_ |
| `FILENAME_DATETIME_FORMAT` | string | `"yyyy-MM-dd_HH-mm-ss"` | _No description_ |
| `FINAL_CLEANUP_ATTEMPTS` | int | `3` | _No description_ |
| `FIVE_FLOAT` | float | `5.0f` | _No description_ |
| `FIVE_INT` | int | `5` | _No description_ |
| `FIVE_INT` | int | `5` | _No description_ |
| `FORCE_SHUTDOWN_TIMEOUT_SECONDS` | int | `10` | _No description_ |
| `FORWARD_SLASH` | string | `"/"` | _No description_ |
| `FRAME_RATE_THRESHOLD` | double | `30.0` | _No description_ |
| `FRAMEWORK_VERSION_4_8` | float | `4.8f` | ===== PERFORMANCE THRESHOLD CONSTANTS ===== |
| `FUNCTION_PARAM_NAME` | string | `"function"` | _No description_ |
| `GAME_LOW_PERFORMANCE_FPS` | int | `20` | _No description_ |
| `GAME_MAX_FRAME_RATE` | int | `120` | _No description_ |
| `GAME_MIN_FRAME_RATE` | int | `30` | ===== GAME SPECIFIC CONSTANTS ===== |
| `GAME_QUALITY_LEVEL_DEFAULT` | int | `3` | _No description_ |
| `GAME_QUALITY_LEVEL_MAX` | int | `5` | _No description_ |
| `GAME_QUALITY_LEVEL_MIN` | int | `0` | _No description_ |
| `GAME_STATE_BACKUP_INTERVAL_MINUTES` | int | `5` | _No description_ |
| `GAME_STATE_CHECK_INTERVAL_SECONDS` | int | `1` | ===== GAME STATE CONSTANTS ===== |
| `GAME_STATE_EMERGENCY_SAVE_THRESHOLD` | int | `10` | _No description_ |
| `GAME_STATE_PERSISTENCE_INTERVAL_MINUTES` | int | `1` | _No description_ |
| `GAME_STATE_RECOVERY_TIMEOUT_SECONDS` | int | `30` | _No description_ |
| `GAME_STATE_TRANSITION_TIMEOUT_SECONDS` | int | `10` | _No description_ |
| `GAME_STATE_VALIDATION_ATTEMPTS` | int | `3` | _No description_ |
| `GAME_TARGET_FRAME_RATE` | int | `60` | _No description_ |
| `GAME_VSYNC_ENABLED_DEFAULT` | bool | `true` | _No description_ |
| `GC_COLLECTION_THRESHOLD_MB` | double | `100.0` | _No description_ |
| `GENERAL_EXCEPTION_MSG` | string | `"Exception for {0}: {1}\nStack Trace: {2}"` | _No description_ |
| `GET_EXECUTING_ASSEMBLY_METHOD` | string | `"GetExecutingAssembly"` | _No description_ |
| `GET_TYPE_METHOD_NAME` | string | `"GetType"` | _No description_ |
| `GRACEFUL_SHUTDOWN_TIMEOUT_SECONDS` | int | `30` | ===== CLEANUP AND SHUTDOWN CONSTANTS ===== |
| `GRAPHICS_ANISOTROPIC_LEVELS` | int | `16` | _No description_ |
| `GRAPHICS_ANTIALIASING_MAX` | int | `8` | _No description_ |
| `GRAPHICS_ANTIALIASING_MIN` | int | `0` | _No description_ |
| `GRAPHICS_DEFAULT_HEIGHT` | int | `1080` | _No description_ |
| `GRAPHICS_DEFAULT_WIDTH` | int | `1920` | _No description_ |
| `GRAPHICS_LOD_BIAS_MAX` | float | `2.0f` | _No description_ |
| `GRAPHICS_LOD_BIAS_MIN` | float | `0.1f` | _No description_ |
| `GRAPHICS_MAX_HEIGHT` | int | `2160` | _No description_ |
| `GRAPHICS_MAX_WIDTH` | int | `3840` | _No description_ |
| `GRAPHICS_MIN_HEIGHT` | int | `480` | _No description_ |
| `GRAPHICS_MIN_WIDTH` | int | `640` | ===== GRAPHICS CONSTANTS ===== |
| `GRAPHICS_SHADOW_DISTANCE_MAX` | float | `500.0f` | _No description_ |
| `GRAPHICS_SHADOW_DISTANCE_MIN` | float | `10.0f` | _No description_ |
| `GRAPHICS_TEXTURE_QUALITY_MAX` | int | `4` | _No description_ |
| `GRAPHICS_TEXTURE_QUALITY_MIN` | int | `0` | _No description_ |
| `GUID_REGEX_PATTERN` | string | `@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}...` | _No description_ |
| `HALF_DOUBLE` | double | `0.5` | _No description_ |
| `HALF_FLOAT` | float | `0.5f` | _No description_ |
| `HARMONY_MOD_ID` | string | `"MixerThreholdMod.Main"` | ===== MOD IDENTIFICATION CONSTANTS ===== |
| `HASH_MD5` | string | `"MD5"` | _No description_ |
| `HASH_SHA256` | string | `"SHA256"` | _No description_ |
| `HEALTH_CHECK_INTERVAL_SECONDS` | int | `30` | ===== MONITORING CONSTANTS ===== |
| `HEARTBEAT_INTERVAL_SECONDS` | int | `5` | _No description_ |
| `HIGH_MEMORY_THRESHOLD_MB` | double | `512.0` | ===== PERFORMANCE OPTIMIZATION CONSTANTS ===== |
| `HIGH_PERFORMANCE_THRESHOLD` | float | `0.001f` | _No description_ |
| `HIGH_PRECISION_DECIMALS` | int | `6` | _No description_ |
| `HIGH_THREAD_COUNT_THRESHOLD` | int | `50` | _No description_ |
| `HIGH_THREAD_PRIORITY` | string | `"AboveNormal"` | _No description_ |
| `HTTP_DEFAULT_PORT` | int | `80` | _No description_ |
| `HTTP_STATUS_INTERNAL_ERROR` | int | `500` | _No description_ |
| `HTTP_STATUS_NOT_FOUND` | int | `404` | _No description_ |
| `HTTP_STATUS_OK` | int | `200` | ===== HTTP AND NETWORK CONSTANTS ===== |
| `HTTPS_DEFAULT_PORT` | int | `443` | _No description_ |
| `HUNDRED_FLOAT` | float | `100.0f` | _No description_ |
| `HUNDRED_INT` | int | `100` | _No description_ |
| `IL2CPP_BUILD` | string | `"IL2CPP"` | _No description_ |
| `IL2CPP_DOMAIN` | string | `"IL2CPP"` | ===== IL2CPP AND MELONLOADER CONSTANTS ===== |
| `INFO_MSG_CONNECTION_RESTORED` | string | `"Connection restored"` | _No description_ |
| `INFO_MSG_FEATURE_DISABLED` | string | `"Feature disabled"` | _No description_ |
| `INFO_MSG_FEATURE_ENABLED` | string | `"Feature enabled"` | _No description_ |
| `INFO_MSG_MAINTENANCE_SCHEDULED` | string | `"Maintenance scheduled"` | _No description_ |
| `INFO_MSG_NEW_VERSION_AVAILABLE` | string | `"New version available"` | _No description_ |
| `INFO_MSG_OPERATION_STARTED` | string | `"Operation started"` | ===== INFORMATION MESSAGE CONSTANTS ===== |
| `INFO_MSG_PROGRESS_UPDATE` | string | `"Progress update available"` | _No description_ |
| `INFO_MSG_STATUS_CHANGED` | string | `"Status changed"` | _No description_ |
| `INFO_MSG_SYSTEM_READY` | string | `"System ready"` | _No description_ |
| `INFO_MSG_UPDATE_AVAILABLE` | string | `"Update available"` | _No description_ |
| `INFO_OPERATION_PREFIX` | string | `"[INFO] "` | _No description_ |
| `INITIALIZATION_CLEANUP_TIMEOUT_MS` | int | `15000` | _No description_ |
| `INITIALIZATION_MAX_RETRIES` | int | `5` | _No description_ |
| `INITIALIZATION_RETRY_DELAY_MS` | int | `1000` | _No description_ |
| `INITIALIZATION_TIMEOUT_SECONDS` | int | `30` | ===== INITIALIZATION CONSTANTS ===== |
| `INITIALIZATION_VALIDATION_TIMEOUT_MS` | int | `10000` | _No description_ |
| `INPUT_DEADZONE_THRESHOLD` | float | `0.1f` | _No description_ |
| `INPUT_DOUBLE_CLICK_INTERVAL_MS` | int | `300` | _No description_ |
| `INPUT_KEY_REPEAT_DELAY_MS` | int | `500` | _No description_ |
| `INPUT_KEY_REPEAT_RATE_MS` | int | `30` | _No description_ |
| `INPUT_LONG_PRESS_DURATION_MS` | int | `1000` | _No description_ |
| `INPUT_MOUSE_WHEEL_SENSITIVITY` | float | `1.0f` | _No description_ |
| `INPUT_POLLING_RATE_HZ` | int | `120` | ===== INPUT CONSTANTS ===== |
| `INPUT_SENSITIVITY_DEFAULT` | float | `1.0f` | _No description_ |
| `INPUT_SENSITIVITY_MAX` | float | `10.0f` | _No description_ |
| `INPUT_SENSITIVITY_MIN` | float | `0.1f` | _No description_ |
| `INTEGER_FORMAT_NO_DECIMALS` | string | `"F0"` | _No description_ |
| `INVALID_MIXER_ID` | int | `-999` | _No description_ |
| `INVALID_MSG_LEVEL_ERROR` | string | `"[ERROR] Invalid log level {0} for Msg method. ...` | ===== ERROR MESSAGE CONSTANTS ===== |
| `INVALID_WARN_LEVEL_ERROR` | string | `"[ERROR] Invalid log level {0} for Warn method....` | _No description_ |
| `IO_EXCEPTION_MSG` | string | `"IOException for {0}: {1}\nStack Trace: {2}"` | _No description_ |
| `IO_RUNNER_PREFIX` | string | `"[IO_RUNNER]"` | _No description_ |
| `ISO_DATETIME_FORMAT` | string | `"yyyy-MM-ddTHH:mm:ss.fffZ"` | _No description_ |
| `JSON_FILE_EXTENSION` | string | `".json"` | ===== FILE OPERATION CONSTANTS ===== |
| `KB_TO_MB` | int | `1024` | _No description_ |
| `LARGE_COLLECTION_SIZE` | int | `100` | _No description_ |
| `LARGE_MEMORY_ALLOCATION_BYTES` | int | `BYTES_PER_MB` | _No description_ |
| `LATEST_LOG_FILENAME` | string | `"Latest.log"` | ===== LOG FILE NAMES ===== |
| `LENGTH_PROPERTY_NAME` | string | `"Length"` | _No description_ |
| `LIGHTING_COLOR_TEMP_MAX` | float | `40000.0f` | _No description_ |
| `LIGHTING_COLOR_TEMP_MIN` | float | `1000.0f` | _No description_ |
| `LIGHTING_INTENSITY_DEFAULT` | float | `1.0f` | _No description_ |
| `LIGHTING_INTENSITY_MAX` | float | `8.0f` | _No description_ |
| `LIGHTING_INTENSITY_MIN` | float | `0.0f` | ===== LIGHTING CONSTANTS ===== |
| `LIGHTING_RANGE_MAX` | float | `1000.0f` | _No description_ |
| `LIGHTING_RANGE_MIN` | float | `0.1f` | _No description_ |
| `LIGHTING_SHADOW_STRENGTH_MAX` | float | `1.0f` | _No description_ |
| `LIGHTING_SHADOW_STRENGTH_MIN` | float | `0.0f` | _No description_ |
| `LIGHTING_SPOT_ANGLE_MAX` | float | `179.0f` | _No description_ |
| `LIGHTING_SPOT_ANGLE_MIN` | float | `1.0f` | _No description_ |
| `LOAD_TIMEOUT_SECONDS` | float | `30f` | _No description_ |
| `LOCALHOST_ADDRESS` | string | `"127.0.0.1"` | _No description_ |
| `LOCALHOST_HOSTNAME` | string | `"localhost"` | _No description_ |
| `LOCK_FILE_EXTENSION` | string | `".lock"` | _No description_ |
| `LOG_FILE_EXTENSION` | string | `".log"` | _No description_ |
| `LOG_LEVEL_CRITICAL` | int | `1` | ===== LOGGING LEVEL CONSTANTS ===== |
| `LOG_LEVEL_ERR` | string | `"err"` | _No description_ |
| `LOG_LEVEL_IMPORTANT` | int | `2` | _No description_ |
| `LOG_LEVEL_MSG` | string | `"msg"` | ===== LOGGING LEVEL STRINGS ===== |
| `LOG_LEVEL_VERBOSE` | int | `3` | _No description_ |
| `LOG_LEVEL_WARN` | string | `"warn"` | _No description_ |
| `LOGS_DIRECTORY_NAME` | string | `"Logs"` | _No description_ |
| `LONG_WAIT_SECONDS` | float | `1.5f` | _No description_ |
| `MANUAL_OPERATION_PREFIX` | string | `"[MANUAL] "` | _No description_ |
| `MATERIAL_ALPHA_CUTOFF_MAX` | float | `1.0f` | _No description_ |
| `MATERIAL_ALPHA_CUTOFF_MIN` | float | `0.0f` | _No description_ |
| `MATERIAL_EMISSION_INTENSITY_MAX` | float | `10.0f` | _No description_ |
| `MATERIAL_EMISSION_INTENSITY_MIN` | float | `0.0f` | _No description_ |
| `MATERIAL_METALLIC_MAX` | float | `1.0f` | _No description_ |
| `MATERIAL_METALLIC_MIN` | float | `0.0f` | ===== MATERIAL CONSTANTS ===== |
| `MATERIAL_NORMAL_SCALE_MAX` | float | `2.0f` | _No description_ |
| `MATERIAL_NORMAL_SCALE_MIN` | float | `0.0f` | _No description_ |
| `MATERIAL_PARALLAX_SCALE_MAX` | float | `0.1f` | _No description_ |
| `MATERIAL_PARALLAX_SCALE_MIN` | float | `0.0f` | _No description_ |
| `MATERIAL_SMOOTHNESS_MAX` | float | `1.0f` | _No description_ |
| `MATERIAL_SMOOTHNESS_MIN` | float | `0.0f` | _No description_ |
| `MAX_CONCURRENT_OPERATIONS` | int | `10` | _No description_ |
| `MAX_EMAIL_LENGTH` | int | `254` | _No description_ |
| `MAX_ERROR_MESSAGE_LENGTH` | int | `500` | _No description_ |
| `MAX_EXPECTED_SAVE_FILE_SIZE_BYTES` | int | `1048576` | _No description_ |
| `MAX_FILENAME_LENGTH` | int | `255` | _No description_ |
| `MAX_PASSWORD_LENGTH` | int | `128` | _No description_ |
| `MAX_PATH_LENGTH` | int | `260` | _No description_ |
| `MAX_PERFORMANCE_WARNINGS` | int | `20` | _No description_ |
| `MAX_REASONABLE_ARRAY_LENGTH` | int | `1000` | _No description_ |
| `MAX_REASONABLE_COLLECTION_SIZE` | int | `1000` | _No description_ |
| `MAX_REASONABLE_STRING_LENGTH` | int | `1000` | _No description_ |
| `MAX_RETRY_ATTEMPTS` | int | `3` | ===== ERROR HANDLING CONSTANTS ===== |
| `MAX_RETRY_ATTEMPTS` | int | `5` | _No description_ |
| `MAX_RETRY_DELAY_MS` | int | `5000` | _No description_ |
| `MAX_SAFE_COLLECTION_SIZE` | int | `10000` | _No description_ |
| `MAX_SAFE_LOOP_ITERATIONS` | int | `1000` | ===== SAFETY AND LIMITS CONSTANTS ===== |
| `MAX_SAFE_NUMERIC_VALUE` | double | `double.MaxValue` | _No description_ |
| `MAX_SAFE_STRING_LENGTH` | int | `10000` | _No description_ |
| `MAX_SAFE_STRING_LENGTH` | int | `1000` | _No description_ |
| `MAX_THREAD_POOL_SIZE` | int | `16` | _No description_ |
| `MAX_USERNAME_LENGTH` | int | `50` | _No description_ |
| `MB_TO_GB` | int | `1024` | _No description_ |
| `MEDIUM_COLLECTION_SIZE` | int | `50` | _No description_ |
| `MEDIUM_MEMORY_ALLOCATION_BYTES` | int | `65536` | _No description_ |
| `MEDIUM_WAIT_SECONDS` | float | `0.5f` | _No description_ |
| `MEDIUM_WAIT_TIME_SECONDS` | float | `0.5f` | _No description_ |
| `MELONLOADER_ASSEMBLY` | string | `"MelonLoader"` | _No description_ |
| `MELONLOADER_DEPENDENCIES_DIR` | string | `"Dependencies"` | _No description_ |
| `MELONLOADER_LIBS_DIR` | string | `"MelonLoader"` | _No description_ |
| `MELONLOADER_LOG_FILENAME` | string | `"MelonLoader.log"` | _No description_ |
| `MELONLOADER_MODS_DIR` | string | `"Mods"` | _No description_ |
| `MELONLOADER_PLUGINS_DIR` | string | `"Plugins"` | _No description_ |
| `MELONLOADER_USER_LIBS_DIR` | string | `"UserLibs"` | _No description_ |
| `MEMORY_CLEANUP_THRESHOLD_PERCENT` | double | `75.0` | _No description_ |
| `MEMORY_CRITICAL_THRESHOLD_PERCENT` | double | `95.0` | _No description_ |
| `MEMORY_FREED_LOG_THRESHOLD_MB` | double | `1.0` | _No description_ |
| `MEMORY_LEAK_THRESHOLD_MB` | double | `100.0` | ===== SYSTEM MONITORING CONSTANTS ===== |
| `MEMORY_OPTIMIZATION_THRESHOLD_KB` | double | `512.0` | _No description_ |
| `MEMORY_OPTIMIZATION_THRESHOLD_MB` | double | `0.5` | _No description_ |
| `MEMORY_PRESSURE_THRESHOLD_MB` | double | `128.0` | ===== ADDITIONAL PERFORMANCE CONSTANTS ===== |
| `MEMORY_SIZE_PRECISION` | int | `2` | _No description_ |
| `MEMORY_THRESHOLD_BYTES` | double | `1048576.0` | _No description_ |
| `MEMORY_THRESHOLD_BYTES` | long | `BYTES_PER_MB` | _No description_ |
| `MEMORY_THRESHOLD_KB` | double | `1024.0` | _No description_ |
| `MEMORY_WARNING_THRESHOLD_PERCENT` | double | `85.0` | _No description_ |
| `MIN_PASSWORD_LENGTH` | int | `8` | ===== VALIDATION RULES CONSTANTS ===== |
| `MIN_THREAD_POOL_SIZE` | int | `2` | _No description_ |
| `MIN_USERNAME_LENGTH` | int | `3` | _No description_ |
| `MIN_VALID_ARRAY_LENGTH` | int | `1` | _No description_ |
| `MIN_VALID_FILE_SIZE_BYTES` | int | `10` | ===== VALIDATION CONSTANTS ===== |
| `MIN_VALID_JSON_LENGTH` | int | `2` | _No description_ |
| `MIN_VALID_NUMERIC_VALUE` | double | `0.0` | _No description_ |
| `MIN_VALID_STRING_LENGTH` | int | `1` | ===== VALIDATION CONSTANTS ===== |
| `MINUTES_TO_HOURS` | int | `60` | _No description_ |
| `MIXER_ACTIVATION_DELAY_MS` | int | `100` | ===== MIXER SPECIFIC OPERATIONAL CONSTANTS ===== |
| `MIXER_ATTACH_ERROR_MSG` | string | `"AttachListenerWhenReady error for Mixer {0}: {...` | _No description_ |
| `MIXER_ATTACH_FINISHED_MSG` | string | `"AttachListenerWhenReady: Finished for Mixer {0}"` | _No description_ |
| `MIXER_ATTACH_STARTED_MSG` | string | `"AttachListenerWhenReady: Started for Mixer {0}"` | ===== MIXER OPERATION MESSAGES ===== |
| `MIXER_AUTO_SAVE_THRESHOLD_CHANGES` | int | `5` | _No description_ |
| `MIXER_BATCH_PROCESSING_SIZE` | int | `50` | _No description_ |
| `MIXER_COMPONENT_SCAN_INTERVAL_SECONDS` | int | `10` | _No description_ |
| `MIXER_CONFIG_ENABLED_DEFAULT` | bool | `true` | _No description_ |
| `MIXER_CONFIG_VALIDATION_TIMEOUT_MS` | int | `2000` | _No description_ |
| `MIXER_DATA_READER_PREFIX` | string | `"[MixerDataReader]"` | _No description_ |
| `MIXER_DATA_SYNC_INTERVAL_SECONDS` | int | `30` | _No description_ |
| `MIXER_DEACTIVATION_DELAY_MS` | int | `50` | _No description_ |
| `MIXER_ID_KEY` | string | `"MixerID"` | _No description_ |
| `MIXER_PRIORITY_QUEUE_CAPACITY` | int | `100` | _No description_ |
| `MIXER_SAVE_FILENAME` | string | `"MixerThresholdSave.json"` | _No description_ |
| `MIXER_START_THRESHOLD_FOUND_MSG` | string | `"AttachListenerWhenReady: StartThrehold found f...` | _No description_ |
| `MIXER_THRESHOLD_CHANGE_SENSITIVITY` | float | `0.01f` | _No description_ |
| `MIXER_THRESHOLD_MAX` | float | `20f` | _No description_ |
| `MIXER_THRESHOLD_MIN` | float | `1f` | ===== MIXER CONFIGURATION CONSTANTS ===== |
| `MIXER_THRESHOLD_VALIDATION_ATTEMPTS` | int | `3` | _No description_ |
| `MIXER_USING_POLLING_MSG` | string | `"AttachListenerWhenReady: Using polling method ...` | _No description_ |
| `MIXER_VALUE_TOLERANCE` | float | `0.001f` | _No description_ |
| `MIXER_VALUES_KEY` | string | `"MixerValues"` | ===== KEY NAME CONSTANTS ===== |
| `MOD_NAME` | string | `"MixerThreholdMod"` | _No description_ |
| `MOD_VERSION` | string | `"1.0.0"` | _No description_ |
| `MODERATE_WAIT_SECONDS` | float | `0.8f` | _No description_ |
| `MONITORING_DATA_RETENTION_HOURS` | int | `24` | _No description_ |
| `MONO_BUILD` | string | `"MONO"` | _No description_ |
| `MS_PER_SECOND` | int | `1000` | _No description_ |
| `MS_TO_SECONDS` | int | `1000` | _No description_ |
| `NETWORK_BUFFER_SIZE_BYTES` | int | `8192` | _No description_ |
| `NETWORK_CONNECTION_TIMEOUT_MS` | int | `10000` | ===== NETWORK AND COMMUNICATION CONSTANTS ===== |
| `NETWORK_MAX_RETRY_ATTEMPTS` | int | `3` | _No description_ |
| `NETWORK_READ_TIMEOUT_MS` | int | `5000` | _No description_ |
| `NETWORK_RETRY_DELAY_MS` | int | `1000` | _No description_ |
| `NETWORK_TIMEOUT_THRESHOLD_SECONDS` | double | `30.0` | _No description_ |
| `NETWORK_WRITE_TIMEOUT_MS` | int | `5000` | _No description_ |
| `NEWLINE` | string | `"\n"` | _No description_ |
| `NO_MIXER_DATA_ERROR` | string | `"[CONSOLE] No mixer data to save. Try adjusting...` | _No description_ |
| `NO_SAVE_PATH_ERROR` | string | `"[CONSOLE] No current save path available. Load...` | _No description_ |
| `NULL_COMMAND_FALLBACK` | string | `"[null_command]"` | ===== FALLBACK AND DEFAULT VALUES ===== |
| `NULL_MESSAGE_FALLBACK` | string | `"[null_message]"` | _No description_ |
| `NULL_PATH_FALLBACK` | string | `"[null_path]"` | _No description_ |
| `NULL_STRING_FALLBACK` | string | `"[null_string]"` | _No description_ |
| `NUMBER_FORMAT_2_DECIMALS` | string | `"F2"` | _No description_ |
| `NUMBER_FORMAT_3_DECIMALS` | string | `"F3"` | _No description_ |
| `ONE_DOUBLE` | double | `1.0` | _No description_ |
| `ONE_FLOAT` | float | `1.0f` | _No description_ |
| `ONE_FLOAT` | float | `1.0f` | _No description_ |
| `ONE_HUNDRED_INT` | int | `100` | _No description_ |
| `ONE_INT` | int | `1` | _No description_ |
| `ONE_INT` | int | `1` | _No description_ |
| `ONE_LONG` | long | `1L` | _No description_ |
| `ONE_THOUSAND_INT` | int | `1000` | _No description_ |
| `OPERATION_CONTEXT_PARAM` | string | `"operationContext"` | _No description_ |
| `OPERATION_FAILED_MSG` | string | `"Operation failed for {0}: {1}\nStack Trace: {2}"` | _No description_ |
| `OPERATION_SUCCESS_MSG` | string | `"Operation succeeded for {0}"` | _No description_ |
| `OPERATION_TIMEOUT_MS` | int | `2000` | ===== TIMEOUT AND PERFORMANCE CONSTANTS ===== |
| `OPTIMIZATION_INTERVAL_SECONDS` | double | `5.0` | _No description_ |
| `OPTIMIZATION_INTERVAL_SECONDS` | int | `30` | _No description_ |
| `ORDER_BY_DESCENDING_METHOD` | string | `"OrderByDescending"` | _No description_ |
| `PARTICLE_EMISSION_RATE_MAX` | float | `1000.0f` | _No description_ |
| `PARTICLE_EMISSION_RATE_MIN` | float | `0.0f` | _No description_ |
| `PARTICLE_GRAVITY_MODIFIER_MAX` | float | `2.0f` | _No description_ |
| `PARTICLE_GRAVITY_MODIFIER_MIN` | float | `-2.0f` | _No description_ |
| `PARTICLE_LIFETIME_MAX` | float | `100.0f` | _No description_ |
| `PARTICLE_LIFETIME_MIN` | float | `0.1f` | _No description_ |
| `PARTICLE_START_SIZE_MAX` | float | `10.0f` | _No description_ |
| `PARTICLE_START_SIZE_MIN` | float | `0.01f` | _No description_ |
| `PARTICLE_START_SPEED_MAX` | float | `100.0f` | _No description_ |
| `PARTICLE_START_SPEED_MIN` | float | `0.0f` | _No description_ |
| `PARTICLE_SYSTEM_MAX_PARTICLES` | int | `10000` | ===== PARTICLE SYSTEM CONSTANTS ===== |
| `PATCH_ENTITY_DESTROY_NAME` | string | `"EntityConfiguration_Destroy_Patch"` | _No description_ |
| `PATCH_OPERATION_PREFIX` | string | `"[PATCH] "` | _No description_ |
| `PATH_COMBINE_METHOD` | string | `"Combine"` | _No description_ |
| `PATH_GET_DIRECTORY_NAME_METHOD` | string | `"GetDirectoryName"` | _No description_ |
| `PATH_GET_FILENAME_METHOD` | string | `"GetFileName"` | _No description_ |
| `PATHS_IDENTIFIER` | string | `"paths"` | _No description_ |
| `PENDING_JUSTIFICATION` | string | `"&lt;Pending&gt;"` | ===== SPECIAL IDENTIFIERS ===== |
| `PERCENTAGE_FORMAT` | string | `"P"` | _No description_ |
| `PERCENTAGE_FORMAT` | string | `"P0"` | _No description_ |
| `PERCENTAGE_FORMAT_ONE_DECIMAL` | string | `"P1"` | _No description_ |
| `PERFORMANCE_CRITICAL_THRESHOLD` | float | `2.0f` | _No description_ |
| `PERFORMANCE_METRICS_HISTORY_COUNT` | int | `100` | _No description_ |
| `PERFORMANCE_METRICS_PREFIX` | string | `"[MixerDataPerformanceMetrics]"` | _No description_ |
| `PERFORMANCE_OPTIMIZER_NAME` | string | `"PerformanceOptimizer"` | ===== COMPONENT NAME CONSTANTS ===== |
| `PERFORMANCE_SAMPLE_INTERVAL_MS` | int | `100` | _No description_ |
| `PERFORMANCE_SAMPLE_RATE` | float | `0.1f` | _No description_ |
| `PERFORMANCE_SLOW_THRESHOLD_MS` | int | `50` | _No description_ |
| `PERFORMANCE_SUMMARY_LOG_INTERVAL` | int | `50` | _No description_ |
| `PERFORMANCE_TOLERANCE` | float | `1.5f` | _No description_ |
| `PERFORMANCE_WARNING_THRESHOLD_MS` | int | `100` | _No description_ |
| `PERFORMANCE_WARNINGS_CLEANUP_BATCH` | int | `10` | _No description_ |
| `PERSISTENCE_PREFIX` | string | `"[PERSISTENCE]"` | _No description_ |
| `PHONE_REGEX_PATTERN` | string | `@"^\+?[\d\s\-\(\)]{10,}$"` | _No description_ |
| `PHYSICS_ANGULAR_VELOCITY_THRESHOLD` | float | `0.1f` | _No description_ |
| `PHYSICS_BOUNCE_THRESHOLD` | float | `2.0f` | _No description_ |
| `PHYSICS_COLLISION_MODE_CONTINUOUS` | int | `1` | _No description_ |
| `PHYSICS_COLLISION_MODE_DISCRETE` | int | `0` | _No description_ |
| `PHYSICS_FRICTION_COEFFICIENT_DEFAULT` | float | `0.6f` | _No description_ |
| `PHYSICS_GRAVITY_DEFAULT` | float | `-9.81f` | ===== PHYSICS CONSTANTS ===== |
| `PHYSICS_RESTITUTION_COEFFICIENT_DEFAULT` | float | `0.0f` | _No description_ |
| `PHYSICS_SOLVER_ITERATIONS_DEFAULT` | int | `6` | _No description_ |
| `PHYSICS_TIME_STEP_FIXED` | float | `0.016666f` | _No description_ |
| `PHYSICS_VELOCITY_THRESHOLD` | float | `0.1f` | _No description_ |
| `POLL_INTERVAL_SECONDS` | float | `0.2f` | _No description_ |
| `PREDICATE_PARAM_NAME` | string | `"predicate"` | ===== PREDICATE AND FUNCTION CONSTANTS ===== |
| `QUALITY_CHECK_INTERVAL_OPERATIONS` | int | `100` | ===== QUALITY ASSURANCE CONSTANTS ===== |
| `QUALITY_CRITICAL_THRESHOLD_PERCENT` | double | `80.0` | _No description_ |
| `QUALITY_METRICS_HISTORY_COUNT` | int | `1000` | _No description_ |
| `QUALITY_THRESHOLD_PERCENT` | double | `95.0` | _No description_ |
| `QUALITY_WARNING_THRESHOLD_PERCENT` | double | `90.0` | _No description_ |
| `QUARTER_FLOAT` | float | `0.25f` | _No description_ |
| `QUESTION_MARK` | string | `"?"` | _No description_ |
| `QUICK_WAIT_SECONDS` | float | `0.3f` | _No description_ |
| `QUOTE_DOUBLE` | string | `"\""` | _No description_ |
| `QUOTE_SINGLE` | string | `"'"` | ===== QUOTE AND ESCAPE CONSTANTS ===== |
| `REGISTRY_HKEY_CURRENT_USER` | string | `"HKEY_CURRENT_USER"` | ===== REGISTRY AND SYSTEM CONSTANTS ===== |
| `REGISTRY_HKEY_LOCAL_MACHINE` | string | `"HKEY_LOCAL_MACHINE"` | _No description_ |
| `RELEASE_MODE` | string | `"RELEASE"` | _No description_ |
| `RESOURCE_CLEANUP_TIMEOUT_MS` | int | `5000` | _No description_ |
| `RETRY_DELAY_BASE_MS` | int | `100` | _No description_ |
| `RETRY_DELAY_MS` | int | `500` | _No description_ |
| `RETRY_DELAY_MULTIPLIER` | int | `2` | _No description_ |
| `SALT_SIZE` | int | `16` | _No description_ |
| `SAVE_COOLDOWN_SECONDS` | int | `2` | _No description_ |
| `SAVE_DATA_IDENTIFIER` | string | `"saveData"` | _No description_ |
| `SAVE_MANAGER_PATCH_PREFIX` | string | `"[SaveManager_Save_Patch]"` | _No description_ |
| `SAVE_MANAGER_TYPE_NOT_FOUND_MSG` | string | `"[PATCH] SaveManager type not found - patch wil...` | ===== SYSTEM AND REFLECTION MESSAGES ===== |
| `SAVE_TIME_KEY` | string | `"SaveTime"` | _No description_ |
| `SCHEDULE_I_DATA_DIR` | string | `"Schedule I_Data"` | _No description_ |
| `SECONDS_PER_MINUTE` | int | `60` | _No description_ |
| `SECONDS_TO_MINUTES` | int | `60` | _No description_ |
| `SECURITY_HASH_SALT_LENGTH` | int | `32` | _No description_ |
| `SECURITY_LOCKOUT_DURATION_MINUTES` | int | `15` | _No description_ |
| `SECURITY_MAX_FAILED_ATTEMPTS` | int | `5` | _No description_ |
| `SECURITY_TOKEN_EXPIRY_MINUTES` | int | `60` | _No description_ |
| `SECURITY_VALIDATION_TIMEOUT_MS` | int | `3000` | ===== SECURITY CONSTANTS ===== |
| `SETTINGS_CONFIG_FILENAME` | string | `"settings.json"` | _No description_ |
| `SHADER_COMPILATION_TIMEOUT_MS` | int | `30000` | ===== SHADER CONSTANTS ===== |
| `SHADER_FLOAT_PRECISION` | int | `6` | _No description_ |
| `SHADER_KEYWORD_MAX_COUNT` | int | `256` | _No description_ |
| `SHADER_PASS_MAX_COUNT` | int | `8` | _No description_ |
| `SHADER_TEXTURE_SLOT_MAX` | int | `16` | _No description_ |
| `SHADER_UNIFORM_BUFFER_SIZE_MAX` | int | `65536` | _No description_ |
| `SHADER_VERTEX_ATTRIBUTE_MAX` | int | `16` | _No description_ |
| `SHORT_WAIT_SECONDS` | float | `0.1f` | ===== WAIT TIME CONSTANTS ===== |
| `SINGLE_ITEM_COUNT` | int | `1` | _No description_ |
| `SINGLE_SPACE` | string | `" "` | ===== SPECIAL CHARACTER CONSTANTS ===== |
| `SIXTY_FLOAT` | float | `60.0f` | _No description_ |
| `SKIP_METHOD` | string | `"Skip"` | _No description_ |
| `SMALL_COLLECTION_SIZE` | int | `10` | _No description_ |
| `SMALL_MEMORY_ALLOCATION_BYTES` | int | `1024` | _No description_ |
| `SQL_DELETE` | string | `"DELETE"` | _No description_ |
| `SQL_FROM` | string | `"FROM"` | _No description_ |
| `SQL_INSERT` | string | `"INSERT"` | _No description_ |
| `SQL_SELECT` | string | `"SELECT"` | _No description_ |
| `SQL_UPDATE` | string | `"UPDATE"` | _No description_ |
| `SQL_WHERE` | string | `"WHERE"` | _No description_ |
| `STANDARD_DATETIME_FORMAT` | string | `"yyyy-MM-dd HH:mm:ss"` | ===== DATETIME FORMAT CONSTANTS ===== |
| `STANDARD_WAIT_SECONDS` | float | `1.0f` | _No description_ |
| `STANDARD_WAIT_TIME_SECONDS` | float | `1.0f` | _No description_ |
| `STARTING_BACKUP_CLEANUP_MSG` | string | `"Starting backup cleanup process for: {0}"` | _No description_ |
| `STATUS_REPORT_INTERVAL_MINUTES` | int | `10` | _No description_ |
| `STRESS_TEST_PREFIX` | string | `"[STRESS_TEST]"` | _No description_ |
| `STRING_CONTAINS_METHOD` | string | `"Contains"` | _No description_ |
| `STRING_FORMAT_FIVE_ARGS` | string | `"{0} {1} {2} {3} {4}"` | _No description_ |
| `STRING_FORMAT_FOUR_ARGS` | string | `"{0} {1} {2} {3}"` | _No description_ |
| `STRING_FORMAT_METHOD` | string | `"Format"` | ===== STRING OPERATION CONSTANTS ===== |
| `STRING_FORMAT_ONE_ARG` | string | `"{0}"` | ===== STRING FORMATTING CONSTANTS ===== |
| `STRING_FORMAT_THREE_ARGS` | string | `"{0} {1} {2}"` | _No description_ |
| `STRING_FORMAT_TWO_ARGS` | string | `"{0} {1}"` | _No description_ |
| `STRING_INDEX_OF_METHOD` | string | `"IndexOf"` | _No description_ |
| `STRING_IS_NULL_OR_EMPTY_METHOD` | string | `"IsNullOrEmpty"` | _No description_ |
| `STRING_SPLIT_METHOD` | string | `"Split"` | _No description_ |
| `STRING_STARTS_WITH_METHOD` | string | `"StartsWith"` | _No description_ |
| `STRING_SUBSTRING_METHOD` | string | `"Substring"` | _No description_ |
| `SUCCESS_MSG_BACKUP_CREATED` | string | `"Backup created successfully"` | _No description_ |
| `SUCCESS_MSG_CLEANUP_COMPLETED` | string | `"Cleanup completed successfully"` | _No description_ |
| `SUCCESS_MSG_CONNECTION_ESTABLISHED` | string | `"Connection established successfully"` | _No description_ |
| `SUCCESS_MSG_DATA_SYNCHRONIZED` | string | `"Data synchronized successfully"` | _No description_ |
| `SUCCESS_MSG_FILE_LOADED` | string | `"File loaded successfully"` | _No description_ |
| `SUCCESS_MSG_FILE_SAVED` | string | `"File saved successfully"` | _No description_ |
| `SUCCESS_MSG_INITIALIZATION_COMPLETE` | string | `"Initialization completed successfully"` | _No description_ |
| `SUCCESS_MSG_OPERATION_COMPLETED` | string | `"Operation completed successfully"` | ===== SUCCESS MESSAGE CONSTANTS ===== |
| `SUCCESS_MSG_SETTINGS_APPLIED` | string | `"Settings applied successfully"` | _No description_ |
| `SUCCESS_MSG_VALIDATION_PASSED` | string | `"Validation passed successfully"` | _No description_ |
| `SUCCESS_RESULT` | string | `"SUCCESS"` | _No description_ |
| `SYNC_CONTEXT_TIMEOUT_MS` | int | `3000` | _No description_ |
| `SYSTEM_HEALTH_CHECK_INTERVAL_SECONDS` | int | `30` | _No description_ |
| `SYSTEM_MONITORING_LOG_INTERVAL` | int | `5` | _No description_ |
| `SYSTEM_MONITORING_THRESHOLD` | double | `3.0` | _No description_ |
| `TAB` | string | `"\t"` | _No description_ |
| `TAKE_METHOD` | string | `"Take"` | _No description_ |
| `TARGET_FRAMEWORK_NET48` | string | `".NETFramework,Version=v4.8.1"` | _No description_ |
| `TARGET_PLATFORM_ANY_CPU` | string | `"AnyCPU"` | _No description_ |
| `TARGET_PLATFORM_X64` | string | `"x64"` | _No description_ |
| `TARGET_PLATFORM_X86` | string | `"x86"` | _No description_ |
| `TASK_COMPLETION_TIMEOUT_MS` | int | `15000` | _No description_ |
| `TASK_SCHEDULER_TIMEOUT_MS` | int | `5000` | _No description_ |
| `TEMP_DIRECTORY_NAME` | string | `"Temp"` | _No description_ |
| `TEMP_FILE_EXTENSION` | string | `".tmp"` | _No description_ |
| `TEN_FLOAT` | float | `10.0f` | _No description_ |
| `TEN_INT` | int | `10` | _No description_ |
| `TEN_INT` | int | `10` | _No description_ |
| `TERRAIN_BASE_MAP_DISTANCE_MAX` | float | `20000.0f` | _No description_ |
| `TERRAIN_BASE_MAP_DISTANCE_MIN` | float | `0.0f` | _No description_ |
| `TERRAIN_BILLBOARD_START_MAX` | float | `2000.0f` | _No description_ |
| `TERRAIN_BILLBOARD_START_MIN` | float | `5.0f` | _No description_ |
| `TERRAIN_CONTROL_TEX_RES_MAX` | int | `2048` | _No description_ |
| `TERRAIN_CONTROL_TEX_RES_MIN` | int | `16` | _No description_ |
| `TERRAIN_DETAIL_RES_MAX` | int | `4048` | _No description_ |
| `TERRAIN_DETAIL_RES_MIN` | int | `0` | _No description_ |
| `TERRAIN_HEIGHT_MAP_RES_MAX` | int | `4097` | _No description_ |
| `TERRAIN_HEIGHT_MAP_RES_MIN` | int | `33` | ===== TERRAIN CONSTANTS ===== |
| `TERRAIN_TREE_DISTANCE_MAX` | float | `5000.0f` | _No description_ |
| `TERRAIN_TREE_DISTANCE_MIN` | float | `0.0f` | _No description_ |
| `TEXT_FILE_EXTENSION` | string | `".txt"` | _No description_ |
| `TEXTURE_ANISO_LEVEL_MAX` | int | `16` | _No description_ |
| `TEXTURE_ANISO_LEVEL_MIN` | int | `1` | _No description_ |
| `TEXTURE_COMPRESSION_QUALITY_MAX` | int | `100` | _No description_ |
| `TEXTURE_COMPRESSION_QUALITY_MIN` | int | `0` | _No description_ |
| `TEXTURE_FILTER_MODE_BILINEAR` | int | `1` | _No description_ |
| `TEXTURE_FILTER_MODE_POINT` | int | `0` | _No description_ |
| `TEXTURE_FILTER_MODE_TRILINEAR` | int | `2` | _No description_ |
| `TEXTURE_MIP_LEVEL_MAX` | int | `14` | _No description_ |
| `TEXTURE_SIZE_MAX` | int | `16384` | _No description_ |
| `TEXTURE_SIZE_MIN` | int | `1` | ===== TEXTURE CONSTANTS ===== |
| `TEXTURE_WRAP_MODE_CLAMP` | int | `1` | _No description_ |
| `TEXTURE_WRAP_MODE_REPEAT` | int | `0` | _No description_ |
| `THIRTY_FLOAT` | float | `30.0f` | _No description_ |
| `THIRTY_INT` | int | `30` | _No description_ |
| `THOUSAND_FLOAT` | float | `1000.0f` | _No description_ |
| `THOUSAND_INT` | int | `1000` | _No description_ |
| `THREAD_CLEANUP_TIMEOUT_MS` | int | `30000` | _No description_ |
| `THREAD_JOIN_TIMEOUT_MS` | int | `10000` | _No description_ |
| `THREAD_LOCK_TIMEOUT_MS` | int | `5000` | ===== THREAD SAFETY CONSTANTS ===== |
| `THREAD_MONITORING_INTERVAL_MS` | int | `5000` | _No description_ |
| `THREAD_POOL_MAX_THREADS` | int | `50` | _No description_ |
| `THREAD_POOL_MIN_THREADS` | int | `2` | _No description_ |
| `THREAD_SLEEP_MAX_MS` | int | `10000` | _No description_ |
| `THREAD_SLEEP_MIN_MS` | int | `1` | ===== THREAD AND SYNCHRONIZATION CONSTANTS ===== |
| `THREE_FLOAT` | float | `3.0f` | _No description_ |
| `THREE_INT` | int | `3` | _No description_ |
| `THREE_INT` | int | `3` | _No description_ |
| `THREE_QUARTERS_FLOAT` | float | `0.75f` | _No description_ |
| `THRESHOLD_VALUE_KEY` | string | `"ThresholdValue"` | _No description_ |
| `TIME_FORMAT_12_HOUR` | string | `"hh:mm:ss tt"` | _No description_ |
| `TIME_FORMAT_24_HOUR` | string | `"HH:mm:ss"` | _No description_ |
| `TIMEOUT_MS_PARAM` | string | `"timeoutMs"` | ===== PARAMETER NAME CONSTANTS ===== |
| `TINY_WAIT_SECONDS` | float | `0.05f` | _No description_ |
| `TWENTY_FLOAT` | float | `20.0f` | _No description_ |
| `TWENTY_INT` | int | `20` | _No description_ |
| `TWENTY_INT` | int | `20` | _No description_ |
| `TWO_DOUBLE` | double | `2.0` | _No description_ |
| `TWO_FLOAT` | float | `2.0f` | _No description_ |
| `TWO_FLOAT` | float | `2.0f` | _No description_ |
| `TWO_INT` | int | `2` | _No description_ |
| `TWO_INT` | int | `2` | _No description_ |
| `TWO_LONG` | long | `2L` | _No description_ |
| `UI_ANIMATION_DURATION_MS` | int | `300` | _No description_ |
| `UI_ELEMENT_FADE_DURATION_MS` | int | `150` | _No description_ |
| `UI_ELEMENT_MIN_HEIGHT_PIXELS` | int | `20` | _No description_ |
| `UI_ELEMENT_MIN_WIDTH_PIXELS` | int | `50` | _No description_ |
| `UI_NOTIFICATION_DISPLAY_DURATION_SECONDS` | int | `3` | _No description_ |
| `UI_PANEL_DEFAULT_HEIGHT_PIXELS` | int | `200` | _No description_ |
| `UI_PANEL_DEFAULT_WIDTH_PIXELS` | int | `300` | _No description_ |
| `UI_POPUP_TIMEOUT_SECONDS` | int | `10` | _No description_ |
| `UI_TOOLTIP_DISPLAY_DELAY_MS` | int | `500` | _No description_ |
| `UI_UPDATE_INTERVAL_MS` | int | `16` | ===== USER INTERFACE CONSTANTS ===== |
| `ULTRA_LONG_WAIT_SECONDS` | float | `10.0f` | _No description_ |
| `ULTRA_SHORT_WAIT_SECONDS` | float | `0.01f` | ===== EXTENDED WAIT TIME CONSTANTS ===== |
| `UNAVAILABLE_STATUS` | string | `"UNAVAILABLE"` | _No description_ |
| `UNITY_DATA_PATH_COMMENT` | string | `"Unity Application.dataPath points to \"Schedul...` | ===== DIRECTORY AND PATH CONSTANTS ===== |
| `UNITY_ENGINE_ASSEMBLY` | string | `"UnityEngine"` | _No description_ |
| `UNITY_INPUT_HORIZONTAL` | string | `"Horizontal"` | _No description_ |
| `UNITY_INPUT_MOUSE_X` | string | `"Mouse X"` | _No description_ |
| `UNITY_INPUT_MOUSE_Y` | string | `"Mouse Y"` | _No description_ |
| `UNITY_INPUT_VERTICAL` | string | `"Vertical"` | _No description_ |
| `UNITY_LAYER_DEFAULT` | string | `"Default"` | _No description_ |
| `UNITY_LAYER_UI` | string | `"UI"` | _No description_ |
| `UNITY_TAG_ENEMY` | string | `"Enemy"` | _No description_ |
| `UNITY_TAG_MAIN_CAMERA` | string | `"MainCamera"` | _No description_ |
| `UNITY_TAG_PLAYER` | string | `"Player"` | ===== UNITY SPECIFIC CONSTANTS ===== |
| `UNITY_TAG_UI` | string | `"UI"` | _No description_ |
| `URL_REGEX_PATTERN` | string | `@"^https?://[^\s/$.?#].[^\s]*$"` | _No description_ |
| `USER_PREFERENCES_FILENAME` | string | `"preferences.json"` | _No description_ |
| `UTC_DATETIME_FORMAT_WITH_MS` | string | `"yyyy-MM-dd HH:mm:ss.fff"` | _No description_ |
| `UTILS_PREFIX` | string | `"[UTILS]"` | _No description_ |
| `VERSION_KEY` | string | `"Version"` | _No description_ |
| `VERSION_REGEX_PATTERN` | string | `@"^\d+\.\d+\.\d+(\.\d+)?$"` | _No description_ |
| `VERY_LONG_WAIT_SECONDS` | float | `5.0f` | _No description_ |
| `WARN_LEVEL_CRITICAL` | int | `1` | _No description_ |
| `WARN_LEVEL_VERBOSE` | int | `2` | _No description_ |
| `WARNING_MSG_BACKUP_RECOMMENDED` | string | `"Backup recommended before proceeding"` | _No description_ |
| `WARNING_MSG_COMPATIBILITY_ISSUE` | string | `"Compatibility issue detected"` | _No description_ |
| `WARNING_MSG_CONFIGURATION_MISMATCH` | string | `"Configuration mismatch detected"` | _No description_ |
| `WARNING_MSG_DEPRECATED_FEATURE` | string | `"Using deprecated feature"` | _No description_ |
| `WARNING_MSG_DISK_SPACE_LOW` | string | `"Low disk space warning"` | _No description_ |
| `WARNING_MSG_MEMORY_USAGE_HIGH` | string | `"High memory usage detected"` | _No description_ |
| `WARNING_MSG_PERFORMANCE_DEGRADED` | string | `"Performance degradation detected"` | ===== WARNING MESSAGE CONSTANTS ===== |
| `WARNING_MSG_RESOURCE_LIMITATION` | string | `"Resource limitation encountered"` | _No description_ |
| `WARNING_MSG_TEMPORARY_FAILURE` | string | `"Temporary failure occurred"` | _No description_ |
| `WARNING_MSG_UNSTABLE_OPERATION` | string | `"Operation may be unstable"` | _No description_ |
| `WARNING_OPERATION_PREFIX` | string | `"[WARNING] "` | _No description_ |
| `WATCHDOG_TIMEOUT_SECONDS` | int | `60` | _No description_ |
| `ZERO_DOUBLE` | double | `0.0` | _No description_ |
| `ZERO_FLOAT` | float | `0.0f` | ===== EXTENDED NUMERIC CONSTANTS ===== |
| `ZERO_FLOAT` | float | `0.0f` | _No description_ |
| `ZERO_INT` | int | `0` | ===== NUMERIC VALIDATION CONSTANTS ===== |
| `ZERO_INT` | int | `0` | _No description_ |
| `ZERO_LONG` | long | `0L` | _No description_ |

### Constants_Original.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ATTACH_TIMEOUT_SECONDS` | float | `10f` | _No description_ |
| `BACKUP_INTERVAL_MINUTES` | int | `5` | _No description_ |
| `BYTES_TO_KB` | int | `1024` | ===== MEMORY AND CONVERSION CONSTANTS ===== |
| `BYTES_TO_MB` | double | `1048576.0` | _No description_ |
| `COMMAND_RECOGNIZED` | string | `"RECOGNIZED"` | ===== CONSOLE COMMAND CONSTANTS ===== |
| `COMMAND_UNKNOWN` | string | `"UNKNOWN"` | _No description_ |
| `CONSOLE_COMMAND_DELAY_MS` | int | `1000` | _No description_ |
| `CONSOLE_HOOK_GAMEOBJECT_NAME` | string | `"MixerConsoleHook"` | _No description_ |
| `CONSOLE_UTC_DATETIME_FORMAT` | string | `"yyyy-MM-dd HH:mm:ss.fff"` | _No description_ |
| `DEFAULT_OPERATION_DELAY` | float | `0f` | _No description_ |
| `DETECTION_DATETIME_FORMAT` | string | `"yyyy-MM-dd HH:mm:ss"` | _No description_ |
| `EMERGENCY_SAVE_FILENAME` | string | `"MixerThresholdSave_Emergency.json"` | _No description_ |
| `EMERGENCY_SAVE_REASON` | string | `"Emergency save before crash/shutdown"` | _No description_ |
| `FILE_COPY_OPERATION_NAME` | string | `"File Copy Operation"` | _No description_ |
| `GAME_SAVE_MAX_ITERATIONS_WARNING` | int | `20` | ===== STRESS TESTING CONSTANTS ===== |
| `GAME_SAVE_MIN_DELAY_SECONDS` | float | `3f` | _No description_ |
| `HARMONY_MOD_ID` | string | `"MixerThreholdMod.Main"` | ===== MOD IDENTIFICATION CONSTANTS ===== |
| `INVALID_ITERATION_COUNT_ERROR` | string | `"[CONSOLE] Invalid iteration count '{0}'. Must ...` | _No description_ |
| `INVALID_MSG_LEVEL_ERROR` | string | `"[ERROR] Invalid log level {0} for Msg method. ...` | ===== ERROR MESSAGE CONSTANTS ===== |
| `INVALID_OPERATION_DELAY_ERROR` | string | `"[ERROR] Invalid operation delay {0}. Must be g...` | _No description_ |
| `INVALID_PARAMETER_ERROR` | string | `"[CONSOLE] Invalid parameter '{0}'. Must be a d...` | _No description_ |
| `INVALID_WARN_LEVEL_ERROR` | string | `"[ERROR] Invalid log level {0} for Warn method....` | _No description_ |
| `JSON_FILE_EXTENSION` | string | `".json"` | ===== FILE OPERATION CONSTANTS ===== |
| `LOAD_TIMEOUT_SECONDS` | float | `30f` | _No description_ |
| `LOG_FILE_DATETIME_FORMAT` | string | `"yyyy-MM-dd HH:mm:ss"` | _No description_ |
| `LOG_LEVEL_CRITICAL` | int | `1` | ===== LOGGING LEVEL CONSTANTS ===== |
| `LOG_LEVEL_IMPORTANT` | int | `2` | _No description_ |
| `LOG_LEVEL_VERBOSE` | int | `3` | _No description_ |
| `LOG_PREFIX_BRIDGE` | string | `"[BRIDGE]"` | _No description_ |
| `LOG_PREFIX_CONSOLE` | string | `"[CONSOLE]"` | _No description_ |
| `LOG_PREFIX_CRITICAL` | string | `"[CRITICAL]"` | _No description_ |
| `LOG_PREFIX_DIR_RESOLVER` | string | `"[DIR-RESOLVER]"` | _No description_ |
| `LOG_PREFIX_ERROR` | string | `"[ERROR]"` | _No description_ |
| `LOG_PREFIX_GAME` | string | `"[GAME]"` | _No description_ |
| `LOG_PREFIX_GAME_ERROR` | string | `"[GAME ERROR]"` | _No description_ |
| `LOG_PREFIX_GAME_WARNING` | string | `"[GAME WARNING]"` | _No description_ |
| `LOG_PREFIX_INFO` | string | `"[Info]"` | ===== LOG PREFIX CONSTANTS ===== |
| `LOG_PREFIX_INIT` | string | `"[INIT]"` | _No description_ |
| `LOG_PREFIX_MANUAL` | string | `"[MANUAL]"` | _No description_ |
| `LOG_PREFIX_MONITOR` | string | `"[MONITOR]"` | _No description_ |
| `LOG_PREFIX_PROFILE` | string | `"[PROFILE]"` | _No description_ |
| `LOG_PREFIX_SAVE` | string | `"[SAVE]"` | _No description_ |
| `LOG_PREFIX_SYSMON` | string | `"[SYSMON]"` | _No description_ |
| `LOG_PREFIX_TRANSACTION` | string | `"[TRANSACTION]"` | _No description_ |
| `LOG_PREFIX_WARN` | string | `"[WARN]"` | _No description_ |
| `MAX_DELAY_WARNING_SECONDS` | float | `10f` | _No description_ |
| `MIXER_CONFIG_ENABLED_DEFAULT` | bool | `true` | _No description_ |
| `MIXER_PREF_MAX_ITERATIONS_WARNING` | int | `100` | _No description_ |
| `MIXER_PREF_PROGRESS_INTERVAL` | int | `10` | _No description_ |
| `MIXER_SAVE_FILENAME` | string | `"MixerThresholdSave.json"` | _No description_ |
| `MIXER_THRESHOLD_MAX` | float | `20f` | _No description_ |
| `MIXER_THRESHOLD_MIN` | float | `1f` | ===== MIXER CONFIGURATION CONSTANTS ===== |
| `MIXER_VALUE_TOLERANCE` | float | `0.001f` | _No description_ |
| `MOD_NAME` | string | `"MixerThreholdMod"` | _No description_ |
| `MOD_VERSION` | string | `"1.0.0"` | _No description_ |
| `MS_PER_SECOND` | int | `1000` | _No description_ |
| `NO_MIXER_DATA_ERROR` | string | `"[CONSOLE] No mixer data to save. Try adjusting...` | _No description_ |
| `NO_SAVE_PATH_ERROR` | string | `"[CONSOLE] No current save path available. Load...` | _No description_ |
| `NOT_AVAILABLE_PATH_FALLBACK` | string | `"[not available yet]"` | _No description_ |
| `NOT_SET_PATH_FALLBACK` | string | `"[not set]"` | _No description_ |
| `NULL_COMMAND_FALLBACK` | string | `"[null]"` | _No description_ |
| `NULL_ERROR_FALLBACK` | string | `"[null error message]"` | _No description_ |
| `NULL_MESSAGE_FALLBACK` | string | `"[null message]"` | ===== NULL MESSAGE FALLBACK CONSTANTS ===== |
| `OPERATION_TIMEOUT_MS` | int | `2000` | ===== TIMEOUT AND PERFORMANCE CONSTANTS ===== |
| `PARAM_TYPE_BOOLEAN` | string | `"BOOLEAN"` | _No description_ |
| `PARAM_TYPE_FLOAT` | string | `"FLOAT"` | _No description_ |
| `PARAM_TYPE_INTEGER` | string | `"INTEGER"` | _No description_ |
| `PARAM_TYPE_STRING` | string | `"STRING"` | _No description_ |
| `PERFORMANCE_FAST` | string | `"FAST"` | _No description_ |
| `PERFORMANCE_MODERATE` | string | `"MODERATE"` | _No description_ |
| `PERFORMANCE_SLOW` | string | `"SLOW"` | _No description_ |
| `PERFORMANCE_SLOW_THRESHOLD_MS` | int | `50` | _No description_ |
| `PERFORMANCE_WARNING_THRESHOLD_MS` | int | `100` | _No description_ |
| `POLL_INTERVAL_SECONDS` | float | `0.2f` | _No description_ |
| `SAVE_COOLDOWN_SECONDS` | int | `2` | _No description_ |
| `SAVE_DATA_VERSION` | string | `"1.0.0"` | _No description_ |
| `SAVE_DATETIME_FORMAT` | string | `"yyyy-MM-dd HH:mm:ss"` | ===== DATETIME FORMAT CONSTANTS ===== |
| `SAVE_PERFORMANCE_WARNING_SECONDS` | float | `1.0f` | _No description_ |
| `SECONDS_PER_MINUTE` | int | `60` | _No description_ |
| `STATUS_FAILED` | string | `"FAILED"` | _No description_ |
| `STATUS_SUCCESS` | string | `"SUCCESS"` | ===== SUCCESS/FAILURE MESSAGE CONSTANTS ===== |
| `STRESS_TEST_PROGRESS_INTERVAL` | int | `5` | _No description_ |
| `SYSTEM_MONITORING_LOG_INTERVAL` | int | `5` | _No description_ |
| `TEMP_FILE_EXTENSION` | string | `".tmp"` | _No description_ |
| `WARN_LEVEL_CRITICAL` | int | `1` | _No description_ |
| `WARN_LEVEL_VERBOSE` | int | `2` | _No description_ |


## Logging Constants

### LoggingConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ATOMIC_FILE_WRITER_PREFIX` | string | `"[ATOMIC-WRITER]"` | _No description_ |
| `BACKUP_CREATED_MESSAGE` | string | `"Backup created successfully"` | _No description_ |
| `BACKUP_LOG_FILENAME` | string | `"MixerThresholdMod_Backup.log"` | _No description_ |
| `BACKUP_MANAGER_PREFIX` | string | `"[BACKUP-MGR]"` | _No description_ |
| `BACKUP_RESTORED_MESSAGE` | string | `"Backup restored successfully"` | _No description_ |
| `CLEANUP_PREFIX` | string | `"[CLEANUP]"` | _No description_ |
| `CONSOLE_COMMAND_PREFIX` | string | `"[CONSOLE-CMD]"` | _No description_ |
| `DATA_INTEGRITY_PREFIX` | string | `"[DATA-INTEGRITY]"` | _No description_ |
| `DEBUG_LOG_FILENAME` | string | `"MixerThresholdMod_Debug.log"` | _No description_ |
| `DEBUG_PREFIX` | string | `"[DEBUG]"` | _No description_ |
| `DIRECTORY_RESOLVER_PREFIX` | string | `"[DIR-RESOLVER]"` | _No description_ |
| `EMERGENCY_SAVE_PREFIX` | string | `"[EMERGENCY-SAVE]"` | _No description_ |
| `ERROR_LOG_FILENAME` | string | `"MixerThresholdMod_Errors.log"` | _No description_ |
| `ERROR_PREFIX` | string | `"[ERROR]"` | _No description_ |
| `GENERAL_PREFIX` | string | `"[GENERAL]"` | _No description_ |
| `INFO_PREFIX` | string | `"[INFO]"` | _No description_ |
| `INIT_PREFIX` | string | `"[INIT]"` | _No description_ |
| `IO_RUNNER_PREFIX` | string | `"[IO-RUNNER]"` | _No description_ |
| `LOG_LEVEL_CRITICAL` | int | `1` | _No description_ |
| `LOG_LEVEL_CRITICAL_STRING` | string | `"CRITICAL"` | _No description_ |
| `LOG_LEVEL_DEBUG_STRING` | string | `"DEBUG"` | _No description_ |
| `LOG_LEVEL_ERROR_STRING` | string | `"ERROR"` | _No description_ |
| `LOG_LEVEL_IMPORTANT` | int | `2` | _No description_ |
| `LOG_LEVEL_INFO_STRING` | string | `"INFO"` | _No description_ |
| `LOG_LEVEL_TRACE_STRING` | string | `"TRACE"` | _No description_ |
| `LOG_LEVEL_VERBOSE` | int | `3` | _No description_ |
| `LOG_LEVEL_WARNING_STRING` | string | `"WARNING"` | _No description_ |
| `LOGGER_PREFIX` | string | `"[LOGGER]"` | _No description_ |
| `MAIN_LOG_FILENAME` | string | `"MixerThresholdMod.log"` | _No description_ |
| `MEMORY_PREFIX` | string | `"[MEMORY]"` | _No description_ |
| `MIXER_VALIDATION_PREFIX` | string | `"[MIXER-VALIDATION]"` | _No description_ |
| `OPERATION_CANCELLED_MESSAGE` | string | `"Operation was cancelled"` | _No description_ |
| `OPERATION_COMPLETE_MESSAGE` | string | `"Operation completed"` | _No description_ |
| `OPERATION_START_MESSAGE` | string | `"Operation started"` | _No description_ |
| `OPERATION_TIMEOUT_MESSAGE` | string | `"Operation timed out"` | _No description_ |
| `PERF_PREFIX` | string | `"[PERF]"` | _No description_ |
| `PERFORMANCE_LOG_FILENAME` | string | `"MixerThresholdMod_Performance.log"` | _No description_ |
| `PERFORMANCE_OPTIMIZER_PREFIX` | string | `"[PERF-OPT]"` | _No description_ |
| `PERSISTENCE_PREFIX` | string | `"[PERSISTENCE]"` | _No description_ |
| `SAVE_FAILURE_MESSAGE` | string | `"Save operation failed"` | _No description_ |
| `SAVE_MANAGER_PREFIX` | string | `"[SAVE-MGR]"` | _No description_ |
| `SAVE_PATCH_PREFIX` | string | `"[SAVE-PATCH]"` | _No description_ |
| `SAVE_SUCCESS_MESSAGE` | string | `"Save operation completed successfully"` | _No description_ |
| `SYSTEM_PREFIX` | string | `"[SYSTEM]"` | _No description_ |
| `TRACE_PREFIX` | string | `"[TRACE]"` | _No description_ |
| `VALIDATION_FAILURE_MESSAGE` | string | `"Validation failed"` | _No description_ |
| `VALIDATION_PREFIX` | string | `"[VALIDATION]"` | _No description_ |
| `VALIDATION_SUCCESS_MESSAGE` | string | `"Validation completed successfully"` | _No description_ |
| `WARN_LEVEL_CRITICAL` | int | `1` | _No description_ |
| `WARN_LEVEL_VERBOSE` | int | `2` | _No description_ |
| `WARNING_PREFIX` | string | `"[WARNING]"` | _No description_ |


## Mixer Constants

### MixerConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `DEFAULT_MIXER_CHANNEL` | int | `0` | _No description_ |
| `DEFAULT_MIXER_GAIN` | float | `0.0f` | _No description_ |
| `DEFAULT_MIXER_PRESET_NAME` | string | `"Default"` | _No description_ |
| `DEFAULT_MIXER_VOLUME` | float | `50.0f` | _No description_ |
| `FACTORY_MIXER_PRESET_NAME` | string | `"Factory"` | _No description_ |
| `GAIN_PRECISION_DECIMALS` | int | `1` | _No description_ |
| `MAX_MIXER_CHANNELS` | int | `16` | _No description_ |
| `MAX_MIXER_NAME_LENGTH` | int | `50` | _No description_ |
| `MAX_MIXER_PRESETS` | int | `20` | _No description_ |
| `MAX_MIXER_VOLUME` | float | `100.0f` | _No description_ |
| `MIN_MIXER_CHANNELS` | int | `1` | _No description_ |
| `MIN_MIXER_NAME_LENGTH` | int | `3` | _No description_ |
| `MIN_MIXER_VOLUME` | float | `0.0f` | _No description_ |
| `MIXER_BACKUP_FILENAME` | string | `"MixerThresholdSave_Backup.json"` | _No description_ |
| `MIXER_CHANNEL_COUNT` | int | `8` | _No description_ |
| `MIXER_CHANNEL_KEY_PREFIX` | string | `"Channel_"` | _No description_ |
| `MIXER_CHANNEL_MUTED_MESSAGE` | string | `"Mixer channel muted"` | _No description_ |
| `MIXER_CHANNEL_UNMUTED_MESSAGE` | string | `"Mixer channel unmuted"` | _No description_ |
| `MIXER_CONFIG_FILENAME` | string | `"MixerConfig.json"` | _No description_ |
| `MIXER_CONFIG_KEY` | string | `"MixerConfig"` | _No description_ |
| `MIXER_CONFIG_UPDATE_MESSAGE` | string | `"Mixer configuration updated"` | _No description_ |
| `MIXER_GAIN_KEY` | string | `"Gain"` | _No description_ |
| `MIXER_GAIN_RANGE` | float | `20.0f` | _No description_ |
| `MIXER_INIT_MESSAGE` | string | `"Mixer initialized successfully"` | _No description_ |
| `MIXER_MUTE_KEY` | string | `"Mute"` | _No description_ |
| `MIXER_PRESET_KEY` | string | `"MixerPreset"` | _No description_ |
| `MIXER_PRESET_LOADED_MESSAGE` | string | `"Mixer preset loaded"` | _No description_ |
| `MIXER_PRESET_SAVED_MESSAGE` | string | `"Mixer preset saved"` | _No description_ |
| `MIXER_PRESET_VERSION` | string | `"1.0"` | _No description_ |
| `MIXER_PRESETS_FILENAME` | string | `"MixerPresets.json"` | _No description_ |
| `MIXER_RESET_MESSAGE` | string | `"Mixer reset to defaults"` | _No description_ |
| `MIXER_SAVE_FILENAME` | string | `"MixerThresholdSave.json"` | _No description_ |
| `MIXER_SETTINGS_FILENAME` | string | `"MixerSettings.json"` | _No description_ |
| `MIXER_SOLO_KEY` | string | `"Solo"` | _No description_ |
| `MIXER_VALIDATION_FAILURE_MESSAGE` | string | `"Mixer validation failed"` | _No description_ |
| `MIXER_VALIDATION_SUCCESS_MESSAGE` | string | `"Mixer validation successful"` | _No description_ |
| `MIXER_VALUE_CHANGED_MESSAGE` | string | `"Mixer value changed"` | _No description_ |
| `MIXER_VALUES_KEY` | string | `"MixerValues"` | _No description_ |
| `MIXER_VOLUME_KEY` | string | `"Volume"` | _No description_ |
| `MIXER_VOLUME_STEP` | float | `5.0f` | _No description_ |
| `USER_MIXER_PRESET_PREFIX` | string | `"User_"` | _No description_ |
| `VOLUME_PRECISION_DECIMALS` | int | `2` | _No description_ |


## Network Constants

### NetworkConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `API_KEY_HEADER` | string | `"X-API-Key"` | _No description_ |
| `BASIC_AUTH_PREFIX` | string | `"Basic "` | _No description_ |
| `BEARER_TOKEN_PREFIX` | string | `"Bearer "` | _No description_ |
| `CONTENT_TYPE_BINARY` | string | `"application/octet-stream"` | _No description_ |
| `CONTENT_TYPE_FORM_URLENCODED` | string | `"application/x-www-form-urlencoded"` | _No description_ |
| `CONTENT_TYPE_HTML` | string | `"text/html"` | _No description_ |
| `CONTENT_TYPE_IMAGE_JPEG` | string | `"image/jpeg"` | _No description_ |
| `CONTENT_TYPE_IMAGE_PNG` | string | `"image/png"` | _No description_ |
| `CONTENT_TYPE_JSON` | string | `"application/json"` | _No description_ |
| `CONTENT_TYPE_MULTIPART_FORM` | string | `"multipart/form-data"` | _No description_ |
| `CONTENT_TYPE_PDF` | string | `"application/pdf"` | _No description_ |
| `CONTENT_TYPE_TEXT` | string | `"text/plain"` | _No description_ |
| `CONTENT_TYPE_XML` | string | `"application/xml"` | _No description_ |
| `CSRF_TOKEN_HEADER` | string | `"X-CSRF-Token"` | _No description_ |
| `ENCODING_ASCII` | string | `"ASCII"` | _No description_ |
| `ENCODING_BASE64` | string | `"Base64"` | _No description_ |
| `ENCODING_HTML` | string | `"HTML"` | _No description_ |
| `ENCODING_JSON` | string | `"JSON"` | _No description_ |
| `ENCODING_URL` | string | `"URL"` | _No description_ |
| `ENCODING_UTF16` | string | `"UTF-16"` | _No description_ |
| `ENCODING_UTF8` | string | `"UTF-8"` | _No description_ |
| `ENCODING_XML` | string | `"XML"` | _No description_ |
| `HTTP_DEFAULT_TIMEOUT_MS` | int | `30000` | _No description_ |
| `HTTP_HEADER_ACCEPT` | string | `"Accept"` | _No description_ |
| `HTTP_HEADER_ACCEPT_ENCODING` | string | `"Accept-Encoding"` | _No description_ |
| `HTTP_HEADER_AUTHORIZATION` | string | `"Authorization"` | _No description_ |
| `HTTP_HEADER_CACHE_CONTROL` | string | `"Cache-Control"` | _No description_ |
| `HTTP_HEADER_CONNECTION` | string | `"Connection"` | _No description_ |
| `HTTP_HEADER_CONTENT_LENGTH` | string | `"Content-Length"` | _No description_ |
| `HTTP_HEADER_CONTENT_TYPE` | string | `"Content-Type"` | _No description_ |
| `HTTP_HEADER_COOKIE` | string | `"Cookie"` | _No description_ |
| `HTTP_HEADER_SET_COOKIE` | string | `"Set-Cookie"` | _No description_ |
| `HTTP_HEADER_USER_AGENT` | string | `"User-Agent"` | _No description_ |
| `HTTP_METHOD_DELETE` | string | `"DELETE"` | _No description_ |
| `HTTP_METHOD_GET` | string | `"GET"` | _No description_ |
| `HTTP_METHOD_HEAD` | string | `"HEAD"` | _No description_ |
| `HTTP_METHOD_OPTIONS` | string | `"OPTIONS"` | _No description_ |
| `HTTP_METHOD_PATCH` | string | `"PATCH"` | _No description_ |
| `HTTP_METHOD_POST` | string | `"POST"` | _No description_ |
| `HTTP_METHOD_PUT` | string | `"PUT"` | _No description_ |
| `HTTP_RETRY_ATTEMPTS` | int | `3` | _No description_ |
| `HTTP_RETRY_DELAY_MS` | int | `1000` | _No description_ |
| `HTTP_STATUS_BAD_GATEWAY` | int | `502` | _No description_ |
| `HTTP_STATUS_BAD_REQUEST` | int | `400` | _No description_ |
| `HTTP_STATUS_CREATED` | int | `201` | _No description_ |
| `HTTP_STATUS_FORBIDDEN` | int | `403` | _No description_ |
| `HTTP_STATUS_GATEWAY_TIMEOUT` | int | `504` | _No description_ |
| `HTTP_STATUS_INTERNAL_SERVER_ERROR` | int | `500` | _No description_ |
| `HTTP_STATUS_NO_CONTENT` | int | `204` | _No description_ |
| `HTTP_STATUS_NOT_FOUND` | int | `404` | _No description_ |
| `HTTP_STATUS_OK` | int | `200` | _No description_ |
| `HTTP_STATUS_SERVICE_UNAVAILABLE` | int | `503` | _No description_ |
| `HTTP_STATUS_UNAUTHORIZED` | int | `401` | _No description_ |
| `JWT_TOKEN_TYPE` | string | `"JWT"` | _No description_ |
| `MAX_TCP_CONNECTIONS` | int | `100` | _No description_ |
| `NETWORK_AUTH_FAILED` | string | `"Authentication failed"` | _No description_ |
| `NETWORK_CLIENT_ERROR` | string | `"Client error occurred"` | _No description_ |
| `NETWORK_CONNECTION_FAILED` | string | `"Network connection failed"` | _No description_ |
| `NETWORK_DNS_FAILED` | string | `"DNS resolution failed"` | _No description_ |
| `NETWORK_INVALID_RESPONSE` | string | `"Invalid network response"` | _No description_ |
| `NETWORK_PROTOCOL_ERROR` | string | `"Protocol error occurred"` | _No description_ |
| `NETWORK_RATE_LIMITED` | string | `"Rate limit exceeded"` | _No description_ |
| `NETWORK_REQUEST_TIMEOUT` | string | `"Network request timed out"` | _No description_ |
| `NETWORK_SERVER_ERROR` | string | `"Server error occurred"` | _No description_ |
| `NETWORK_SSL_ERROR` | string | `"SSL/TLS error occurred"` | _No description_ |
| `OAUTH_TOKEN_TYPE` | string | `"OAuth"` | _No description_ |
| `PROTOCOL_FTP` | string | `"FTP"` | _No description_ |
| `PROTOCOL_HTTP` | string | `"HTTP"` | _No description_ |
| `PROTOCOL_HTTPS` | string | `"HTTPS"` | _No description_ |
| `PROTOCOL_IMAP` | string | `"IMAP"` | _No description_ |
| `PROTOCOL_POP3` | string | `"POP3"` | _No description_ |
| `PROTOCOL_SMTP` | string | `"SMTP"` | _No description_ |
| `PROTOCOL_SSH` | string | `"SSH"` | _No description_ |
| `PROTOCOL_TCP` | string | `"TCP"` | _No description_ |
| `PROTOCOL_UDP` | string | `"UDP"` | _No description_ |
| `PROTOCOL_WEBSOCKET` | string | `"WebSocket"` | _No description_ |
| `SESSION_COOKIE_NAME` | string | `"SessionId"` | _No description_ |
| `SOCKET_KEEP_ALIVE_ENABLED` | bool | `true` | _No description_ |
| `SOCKET_NO_DELAY_ENABLED` | bool | `true` | _No description_ |
| `SSL_CERT_VALIDATION_ENABLED` | bool | `true` | _No description_ |
| `TCP_BUFFER_SIZE` | int | `8192` | _No description_ |
| `TCP_CONNECTION_TIMEOUT_MS` | int | `5000` | _No description_ |
| `TCP_DEFAULT_PORT` | int | `8080` | _No description_ |
| `TCP_READ_TIMEOUT_MS` | int | `10000` | _No description_ |
| `TCP_WRITE_TIMEOUT_MS` | int | `10000` | _No description_ |
| `TLS_VERSION_12` | string | `"TLS 1.2"` | _No description_ |
| `TLS_VERSION_13` | string | `"TLS 1.3"` | _No description_ |
| `UDP_BUFFER_SIZE` | int | `4096` | _No description_ |
| `UDP_DEFAULT_PORT` | int | `8081` | _No description_ |
| `UDP_SOCKET_TIMEOUT_MS` | int | `5000` | _No description_ |


## Performance Constants

### PerformanceConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ASYNC_QUEUE_LIMIT` | int | `50` | _No description_ |
| `BACKGROUND_TASK_INTERVAL_MS` | int | `5000` | _No description_ |
| `BACKUP_OPERATION_TIMEOUT_MS` | int | `30000` | _No description_ |
| `BATCH_OPERATION_SIZE` | int | `10` | _No description_ |
| `BUFFER_SIZE_LARGE` | int | `65536` | _No description_ |
| `BUFFER_SIZE_SMALL` | int | `1024` | _No description_ |
| `BUFFER_SIZE_STANDARD` | int | `4096` | _No description_ |
| `CACHE_EXPIRY_MS` | int | `300000` | _No description_ |
| `CONSOLE_COMMAND_DELAY_MS` | int | `1000` | _No description_ |
| `CPU_CRITICAL_THRESHOLD_PERCENT` | int | `90` | _No description_ |
| `CPU_MONITORING_INTERVAL_MS` | int | `15000` | _No description_ |
| `CPU_WARNING_THRESHOLD_PERCENT` | int | `75` | _No description_ |
| `DATABASE_TIMEOUT_MS` | int | `15000` | _No description_ |
| `EMERGENCY_SAVE_TIMEOUT_MS` | int | `2000` | _No description_ |
| `FILE_OPERATION_TIMEOUT_MS` | int | `5000` | _No description_ |
| `GC_COLLECTION_THRESHOLD_BYTES` | long | `52428800` | _No description_ |
| `IO_MONITORING_INTERVAL_MS` | int | `10000` | _No description_ |
| `LONG_WAIT_MS` | int | `1000` | _No description_ |
| `MAX_CACHE_SIZE` | int | `100` | _No description_ |
| `MEDIUM_WAIT_MS` | int | `250` | _No description_ |
| `MEMORY_CRITICAL_THRESHOLD_BYTES` | long | `524288000` | _No description_ |
| `MEMORY_MONITORING_INTERVAL_MS` | int | `30000` | _No description_ |
| `MEMORY_WARNING_THRESHOLD_BYTES` | long | `104857600` | _No description_ |
| `NETWORK_TIMEOUT_MS` | int | `30000` | _No description_ |
| `OPERATION_TIMEOUT_MS` | int | `2000` | _No description_ |
| `PERFORMANCE_CRITICAL_THRESHOLD_MS` | int | `500` | _No description_ |
| `PERFORMANCE_MONITORING_INTERVAL_MS` | int | `60000` | _No description_ |
| `PERFORMANCE_SLOW_THRESHOLD_MS` | int | `50` | _No description_ |
| `PERFORMANCE_WARNING_THRESHOLD_MS` | int | `100` | _No description_ |
| `POLLING_INTERVAL_MS` | int | `100` | _No description_ |
| `RETRY_DELAY_MS` | int | `500` | _No description_ |
| `SAVE_OPERATION_TIMEOUT_MS` | int | `10000` | _No description_ |
| `SHORT_WAIT_MS` | int | `100` | _No description_ |
| `SYSTEM_MONITORING_LOG_INTERVAL` | int | `5` | _No description_ |
| `THREAD_POOL_MAX_THREADS` | int | `16` | _No description_ |
| `THREAD_POOL_MIN_THREADS` | int | `2` | _No description_ |
| `VERY_LONG_WAIT_MS` | int | `5000` | _No description_ |


## System Constants

### SystemConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `API_VERSION` | string | `"1.0.0"` | _No description_ |
| `APPDATA_ENV_VAR` | string | `"APPDATA"` | _No description_ |
| `BACKEND_IL2CPP` | string | `"IL2CPP"` | _No description_ |
| `BACKEND_MONO` | string | `"Mono"` | _No description_ |
| `BUILD_CONFIG_DEBUG` | string | `"Debug"` | _No description_ |
| `BUILD_CONFIG_RELEASE` | string | `"Release"` | _No description_ |
| `COMPONENT_ATOMIC_FILE_WRITER` | string | `"AtomicFileWriter"` | _No description_ |
| `COMPONENT_BACKUP_MANAGER` | string | `"MixerDataBackupManager"` | _No description_ |
| `COMPONENT_CONSOLE_COMMAND` | string | `"ConsoleCommandHandler"` | _No description_ |
| `COMPONENT_DATA_INTEGRITY` | string | `"DataIntegrityTracker"` | _No description_ |
| `COMPONENT_EMERGENCY_SAVE` | string | `"EmergencySaveManager"` | _No description_ |
| `COMPONENT_IO_RUNNER` | string | `"CancellableIoRunner"` | _No description_ |
| `COMPONENT_LOGGER` | string | `"Logger"` | _No description_ |
| `COMPONENT_MIXER_VALIDATION` | string | `"MixerValidation"` | _No description_ |
| `COMPONENT_PERFORMANCE_OPTIMIZER` | string | `"PerformanceOptimizer"` | _No description_ |
| `COMPONENT_SAVE_MANAGER` | string | `"SaveManager"` | _No description_ |
| `CONFIG_AUTO_BACKUP` | string | `"AutoBackup"` | _No description_ |
| `CONFIG_DEBUG_MODE` | string | `"DebugMode"` | _No description_ |
| `CONFIG_EMERGENCY_SAVE` | string | `"EmergencySave"` | _No description_ |
| `CONFIG_MEMORY_OPTIMIZATION` | string | `"MemoryOptimization"` | _No description_ |
| `CONFIG_PERFORMANCE_MONITORING` | string | `"PerformanceMonitoring"` | _No description_ |
| `CONFIG_THREAD_SAFETY_MODE` | string | `"ThreadSafetyMode"` | _No description_ |
| `CONFIG_VALIDATION_ENABLED` | string | `"ValidationEnabled"` | _No description_ |
| `CONFIG_VERBOSE_LOGGING` | string | `"VerboseLogging"` | _No description_ |
| `CONFIG_VERSION` | string | `"1.0"` | _No description_ |
| `DOTNET_VERSION` | string | `"4.8.1"` | _No description_ |
| `ENVIRONMENT_DEVELOPMENT` | string | `"Development"` | _No description_ |
| `ENVIRONMENT_PRODUCTION` | string | `"Production"` | _No description_ |
| `ENVIRONMENT_TESTING` | string | `"Testing"` | _No description_ |
| `GAME_DATA_PATH_ENV_VAR` | string | `"GAME_DATA_PATH"` | _No description_ |
| `GAME_REGISTRY_KEY_BASE` | string | `"HKEY_CURRENT_USER\\SOFTWARE"` | _No description_ |
| `LOCALAPPDATA_ENV_VAR` | string | `"LOCALAPPDATA"` | _No description_ |
| `MELONLOADER_ASSEMBLY` | string | `"MelonLoader"` | _No description_ |
| `MIN_MELONLOADER_VERSION` | string | `"0.5.7"` | _No description_ |
| `MIN_UNITY_VERSION` | string | `"2019.4.0"` | _No description_ |
| `MOD_ASSEMBLY_NAME` | string | `"MixerThreholdMod-1_0_0"` | _No description_ |
| `MOD_AUTHOR` | string | `"MixerThresholdMod Team"` | _No description_ |
| `MOD_DESCRIPTION` | string | `"Advanced mixer threshold management mod"` | _No description_ |
| `MOD_GUID` | string | `"com.mixerthresholdmod.1_0_0"` | _No description_ |
| `MOD_NAME` | string | `"MixerThreholdMod"` | _No description_ |
| `MOD_NAMESPACE` | string | `"MixerThreholdMod_1_0_0"` | _No description_ |
| `MOD_REGISTRY_KEY` | string | `"HKEY_CURRENT_USER\\SOFTWARE\\MixerThresholdMod"` | _No description_ |
| `MOD_VERSION` | string | `"1.0.0"` | _No description_ |
| `NEWTONSOFT_JSON_ASSEMBLY` | string | `"Newtonsoft.Json"` | _No description_ |
| `PLATFORM_LINUX` | string | `"Linux"` | _No description_ |
| `PLATFORM_MACOS` | string | `"macOS"` | _No description_ |
| `PLATFORM_UNITY` | string | `"Unity"` | _No description_ |
| `PLATFORM_WINDOWS` | string | `"Windows"` | _No description_ |
| `PROGRAM_FILES_ENV_VAR` | string | `"PROGRAMFILES"` | _No description_ |
| `PROGRAM_FILES_X86_ENV_VAR` | string | `"PROGRAMFILES(X86)"` | _No description_ |
| `SAVE_FORMAT_VERSION` | string | `"1.0"` | _No description_ |
| `SYSTEM_ASSEMBLY` | string | `"System"` | _No description_ |
| `SYSTEM_CORE_ASSEMBLY` | string | `"System.Core"` | _No description_ |
| `UNITY_CORE_ASSEMBLY` | string | `"UnityEngine.CoreModule"` | _No description_ |
| `UNITY_ENGINE_ASSEMBLY` | string | `"UnityEngine"` | _No description_ |
| `UNITY_PATH_ENV_VAR` | string | `"UNITY_PATH"` | _No description_ |
| `UNITY_REGISTRY_KEY` | string | `"HKEY_LOCAL_MACHINE\\SOFTWARE\\Unity Technologies"` | _No description_ |
| `USER_PROFILE_ENV_VAR` | string | `"USERPROFILE"` | _No description_ |


## Threading Constants

### ThreadingConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ASYNC_RETRY_COUNT` | int | `3` | _No description_ |
| `ASYNC_RETRY_DELAY_MS` | int | `1000` | _No description_ |
| `BACKGROUND_TASK_INTERVAL_MS` | int | `5000` | _No description_ |
| `BACKGROUND_THREAD_PREFIX` | string | `"Background_"` | _No description_ |
| `BACKUP_MUTEX_NAME` | string | `"MixerThresholdMod_BackupMutex"` | _No description_ |
| `BACKUP_THREAD_NAME` | string | `"BackupThread"` | _No description_ |
| `CANCELLATION_CHECK_INTERVAL_MS` | int | `100` | _No description_ |
| `CRITICAL_SECTION_TIMEOUT_MS` | int | `1000` | _No description_ |
| `DEADLOCK_DETECTED_MESSAGE` | string | `"Potential deadlock detected"` | _No description_ |
| `DEFAULT_CANCELLATION_TIMEOUT_MS` | int | `30000` | _No description_ |
| `EVENT_WAIT_TIMEOUT_MS` | int | `30000` | _No description_ |
| `EXTENDED_CANCELLATION_TIMEOUT_MS` | int | `120000` | _No description_ |
| `FILE_ACCESS_SEMAPHORE_NAME` | string | `"MixerThresholdMod_FileAccess"` | _No description_ |
| `GLOBAL_OPERATION_EVENT_NAME` | string | `"MixerThresholdMod_GlobalOperation"` | _No description_ |
| `IO_THREAD_PREFIX` | string | `"IO_"` | _No description_ |
| `LOCK_TIMEOUT_MS` | int | `2000` | _No description_ |
| `MAIN_THREAD_BLOCKED_WARNING` | string | `"WARNING: Operation may block main thread"` | _No description_ |
| `MAIN_THREAD_NAME` | string | `"MainThread"` | _No description_ |
| `MAX_ASYNC_QUEUE_SIZE` | int | `50` | _No description_ |
| `MAX_COMPLETION_PORT_THREADS` | int | `16` | _No description_ |
| `MAX_CONCURRENT_OPERATIONS` | int | `10` | _No description_ |
| `MAX_WORKER_THREADS` | int | `16` | _No description_ |
| `MIN_COMPLETION_PORT_THREADS` | int | `2` | _No description_ |
| `MIN_WORKER_THREADS` | int | `2` | _No description_ |
| `MONITORING_THREAD_NAME` | string | `"MonitoringThread"` | _No description_ |
| `MUTEX_TIMEOUT_MS` | int | `5000` | _No description_ |
| `PERFORMANCE_SEMAPHORE_NAME` | string | `"MixerThresholdMod_Performance"` | _No description_ |
| `PERFORMANCE_THREAD_NAME` | string | `"PerformanceThread"` | _No description_ |
| `QUICK_CANCELLATION_TIMEOUT_MS` | int | `5000` | _No description_ |
| `RESOURCE_CONTENTION_MESSAGE` | string | `"Resource contention detected"` | _No description_ |
| `RW_LOCK_TIMEOUT_MS` | int | `5000` | _No description_ |
| `SAVE_MUTEX_NAME` | string | `"MixerThresholdMod_SaveMutex"` | _No description_ |
| `SAVE_THREAD_NAME` | string | `"SaveThread"` | _No description_ |
| `SEMAPHORE_TIMEOUT_MS` | int | `10000` | _No description_ |
| `THREAD_ABORTED_MESSAGE` | string | `"Thread was aborted"` | _No description_ |
| `THREAD_EXCEPTION_MESSAGE` | string | `"Thread encountered an exception"` | _No description_ |
| `THREAD_IDLE_TIMEOUT_MS` | int | `30000` | _No description_ |
| `THREAD_POOL_QUEUE_LIMIT` | int | `100` | _No description_ |
| `THREAD_PRIORITY_BACKGROUND` | string | `"Lowest"` | _No description_ |
| `THREAD_PRIORITY_CRITICAL` | string | `"Highest"` | _No description_ |
| `THREAD_PRIORITY_HIGH` | string | `"AboveNormal"` | _No description_ |
| `THREAD_PRIORITY_LOW` | string | `"BelowNormal"` | _No description_ |
| `THREAD_PRIORITY_NORMAL` | string | `"Normal"` | _No description_ |
| `THREAD_STARTED_MESSAGE` | string | `"Thread started successfully"` | _No description_ |
| `THREAD_STOPPED_MESSAGE` | string | `"Thread stopped successfully"` | _No description_ |
| `THREAD_TIMEOUT_MESSAGE` | string | `"Thread operation timed out"` | _No description_ |
| `VALIDATION_LOCK_NAME` | string | `"MixerThresholdMod_Validation"` | _No description_ |
| `VALIDATION_THREAD_NAME` | string | `"ValidationThread"` | _No description_ |


## Utility Constants

### UtilityConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ACTIVE_STRING` | string | `"Active"` | _No description_ |
| `AMPERSAND_CHAR` | char | `'&'` | _No description_ |
| `APOSTROPHE_CHAR` | char | `'\''` | _No description_ |
| `ARRAY_GROWTH_FACTOR` | double | `1.5` | _No description_ |
| `ASTERISK_CHAR` | char | `'*'` | _No description_ |
| `AT_SYMBOL_CHAR` | char | `'@'` | _No description_ |
| `BACKSLASH_CHAR` | char | `'\\'` | _No description_ |
| `BACKSPACE_CHAR` | char | `'\b'` | _No description_ |
| `BELL_CHAR` | char | `'\a'` | _No description_ |
| `BINARY_FORMAT_PREFIX` | string | `"0b"` | _No description_ |
| `BYTES_PER_GB` | long | `1024 * 1024 * 1024` | _No description_ |
| `BYTES_PER_KB` | long | `1024` | _No description_ |
| `BYTES_PER_MB` | long | `1024 * 1024` | _No description_ |
| `BYTES_PER_TB` | long | `1024L * 1024L * 1024L * 1024L` | _No description_ |
| `CARET_CHAR` | char | `'^'` | _No description_ |
| `CARRIAGE_RETURN` | string | `"\r"` | _No description_ |
| `CELSIUS_TO_FAHRENHEIT_MULTIPLIER` | double | `1.8` | _No description_ |
| `CELSIUS_TO_FAHRENHEIT_OFFSET` | double | `32.0` | _No description_ |
| `CM_PER_METER` | double | `100.0` | _No description_ |
| `COLLECTION_RESIZE_THRESHOLD` | double | `0.75` | _No description_ |
| `COLON_SEPARATOR` | string | `":"` | _No description_ |
| `COMMA_SEPARATOR` | string | `","` | _No description_ |
| `CULTURE_DE_DE` | string | `"de-DE"` | _No description_ |
| `CULTURE_EN_GB` | string | `"en-GB"` | _No description_ |
| `CULTURE_EN_US` | string | `"en-US"` | _No description_ |
| `CULTURE_ES_ES` | string | `"es-ES"` | _No description_ |
| `CULTURE_FR_FR` | string | `"fr-FR"` | _No description_ |
| `CULTURE_INVARIANT` | string | `""` | _No description_ |
| `CULTURE_IT_IT` | string | `"it-IT"` | _No description_ |
| `CULTURE_JA_JP` | string | `"ja-JP"` | _No description_ |
| `CULTURE_KO_KR` | string | `"ko-KR"` | _No description_ |
| `CULTURE_PT_BR` | string | `"pt-BR"` | _No description_ |
| `CULTURE_PT_PT` | string | `"pt-PT"` | _No description_ |
| `CULTURE_RU_RU` | string | `"ru-RU"` | _No description_ |
| `CULTURE_ZH_CN` | string | `"zh-CN"` | _No description_ |
| `CULTURE_ZH_TW` | string | `"zh-TW"` | _No description_ |
| `CURRENCY_FORMAT` | string | `"C"` | _No description_ |
| `DASH_SEPARATOR` | string | `"-"` | _No description_ |
| `DATE_FORMAT_ISO8601` | string | `"yyyy-MM-ddTHH:mm:ss.fffZ"` | _No description_ |
| `DATE_FORMAT_LONG` | string | `"yyyy-MM-dd HH:mm:ss"` | _No description_ |
| `DATE_FORMAT_SHORT` | string | `"yyyy-MM-dd"` | _No description_ |
| `DAYS_PER_WEEK` | int | `7` | _No description_ |
| `DEFAULT_COLLECTION_CAPACITY` | int | `10` | _No description_ |
| `DEFAULT_DICTIONARY_CAPACITY` | int | `16` | _No description_ |
| `DEFAULT_HASHSET_CAPACITY` | int | `16` | _No description_ |
| `DEGREES_TO_RADIANS` | double | `PI / 180.0` | _No description_ |
| `DELETE_CHAR` | char | `'\x7F'` | _No description_ |
| `DISABLED_STRING` | string | `"Disabled"` | _No description_ |
| `DOLLAR_SYMBOL_CHAR` | char | `'$'` | _No description_ |
| `DOT_SEPARATOR` | string | `"."` | _No description_ |
| `E` | double | `2.71828182845904523536` | _No description_ |
| `EMPTY_ARRAY_SIZE` | int | `0` | _No description_ |
| `EMPTY_STRING` | string | `""` | _No description_ |
| `ENABLED_STRING` | string | `"Enabled"` | _No description_ |
| `EQUALS_CHAR` | char | `'='` | _No description_ |
| `ESCAPE_CHAR` | char | `'\x1B'` | _No description_ |
| `EXCLAMATION_MARK_CHAR` | char | `'!'` | _No description_ |
| `EXPONENTIAL_FORMAT` | string | `"E"` | _No description_ |
| `FALSE_STRING` | string | `"False"` | _No description_ |
| `FEET_PER_YARD` | double | `3.0` | _No description_ |
| `FILENAME_TIMESTAMP_FORMAT` | string | `"yyyyMMdd_HHmmss"` | _No description_ |
| `FORM_FEED_CHAR` | char | `'\f'` | _No description_ |
| `FORWARD_SLASH_CHAR` | char | `'/'` | _No description_ |
| `GOLDEN_RATIO` | double | `1.61803398874989484820` | _No description_ |
| `GRAVE_ACCENT_CHAR` | char | `'`'` | _No description_ |
| `GRAVITY_ACCELERATION` | double | `9.80665` | _No description_ |
| `GREATER_THAN_CHAR` | char | `'&gt;'` | _No description_ |
| `HASH_SYMBOL_CHAR` | char | `'#'` | _No description_ |
| `HEX_FORMAT_LOWER` | string | `"x"` | _No description_ |
| `HEX_FORMAT_PREFIX` | string | `"0x"` | _No description_ |
| `HEX_FORMAT_UPPER` | string | `"X"` | _No description_ |
| `HOURS_PER_DAY` | int | `24` | _No description_ |
| `INACTIVE_STRING` | string | `"Inactive"` | _No description_ |
| `INCHES_PER_FOOT` | double | `12.0` | _No description_ |
| `INTEGER_FORMAT_LEADING_ZEROS` | string | `"D4"` | _No description_ |
| `LARGE_COLLECTION_CAPACITY` | int | `1000` | _No description_ |
| `LEFT_BRACE_CHAR` | char | `'{'` | _No description_ |
| `LEFT_BRACKET_CHAR` | char | `'['` | _No description_ |
| `LEFT_PAREN_CHAR` | char | `'('` | _No description_ |
| `LESS_THAN_CHAR` | char | `'&lt;'` | _No description_ |
| `LN_10` | double | `2.30258509299404568402` | _No description_ |
| `LN_2` | double | `0.69314718055994530942` | _No description_ |
| `LOG_TIMESTAMP_FORMAT` | string | `"yyyy-MM-dd HH:mm:ss.fff"` | _No description_ |
| `MAX_RECOMMENDED_COLLECTION_SIZE` | int | `10000` | _No description_ |
| `METERS_PER_KM` | double | `1000.0` | _No description_ |
| `MINUS_CHAR` | char | `'-'` | _No description_ |
| `MINUTES_PER_HOUR` | int | `60` | _No description_ |
| `MONTHS_PER_YEAR` | int | `12` | _No description_ |
| `MS_PER_SECOND` | int | `1000` | _No description_ |
| `NEWLINE` | string | `"\n"` | _No description_ |
| `NO_STRING` | string | `"No"` | _No description_ |
| `NULL_CHAR` | char | `'\0'` | _No description_ |
| `NUMBER_FORMAT_2_DECIMAL` | string | `"F2"` | _No description_ |
| `NUMBER_FORMAT_4_DECIMAL` | string | `"F4"` | _No description_ |
| `OFF_STRING` | string | `"Off"` | _No description_ |
| `ON_STRING` | string | `"On"` | _No description_ |
| `PERCENT_SYMBOL_CHAR` | char | `'%'` | _No description_ |
| `PERCENTAGE_FORMAT` | string | `"P"` | _No description_ |
| `PI` | double | `3.14159265358979323846` | _No description_ |
| `PIPE_SEPARATOR` | string | `"\|"` | _No description_ |
| `PLANCK_CONSTANT` | double | `6.62607015e-34` | _No description_ |
| `PLUS_CHAR` | char | `'+'` | _No description_ |
| `QUESTION_MARK_CHAR` | char | `'?'` | _No description_ |
| `QUOTE_CHAR` | char | `'"'` | _No description_ |
| `RADIANS_TO_DEGREES` | double | `180.0 / PI` | _No description_ |
| `RIGHT_BRACE_CHAR` | char | `'}'` | _No description_ |
| `RIGHT_BRACKET_CHAR` | char | `']'` | _No description_ |
| `RIGHT_PAREN_CHAR` | char | `')'` | _No description_ |
| `SECONDS_PER_MINUTE` | int | `60` | _No description_ |
| `SEMICOLON_SEPARATOR` | string | `";"` | _No description_ |
| `SPACE` | string | `" "` | _No description_ |
| `SPEED_OF_LIGHT` | double | `299792458.0` | _No description_ |
| `SQRT_2` | double | `1.41421356237309504880` | _No description_ |
| `SQRT_3` | double | `1.73205080756887729352` | _No description_ |
| `TAB` | string | `"\t"` | _No description_ |
| `TILDE_CHAR` | char | `'~'` | _No description_ |
| `TIME_FORMAT_12HOUR` | string | `"hh:mm:ss tt"` | _No description_ |
| `TIME_FORMAT_24HOUR` | string | `"HH:mm:ss"` | _No description_ |
| `TIME_FORMAT_ONLY` | string | `"HH:mm:ss"` | _No description_ |
| `TIMESTAMP_FORMAT_MS` | string | `"yyyy-MM-dd HH:mm:ss.fff"` | _No description_ |
| `TRUE_STRING` | string | `"True"` | _No description_ |
| `UNDERSCORE_SEPARATOR` | string | `"_"` | _No description_ |
| `UNIX_LINE_ENDING` | string | `"\n"` | _No description_ |
| `UTC_TIMESTAMP_FORMAT` | string | `"yyyy-MM-ddTHH:mm:ss.fffZ"` | _No description_ |
| `VERTICAL_TAB_CHAR` | char | `'\v'` | _No description_ |
| `WINDOWS_LINE_ENDING` | string | `"\r\n"` | _No description_ |
| `YES_STRING` | string | `"Yes"` | _No description_ |


## Validation Constants

### ValidationConstants.cs

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ALPHABETIC_REGEX_PATTERN` | string | `@"^[a-zA-Z]+$"` | _No description_ |
| `ALPHANUMERIC_REGEX_PATTERN` | string | `@"^[a-zA-Z0-9]+$"` | _No description_ |
| `BACKUP_INTEGRITY_CHECK_ENABLED` | bool | `true` | _No description_ |
| `CHECKSUM_VALIDATION_ENABLED` | bool | `true` | _No description_ |
| `CORRUPTION_DETECTION_ENABLED` | bool | `true` | _No description_ |
| `CREDIT_CARD_REGEX_PATTERN` | string | `@"^\d{4}[\s-]?\d{4}[\s-]?\d{4}[\s-]?\d{4}$"` | _No description_ |
| `DEFAULT_DECIMAL_PRECISION` | int | `2` | _No description_ |
| `EMAIL_MAX_LENGTH` | int | `254` | _No description_ |
| `EMAIL_REGEX_PATTERN` | string | `@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2...` | _No description_ |
| `FILENAME_REGEX_PATTERN` | string | `@"^[^&lt;&gt;:""/\\\|?*]+$"` | _No description_ |
| `GUID_REGEX_PATTERN` | string | `@"^[{(]?[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){...` | _No description_ |
| `HEX_COLOR_REGEX_PATTERN` | string | `@"^#([A-Fa-f0-9]{6}\|[A-Fa-f0-9]{3})$"` | _No description_ |
| `INTEGRITY_CHECK_INTERVAL_MS` | int | `30000` | _No description_ |
| `INTEGRITY_HASH_ALGORITHM` | string | `"SHA256"` | _No description_ |
| `IPV4_REGEX_PATTERN` | string | `@"^(?:(?:25[0-5]\|2[0-4][0-9]\|[01]?[0-9][0-9]?...` | _No description_ |
| `IPV6_REGEX_PATTERN` | string | `@"^(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$"` | _No description_ |
| `MAX_ARRAY_LENGTH` | int | `10000` | _No description_ |
| `MAX_DECIMAL_PRECISION` | int | `10` | _No description_ |
| `MAX_NUMERIC_VALUE` | double | `double.MaxValue` | _No description_ |
| `MAX_STRING_LENGTH` | int | `1000` | _No description_ |
| `MAX_VALIDATION_ATTEMPTS` | int | `3` | _No description_ |
| `MIN_ARRAY_LENGTH` | int | `0` | _No description_ |
| `MIN_NUMERIC_VALUE` | double | `double.MinValue` | _No description_ |
| `MIN_STRING_LENGTH` | int | `1` | _No description_ |
| `NUMERIC_REGEX_PATTERN` | string | `@"^[0-9]+$"` | _No description_ |
| `PASSWORD_MAX_LENGTH` | int | `128` | _No description_ |
| `PASSWORD_MIN_LENGTH` | int | `8` | _No description_ |
| `PHONE_REGEX_PATTERN` | string | `@"^\+?[1-9]\d{1,14}$"` | _No description_ |
| `STRONG_PASSWORD_REGEX_PATTERN` | string | `@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]...` | _No description_ |
| `UNIX_PATH_REGEX_PATTERN` | string | `@"^\/(?:[^\/\0]+\/)*[^\/\0]*$"` | _No description_ |
| `URL_MAX_LENGTH` | int | `2048` | _No description_ |
| `URL_REGEX_PATTERN` | string | `@"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,...` | _No description_ |
| `VALIDATION_CUSTOM` | string | `"Custom validation failed"` | _No description_ |
| `VALIDATION_DUPLICATE` | string | `"Value already exists"` | _No description_ |
| `VALIDATION_FILE_SIZE` | string | `"File size exceeds maximum allowed"` | _No description_ |
| `VALIDATION_FILE_TYPE` | string | `"Invalid file type"` | _No description_ |
| `VALIDATION_INVALID_DATE` | string | `"Invalid date format"` | _No description_ |
| `VALIDATION_INVALID_EMAIL` | string | `"Invalid email address"` | _No description_ |
| `VALIDATION_INVALID_FORMAT` | string | `"Invalid format"` | _No description_ |
| `VALIDATION_INVALID_PHONE` | string | `"Invalid phone number"` | _No description_ |
| `VALIDATION_INVALID_URL` | string | `"Invalid URL format"` | _No description_ |
| `VALIDATION_LENGTH_TEMPLATE` | string | `"Length must be between {0} and {1} characters"` | _No description_ |
| `VALIDATION_NOT_NUMERIC` | string | `"Value must be numeric"` | _No description_ |
| `VALIDATION_RANGE_TEMPLATE` | string | `"Value must be between {0} and {1}"` | _No description_ |
| `VALIDATION_REQUIRED` | string | `"This field is required"` | _No description_ |
| `VALIDATION_RESULT_CANCELLED` | string | `"Cancelled"` | _No description_ |
| `VALIDATION_RESULT_ERROR` | string | `"Error"` | _No description_ |
| `VALIDATION_RESULT_FAILURE` | string | `"Failure"` | _No description_ |
| `VALIDATION_RESULT_PENDING` | string | `"Pending"` | _No description_ |
| `VALIDATION_RESULT_SKIPPED` | string | `"Skipped"` | _No description_ |
| `VALIDATION_RESULT_SUCCESS` | string | `"Success"` | _No description_ |
| `VALIDATION_RESULT_TIMEOUT` | string | `"Timeout"` | _No description_ |
| `VALIDATION_RESULT_WARNING` | string | `"Warning"` | _No description_ |
| `VALIDATION_TIMEOUT_MS` | int | `5000` | _No description_ |
| `VALIDATION_TYPE_BOOLEAN` | string | `"Boolean"` | _No description_ |
| `VALIDATION_TYPE_CUSTOM` | string | `"Custom"` | _No description_ |
| `VALIDATION_TYPE_DATE` | string | `"Date"` | _No description_ |
| `VALIDATION_TYPE_EMAIL` | string | `"Email"` | _No description_ |
| `VALIDATION_TYPE_FILE` | string | `"File"` | _No description_ |
| `VALIDATION_TYPE_JSON` | string | `"JSON"` | _No description_ |
| `VALIDATION_TYPE_NUMERIC` | string | `"Numeric"` | _No description_ |
| `VALIDATION_TYPE_PASSWORD` | string | `"Password"` | _No description_ |
| `VALIDATION_TYPE_PHONE` | string | `"Phone"` | _No description_ |
| `VALIDATION_TYPE_STRING` | string | `"String"` | _No description_ |
| `VALIDATION_TYPE_URL` | string | `"URL"` | _No description_ |
| `VALIDATION_TYPE_XML` | string | `"XML"` | _No description_ |
| `VALIDATION_WEAK_PASSWORD` | string | `"Password must contain uppercase, lowercase, nu...` | _No description_ |
| `VERSION_REGEX_PATTERN` | string | `@"^\d+\.\d+(\.\d+)?(\.\d+)?$"` | _No description_ |
| `WINDOWS_PATH_REGEX_PATTERN` | string | `@"^[a-zA-Z]:\\(?:[^&lt;&gt;:""/\\\|?*]+\\)*[^&l...` | _No description_ |


## Usage Statistics

### Constants by Type

| Type | Count | Percentage |
|------|-------|------------|
| string | 1046 | 56.9% |
| int | 503 | 27.4% |
| float | 170 | 9.2% |
| double | 57 | 3.1% |
| char | 36 | 2% |
| long | 17 | 0.9% |
| bool | 10 | 0.5% |

## Usage Examples

Below are examples of how some key constants are used in the codebase:

### LOG_LEVEL_CRITICAL

**Definition**: `int LOG_LEVEL_CRITICAL = LoggingConstants.LOG_LEVEL_CRITICAL`

**Usage Examples**:
```
// CpuMonitor.cs:37
Main.logger?.Msg(LOG_LEVEL_CRITICAL, string.Format(STRING_FORMAT_TWO_ARGS, CPU_MONITOR_PREFIX, string.Format("Current CPU usage: {0:F2}%", cpuUsage)));

// IL2CPPTypeResolver.cs:77
logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] Build environment detected: {0}", _isIL2CPP ? IL2CPP_BUILD : MONO_BUILD));

```

### LOG_LEVEL_VERBOSE

**Definition**: `int LOG_LEVEL_VERBOSE = LoggingConstants.LOG_LEVEL_VERBOSE`

**Usage Examples**:
```
// IL2CPPTypeResolver.cs:118
logger.Msg(LOG_LEVEL_VERBOSE, string.Format("[TYPE_RESOLVER] IL2CPP: MixingStationConfiguration found in assembly: {0}", assembly.FullName));

// Logger.cs:37
public int CurrentMsgLogLevel = LOG_LEVEL_VERBOSE;

```


---

_This documentation was auto-generated by Generate-ConstantsDoc.ps1 on 2025-07-16 16:55:50_

