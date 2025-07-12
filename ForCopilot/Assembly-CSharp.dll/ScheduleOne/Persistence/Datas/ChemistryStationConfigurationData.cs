using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000428 RID: 1064
	[Serializable]
	public class ChemistryStationConfigurationData : SaveData
	{
		// Token: 0x0600165E RID: 5726 RVA: 0x00063C95 File Offset: 0x00061E95
		public ChemistryStationConfigurationData(StationRecipeFieldData recipe, ObjectFieldData destination)
		{
			this.Recipe = recipe;
			this.Destination = destination;
		}

		// Token: 0x0400142D RID: 5165
		public StationRecipeFieldData Recipe;

		// Token: 0x0400142E RID: 5166
		public ObjectFieldData Destination;
	}
}
