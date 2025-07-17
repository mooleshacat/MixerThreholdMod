# Constants Audit Report

**Generated**: 2025-07-17 00:51:40
**Total Constants**: 1839
**Files Scanned**: 15
**Code Files Analyzed**: 49

## Executive Summary

⚠️ **MODERATE UTILIZATION** - 5.2% of constants are actively used.

**Recommendation**: Review and clean up unused constants to reduce code bloat.

| Metric | Value | Status |
|--------|-------|--------|
| **Total Constants** | 1839 | - |
| **Used Constants** | 95 | ⚠️ Needs Review |
| **Unused Constants** | 1744 | 🚨 Many |
| **Duplicate Names** | 195 | 🚨 Many |
| **Utilization Rate** | 5.2% | 🚨 Poor |

## Constants by File

| File | Total | Used | Unused | Utilization |
|------|-------|------|--------|-------------|
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\Constants_LargeOriginal.cs; FileName=Constants_LargeOriginal.cs; ConstantCount=832; UsedCount=60; PercentUsed=7.2}.FileName) | 832 | 60 | 772 | 🚨 7.2% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\AllConstants.cs; FileName=AllConstants.cs; ConstantCount=129; UsedCount=7; PercentUsed=5.4}.FileName) | 129 | 7 | 122 | 🚨 5.4% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\UtilityConstants.cs; FileName=UtilityConstants.cs; ConstantCount=127; UsedCount=0; PercentUsed=0}.FileName) | 127 | 0 | 127 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\NetworkConstants.cs; FileName=NetworkConstants.cs; ConstantCount=90; UsedCount=0; PercentUsed=0}.FileName) | 90 | 0 | 90 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\Constants_Original.cs; FileName=Constants_Original.cs; ConstantCount=86; UsedCount=8; PercentUsed=9.3}.FileName) | 86 | 8 | 78 | 🚨 9.3% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\GameConstants.cs; FileName=GameConstants.cs; ConstantCount=78; UsedCount=0; PercentUsed=0}.FileName) | 78 | 0 | 78 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\FileConstants.cs; FileName=FileConstants.cs; ConstantCount=73; UsedCount=0; PercentUsed=0}.FileName) | 73 | 0 | 73 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\ValidationConstants.cs; FileName=ValidationConstants.cs; ConstantCount=69; UsedCount=0; PercentUsed=0}.FileName) | 69 | 0 | 69 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\ErrorConstants.cs; FileName=ErrorConstants.cs; ConstantCount=65; UsedCount=0; PercentUsed=0}.FileName) | 65 | 0 | 65 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\SystemConstants.cs; FileName=SystemConstants.cs; ConstantCount=58; UsedCount=1; PercentUsed=1.7}.FileName) | 58 | 1 | 57 | 🚨 1.7% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\Constants.cs; FileName=Constants.cs; ConstantCount=54; UsedCount=6; PercentUsed=11.1}.FileName) | 54 | 6 | 48 | 🚨 11.1% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\LoggingConstants.cs; FileName=LoggingConstants.cs; ConstantCount=51; UsedCount=7; PercentUsed=13.7}.FileName) | 51 | 7 | 44 | 🚨 13.7% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\ThreadingConstants.cs; FileName=ThreadingConstants.cs; ConstantCount=48; UsedCount=1; PercentUsed=2.1}.FileName) | 48 | 1 | 47 | 🚨 2.1% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\MixerConstants.cs; FileName=MixerConstants.cs; ConstantCount=42; UsedCount=0; PercentUsed=0}.FileName) | 42 | 0 | 42 | 🚨 0% |
| $(@{File=C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0\Constants\PerformanceConstants.cs; FileName=PerformanceConstants.cs; ConstantCount=37; UsedCount=0; PercentUsed=0}.FileName) | 37 | 0 | 37 | 🚨 0% |

## Unused Constants

The following 1744 constants are declared but never used:

### AllConstants.cs

- **ALTERNATIVE_ACCESS** (string) - Line 311
- **AUDIO_DEFAULT_VOLUME** (float) - Line 205
- **AUDIO_MASTER_VOLUME** (float) - Line 204
- **BACKEND_IL2CPP** (string) - Line 184
- **BACKUP_CREATED_MESSAGE** (string) - Line 50
- **BACKUP_DIRECTORY** (string) - Line 114
- **BACKUP_EXTENSION** (string) - Line 109
- **BACKUP_THREAD_NAME** (string) - Line 137
- **BYTES_PER_KB** (long) - Line 280
- **BYTES_PER_MB** (long) - Line 281
- **COMMA_SEPARATOR** (string) - Line 272
- **COMPATIBILITY_ASSURANCE** (string) - Line 320
- **COMPONENT_BACKUP_MANAGER** (string) - Line 190
- **COMPONENT_LOGGER** (string) - Line 189
- **COMPONENT_SAVE_MANAGER** (string) - Line 188
- **CONFIG_DIRECTORY** (string) - Line 116
- **CONSOLE_COMMAND_DELAY_MS** (int) - Line 65
- **CONTENT_TYPE_JSON** (string) - Line 254
- **CONTENT_TYPE_TEXT** (string) - Line 256
- **CONTENT_TYPE_XML** (string) - Line 255
- **CULTURE_EN_US** (string) - Line 290
- **CULTURE_INVARIANT** (string) - Line 291
- **DATE_FORMAT_ISO8601** (string) - Line 285
- **DEFAULT_CANCELLATION_TIMEOUT_MS** (int) - Line 142
- **DEFAULT_DECIMAL_PRECISION** (int) - Line 225
- **DEFAULT_MIXER_CHANNEL** (int) - Line 86
- **DEFAULT_MIXER_VOLUME** (float) - Line 89
- **DEGREES_TO_RADIANS** (double) - Line 277
- **DOMAIN_FILES_COUNT** (int) - Line 305
- **DOTNET_VERSION** (string) - Line 185
- **E** (double) - Line 276
- **EMAIL_REGEX_PATTERN** (string) - Line 228
- **EMPTY_STRING** (string) - Line 269
- **ERROR_CODE_FILE_NOT_FOUND** (int) - Line 158
- **ERROR_CODE_GENERAL** (int) - Line 157
- **ERROR_CODE_SUCCESS** (int) - Line 156
- **ERROR_CODE_TIMEOUT** (int) - Line 159
- **ERROR_LOG_FILENAME** (string) - Line 54
- **ERROR_MESSAGE_FILE_OPERATION** (string) - Line 163
- **ERROR_MESSAGE_GENERIC** (string) - Line 162
- **ERROR_MESSAGE_TIMEOUT** (string) - Line 164
- **FILE_OP_BACKUP** (string) - Line 121
- **FILE_OP_READ** (string) - Line 119
- **FILE_OP_WRITE** (string) - Line 120
- **FILE_OPERATION_TIMEOUT_MS** (int) - Line 66
- **GRAPHICS_SCREEN_HEIGHT** (int) - Line 210
- **GRAPHICS_SCREEN_WIDTH** (int) - Line 209
- **GRAPHICS_TARGET_FPS** (int) - Line 208
- **HTTP_DEFAULT_TIMEOUT_MS** (int) - Line 259
- **HTTP_METHOD_GET** (string) - Line 244
- **HTTP_METHOD_POST** (string) - Line 245
- **HTTP_METHOD_PUT** (string) - Line 246
- **HTTP_STATUS_INTERNAL_SERVER_ERROR** (int) - Line 251
- **HTTP_STATUS_NOT_FOUND** (int) - Line 250
- **HTTP_STATUS_OK** (int) - Line 249
- **JSON_EXTENSION** (string) - Line 108
- **LOCK_TIMEOUT_MS** (int) - Line 141
- **LOG_EXTENSION** (string) - Line 111
- **LOG_LEVEL_IMPORTANT** (int) - Line 35
- **LOGGER_PREFIX** (string) - Line 44
- **LOGS_DIRECTORY** (string) - Line 115
- **MAIN_LOG_FILENAME** (string) - Line 53
- **MAIN_THREAD_NAME** (string) - Line 135
- **MAX_FILE_SIZE_BYTES** (long) - Line 124
- **MAX_FILENAME_LENGTH** (int) - Line 125
- **MAX_MIXER_VOLUME** (float) - Line 87
- **MAX_PATH_LENGTH** (int) - Line 126
- **MAX_STRING_LENGTH** (int) - Line 224
- **MAX_WORKER_THREADS** (int) - Line 146
- **MEDIUM_WAIT_MS** (int) - Line 76
- **MIN_MIXER_VOLUME** (float) - Line 88
- **MIN_STRING_LENGTH** (int) - Line 223
- **MIN_WORKER_THREADS** (int) - Line 145
- **MIXER_BACKUP_FILENAME** (string) - Line 98
- **MIXER_CHANNEL_COUNT** (int) - Line 85
- **MIXER_CONFIG_FILENAME** (string) - Line 99
- **MIXER_GAIN_KEY** (string) - Line 94
- **MIXER_SAVE_FILENAME** (string) - Line 97
- **MIXER_VALIDATION_PREFIX** (string) - Line 45
- **MIXER_VALUES_KEY** (string) - Line 92
- **MIXER_VOLUME_KEY** (string) - Line 93
- **MOD_NAME** (string) - Line 178
- **MOD_NAMESPACE** (string) - Line 180
- **MOD_VERSION** (string) - Line 179
- **MS_PER_SECOND** (int) - Line 282
- **MUTEX_TIMEOUT_MS** (int) - Line 140
- **NEWLINE** (string) - Line 271
- **NUMERIC_REGEX_PATTERN** (string) - Line 230
- **OPERATION_TIMEOUT_MS** (int) - Line 64
- **ORGANIZATION_PRINCIPLE** (string) - Line 314
- **PERFORMANCE_CRITICAL_THRESHOLD_MS** (int) - Line 71
- **PERFORMANCE_LOG_FILENAME** (string) - Line 55
- **PERFORMANCE_SLOW_THRESHOLD_MS** (int) - Line 70
- **PERFORMANCE_WARNING_THRESHOLD_MS** (int) - Line 69
- **PHYSICS_FIXED_TIMESTEP** (float) - Line 214
- **PHYSICS_GRAVITY** (float) - Line 213
- **PI** (double) - Line 275
- **PLATFORM_UNITY** (string) - Line 183
- **RECOVERY_STRATEGY_EMERGENCY_SAVE** (string) - Line 169
- **RECOVERY_STRATEGY_FALLBACK** (string) - Line 168
- **RECOVERY_STRATEGY_RETRY** (string) - Line 167
- **RETRY_DELAY_MS** (int) - Line 74
- **SAVE_FAILURE_MESSAGE** (string) - Line 49
- **SAVE_MANAGER_PREFIX** (string) - Line 41
- **SAVE_SUCCESS_MESSAGE** (string) - Line 48
- **SAVE_THREAD_NAME** (string) - Line 136
- **SHORT_WAIT_MS** (int) - Line 75
- **SPACE** (string) - Line 270
- **TCP_CONNECTION_TIMEOUT_MS** (int) - Line 260
- **TEMP_EXTENSION** (string) - Line 110
- **THREAD_SAFETY_GUARANTEE** (string) - Line 317
- **TIME_FORMAT_24HOUR** (string) - Line 286
- **TIMESTAMP_FORMAT_MS** (string) - Line 287
- **TOTAL_CONSTANTS_COUNT** (int) - Line 302
- **UI_BUTTON_HEIGHT** (int) - Line 201
- **UI_BUTTON_WIDTH** (int) - Line 200
- **UI_FONT_SIZE_DEFAULT** (int) - Line 199
- **URL_REGEX_PATTERN** (string) - Line 229
- **USAGE_PATTERN** (string) - Line 308
- **VALIDATION_INVALID_EMAIL** (string) - Line 235
- **VALIDATION_INVALID_FORMAT** (string) - Line 234
- **VALIDATION_REQUIRED** (string) - Line 233

### Constants.cs

- **BACKUP_EXTENSION** (string) - Line 51
- **CHANNEL_COUNT** (int) - Line 94
- **CONSTANTS_REFACTOR_AUTHOR** (string) - Line 157
- **CONSTANTS_REFACTOR_DATE** (string) - Line 156
- **CONSTANTS_REFACTOR_DESCRIPTION** (string) - Line 158
- **CONSTANTS_REFACTOR_VERSION** (string) - Line 155
- **DIR_BACKUP** (string) - Line 104
- **DOMAIN_FILES_COUNT** (int) - Line 149
- **DOMAIN_IMPORT_EXAMPLE** (string) - Line 140
- **ERROR_CODE_GENERAL** (int) - Line 61
- **ERROR_CODE_SUCCESS** (int) - Line 60
- **EXT_BACKUP** (string) - Line 102
- **EXT_JSON** (string) - Line 101
- **EXT_LOG** (string) - Line 103
- **FILE_NOT_FOUND** (int) - Line 123
- **JSON_EXTENSION** (string) - Line 50
- **LOCK_TIMEOUT** (int) - Line 115
- **LOG_LEVEL_IMPORTANT** (int) - Line 41
- **MAIN_THREAD** (string) - Line 112
- **MAX_VOLUME** (float) - Line 95
- **MIGRATION_INSTRUCTIONS** (string) - Line 137
- **MIXER_SAVE_FILENAME** (string) - Line 47
- **MIXER_VALUES_KEY** (string) - Line 48
- **MOD_NAME** (string) - Line 57
- **MOD_VERSION** (string) - Line 58
- **MSG_FAILURE** (string) - Line 77
- **MSG_SUCCESS** (string) - Line 76
- **MUTEX_TIMEOUT** (int) - Line 114
- **OP_READ** (string) - Line 105
- **OP_WRITE** (string) - Line 106
- **OPERATION_TIMEOUT_MS** (int) - Line 44
- **PERFORMANCE_WARNING_THRESHOLD_MS** (int) - Line 45
- **PREFIX_BACKUP** (string) - Line 74
- **PREFIX_LOGGER** (string) - Line 75
- **PREFIX_SAVE** (string) - Line 73
- **RETRY_DELAY** (int) - Line 86
- **SAVE_FILE** (string) - Line 92
- **SAVE_MANAGER_PREFIX** (string) - Line 53
- **SAVE_THREAD** (string) - Line 113
- **SEPARATION_BENEFITS** (string) - Line 143
- **STRATEGY_EMERGENCY** (string) - Line 126
- **STRATEGY_RETRY** (string) - Line 125
- **THRESHOLD_WARNING** (int) - Line 85
- **TIMEOUT** (int) - Line 124
- **TIMEOUT_FILE_OP** (int) - Line 84
- **TIMEOUT_OPERATION** (int) - Line 83
- **TOTAL_CONSTANTS_AVAILABLE** (int) - Line 146
- **VALUES_KEY** (string) - Line 93

### Constants_LargeOriginal.cs

