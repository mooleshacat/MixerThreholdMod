using System;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C1E RID: 3102
	public class ChemistryCookOperation
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06005463 RID: 21603 RVA: 0x00164A64 File Offset: 0x00162C64
		[CodegenExclude]
		public StationRecipe Recipe
		{
			get
			{
				if (this.recipe == null)
				{
					this.recipe = Singleton<ChemistryStationCanvas>.Instance.Recipes.Find((StationRecipe r) => r.RecipeID == this.RecipeID);
				}
				return this.recipe;
			}
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x00164A9B File Offset: 0x00162C9B
		public ChemistryCookOperation(StationRecipe recipe, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime = 0)
		{
			this.RecipeID = recipe.RecipeID;
			this.ProductQuality = productQuality;
			this.StartLiquidColor = startLiquidColor;
			this.LiquidLevel = liquidLevel;
			this.CurrentTime = currentTime;
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x00164ACD File Offset: 0x00162CCD
		public ChemistryCookOperation(string recipeID, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime = 0)
		{
			this.RecipeID = recipeID;
			this.ProductQuality = productQuality;
			this.StartLiquidColor = startLiquidColor;
			this.LiquidLevel = liquidLevel;
			this.CurrentTime = currentTime;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x0000494F File Offset: 0x00002B4F
		public ChemistryCookOperation()
		{
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x00164AFA File Offset: 0x00162CFA
		public void Progress(int mins)
		{
			this.CurrentTime += mins;
			int currentTime = this.CurrentTime;
			int cookTime_Mins = this.Recipe.CookTime_Mins;
		}

		// Token: 0x04003EDA RID: 16090
		[CodegenExclude]
		private StationRecipe recipe;

		// Token: 0x04003EDB RID: 16091
		public string RecipeID;

		// Token: 0x04003EDC RID: 16092
		public EQuality ProductQuality;

		// Token: 0x04003EDD RID: 16093
		public Color StartLiquidColor;

		// Token: 0x04003EDE RID: 16094
		public float LiquidLevel;

		// Token: 0x04003EDF RID: 16095
		public int CurrentTime;
	}
}
