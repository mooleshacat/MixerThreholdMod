using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FE RID: 1022
	public class DeserializedItemSet
	{
		// Token: 0x06001611 RID: 5649 RVA: 0x00063056 File Offset: 0x00061256
		public ItemInstance GetItemAt(int index)
		{
			if (this.Items == null || index < 0 || index >= this.Items.Length)
			{
				return null;
			}
			return this.Items[index];
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00063079 File Offset: 0x00061279
		public SlotFilter GetSlotFilterAt(int index)
		{
			if (this.SlotFilters == null || index < 0 || index >= this.SlotFilters.Length)
			{
				return null;
			}
			return this.SlotFilters[index];
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0006309C File Offset: 0x0006129C
		public void LoadTo(List<ItemSlot> slots)
		{
			for (int i = 0; i < slots.Count; i++)
			{
				if (this.Items != null && this.Items.Length > i)
				{
					slots[i].SetStoredItem(this.Items[i], false);
				}
				if (this.SlotFilters != null && this.SlotFilters.Length > i)
				{
					slots[i].SetPlayerFilter(this.SlotFilters[i], false);
				}
			}
		}

		// Token: 0x040013DA RID: 5082
		public ItemInstance[] Items;

		// Token: 0x040013DB RID: 5083
		public SlotFilter[] SlotFilters;
	}
}
