using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200044A RID: 1098
	public class DryingRackData : GridItemData
	{
		// Token: 0x06001681 RID: 5761 RVA: 0x00064174 File Offset: 0x00062374
		public DryingRackData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet input, ItemSet output, DryingOperation[] dryingOperations) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Input = input;
			this.Output = output;
			this.DryingOperations = dryingOperations;
		}

		// Token: 0x04001487 RID: 5255
		public ItemSet Input;

		// Token: 0x04001488 RID: 5256
		public ItemSet Output;

		// Token: 0x04001489 RID: 5257
		public DryingOperation[] DryingOperations;
	}
}
