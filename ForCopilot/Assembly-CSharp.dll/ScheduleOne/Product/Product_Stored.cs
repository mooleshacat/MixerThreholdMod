using System;
using ScheduleOne.Packaging;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200093E RID: 2366
	public class Product_Stored : StoredItem
	{
		// Token: 0x0600401C RID: 16412 RVA: 0x0010F305 File Offset: 0x0010D505
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			(_item as ProductItemInstance).SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x04002D93 RID: 11667
		public FilledPackagingVisuals Visuals;
	}
}
