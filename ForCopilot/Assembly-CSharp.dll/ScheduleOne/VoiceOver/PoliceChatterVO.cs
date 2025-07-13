using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000283 RID: 643
	public class PoliceChatterVO : VOEmitter
	{
		// Token: 0x06000D73 RID: 3443 RVA: 0x0003B67B File Offset: 0x0003987B
		public override void Play(EVOLineType lineType)
		{
			if (lineType == EVOLineType.PoliceChatter)
			{
				this.PlayChatter();
				return;
			}
			base.Play(lineType);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0003B690 File Offset: 0x00039890
		private void PlayChatter()
		{
			if (this.chatterRoutine != null)
			{
				base.StopCoroutine(this.chatterRoutine);
			}
			this.chatterRoutine = base.StartCoroutine(this.<PlayChatter>g__Play|5_0());
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0003B6C0 File Offset: 0x000398C0
		[CompilerGenerated]
		private IEnumerator <PlayChatter>g__Play|5_0()
		{
			this.StartBeep.Play();
			this.Static.Play();
			yield return new WaitForSeconds(0.25f);
			base.Play(EVOLineType.PoliceChatter);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => !this.audioSourceController.isPlaying);
			this.StartEndBeep.Play();
			this.Static.Stop();
			this.chatterRoutine = null;
			yield break;
		}

		// Token: 0x04000DDF RID: 3551
		public AudioSourceController StartBeep;

		// Token: 0x04000DE0 RID: 3552
		public AudioSourceController StartEndBeep;

		// Token: 0x04000DE1 RID: 3553
		public AudioSourceController Static;

		// Token: 0x04000DE2 RID: 3554
		private Coroutine chatterRoutine;
	}
}
