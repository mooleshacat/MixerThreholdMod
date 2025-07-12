using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000C51 RID: 3153
	[Serializable]
	public class WateringCanInstance : StorableItemInstance
	{
		// Token: 0x060058E9 RID: 22761 RVA: 0x000D7CEA File Offset: 0x000D5EEA
		public WateringCanInstance()
		{
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x00177DB4 File Offset: 0x00175FB4
		public WateringCanInstance(ItemDefinition definition, int quantity, float fillAmount) : base(definition, quantity)
		{
			this.CurrentFillAmount = fillAmount;
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x00177DC8 File Offset: 0x00175FC8
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new WateringCanInstance(base.Definition, quantity, this.CurrentFillAmount);
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x00177DF4 File Offset: 0x00175FF4
		public void ChangeFillAmount(float change)
		{
			this.CurrentFillAmount = Mathf.Clamp(this.CurrentFillAmount + change, 0f, 15f);
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x00177E26 File Offset: 0x00176026
		public override ItemData GetItemData()
		{
			return new WateringCanData(this.ID, this.Quantity, this.CurrentFillAmount);
		}

		// Token: 0x0400410F RID: 16655
		public float CurrentFillAmount;
	}
}
