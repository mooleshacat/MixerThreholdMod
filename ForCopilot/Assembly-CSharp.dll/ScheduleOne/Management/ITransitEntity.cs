using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005B7 RID: 1463
	public interface ITransitEntity
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600240C RID: 9228
		string Name { get; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600240D RID: 9229
		// (set) Token: 0x0600240E RID: 9230
		List<ItemSlot> InputSlots { get; set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600240F RID: 9231
		// (set) Token: 0x06002410 RID: 9232
		List<ItemSlot> OutputSlots { get; set; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06002411 RID: 9233
		Transform LinkOrigin { get; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002412 RID: 9234
		Transform[] AccessPoints { get; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002413 RID: 9235
		bool Selectable { get; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002414 RID: 9236
		bool IsAcceptingItems { get; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06002415 RID: 9237
		bool IsDestroyed { get; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002416 RID: 9238
		Guid GUID { get; }

		// Token: 0x06002417 RID: 9239
		void ShowOutline(Color color);

		// Token: 0x06002418 RID: 9240
		void HideOutline();

		// Token: 0x06002419 RID: 9241 RVA: 0x00094370 File Offset: 0x00092570
		void InsertItemIntoInput(ItemInstance item, NPC inserter = null)
		{
			if (this.GetInputCapacityForItem(item, inserter, true) < item.Quantity)
			{
				Console.LogWarning("ITransitEntity InsertItem() called but item won't fit!", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.InputSlots.Count; i++)
			{
				if (!this.InputSlots[i].IsLocked && !this.InputSlots[i].IsAddLocked)
				{
					int capacityForItem = this.InputSlots[i].GetCapacityForItem(item, true);
					if (capacityForItem > 0)
					{
						int num2 = Mathf.Min(capacityForItem, num);
						if (this.InputSlots[i].ItemInstance == null)
						{
							this.InputSlots[i].SetStoredItem(item, false);
						}
						else
						{
							this.InputSlots[i].ChangeQuantity(num2, false);
						}
						num -= num2;
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x00094448 File Offset: 0x00092648
		void InsertItemIntoOutput(ItemInstance item, NPC inserter = null)
		{
			if (this.GetOutputCapacityForItem(item, inserter) < item.Quantity)
			{
				Console.LogWarning("ITransitEntity InsertItem() called but item won't fit!", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.OutputSlots.Count; i++)
			{
				if (!this.OutputSlots[i].IsLocked && !this.OutputSlots[i].IsAddLocked)
				{
					int capacityForItem = this.OutputSlots[i].GetCapacityForItem(item, false);
					if (capacityForItem > 0)
					{
						int num2 = Mathf.Min(capacityForItem, num);
						if (this.OutputSlots[i].ItemInstance == null)
						{
							this.OutputSlots[i].SetStoredItem(item, false);
						}
						else
						{
							this.OutputSlots[i].ChangeQuantity(num2, false);
						}
						num -= num2;
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x00094520 File Offset: 0x00092720
		int GetInputCapacityForItem(ItemInstance item, NPC asker = null, bool checkPlayerFilters = true)
		{
			int num = 0;
			NetworkObject networkObject = (asker != null) ? asker.NetworkObject : null;
			int i = 0;
			while (i < this.InputSlots.Count)
			{
				if (!this.InputSlots[i].IsLocked && !this.InputSlots[i].IsAddLocked)
				{
					goto IL_83;
				}
				bool flag = false;
				if (networkObject != null && this.InputSlots[i].ActiveLock != null && this.InputSlots[i].ActiveLock.LockOwner == networkObject)
				{
					flag = true;
				}
				if (flag)
				{
					goto IL_83;
				}
				IL_99:
				i++;
				continue;
				IL_83:
				num += this.InputSlots[i].GetCapacityForItem(item, checkPlayerFilters);
				goto IL_99;
			}
			return num;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000945DC File Offset: 0x000927DC
		int GetOutputCapacityForItem(ItemInstance item, NPC asker = null)
		{
			int num = 0;
			NetworkObject networkObject = (asker != null) ? asker.NetworkObject : null;
			int i = 0;
			while (i < this.OutputSlots.Count)
			{
				if (!this.OutputSlots[i].IsLocked && !this.OutputSlots[i].IsAddLocked)
				{
					goto IL_83;
				}
				bool flag = false;
				if (networkObject != null && this.OutputSlots[i].ActiveLock != null && this.OutputSlots[i].ActiveLock.LockOwner == networkObject)
				{
					flag = true;
				}
				if (flag)
				{
					goto IL_83;
				}
				IL_99:
				i++;
				continue;
				IL_83:
				num += this.OutputSlots[i].GetCapacityForItem(item, false);
				goto IL_99;
			}
			return num;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x00094698 File Offset: 0x00092898
		ItemSlot GetOutputItemContainer(ItemInstance item)
		{
			return this.OutputSlots.FirstOrDefault((ItemSlot x) => x.ItemInstance == item);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000946CC File Offset: 0x000928CC
		List<ItemSlot> ReserveInputSlotsForItem(ItemInstance item, NetworkObject locker)
		{
			List<ItemSlot> list = new List<ItemSlot>();
			int num = item.Quantity;
			for (int i = 0; i < this.InputSlots.Count; i++)
			{
				int capacityForItem = this.InputSlots[i].GetCapacityForItem(item, false);
				if (capacityForItem != 0)
				{
					int num2 = Mathf.Min(capacityForItem, num);
					num -= num2;
					this.InputSlots[i].ApplyLock(locker, "Employee is about to place an item here", false);
					list.Add(this.InputSlots[i]);
					if (num <= 0)
					{
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x00094750 File Offset: 0x00092950
		void RemoveSlotLocks(NetworkObject locker)
		{
			for (int i = 0; i < this.InputSlots.Count; i++)
			{
				if (this.InputSlots[i].ActiveLock != null && this.InputSlots[i].ActiveLock.LockOwner == locker)
				{
					this.InputSlots[i].RemoveLock(false);
				}
			}
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000947B8 File Offset: 0x000929B8
		ItemSlot GetFirstSlotContainingItem(string id, ITransitEntity.ESlotType searchType)
		{
			if (searchType == ITransitEntity.ESlotType.Output || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int i = 0; i < this.OutputSlots.Count; i++)
				{
					if (this.OutputSlots[i].ItemInstance != null && this.OutputSlots[i].ItemInstance.ID == id)
					{
						return this.OutputSlots[i];
					}
				}
			}
			if (searchType == ITransitEntity.ESlotType.Input || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int j = 0; j < this.InputSlots.Count; j++)
				{
					if (this.InputSlots[j].ItemInstance != null && this.InputSlots[j].ItemInstance.ID == id)
					{
						return this.InputSlots[j];
					}
				}
			}
			return null;
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x00094880 File Offset: 0x00092A80
		ItemSlot GetFirstSlotContainingTemplateItem(ItemInstance templateItem, ITransitEntity.ESlotType searchType)
		{
			if (searchType == ITransitEntity.ESlotType.Output || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int i = 0; i < this.OutputSlots.Count; i++)
				{
					if (this.OutputSlots[i].ItemInstance != null && this.OutputSlots[i].ItemInstance.CanStackWith(templateItem, false))
					{
						return this.OutputSlots[i];
					}
				}
			}
			if (searchType == ITransitEntity.ESlotType.Input || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int j = 0; j < this.InputSlots.Count; j++)
				{
					if (this.InputSlots[j].ItemInstance != null && this.InputSlots[j].ItemInstance.CanStackWith(templateItem, false))
					{
						return this.InputSlots[j];
					}
				}
			}
			return null;
		}

		// Token: 0x020005B8 RID: 1464
		public enum ESlotType
		{
			// Token: 0x04001AD2 RID: 6866
			Input,
			// Token: 0x04001AD3 RID: 6867
			Output,
			// Token: 0x04001AD4 RID: 6868
			Both
		}
	}
}
