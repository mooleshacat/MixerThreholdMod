using System;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C0B RID: 3083
	public class Meth_Stored : StoredItem
	{
		// Token: 0x0600531B RID: 21275 RVA: 0x0015F080 File Offset: 0x0015D280
		public override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
		{
			base.InitializeStoredItem(_item, grid, _originCoordinate, _rotation);
			MethInstance methInstance = _item as MethInstance;
			if (methInstance != null)
			{
				this.Visuals.Setup(methInstance.Definition as MethDefinition);
			}
		}

		// Token: 0x04003E28 RID: 15912
		public MethVisuals Visuals;
	}
}
