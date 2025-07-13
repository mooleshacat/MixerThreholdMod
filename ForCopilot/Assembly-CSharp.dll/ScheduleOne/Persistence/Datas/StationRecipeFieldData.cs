using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000437 RID: 1079
	[Serializable]
	public class StationRecipeFieldData
	{
		// Token: 0x0600166D RID: 5741 RVA: 0x00063DBE File Offset: 0x00061FBE
		public StationRecipeFieldData(string recipeID)
		{
			this.RecipeID = recipeID;
		}

		// Token: 0x04001446 RID: 5190
		public string RecipeID;
	}
}
