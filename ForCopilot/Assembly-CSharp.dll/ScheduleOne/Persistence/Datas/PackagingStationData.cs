using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000450 RID: 1104
	[Serializable]
	public class PackagingStationData : GridItemData
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x000642A1 File Offset: 0x000624A1
		public PackagingStationData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet contents) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Contents = contents;
		}

		// Token: 0x0400149B RID: 5275
		public ItemSet Contents;
	}
}
