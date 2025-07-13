using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Audio
{
	// Token: 0x020007DD RID: 2013
	[RequireComponent(typeof(AudioSource))]
	public class AudioSourceController : MonoBehaviour
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x000E57F2 File Offset: 0x000E39F2
		// (set) Token: 0x0600367E RID: 13950 RVA: 0x000E57FA File Offset: 0x000E39FA
		public float Volume { get; protected set; } = 1f;

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x000E5803 File Offset: 0x000E3A03
		public bool isPlaying
		{
			get
			{
				return this.AudioSource.isPlaying;
			}
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x000E5810 File Offset: 0x000E3A10
		private void Awake()
		{
			this.DoPauseStuff();
			this.basePitch = this.AudioSource.pitch;
			this.AudioSource.volume = 0f;
			if (this.AudioSource.playOnAwake)
			{
				this.isPlayingCached = true;
			}
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000E5850 File Offset: 0x000E3A50
		private void Start()
		{
			this.SetVolume(this.DefaultVolume);
			Singleton<AudioManager>.Instance.onSettingsChanged.AddListener(new UnityAction(this.ApplyVolume));
			if (this.AudioType == EAudioType.Music)
			{
				this.AudioSource.outputAudioMixerGroup = Singleton<AudioManager>.Instance.MusicMixer;
				return;
			}
			if (SceneManager.GetActiveScene().name == "Main")
			{
				this.AudioSource.outputAudioMixerGroup = Singleton<AudioManager>.Instance.MainGameMixer;
				return;
			}
			this.AudioSource.outputAudioMixerGroup = Singleton<AudioManager>.Instance.MenuMixer;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000E58E8 File Offset: 0x000E3AE8
		private void DoPauseStuff()
		{
			if (Singleton<PauseMenu>.InstanceExists)
			{
				Singleton<PauseMenu>.Instance.onPause.RemoveListener(new UnityAction(this.Pause));
				Singleton<PauseMenu>.Instance.onPause.AddListener(new UnityAction(this.Pause));
				Singleton<PauseMenu>.Instance.onResume.RemoveListener(new UnityAction(this.Unpause));
				Singleton<PauseMenu>.Instance.onResume.AddListener(new UnityAction(this.Pause));
			}
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000E5968 File Offset: 0x000E3B68
		private void OnDestroy()
		{
			if (Singleton<AudioManager>.Instance != null)
			{
				Singleton<AudioManager>.Instance.onSettingsChanged.RemoveListener(new UnityAction(this.ApplyVolume));
			}
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x000E5992 File Offset: 0x000E3B92
		private void OnValidate()
		{
			if (this.AudioSource == null)
			{
				this.AudioSource = base.GetComponent<AudioSource>();
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x000E59AE File Offset: 0x000E3BAE
		private void FixedUpdate()
		{
			if (this.isPlayingCached)
			{
				this.ApplyVolume();
				if (!this.AudioSource.isPlaying && !this.paused)
				{
					this.isPlayingCached = false;
				}
			}
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x000E59DA File Offset: 0x000E3BDA
		private void Pause()
		{
			this.paused = true;
			this.AudioSource.Pause();
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x000E59EE File Offset: 0x000E3BEE
		private void Unpause()
		{
			this.paused = false;
			this.AudioSource.UnPause();
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x000E5A02 File Offset: 0x000E3C02
		public void SetVolume(float volume)
		{
			this.Volume = volume;
			this.ApplyVolume();
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x000E5A14 File Offset: 0x000E3C14
		public void ApplyVolume()
		{
			if (!Singleton<AudioManager>.InstanceExists)
			{
				return;
			}
			if (this.DEBUG)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Applying volume: ",
					this.Volume.ToString(),
					" * ",
					Singleton<AudioManager>.Instance.GetVolume(this.AudioType, true).ToString(),
					" * ",
					this.VolumeMultiplier.ToString()
				}));
			}
			this.AudioSource.volume = this.Volume * Singleton<AudioManager>.Instance.GetVolume(this.AudioType, true) * this.VolumeMultiplier;
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000E5AC0 File Offset: 0x000E3CC0
		public void ApplyPitch()
		{
			if (this.RandomizePitch)
			{
				this.AudioSource.pitch = UnityEngine.Random.Range(this.MinPitch, this.MaxPitch) * this.PitchMultiplier;
				return;
			}
			this.AudioSource.pitch = this.basePitch * this.PitchMultiplier;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000E5B11 File Offset: 0x000E3D11
		public virtual void Play()
		{
			this.ApplyPitch();
			this.ApplyVolume();
			this.isPlayingCached = true;
			this.AudioSource.Play();
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000E5B34 File Offset: 0x000E3D34
		public virtual void PlayOneShot(bool duplicateAudioSource = false)
		{
			if (this.RandomizePitch)
			{
				this.AudioSource.pitch = UnityEngine.Random.Range(this.MinPitch, this.MaxPitch) * this.PitchMultiplier;
			}
			this.ApplyVolume();
			if (!duplicateAudioSource)
			{
				this.AudioSource.PlayOneShot(this.AudioSource.clip, 1f);
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, NetworkSingleton<GameManager>.Instance.Temp);
			gameObject.transform.position = base.transform.position;
			gameObject.GetComponent<AudioSourceController>().PlayOneShot(false);
			if (this.AudioSource.clip != null)
			{
				UnityEngine.Object.Destroy(gameObject, this.AudioSource.clip.length + 0.1f);
				return;
			}
			UnityEngine.Object.Destroy(gameObject, 5f);
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000E5C04 File Offset: 0x000E3E04
		public void Stop()
		{
			this.AudioSource.Stop();
		}

		// Token: 0x040026B5 RID: 9909
		public bool DEBUG;

		// Token: 0x040026B7 RID: 9911
		public AudioSource AudioSource;

		// Token: 0x040026B8 RID: 9912
		[Header("Settings")]
		public EAudioType AudioType;

		// Token: 0x040026B9 RID: 9913
		[Range(0f, 1f)]
		public float DefaultVolume = 1f;

		// Token: 0x040026BA RID: 9914
		public bool RandomizePitch;

		// Token: 0x040026BB RID: 9915
		public float MinPitch = 0.9f;

		// Token: 0x040026BC RID: 9916
		public float MaxPitch = 1.1f;

		// Token: 0x040026BD RID: 9917
		[Range(0f, 2f)]
		public float VolumeMultiplier = 1f;

		// Token: 0x040026BE RID: 9918
		[Range(0f, 2f)]
		public float PitchMultiplier = 1f;

		// Token: 0x040026BF RID: 9919
		private bool paused;

		// Token: 0x040026C0 RID: 9920
		private bool isPlayingCached;

		// Token: 0x040026C1 RID: 9921
		private float basePitch = 1f;
	}
}
