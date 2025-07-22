using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Mixer-specific constants for configuration, operations, and data handling
    ///  IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    ///  THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class MixerConstants
    {
        #region Mixer Configuration
        /// <summary>Mixer channel count (8 channels)</summary>
        public const int MIXER_CHANNEL_COUNT = 8;

        /// <summary>Default mixer channel (channel 0)</summary>
        public const int DEFAULT_MIXER_CHANNEL = 0;

        /// <summary>Maximum mixer volume level (100)</summary>
        public const float MAX_MIXER_VOLUME = 100.0f;

        /// <summary>Minimum mixer volume level (0)</summary>
        public const float MIN_MIXER_VOLUME = 0.0f;

        /// <summary>Default mixer volume level (50)</summary>
        public const float DEFAULT_MIXER_VOLUME = 50.0f;

        /// <summary>Mixer volume step size (5.0)</summary>
        public const float MIXER_VOLUME_STEP = 5.0f;

        /// <summary>Mixer gain adjustment range (-20dB to +20dB)</summary>
        public const float MIXER_GAIN_RANGE = 20.0f;

        /// <summary>Default mixer gain (0dB)</summary>
        public const float DEFAULT_MIXER_GAIN = 0.0f;
        #endregion

        #region Mixer Data Keys
        /// <summary>Mixer values key for save data</summary>
        public const string MIXER_VALUES_KEY = "MixerValues";

        /// <summary>Mixer channel key prefix</summary>
        public const string MIXER_CHANNEL_KEY_PREFIX = "Channel_";

        /// <summary>Mixer volume key</summary>
        public const string MIXER_VOLUME_KEY = "Volume";

        /// <summary>Mixer gain key</summary>
        public const string MIXER_GAIN_KEY = "Gain";

        /// <summary>Mixer mute key</summary>
        public const string MIXER_MUTE_KEY = "Mute";

        /// <summary>Mixer solo key</summary>
        public const string MIXER_SOLO_KEY = "Solo";

        /// <summary>Mixer configuration key</summary>
        public const string MIXER_CONFIG_KEY = "MixerConfig";

        /// <summary>Mixer preset key</summary>
        public const string MIXER_PRESET_KEY = "MixerPreset";
        #endregion

        #region Mixer Operation Messages
        /// <summary>Message for mixer initialization</summary>
        public const string MIXER_INIT_MESSAGE = "Mixer initialized successfully";

        /// <summary>Message for mixer configuration update</summary>
        public const string MIXER_CONFIG_UPDATE_MESSAGE = "Mixer configuration updated";

        /// <summary>Message for mixer value changed</summary>
        public const string MIXER_VALUE_CHANGED_MESSAGE = "Mixer value changed";

        /// <summary>Message for mixer channel muted</summary>
        public const string MIXER_CHANNEL_MUTED_MESSAGE = "Mixer channel muted";

        /// <summary>Message for mixer channel unmuted</summary>
        public const string MIXER_CHANNEL_UNMUTED_MESSAGE = "Mixer channel unmuted";

        /// <summary>Message for mixer preset loaded</summary>
        public const string MIXER_PRESET_LOADED_MESSAGE = "Mixer preset loaded";

        /// <summary>Message for mixer preset saved</summary>
        public const string MIXER_PRESET_SAVED_MESSAGE = "Mixer preset saved";

        /// <summary>Message for mixer reset</summary>
        public const string MIXER_RESET_MESSAGE = "Mixer reset to defaults";

        /// <summary>Message for mixer validation success</summary>
        public const string MIXER_VALIDATION_SUCCESS_MESSAGE = "Mixer validation successful";

        /// <summary>Message for mixer validation failure</summary>
        public const string MIXER_VALIDATION_FAILURE_MESSAGE = "Mixer validation failed";
        #endregion

        #region Mixer File Names
        /// <summary>Mixer save file name</summary>
        public const string MIXER_SAVE_FILENAME = "MixerThresholdSave.json";

        /// <summary>Mixer backup file name</summary>
        public const string MIXER_BACKUP_FILENAME = "MixerThresholdSave_Backup.json";

        /// <summary>Mixer config file name</summary>
        public const string MIXER_CONFIG_FILENAME = "MixerConfig.json";

        /// <summary>Mixer presets file name</summary>
        public const string MIXER_PRESETS_FILENAME = "MixerPresets.json";

        /// <summary>Mixer settings file name</summary>
        public const string MIXER_SETTINGS_FILENAME = "MixerSettings.json";
        #endregion

        #region Mixer Validation
        /// <summary>Maximum allowed mixer channels for validation</summary>
        public const int MAX_MIXER_CHANNELS = 16;

        /// <summary>Minimum allowed mixer channels for validation</summary>
        public const int MIN_MIXER_CHANNELS = 1;

        /// <summary>Volume precision for validation (2 decimal places)</summary>
        public const int VOLUME_PRECISION_DECIMALS = 2;

        /// <summary>Gain precision for validation (1 decimal place)</summary>
        public const int GAIN_PRECISION_DECIMALS = 1;

        /// <summary>Maximum mixer name length</summary>
        public const int MAX_MIXER_NAME_LENGTH = 50;

        /// <summary>Minimum mixer name length</summary>
        public const int MIN_MIXER_NAME_LENGTH = 3;
        #endregion

        #region Mixer Presets
        /// <summary>Default mixer preset name</summary>
        public const string DEFAULT_MIXER_PRESET_NAME = "Default";

        /// <summary>Factory mixer preset name</summary>
        public const string FACTORY_MIXER_PRESET_NAME = "Factory";

        /// <summary>User mixer preset prefix</summary>
        public const string USER_MIXER_PRESET_PREFIX = "User_";

        /// <summary>Maximum number of mixer presets</summary>
        public const int MAX_MIXER_PRESETS = 20;

        /// <summary>Mixer preset version</summary>
        public const string MIXER_PRESET_VERSION = "1.0";
        #endregion
    }
}