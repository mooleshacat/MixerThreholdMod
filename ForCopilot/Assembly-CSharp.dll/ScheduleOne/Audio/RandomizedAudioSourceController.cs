using System;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007F3 RID: 2035
	public class RandomizedAudioSourceController : AudioSourceController
	{
		// Token: 0x060036E2 RID: 14050 RVA: 0x000E6FE4 File Offset: 0x000E51E4
		public override void Play()
		{
			if (this.Clips.Length == 0)
			{
				Console.LogWarning("RandomizedAudioSourceController: No clips to play", null);
				return;
			}
			int num = UnityEngine.Random.Range(0, this.Clips.Length);
			this.AudioSource.clip = this.Clips[num];
			base.Play();
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000E7030 File Offset: 0x000E5230
		public override void PlayOneShot(bool duplicateAudioSource = false)
		{
			if (this.Clips.Length == 0)
			{
				Console.LogWarning("RandomizedAudioSourceController: No clips to play", null);
				return;
			}
			int num = UnityEngine.Random.Range(0, this.Clips.Length);
			this.AudioSource.clip = this.Clips[num];
			base.PlayOneShot(duplicateAudioSource);
		}

		// Token: 0x04002723 RID: 10019
		public AudioClip[] Clips;
	}
}
