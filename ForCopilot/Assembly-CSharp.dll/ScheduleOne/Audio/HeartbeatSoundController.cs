using System;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007EB RID: 2027
	public class HeartbeatSoundController : MonoBehaviour
	{
		// Token: 0x060036BE RID: 14014 RVA: 0x000E6822 File Offset: 0x000E4A22
		private void Awake()
		{
			this.VolumeController.Initialize();
			this.VolumeController.SetDefault(0f);
			this.PitchController.Initialize();
			this.PitchController.SetDefault(1f);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000E685C File Offset: 0x000E4A5C
		private void Update()
		{
			this.sound.VolumeMultiplier = this.VolumeController.CurrentValue;
			this.sound.PitchMultiplier = this.PitchController.CurrentValue;
			this.sound.ApplyPitch();
			if (this.sound.VolumeMultiplier > 0f)
			{
				if (!this.sound.isPlaying)
				{
					this.sound.Play();
					return;
				}
			}
			else if (this.sound.isPlaying)
			{
				this.sound.Stop();
			}
		}

		// Token: 0x040026FE RID: 9982
		public AudioSourceController sound;

		// Token: 0x040026FF RID: 9983
		public FloatSmoother VolumeController;

		// Token: 0x04002700 RID: 9984
		public FloatSmoother PitchController;
	}
}