- **ACTION_PARAM_NAME** (string) - Line 739
- **ADVANCED_SAVE_OPERATION_PREFIX** (string) - Line 120
- **ANIMATION_BLEND_TIME_DEFAULT** (float) - Line 1943
- **ANIMATION_CROSSFADE_TIME_DEFAULT** (float) - Line 1946
- **ANIMATION_FRAME_RATE_HIGH** (int) - Line 1940
- **ANIMATION_FRAME_RATE_STANDARD** (int) - Line 1937
- **ANIMATION_LOOP_COUNT_INFINITE** (int) - Line 1964
- **ANIMATION_PLAYBACK_SPEED_DEFAULT** (float) - Line 1955
- **ANIMATION_PLAYBACK_SPEED_MAX** (float) - Line 1952
- **ANIMATION_PLAYBACK_SPEED_MIN** (float) - Line 1949
- **ANIMATION_TANGENT_MODE_LINEAR** (int) - Line 1958
- **ANIMATION_TANGENT_MODE_SMOOTH** (int) - Line 1961
- **APP_CONFIG_FILENAME** (string) - Line 1009
- **ARRAY_COPY_METHOD** (string) - Line 619
- **ARRAY_EMPTY_METHOD** (string) - Line 622
- **ASSEMBLY_CSHARP** (string) - Line 354
- **ASSEMBLY_FILE_VERSION_FORMAT** (string) - Line 1319
- **ASSEMBLY_LOAD_METHOD** (string) - Line 409
- **ASSEMBLY_LOCATION_PROPERTY** (string) - Line 412
- **ASSEMBLY_VERSION_FORMAT** (string) - Line 1316
- **ASYNC_CANCELLATION_TIMEOUT_SECONDS** (int) - Line 321
- **ATTACH_TIMEOUT_SECONDS** (float) - Line 55
- **ATTEMPTING_BACKUP_MSG** (string) - Line 814
- **ATTEMPTING_DELETE_BACKUP_MSG** (string) - Line 817
- **AUDIO_BIT_DEPTH_STANDARD** (int) - Line 1822
- **AUDIO_BUFFER_SIZE_DEFAULT** (int) - Line 1813
- **AUDIO_CHANNELS_MONO** (int) - Line 1816
- **AUDIO_CHANNELS_STEREO** (int) - Line 1819
- **AUDIO_FADE_DURATION_SECONDS** (float) - Line 1807
- **AUDIO_PROCESSING_INTERVAL_MS** (int) - Line 1825
- **AUDIO_SAMPLE_RATE_STANDARD** (int) - Line 1810
- **AUDIO_VOLUME_DEFAULT** (float) - Line 1804
- **AUDIO_VOLUME_MAX** (float) - Line 1801
- **AUDIO_VOLUME_MIN** (float) - Line 1798
- **BACKGROUND_THREAD_PRIORITY** (string) - Line 1471
- **BACKSLASH** (string) - Line 600
- **BACKUP_ACCESS_DENIED_WARNING** (string) - Line 808
- **BACKUP_CLEANUP_COMPLETED_MSG** (string) - Line 805
- **BACKUP_DIRECTORY_NAME** (string) - Line 1033
- **BACKUP_DIRECTORY_NOT_EXIST_WARNING** (string) - Line 811
- **BACKUP_FILE_EXTENSION** (string) - Line 344
- **BACKUP_FILENAME_PATTERN** (string) - Line 96
- **BACKUP_FILENAME_WILDCARD** (string) - Line 99
- **BACKUP_INTERVAL_MINUTES** (int) - Line 64
- **BACKUP_OPERATION_PREFIX** (string) - Line 126
- **BACKUP_ROOT_MSG** (string) - Line 861
- **BACKUP_SAVE_FOLDER_COMPLETED_MSG** (string) - Line 843
- **BACKUP_SAVE_FOLDER_ERROR_MSG** (string) - Line 849
- **BACKUP_SAVE_FOLDER_FINISHED_MSG** (string) - Line 852
- **BACKUP_SAVE_FOLDER_GENERIC_STARTED_MSG** (string) - Line 855
- **BACKUP_SAVE_FOLDER_IN_PROGRESS_MSG** (string) - Line 840
- **BACKUP_SAVE_FOLDER_NULL_PATH_MSG** (string) - Line 846
- **BACKUP_SAVE_FOLDER_STARTED_MSG** (string) - Line 837
- **BACKUP_SOURCE_NOT_FOUND_WARNING** (string) - Line 802
- **BACKUP_SUCCESS_MSG** (string) - Line 793
- **BACKUP_TASK_FAILED_MSG** (string) - Line 858
- **BAK_FILE_EXTENSION** (string) - Line 510
- **BINDING_FLAGS_INSTANCE** (string) - Line 434
- **BINDING_FLAGS_NON_PUBLIC** (string) - Line 428
- **BINDING_FLAGS_PUBLIC** (string) - Line 425
- **BINDING_FLAGS_STATIC** (string) - Line 431
- **BRACKET_CLOSE_ANGLE** (string) - Line 2436
- **BRACKET_CLOSE_CURLY** (string) - Line 2430
- **BRACKET_CLOSE_PAREN** (string) - Line 2418
- **BRACKET_CLOSE_SQUARE** (string) - Line 2424
- **BRACKET_OPEN_ANGLE** (string) - Line 2433
- **BRACKET_OPEN_CURLY** (string) - Line 2427
- **BRACKET_OPEN_PAREN** (string) - Line 2415
- **BRACKET_OPEN_SQUARE** (string) - Line 2421
- **BRIEF_WAIT_SECONDS** (float) - Line 962
- **BUILD_CONFIG_DEBUG** (string) - Line 1254
- **BUILD_CONFIG_RELEASE** (string) - Line 1257
- **BYTES_PER_GB** (long) - Line 1443
- **BYTES_PER_KB** (int) - Line 1437
- **BYTES_PER_MB** (int) - Line 1440
- **BYTES_TO_KB** (int) - Line 176
- **BYTES_TO_KILOBYTES** (int) - Line 566
- **BYTES_TO_MB** (double) - Line 179
- **CACHE_DIRECTORY_NAME** (string) - Line 1018
- **CACHE_EVICTION_THRESHOLD** (double) - Line 996
- **CALLBACK_PARAM_NAME** (string) - Line 745
- **CAMERA_FAR_CLIP_DEFAULT** (float) - Line 2119
- **CAMERA_FAR_CLIP_MAX** (float) - Line 2116
- **CAMERA_FAR_CLIP_MIN** (float) - Line 2113
- **CAMERA_FOV_DEFAULT** (float) - Line 2101
- **CAMERA_FOV_MAX** (float) - Line 2098
- **CAMERA_FOV_MIN** (float) - Line 2095
- **CAMERA_NEAR_CLIP_DEFAULT** (float) - Line 2110
- **CAMERA_NEAR_CLIP_MAX** (float) - Line 2107
- **CAMERA_NEAR_CLIP_MIN** (float) - Line 2104
- **CAMERA_ORTHO_SIZE_MAX** (float) - Line 2125
- **CAMERA_ORTHO_SIZE_MIN** (float) - Line 2122
- **CANCELLATION_TOKEN_PARAM** (string) - Line 223
- **COMMA_SEPARATOR** (string) - Line 588
- **COMMAND_HELP** (string) - Line 213
- **COMMAND_MIXER_EMERGENCY** (string) - Line 201
- **COMMAND_MIXER_PATH** (string) - Line 198
- **COMMAND_MIXER_RESET** (string) - Line 216
- **COMMAND_MIXER_SAVE** (string) - Line 195
- **COMMAND_RESET_MIXER_VALUES** (string) - Line 192
- **COMMAND_SAVE_GAME_STRESS** (string) - Line 207
- **COMMAND_SAVE_MONITOR** (string) - Line 210
- **COMMAND_SAVE_PREF_STRESS** (string) - Line 204
- **CONFIG_AUTO_SAVE_INTERVAL_MINUTES** (int) - Line 1608
- **CONFIG_BACKUP_RETENTION_COUNT** (int) - Line 1602
- **CONFIG_DIRECTORY_NAME** (string) - Line 1024
- **CONFIG_FILE_CHECK_INTERVAL_MINUTES** (int) - Line 1599
- **CONFIG_RELOAD_DELAY_MS** (int) - Line 1611
- **CONFIG_VALIDATION_TIMEOUT_MS** (int) - Line 1605
- **CONSOLE_BYPASS_PARAM_DESC** (string) - Line 703
- **CONSOLE_COMMAND_DELAY_MS** (int) - Line 40
- **CONSOLE_COUNT_PARAM_DESC** (string) - Line 700
- **CONSOLE_DELAY_PARAM_DESC** (string) - Line 697
- **CONSOLE_EXAMPLES_HEADER** (string) - Line 673
- **CONSOLE_FORMATTING_NOTE** (string) - Line 685
- **CONSOLE_INVALID_COUNT_ERROR** (string) - Line 694
- **CONSOLE_LOG_FILENAME** (string) - Line 882
- **CONSOLE_MESSAGE_PREFIX** (string) - Line 654
- **CONSOLE_MISSING_COUNT_ERROR** (string) - Line 688
- **CONSOLE_MISSING_MESSAGE_ERROR** (string) - Line 691
- **CONSOLE_OPTIONAL_HEADER** (string) - Line 679
- **CONSOLE_PARAMETERS_INFO** (string) - Line 682
- **CONSOLE_REQUIRED_HEADER** (string) - Line 676
- **COUNT_PROPERTY_NAME** (string) - Line 418
- **CPU_USAGE_THRESHOLD_PERCENT** (double) - Line 984
- **CREATE_FAILURE_METHOD** (string) - Line 635
- **CREATE_SUCCESS_METHOD** (string) - Line 638
- **CRITICAL_ERROR_THRESHOLD** (int) - Line 1430
- **CRITICAL_THREAD_PRIORITY** (string) - Line 1477
- **CRLF** (string) - Line 609
- **CSHARP_LANGUAGE_VERSION** (string) - Line 1272
- **CULTURE_AR_SA** (string) - Line 2562
- **CULTURE_BG_BG** (string) - Line 2595
- **CULTURE_CS_CZ** (string) - Line 2586
- **CULTURE_DA_DK** (string) - Line 2550
- **CULTURE_DE_DE** (string) - Line 2511
- **CULTURE_EL_GR** (string) - Line 2592
- **CULTURE_EN_UK** (string) - Line 2508
- **CULTURE_EN_US** (string) - Line 2505
- **CULTURE_ES_ES** (string) - Line 2517
- **CULTURE_ET_EE** (string) - Line 2613
- **CULTURE_FI_FI** (string) - Line 2553
- **CULTURE_FR_FR** (string) - Line 2514
- **CULTURE_HE_IL** (string) - Line 2565
- **CULTURE_HI_IN** (string) - Line 2568
- **CULTURE_HR_HR** (string) - Line 2601
- **CULTURE_HU_HU** (string) - Line 2589
- **CULTURE_ID_ID** (string) - Line 2577
- **CULTURE_INVARIANT** (string) - Line 2502
- **CULTURE_IT_IT** (string) - Line 2520
- **CULTURE_JA_JP** (string) - Line 2523
- **CULTURE_KO_KR** (string) - Line 2532
- **CULTURE_LT_LT** (string) - Line 2619
- **CULTURE_LV_LV** (string) - Line 2616
- **CULTURE_MS_MY** (string) - Line 2580
- **CULTURE_NL_NL** (string) - Line 2541
- **CULTURE_NO_NO** (string) - Line 2547
- **CULTURE_PL_PL** (string) - Line 2556
- **CULTURE_PT_BR** (string) - Line 2538
- **CULTURE_RO_RO** (string) - Line 2598
- **CULTURE_RU_RU** (string) - Line 2535
- **CULTURE_SK_SK** (string) - Line 2610
- **CULTURE_SL_SI** (string) - Line 2607
- **CULTURE_SR_RS** (string) - Line 2604
- **CULTURE_SV_SE** (string) - Line 2544
- **CULTURE_TH_TH** (string) - Line 2571
- **CULTURE_TR_TR** (string) - Line 2559
- **CULTURE_UK_UA** (string) - Line 2583
- **CULTURE_VI_VN** (string) - Line 2574
- **CULTURE_ZH_CN** (string) - Line 2526
- **CULTURE_ZH_TW** (string) - Line 2529
- **CURRENCY_FORMAT** (string) - Line 1179
- **CURRENT_DOMAIN_PROPERTY** (string) - Line 421
- **DATA_DIRECTORY_NAME** (string) - Line 1027
- **DATA_TYPE_BOOLEAN** (string) - Line 717
- **DATA_TYPE_DOUBLE** (string) - Line 729
- **DATA_TYPE_FLOAT** (string) - Line 726
- **DATA_TYPE_INTEGER** (string) - Line 723
- **DATA_TYPE_OBJECT** (string) - Line 732
- **DATA_TYPE_STRING** (string) - Line 720
- **DATABASE_DEFAULT_PAGE_SIZE** (int) - Line 1097
- **DATABASE_POOL_SIZE** (int) - Line 1100
- **DATABASE_TIMEOUT_THRESHOLD_SECONDS** (double) - Line 993
- **DATE_FORMAT_LONG** (string) - Line 1194
- **DATE_FORMAT_SHORT** (string) - Line 1191
- **DEBUG_MODE** (string) - Line 384
- **DECIMAL_FORMAT_ONE_PLACE** (string) - Line 1539
- **DECIMAL_FORMAT_THREE_PLACES** (string) - Line 1545
- **DECIMAL_FORMAT_TWO_PLACES** (string) - Line 1542
- **DEFAULT_CULTURE** (string) - Line 1003
- **DEFAULT_DECIMAL_PRECISION** (int) - Line 364
- **DEFAULT_ENCODING** (string) - Line 1006
- **DEFAULT_FILE_BUFFER_SIZE** (int) - Line 298
- **DEFAULT_LANGUAGE** (string) - Line 1000
- **DEFAULT_MIXER_ID** (int) - Line 271
- **DEFAULT_OPERATION_DELAY** (float) - Line 188
- **DEFAULT_RETRY_ATTEMPTS** (int) - Line 1415
- **DEFAULT_THREAD_POOL_SIZE** (int) - Line 1483
- **DEFAULT_THREAD_PRIORITY** (string) - Line 1468
- **DELEGATE_PARAM_NAME** (string) - Line 748
- **DELIMITER_AMPERSAND** (string) - Line 2375
- **DELIMITER_AT_SYMBOL** (string) - Line 2384
- **DELIMITER_BACKTICK** (string) - Line 2408
- **DELIMITER_CARET** (string) - Line 2393
- **DELIMITER_COLON** (string) - Line 2369
- **DELIMITER_COMMA** (string) - Line 2354
- **DELIMITER_DOLLAR** (string) - Line 2390
- **DELIMITER_EQUALS** (string) - Line 2372
- **DELIMITER_EXCLAMATION** (string) - Line 2411
- **DELIMITER_HASH** (string) - Line 2381
- **DELIMITER_MINUS** (string) - Line 2399
- **DELIMITER_PERCENT** (string) - Line 2387
- **DELIMITER_PIPE** (string) - Line 2360
- **DELIMITER_PLUS** (string) - Line 2396
- **DELIMITER_QUESTION_MARK** (string) - Line 2378
- **DELIMITER_SEMICOLON** (string) - Line 2357
- **DELIMITER_SPACE** (string) - Line 2366
- **DELIMITER_TAB** (string) - Line 2363
- **DELIMITER_TILDE** (string) - Line 2405
- **DELIMITER_UNDERSCORE** (string) - Line 2402
- **DETECT_DIRS_IDENTIFIER** (string) - Line 764
- **DIAGNOSTIC_CHECK_INTERVAL_SECONDS** (int) - Line 1580
- **DIAGNOSTIC_CRITICAL_THRESHOLD** (int) - Line 1595
- **DIAGNOSTIC_ERROR_THRESHOLD** (int) - Line 1592
- **DIAGNOSTIC_LOG_RETENTION_DAYS** (int) - Line 1583
- **DIAGNOSTIC_METRIC_INTERVAL_MS** (int) - Line 1586
- **DIAGNOSTIC_WARNING_THRESHOLD** (int) - Line 1589
- **DIAGNOSTICS_PREFIX** (string) - Line 138
- **DIRECTORIES_IDENTIFIER** (string) - Line 761
- **DIRECTORY_CREATE_METHOD** (string) - Line 453
- **DIRECTORY_EXISTS_METHOD** (string) - Line 441
- **DISK_USAGE_THRESHOLD_PERCENT** (double) - Line 987
- **DOUBLE_BACKSLASH** (string) - Line 603
- **ELLIPSIS** (string) - Line 591
- **EMAIL_REGEX_PATTERN** (string) - Line 1141
- **EMERGENCY_CLEANUP_TIMEOUT_MS** (int) - Line 1640
- **EMERGENCY_SAVE_FILENAME** (string) - Line 93
- **EMPTY_STRING_ARRAY** (string) - Line 616
- **ENCODING_ASCII** (string) - Line 2486
- **ENCODING_BASE64** (string) - Line 2495
- **ENCODING_HEX** (string) - Line 2498
- **ENCODING_ISO_8859_1** (string) - Line 2489
- **ENCODING_UTF16** (string) - Line 2480
- **ENCODING_UTF32** (string) - Line 2483
- **ENCODING_UTF8** (string) - Line 2477
- **ENCODING_WINDOWS_1252** (string) - Line 2492
- **ENCRYPTION_AES** (string) - Line 1122
- **ENCRYPTION_KEY_SIZE** (int) - Line 1134
- **ENCRYPTION_RSA** (string) - Line 1125
- **ENVIRONMENT_HOME** (string) - Line 1052
- **ENVIRONMENT_PATH** (string) - Line 1043
- **ENVIRONMENT_TEMP** (string) - Line 1046
- **ENVIRONMENT_USER** (string) - Line 1049
- **ERROR_MSG_ACCESS_DENIED** (string) - Line 2215
- **ERROR_MSG_ALREADY_INITIALIZED** (string) - Line 2230
- **ERROR_MSG_CONNECTION_FAILED** (string) - Line 2242
- **ERROR_MSG_DATA_CORRUPTION** (string) - Line 2245
- **ERROR_MSG_EMPTY_STRING** (string) - Line 2206
- **ERROR_MSG_FILE_NOT_FOUND** (string) - Line 2212
- **ERROR_MSG_INSUFFICIENT_MEMORY** (string) - Line 2251
- **ERROR_MSG_INVALID_CONFIGURATION** (string) - Line 2254
- **ERROR_MSG_INVALID_FORMAT** (string) - Line 2224
- **ERROR_MSG_INVALID_OPERATION** (string) - Line 2236
- **ERROR_MSG_INVALID_RANGE** (string) - Line 2209
- **ERROR_MSG_NOT_INITIALIZED** (string) - Line 2233
- **ERROR_MSG_NOT_SUPPORTED** (string) - Line 2227
- **ERROR_MSG_NULL_PARAMETER** (string) - Line 2203
- **ERROR_MSG_OPERATION_CANCELLED** (string) - Line 2221
- **ERROR_MSG_OPERATION_FAILED** (string) - Line 2257
- **ERROR_MSG_RESOURCE_EXHAUSTED** (string) - Line 2239
- **ERROR_MSG_TIMEOUT_OCCURRED** (string) - Line 2218
- **ERROR_MSG_VERSION_MISMATCH** (string) - Line 2248
- **ERROR_OPERATION_PREFIX** (string) - Line 663
- **ERROR_RECOVERY_TIMEOUT_MS** (int) - Line 1433
- **ESCAPE_ALERT** (string) - Line 2470
- **ESCAPE_BACKSLASH** (string) - Line 2446
- **ESCAPE_BACKSPACE** (string) - Line 2467
- **ESCAPE_CARRIAGE_RETURN** (string) - Line 2455
- **ESCAPE_FORM_FEED** (string) - Line 2464
- **ESCAPE_FORWARD_SLASH** (string) - Line 2449
- **ESCAPE_NEWLINE** (string) - Line 2452
- **ESCAPE_NULL** (string) - Line 2473
- **ESCAPE_TAB** (string) - Line 2458
- **ESCAPE_VERTICAL_TAB** (string) - Line 2461
- **EXCEPTION_MESSAGE_PROPERTY** (string) - Line 397
- **EXCEPTION_STACKTRACE_PROPERTY** (string) - Line 400
- **EXTENDED_LONG_WAIT_SECONDS** (float) - Line 971
- **EXTENDED_WAIT_SECONDS** (float) - Line 340
- **EXTRA_LARGE_COLLECTION_SIZE** (int) - Line 1408
- **FAILURE_RESULT** (string) - Line 644
- **FIFTY_INT** (int) - Line 946
- **FIFTY_INT** (int) - Line 1344
- **FILE_DELETE_METHOD** (string) - Line 450
- **FILE_EXISTS_METHOD** (string) - Line 438
- **FILE_LOCK_PREFIX** (string) - Line 132
- **FILE_OPERATION_TIMEOUT_SECONDS** (float) - Line 304
- **FILE_READ_ALL_TEXT_METHOD** (string) - Line 444
- **FILE_WRITE_ALL_TEXT_METHOD** (string) - Line 447
- **FINAL_CLEANUP_ATTEMPTS** (int) - Line 1643
- **FIVE_FLOAT** (float) - Line 901
- **FIVE_INT** (int) - Line 1335
- **FIVE_INT** (int) - Line 934
- **FORCE_SHUTDOWN_TIMEOUT_SECONDS** (int) - Line 1634
- **FORWARD_SLASH** (string) - Line 597
- **FRAME_RATE_THRESHOLD** (double) - Line 553
- **FRAMEWORK_VERSION_4_8** (float) - Line 517
- **FUNCTION_PARAM_NAME** (string) - Line 742
- **GAME_LOW_PERFORMANCE_FPS** (int) - Line 1508
- **GAME_MAX_FRAME_RATE** (int) - Line 1505
- **GAME_MIN_FRAME_RATE** (int) - Line 1499
- **GAME_QUALITY_LEVEL_DEFAULT** (int) - Line 1520
- **GAME_QUALITY_LEVEL_MAX** (int) - Line 1517
- **GAME_QUALITY_LEVEL_MIN** (int) - Line 1514
- **GAME_STATE_BACKUP_INTERVAL_MINUTES** (int) - Line 1754
- **GAME_STATE_CHECK_INTERVAL_SECONDS** (int) - Line 1745
- **GAME_STATE_EMERGENCY_SAVE_THRESHOLD** (int) - Line 1763
- **GAME_STATE_PERSISTENCE_INTERVAL_MINUTES** (int) - Line 1751
- **GAME_STATE_RECOVERY_TIMEOUT_SECONDS** (int) - Line 1760
- **GAME_STATE_TRANSITION_TIMEOUT_SECONDS** (int) - Line 1748
- **GAME_STATE_VALIDATION_ATTEMPTS** (int) - Line 1757
- **GAME_TARGET_FRAME_RATE** (int) - Line 1502
- **GAME_VSYNC_ENABLED_DEFAULT** (bool) - Line 1511
- **GC_COLLECTION_THRESHOLD_MB** (double) - Line 526
- **GET_EXECUTING_ASSEMBLY_METHOD** (string) - Line 406
- **GET_TYPE_METHOD_NAME** (string) - Line 403
- **GRACEFUL_SHUTDOWN_TIMEOUT_SECONDS** (int) - Line 1631
- **GRAPHICS_ANISOTROPIC_LEVELS** (int) - Line 1865
- **GRAPHICS_ANTIALIASING_MAX** (int) - Line 1871
- **GRAPHICS_ANTIALIASING_MIN** (int) - Line 1868
- **GRAPHICS_DEFAULT_HEIGHT** (int) - Line 1838
- **GRAPHICS_DEFAULT_WIDTH** (int) - Line 1835
- **GRAPHICS_LOD_BIAS_MAX** (float) - Line 1862
- **GRAPHICS_LOD_BIAS_MIN** (float) - Line 1859
- **GRAPHICS_MAX_HEIGHT** (int) - Line 1844
- **GRAPHICS_MAX_WIDTH** (int) - Line 1841
- **GRAPHICS_MIN_HEIGHT** (int) - Line 1832
- **GRAPHICS_MIN_WIDTH** (int) - Line 1829
- **GRAPHICS_SHADOW_DISTANCE_MAX** (float) - Line 1856
- **GRAPHICS_SHADOW_DISTANCE_MIN** (float) - Line 1853
- **GRAPHICS_TEXTURE_QUALITY_MAX** (int) - Line 1850
- **GRAPHICS_TEXTURE_QUALITY_MIN** (int) - Line 1847
- **GUID_REGEX_PATTERN** (string) - Line 1153
- **HALF_DOUBLE** (double) - Line 1380
- **HALF_FLOAT** (float) - Line 1362
- **HARMONY_MOD_ID** (string) - Line 103
- **HASH_MD5** (string) - Line 1131
- **HASH_SHA256** (string) - Line 1128
- **HEALTH_CHECK_INTERVAL_SECONDS** (int) - Line 1647
- **HEARTBEAT_INTERVAL_SECONDS** (int) - Line 1656
- **HIGH_PERFORMANCE_THRESHOLD** (float) - Line 532
- **HIGH_PRECISION_DECIMALS** (int) - Line 367
- **HIGH_THREAD_PRIORITY** (string) - Line 1474
- **HTTP_DEFAULT_PORT** (int) - Line 1084
- **HTTP_STATUS_INTERNAL_ERROR** (int) - Line 1081
- **HTTP_STATUS_NOT_FOUND** (int) - Line 1078
- **HTTP_STATUS_OK** (int) - Line 1075
- **HTTPS_DEFAULT_PORT** (int) - Line 1087
- **HUNDRED_FLOAT** (float) - Line 916
- **HUNDRED_INT** (int) - Line 949
- **IL2CPP_DOMAIN** (string) - Line 1235
- **INFO_MSG_CONNECTION_RESTORED** (string) - Line 2347
- **INFO_MSG_FEATURE_DISABLED** (string) - Line 2341
- **INFO_MSG_FEATURE_ENABLED** (string) - Line 2338
- **INFO_MSG_MAINTENANCE_SCHEDULED** (string) - Line 2335
- **INFO_MSG_NEW_VERSION_AVAILABLE** (string) - Line 2332
- **INFO_MSG_OPERATION_STARTED** (string) - Line 2323
- **INFO_MSG_PROGRESS_UPDATE** (string) - Line 2326
- **INFO_MSG_STATUS_CHANGED** (string) - Line 2329
- **INFO_MSG_SYSTEM_READY** (string) - Line 2344
- **INFO_MSG_UPDATE_AVAILABLE** (string) - Line 2350
- **INFO_OPERATION_PREFIX** (string) - Line 669
- **INITIALIZATION_CLEANUP_TIMEOUT_MS** (int) - Line 1627
- **INITIALIZATION_MAX_RETRIES** (int) - Line 1621
- **INITIALIZATION_RETRY_DELAY_MS** (int) - Line 1618
- **INITIALIZATION_TIMEOUT_SECONDS** (int) - Line 1615
- **INITIALIZATION_VALIDATION_TIMEOUT_MS** (int) - Line 1624
- **INPUT_DEADZONE_THRESHOLD** (float) - Line 1878
- **INPUT_DOUBLE_CLICK_INTERVAL_MS** (int) - Line 1890
- **INPUT_KEY_REPEAT_DELAY_MS** (int) - Line 1896
- **INPUT_KEY_REPEAT_RATE_MS** (int) - Line 1899
- **INPUT_LONG_PRESS_DURATION_MS** (int) - Line 1893
- **INPUT_MOUSE_WHEEL_SENSITIVITY** (float) - Line 1902
- **INPUT_POLLING_RATE_HZ** (int) - Line 1875
- **INPUT_SENSITIVITY_DEFAULT** (float) - Line 1887
- **INPUT_SENSITIVITY_MAX** (float) - Line 1884
- **INPUT_SENSITIVITY_MIN** (float) - Line 1881
- **INTEGER_FORMAT_NO_DECIMALS** (string) - Line 1548
- **INVALID_MIXER_ID** (int) - Line 274
- **IO_RUNNER_PREFIX** (string) - Line 135
- **ISO_DATETIME_FORMAT** (string) - Line 255
- **JSON_FILE_EXTENSION** (string) - Line 81
- **KB_TO_MB** (int) - Line 569
- **LARGE_COLLECTION_SIZE** (int) - Line 1405
- **LARGE_MEMORY_ALLOCATION_BYTES** (int) - Line 1455
- **LATEST_LOG_FILENAME** (string) - Line 879
- **LENGTH_PROPERTY_NAME** (string) - Line 415
- **LIGHTING_COLOR_TEMP_MAX** (float) - Line 2020
- **LIGHTING_COLOR_TEMP_MIN** (float) - Line 2017
- **LIGHTING_INTENSITY_DEFAULT** (float) - Line 1996
- **LIGHTING_INTENSITY_MAX** (float) - Line 1993
- **LIGHTING_INTENSITY_MIN** (float) - Line 1990
- **LIGHTING_RANGE_MAX** (float) - Line 2002
- **LIGHTING_RANGE_MIN** (float) - Line 1999
- **LIGHTING_SHADOW_STRENGTH_MAX** (float) - Line 2014
- **LIGHTING_SHADOW_STRENGTH_MIN** (float) - Line 2011
- **LIGHTING_SPOT_ANGLE_MAX** (float) - Line 2008
- **LIGHTING_SPOT_ANGLE_MIN** (float) - Line 2005
- **LOAD_TIMEOUT_SECONDS** (float) - Line 52
- **LOCALHOST_ADDRESS** (string) - Line 1090
- **LOCALHOST_HOSTNAME** (string) - Line 1093
- **LOCK_FILE_EXTENSION** (string) - Line 87
- **LOG_FILE_EXTENSION** (string) - Line 347
- **LOG_LEVEL_ERR** (string) - Line 713
- **LOG_LEVEL_IMPORTANT** (int) - Line 24
- **LOG_LEVEL_MSG** (string) - Line 707
- **LOG_LEVEL_WARN** (string) - Line 710
- **LOGS_DIRECTORY_NAME** (string) - Line 1021
- **LONG_WAIT_SECONDS** (float) - Line 337
- **MANUAL_OPERATION_PREFIX** (string) - Line 657
- **MATERIAL_ALPHA_CUTOFF_MAX** (float) - Line 2162
- **MATERIAL_ALPHA_CUTOFF_MIN** (float) - Line 2159
- **MATERIAL_EMISSION_INTENSITY_MAX** (float) - Line 2156
- **MATERIAL_EMISSION_INTENSITY_MIN** (float) - Line 2153
- **MATERIAL_METALLIC_MAX** (float) - Line 2132
- **MATERIAL_METALLIC_MIN** (float) - Line 2129
- **MATERIAL_NORMAL_SCALE_MAX** (float) - Line 2144
- **MATERIAL_NORMAL_SCALE_MIN** (float) - Line 2141
- **MATERIAL_PARALLAX_SCALE_MAX** (float) - Line 2150
- **MATERIAL_PARALLAX_SCALE_MIN** (float) - Line 2147
- **MATERIAL_SMOOTHNESS_MAX** (float) - Line 2138
- **MATERIAL_SMOOTHNESS_MIN** (float) - Line 2135
- **MAX_EMAIL_LENGTH** (int) - Line 1169
- **MAX_ERROR_MESSAGE_LENGTH** (int) - Line 284
- **MAX_EXPECTED_SAVE_FILE_SIZE_BYTES** (int) - Line 311
- **MAX_FILENAME_LENGTH** (int) - Line 1172
- **MAX_PASSWORD_LENGTH** (int) - Line 1160
- **MAX_PATH_LENGTH** (int) - Line 1175
- **MAX_REASONABLE_ARRAY_LENGTH** (int) - Line 1570
- **MAX_REASONABLE_COLLECTION_SIZE** (int) - Line 1411
- **MAX_REASONABLE_STRING_LENGTH** (int) - Line 1561
- **MAX_RETRY_ATTEMPTS** (int) - Line 278
- **MAX_RETRY_ATTEMPTS** (int) - Line 1418
- **MAX_RETRY_DELAY_MS** (int) - Line 1427
- **MAX_SAFE_COLLECTION_SIZE** (int) - Line 377
- **MAX_SAFE_LOOP_ITERATIONS** (int) - Line 374
- **MAX_SAFE_NUMERIC_VALUE** (double) - Line 1576
- **MAX_SAFE_STRING_LENGTH** (int) - Line 380
- **MAX_SAFE_STRING_LENGTH** (int) - Line 1564
- **MAX_THREAD_POOL_SIZE** (int) - Line 1486
- **MAX_USERNAME_LENGTH** (int) - Line 1166
- **MB_TO_GB** (int) - Line 572
- **MEDIUM_COLLECTION_SIZE** (int) - Line 1402
- **MEDIUM_MEMORY_ALLOCATION_BYTES** (int) - Line 1452
- **MEDIUM_WAIT_SECONDS** (float) - Line 331
- **MEDIUM_WAIT_TIME_SECONDS** (float) - Line 538
- **MELONLOADER_DEPENDENCIES_DIR** (string) - Line 1250
- **MELONLOADER_LIBS_DIR** (string) - Line 1244
- **MELONLOADER_LOG_FILENAME** (string) - Line 885
- **MELONLOADER_MODS_DIR** (string) - Line 1238
- **MELONLOADER_PLUGINS_DIR** (string) - Line 1241
- **MELONLOADER_USER_LIBS_DIR** (string) - Line 1247
- **MEMORY_CLEANUP_THRESHOLD_PERCENT** (double) - Line 1458
- **MEMORY_CRITICAL_THRESHOLD_PERCENT** (double) - Line 1464
- **MEMORY_LEAK_THRESHOLD_MB** (double) - Line 288
- **MEMORY_OPTIMIZATION_THRESHOLD_KB** (double) - Line 520
- **MEMORY_OPTIMIZATION_THRESHOLD_MB** (double) - Line 523
- **MEMORY_PRESSURE_THRESHOLD_MB** (double) - Line 981
- **MEMORY_SIZE_PRECISION** (int) - Line 370
- **MEMORY_THRESHOLD_KB** (double) - Line 547
- **MEMORY_WARNING_THRESHOLD_PERCENT** (double) - Line 1461
- **MIN_PASSWORD_LENGTH** (int) - Line 1157
- **MIN_THREAD_POOL_SIZE** (int) - Line 1480
- **MIN_USERNAME_LENGTH** (int) - Line 1163
- **MIN_VALID_ARRAY_LENGTH** (int) - Line 1567
- **MIN_VALID_FILE_SIZE_BYTES** (int) - Line 308
- **MIN_VALID_JSON_LENGTH** (int) - Line 314
- **MIN_VALID_NUMERIC_VALUE** (double) - Line 1573
- **MIN_VALID_STRING_LENGTH** (int) - Line 1558
- **MINUTES_TO_HOURS** (int) - Line 581
- **MIXER_ACTIVATION_DELAY_MS** (int) - Line 1714
- **MIXER_ATTACH_ERROR_MSG** (string) - Line 833
- **MIXER_ATTACH_FINISHED_MSG** (string) - Line 824
- **MIXER_ATTACH_STARTED_MSG** (string) - Line 821
- **MIXER_AUTO_SAVE_THRESHOLD_CHANGES** (int) - Line 1738
- **MIXER_BATCH_PROCESSING_SIZE** (int) - Line 1729
- **MIXER_COMPONENT_SCAN_INTERVAL_SECONDS** (int) - Line 1726
- **MIXER_CONFIG_ENABLED_DEFAULT** (bool) - Line 74
- **MIXER_CONFIG_VALIDATION_TIMEOUT_MS** (int) - Line 1741
- **MIXER_DATA_SYNC_INTERVAL_SECONDS** (int) - Line 1723
- **MIXER_DEACTIVATION_DELAY_MS** (int) - Line 1717
- **MIXER_ID_KEY** (string) - Line 239
- **MIXER_PRIORITY_QUEUE_CAPACITY** (int) - Line 1732
- **MIXER_SAVE_FILENAME** (string) - Line 90
- **MIXER_START_THRESHOLD_FOUND_MSG** (string) - Line 827
- **MIXER_THRESHOLD_CHANGE_SENSITIVITY** (float) - Line 1735
- **MIXER_THRESHOLD_MAX** (float) - Line 71
- **MIXER_THRESHOLD_MIN** (float) - Line 68
- **MIXER_THRESHOLD_VALIDATION_ATTEMPTS** (int) - Line 1720
- **MIXER_USING_POLLING_MSG** (string) - Line 830
- **MIXER_VALUE_TOLERANCE** (float) - Line 77
- **MIXER_VALUES_KEY** (string) - Line 230
- **MOD_NAME** (string) - Line 109
- **MOD_VERSION** (string) - Line 106
- **MODERATE_WAIT_SECONDS** (float) - Line 968
- **MONITORING_DATA_RETENTION_HOURS** (int) - Line 1659
- **MS_PER_SECOND** (int) - Line 182
- **MS_TO_SECONDS** (int) - Line 575
- **NETWORK_BUFFER_SIZE_BYTES** (int) - Line 1678
- **NETWORK_CONNECTION_TIMEOUT_MS** (int) - Line 1663
- **NETWORK_MAX_RETRY_ATTEMPTS** (int) - Line 1675
- **NETWORK_READ_TIMEOUT_MS** (int) - Line 1666
- **NETWORK_RETRY_DELAY_MS** (int) - Line 1672
- **NETWORK_TIMEOUT_THRESHOLD_SECONDS** (double) - Line 990
- **NETWORK_WRITE_TIMEOUT_MS** (int) - Line 1669
- **NEWLINE** (string) - Line 606
- **NO_MIXER_DATA_ERROR** (string) - Line 172
- **NO_SAVE_PATH_ERROR** (string) - Line 169
- **NULL_COMMAND_FALLBACK** (string) - Line 259
- **NULL_PATH_FALLBACK** (string) - Line 262
- **NULL_STRING_FALLBACK** (string) - Line 265
- **NUMBER_FORMAT_2_DECIMALS** (string) - Line 1185
- **NUMBER_FORMAT_3_DECIMALS** (string) - Line 1188
- **ONE_FLOAT** (float) - Line 892
- **ONE_FLOAT** (float) - Line 1356
- **ONE_HUNDRED_INT** (int) - Line 1347
- **ONE_THOUSAND_INT** (int) - Line 1350
- **OPERATION_CONTEXT_PARAM** (string) - Line 226
- **OPERATION_FAILED_MSG** (string) - Line 780
- **OPERATION_TIMEOUT_MS** (int) - Line 37
- **ORDER_BY_DESCENDING_METHOD** (string) - Line 625
- **PARTICLE_EMISSION_RATE_MAX** (float) - Line 2030
- **PARTICLE_EMISSION_RATE_MIN** (float) - Line 2027
- **PARTICLE_GRAVITY_MODIFIER_MAX** (float) - Line 2054
- **PARTICLE_GRAVITY_MODIFIER_MIN** (float) - Line 2051
- **PARTICLE_LIFETIME_MAX** (float) - Line 2036
- **PARTICLE_LIFETIME_MIN** (float) - Line 2033
- **PARTICLE_START_SIZE_MAX** (float) - Line 2048
- **PARTICLE_START_SIZE_MIN** (float) - Line 2045
- **PARTICLE_START_SPEED_MAX** (float) - Line 2042
- **PARTICLE_START_SPEED_MIN** (float) - Line 2039
- **PARTICLE_SYSTEM_MAX_PARTICLES** (int) - Line 2024
- **PATCH_ENTITY_DESTROY_NAME** (string) - Line 112
- **PATCH_OPERATION_PREFIX** (string) - Line 660
- **PATH_COMBINE_METHOD** (string) - Line 456
- **PATH_GET_DIRECTORY_NAME_METHOD** (string) - Line 459
- **PATH_GET_FILENAME_METHOD** (string) - Line 462
- **PATHS_IDENTIFIER** (string) - Line 758
- **PERCENTAGE_FORMAT** (string) - Line 1551
- **PERCENTAGE_FORMAT** (string) - Line 1182
- **PERCENTAGE_FORMAT_ONE_DECIMAL** (string) - Line 1554
- **PERFORMANCE_CRITICAL_THRESHOLD** (float) - Line 541
- **PERFORMANCE_METRICS_HISTORY_COUNT** (int) - Line 1294
- **PERFORMANCE_SAMPLE_INTERVAL_MS** (int) - Line 291
- **PERFORMANCE_SAMPLE_RATE** (float) - Line 529
- **PERFORMANCE_SLOW_THRESHOLD_MS** (int) - Line 46
- **PERFORMANCE_SUMMARY_LOG_INTERVAL** (int) - Line 1297
- **PERFORMANCE_TOLERANCE** (float) - Line 562
- **PERFORMANCE_WARNING_THRESHOLD_MS** (int) - Line 43
- **PHONE_REGEX_PATTERN** (string) - Line 1144
- **PHYSICS_ANGULAR_VELOCITY_THRESHOLD** (float) - Line 1924
- **PHYSICS_BOUNCE_THRESHOLD** (float) - Line 1927
- **PHYSICS_COLLISION_MODE_CONTINUOUS** (int) - Line 1915
- **PHYSICS_COLLISION_MODE_DISCRETE** (int) - Line 1912
- **PHYSICS_FRICTION_COEFFICIENT_DEFAULT** (float) - Line 1930
- **PHYSICS_GRAVITY_DEFAULT** (float) - Line 1906
- **PHYSICS_RESTITUTION_COEFFICIENT_DEFAULT** (float) - Line 1933
- **PHYSICS_SOLVER_ITERATIONS_DEFAULT** (int) - Line 1918
- **PHYSICS_TIME_STEP_FIXED** (float) - Line 1909
- **PHYSICS_VELOCITY_THRESHOLD** (float) - Line 1921
- **POLL_INTERVAL_SECONDS** (float) - Line 58
- **PREDICATE_PARAM_NAME** (string) - Line 736
- **QUALITY_CHECK_INTERVAL_OPERATIONS** (int) - Line 1698
- **QUALITY_CRITICAL_THRESHOLD_PERCENT** (double) - Line 1707
- **QUALITY_METRICS_HISTORY_COUNT** (int) - Line 1710
- **QUALITY_THRESHOLD_PERCENT** (double) - Line 1701
- **QUALITY_WARNING_THRESHOLD_PERCENT** (double) - Line 1704
- **QUARTER_FLOAT** (float) - Line 1365
- **QUESTION_MARK** (string) - Line 594
- **QUICK_WAIT_SECONDS** (float) - Line 965
- **QUOTE_DOUBLE** (string) - Line 2443
- **QUOTE_SINGLE** (string) - Line 2440
- **REGISTRY_HKEY_CURRENT_USER** (string) - Line 1037
- **REGISTRY_HKEY_LOCAL_MACHINE** (string) - Line 1040
- **RELEASE_MODE** (string) - Line 387
- **RESOURCE_CLEANUP_TIMEOUT_MS** (int) - Line 1637
- **RETRY_DELAY_BASE_MS** (int) - Line 1421
- **RETRY_DELAY_MS** (int) - Line 281
- **RETRY_DELAY_MULTIPLIER** (int) - Line 1424
- **SALT_SIZE** (int) - Line 1137
- **SAVE_COOLDOWN_SECONDS** (int) - Line 61
- **SAVE_DATA_IDENTIFIER** (string) - Line 755
- **SAVE_MANAGER_TYPE_NOT_FOUND_MSG** (string) - Line 868
- **SAVE_TIME_KEY** (string) - Line 233
- **SCHEDULE_I_DATA_DIR** (string) - Line 875
- **SECONDS_PER_MINUTE** (int) - Line 185
- **SECONDS_TO_MINUTES** (int) - Line 578
- **SECURITY_HASH_SALT_LENGTH** (int) - Line 1688
- **SECURITY_LOCKOUT_DURATION_MINUTES** (int) - Line 1694
- **SECURITY_MAX_FAILED_ATTEMPTS** (int) - Line 1691
- **SECURITY_TOKEN_EXPIRY_MINUTES** (int) - Line 1685
- **SECURITY_VALIDATION_TIMEOUT_MS** (int) - Line 1682
- **SETTINGS_CONFIG_FILENAME** (string) - Line 1012
- **SHADER_COMPILATION_TIMEOUT_MS** (int) - Line 1968
- **SHADER_FLOAT_PRECISION** (int) - Line 1971
- **SHADER_KEYWORD_MAX_COUNT** (int) - Line 1986
- **SHADER_PASS_MAX_COUNT** (int) - Line 1983
- **SHADER_TEXTURE_SLOT_MAX** (int) - Line 1974
- **SHADER_UNIFORM_BUFFER_SIZE_MAX** (int) - Line 1977
- **SHADER_VERTEX_ATTRIBUTE_MAX** (int) - Line 1980
- **SHORT_WAIT_SECONDS** (float) - Line 328
- **SINGLE_ITEM_COUNT** (int) - Line 1396
- **SINGLE_SPACE** (string) - Line 585
- **SIXTY_FLOAT** (float) - Line 913
- **SKIP_METHOD** (string) - Line 628
- **SMALL_COLLECTION_SIZE** (int) - Line 1399
- **SMALL_MEMORY_ALLOCATION_BYTES** (int) - Line 1449
- **SQL_DELETE** (string) - Line 1112
- **SQL_FROM** (string) - Line 1118
- **SQL_INSERT** (string) - Line 1106
- **SQL_SELECT** (string) - Line 1103
- **SQL_UPDATE** (string) - Line 1109
- **SQL_WHERE** (string) - Line 1115
- **STANDARD_DATETIME_FORMAT** (string) - Line 246
- **STANDARD_WAIT_SECONDS** (float) - Line 334
- **STANDARD_WAIT_TIME_SECONDS** (float) - Line 535
- **STARTING_BACKUP_CLEANUP_MSG** (string) - Line 864
- **STATUS_REPORT_INTERVAL_MINUTES** (int) - Line 1650
- **STRING_CONTAINS_METHOD** (string) - Line 475
- **STRING_FORMAT_FIVE_ARGS** (string) - Line 1536
- **STRING_FORMAT_FOUR_ARGS** (string) - Line 1533
- **STRING_FORMAT_METHOD** (string) - Line 466
- **STRING_FORMAT_ONE_ARG** (string) - Line 1524
- **STRING_FORMAT_THREE_ARGS** (string) - Line 1530
- **STRING_INDEX_OF_METHOD** (string) - Line 484
- **STRING_IS_NULL_OR_EMPTY_METHOD** (string) - Line 469
- **STRING_SPLIT_METHOD** (string) - Line 478
- **STRING_STARTS_WITH_METHOD** (string) - Line 472
- **STRING_SUBSTRING_METHOD** (string) - Line 481
- **SUCCESS_MSG_BACKUP_CREATED** (string) - Line 2279
- **SUCCESS_MSG_CLEANUP_COMPLETED** (string) - Line 2288
- **SUCCESS_MSG_CONNECTION_ESTABLISHED** (string) - Line 2270
- **SUCCESS_MSG_DATA_SYNCHRONIZED** (string) - Line 2285
- **SUCCESS_MSG_FILE_LOADED** (string) - Line 2267
- **SUCCESS_MSG_FILE_SAVED** (string) - Line 2264
- **SUCCESS_MSG_INITIALIZATION_COMPLETE** (string) - Line 2273
- **SUCCESS_MSG_OPERATION_COMPLETED** (string) - Line 2261
- **SUCCESS_MSG_SETTINGS_APPLIED** (string) - Line 2282
- **SUCCESS_MSG_VALIDATION_PASSED** (string) - Line 2276
- **SUCCESS_RESULT** (string) - Line 641
- **SYNC_CONTEXT_TIMEOUT_MS** (int) - Line 1062
- **SYSTEM_HEALTH_CHECK_INTERVAL_SECONDS** (int) - Line 294
- **SYSTEM_MONITORING_LOG_INTERVAL** (int) - Line 49
- **SYSTEM_MONITORING_THRESHOLD** (double) - Line 559
- **TAB** (string) - Line 612
- **TAKE_METHOD** (string) - Line 631
- **TARGET_FRAMEWORK_NET48** (string) - Line 1269
- **TARGET_PLATFORM_ANY_CPU** (string) - Line 1260
- **TARGET_PLATFORM_X64** (string) - Line 1266
- **TARGET_PLATFORM_X86** (string) - Line 1263
- **TASK_COMPLETION_TIMEOUT_MS** (int) - Line 324
- **TASK_SCHEDULER_TIMEOUT_MS** (int) - Line 1065
- **TEMP_DIRECTORY_NAME** (string) - Line 1030
- **TEN_FLOAT** (float) - Line 904
- **TEN_INT** (int) - Line 1338
- **TEN_INT** (int) - Line 937
- **TERRAIN_BASE_MAP_DISTANCE_MAX** (float) - Line 2079
- **TERRAIN_BASE_MAP_DISTANCE_MIN** (float) - Line 2076
- **TERRAIN_BILLBOARD_START_MAX** (float) - Line 2091
- **TERRAIN_BILLBOARD_START_MIN** (float) - Line 2088
- **TERRAIN_CONTROL_TEX_RES_MAX** (int) - Line 2073
- **TERRAIN_CONTROL_TEX_RES_MIN** (int) - Line 2070
- **TERRAIN_DETAIL_RES_MAX** (int) - Line 2067
- **TERRAIN_DETAIL_RES_MIN** (int) - Line 2064
- **TERRAIN_HEIGHT_MAP_RES_MAX** (int) - Line 2061
- **TERRAIN_HEIGHT_MAP_RES_MIN** (int) - Line 2058
- **TERRAIN_TREE_DISTANCE_MAX** (float) - Line 2085
- **TERRAIN_TREE_DISTANCE_MIN** (float) - Line 2082
- **TEXT_FILE_EXTENSION** (string) - Line 350
- **TEXTURE_ANISO_LEVEL_MAX** (int) - Line 2184
- **TEXTURE_ANISO_LEVEL_MIN** (int) - Line 2181
- **TEXTURE_COMPRESSION_QUALITY_MAX** (int) - Line 2178
- **TEXTURE_COMPRESSION_QUALITY_MIN** (int) - Line 2175
- **TEXTURE_FILTER_MODE_BILINEAR** (int) - Line 2190
- **TEXTURE_FILTER_MODE_POINT** (int) - Line 2187
- **TEXTURE_FILTER_MODE_TRILINEAR** (int) - Line 2193
- **TEXTURE_MIP_LEVEL_MAX** (int) - Line 2172
- **TEXTURE_SIZE_MAX** (int) - Line 2169
- **TEXTURE_SIZE_MIN** (int) - Line 2166
- **TEXTURE_WRAP_MODE_CLAMP** (int) - Line 2199
- **TEXTURE_WRAP_MODE_REPEAT** (int) - Line 2196
- **THIRTY_FLOAT** (float) - Line 910
- **THIRTY_INT** (int) - Line 943
- **THOUSAND_FLOAT** (float) - Line 919
- **THOUSAND_INT** (int) - Line 952
- **THREAD_CLEANUP_TIMEOUT_MS** (int) - Line 1492
- **THREAD_JOIN_TIMEOUT_MS** (int) - Line 1495
- **THREAD_LOCK_TIMEOUT_MS** (int) - Line 318
- **THREAD_MONITORING_INTERVAL_MS** (int) - Line 1489
- **THREAD_POOL_MAX_THREADS** (int) - Line 1071
- **THREAD_POOL_MIN_THREADS** (int) - Line 1068
- **THREAD_SLEEP_MAX_MS** (int) - Line 1059
- **THREAD_SLEEP_MIN_MS** (int) - Line 1056
- **THREE_FLOAT** (float) - Line 898
- **THREE_INT** (int) - Line 1332
- **THREE_INT** (int) - Line 931
- **THREE_QUARTERS_FLOAT** (float) - Line 1368
- **THRESHOLD_VALUE_KEY** (string) - Line 242
- **TIME_FORMAT_12_HOUR** (string) - Line 1197
- **TIME_FORMAT_24_HOUR** (string) - Line 1200
- **TIMEOUT_MS_PARAM** (string) - Line 220
- **TINY_WAIT_SECONDS** (float) - Line 959
- **TWENTY_FLOAT** (float) - Line 907
- **TWENTY_INT** (int) - Line 940
- **TWENTY_INT** (int) - Line 1341
- **TWO_DOUBLE** (double) - Line 1377
- **TWO_FLOAT** (float) - Line 1359
- **TWO_FLOAT** (float) - Line 895
- **TWO_INT** (int) - Line 1329
- **TWO_INT** (int) - Line 928
- **TWO_LONG** (long) - Line 1389
- **UI_ANIMATION_DURATION_MS** (int) - Line 1770
- **UI_ELEMENT_FADE_DURATION_MS** (int) - Line 1773
- **UI_ELEMENT_MIN_HEIGHT_PIXELS** (int) - Line 1788
- **UI_ELEMENT_MIN_WIDTH_PIXELS** (int) - Line 1785
- **UI_NOTIFICATION_DISPLAY_DURATION_SECONDS** (int) - Line 1779
- **UI_PANEL_DEFAULT_HEIGHT_PIXELS** (int) - Line 1794
- **UI_PANEL_DEFAULT_WIDTH_PIXELS** (int) - Line 1791
- **UI_POPUP_TIMEOUT_SECONDS** (int) - Line 1782
- **UI_TOOLTIP_DISPLAY_DELAY_MS** (int) - Line 1776
- **UI_UPDATE_INTERVAL_MS** (int) - Line 1767
- **ULTRA_LONG_WAIT_SECONDS** (float) - Line 977
- **ULTRA_SHORT_WAIT_SECONDS** (float) - Line 956
- **UNAVAILABLE_STATUS** (string) - Line 650
- **UNITY_DATA_PATH_COMMENT** (string) - Line 872
- **UNITY_ENGINE_ASSEMBLY** (string) - Line 357
- **UNITY_INPUT_HORIZONTAL** (string) - Line 1222
- **UNITY_INPUT_MOUSE_X** (string) - Line 1228
- **UNITY_INPUT_MOUSE_Y** (string) - Line 1231
- **UNITY_INPUT_VERTICAL** (string) - Line 1225
- **UNITY_LAYER_DEFAULT** (string) - Line 1216
- **UNITY_LAYER_UI** (string) - Line 1219
- **UNITY_TAG_ENEMY** (string) - Line 1207
- **UNITY_TAG_MAIN_CAMERA** (string) - Line 1210
- **UNITY_TAG_PLAYER** (string) - Line 1204
- **UNITY_TAG_UI** (string) - Line 1213
- **URL_REGEX_PATTERN** (string) - Line 1147
- **USER_PREFERENCES_FILENAME** (string) - Line 1015
- **UTC_DATETIME_FORMAT_WITH_MS** (string) - Line 249
- **UTILS_PREFIX** (string) - Line 144
- **VERSION_KEY** (string) - Line 236
- **VERSION_REGEX_PATTERN** (string) - Line 1150
- **VERY_LONG_WAIT_SECONDS** (float) - Line 974
- **WARNING_MSG_BACKUP_RECOMMENDED** (string) - Line 2307
- **WARNING_MSG_COMPATIBILITY_ISSUE** (string) - Line 2310
- **WARNING_MSG_CONFIGURATION_MISMATCH** (string) - Line 2313
- **WARNING_MSG_DEPRECATED_FEATURE** (string) - Line 2301
- **WARNING_MSG_DISK_SPACE_LOW** (string) - Line 2298
- **WARNING_MSG_MEMORY_USAGE_HIGH** (string) - Line 2295
- **WARNING_MSG_PERFORMANCE_DEGRADED** (string) - Line 2292
- **WARNING_MSG_RESOURCE_LIMITATION** (string) - Line 2316
- **WARNING_MSG_TEMPORARY_FAILURE** (string) - Line 2319
- **WARNING_MSG_UNSTABLE_OPERATION** (string) - Line 2304
- **WARNING_OPERATION_PREFIX** (string) - Line 666
- **WATCHDOG_TIMEOUT_SECONDS** (int) - Line 1653
- **ZERO_FLOAT** (float) - Line 889
- **ZERO_FLOAT** (float) - Line 1353
- **ZERO_LONG** (long) - Line 1383

