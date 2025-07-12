using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000451 RID: 1105
	[Serializable]
	public class PlaceableStorageData : GridItemData
	{
		// Token: 0x06001688 RID: 5768 RVA: 0x000642BA File Offset: 0x000624BA
		public PlaceableStorageData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet contents) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Contents = contents;
		}

		// Token: 0x0400149C RID: 5276
		public ItemSet Contents;
	}
}
