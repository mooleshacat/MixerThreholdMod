using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000455 RID: 1109
	public class StorageSurfaceItemData : SurfaceItemData
	{
		// Token: 0x0600168C RID: 5772 RVA: 0x00064354 File Offset: 0x00062554
		public StorageSurfaceItemData(Guid guid, ItemInstance item, int loadOrder, string parentSurfaceGUID, Vector3 pos, Quaternion rot, ItemSet contents) : base(guid, item, loadOrder, parentSurfaceGUID, pos, rot)
		{
			this.Contents = contents;
		}

		// Token: 0x040014A6 RID: 5286
		public ItemSet Contents;
	}
}
