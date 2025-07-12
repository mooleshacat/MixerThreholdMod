using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200096F RID: 2415
	[Serializable]
	public class CashInstance : StorableItemInstance
	{
		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x00114945 File Offset: 0x00112B45
		// (set) Token: 0x06004174 RID: 16756 RVA: 0x0011494D File Offset: 0x00112B4D
		public float Balance { get; protected set; }

		// Token: 0x06004175 RID: 16757 RVA: 0x000D7CEA File Offset: 0x000D5EEA
		public CashInstance()
		{
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x00114956 File Offset: 0x00112B56
		public CashInstance(ItemDefinition definition, int quantity) : base(definition, quantity)
		{
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x00114960 File Offset: 0x00112B60
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new CashInstance(base.Definition, quantity);
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x00114986 File Offset: 0x00112B86
		public void ChangeBalance(float amount)
		{
			this.SetBalance(this.Balance + amount, false);
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x00114998 File Offset: 0x00112B98
		public void SetBalance(float newBalance, bool blockClear = false)
		{
			this.Balance = Mathf.Clamp(newBalance, 0f, 1E+09f);
			if (this.Balance <= 0f && !blockClear)
			{
				base.RequestClearSlot();
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x001149E4 File Offset: 0x00112BE4
		public override ItemData GetItemData()
		{
			return new CashData(this.ID, this.Quantity, this.Balance);
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x001149FD File Offset: 0x00112BFD
		public override float GetMonetaryValue()
		{
			return this.Balance;
		}

		// Token: 0x04002EA0 RID: 11936
		public const float MAX_BALANCE = 1E+09f;
	}
}
