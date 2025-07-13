using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000446 RID: 1094
	[Serializable]
	public class BrickPressData : GridItemData
	{
		// Token: 0x0600167D RID: 5757 RVA: 0x00064097 File Offset: 0x00062297
		public BrickPressData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet contents) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Contents = contents;
		}

		// Token: 0x04001477 RID: 5239
		public ItemSet Contents;
	}
}
