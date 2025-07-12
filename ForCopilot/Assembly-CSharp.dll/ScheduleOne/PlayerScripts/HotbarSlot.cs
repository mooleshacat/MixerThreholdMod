using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200060D RID: 1549
	public class HotbarSlot : ItemSlot
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000996F9 File Offset: 0x000978F9
		// (set) Token: 0x060025F3 RID: 9715 RVA: 0x00099701 File Offset: 0x00097901
		public bool IsEquipped { get; protected set; }

		// Token: 0x060025F4 RID: 9716 RVA: 0x0009970C File Offset: 0x0009790C
		public override void SetStoredItem(ItemInstance instance, bool _internal = false)
		{
			if ((_internal || base.SlotOwner == null) && this.IsEquipped && this.Equippable != null)
			{
				this.Equippable.Unequip();
				this.Equippable = null;
			}
			base.SetStoredItem(instance, _internal);
			if ((_internal || base.SlotOwner == null) && this.IsEquipped && instance != null && instance.Equippable != null)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped != null)
				{
					PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped.Invoke();
				}
				this.Equippable = UnityEngine.Object.Instantiate<GameObject>(instance.Equippable.gameObject, PlayerSingleton<PlayerInventory>.Instance.equipContainer).GetComponent<Equippable>();
				this.Equippable.Equip(instance);
			}
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000997C4 File Offset: 0x000979C4
		public override void ClearStoredInstance(bool _internal = false)
		{
			if ((_internal || base.SlotOwner == null) && this.IsEquipped && this.Equippable != null)
			{
				this.Equippable.Unequip();
				this.Equippable = null;
			}
			base.ClearStoredInstance(_internal);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x00099800 File Offset: 0x00097A00
		public virtual void Equip()
		{
			this.IsEquipped = true;
			if (base.ItemInstance != null && base.ItemInstance.Equippable != null)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped != null)
				{
					PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped.Invoke();
				}
				this.Equippable = UnityEngine.Object.Instantiate<GameObject>(base.ItemInstance.Equippable.gameObject, PlayerSingleton<PlayerInventory>.Instance.equipContainer).GetComponent<Equippable>();
				this.Equippable.Equip(base.ItemInstance);
			}
			if (this.onEquipChanged != null)
			{
				this.onEquipChanged(true);
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x00099899 File Offset: 0x00097A99
		public virtual void Unequip()
		{
			if (this.Equippable != null)
			{
				this.Equippable.Unequip();
				this.Equippable = null;
			}
			this.IsEquipped = false;
			if (this.onEquipChanged != null)
			{
				this.onEquipChanged(false);
			}
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00014B5A File Offset: 0x00012D5A
		public override bool CanSlotAcceptCash()
		{
			return false;
		}

		// Token: 0x04001C0A RID: 7178
		public Equippable Equippable;

		// Token: 0x04001C0B RID: 7179
		public HotbarSlot.EquipEvent onEquipChanged;

		// Token: 0x0200060E RID: 1550
		// (Invoke) Token: 0x060025FB RID: 9723
		public delegate void EquipEvent(bool equipped);
	}
}
