using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E1 RID: 2273
	public class PalletSlotDetector : MonoBehaviour
	{
		// Token: 0x06003D5D RID: 15709 RVA: 0x00102C2C File Offset: 0x00100E2C
		protected virtual void OnTriggerStay(Collider other)
		{
			this.pallet.TriggerStay(other);
		}

		// Token: 0x04002BFA RID: 11258
		public Pallet pallet;
	}
}
