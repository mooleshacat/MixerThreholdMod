using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000458 RID: 1112
	public class ToggleableSurfaceItemData : SurfaceItemData
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x000643A9 File Offset: 0x000625A9
		public ToggleableSurfaceItemData(Guid guid, ItemInstance item, int loadOrder, string parentSurfaceGUID, Vector3 pos, Quaternion rot, bool isOn) : base(guid, item, loadOrder, parentSurfaceGUID, pos, rot)
		{
			this.IsOn = isOn;
		}

		// Token: 0x040014AB RID: 5291
		public bool IsOn;
	}
}
