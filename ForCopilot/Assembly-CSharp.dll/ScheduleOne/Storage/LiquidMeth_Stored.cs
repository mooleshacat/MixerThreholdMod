using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008DB RID: 2267
	public class LiquidMeth_Stored : StoredItem
	{
		// Token: 0x06003CFD RID: 15613 RVA: 0x00100EA0 File Offset: 0x000FF0A0
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			LiquidMethDefinition def = _item.Definition as LiquidMethDefinition;
			if (this.Visuals != null)
			{
				this.Visuals.Setup(def);
			}
		}

		// Token: 0x04002BDC RID: 11228
		public LiquidMethVisuals Visuals;
	}
}
