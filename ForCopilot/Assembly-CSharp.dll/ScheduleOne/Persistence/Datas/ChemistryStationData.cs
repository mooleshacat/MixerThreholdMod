using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000449 RID: 1097
	public class ChemistryStationData : GridItemData
	{
		// Token: 0x06001680 RID: 5760 RVA: 0x00064120 File Offset: 0x00062320
		public ChemistryStationData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet inputContents, ItemSet outputContents, string currentRecipeID, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.InputContents = inputContents;
			this.OutputContents = outputContents;
			this.CurrentRecipeID = currentRecipeID;
			this.ProductQuality = productQuality;
			this.StartLiquidColor = startLiquidColor;
			this.LiquidLevel = liquidLevel;
			this.CurrentTime = currentTime;
		}

		// Token: 0x04001480 RID: 5248
		public ItemSet InputContents;

		// Token: 0x04001481 RID: 5249
		public ItemSet OutputContents;

		// Token: 0x04001482 RID: 5250
		public string CurrentRecipeID;

		// Token: 0x04001483 RID: 5251
		public EQuality ProductQuality;

		// Token: 0x04001484 RID: 5252
		public Color StartLiquidColor;

		// Token: 0x04001485 RID: 5253
		public float LiquidLevel;

		// Token: 0x04001486 RID: 5254
		public int CurrentTime;
	}
}
