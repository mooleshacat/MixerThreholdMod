using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005EC RID: 1516
	public class RankData : SaveData
	{
		// Token: 0x0600254C RID: 9548 RVA: 0x00097AFD File Offset: 0x00095CFD
		public RankData(int rank, int tier, int xp, int totalXP)
		{
			this.Rank = rank;
			this.Tier = tier;
			this.XP = xp;
			this.TotalXP = totalXP;
		}

		// Token: 0x04001B8C RID: 7052
		public int Rank;

		// Token: 0x04001B8D RID: 7053
		public int Tier;

		// Token: 0x04001B8E RID: 7054
		public int XP;

		// Token: 0x04001B8F RID: 7055
		public int TotalXP;
	}
}
