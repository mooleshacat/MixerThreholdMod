using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200070C RID: 1804
	[Serializable]
	public class VocalReactionDatabase
	{
		// Token: 0x06003100 RID: 12544 RVA: 0x000CCD18 File Offset: 0x000CAF18
		public VocalReactionDatabase.Entry GetEntry(string key)
		{
			foreach (VocalReactionDatabase.Entry entry in this.Entries)
			{
				if (entry.Key == key)
				{
					return entry;
				}
			}
			return null;
		}

		// Token: 0x0400226D RID: 8813
		public List<VocalReactionDatabase.Entry> Entries = new List<VocalReactionDatabase.Entry>();

		// Token: 0x0200070D RID: 1805
		[Serializable]
		public class Entry
		{
			// Token: 0x17000706 RID: 1798
			// (get) Token: 0x06003102 RID: 12546 RVA: 0x000CCD8F File Offset: 0x000CAF8F
			public string name
			{
				get
				{
					return this.Key;
				}
			}

			// Token: 0x06003103 RID: 12547 RVA: 0x000CCD97 File Offset: 0x000CAF97
			public string GetRandomReaction()
			{
				return this.Reactions[UnityEngine.Random.Range(0, this.Reactions.Length)];
			}

			// Token: 0x0400226E RID: 8814
			public string Key;

			// Token: 0x0400226F RID: 8815
			public string[] Reactions;
		}
	}
}
