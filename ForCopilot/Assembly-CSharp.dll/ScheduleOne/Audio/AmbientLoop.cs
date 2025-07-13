using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007D7 RID: 2007
	[RequireComponent(typeof(AudioSourceController))]
	public class AmbientLoop : MonoBehaviour
	{
		// Token: 0x06003650 RID: 13904 RVA: 0x000E4D15 File Offset: 0x000E2F15
		private void Start()
		{
			this.audioSourceController = base.GetComponent<AudioSourceController>();
			this.audioSourceController.Play();
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000E4D30 File Offset: 0x000E2F30
		private void Update()
		{
			if (this.FadeDuringMusic)
			{
				this.musicScale = Singleton<AudioManager>.Instance.GetScaledMusicVolumeMultiplier(0.3f);
			}
			else
			{
				this.musicScale = 1f;
			}
			float num = this.VolumeCurve.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			this.audioSourceController.VolumeMultiplier = num * this.musicScale;
		}

		// Token: 0x0400267F RID: 9855
		public const float MUSIC_FADE_MULTIPLIER = 0.3f;

		// Token: 0x04002680 RID: 9856
		public AnimationCurve VolumeCurve;

		// Token: 0x04002681 RID: 9857
		public bool FadeDuringMusic = true;

		// Token: 0x04002682 RID: 9858
		private AudioSourceController audioSourceController;

		// Token: 0x04002683 RID: 9859
		private float musicScale = 1f;
	}
}
