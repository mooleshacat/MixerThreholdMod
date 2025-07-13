using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E8 RID: 2280
	[RequireComponent(typeof(StorageEntity))]
	public class StorageEntityVisualizer : StorageVisualizer
	{
		// Token: 0x06003DC2 RID: 15810 RVA: 0x00104A8C File Offset: 0x00102C8C
		protected virtual void Start()
		{
			this.storageEntity = base.GetComponent<StorageEntity>();
			this.storageEntity.onContentsChanged.AddListener(new UnityAction(base.QueueRefresh));
			for (int i = 0; i < this.storageEntity.ItemSlots.Count; i++)
			{
				base.AddSlot(this.storageEntity.ItemSlots[i], false);
			}
			if (this.storageEntity.ItemCount > 0)
			{
				base.QueueRefresh();
			}
		}

		// Token: 0x04002C15 RID: 11285
		private StorageEntity storageEntity;
	}
}
