using System;
using UnityEngine;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005EB RID: 1515
	public class Unlockable
	{
		// Token: 0x0600254B RID: 9547 RVA: 0x00097AE0 File Offset: 0x00095CE0
		public Unlockable(FullRank rank, string title, Sprite icon)
		{
			this.Rank = rank;
			this.Title = title;
			this.Icon = icon;
		}

		// Token: 0x04001B89 RID: 7049
		public FullRank Rank;

		// Token: 0x04001B8A RID: 7050
		public string Title;

		// Token: 0x04001B8B RID: 7051
		public Sprite Icon;
	}
}
