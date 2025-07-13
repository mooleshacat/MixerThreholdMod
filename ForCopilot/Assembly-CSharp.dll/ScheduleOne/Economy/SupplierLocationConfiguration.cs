using System;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006BC RID: 1724
	public class SupplierLocationConfiguration : MonoBehaviour
	{
		// Token: 0x06002F54 RID: 12116 RVA: 0x000C6C1B File Offset: 0x000C4E1B
		public void Activate()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000C6C29 File Offset: 0x000C4E29
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04002148 RID: 8520
		public string SupplierID;
	}
}
