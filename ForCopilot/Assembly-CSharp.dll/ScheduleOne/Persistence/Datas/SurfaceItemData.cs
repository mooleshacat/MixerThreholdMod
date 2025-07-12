using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000456 RID: 1110
	[Serializable]
	public class SurfaceItemData : BuildableItemData
	{
		// Token: 0x0600168D RID: 5773 RVA: 0x0006436D File Offset: 0x0006256D
		public SurfaceItemData(Guid guid, ItemInstance item, int loadOrder, string parentSurfaceGUID, Vector3 pos, Quaternion rot) : base(guid, item, loadOrder)
		{
			this.ParentSurfaceGUID = parentSurfaceGUID;
			this.RelativePosition = pos;
			this.RelativeRotation = rot;
		}

		// Token: 0x040014A7 RID: 5287
		public string ParentSurfaceGUID;

		// Token: 0x040014A8 RID: 5288
		public Vector3 RelativePosition;

		// Token: 0x040014A9 RID: 5289
		public Quaternion RelativeRotation;
	}
}
