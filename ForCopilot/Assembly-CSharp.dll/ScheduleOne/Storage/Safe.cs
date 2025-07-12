using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E3 RID: 2275
	public class Safe : StorageEntity
	{
		// Token: 0x06003D66 RID: 15718 RVA: 0x00102D34 File Offset: 0x00100F34
		public float GetCash()
		{
			float num = 0f;
			for (int i = 0; i < base.ItemSlots.Count; i++)
			{
				if (base.ItemSlots[i].ItemInstance != null && base.ItemSlots[i].ItemInstance is CashInstance)
				{
					CashInstance cashInstance = base.ItemSlots[i].ItemInstance as CashInstance;
					num += cashInstance.Balance;
				}
			}
			return num;
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x00102DAC File Offset: 0x00100FAC
		public void RemoveCash(float amount)
		{
			amount = Mathf.Abs(amount);
			float num = amount;
			for (int i = 0; i < base.ItemSlots.Count; i++)
			{
				if (base.ItemSlots[i].ItemInstance != null && base.ItemSlots[i].ItemInstance is CashInstance)
				{
					CashInstance cashInstance = base.ItemSlots[i].ItemInstance as CashInstance;
					float num2 = Mathf.Min(cashInstance.Balance, num);
					cashInstance.ChangeBalance(-num2);
					num -= num2;
				}
				if (num <= 0f)
				{
					break;
				}
			}
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x00102E3A File Offset: 0x0010103A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00102E53 File Offset: 0x00101053
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.SafeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x00102E6C File Offset: 0x0010106C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x00102E7A File Offset: 0x0010107A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002BFE RID: 11262
		private bool dll_Excuted;

		// Token: 0x04002BFF RID: 11263
		private bool dll_Excuted;
	}
}
