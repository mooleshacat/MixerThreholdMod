using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000454 RID: 1108
	public class SoilPourerData : GridItemData
	{
		// Token: 0x0600168B RID: 5771 RVA: 0x0006433B File Offset: 0x0006253B
		public SoilPourerData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, string soilID) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.SoilID = soilID;
		}

		// Token: 0x040014A5 RID: 5285
		public string SoilID;
	}
}
