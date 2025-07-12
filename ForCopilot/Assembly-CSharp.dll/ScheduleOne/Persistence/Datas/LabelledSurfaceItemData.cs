using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200044D RID: 1101
	public class LabelledSurfaceItemData : SurfaceItemData
	{
		// Token: 0x06001684 RID: 5764 RVA: 0x000641FA File Offset: 0x000623FA
		public LabelledSurfaceItemData(Guid guid, ItemInstance item, int loadOrder, string parentSurfaceGUID, Vector3 pos, Quaternion rot, string message) : base(guid, item, loadOrder, parentSurfaceGUID, pos, rot)
		{
			this.Message = message;
		}

		// Token: 0x0400148E RID: 5262
		public string Message;
	}
}
