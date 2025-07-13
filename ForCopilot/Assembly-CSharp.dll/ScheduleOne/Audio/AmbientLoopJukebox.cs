using System;
using System.Collections.Generic;
using GameKit.Utilities;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007D8 RID: 2008
	[RequireComponent(typeof(AudioSourceController))]
	public class AmbientLoopJukebox : MonoBehaviour
	{
		// Token: 0x06003653 RID: 13907 RVA: 0x000E4DB4 File Offset: 0x000E2FB4
		private void Start()
		{
			this.audioSourceController = base.GetComponent<AudioSourceController>();
			this.audioSourceController.Play();
			Arrays.Shuffle<AudioClip>(this.Clips);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000E4E09 File Offset: 0x000E3009
		private void Update()
		{
			this.musicScale = Singleton<AudioManager>.Instance.GetScaledMusicVolumeMultiplier(0.3f);
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000E4E20 File Offset: 0x000E3020
		private void MinPass()
		{
			float num = this.VolumeCurve.Evaluate((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f);
			this.audioSourceController.VolumeMultiplier = num * this.musicScale;
			if (!this.audioSourceController.isPlaying)
			{
				this.currentClipIndex = (this.currentClipIndex + 1) % this.Clips.Count;
				this.audioSourceController.AudioSource.clip = this.Clips[this.currentClipIndex];
				this.audioSourceController.Play();
			}
		}

		// Token: 0x04002684 RID: 9860
		public AnimationCurve VolumeCurve;

		// Token: 0x04002685 RID: 9861
		public List<AudioClip> Clips = new List<AudioClip>();

		// Token: 0x04002686 RID: 9862
		private AudioSourceController audioSourceController;

		// Token: 0x04002687 RID: 9863
		private int currentClipIndex;

		// Token: 0x04002688 RID: 9864
		private float musicScale = 1f;
	}
}
