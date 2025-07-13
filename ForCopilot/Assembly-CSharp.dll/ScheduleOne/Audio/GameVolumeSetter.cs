using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007EA RID: 2026
	public class GameVolumeSetter : MonoBehaviour
	{
		// Token: 0x060036BC RID: 14012 RVA: 0x000E67FD File Offset: 0x000E49FD
		private void Update()
		{
			Singleton<AudioManager>.Instance.SetGameVolumeMultipler(this.VolumeMultiplier);
		}

		// Token: 0x040026FD RID: 9981
		[Range(0f, 1f)]
		public float VolumeMultiplier = 1f;
	}
}
