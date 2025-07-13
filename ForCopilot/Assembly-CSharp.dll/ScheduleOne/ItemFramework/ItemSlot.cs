using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000991 RID: 2449
	[Serializable]
	public class ItemSlot
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x00116253 File Offset: 0x00114453
		// (set) Token: 0x06004210 RID: 16912 RVA: 0x0011625B File Offset: 0x0011445B
		public ItemInstance ItemInstance { get; protected set; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x00116264 File Offset: 0x00114464
		// (set) Token: 0x06004212 RID: 16914 RVA: 0x0011626C File Offset: 0x0011446C
		public IItemSlotOwner SlotOwner { get; protected set; }

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x00116275 File Offset: 0x00114475
		private int SlotIndex
		{
			get
			{
				return this.SlotOwner.ItemSlots.IndexOf(this);
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x00116288 File Offset: 0x00114488
		public int Quantity
		{
			get
			{
				if (this.ItemInstance == null)
				{
					return 0;
				}
				return this.ItemInstance.Quantity;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x0011629F File Offset: 0x0011449F
		public bool IsAtCapacity
		{
			get
			{
				return this.ItemInstance != null && this.Quantity >= this.ItemInstance.StackLimit;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x001162C1 File Offset: 0x001144C1
		public bool IsLocked
		{
			get
			{
				return this.ActiveLock != null;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x001162CC File Offset: 0x001144CC
		// (set) Token: 0x06004218 RID: 16920 RVA: 0x001162D4 File Offset: 0x001144D4
		public ItemSlotLock ActiveLock { get; protected set; }

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x001162DD File Offset: 0x001144DD
		// (set) Token: 0x0600421A RID: 16922 RVA: 0x001162E5 File Offset: 0x001144E5
		public bool IsRemovalLocked { get; protected set; }

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x001162EE File Offset: 0x001144EE
		// (set) Token: 0x0600421C RID: 16924 RVA: 0x001162F6 File Offset: 0x001144F6
		public bool IsAddLocked { get; protected set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x001162FF File Offset: 0x001144FF
		// (set) Token: 0x0600421E RID: 16926 RVA: 0x00116307 File Offset: 0x00114507
		protected List<ItemFilter> HardFilters { get; set; } = new List<ItemFilter>();

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x00116310 File Offset: 0x00114510
		// (set) Token: 0x06004220 RID: 16928 RVA: 0x00116318 File Offset: 0x00114518
		public bool CanPlayerSetFilter { get; set; }

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x00116321 File Offset: 0x00114521
		// (set) Token: 0x06004222 RID: 16930 RVA: 0x00116329 File Offset: 0x00114529
		public SlotFilter PlayerFilter { get; set; } = new SlotFilter();

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x00116332 File Offset: 0x00114532
		// (set) Token: 0x06004224 RID: 16932 RVA: 0x0011633A File Offset: 0x0011453A
		public ItemSlotSiblingSet SiblingSet { get; set; }

		// Token: 0x06004225 RID: 16933 RVA: 0x00116343 File Offset: 0x00114543
		public void SetSlotOwner(IItemSlotOwner owner)
		{
			this.SlotOwner = owner;
			this.SlotOwner.ItemSlots.Add(this);
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x0011635D File Offset: 0x0011455D
		public void SetSiblingSet(ItemSlotSiblingSet set)
		{
			if (this.SiblingSet != null)
			{
				Console.LogError("SetSiblingSet called on ItemSlot that already has a sibling set! Refusing.", null);
				return;
			}
			this.SiblingSet = set;
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x00116380 File Offset: 0x00114580
		public ItemSlot()
		{
			this.CanPlayerSetFilter = false;
			this.HardFilters = new List<ItemFilter>();
			this.PlayerFilter = new SlotFilter();
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x001163BB File Offset: 0x001145BB
		public ItemSlot(bool canPlayerSetFilter = false)
		{
			this.CanPlayerSetFilter = canPlayerSetFilter;
			this.HardFilters = new List<ItemFilter>();
			this.PlayerFilter = new SlotFilter();
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x001163F6 File Offset: 0x001145F6
		public void ReplicateStoredInstance()
		{
			if (this.SlotOwner == null)
			{
				return;
			}
			this.SlotOwner.SetStoredInstance(null, this.SlotIndex, this.ItemInstance);
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x0011641C File Offset: 0x0011461C
		public virtual void SetStoredItem(ItemInstance instance, bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogError("SetStoredInstance called on ItemSlot that is locked! Refusing.", null);
				return;
			}
			if (this.IsRemovalLocked)
			{
				Console.LogWarning("SetStoredItem called on ItemSlot that isRemovalLocked. You probably shouldn't do this.", null);
			}
			if (_internal || this.SlotOwner == null)
			{
				if (this.ItemInstance != null)
				{
					this.ClearStoredInstance(true);
				}
				this.ItemInstance = instance;
				if (this.ItemInstance != null)
				{
					ItemInstance itemInstance = this.ItemInstance;
					itemInstance.onDataChanged = (Action)Delegate.Combine(itemInstance.onDataChanged, new Action(this.ItemDataChanged));
					ItemInstance itemInstance2 = this.ItemInstance;
					itemInstance2.requestClearSlot = (Action)Delegate.Combine(itemInstance2.requestClearSlot, new Action(this.ClearItemInstanceRequested));
				}
				if (this.onItemDataChanged != null)
				{
					this.onItemDataChanged();
				}
				if (this.onItemInstanceChanged != null)
				{
					this.onItemInstanceChanged();
				}
				this.ItemDataChanged();
				return;
			}
			this.SlotOwner.SetStoredInstance(null, this.SlotIndex, instance);
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x0011650C File Offset: 0x0011470C
		public virtual void InsertItem(ItemInstance item)
		{
			if (this.ItemInstance == null)
			{
				this.AddItem(item, false);
				return;
			}
			if (this.ItemInstance.CanStackWith(item, true))
			{
				this.ChangeQuantity(item.Quantity, false);
				return;
			}
			Console.LogWarning("InsertItem called with item that cannot stack with current item. Refusing.", null);
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00116547 File Offset: 0x00114747
		public virtual void AddItem(ItemInstance item, bool _internal = false)
		{
			if (this.ItemInstance == null)
			{
				this.SetStoredItem(item, _internal);
				return;
			}
			if (!this.ItemInstance.CanStackWith(item, true))
			{
				Console.LogWarning("AddItem called with item that cannot stack with current item. Refusing.", null);
				return;
			}
			this.ChangeQuantity(item.Quantity, _internal);
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00116584 File Offset: 0x00114784
		public virtual void ClearStoredInstance(bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogError("ClearStoredInstance called on ItemSlot that is locked! Refusing.", null);
				return;
			}
			if (this.IsRemovalLocked)
			{
				Console.LogError("ClearStoredInstance called on ItemSlot that is removal locked! Refusing.", null);
				return;
			}
			if (this.ItemInstance == null)
			{
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				ItemInstance itemInstance = this.ItemInstance;
				itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.ItemDataChanged));
				ItemInstance itemInstance2 = this.ItemInstance;
				itemInstance2.requestClearSlot = (Action)Delegate.Remove(itemInstance2.requestClearSlot, new Action(this.ClearItemInstanceRequested));
				this.ItemInstance = null;
				if (this.onItemDataChanged != null)
				{
					this.onItemDataChanged();
				}
				if (this.onItemInstanceChanged != null)
				{
					this.onItemInstanceChanged();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetStoredInstance(null, this.SlotIndex, null);
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x00116660 File Offset: 0x00114860
		public void SetQuantity(int amount, bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogError("SetQuantity called on ItemSlot that is locked! Refusing.", null);
				return;
			}
			if (this.ItemInstance == null)
			{
				Console.LogWarning("ChangeQuantity called but ItemInstance is null", null);
				return;
			}
			if (amount < this.ItemInstance.Quantity && this.IsRemovalLocked)
			{
				Console.LogError("SetQuantity called on ItemSlot and passed lower quantity that current, and isRemovalLocked = true. Refusing.", null);
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				this.ItemInstance.SetQuantity(amount);
				return;
			}
			this.SlotOwner.SetItemSlotQuantity(this.SlotIndex, amount);
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x001166E4 File Offset: 0x001148E4
		public void ChangeQuantity(int change, bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogWarning("isLocked = true!", null);
				return;
			}
			if (this.ItemInstance == null)
			{
				Console.LogWarning("ChangeQuantity called but ItemInstance is null", null);
				return;
			}
			if (this.IsRemovalLocked && change < 0)
			{
				Console.Log("Removal locked!", null);
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				this.ItemInstance.ChangeQuantity(change);
				return;
			}
			this.SlotOwner.SetItemSlotQuantity(this.SlotIndex, this.Quantity + change);
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x00116762 File Offset: 0x00114962
		protected virtual void ItemDataChanged()
		{
			if (this.ItemInstance != null && this.ItemInstance.Quantity <= 0)
			{
				this.ClearStoredInstance(false);
				return;
			}
			if (this.onItemDataChanged != null)
			{
				this.onItemDataChanged();
			}
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x00116795 File Offset: 0x00114995
		protected virtual void ClearItemInstanceRequested()
		{
			this.ClearStoredInstance(false);
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x0011679E File Offset: 0x0011499E
		public void AddFilter(ItemFilter filter)
		{
			if (this.HardFilters == null)
			{
				this.HardFilters = new List<ItemFilter>();
			}
			this.HardFilters.Add(filter);
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x001167C0 File Offset: 0x001149C0
		public void ApplyLock(NetworkObject lockOwner, string lockReason, bool _internal = false)
		{
			if (_internal || this.SlotOwner == null)
			{
				this.ActiveLock = new ItemSlotLock(this, lockOwner, lockReason);
				if (this.onLocked != null)
				{
					this.onLocked();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetSlotLocked(null, this.SlotIndex, true, lockOwner, lockReason);
			}
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x00116810 File Offset: 0x00114A10
		public void RemoveLock(bool _internal = false)
		{
			if (_internal || this.SlotOwner == null)
			{
				this.ActiveLock = null;
				if (this.onUnlocked != null)
				{
					this.onUnlocked();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetSlotLocked(null, this.SlotIndex, false, null, string.Empty);
			}
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x0011685C File Offset: 0x00114A5C
		public void SetIsRemovalLocked(bool locked)
		{
			this.IsRemovalLocked = locked;
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x00116865 File Offset: 0x00114A65
		public void SetIsAddLocked(bool locked)
		{
			this.IsAddLocked = locked;
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x00116870 File Offset: 0x00114A70
		public virtual bool DoesItemMatchHardFilters(ItemInstance item)
		{
			using (List<ItemFilter>.Enumerator enumerator = this.HardFilters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.DoesItemMatchFilter(item))
					{
						return false;
					}
				}
			}
			return !(item is CashInstance) || this.CanSlotAcceptCash();
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x001168DC File Offset: 0x00114ADC
		public virtual bool DoesItemMatchPlayerFilters(ItemInstance item)
		{
			return this.PlayerFilter.DoesItemMatchFilter(item);
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x001168EF File Offset: 0x00114AEF
		public void SetFilterable(bool filterable)
		{
			this.CanPlayerSetFilter = filterable;
			if (!filterable)
			{
				this.PlayerFilter = new SlotFilter();
			}
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x00116906 File Offset: 0x00114B06
		public void SetPlayerFilter(SlotFilter filter, bool _internal = false)
		{
			if (filter == null)
			{
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				this.PlayerFilter = filter;
				if (this.onFilterChange != null)
				{
					this.onFilterChange();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetSlotFilter(null, this.SlotIndex, filter);
			}
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00116948 File Offset: 0x00114B48
		public virtual int GetCapacityForItem(ItemInstance item, bool checkPlayerFilters = false)
		{
			if (!this.DoesItemMatchHardFilters(item))
			{
				return 0;
			}
			if (checkPlayerFilters && !this.DoesItemMatchPlayerFilters(item))
			{
				return 0;
			}
			if (this.ItemInstance == null || this.ItemInstance.CanStackWith(item, false))
			{
				return item.StackLimit - this.Quantity;
			}
			return 0;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool CanSlotAcceptCash()
		{
			return true;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00116994 File Offset: 0x00114B94
		public static bool TryInsertItemIntoSet(List<ItemSlot> ItemSlots, ItemInstance item)
		{
			int num = item.Quantity;
			int num2 = 0;
			while (num2 < ItemSlots.Count && num > 0)
			{
				if (!ItemSlots[num2].IsLocked && !ItemSlots[num2].IsAddLocked && ItemSlots[num2].ItemInstance != null && ItemSlots[num2].ItemInstance.CanStackWith(item, true))
				{
					int num3 = Mathf.Min(item.StackLimit - ItemSlots[num2].ItemInstance.Quantity, num);
					num -= num3;
					ItemSlots[num2].ChangeQuantity(num3, false);
				}
				num2++;
			}
			int num4 = 0;
			while (num4 < ItemSlots.Count && num > 0)
			{
				if (!ItemSlots[num4].IsLocked && !ItemSlots[num4].IsAddLocked && ItemSlots[num4].ItemInstance == null)
				{
					num -= item.StackLimit;
					ItemSlots[num4].SetStoredItem(item, false);
					break;
				}
				num4++;
			}
			return num <= 0;
		}

		// Token: 0x04002F0A RID: 12042
		public Action onItemDataChanged;

		// Token: 0x04002F0B RID: 12043
		public Action onItemInstanceChanged;

		// Token: 0x04002F0D RID: 12045
		public Action onLocked;

		// Token: 0x04002F0E RID: 12046
		public Action onUnlocked;

		// Token: 0x04002F14 RID: 12052
		public Action onFilterChange;
	}
}
