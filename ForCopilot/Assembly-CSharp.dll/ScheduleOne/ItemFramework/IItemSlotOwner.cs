using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000982 RID: 2434
	public interface IItemSlotOwner
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060041A0 RID: 16800
		// (set) Token: 0x060041A1 RID: 16801
		List<ItemSlot> ItemSlots { get; set; }

		// Token: 0x060041A2 RID: 16802
		void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance);

		// Token: 0x060041A3 RID: 16803
		void SetItemSlotQuantity(int itemSlotIndex, int quantity);

		// Token: 0x060041A4 RID: 16804
		void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason);

		// Token: 0x060041A5 RID: 16805
		void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter);

		// Token: 0x060041A6 RID: 16806 RVA: 0x00114D74 File Offset: 0x00112F74
		void SendItemSlotDataToClient(NetworkConnection conn)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].IsLocked)
				{
					this.SetSlotLocked(conn, i, true, this.ItemSlots[i].ActiveLock.LockOwner, this.ItemSlots[i].ActiveLock.LockReason);
				}
				if (!this.ItemSlots[i].PlayerFilter.IsDefault())
				{
					this.SetSlotFilter(conn, i, this.ItemSlots[i].PlayerFilter);
				}
				if (this.ItemSlots[i].ItemInstance != null)
				{
					this.SetStoredInstance(conn, i, this.ItemSlots[i].ItemInstance);
				}
			}
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x00114E42 File Offset: 0x00113042
		int GetTotalItemCount()
		{
			return this.ItemSlots.Sum((ItemSlot x) => x.Quantity);
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x00114E70 File Offset: 0x00113070
		int GetItemCount(string id)
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.ID == id)
				{
					num += this.ItemSlots[i].Quantity;
				}
			}
			return num;
		}
	}
}
