using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009DD RID: 2525
	public class PoliceBelt : Accessory
	{
		// Token: 0x0600443C RID: 17468 RVA: 0x0011ED56 File Offset: 0x0011CF56
		public void SetBatonVisible(bool vis)
		{
			this.BatonObject.gameObject.SetActive(vis);
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0011ED69 File Offset: 0x0011CF69
		public void SetTaserVisible(bool vis)
		{
			this.TaserObject.gameObject.SetActive(vis);
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0011ED7C File Offset: 0x0011CF7C
		public void SetGunVisible(bool vis)
		{
			this.GunObject.gameObject.SetActive(vis);
		}

		// Token: 0x04003125 RID: 12581
		[Header("References")]
		[SerializeField]
		protected GameObject BatonObject;

		// Token: 0x04003126 RID: 12582
		[SerializeField]
		protected GameObject TaserObject;

		// Token: 0x04003127 RID: 12583
		[SerializeField]
		protected GameObject GunObject;
	}
}
