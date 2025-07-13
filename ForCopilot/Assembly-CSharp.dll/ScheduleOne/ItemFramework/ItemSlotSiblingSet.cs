using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000993 RID: 2451
	public class ItemSlotSiblingSet : MonoBehaviour
	{
		// Token: 0x06004245 RID: 16965 RVA: 0x00116AF0 File Offset: 0x00114CF0
		public ItemSlotSiblingSet(params ItemSlot[] slots)
		{
			foreach (ItemSlot slot in slots)
			{
				this.AddSlot(slot);
			}
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00116B2C File Offset: 0x00114D2C
		public ItemSlotSiblingSet(List<ItemSlot> slots)
		{
			foreach (ItemSlot slot in slots)
			{
				this.AddSlot(slot);
			}
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x00116B8C File Offset: 0x00114D8C
		public void AddSlot(ItemSlot slot)
		{
			if (this.Slots.Contains(slot))
			{
				Debug.LogWarning("Slot already exists in this sibling set");
				return;
			}
			this.Slots.Add(slot);
			slot.SetSiblingSet(this);
		}

		// Token: 0x04002F19 RID: 12057
		public List<ItemSlot> Slots = new List<ItemSlot>();
	}
}