### Constants_Original.cs

- **ATTACH_TIMEOUT_SECONDS** (float) - Line 57
- **BACKUP_INTERVAL_MINUTES** (int) - Line 66
- **BYTES_TO_KB** (int) - Line 105
- **BYTES_TO_MB** (double) - Line 108
- **COMMAND_RECOGNIZED** (string) - Line 233
- **COMMAND_UNKNOWN** (string) - Line 236
- **CONSOLE_COMMAND_DELAY_MS** (int) - Line 42
- **CONSOLE_HOOK_GAMEOBJECT_NAME** (string) - Line 271
- **CONSOLE_UTC_DATETIME_FORMAT** (string) - Line 284
- **DEFAULT_OPERATION_DELAY** (float) - Line 117
- **DETECTION_DATETIME_FORMAT** (string) - Line 287
- **EMERGENCY_SAVE_FILENAME** (string) - Line 220
- **EMERGENCY_SAVE_REASON** (string) - Line 229
- **FILE_COPY_OPERATION_NAME** (string) - Line 223
- **GAME_SAVE_MAX_ITERATIONS_WARNING** (int) - Line 70
- **GAME_SAVE_MIN_DELAY_SECONDS** (float) - Line 76
- **HARMONY_MOD_ID** (string) - Line 268
- **INVALID_ITERATION_COUNT_ERROR** (string) - Line 130
- **INVALID_OPERATION_DELAY_ERROR** (string) - Line 127
- **INVALID_PARAMETER_ERROR** (string) - Line 133
- **JSON_FILE_EXTENSION** (string) - Line 211
- **LOAD_TIMEOUT_SECONDS** (float) - Line 54
- **LOG_FILE_DATETIME_FORMAT** (string) - Line 290
- **LOG_LEVEL_IMPORTANT** (int) - Line 26
- **LOG_PREFIX_BRIDGE** (string) - Line 158
- **LOG_PREFIX_CONSOLE** (string) - Line 155
- **LOG_PREFIX_CRITICAL** (string) - Line 152
- **LOG_PREFIX_DIR_RESOLVER** (string) - Line 167
- **LOG_PREFIX_ERROR** (string) - Line 149
- **LOG_PREFIX_GAME** (string) - Line 191
- **LOG_PREFIX_GAME_ERROR** (string) - Line 185
- **LOG_PREFIX_GAME_WARNING** (string) - Line 188
- **LOG_PREFIX_INFO** (string) - Line 143
- **LOG_PREFIX_INIT** (string) - Line 170
- **LOG_PREFIX_MANUAL** (string) - Line 182
- **LOG_PREFIX_MONITOR** (string) - Line 176
- **LOG_PREFIX_PROFILE** (string) - Line 173
- **LOG_PREFIX_SAVE** (string) - Line 161
- **LOG_PREFIX_SYSMON** (string) - Line 164
- **LOG_PREFIX_TRANSACTION** (string) - Line 179
- **LOG_PREFIX_WARN** (string) - Line 146
- **MAX_DELAY_WARNING_SECONDS** (float) - Line 79
- **MIXER_CONFIG_ENABLED_DEFAULT** (bool) - Line 98
- **MIXER_PREF_MAX_ITERATIONS_WARNING** (int) - Line 73
- **MIXER_PREF_PROGRESS_INTERVAL** (int) - Line 88
- **MIXER_SAVE_FILENAME** (string) - Line 217
- **MIXER_THRESHOLD_MAX** (float) - Line 95
- **MIXER_THRESHOLD_MIN** (float) - Line 92
- **MIXER_VALUE_TOLERANCE** (float) - Line 101
- **MOD_NAME** (string) - Line 277
- **MOD_VERSION** (string) - Line 274
- **MS_PER_SECOND** (int) - Line 111
- **NO_MIXER_DATA_ERROR** (string) - Line 139
- **NO_SAVE_PATH_ERROR** (string) - Line 136
- **NOT_AVAILABLE_PATH_FALLBACK** (string) - Line 207
- **NOT_SET_PATH_FALLBACK** (string) - Line 204
- **NULL_COMMAND_FALLBACK** (string) - Line 201
- **NULL_ERROR_FALLBACK** (string) - Line 198
- **OPERATION_TIMEOUT_MS** (int) - Line 39
- **PARAM_TYPE_BOOLEAN** (string) - Line 248
- **PARAM_TYPE_FLOAT** (string) - Line 245
- **PARAM_TYPE_INTEGER** (string) - Line 242
- **PARAM_TYPE_STRING** (string) - Line 239
- **PERFORMANCE_FAST** (string) - Line 258
- **PERFORMANCE_MODERATE** (string) - Line 261
- **PERFORMANCE_SLOW** (string) - Line 264
- **PERFORMANCE_SLOW_THRESHOLD_MS** (int) - Line 48
- **PERFORMANCE_WARNING_THRESHOLD_MS** (int) - Line 45
- **POLL_INTERVAL_SECONDS** (float) - Line 60
- **SAVE_COOLDOWN_SECONDS** (int) - Line 63
- **SAVE_DATA_VERSION** (string) - Line 226
- **SAVE_DATETIME_FORMAT** (string) - Line 281
- **SAVE_PERFORMANCE_WARNING_SECONDS** (float) - Line 82
- **SECONDS_PER_MINUTE** (int) - Line 114
- **STATUS_FAILED** (string) - Line 255
- **STATUS_SUCCESS** (string) - Line 252
- **STRESS_TEST_PROGRESS_INTERVAL** (int) - Line 85
- **SYSTEM_MONITORING_LOG_INTERVAL** (int) - Line 51

