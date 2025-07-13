using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Audio;

namespace ScheduleOne.Audio
{
	// Token: 0x020007EE RID: 2030
	public class MusicPlayer : PersistentSingleton<MusicPlayer>
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060036C5 RID: 14021 RVA: 0x000E6A79 File Offset: 0x000E4C79
		public bool IsPlaying
		{
			get
			{
				return this._currentTrack != null && this._currentTrack.IsPlaying;
			}
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000E6A98 File Offset: 0x000E4C98
		public void OnValidate()
		{
			this.Tracks = new List<MusicTrack>(base.GetComponentsInChildren<MusicTrack>());
			for (int i = 0; i < this.Tracks.Count - 1; i++)
			{
				if (this.Tracks[i].Priority > this.Tracks[i + 1].Priority)
				{
					this.Tracks[i].transform.SetSiblingIndex(i + 1);
				}
			}
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x000E6B10 File Offset: 0x000E4D10
		protected override void Start()
		{
			base.Start();
			if (Singleton<MusicPlayer>.Instance == null || Singleton<MusicPlayer>.Instance != this)
			{
				return;
			}
			base.InvokeRepeating("UpdateTracks", 0f, 0.2f);
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				this.SetMusicDistorted(false, 0.5f);
			});
			this.DefaultSnapshot.TransitionTo(0.1f);
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000E6B7E File Offset: 0x000E4D7E
		private void Update()
		{
			MusicPlayer.TimeSinceLastAmbientTrack += Time.unscaledDeltaTime;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000E6B90 File Offset: 0x000E4D90
		public void SetMusicDistorted(bool distorted, float transition = 5f)
		{
			if (distorted)
			{
				this.DistortedSnapshot.TransitionTo(transition);
				return;
			}
			this.DefaultSnapshot.TransitionTo(transition);
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000E6BB0 File Offset: 0x000E4DB0
		public void SetTrackEnabled(string trackName, bool enabled)
		{
			MusicTrack musicTrack = this.Tracks.Find((MusicTrack t) => t.TrackName == trackName);
			if (musicTrack == null)
			{
				Console.LogWarning("Music track not found: " + trackName, null);
				return;
			}
			if (enabled)
			{
				musicTrack.Enable();
				return;
			}
			musicTrack.Disable();
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000E6C14 File Offset: 0x000E4E14
		public void StopTrack(string trackName)
		{
			MusicTrack musicTrack = this.Tracks.Find((MusicTrack t) => t.TrackName == trackName);
			if (musicTrack == null)
			{
				Console.LogWarning("Music track not found: " + trackName, null);
				return;
			}
			musicTrack.Stop();
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000E6C6C File Offset: 0x000E4E6C
		public void StopAndDisableTracks()
		{
			foreach (MusicTrack musicTrack in this.Tracks)
			{
				musicTrack.Disable();
				musicTrack.Stop();
			}
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000E6CC4 File Offset: 0x000E4EC4
		private void UpdateTracks()
		{
			if (this._currentTrack != null && !this._currentTrack.IsPlaying)
			{
				this._currentTrack = null;
			}
			MusicTrack musicTrack = null;
			foreach (MusicTrack musicTrack2 in this.Tracks)
			{
				if (musicTrack2.Enabled && (musicTrack == null || musicTrack2.Priority > musicTrack.Priority))
				{
					musicTrack = musicTrack2;
				}
			}
			if (this._currentTrack != musicTrack && musicTrack != null)
			{
				if (this._currentTrack != null)
				{
					this._currentTrack.Stop();
				}
				this._currentTrack = musicTrack;
				if (this._currentTrack != null)
				{
					this._currentTrack.Play();
				}
			}
		}

		// Token: 0x04002711 RID: 10001
		public static float TimeSinceLastAmbientTrack = 100000f;

		// Token: 0x04002712 RID: 10002
		public List<MusicTrack> Tracks = new List<MusicTrack>();

		// Token: 0x04002713 RID: 10003
		public AudioMixerGroup MusicMixer;

		// Token: 0x04002714 RID: 10004
		public AudioMixerSnapshot DefaultSnapshot;

		// Token: 0x04002715 RID: 10005
		public AudioMixerSnapshot DistortedSnapshot;

		// Token: 0x04002716 RID: 10006
		private MusicTrack _currentTrack;
	}
}
