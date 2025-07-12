using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007F9 RID: 2041
	public class StartLoopStopAudio : MonoBehaviour
	{
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060036F7 RID: 14071 RVA: 0x000E7413 File Offset: 0x000E5613
		// (set) Token: 0x060036F8 RID: 14072 RVA: 0x000E741B File Offset: 0x000E561B
		public bool Runnning { get; private set; }

		// Token: 0x060036F9 RID: 14073 RVA: 0x000E7424 File Offset: 0x000E5624
		private void Update()
		{
			if (!this.Runnning)
			{
				this.timeSinceStop += Time.deltaTime;
				if (this.FadeLoopOut)
				{
					this.LoopSound.VolumeMultiplier = Mathf.Lerp(1f, 0f, this.timeSinceStop / this.StopSound.AudioSource.clip.length);
				}
				else
				{
					this.LoopSound.VolumeMultiplier = 0f;
				}
				if (this.LoopSound.isPlaying && this.LoopSound.VolumeMultiplier == 0f)
				{
					this.LoopSound.Stop();
				}
				return;
			}
			this.timeSinceStart += Time.deltaTime;
			if (this.FadeLoopIn)
			{
				this.LoopSound.VolumeMultiplier = Mathf.Lerp(0f, 1f, this.timeSinceStart / this.StartSound.AudioSource.clip.length);
				return;
			}
			this.LoopSound.VolumeMultiplier = 1f;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x000E7528 File Offset: 0x000E5728
		[Button]
		public void StartAudio()
		{
			if (this.Runnning)
			{
				return;
			}
			this.Runnning = true;
			this.timeSinceStart = 0f;
			this.LoopSound.Play();
			this.LoopSound.AudioSource.loop = true;
			this.StartSound.Play();
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000E7577 File Offset: 0x000E5777
		[Button]
		public void StopAudio()
		{
			if (!this.Runnning)
			{
				return;
			}
			this.Runnning = false;
			this.timeSinceStop = 0f;
			this.StartSound.Stop();
			this.StopSound.Play();
		}

		// Token: 0x04002735 RID: 10037
		public AudioSourceController StartSound;

		// Token: 0x04002736 RID: 10038
		public AudioSourceController LoopSound;

		// Token: 0x04002737 RID: 10039
		public AudioSourceController StopSound;

		// Token: 0x04002738 RID: 10040
		public bool FadeLoopIn;

		// Token: 0x04002739 RID: 10041
		public bool FadeLoopOut;

		// Token: 0x0400273A RID: 10042
		private float timeSinceStart;

		// Token: 0x0400273B RID: 10043
		private float timeSinceStop;
	}
}
