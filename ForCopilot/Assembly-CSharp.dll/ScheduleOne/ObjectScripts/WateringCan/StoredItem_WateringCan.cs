using System;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000C4F RID: 3151
	public class StoredItem_WateringCan : StoredItem
	{
		// Token: 0x060058E5 RID: 22757 RVA: 0x00177D6C File Offset: 0x00175F6C
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			WateringCanInstance wateringCanInstance = _item as WateringCanInstance;
			if (wateringCanInstance == null)
			{
				return;
			}
			this.Visuals.SetFillLevel(wateringCanInstance.CurrentFillAmount / 15f);
		}

		// Token: 0x0400410C RID: 16652
		public WateringCanVisuals Visuals;
	}
}
