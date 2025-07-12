using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FF RID: 1023
	[Serializable]
	public class ItemSet
	{
		// Token: 0x06001614 RID: 5652 RVA: 0x0006310C File Offset: 0x0006130C
		public ItemSet(List<ItemData> items)
		{
			this.Items = new string[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				this.Items[i] = items[i].GetJson(false);
			}
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00063156 File Offset: 0x00061356
		public string GetJSON()
		{
			return JsonUtility.ToJson(this, true);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00063160 File Offset: 0x00061360
		public ItemSet(List<ItemSlot> itemSlots)
		{
			this.Items = new string[itemSlots.Count];
			this.SlotFilters = new SlotFilter[itemSlots.Count];
			for (int i = 0; i < itemSlots.Count; i++)
			{
				if (itemSlots[i].ItemInstance != null)
				{
					this.Items[i] = itemSlots[i].ItemInstance.GetItemData().GetJson(false);
				}
				else
				{
					this.Items[i] = new ItemData(string.Empty, 0).GetJson(false);
				}
				if (!itemSlots[i].PlayerFilter.IsDefault())
				{
					this.SlotFilters[i] = itemSlots[i].PlayerFilter;
				}
				else
				{
					this.SlotFilters[i] = null;
				}
			}
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00063224 File Offset: 0x00061424
		public ItemSet(ItemSlot[] itemSlots)
		{
			this.Items = new string[itemSlots.Length];
			this.SlotFilters = new SlotFilter[itemSlots.Length];
			for (int i = 0; i < itemSlots.Length; i++)
			{
				if (itemSlots[i].ItemInstance != null)
				{
					this.Items[i] = itemSlots[i].ItemInstance.GetItemData().GetJson(false);
				}
				else
				{
					this.Items[i] = new ItemData(string.Empty, 0).GetJson(false);
				}
				if (!itemSlots[i].PlayerFilter.IsDefault())
				{
					this.SlotFilters[i] = itemSlots[i].PlayerFilter;
				}
				else
				{
					this.SlotFilters[i] = null;
				}
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000632CC File Offset: 0x000614CC
		public void LoadTo(List<ItemSlot> slots)
		{
			for (int i = 0; i < slots.Count; i++)
			{
				if (this.Items != null && this.Items.Length > i)
				{
					slots[i].SetStoredItem(ItemDeserializer.LoadItem(this.Items[i]), false);
				}
				if (this.SlotFilters != null && this.SlotFilters.Length > i)
				{
					slots[i].SetPlayerFilter(this.SlotFilters[i], false);
				}
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0006333F File Offset: 0x0006153F
		public void LoadTo(ItemSlot[] slots)
		{
			this.LoadTo(slots.ToList<ItemSlot>());
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00063350 File Offset: 0x00061550
		public void LoadTo(ItemSlot slot, int index = 0)
		{
			if (this.Items != null && this.Items.Length > index)
			{
				slot.SetStoredItem(ItemDeserializer.LoadItem(this.Items[index]), false);
			}
			if (this.SlotFilters != null && this.SlotFilters.Length > index)
			{
				slot.SetPlayerFilter(this.SlotFilters[index], false);
			}
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000633A8 File Offset: 0x000615A8
		public static bool TryDeserialize(string json, out DeserializedItemSet itemSet)
		{
			itemSet = new DeserializedItemSet();
			ItemSet set = null;
			try
			{
				set = JsonUtility.FromJson<ItemSet>(json);
			}
			catch (Exception ex)
			{
				string str = "Failed to deserialize ItemSet from JSON: ";
				string str2 = "\nException: ";
				Exception ex2 = ex;
				Console.LogError(str + json + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				return false;
			}
			return ItemSet.TryDeserialize(set, out itemSet);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00063408 File Offset: 0x00061608
		public static bool TryDeserialize(ItemSet set, out DeserializedItemSet itemSet)
		{
			itemSet = new DeserializedItemSet();
			if (set == null)
			{
				Console.LogError("ItemSet is null", null);
				return false;
			}
			if (set.Items != null)
			{
				itemSet.Items = new ItemInstance[set.Items.Length];
				itemSet.SlotFilters = new SlotFilter[set.Items.Length];
				for (int i = 0; i < set.Items.Length; i++)
				{
					itemSet.Items[i] = ItemDeserializer.LoadItem(set.Items[i]);
					if (set.SlotFilters != null && set.SlotFilters.Length > i)
					{
						itemSet.SlotFilters[i] = set.SlotFilters[i];
					}
					else
					{
						itemSet.SlotFilters[i] = null;
					}
				}
			}
			return true;
		}

		// Token: 0x040013DC RID: 5084
		public string[] Items;

		// Token: 0x040013DD RID: 5085
		public SlotFilter[] SlotFilters;
	}
}
