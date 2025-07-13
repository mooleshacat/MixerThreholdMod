using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200045B RID: 1115
	[Serializable]
	public class PlantData : SaveData
	{
		// Token: 0x06001692 RID: 5778 RVA: 0x000643F1 File Offset: 0x000625F1
		public PlantData(string seedID, float growthProgress, float yieldLevel, float qualityLevel, int[] activeBuds)
		{
			this.SeedID = seedID;
			this.GrowthProgress = growthProgress;
			this.YieldLevel = yieldLevel;
			this.QualityLevel = qualityLevel;
			this.ActiveBuds = activeBuds;
		}

		// Token: 0x040014AF RID: 5295
		public string SeedID;

		// Token: 0x040014B0 RID: 5296
		public float GrowthProgress;

		// Token: 0x040014B1 RID: 5297
		public float YieldLevel;

		// Token: 0x040014B2 RID: 5298
		public float QualityLevel;

		// Token: 0x040014B3 RID: 5299
		public int[] ActiveBuds;
	}
}
