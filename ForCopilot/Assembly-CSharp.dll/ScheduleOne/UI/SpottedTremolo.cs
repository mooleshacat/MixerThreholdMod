using System;
using ScheduleOne.Audio;
using ScheduleOne.Stealth;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009F6 RID: 2550
	public class SpottedTremolo : MonoBehaviour
	{
		// Token: 0x060044AD RID: 17581 RVA: 0x001205DC File Offset: 0x0011E7DC
		public void Update()
		{
			this.Intensity = ((this.PlayerVisibility.HighestVisionEvent != null) ? this.PlayerVisibility.HighestVisionEvent.NormalizedNoticeLevel : 0f);
			if (this.Intensity > this.smoothedIntensity)
			{
				this.smoothedIntensity = Mathf.MoveTowards(this.smoothedIntensity, this.Intensity, Time.deltaTime / this.SmoothTime);
			}
			else
			{
				this.smoothedIntensity = Mathf.MoveTowards(this.smoothedIntensity, this.Intensity, Time.deltaTime / 3f);
			}
			float num = Mathf.Lerp(this.MinVolume, this.MaxVolume, this.smoothedIntensity);
			this.Loop.VolumeMultiplier = num;
			this.Loop.PitchMultiplier = Mathf.Lerp(this.MinPitch, this.MaxPitch, this.smoothedIntensity);
			this.Loop.ApplyPitch();
			if (num > 0f && !this.Loop.isPlaying)
			{
				this.Loop.Play();
				return;
			}
			if (num <= 0f && this.Loop.isPlaying)
			{
				this.Loop.Stop();
			}
		}

		// Token: 0x04003181 RID: 12673
		[Range(0f, 1f)]
		public float Intensity;

		// Token: 0x04003182 RID: 12674
		public AudioSourceController Loop;

		// Token: 0x04003183 RID: 12675
		public PlayerVisibility PlayerVisibility;

		// Token: 0x04003184 RID: 12676
		[Header("Settings")]
		public float MinVolume;

		// Token: 0x04003185 RID: 12677
		public float MaxVolume = 1f;

		// Token: 0x04003186 RID: 12678
		public float MinPitch = 0.9f;

		// Token: 0x04003187 RID: 12679
		public float MaxPitch = 1.2f;

		// Token: 0x04003188 RID: 12680
		public float SmoothTime = 0.5f;

		// Token: 0x04003189 RID: 12681
		[Range(0f, 1f)]
		[SerializeField]
		private float smoothedIntensity;
	}
}