### ErrorConstants.cs

- **ERROR_CODE_ACCESS_DENIED** (int) - Line 24
- **ERROR_CODE_CONFIGURATION** (int) - Line 51
- **ERROR_CODE_DATA_CORRUPTION** (int) - Line 57
- **ERROR_CODE_FILE_NOT_FOUND** (int) - Line 21
- **ERROR_CODE_GENERAL** (int) - Line 18
- **ERROR_CODE_INVALID_ARGUMENT** (int) - Line 36
- **ERROR_CODE_INVALID_OPERATION** (int) - Line 27
- **ERROR_CODE_NETWORK** (int) - Line 42
- **ERROR_CODE_OPERATION_CANCELLED** (int) - Line 39
- **ERROR_CODE_OUT_OF_MEMORY** (int) - Line 33
- **ERROR_CODE_PERMISSION** (int) - Line 54
- **ERROR_CODE_SERIALIZATION** (int) - Line 45
- **ERROR_CODE_SUCCESS** (int) - Line 15
- **ERROR_CODE_TIMEOUT** (int) - Line 30
- **ERROR_CODE_VALIDATION** (int) - Line 48
- **ERROR_CODE_VERSION_MISMATCH** (int) - Line 60
- **ERROR_CONTEXT_BACKUP** (string) - Line 199
- **ERROR_CONTEXT_CONFIGURATION** (string) - Line 205
- **ERROR_CONTEXT_INITIALIZATION** (string) - Line 202
- **ERROR_CONTEXT_LOAD** (string) - Line 193
- **ERROR_CONTEXT_NETWORK** (string) - Line 208
- **ERROR_CONTEXT_PERFORMANCE** (string) - Line 214
- **ERROR_CONTEXT_SAVE** (string) - Line 190
- **ERROR_CONTEXT_SECURITY** (string) - Line 217
- **ERROR_CONTEXT_THREADING** (string) - Line 211
- **ERROR_CONTEXT_VALIDATION** (string) - Line 196
- **ERROR_MESSAGE_CANCELLATION** (string) - Line 92
- **ERROR_MESSAGE_CONFIGURATION** (string) - Line 80
- **ERROR_MESSAGE_DATA_CORRUPTION** (string) - Line 98
- **ERROR_MESSAGE_FILE_OPERATION** (string) - Line 68
- **ERROR_MESSAGE_GENERIC** (string) - Line 65
- **ERROR_MESSAGE_LOAD_OPERATION** (string) - Line 74
- **ERROR_MESSAGE_MEMORY** (string) - Line 86
- **ERROR_MESSAGE_NETWORK** (string) - Line 83
- **ERROR_MESSAGE_NOT_SUPPORTED** (string) - Line 107
- **ERROR_MESSAGE_PERMISSION** (string) - Line 95
- **ERROR_MESSAGE_RESOURCE_UNAVAILABLE** (string) - Line 104
- **ERROR_MESSAGE_SAVE_OPERATION** (string) - Line 71
- **ERROR_MESSAGE_TIMEOUT** (string) - Line 89
- **ERROR_MESSAGE_VALIDATION** (string) - Line 77
- **ERROR_MESSAGE_VERSION_MISMATCH** (string) - Line 101
- **ERROR_SEVERITY_CRITICAL** (string) - Line 170
- **ERROR_SEVERITY_HIGH** (string) - Line 173
- **ERROR_SEVERITY_INFO** (string) - Line 185
- **ERROR_SEVERITY_LOW** (string) - Line 179
- **ERROR_SEVERITY_MEDIUM** (string) - Line 176
- **ERROR_SEVERITY_WARNING** (string) - Line 182
- **EXCEPTION_TYPE_ARGUMENT** (string) - Line 115
- **EXCEPTION_TYPE_FILE_NOT_FOUND** (string) - Line 124
- **EXCEPTION_TYPE_INVALID_OPERATION** (string) - Line 121
- **EXCEPTION_TYPE_NOT_SUPPORTED** (string) - Line 139
- **EXCEPTION_TYPE_NULL_REFERENCE** (string) - Line 118
- **EXCEPTION_TYPE_OPERATION_CANCELLED** (string) - Line 136
- **EXCEPTION_TYPE_OUT_OF_MEMORY** (string) - Line 133
- **EXCEPTION_TYPE_SYSTEM** (string) - Line 112
- **EXCEPTION_TYPE_TIMEOUT** (string) - Line 130
- **EXCEPTION_TYPE_UNAUTHORIZED_ACCESS** (string) - Line 127
- **RECOVERY_STRATEGY_ABORT** (string) - Line 153
- **RECOVERY_STRATEGY_BACKUP_RESTORE** (string) - Line 162
- **RECOVERY_STRATEGY_DEFAULT** (string) - Line 156
- **RECOVERY_STRATEGY_EMERGENCY_SAVE** (string) - Line 159
- **RECOVERY_STRATEGY_FALLBACK** (string) - Line 147
- **RECOVERY_STRATEGY_RESET_DEFAULTS** (string) - Line 165
- **RECOVERY_STRATEGY_RETRY** (string) - Line 144
- **RECOVERY_STRATEGY_SKIP** (string) - Line 150

