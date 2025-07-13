using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009AF RID: 2479
	public class Hair : Accessory
	{
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06004343 RID: 17219 RVA: 0x0011AECF File Offset: 0x001190CF
		// (set) Token: 0x06004344 RID: 17220 RVA: 0x0011AED7 File Offset: 0x001190D7
		public bool BlockedByHat { get; protected set; }

		// Token: 0x06004345 RID: 17221 RVA: 0x0011AEE0 File Offset: 0x001190E0
		public void SetBlockedByHat(bool blocked)
		{
			this.BlockedByHat = blocked;
			if (blocked)
			{
				this.BlockHair();
				return;
			}
			this.UnBlockHair();
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x0011AEFC File Offset: 0x001190FC
		protected virtual void BlockHair()
		{
			GameObject[] array = this.hairToHide;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x0011AF28 File Offset: 0x00119128
		protected virtual void UnBlockHair()
		{
			GameObject[] array = this.hairToHide;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}

		// Token: 0x04003007 RID: 12295
		[SerializeField]
		private GameObject[] hairToHide;
	}
}
