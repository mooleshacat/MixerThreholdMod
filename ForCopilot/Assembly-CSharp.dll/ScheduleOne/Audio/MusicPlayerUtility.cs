using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007F1 RID: 2033
	public class MusicPlayerUtility : MonoBehaviour
	{
		// Token: 0x060036D5 RID: 14037 RVA: 0x000E6DFB File Offset: 0x000E4FFB
		public void PlayTrack(string trackName)
		{
			Singleton<MusicPlayer>.Instance.SetTrackEnabled(trackName, true);
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x000E6E09 File Offset: 0x000E5009
		public void StopTracks()
		{
			Singleton<MusicPlayer>.Instance.StopAndDisableTracks();
		}
	}
}