### FileConstants.cs

- **ALL_FILES_PATTERN** (string) - Line 144
- **ALT_PATH_SEPARATOR_CHAR** (char) - Line 202
- **AVI_EXTENSION** (string) - Line 75
- **BACKUP_DIRECTORY** (string) - Line 80
- **BACKUP_EXTENSION** (string) - Line 18
- **BACKUP_FILES_PATTERN** (string) - Line 153
- **BIN_EXTENSION** (string) - Line 42
- **CACHE_DIRECTORY** (string) - Line 95
- **CONFIG_DIRECTORY** (string) - Line 86
- **CONFIG_EXTENSION** (string) - Line 27
- **CONFIG_FILES_PATTERN** (string) - Line 159
- **CURRENT_DIRECTORY** (string) - Line 211
- **DATA_DIRECTORY** (string) - Line 89
- **DATA_EXTENSION** (string) - Line 33
- **DIRECTORY_SEPARATOR** (string) - Line 208
- **DLL_EXTENSION** (string) - Line 45
- **DLL_FILES_PATTERN** (string) - Line 162
- **EXE_EXTENSION** (string) - Line 48
- **EXE_FILES_PATTERN** (string) - Line 165
- **FILE_ACCESS_READ** (string) - Line 219
- **FILE_ACCESS_READ_WRITE** (string) - Line 225
- **FILE_ACCESS_WRITE** (string) - Line 222
- **FILE_BUFFER_SIZE** (int) - Line 185
- **FILE_MODE_APPEND** (string) - Line 243
- **FILE_MODE_CREATE** (string) - Line 228
- **FILE_MODE_CREATE_NEW** (string) - Line 231
- **FILE_MODE_OPEN** (string) - Line 234
- **FILE_MODE_OPEN_OR_CREATE** (string) - Line 237
- **FILE_MODE_TRUNCATE** (string) - Line 240
- **FILE_OP_APPEND** (string) - Line 118
- **FILE_OP_BACKUP** (string) - Line 133
- **FILE_OP_COPY** (string) - Line 124
- **FILE_OP_CREATE** (string) - Line 130
- **FILE_OP_DELETE** (string) - Line 121
- **FILE_OP_MOVE** (string) - Line 127
- **FILE_OP_READ** (string) - Line 112
- **FILE_OP_RESTORE** (string) - Line 136
- **FILE_OP_VALIDATE** (string) - Line 139
- **FILE_OP_WRITE** (string) - Line 115
- **JPEG_EXTENSION** (string) - Line 63
- **JPG_EXTENSION** (string) - Line 60
- **JSON_EXTENSION** (string) - Line 15
- **JSON_FILES_PATTERN** (string) - Line 147
- **LARGE_FILE_BUFFER_SIZE** (int) - Line 188
- **LOG_EXTENSION** (string) - Line 24
- **LOG_FILES_PATTERN** (string) - Line 150
- **LOGS_DIRECTORY** (string) - Line 83
- **MAX_FILE_SIZE_BYTES** (long) - Line 170
- **MAX_FILENAME_LENGTH** (int) - Line 176
- **MAX_PATH_LENGTH** (int) - Line 179
- **MIN_FILE_SIZE_BYTES** (long) - Line 173
- **MIN_FILENAME_LENGTH** (int) - Line 182
- **MODS_DIRECTORY** (string) - Line 101
- **MP3_EXTENSION** (string) - Line 69
- **MP4_EXTENSION** (string) - Line 72
- **PARENT_DIRECTORY** (string) - Line 214
- **PATH_SEPARATOR_CHAR** (char) - Line 199
- **PLUGINS_DIRECTORY** (string) - Line 98
- **PNG_EXTENSION** (string) - Line 57
- **PRESETS_DIRECTORY** (string) - Line 107
- **RAR_EXTENSION** (string) - Line 54
- **SETTINGS_DIRECTORY** (string) - Line 104
- **SETTINGS_EXTENSION** (string) - Line 30
- **TEMP_DIRECTORY** (string) - Line 92
- **TEMP_EXTENSION** (string) - Line 21
- **TEMP_FILES_PATTERN** (string) - Line 156
- **TXT_EXTENSION** (string) - Line 39
- **UNIX_PATH_SEPARATOR** (string) - Line 196
- **VOLUME_SEPARATOR_CHAR** (char) - Line 205
- **WAV_EXTENSION** (string) - Line 66
- **WINDOWS_PATH_SEPARATOR** (string) - Line 193
- **XML_EXTENSION** (string) - Line 36
- **ZIP_EXTENSION** (string) - Line 51

