using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200044B RID: 1099
	[Serializable]
	public class GridItemData : BuildableItemData
	{
		// Token: 0x06001682 RID: 5762 RVA: 0x000641A0 File Offset: 0x000623A0
		public GridItemData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation) : base(guid, item, loadOrder)
		{
			this.GridGUID = grid.GUID.ToString();
			this.OriginCoordinate = originCoordinate;
			this.Rotation = rotation;
		}

		// Token: 0x0400148A RID: 5258
		public string GridGUID;

		// Token: 0x0400148B RID: 5259
		public Vector2 OriginCoordinate;

		// Token: 0x0400148C RID: 5260
		public int Rotation;
	}
}
