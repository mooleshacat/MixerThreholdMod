using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.Networking;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200074F RID: 1871
	public class Settings : PersistentSingleton<Settings>
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x000D162F File Offset: 0x000CF82F
		// (set) Token: 0x06003240 RID: 12864 RVA: 0x000D1637 File Offset: 0x000CF837
		public Settings.UnitType unitType { get; protected set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x000D1640 File Offset: 0x000CF840
		public bool PausingFreezesTime
		{
			get
			{
				return Player.PlayerList.Count <= 1 && !Singleton<Lobby>.Instance.IsInLobby;
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000D1660 File Offset: 0x000CF860
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Settings>.Instance == null || Singleton<Settings>.Instance != this)
			{
				return;
			}
			this.playerControls = this.InputActions.FindActionMap("Generic", false);
			this.DisplaySettings = this.ReadDisplaySettings();
			this.UnappliedDisplaySettings = this.ReadDisplaySettings();
			this.GraphicsSettings = this.ReadGraphicsSettings();
			this.AudioSettings = this.ReadAudioSettings();
			this.InputSettings = this.ReadInputSettings();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				this.LaunchArgs.Add(commandLineArgs[i]);
				if (commandLineArgs[i] == "-beta")
				{
					GameManager.IS_BETA = true;
				}
			}
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000D1718 File Offset: 0x000CF918
		protected override void Start()
		{
			base.Start();
			this.ApplyDisplaySettings(this.DisplaySettings);
			this.ApplyGraphicsSettings(this.GraphicsSettings);
			this.ApplyAudioSettings(this.AudioSettings);
			this.ApplyInputSettings(this.InputSettings);
			if (SteamManager.Initialized)
			{
				this.LaunchArgs.Contains("-disablecountrycheck");
			}
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000D1774 File Offset: 0x000CF974
		private void CheckCountryCode()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			string ipcountry = SteamUtils.GetIPCountry();
			Console.Log("Country code: " + ipcountry, null);
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x000D17A0 File Offset: 0x000CF9A0
		public void ApplyDisplaySettings(DisplaySettings settings)
		{
			Resolution[] array = DisplaySettings.GetResolutions().ToArray();
			Resolution resolution = array[Mathf.Clamp(settings.ResolutionIndex, 0, array.Length - 1)];
			FullScreenMode fullScreenMode = FullScreenMode.Windowed;
			switch (settings.DisplayMode)
			{
			case DisplaySettings.EDisplayMode.Windowed:
				fullScreenMode = FullScreenMode.Windowed;
				break;
			case DisplaySettings.EDisplayMode.FullscreenWindow:
				fullScreenMode = FullScreenMode.FullScreenWindow;
				break;
			case DisplaySettings.EDisplayMode.ExclusiveFullscreen:
				fullScreenMode = FullScreenMode.ExclusiveFullScreen;
				break;
			}
			Screen.fullScreenMode = fullScreenMode;
			Screen.SetResolution(resolution.width, resolution.height, settings.DisplayMode == DisplaySettings.EDisplayMode.ExclusiveFullscreen || settings.DisplayMode == DisplaySettings.EDisplayMode.FullscreenWindow);
			QualitySettings.vSyncCount = (settings.VSync ? 1 : 0);
			Application.targetFrameRate = settings.TargetFPS;
			List<DisplayInfo> list = new List<DisplayInfo>();
			Screen.GetDisplayLayout(list);
			DisplayInfo displayInfo = list[Mathf.Clamp(settings.ActiveDisplayIndex, 0, list.Count - 1)];
			this.MoveMainWindowTo(displayInfo);
			CanvasScaler.SetScaleFactor(settings.UIScale);
			Singleton<Settings>.Instance.CameraBobIntensity = settings.CameraBobbing;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x000D188E File Offset: 0x000CFA8E
		private void MoveMainWindowTo(DisplayInfo displayInfo)
		{
			Console.Log("Moving main window to display: " + displayInfo.name, null);
			Screen.MoveMainWindowTo(displayInfo, new Vector2Int(displayInfo.width / 2, displayInfo.height / 2));
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000D18C3 File Offset: 0x000CFAC3
		public void ReloadGraphicsSettings()
		{
			this.ApplyGraphicsSettings(this.GraphicsSettings);
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000D18D4 File Offset: 0x000CFAD4
		public void ApplyGraphicsSettings(GraphicsSettings settings)
		{
			QualitySettings.SetQualityLevel((int)settings.GraphicsQuality);
			PlayerCamera.SetAntialiasingMode(settings.AntiAliasingMode);
			this.CameraFOV = settings.FOV;
			this.SSAO.SetActive(settings.SSAO);
			this.GodRays.SetActive(settings.GodRays);
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000D1925 File Offset: 0x000CFB25
		public void ReloadAudioSettings()
		{
			this.ApplyAudioSettings(this.AudioSettings);
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x000D1934 File Offset: 0x000CFB34
		public void ApplyAudioSettings(AudioSettings settings)
		{
			Singleton<AudioManager>.Instance.SetMasterVolume(settings.MasterVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Ambient, settings.AmbientVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Music, settings.MusicVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.FX, settings.SFXVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.UI, settings.UIVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Voice, settings.DialogueVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Footsteps, settings.FootstepsVolume);
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x000D19B7 File Offset: 0x000CFBB7
		public void ReloadInputSettings()
		{
			this.ApplyInputSettings(this.InputSettings);
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000D19C8 File Offset: 0x000CFBC8
		public void ApplyInputSettings(InputSettings settings)
		{
			this.InputSettings = settings;
			this.LookSensitivity = settings.MouseSensitivity;
			this.InvertMouse = settings.InvertMouse;
			this.SprintMode = settings.SprintMode;
			this.InputActions.Disable();
			InputActionRebindingExtensions.LoadBindingOverridesFromJson(this.InputActions, settings.BindingOverrides, true);
			this.InputActions.Enable();
			this.GameInput.PlayerInput.actions = this.InputActions;
			Action action = this.onInputsApplied;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000D1A50 File Offset: 0x000CFC50
		public void WriteDisplaySettings(DisplaySettings settings)
		{
			this.DisplaySettings = settings;
			this.UnappliedDisplaySettings = settings;
			PlayerPrefs.SetInt("ResolutionIndex", settings.ResolutionIndex);
			PlayerPrefs.SetInt("DisplayMode", (int)settings.DisplayMode);
			PlayerPrefs.SetInt("VSync", settings.VSync ? 1 : 0);
			PlayerPrefs.SetInt("TargetFPS", settings.TargetFPS);
			PlayerPrefs.SetFloat("UIScale", settings.UIScale);
			PlayerPrefs.SetFloat("CameraBobbing", settings.CameraBobbing);
			PlayerPrefs.SetInt("ActiveDisplayIndex", settings.ActiveDisplayIndex);
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000D1AE4 File Offset: 0x000CFCE4
		public DisplaySettings ReadDisplaySettings()
		{
			return new DisplaySettings
			{
				ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", Screen.resolutions.Length - 1),
				DisplayMode = (DisplaySettings.EDisplayMode)PlayerPrefs.GetInt("DisplayMode", 2),
				VSync = (PlayerPrefs.GetInt("VSync", 1) == 1),
				TargetFPS = PlayerPrefs.GetInt("TargetFPS", 90),
				UIScale = PlayerPrefs.GetFloat("UIScale", 1f),
				CameraBobbing = PlayerPrefs.GetFloat("CameraBobbing", 0.7f),
				ActiveDisplayIndex = PlayerPrefs.GetInt("ActiveDisplayIndex", 0)
			};
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x000D1B8C File Offset: 0x000CFD8C
		public void WriteGraphicsSettings(GraphicsSettings settings)
		{
			this.GraphicsSettings = settings;
			PlayerPrefs.SetInt("QualityLevel", (int)settings.GraphicsQuality);
			PlayerPrefs.SetInt("AntiAliasing", (int)settings.AntiAliasingMode);
			PlayerPrefs.SetFloat("FOV", settings.FOV);
			PlayerPrefs.SetInt("SSAO", settings.SSAO ? 1 : 0);
			PlayerPrefs.SetInt("GodRays", settings.GodRays ? 1 : 0);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x000D1BFC File Offset: 0x000CFDFC
		public GraphicsSettings ReadGraphicsSettings()
		{
			return new GraphicsSettings
			{
				GraphicsQuality = (GraphicsSettings.EGraphicsQuality)PlayerPrefs.GetInt("QualityLevel", 2),
				AntiAliasingMode = (GraphicsSettings.EAntiAliasingMode)PlayerPrefs.GetInt("AntiAliasing", 2),
				FOV = PlayerPrefs.GetFloat("FOV", 80f),
				SSAO = (PlayerPrefs.GetInt("SSAO", 1) == 1),
				GodRays = (PlayerPrefs.GetInt("GodRays", 1) == 1)
			};
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000D1C70 File Offset: 0x000CFE70
		public void WriteAudioSettings(AudioSettings settings)
		{
			this.AudioSettings = settings;
			PlayerPrefs.SetFloat("MasterVolume", settings.MasterVolume);
			PlayerPrefs.SetFloat("AmbientVolume", settings.AmbientVolume);
			PlayerPrefs.SetFloat("MusicVolume", settings.MusicVolume);
			PlayerPrefs.SetFloat("SFXVolume", settings.SFXVolume);
			PlayerPrefs.SetFloat("UIVolume", settings.UIVolume);
			PlayerPrefs.SetFloat("DialogueVolume", settings.DialogueVolume);
			PlayerPrefs.SetFloat("FootstepsVolume", settings.FootstepsVolume);
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000D1CF4 File Offset: 0x000CFEF4
		public AudioSettings ReadAudioSettings()
		{
			return new AudioSettings
			{
				MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f),
				AmbientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f),
				MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f),
				SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f),
				UIVolume = PlayerPrefs.GetFloat("UIVolume", 1f),
				DialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f),
				FootstepsVolume = PlayerPrefs.GetFloat("FootstepsVolume", 1f)
			};
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000D1D9C File Offset: 0x000CFF9C
		public void WriteInputSettings(InputSettings settings)
		{
			this.InputSettings = settings;
			PlayerPrefs.SetFloat("MouseSensitivity", settings.MouseSensitivity);
			PlayerPrefs.SetInt("InvertMouse", settings.InvertMouse ? 1 : 0);
			PlayerPrefs.SetInt("SprintMode", (int)settings.SprintMode);
			string value = InputActionRebindingExtensions.SaveBindingOverridesAsJson(this.GameInput.PlayerInput.actions);
			PlayerPrefs.SetString("BindingOverrides", value);
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x000D1E08 File Offset: 0x000D0008
		public InputSettings ReadInputSettings()
		{
			return new InputSettings
			{
				MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f),
				InvertMouse = (PlayerPrefs.GetInt("InvertMouse", 0) == 1),
				SprintMode = (InputSettings.EActionMode)PlayerPrefs.GetInt("SprintMode", 0),
				BindingOverrides = PlayerPrefs.GetString("BindingOverrides", InputActionRebindingExtensions.SaveBindingOverridesAsJson(this.GameInput.PlayerInput.actions))
			};
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000D1E7C File Offset: 0x000D007C
		public string GetActionControlPath(string actionName)
		{
			InputAction inputAction = this.playerControls.FindAction(actionName, false);
			if (inputAction == null)
			{
				Console.LogError("Could not find action with name '" + actionName + "'", null);
				return string.Empty;
			}
			return inputAction.controls[0].path;
		}

		// Token: 0x04002381 RID: 9089
		public const float MinYPos = -20f;

		// Token: 0x04002382 RID: 9090
		public const string BETA_ARG = "-beta";

		// Token: 0x04002383 RID: 9091
		public const string DISABLE_COUNTRY_CHECK_ARG = "-disablecountrycheck";

		// Token: 0x04002384 RID: 9092
		public const bool COUNTRY_CHECK = false;

		// Token: 0x04002386 RID: 9094
		public List<string> LaunchArgs = new List<string>();

		// Token: 0x04002387 RID: 9095
		public DisplaySettings DisplaySettings;

		// Token: 0x04002388 RID: 9096
		public DisplaySettings UnappliedDisplaySettings;

		// Token: 0x04002389 RID: 9097
		public GraphicsSettings GraphicsSettings = new GraphicsSettings();

		// Token: 0x0400238A RID: 9098
		public AudioSettings AudioSettings = new AudioSettings();

		// Token: 0x0400238B RID: 9099
		public InputSettings InputSettings = new InputSettings();

		// Token: 0x0400238C RID: 9100
		public InputActionAsset InputActions;

		// Token: 0x0400238D RID: 9101
		public GameInput GameInput;

		// Token: 0x0400238E RID: 9102
		public ScriptableRendererFeature SSAO;

		// Token: 0x0400238F RID: 9103
		public ScriptableRendererFeature GodRays;

		// Token: 0x04002390 RID: 9104
		[Header("Camera")]
		public float LookSensitivity = 1f;

		// Token: 0x04002391 RID: 9105
		public bool InvertMouse;

		// Token: 0x04002392 RID: 9106
		public float CameraFOV = 75f;

		// Token: 0x04002393 RID: 9107
		public InputSettings.EActionMode SprintMode = InputSettings.EActionMode.Hold;

		// Token: 0x04002394 RID: 9108
		[Range(0f, 1f)]
		public float CameraBobIntensity = 1f;

		// Token: 0x04002395 RID: 9109
		private InputActionMap playerControls;

		// Token: 0x04002396 RID: 9110
		public Action onDisplayChanged;

		// Token: 0x04002397 RID: 9111
		public Action onInputsApplied;

		// Token: 0x02000750 RID: 1872
		public enum UnitType
		{
			// Token: 0x04002399 RID: 9113
			Metric,
			// Token: 0x0400239A RID: 9114
			Imperial
		}
	}
}
