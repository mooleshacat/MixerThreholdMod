using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000448 RID: 1096
	public class CauldronData : GridItemData
	{
		// Token: 0x0600167F RID: 5759 RVA: 0x000640E4 File Offset: 0x000622E4
		public CauldronData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet ingredients, ItemSet liquid, ItemSet output, int remainingCookTime, EQuality inputQuality) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Ingredients = ingredients;
			this.Liquid = liquid;
			this.Output = output;
			this.RemainingCookTime = remainingCookTime;
			this.InputQuality = inputQuality;
		}

		// Token: 0x0400147B RID: 5243
		public ItemSet Ingredients;

		// Token: 0x0400147C RID: 5244
		public ItemSet Liquid;

		// Token: 0x0400147D RID: 5245
		public ItemSet Output;

		// Token: 0x0400147E RID: 5246
		public int RemainingCookTime;

		// Token: 0x0400147F RID: 5247
		public EQuality InputQuality;
	}
}
