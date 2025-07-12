using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E2 RID: 2530
	public class ACEntry : MonoBehaviour
	{
		// Token: 0x0600444C RID: 17484 RVA: 0x0011EF68 File Offset: 0x0011D168
		private void Awake()
		{
			if (this.DevOnly && !Application.isEditor)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0400312A RID: 12586
		public bool DevOnly;
	}
}
