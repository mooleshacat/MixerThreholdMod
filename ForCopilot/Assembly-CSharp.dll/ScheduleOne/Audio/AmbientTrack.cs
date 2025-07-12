using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007DB RID: 2011
	public class AmbientTrack : MonoBehaviour
	{
		// Token: 0x0600365C RID: 13916 RVA: 0x000E505C File Offset: 0x000E325C
		private void Awake()
		{
			for (int i = 0; i < this.Tracks.Count; i++)
			{
				int index = UnityEngine.Random.Range(i, this.Tracks.Count);
				MusicTrack value = this.Tracks[index];
				this.Tracks[index] = this.Tracks[i];
				this.Tracks[i] = value;
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000E50C4 File Offset: 0x000E32C4
		[Button]
		public void ForcePlay()
		{
			AmbientTrack.LastPlayedTrack = this;
			MusicPlayer.TimeSinceLastAmbientTrack = 0f;
			this.playTrack = false;
			AmbientTrack.TrackQueued = false;
			this.Tracks[0].Enable();
			this.Tracks.Add(this.Tracks[0]);
			this.Tracks.RemoveAt(0);
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000E5122 File Offset: 0x000E3322
		public void Stop()
		{
			this.Tracks[0].Disable();
			this.Tracks[0].Stop();
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000E5148 File Offset: 0x000E3348
		private void Update()
		{
			if (!NetworkSingleton<TimeManager>.InstanceExists)
			{
				this.trackRandomized = false;
				AmbientTrack.TrackQueued = false;
				return;
			}
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.MinTime, this.MaxTime))
			{
				if (!this.trackRandomized)
				{
					this.playTrack = (UnityEngine.Random.value < this.Chance && MusicPlayer.TimeSinceLastAmbientTrack > 540f && AmbientTrack.LastPlayedTrack != this && !AmbientTrack.TrackQueued && this.Tracks.Count > 0 && Time.timeSinceLevelLoad > 20f && !GameManager.IS_TUTORIAL && this.CanStartAmbientTrack());
					this.startTime = TimeManager.AddMinutesTo24HourTime(currentTime, UnityEngine.Random.Range(0, 120));
					if (this.playTrack)
					{
						Console.Log("Will play " + this.Tracks[0].TrackName + " at " + this.startTime.ToString(), null);
						AmbientTrack.TrackQueued = true;
						MusicPlayer.TimeSinceLastAmbientTrack = 0f;
					}
					this.trackRandomized = true;
				}
				if (this.playTrack && !this.Tracks[0].Enabled && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.startTime, this.MaxTime))
				{
					AmbientTrack.LastPlayedTrack = this;
					MusicPlayer.TimeSinceLastAmbientTrack = 0f;
					this.playTrack = false;
					AmbientTrack.TrackQueued = false;
					this.Tracks[0].Enable();
					this.Tracks.Add(this.Tracks[0]);
					this.Tracks.RemoveAt(0);
					return;
				}
			}
			else
			{
				this.trackRandomized = false;
				this.playTrack = false;
				foreach (MusicTrack musicTrack in this.Tracks)
				{
					musicTrack.Disable();
				}
			}
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000E533C File Offset: 0x000E353C
		protected virtual bool CanStartAmbientTrack()
		{
			if (Player.Local.CurrentProperty != null)
			{
				foreach (Jukebox jukebox in Player.Local.CurrentProperty.GetBuildablesOfType<Jukebox>())
				{
					if (jukebox.IsPlaying && jukebox.CurrentVolume > 0)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x04002695 RID: 9877
		public const float MIN_TIME_BETWEEN_AMBIENT_TRACKS = 540f;

		// Token: 0x04002696 RID: 9878
		public static AmbientTrack LastPlayedTrack;

		// Token: 0x04002697 RID: 9879
		public static bool TrackQueued;

		// Token: 0x04002698 RID: 9880
		public List<MusicTrack> Tracks = new List<MusicTrack>();

		// Token: 0x04002699 RID: 9881
		public int MinTime;

		// Token: 0x0400269A RID: 9882
		public int MaxTime;

		// Token: 0x0400269B RID: 9883
		public float Chance = 0.3f;

		// Token: 0x0400269C RID: 9884
		private int startTime;

		// Token: 0x0400269D RID: 9885
		private bool playTrack;

		// Token: 0x0400269E RID: 9886
		private bool trackRandomized;
	}
}
