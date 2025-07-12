using System;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000286 RID: 646
	[Serializable]
	public class VODatabaseEntry
	{
		// Token: 0x06000D82 RID: 3458 RVA: 0x0003B87C File Offset: 0x00039A7C
		public AudioClip GetRandomClip()
		{
			if (this.Clips.Length == 0)
			{
				return null;
			}
			AudioClip audioClip = this.Clips[UnityEngine.Random.Range(0, this.Clips.Length)];
			int num = 0;
			while (audioClip == this.lastClip && this.Clips.Length != 1 && num <= 5)
			{
				audioClip = this.Clips[UnityEngine.Random.Range(0, this.Clips.Length)];
				num++;
			}
			this.lastClip = audioClip;
			return audioClip;
		}

		// Token: 0x04000DE8 RID: 3560
		public EVOLineType LineType;

		// Token: 0x04000DE9 RID: 3561
		public AudioClip[] Clips;

		// Token: 0x04000DEA RID: 3562
		private AudioClip lastClip;

		// Token: 0x04000DEB RID: 3563
		public float VolumeMultiplier = 1f;
	}
}