### GameConstants.cs

- **ANIMATION_BLEND_TIME** (float) - Line 221
- **ANIMATION_FADE_IN_TIME** (float) - Line 224
- **ANIMATION_FADE_OUT_TIME** (float) - Line 227
- **ANIMATION_LOOP_MODE** (string) - Line 230
- **ANIMATION_PLAY_MODE** (string) - Line 233
- **ANIMATION_SPEED_DEFAULT** (float) - Line 212
- **ANIMATION_SPEED_FAST** (float) - Line 215
- **ANIMATION_SPEED_SLOW** (float) - Line 218
- **AUDIO_BIT_DEPTH** (int) - Line 103
- **AUDIO_BUFFER_SIZE** (int) - Line 97
- **AUDIO_CHANNELS** (int) - Line 100
- **AUDIO_DEFAULT_VOLUME** (float) - Line 82
- **AUDIO_FADE_TIME** (float) - Line 91
- **AUDIO_MASTER_VOLUME** (float) - Line 79
- **AUDIO_MAX_VOLUME** (float) - Line 88
- **AUDIO_MIN_VOLUME** (float) - Line 85
- **AUDIO_SAMPLE_RATE** (int) - Line 94
- **GAME_STATE_CREDITS** (string) - Line 207
- **GAME_STATE_GAME_OVER** (string) - Line 201
- **GAME_STATE_LOADING** (string) - Line 192
- **GAME_STATE_MAIN_MENU** (string) - Line 189
- **GAME_STATE_PAUSED** (string) - Line 198
- **GAME_STATE_PLAYING** (string) - Line 195
- **GAME_STATE_SETTINGS** (string) - Line 204
- **GAMEPLAY_DEFAULT_HEALTH** (float) - Line 250
- **GAMEPLAY_DEFAULT_JUMP_FORCE** (float) - Line 256
- **GAMEPLAY_DEFAULT_SPEED** (float) - Line 253
- **GAMEPLAY_INVINCIBILITY_TIME** (float) - Line 262
- **GAMEPLAY_MAX_LEVEL** (int) - Line 241
- **GAMEPLAY_PLAYER_LIVES** (int) - Line 238
- **GAMEPLAY_RESPAWN_TIME** (float) - Line 259
- **GAMEPLAY_START_LEVEL** (int) - Line 244
- **GAMEPLAY_XP_PER_LEVEL** (int) - Line 247
- **GRAPHICS_AA_SAMPLES** (int) - Line 123
- **GRAPHICS_ANISO_FILTERING** (int) - Line 126
- **GRAPHICS_MIN_FPS** (int) - Line 117
- **GRAPHICS_RENDER_DISTANCE** (float) - Line 135
- **GRAPHICS_SCREEN_HEIGHT** (int) - Line 111
- **GRAPHICS_SCREEN_WIDTH** (int) - Line 108
- **GRAPHICS_SHADOW_QUALITY** (string) - Line 129
- **GRAPHICS_TARGET_FPS** (int) - Line 114
- **GRAPHICS_TEXTURE_QUALITY** (string) - Line 132
- **GRAPHICS_VSYNC_ENABLED** (bool) - Line 120
- **INPUT_BUFFER_SIZE** (int) - Line 181
- **INPUT_CONTROLLER_DEADZONE** (float) - Line 172
- **INPUT_DOUBLE_CLICK_TIME_MS** (int) - Line 175
- **INPUT_LONG_PRESS_TIME_MS** (int) - Line 178
- **INPUT_MAX_LAG_COMPENSATION_MS** (int) - Line 184
- **INPUT_MOUSE_SENSITIVITY** (float) - Line 169
- **PHYSICS_COLLISION_DETECTION** (string) - Line 158
- **PHYSICS_DEFAULT_BOUNCE** (float) - Line 152
- **PHYSICS_DEFAULT_FRICTION** (float) - Line 149
- **PHYSICS_FIXED_TIMESTEP** (float) - Line 143
- **PHYSICS_GRAVITY** (float) - Line 140
- **PHYSICS_MAX_TIMESTEP** (float) - Line 146
- **PHYSICS_SLEEP_THRESHOLD** (float) - Line 155
- **PHYSICS_SOLVER_ITERATIONS** (int) - Line 161
- **PHYSICS_VELOCITY_ITERATIONS** (int) - Line 164
- **UI_BUTTON_HEIGHT** (int) - Line 27
- **UI_BUTTON_WIDTH** (int) - Line 24
- **UI_COLOR_BACKGROUND** (string) - Line 68
- **UI_COLOR_DISABLED** (string) - Line 74
- **UI_COLOR_ERROR** (string) - Line 65
- **UI_COLOR_PRIMARY** (string) - Line 53
- **UI_COLOR_SECONDARY** (string) - Line 56
- **UI_COLOR_SUCCESS** (string) - Line 59
- **UI_COLOR_TEXT** (string) - Line 71
- **UI_COLOR_WARNING** (string) - Line 62
- **UI_ELEMENT_SPACING** (int) - Line 33
- **UI_FADE_DURATION_MS** (int) - Line 48
- **UI_FONT_SIZE_DEFAULT** (int) - Line 15
- **UI_FONT_SIZE_LARGE** (int) - Line 18
- **UI_FONT_SIZE_SMALL** (int) - Line 21
- **UI_PANEL_PADDING** (int) - Line 30
- **UI_SCROLLBAR_WIDTH** (int) - Line 42
- **UI_TOOLTIP_DELAY_MS** (int) - Line 45
- **UI_WINDOW_MIN_HEIGHT** (int) - Line 39
- **UI_WINDOW_MIN_WIDTH** (int) - Line 36

### LoggingConstants.cs

- **ATOMIC_FILE_WRITER_PREFIX** (string) - Line 47
- **BACKUP_CREATED_MESSAGE** (string) - Line 115
- **BACKUP_LOG_FILENAME** (string) - Line 153
- **BACKUP_MANAGER_PREFIX** (string) - Line 50
- **BACKUP_RESTORED_MESSAGE** (string) - Line 118
- **CLEANUP_PREFIX** (string) - Line 101
- **CONSOLE_COMMAND_PREFIX** (string) - Line 56
- **DATA_INTEGRITY_PREFIX** (string) - Line 68
- **DEBUG_LOG_FILENAME** (string) - Line 150
- **DEBUG_PREFIX** (string) - Line 86
- **ERROR_LOG_FILENAME** (string) - Line 144
- **ERROR_PREFIX** (string) - Line 77
- **GENERAL_PREFIX** (string) - Line 71
- **INFO_PREFIX** (string) - Line 83
- **INIT_PREFIX** (string) - Line 98
- **IO_RUNNER_PREFIX** (string) - Line 59
- **LOG_LEVEL_CRITICAL_STRING** (string) - Line 158
- **LOG_LEVEL_DEBUG_STRING** (string) - Line 170
- **LOG_LEVEL_ERROR_STRING** (string) - Line 161
- **LOG_LEVEL_IMPORTANT** (int) - Line 18
- **LOG_LEVEL_INFO_STRING** (string) - Line 167
- **LOG_LEVEL_TRACE_STRING** (string) - Line 173
- **LOG_LEVEL_WARNING_STRING** (string) - Line 164
- **LOGGER_PREFIX** (string) - Line 41
- **MAIN_LOG_FILENAME** (string) - Line 141
- **MEMORY_PREFIX** (string) - Line 95
- **MIXER_VALIDATION_PREFIX** (string) - Line 44
- **OPERATION_CANCELLED_MESSAGE** (string) - Line 136
- **OPERATION_COMPLETE_MESSAGE** (string) - Line 130
- **OPERATION_START_MESSAGE** (string) - Line 127
- **OPERATION_TIMEOUT_MESSAGE** (string) - Line 133
- **PERF_PREFIX** (string) - Line 92
- **PERFORMANCE_LOG_FILENAME** (string) - Line 147
- **PERFORMANCE_OPTIMIZER_PREFIX** (string) - Line 62
- **SAVE_FAILURE_MESSAGE** (string) - Line 112
- **SAVE_MANAGER_PREFIX** (string) - Line 32
- **SAVE_PATCH_PREFIX** (string) - Line 65
- **SAVE_SUCCESS_MESSAGE** (string) - Line 109
- **SYSTEM_PREFIX** (string) - Line 74
- **TRACE_PREFIX** (string) - Line 89
- **VALIDATION_FAILURE_MESSAGE** (string) - Line 124
- **VALIDATION_PREFIX** (string) - Line 104
- **VALIDATION_SUCCESS_MESSAGE** (string) - Line 121
- **WARNING_PREFIX** (string) - Line 80

### MixerConstants.cs

- **DEFAULT_MIXER_CHANNEL** (int) - Line 18
- **DEFAULT_MIXER_GAIN** (float) - Line 36
- **DEFAULT_MIXER_PRESET_NAME** (string) - Line 136
- **DEFAULT_MIXER_VOLUME** (float) - Line 27
- **FACTORY_MIXER_PRESET_NAME** (string) - Line 139
- **GAIN_PRECISION_DECIMALS** (int) - Line 125
- **MAX_MIXER_CHANNELS** (int) - Line 116
- **MAX_MIXER_NAME_LENGTH** (int) - Line 128
- **MAX_MIXER_PRESETS** (int) - Line 145
- **MAX_MIXER_VOLUME** (float) - Line 21
- **MIN_MIXER_CHANNELS** (int) - Line 119
- **MIN_MIXER_NAME_LENGTH** (int) - Line 131
- **MIN_MIXER_VOLUME** (float) - Line 24
- **MIXER_BACKUP_FILENAME** (string) - Line 102
- **MIXER_CHANNEL_COUNT** (int) - Line 15
- **MIXER_CHANNEL_KEY_PREFIX** (string) - Line 44
- **MIXER_CHANNEL_MUTED_MESSAGE** (string) - Line 76
- **MIXER_CHANNEL_UNMUTED_MESSAGE** (string) - Line 79
- **MIXER_CONFIG_FILENAME** (string) - Line 105
- **MIXER_CONFIG_KEY** (string) - Line 59
- **MIXER_CONFIG_UPDATE_MESSAGE** (string) - Line 70
- **MIXER_GAIN_KEY** (string) - Line 50
- **MIXER_GAIN_RANGE** (float) - Line 33
- **MIXER_INIT_MESSAGE** (string) - Line 67
- **MIXER_MUTE_KEY** (string) - Line 53
- **MIXER_PRESET_KEY** (string) - Line 62
- **MIXER_PRESET_LOADED_MESSAGE** (string) - Line 82
- **MIXER_PRESET_SAVED_MESSAGE** (string) - Line 85
- **MIXER_PRESET_VERSION** (string) - Line 148
- **MIXER_PRESETS_FILENAME** (string) - Line 108
- **MIXER_RESET_MESSAGE** (string) - Line 88
- **MIXER_SAVE_FILENAME** (string) - Line 99
- **MIXER_SETTINGS_FILENAME** (string) - Line 111
- **MIXER_SOLO_KEY** (string) - Line 56
- **MIXER_VALIDATION_FAILURE_MESSAGE** (string) - Line 94
- **MIXER_VALIDATION_SUCCESS_MESSAGE** (string) - Line 91
- **MIXER_VALUE_CHANGED_MESSAGE** (string) - Line 73
- **MIXER_VALUES_KEY** (string) - Line 41
- **MIXER_VOLUME_KEY** (string) - Line 47
- **MIXER_VOLUME_STEP** (float) - Line 30
- **USER_MIXER_PRESET_PREFIX** (string) - Line 142
- **VOLUME_PRECISION_DECIMALS** (int) - Line 122

### NetworkConstants.cs

