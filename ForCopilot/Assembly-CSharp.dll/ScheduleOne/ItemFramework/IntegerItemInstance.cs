using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000985 RID: 2437
	public class IntegerItemInstance : StorableItemInstance
	{
		// Token: 0x060041AE RID: 16814 RVA: 0x000D7CEA File Offset: 0x000D5EEA
		public IntegerItemInstance()
		{
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x00114EFE File Offset: 0x001130FE
		public IntegerItemInstance(ItemDefinition definition, int quantity, int value) : base(definition, quantity)
		{
			this.Value = value;
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x00114F10 File Offset: 0x00113110
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new IntegerItemInstance(base.Definition, quantity, this.Value);
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x00114F3C File Offset: 0x0011313C
		public void ChangeValue(int change)
		{
			this.Value += change;
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x00114F5F File Offset: 0x0011315F
		public void SetValue(int value)
		{
			this.Value = value;
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x00114F7B File Offset: 0x0011317B
		public override ItemData GetItemData()
		{
			return new IntegerItemData(this.ID, this.Quantity, this.Value);
		}

		// Token: 0x04002ECB RID: 11979
		public int Value;
	}
}
