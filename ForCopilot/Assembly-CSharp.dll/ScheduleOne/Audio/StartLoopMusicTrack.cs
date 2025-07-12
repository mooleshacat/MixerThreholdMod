using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007F7 RID: 2039
	public class StartLoopMusicTrack : MusicTrack
	{
		// Token: 0x060036EC RID: 14060 RVA: 0x000E7270 File Offset: 0x000E5470
		protected override void Awake()
		{
			base.Awake();
			this.AutoFadeOut = false;
			this.LoopSound.AudioSource.loop = true;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000E7290 File Offset: 0x000E5490
		public override void Update()
		{
			base.Update();
			if (base.IsPlaying)
			{
				if (!this.Controller.AudioSource.isPlaying && !this.LoopSound.isPlaying)
				{
					this.LoopSound.Play();
				}
				this.LoopSound.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
				return;
			}
			this.LoopSound.VolumeMultiplier = this.volumeMultiplier * this.VolumeMultiplier;
			if (this.LoopSound.VolumeMultiplier == 0f)
			{
				this.LoopSound.AudioSource.Stop();
			}
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000E7328 File Offset: 0x000E5528
		public override void Play()
		{
			base.Play();
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Play>g__WaitForStart|3_0());
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x000E7349 File Offset: 0x000E5549
		[CompilerGenerated]
		private IEnumerator <Play>g__WaitForStart|3_0()
		{
			while (base.IsPlaying)
			{
				if (this.Controller.AudioSource.clip.length - this.Controller.AudioSource.time <= Time.deltaTime)
				{
					Console.Log("Starting loop for " + this.TrackName, null);
					this.LoopSound.Play();
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x04002730 RID: 10032
		public AudioSourceController LoopSound;
	}
}