- **API_KEY_HEADER** (string) - Line 248
- **BASIC_AUTH_PREFIX** (string) - Line 254
- **BEARER_TOKEN_PREFIX** (string) - Line 251
- **CONTENT_TYPE_BINARY** (string) - Line 132
- **CONTENT_TYPE_FORM_URLENCODED** (string) - Line 126
- **CONTENT_TYPE_HTML** (string) - Line 123
- **CONTENT_TYPE_IMAGE_JPEG** (string) - Line 141
- **CONTENT_TYPE_IMAGE_PNG** (string) - Line 138
- **CONTENT_TYPE_JSON** (string) - Line 114
- **CONTENT_TYPE_MULTIPART_FORM** (string) - Line 129
- **CONTENT_TYPE_PDF** (string) - Line 135
- **CONTENT_TYPE_TEXT** (string) - Line 120
- **CONTENT_TYPE_XML** (string) - Line 117
- **CSRF_TOKEN_HEADER** (string) - Line 266
- **ENCODING_ASCII** (string) - Line 219
- **ENCODING_BASE64** (string) - Line 222
- **ENCODING_HTML** (string) - Line 228
- **ENCODING_JSON** (string) - Line 231
- **ENCODING_URL** (string) - Line 225
- **ENCODING_UTF16** (string) - Line 216
- **ENCODING_UTF8** (string) - Line 213
- **ENCODING_XML** (string) - Line 234
- **HTTP_DEFAULT_TIMEOUT_MS** (int) - Line 36
- **HTTP_HEADER_ACCEPT** (string) - Line 85
- **HTTP_HEADER_ACCEPT_ENCODING** (string) - Line 97
- **HTTP_HEADER_AUTHORIZATION** (string) - Line 88
- **HTTP_HEADER_CACHE_CONTROL** (string) - Line 100
- **HTTP_HEADER_CONNECTION** (string) - Line 103
- **HTTP_HEADER_CONTENT_LENGTH** (string) - Line 94
- **HTTP_HEADER_CONTENT_TYPE** (string) - Line 82
- **HTTP_HEADER_COOKIE** (string) - Line 106
- **HTTP_HEADER_SET_COOKIE** (string) - Line 109
- **HTTP_HEADER_USER_AGENT** (string) - Line 91
- **HTTP_METHOD_DELETE** (string) - Line 24
- **HTTP_METHOD_GET** (string) - Line 15
- **HTTP_METHOD_HEAD** (string) - Line 30
- **HTTP_METHOD_OPTIONS** (string) - Line 33
- **HTTP_METHOD_PATCH** (string) - Line 27
- **HTTP_METHOD_POST** (string) - Line 18
- **HTTP_METHOD_PUT** (string) - Line 21
- **HTTP_RETRY_ATTEMPTS** (int) - Line 39
- **HTTP_RETRY_DELAY_MS** (int) - Line 42
- **HTTP_STATUS_BAD_GATEWAY** (int) - Line 71
- **HTTP_STATUS_BAD_REQUEST** (int) - Line 56
- **HTTP_STATUS_CREATED** (int) - Line 50
- **HTTP_STATUS_FORBIDDEN** (int) - Line 62
- **HTTP_STATUS_GATEWAY_TIMEOUT** (int) - Line 77
- **HTTP_STATUS_INTERNAL_SERVER_ERROR** (int) - Line 68
- **HTTP_STATUS_NO_CONTENT** (int) - Line 53
- **HTTP_STATUS_NOT_FOUND** (int) - Line 65
- **HTTP_STATUS_OK** (int) - Line 47
- **HTTP_STATUS_SERVICE_UNAVAILABLE** (int) - Line 74
- **HTTP_STATUS_UNAUTHORIZED** (int) - Line 59
- **JWT_TOKEN_TYPE** (string) - Line 257
- **MAX_TCP_CONNECTIONS** (int) - Line 164
- **NETWORK_AUTH_FAILED** (string) - Line 280
- **NETWORK_CLIENT_ERROR** (string) - Line 286
- **NETWORK_CONNECTION_FAILED** (string) - Line 271
- **NETWORK_DNS_FAILED** (string) - Line 292
- **NETWORK_INVALID_RESPONSE** (string) - Line 277
- **NETWORK_PROTOCOL_ERROR** (string) - Line 295
- **NETWORK_RATE_LIMITED** (string) - Line 298
- **NETWORK_REQUEST_TIMEOUT** (string) - Line 274
- **NETWORK_SERVER_ERROR** (string) - Line 283
- **NETWORK_SSL_ERROR** (string) - Line 289
- **OAUTH_TOKEN_TYPE** (string) - Line 260
- **PROTOCOL_FTP** (string) - Line 196
- **PROTOCOL_HTTP** (string) - Line 181
- **PROTOCOL_HTTPS** (string) - Line 184
- **PROTOCOL_IMAP** (string) - Line 202
- **PROTOCOL_POP3** (string) - Line 205
- **PROTOCOL_SMTP** (string) - Line 199
- **PROTOCOL_SSH** (string) - Line 208
- **PROTOCOL_TCP** (string) - Line 187
- **PROTOCOL_UDP** (string) - Line 190
- **PROTOCOL_WEBSOCKET** (string) - Line 193
- **SESSION_COOKIE_NAME** (string) - Line 263
- **SOCKET_KEEP_ALIVE_ENABLED** (bool) - Line 173
- **SOCKET_NO_DELAY_ENABLED** (bool) - Line 176
- **SSL_CERT_VALIDATION_ENABLED** (bool) - Line 245
- **TCP_BUFFER_SIZE** (int) - Line 167
- **TCP_CONNECTION_TIMEOUT_MS** (int) - Line 152
- **TCP_DEFAULT_PORT** (int) - Line 146
- **TCP_READ_TIMEOUT_MS** (int) - Line 155
- **TCP_WRITE_TIMEOUT_MS** (int) - Line 158
- **TLS_VERSION_12** (string) - Line 239
- **TLS_VERSION_13** (string) - Line 242
- **UDP_BUFFER_SIZE** (int) - Line 170
- **UDP_DEFAULT_PORT** (int) - Line 149
- **UDP_SOCKET_TIMEOUT_MS** (int) - Line 161

### PerformanceConstants.cs

- **ASYNC_QUEUE_LIMIT** (int) - Line 130
- **BACKGROUND_TASK_INTERVAL_MS** (int) - Line 99
- **BACKUP_OPERATION_TIMEOUT_MS** (int) - Line 27
- **BATCH_OPERATION_SIZE** (int) - Line 133
- **BUFFER_SIZE_LARGE** (int) - Line 107
- **BUFFER_SIZE_SMALL** (int) - Line 110
- **BUFFER_SIZE_STANDARD** (int) - Line 104
- **CACHE_EXPIRY_MS** (int) - Line 116
- **CONSOLE_COMMAND_DELAY_MS** (int) - Line 18
- **CPU_CRITICAL_THRESHOLD_PERCENT** (int) - Line 59
- **CPU_MONITORING_INTERVAL_MS** (int) - Line 73
- **CPU_WARNING_THRESHOLD_PERCENT** (int) - Line 56
- **DATABASE_TIMEOUT_MS** (int) - Line 36
- **EMERGENCY_SAVE_TIMEOUT_MS** (int) - Line 30
- **FILE_OPERATION_TIMEOUT_MS** (int) - Line 21
- **GC_COLLECTION_THRESHOLD_BYTES** (long) - Line 121
- **IO_MONITORING_INTERVAL_MS** (int) - Line 76
- **LONG_WAIT_MS** (int) - Line 90
- **MAX_CACHE_SIZE** (int) - Line 113
- **MEDIUM_WAIT_MS** (int) - Line 87
- **MEMORY_CRITICAL_THRESHOLD_BYTES** (long) - Line 53
- **MEMORY_MONITORING_INTERVAL_MS** (int) - Line 70
- **MEMORY_WARNING_THRESHOLD_BYTES** (long) - Line 50
- **NETWORK_TIMEOUT_MS** (int) - Line 33
- **OPERATION_TIMEOUT_MS** (int) - Line 15
- **PERFORMANCE_CRITICAL_THRESHOLD_MS** (int) - Line 47
- **PERFORMANCE_MONITORING_INTERVAL_MS** (int) - Line 67
- **PERFORMANCE_SLOW_THRESHOLD_MS** (int) - Line 44
- **PERFORMANCE_WARNING_THRESHOLD_MS** (int) - Line 41
- **POLLING_INTERVAL_MS** (int) - Line 96
- **RETRY_DELAY_MS** (int) - Line 81
- **SAVE_OPERATION_TIMEOUT_MS** (int) - Line 24
- **SHORT_WAIT_MS** (int) - Line 84
- **SYSTEM_MONITORING_LOG_INTERVAL** (int) - Line 64
- **THREAD_POOL_MAX_THREADS** (int) - Line 127
- **THREAD_POOL_MIN_THREADS** (int) - Line 124
- **VERY_LONG_WAIT_MS** (int) - Line 93

### SystemConstants.cs

- **API_VERSION** (string) - Line 93
- **APPDATA_ENV_VAR** (string) - Line 167
- **BACKEND_IL2CPP** (string) - Line 50
- **BACKEND_MONO** (string) - Line 53
- **BUILD_CONFIG_DEBUG** (string) - Line 107
- **BUILD_CONFIG_RELEASE** (string) - Line 110
- **COMPONENT_ATOMIC_FILE_WRITER** (string) - Line 124
- **COMPONENT_BACKUP_MANAGER** (string) - Line 127
- **COMPONENT_CONSOLE_COMMAND** (string) - Line 133
- **COMPONENT_DATA_INTEGRITY** (string) - Line 142
- **COMPONENT_EMERGENCY_SAVE** (string) - Line 130
- **COMPONENT_IO_RUNNER** (string) - Line 136
- **COMPONENT_LOGGER** (string) - Line 118
- **COMPONENT_MIXER_VALIDATION** (string) - Line 121
- **COMPONENT_PERFORMANCE_OPTIMIZER** (string) - Line 139
- **COMPONENT_SAVE_MANAGER** (string) - Line 115
- **CONFIG_AUTO_BACKUP** (string) - Line 190
- **CONFIG_DEBUG_MODE** (string) - Line 181
- **CONFIG_EMERGENCY_SAVE** (string) - Line 193
- **CONFIG_MEMORY_OPTIMIZATION** (string) - Line 202
- **CONFIG_PERFORMANCE_MONITORING** (string) - Line 187
- **CONFIG_THREAD_SAFETY_MODE** (string) - Line 199
- **CONFIG_VALIDATION_ENABLED** (string) - Line 196
- **CONFIG_VERBOSE_LOGGING** (string) - Line 184
- **CONFIG_VERSION** (string) - Line 87
- **DOTNET_VERSION** (string) - Line 56
- **ENVIRONMENT_DEVELOPMENT** (string) - Line 98
- **ENVIRONMENT_PRODUCTION** (string) - Line 101
- **ENVIRONMENT_TESTING** (string) - Line 104
- **GAME_DATA_PATH_ENV_VAR** (string) - Line 161
- **GAME_REGISTRY_KEY_BASE** (string) - Line 150
- **LOCALAPPDATA_ENV_VAR** (string) - Line 170
- **MIN_MELONLOADER_VERSION** (string) - Line 84
- **MIN_UNITY_VERSION** (string) - Line 81
- **MOD_ASSEMBLY_NAME** (string) - Line 30
- **MOD_AUTHOR** (string) - Line 21
- **MOD_DESCRIPTION** (string) - Line 24
- **MOD_GUID** (string) - Line 33
- **MOD_NAME** (string) - Line 15
- **MOD_NAMESPACE** (string) - Line 27
- **MOD_REGISTRY_KEY** (string) - Line 153
- **MOD_VERSION** (string) - Line 18
- **NEWTONSOFT_JSON_ASSEMBLY** (string) - Line 76
- **PLATFORM_LINUX** (string) - Line 41
- **PLATFORM_MACOS** (string) - Line 44
- **PLATFORM_UNITY** (string) - Line 47
- **PLATFORM_WINDOWS** (string) - Line 38
- **PROGRAM_FILES_ENV_VAR** (string) - Line 173
- **PROGRAM_FILES_X86_ENV_VAR** (string) - Line 176
- **SAVE_FORMAT_VERSION** (string) - Line 90
- **SYSTEM_ASSEMBLY** (string) - Line 70
- **SYSTEM_CORE_ASSEMBLY** (string) - Line 73
- **UNITY_CORE_ASSEMBLY** (string) - Line 67
- **UNITY_ENGINE_ASSEMBLY** (string) - Line 64
- **UNITY_PATH_ENV_VAR** (string) - Line 158
- **UNITY_REGISTRY_KEY** (string) - Line 147
- **USER_PROFILE_ENV_VAR** (string) - Line 164

### ThreadingConstants.cs

- **ASYNC_RETRY_COUNT** (int) - Line 119
- **ASYNC_RETRY_DELAY_MS** (int) - Line 122
- **BACKGROUND_TASK_INTERVAL_MS** (int) - Line 125
- **BACKGROUND_THREAD_PREFIX** (string) - Line 19
- **BACKUP_MUTEX_NAME** (string) - Line 159
- **BACKUP_THREAD_NAME** (string) - Line 28
- **CANCELLATION_CHECK_INTERVAL_MS** (int) - Line 108
- **CRITICAL_SECTION_TIMEOUT_MS** (int) - Line 71
- **DEADLOCK_DETECTED_MESSAGE** (string) - Line 148
- **DEFAULT_CANCELLATION_TIMEOUT_MS** (int) - Line 99
- **EVENT_WAIT_TIMEOUT_MS** (int) - Line 77
- **EXTENDED_CANCELLATION_TIMEOUT_MS** (int) - Line 105
- **FILE_ACCESS_SEMAPHORE_NAME** (string) - Line 162
- **GLOBAL_OPERATION_EVENT_NAME** (string) - Line 171
- **IO_THREAD_PREFIX** (string) - Line 22
- **LOCK_TIMEOUT_MS** (int) - Line 68
- **MAIN_THREAD_BLOCKED_WARNING** (string) - Line 145
- **MAIN_THREAD_NAME** (string) - Line 16
- **MAX_ASYNC_QUEUE_SIZE** (int) - Line 116
- **MAX_COMPLETION_PORT_THREADS** (int) - Line 51
- **MAX_WORKER_THREADS** (int) - Line 45
- **MIN_COMPLETION_PORT_THREADS** (int) - Line 48
- **MIN_WORKER_THREADS** (int) - Line 42
- **MONITORING_THREAD_NAME** (string) - Line 31
- **MUTEX_TIMEOUT_MS** (int) - Line 62
- **PERFORMANCE_SEMAPHORE_NAME** (string) - Line 165
- **PERFORMANCE_THREAD_NAME** (string) - Line 37
- **QUICK_CANCELLATION_TIMEOUT_MS** (int) - Line 102
- **RESOURCE_CONTENTION_MESSAGE** (string) - Line 151
- **RW_LOCK_TIMEOUT_MS** (int) - Line 74
- **SAVE_MUTEX_NAME** (string) - Line 156
- **SAVE_THREAD_NAME** (string) - Line 25
- **SEMAPHORE_TIMEOUT_MS** (int) - Line 65
- **THREAD_ABORTED_MESSAGE** (string) - Line 136
- **THREAD_EXCEPTION_MESSAGE** (string) - Line 142
- **THREAD_IDLE_TIMEOUT_MS** (int) - Line 57
- **THREAD_POOL_QUEUE_LIMIT** (int) - Line 54
- **THREAD_PRIORITY_BACKGROUND** (string) - Line 91
- **THREAD_PRIORITY_CRITICAL** (string) - Line 94
- **THREAD_PRIORITY_HIGH** (string) - Line 82
- **THREAD_PRIORITY_LOW** (string) - Line 88
- **THREAD_PRIORITY_NORMAL** (string) - Line 85
- **THREAD_STARTED_MESSAGE** (string) - Line 130
- **THREAD_STOPPED_MESSAGE** (string) - Line 133
- **THREAD_TIMEOUT_MESSAGE** (string) - Line 139
- **VALIDATION_LOCK_NAME** (string) - Line 168
- **VALIDATION_THREAD_NAME** (string) - Line 34

### UtilityConstants.cs

