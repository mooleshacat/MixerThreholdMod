using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Audio
{
	// Token: 0x020007DC RID: 2012
	public class AudioManager : PersistentSingleton<AudioManager>
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x000E53DA File Offset: 0x000E35DA
		public float MasterVolume
		{
			get
			{
				return this.masterVolume;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x000E53E2 File Offset: 0x000E35E2
		public float AmbientVolume
		{
			get
			{
				return this.ambientVolume * this.masterVolume;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x000E53F1 File Offset: 0x000E35F1
		public float UnscaledAmbientVolume
		{
			get
			{
				return this.ambientVolume;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06003665 RID: 13925 RVA: 0x000E53F9 File Offset: 0x000E35F9
		public float FootstepsVolume
		{
			get
			{
				return this.footstepsVolume * this.masterVolume;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x000E5408 File Offset: 0x000E3608
		public float UnscaledFootstepsVolume
		{
			get
			{
				return this.footstepsVolume;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003667 RID: 13927 RVA: 0x000E5410 File Offset: 0x000E3610
		public float FXVolume
		{
			get
			{
				return this.fxVolume * this.masterVolume;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06003668 RID: 13928 RVA: 0x000E541F File Offset: 0x000E361F
		public float UnscaledFXVolume
		{
			get
			{
				return this.fxVolume;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003669 RID: 13929 RVA: 0x000E5427 File Offset: 0x000E3627
		public float UIVolume
		{
			get
			{
				return this.uiVolume * this.masterVolume;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600366A RID: 13930 RVA: 0x000E5436 File Offset: 0x000E3636
		public float UnscaledUIVolume
		{
			get
			{
				return this.uiVolume;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600366B RID: 13931 RVA: 0x000E543E File Offset: 0x000E363E
		public float MusicVolume
		{
			get
			{
				return this.musicVolume * this.masterVolume * 0.7f;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x000E5453 File Offset: 0x000E3653
		public float UnscaledMusicVolume
		{
			get
			{
				return this.musicVolume;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x000E545B File Offset: 0x000E365B
		public float VoiceVolume
		{
			get
			{
				return this.voiceVolume * this.masterVolume * 0.5f;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x000E5470 File Offset: 0x000E3670
		public float UnscaledVoiceVolume
		{
			get
			{
				return this.voiceVolume;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x000E5478 File Offset: 0x000E3678
		// (set) Token: 0x06003670 RID: 13936 RVA: 0x000E5480 File Offset: 0x000E3680
		public float WorldMusicVolumeMultiplier { get; private set; } = 1f;

		// Token: 0x06003671 RID: 13937 RVA: 0x000E5489 File Offset: 0x000E3689
		public float GetScaledMusicVolumeMultiplier(float min)
		{
			return Mathf.Lerp(min, 1f, this.WorldMusicVolumeMultiplier);
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000E549C File Offset: 0x000E369C
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<AudioManager>.Instance == null || Singleton<AudioManager>.Instance != this)
			{
				return;
			}
			this.SetGameVolume(0f);
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000E54CA File Offset: 0x000E36CA
		protected override void Start()
		{
			base.Start();
			if (Singleton<AudioManager>.Instance == null || Singleton<AudioManager>.Instance != this)
			{
				return;
			}
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				this.SetDistorted(false, 0.5f);
			});
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000E5508 File Offset: 0x000E3708
		protected void Update()
		{
			if (SceneManager.GetActiveScene().name == "Main" && !Singleton<LoadingScreen>.Instance.IsOpen)
			{
				if (this.currentGameVolume < 1f)
				{
					this.SetGameVolume(this.currentGameVolume + Time.deltaTime * 1f);
				}
			}
			else if (this.currentGameVolume > 0f)
			{
				this.SetGameVolume(this.currentGameVolume - Time.deltaTime * 1f);
			}
			if (Singleton<MusicPlayer>.Instance.IsPlaying)
			{
				this.WorldMusicVolumeMultiplier = Mathf.Lerp(this.WorldMusicVolumeMultiplier, 0f, Time.deltaTime / 4f);
				return;
			}
			this.WorldMusicVolumeMultiplier = Mathf.Lerp(this.WorldMusicVolumeMultiplier, 1f, Time.deltaTime / 4f);
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000E55D5 File Offset: 0x000E37D5
		public void SetGameVolumeMultipler(float value)
		{
			this.gameVolumeMultiplier = value;
			this.SetGameVolume(this.currentGameVolume);
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000E55EA File Offset: 0x000E37EA
		public void SetDistorted(bool distorted, float transition = 5f)
		{
			if (distorted)
			{
				this.DistortedSnapshot.TransitionTo(transition);
				return;
			}
			this.DefaultSnapshot.TransitionTo(transition);
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000E5608 File Offset: 0x000E3808
		private void SetGameVolume(float value)
		{
			this.currentGameVolume = value;
			value = Mathf.Lerp(value * this.gameVolumeMultiplier, 0.0001f, 0.0001f);
			this.MainGameMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000E5658 File Offset: 0x000E3858
		public float GetVolume(EAudioType audioType, bool scaled = true)
		{
			switch (audioType)
			{
			case EAudioType.Ambient:
				if (!scaled)
				{
					return this.UnscaledAmbientVolume;
				}
				return this.AmbientVolume;
			case EAudioType.Footsteps:
				if (!scaled)
				{
					return this.UnscaledFootstepsVolume;
				}
				return this.FootstepsVolume;
			case EAudioType.FX:
				if (!scaled)
				{
					return this.UnscaledFXVolume;
				}
				return this.FXVolume;
			case EAudioType.UI:
				if (!scaled)
				{
					return this.UnscaledUIVolume;
				}
				return this.UIVolume;
			case EAudioType.Music:
				if (!scaled)
				{
					return this.UnscaledMusicVolume;
				}
				return this.MusicVolume;
			case EAudioType.Voice:
				if (!scaled)
				{
					return this.UnscaledVoiceVolume;
				}
				return this.VoiceVolume;
			default:
				return 1f;
			}
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x000E56F0 File Offset: 0x000E38F0
		public void SetMasterVolume(float volume)
		{
			this.masterVolume = volume;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000E56FC File Offset: 0x000E38FC
		public void SetVolume(EAudioType type, float volume)
		{
			switch (type)
			{
			case EAudioType.Ambient:
				this.ambientVolume = volume;
				return;
			case EAudioType.Footsteps:
				this.footstepsVolume = volume;
				return;
			case EAudioType.FX:
				this.fxVolume = volume;
				return;
			case EAudioType.UI:
				this.uiVolume = volume;
				return;
			case EAudioType.Music:
				this.musicVolume = volume;
				return;
			case EAudioType.Voice:
				this.voiceVolume = volume;
				return;
			default:
				return;
			}
		}

		// Token: 0x0400269F RID: 9887
		public const float MIN_WORLD_MUSIC_VOLUME_MULTIPLIER = 0f;

		// Token: 0x040026A0 RID: 9888
		public const float MUSIC_FADE_TIME = 4f;

		// Token: 0x040026A1 RID: 9889
		[Range(0f, 2f)]
		[SerializeField]
		protected float masterVolume = 1f;

		// Token: 0x040026A2 RID: 9890
		[Range(0f, 2f)]
		[SerializeField]
		protected float ambientVolume = 1f;

		// Token: 0x040026A3 RID: 9891
		[Range(0f, 2f)]
		[SerializeField]
		protected float footstepsVolume = 1f;

		// Token: 0x040026A4 RID: 9892
		[Range(0f, 2f)]
		[SerializeField]
		protected float fxVolume = 1f;

		// Token: 0x040026A5 RID: 9893
		[Range(0f, 2f)]
		[SerializeField]
		protected float uiVolume = 1f;

		// Token: 0x040026A6 RID: 9894
		[Range(0f, 2f)]
		[SerializeField]
		protected float musicVolume = 1f;

		// Token: 0x040026A7 RID: 9895
		[Range(0f, 2f)]
		[SerializeField]
		protected float voiceVolume = 1f;

		// Token: 0x040026A8 RID: 9896
		public UnityEvent onSettingsChanged = new UnityEvent();

		// Token: 0x040026A9 RID: 9897
		[Header("Generic Door Sounds")]
		public AudioSourceController DoorOpen;

		// Token: 0x040026AA RID: 9898
		public AudioSourceController DoorClose;

		// Token: 0x040026AB RID: 9899
		[Header("Mixers")]
		public AudioMixerGroup MainGameMixer;

		// Token: 0x040026AC RID: 9900
		public AudioMixerGroup MenuMixer;

		// Token: 0x040026AD RID: 9901
		public AudioMixerGroup MusicMixer;

		// Token: 0x040026AE RID: 9902
		private float currentGameVolume = 1f;

		// Token: 0x040026AF RID: 9903
		private const float minGameVolume = 0.0001f;

		// Token: 0x040026B0 RID: 9904
		private const float maxGameVolume = 0.0001f;

		// Token: 0x040026B1 RID: 9905
		private float gameVolumeMultiplier = 1f;

		// Token: 0x040026B3 RID: 9907
		public AudioMixerSnapshot DefaultSnapshot;

		// Token: 0x040026B4 RID: 9908
		public AudioMixerSnapshot DistortedSnapshot;
	}
}
