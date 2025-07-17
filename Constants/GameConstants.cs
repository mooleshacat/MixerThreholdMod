using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Game-specific constants for UI, graphics, audio, physics, and gameplay elements
    /// ⚠️ IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    /// ⚠️ THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class GameConstants
    {
        #region UI Constants
        /// <summary>Default UI font size</summary>
        public const int UI_FONT_SIZE_DEFAULT = 12;

        /// <summary>Large UI font size</summary>
        public const int UI_FONT_SIZE_LARGE = 16;

        /// <summary>Small UI font size</summary>
        public const int UI_FONT_SIZE_SMALL = 10;

        /// <summary>UI button width</summary>
        public const int UI_BUTTON_WIDTH = 100;

        /// <summary>UI button height</summary>
        public const int UI_BUTTON_HEIGHT = 30;

        /// <summary>UI panel padding</summary>
        public const int UI_PANEL_PADDING = 10;

        /// <summary>UI element spacing</summary>
        public const int UI_ELEMENT_SPACING = 5;

        /// <summary>UI window minimum width</summary>
        public const int UI_WINDOW_MIN_WIDTH = 200;

        /// <summary>UI window minimum height</summary>
        public const int UI_WINDOW_MIN_HEIGHT = 150;

        /// <summary>UI scrollbar width</summary>
        public const int UI_SCROLLBAR_WIDTH = 16;

        /// <summary>UI tooltip delay in milliseconds</summary>
        public const int UI_TOOLTIP_DELAY_MS = 500;

        /// <summary>UI fade animation duration in milliseconds</summary>
        public const int UI_FADE_DURATION_MS = 250;
        #endregion

        #region Color Constants
        /// <summary>Primary UI color (hex)</summary>
        public const string UI_COLOR_PRIMARY = "#2196F3";

        /// <summary>Secondary UI color (hex)</summary>
        public const string UI_COLOR_SECONDARY = "#FF9800";

        /// <summary>Success UI color (hex)</summary>
        public const string UI_COLOR_SUCCESS = "#4CAF50";

        /// <summary>Warning UI color (hex)</summary>
        public const string UI_COLOR_WARNING = "#FF5722";

        /// <summary>Error UI color (hex)</summary>
        public const string UI_COLOR_ERROR = "#F44336";

        /// <summary>Background UI color (hex)</summary>
        public const string UI_COLOR_BACKGROUND = "#303030";

        /// <summary>Text UI color (hex)</summary>
        public const string UI_COLOR_TEXT = "#FFFFFF";

        /// <summary>Disabled UI color (hex)</summary>
        public const string UI_COLOR_DISABLED = "#757575";
        #endregion

        #region Audio Constants
        /// <summary>Master audio volume (0-1)</summary>
        public const float AUDIO_MASTER_VOLUME = 1.0f;

        /// <summary>Default audio volume (0-1)</summary>
        public const float AUDIO_DEFAULT_VOLUME = 0.8f;

        /// <summary>Minimum audio volume (0-1)</summary>
        public const float AUDIO_MIN_VOLUME = 0.0f;

        /// <summary>Maximum audio volume (0-1)</summary>
        public const float AUDIO_MAX_VOLUME = 1.0f;

        /// <summary>Audio fade time in seconds</summary>
        public const float AUDIO_FADE_TIME = 0.5f;

        /// <summary>Audio sample rate (Hz)</summary>
        public const int AUDIO_SAMPLE_RATE = 44100;

        /// <summary>Audio buffer size</summary>
        public const int AUDIO_BUFFER_SIZE = 1024;

        /// <summary>Audio channels (stereo)</summary>
        public const int AUDIO_CHANNELS = 2;

        /// <summary>Audio bit depth</summary>
        public const int AUDIO_BIT_DEPTH = 16;
        #endregion

        #region Graphics Constants
        /// <summary>Default screen width</summary>
        public const int GRAPHICS_SCREEN_WIDTH = 1920;

        /// <summary>Default screen height</summary>
        public const int GRAPHICS_SCREEN_HEIGHT = 1080;

        /// <summary>Target frame rate</summary>
        public const int GRAPHICS_TARGET_FPS = 60;

        /// <summary>Minimum frame rate threshold</summary>
        public const int GRAPHICS_MIN_FPS = 30;

        /// <summary>VSync enabled</summary>
        public const bool GRAPHICS_VSYNC_ENABLED = true;

        /// <summary>Anti-aliasing samples</summary>
        public const int GRAPHICS_AA_SAMPLES = 4;

        /// <summary>Anisotropic filtering level</summary>
        public const int GRAPHICS_ANISO_FILTERING = 16;

        /// <summary>Shadow quality level</summary>
        public const string GRAPHICS_SHADOW_QUALITY = "High";

        /// <summary>Texture quality level</summary>
        public const string GRAPHICS_TEXTURE_QUALITY = "Full Res";

        /// <summary>Render distance</summary>
        public const float GRAPHICS_RENDER_DISTANCE = 1000.0f;
        #endregion

        #region Physics Constants
        /// <summary>Gravity value (m/s²)</summary>
        public const float PHYSICS_GRAVITY = -9.81f;

        /// <summary>Fixed timestep for physics (seconds)</summary>
        public const float PHYSICS_FIXED_TIMESTEP = 0.02f;

        /// <summary>Maximum allowed timestep (seconds)</summary>
        public const float PHYSICS_MAX_TIMESTEP = 0.333f;

        /// <summary>Default friction coefficient</summary>
        public const float PHYSICS_DEFAULT_FRICTION = 0.6f;

        /// <summary>Default bounce coefficient</summary>
        public const float PHYSICS_DEFAULT_BOUNCE = 0.3f;

        /// <summary>Sleep threshold for physics objects</summary>
        public const float PHYSICS_SLEEP_THRESHOLD = 0.005f;

        /// <summary>Collision detection precision</summary>
        public const string PHYSICS_COLLISION_DETECTION = "Continuous";

        /// <summary>Physics solver iterations</summary>
        public const int PHYSICS_SOLVER_ITERATIONS = 6;

        /// <summary>Physics velocity iterations</summary>
        public const int PHYSICS_VELOCITY_ITERATIONS = 1;
        #endregion

        #region Input Constants
        /// <summary>Input sensitivity for mouse</summary>
        public const float INPUT_MOUSE_SENSITIVITY = 2.0f;

        /// <summary>Input deadzone for controllers</summary>
        public const float INPUT_CONTROLLER_DEADZONE = 0.2f;

        /// <summary>Double-click time threshold (milliseconds)</summary>
        public const int INPUT_DOUBLE_CLICK_TIME_MS = 300;

        /// <summary>Long press time threshold (milliseconds)</summary>
        public const int INPUT_LONG_PRESS_TIME_MS = 500;

        /// <summary>Input buffer size for commands</summary>
        public const int INPUT_BUFFER_SIZE = 10;

        /// <summary>Maximum input lag compensation (milliseconds)</summary>
        public const int INPUT_MAX_LAG_COMPENSATION_MS = 100;
        #endregion

        #region Game State Constants
        /// <summary>Main menu state identifier</summary>
        public const string GAME_STATE_MAIN_MENU = "MainMenu";

        /// <summary>Loading state identifier</summary>
        public const string GAME_STATE_LOADING = "Loading";

        /// <summary>Playing state identifier</summary>
        public const string GAME_STATE_PLAYING = "Playing";

        /// <summary>Paused state identifier</summary>
        public const string GAME_STATE_PAUSED = "Paused";

        /// <summary>Game over state identifier</summary>
        public const string GAME_STATE_GAME_OVER = "GameOver";

        /// <summary>Settings state identifier</summary>
        public const string GAME_STATE_SETTINGS = "Settings";

        /// <summary>Credits state identifier</summary>
        public const string GAME_STATE_CREDITS = "Credits";
        #endregion

        #region Animation Constants
        /// <summary>Default animation speed multiplier</summary>
        public const float ANIMATION_SPEED_DEFAULT = 1.0f;

        /// <summary>Fast animation speed multiplier</summary>
        public const float ANIMATION_SPEED_FAST = 2.0f;

        /// <summary>Slow animation speed multiplier</summary>
        public const float ANIMATION_SPEED_SLOW = 0.5f;

        /// <summary>Animation blend time (seconds)</summary>
        public const float ANIMATION_BLEND_TIME = 0.2f;

        /// <summary>Animation fade in time (seconds)</summary>
        public const float ANIMATION_FADE_IN_TIME = 0.3f;

        /// <summary>Animation fade out time (seconds)</summary>
        public const float ANIMATION_FADE_OUT_TIME = 0.2f;

        /// <summary>Animation loop mode</summary>
        public const string ANIMATION_LOOP_MODE = "Loop";

        /// <summary>Animation play mode</summary>
        public const string ANIMATION_PLAY_MODE = "StopSameLayer";
        #endregion

        #region Gameplay Constants
        /// <summary>Player lives count</summary>
        public const int GAMEPLAY_PLAYER_LIVES = 3;

        /// <summary>Maximum player level</summary>
        public const int GAMEPLAY_MAX_LEVEL = 100;

        /// <summary>Starting player level</summary>
        public const int GAMEPLAY_START_LEVEL = 1;

        /// <summary>Experience points per level</summary>
        public const int GAMEPLAY_XP_PER_LEVEL = 1000;

        /// <summary>Default player health</summary>
        public const float GAMEPLAY_DEFAULT_HEALTH = 100.0f;

        /// <summary>Default player speed</summary>
        public const float GAMEPLAY_DEFAULT_SPEED = 5.0f;

        /// <summary>Default jump force</summary>
        public const float GAMEPLAY_DEFAULT_JUMP_FORCE = 10.0f;

        /// <summary>Respawn time (seconds)</summary>
        public const float GAMEPLAY_RESPAWN_TIME = 3.0f;

        /// <summary>Invincibility time after respawn (seconds)</summary>
        public const float GAMEPLAY_INVINCIBILITY_TIME = 2.0f;
        #endregion
    }
}