- **ACTIVE_STRING** (string) - Line 311
- **AMPERSAND_CHAR** (char) - Line 364
- **APOSTROPHE_CHAR** (char) - Line 343
- **ARRAY_GROWTH_FACTOR** (double) - Line 279
- **ASTERISK_CHAR** (char) - Line 367
- **AT_SYMBOL_CHAR** (char) - Line 352
- **BACKSLASH_CHAR** (char) - Line 346
- **BACKSPACE_CHAR** (char) - Line 325
- **BELL_CHAR** (char) - Line 322
- **BINARY_FORMAT_PREFIX** (string) - Line 209
- **BYTES_PER_GB** (long) - Line 109
- **BYTES_PER_KB** (long) - Line 103
- **BYTES_PER_MB** (long) - Line 106
- **BYTES_PER_TB** (long) - Line 112
- **CARET_CHAR** (char) - Line 391
- **CARRIAGE_RETURN** (string) - Line 27
- **CELSIUS_TO_FAHRENHEIT_MULTIPLIER** (double) - Line 148
- **CELSIUS_TO_FAHRENHEIT_OFFSET** (double) - Line 145
- **CM_PER_METER** (double) - Line 139
- **COLLECTION_RESIZE_THRESHOLD** (double) - Line 282
- **COLON_SEPARATOR** (string) - Line 54
- **COMMA_SEPARATOR** (string) - Line 36
- **CULTURE_DE_DE** (string) - Line 226
- **CULTURE_EN_GB** (string) - Line 223
- **CULTURE_EN_US** (string) - Line 220
- **CULTURE_ES_ES** (string) - Line 232
- **CULTURE_FR_FR** (string) - Line 229
- **CULTURE_INVARIANT** (string) - Line 217
- **CULTURE_IT_IT** (string) - Line 235
- **CULTURE_JA_JP** (string) - Line 238
- **CULTURE_KO_KR** (string) - Line 250
- **CULTURE_PT_BR** (string) - Line 256
- **CULTURE_PT_PT** (string) - Line 253
- **CULTURE_RU_RU** (string) - Line 247
- **CULTURE_ZH_CN** (string) - Line 241
- **CULTURE_ZH_TW** (string) - Line 244
- **CURRENCY_FORMAT** (string) - Line 185
- **DASH_SEPARATOR** (string) - Line 48
- **DATE_FORMAT_ISO8601** (string) - Line 153
- **DATE_FORMAT_LONG** (string) - Line 159
- **DATE_FORMAT_SHORT** (string) - Line 156
- **DAYS_PER_WEEK** (int) - Line 127
- **DEFAULT_COLLECTION_CAPACITY** (int) - Line 264
- **DEFAULT_DICTIONARY_CAPACITY** (int) - Line 273
- **DEFAULT_HASHSET_CAPACITY** (int) - Line 276
- **DEGREES_TO_RADIANS** (double) - Line 86
- **DELETE_CHAR** (char) - Line 337
- **DISABLED_STRING** (string) - Line 308
- **DOLLAR_SYMBOL_CHAR** (char) - Line 358
- **DOT_SEPARATOR** (string) - Line 51
- **E** (double) - Line 68
- **EMPTY_ARRAY_SIZE** (int) - Line 261
- **EMPTY_STRING** (string) - Line 15
- **ENABLED_STRING** (string) - Line 305
- **EQUALS_CHAR** (char) - Line 376
- **ESCAPE_CHAR** (char) - Line 334
- **EXCLAMATION_MARK_CHAR** (char) - Line 382
- **EXPONENTIAL_FORMAT** (string) - Line 200
- **FALSE_STRING** (string) - Line 290
- **FEET_PER_YARD** (double) - Line 136
- **FILENAME_TIMESTAMP_FORMAT** (string) - Line 174
- **FORM_FEED_CHAR** (char) - Line 328
- **FORWARD_SLASH_CHAR** (char) - Line 349
- **GOLDEN_RATIO** (double) - Line 71
- **GRAVE_ACCENT_CHAR** (char) - Line 388
- **GRAVITY_ACCELERATION** (double) - Line 92
- **GREATER_THAN_CHAR** (char) - Line 415
- **HASH_SYMBOL_CHAR** (char) - Line 355
- **HEX_FORMAT_LOWER** (string) - Line 206
- **HEX_FORMAT_PREFIX** (string) - Line 212
- **HEX_FORMAT_UPPER** (string) - Line 203
- **HOURS_PER_DAY** (int) - Line 124
- **INACTIVE_STRING** (string) - Line 314
- **INCHES_PER_FOOT** (double) - Line 133
- **INTEGER_FORMAT_LEADING_ZEROS** (string) - Line 197
- **LARGE_COLLECTION_CAPACITY** (int) - Line 267
- **LEFT_BRACE_CHAR** (char) - Line 406
- **LEFT_BRACKET_CHAR** (char) - Line 400
- **LEFT_PAREN_CHAR** (char) - Line 394
- **LESS_THAN_CHAR** (char) - Line 412
- **LN_10** (double) - Line 83
- **LN_2** (double) - Line 80
- **LOG_TIMESTAMP_FORMAT** (string) - Line 177
- **MAX_RECOMMENDED_COLLECTION_SIZE** (int) - Line 270
- **METERS_PER_KM** (double) - Line 142
- **MINUS_CHAR** (char) - Line 373
- **MINUTES_PER_HOUR** (int) - Line 121
- **MONTHS_PER_YEAR** (int) - Line 130
- **MS_PER_SECOND** (int) - Line 115
- **NEWLINE** (string) - Line 24
- **NO_STRING** (string) - Line 296
- **NULL_CHAR** (char) - Line 319
- **NUMBER_FORMAT_2_DECIMAL** (string) - Line 191
- **NUMBER_FORMAT_4_DECIMAL** (string) - Line 194
- **OFF_STRING** (string) - Line 302
- **ON_STRING** (string) - Line 299
- **PERCENT_SYMBOL_CHAR** (char) - Line 361
- **PERCENTAGE_FORMAT** (string) - Line 188
- **PI** (double) - Line 65
- **PIPE_SEPARATOR** (string) - Line 42
- **PLANCK_CONSTANT** (double) - Line 98
- **PLUS_CHAR** (char) - Line 370
- **QUESTION_MARK_CHAR** (char) - Line 379
- **QUOTE_CHAR** (char) - Line 340
- **RADIANS_TO_DEGREES** (double) - Line 89
- **RIGHT_BRACE_CHAR** (char) - Line 409
- **RIGHT_BRACKET_CHAR** (char) - Line 403
- **RIGHT_PAREN_CHAR** (char) - Line 397
- **SECONDS_PER_MINUTE** (int) - Line 118
- **SEMICOLON_SEPARATOR** (string) - Line 39
- **SPACE** (string) - Line 18
- **SPEED_OF_LIGHT** (double) - Line 95
- **SQRT_2** (double) - Line 74
- **SQRT_3** (double) - Line 77
- **TAB** (string) - Line 21
- **TILDE_CHAR** (char) - Line 385
- **TIME_FORMAT_12HOUR** (string) - Line 165
- **TIME_FORMAT_24HOUR** (string) - Line 168
- **TIME_FORMAT_ONLY** (string) - Line 162
- **TIMESTAMP_FORMAT_MS** (string) - Line 171
- **TRUE_STRING** (string) - Line 287
- **UNDERSCORE_SEPARATOR** (string) - Line 45
- **UNIX_LINE_ENDING** (string) - Line 33
- **UTC_TIMESTAMP_FORMAT** (string) - Line 180
- **VERTICAL_TAB_CHAR** (char) - Line 331
- **WINDOWS_LINE_ENDING** (string) - Line 30
- **YES_STRING** (string) - Line 293

### ValidationConstants.cs

- **ALPHABETIC_REGEX_PATTERN** (string) - Line 68
- **ALPHANUMERIC_REGEX_PATTERN** (string) - Line 62
- **BACKUP_INTEGRITY_CHECK_ENABLED** (bool) - Line 153
- **CHECKSUM_VALIDATION_ENABLED** (bool) - Line 147
- **CORRUPTION_DETECTION_ENABLED** (bool) - Line 156
- **CREDIT_CARD_REGEX_PATTERN** (string) - Line 98
- **DEFAULT_DECIMAL_PRECISION** (int) - Line 27
- **EMAIL_MAX_LENGTH** (int) - Line 39
- **EMAIL_REGEX_PATTERN** (string) - Line 53
- **FILENAME_REGEX_PATTERN** (string) - Line 89
- **GUID_REGEX_PATTERN** (string) - Line 83
- **HEX_COLOR_REGEX_PATTERN** (string) - Line 80
- **INTEGRITY_CHECK_INTERVAL_MS** (int) - Line 165
- **INTEGRITY_HASH_ALGORITHM** (string) - Line 150
- **IPV4_REGEX_PATTERN** (string) - Line 74
- **IPV6_REGEX_PATTERN** (string) - Line 77
- **MAX_ARRAY_LENGTH** (int) - Line 36
- **MAX_DECIMAL_PRECISION** (int) - Line 30
- **MAX_NUMERIC_VALUE** (double) - Line 24
- **MAX_STRING_LENGTH** (int) - Line 18
- **MAX_VALIDATION_ATTEMPTS** (int) - Line 159
- **MIN_ARRAY_LENGTH** (int) - Line 33
- **MIN_NUMERIC_VALUE** (double) - Line 21
- **MIN_STRING_LENGTH** (int) - Line 15
- **NUMERIC_REGEX_PATTERN** (string) - Line 65
- **PASSWORD_MAX_LENGTH** (int) - Line 48
- **PASSWORD_MIN_LENGTH** (int) - Line 45
- **PHONE_REGEX_PATTERN** (string) - Line 59
- **STRONG_PASSWORD_REGEX_PATTERN** (string) - Line 71
- **UNIX_PATH_REGEX_PATTERN** (string) - Line 95
- **URL_MAX_LENGTH** (int) - Line 42
- **URL_REGEX_PATTERN** (string) - Line 56
- **VALIDATION_CUSTOM** (string) - Line 142
- **VALIDATION_DUPLICATE** (string) - Line 139
- **VALIDATION_FILE_SIZE** (string) - Line 133
- **VALIDATION_FILE_TYPE** (string) - Line 136
- **VALIDATION_INVALID_DATE** (string) - Line 127
- **VALIDATION_INVALID_EMAIL** (string) - Line 115
- **VALIDATION_INVALID_FORMAT** (string) - Line 106
- **VALIDATION_INVALID_PHONE** (string) - Line 121
- **VALIDATION_INVALID_URL** (string) - Line 118
- **VALIDATION_LENGTH_TEMPLATE** (string) - Line 109
- **VALIDATION_NOT_NUMERIC** (string) - Line 130
- **VALIDATION_RANGE_TEMPLATE** (string) - Line 112
- **VALIDATION_REQUIRED** (string) - Line 103
- **VALIDATION_RESULT_CANCELLED** (string) - Line 229
- **VALIDATION_RESULT_ERROR** (string) - Line 214
- **VALIDATION_RESULT_FAILURE** (string) - Line 211
- **VALIDATION_RESULT_PENDING** (string) - Line 223
- **VALIDATION_RESULT_SKIPPED** (string) - Line 220
- **VALIDATION_RESULT_SUCCESS** (string) - Line 208
- **VALIDATION_RESULT_TIMEOUT** (string) - Line 226
- **VALIDATION_RESULT_WARNING** (string) - Line 217
- **VALIDATION_TIMEOUT_MS** (int) - Line 162
- **VALIDATION_TYPE_BOOLEAN** (string) - Line 176
- **VALIDATION_TYPE_CUSTOM** (string) - Line 203
- **VALIDATION_TYPE_DATE** (string) - Line 179
- **VALIDATION_TYPE_EMAIL** (string) - Line 182
- **VALIDATION_TYPE_FILE** (string) - Line 194
- **VALIDATION_TYPE_JSON** (string) - Line 197
- **VALIDATION_TYPE_NUMERIC** (string) - Line 173
- **VALIDATION_TYPE_PASSWORD** (string) - Line 191
- **VALIDATION_TYPE_PHONE** (string) - Line 188
- **VALIDATION_TYPE_STRING** (string) - Line 170
- **VALIDATION_TYPE_URL** (string) - Line 185
- **VALIDATION_TYPE_XML** (string) - Line 200
- **VALIDATION_WEAK_PASSWORD** (string) - Line 124
- **VERSION_REGEX_PATTERN** (string) - Line 86
- **WINDOWS_PATH_REGEX_PATTERN** (string) - Line 92

### Cleanup Recommendations

🚨 **High Priority**: 1744 unused constants detected.

1. **Immediate Action**: Review constants for removal
2. **Impact Assessment**: Verify constants aren't used in external modules
3. **Gradual Cleanup**: Remove in batches to avoid merge conflicts

## Duplicate Constants

The following constants have duplicate names across files:

| Constant Name | Occurrences | Files |
|---------------|-------------|-------|
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 5 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants_LargeOriginal.cs, Constants_Original.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 4 | AllConstants.cs, Constants.cs, Constants_LargeOriginal.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | Constants_LargeOriginal.cs, Constants_Original.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | Constants_LargeOriginal.cs, Constants_Original.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 3 | AllConstants.cs, Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, Constants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, Constants_Original.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | PerformanceConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | Constants_LargeOriginal.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, FileConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ThreadingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, LoggingConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, PerformanceConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, MixerConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ValidationConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, UtilityConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, NetworkConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, ErrorConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, SystemConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 2 | AllConstants.cs, GameConstants.cs |

### Deduplication Recommendations

1. **Review Values**: Ensure duplicate constants have identical values
2. **Choose Primary**: Select one file to contain each constant
3. **Update References**: Modify usages to reference the primary location
4. **Remove Duplicates**: Delete redundant constant declarations

## High Usage Constants

Constants with the highest usage across the codebase:

| Constant | Usage Count | Example Files |
|----------|-------------|---------------|
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 100 | GameDirectoryDetectionLogger.cs, GameInstallDirectoryResolver.cs, MelonLoaderDirectoryResolver.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 75 | CpuMonitor.cs, IL2CPPTypeResolver.cs, Logger.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 30 | IL2CPPTypeResolver.cs, Logger.cs, MixerDataReader.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 28 | PerformanceOptimizer.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 20 | EntityConfiguration_Destroy_Patch.cs, LoadManager_LoadedGameFolderPath_Patch.cs, SaveManager_Save_Patch.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 16 | MixerDataBackupManager.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 8 | StressTestManager.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 8 | Logger.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 8 | Logger.cs, ... |
| $(Microsoft.PowerShell.Commands.GroupInfo.Name) | 6 | PerformanceOptimizer.cs, ... |

## Low Utilization Files

Files with less than 50% constant utilization:

### GameConstants.cs

- **Total Constants**: 78
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### FileConstants.cs

- **Total Constants**: 73
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### PerformanceConstants.cs

- **Total Constants**: 37
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### NetworkConstants.cs

- **Total Constants**: 90
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### MixerConstants.cs

- **Total Constants**: 42
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### ValidationConstants.cs

- **Total Constants**: 69
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### UtilityConstants.cs

- **Total Constants**: 127
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### ErrorConstants.cs

- **Total Constants**: 65
- **Used Constants**: 0
- **Utilization**: 0%

**Recommendation**: Review this file for potential cleanup opportunities.

### SystemConstants.cs

- **Total Constants**: 58
- **Used Constants**: 1
- **Utilization**: 1.7%

**Recommendation**: Review this file for potential cleanup opportunities.

### ThreadingConstants.cs

- **Total Constants**: 48
- **Used Constants**: 1
- **Utilization**: 2.1%

**Recommendation**: Review this file for potential cleanup opportunities.

### AllConstants.cs

- **Total Constants**: 129
- **Used Constants**: 7
- **Utilization**: 5.4%

**Recommendation**: Review this file for potential cleanup opportunities.

### Constants_LargeOriginal.cs

- **Total Constants**: 832
- **Used Constants**: 60
- **Utilization**: 7.2%

**Recommendation**: Review this file for potential cleanup opportunities.

### Constants_Original.cs

- **Total Constants**: 86
- **Used Constants**: 8
- **Utilization**: 9.3%

**Recommendation**: Review this file for potential cleanup opportunities.

### Constants.cs

- **Total Constants**: 54
- **Used Constants**: 6
- **Utilization**: 11.1%

**Recommendation**: Review this file for potential cleanup opportunities.

### LoggingConstants.cs

- **Total Constants**: 51
- **Used Constants**: 7
- **Utilization**: 13.7%

**Recommendation**: Review this file for potential cleanup opportunities.

## Action Plan

### Priority Actions

#### 🚨 HIGH - Remove Unused Constants

**1744 unused constants** should be reviewed for removal:

1. **Verify**: Confirm constants aren't used in external modules or config files
2. **Document**: Note reasons for removal in commit messages
3. **Remove**: Delete unused constant declarations
4. **Test**: Ensure removal doesn't break compilation or runtime

#### ⚠️ MEDIUM - Consolidate Duplicate Constants

**195 duplicate constant names** detected:

1. **Audit Values**: Ensure duplicates have identical values
2. **Choose Primary**: Select one authoritative location per constant
3. **Update References**: Modify code to use primary constant location
4. **Remove Duplicates**: Delete redundant declarations

### Best Practices

1. **Naming Conventions**: Use descriptive, consistent constant names
2. **Organization**: Group related constants in appropriate files
3. **Documentation**: Add comments explaining constant purposes
4. **Regular Audits**: Run this analysis before major releases
5. **Usage Verification**: Ensure new constants are actually used

## Technical Analysis Details

### Scan Coverage

- **Constants Files**: 15 files
- **Usage Files**: 49 files
- **Total Constants**: 1839
- **Total Usage References**: 395

### Exclusions

The following directories were excluded from analysis:
- ForCopilot/ - GitHub Copilot instruction files
- Scripts/ - DevOps and build scripts
- Legacy/ - Deprecated code

### Detection Patterns

- Constants: public const Type NAME = value
- Usage: Word boundary matches in non-constant files
- Files: Constants.cs and files in Constants/ directory

---

**Utilization Target**: 85%+ for well-maintained codebases

*Generated by MixerThreholdMod DevOps Suite - Constants Audit Generator*
