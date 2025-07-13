using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x020009B8 RID: 2488
	[RequireComponent(typeof(AvatarEquippable))]
	public class AvatarEquippableLookAt : MonoBehaviour
	{
		// Token: 0x0600436D RID: 17261 RVA: 0x0011B623 File Offset: 0x00119823
		private void Start()
		{
			this.avatar = base.GetComponentInParent<Avatar>();
			if (this.avatar == null)
			{
				Debug.LogError("AvatarEquippableLookAt must be a child of an Avatar object.");
				return;
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0011B64A File Offset: 0x0011984A
		private void LateUpdate()
		{
			if (this.avatar == null)
			{
				return;
			}
			this.avatar.LookController.OverrideLookTarget(this.avatar.CurrentEquippable.transform.position, this.Priority, false);
		}

		// Token: 0x0400302B RID: 12331
		public int Priority;

		// Token: 0x0400302C RID: 12332
		private Avatar avatar;
	}
}
