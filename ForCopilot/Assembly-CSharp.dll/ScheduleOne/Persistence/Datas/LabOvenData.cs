using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200044E RID: 1102
	public class LabOvenData : GridItemData
	{
		// Token: 0x06001685 RID: 5765 RVA: 0x00064214 File Offset: 0x00062414
		public LabOvenData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet inputContents, ItemSet outputContents, string ingredientID, int currentIngredientQuantity, EQuality ingredientQuality, string productID, int currentCookProgress) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.InputContents = inputContents;
			this.OutputContents = outputContents;
			this.CurrentIngredientID = ingredientID;
			this.CurrentIngredientQuantity = currentIngredientQuantity;
			this.CurrentIngredientQuality = ingredientQuality;
			this.CurrentProductID = productID;
			this.CurrentCookProgress = currentCookProgress;
		}

		// Token: 0x0400148F RID: 5263
		public ItemSet InputContents;

		// Token: 0x04001490 RID: 5264
		public ItemSet OutputContents;

		// Token: 0x04001491 RID: 5265
		public string CurrentIngredientID;

		// Token: 0x04001492 RID: 5266
		public int CurrentIngredientQuantity;

		// Token: 0x04001493 RID: 5267
		public EQuality CurrentIngredientQuality;

		// Token: 0x04001494 RID: 5268
		public string CurrentProductID;

		// Token: 0x04001495 RID: 5269
		public int CurrentCookProgress;
	}
}
