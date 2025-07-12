using System;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007F2 RID: 2034
	[RequireComponent(typeof(AudioSourceController))]
	public class MusicTrack : MonoBehaviour
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060036D8 RID: 14040 RVA: 0x000E6E15 File Offset: 0x000E5015
		// (set) Token: 0x060036D9 RID: 14041 RVA: 0x000E6E1D File Offset: 0x000E501D
		public bool IsPlaying { get; private set; }

		// Token: 0x060036DA RID: 14042 RVA: 0x000E6E26 File Offset: 0x000E5026
		private void OnValidate()
		{
			base.gameObject.name = this.TrackName + " (" + this.Priority.ToString() + ")";
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000E6E53 File Offset: 0x000E5053
		public void Enable()
		{
			this.Enabled = true;
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000E6E5C File Offset: 0x000E505C
		public void Disable()
		{
			this.Enabled = false;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000E6E65 File Offset: 0x000E5065
		protected virtual void Awake()
		{
			this.volumeMultiplier = 0f;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000E6E74 File Offset: 0x000E5074
		public virtual void Update()
		{
			if (this.IsPlaying && this.Controller.AudioSource.time >= this.Controller.AudioSource.clip.length - this.FadeOutTime && this.AutoFadeOut)
			{
				this.Stop();
				this.Disable();
			}
			if (this.IsPlaying)
			{
				this.volumeMultiplier = Mathf.Min(this.volumeMultiplier + Time.deltaTime / this.FadeInTime, 1f);
				this.Controller.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
				return;
			}
			this.volumeMultiplier = Mathf.Max(this.volumeMultiplier - Time.deltaTime / this.FadeOutTime, 0f);
			this.Controller.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
			if (this.Controller.VolumeMultiplier == 0f)
			{
				this.Controller.AudioSource.Stop();
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000E6F6C File Offset: 0x000E516C
		public virtual void Play()
		{
			this.IsPlaying = true;
			this.Controller.Play();
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000E6F80 File Offset: 0x000E5180
		public virtual void Stop()
		{
			this.IsPlaying = false;
		}

		// Token: 0x0400271A RID: 10010
		public bool Enabled;

		// Token: 0x0400271B RID: 10011
		public string TrackName = "Track";

		// Token: 0x0400271C RID: 10012
		public int Priority = 1;

		// Token: 0x0400271D RID: 10013
		public float FadeInTime = 1f;

		// Token: 0x0400271E RID: 10014
		public float FadeOutTime = 2f;

		// Token: 0x0400271F RID: 10015
		public AudioSourceController Controller;

		// Token: 0x04002720 RID: 10016
		public float VolumeMultiplier = 1f;

		// Token: 0x04002721 RID: 10017
		public bool AutoFadeOut = true;

		// Token: 0x04002722 RID: 10018
		protected float volumeMultiplier = 1f;
	}
}
