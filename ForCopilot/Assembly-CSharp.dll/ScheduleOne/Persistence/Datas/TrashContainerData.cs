using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000459 RID: 1113
	public class TrashContainerData : GridItemData
	{
		// Token: 0x06001690 RID: 5776 RVA: 0x000643C2 File Offset: 0x000625C2
		public TrashContainerData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, TrashContentData contentData) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.ContentData = contentData;
		}

		// Token: 0x040014AC RID: 5292
		public TrashContentData ContentData;
	}
}
