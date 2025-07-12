using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.VoiceOver
{
	// Token: 0x02000285 RID: 645
	[CreateAssetMenu(fileName = "VODatabase", menuName = "ScriptableObjects/VODatabase")]
	[Serializable]
	public class VODatabase : ScriptableObject
	{
		// Token: 0x06000D7F RID: 3455 RVA: 0x0003B7E0 File Offset: 0x000399E0
		public VODatabaseEntry GetEntry(EVOLineType lineType)
		{
			foreach (VODatabaseEntry vodatabaseEntry in this.Entries)
			{
				if (vodatabaseEntry.LineType == lineType)
				{
					return vodatabaseEntry;
				}
			}
			return null;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003B83C File Offset: 0x00039A3C
		public AudioClip GetRandomClip(EVOLineType lineType)
		{
			VODatabaseEntry entry = this.GetEntry(lineType);
			if (entry != null)
			{
				return entry.GetRandomClip();
			}
			return null;
		}

		// Token: 0x04000DE6 RID: 3558
		[Range(0f, 2f)]
		public float VolumeMultiplier = 1f;

		// Token: 0x04000DE7 RID: 3559
		public List<VODatabaseEntry> Entries = new List<VODatabaseEntry>();
	}
}
