using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000457 RID: 1111
	public class ToggleableItemData : GridItemData
	{
		// Token: 0x0600168E RID: 5774 RVA: 0x00064390 File Offset: 0x00062590
		public ToggleableItemData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, bool isOn) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.IsOn = isOn;
		}

		// Token: 0x040014AA RID: 5290
		public bool IsOn;
	}
}
