using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Money;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x020006BD RID: 1725
	public class SupplierStash : MonoBehaviour
	{
		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002F57 RID: 12119 RVA: 0x000C6C37 File Offset: 0x000C4E37
		// (set) Token: 0x06002F58 RID: 12120 RVA: 0x000C6C3F File Offset: 0x000C4E3F
		public float CashAmount { get; private set; }

		// Token: 0x06002F59 RID: 12121 RVA: 0x000C6C48 File Offset: 0x000C4E48
		protected virtual void Awake()
		{
			this.IntObj.SetMessage("View " + this.Supplier.fullName + "'s stash");
			this.IntObj.enabled = this.Supplier.RelationData.Unlocked;
			NPCRelationData relationData = this.Supplier.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType type, bool b)
			{
				this.SupplierUnlocked();
			}));
			this.Storage.StorageEntityName = this.Supplier.fullName + "'s Stash";
			this.Interacted();
			this.RecalculateCash();
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.RecalculateCash));
			this.StashPoI.enabled = this.Supplier.RelationData.Unlocked;
			this.StashPoI.SetMainText(this.Supplier.fullName + "'s Stash");
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000C6D44 File Offset: 0x000C4F44
		protected virtual void Start()
		{
			this.UpdateDeadDrop();
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.UpdateDeadDrop));
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000C6D84 File Offset: 0x000C4F84
		private void SupplierUnlocked()
		{
			Console.Log("Supplier unlocked: " + this.Supplier.fullName, null);
			this.StashPoI.enabled = true;
			this.IntObj.enabled = true;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000C6DBC File Offset: 0x000C4FBC
		private void RecalculateCash()
		{
			float num = 0f;
			for (int i = 0; i < this.Storage.ItemSlots.Count; i++)
			{
				if (this.Storage.ItemSlots[i] != null && this.Storage.ItemSlots[i].ItemInstance != null && this.Storage.ItemSlots[i].ItemInstance is CashInstance)
				{
					num += (this.Storage.ItemSlots[i].ItemInstance as CashInstance).Balance;
				}
			}
			this.CashAmount = num;
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000C6E60 File Offset: 0x000C5060
		private void Interacted()
		{
			this.Storage.StorageEntitySubtitle = string.Concat(new string[]
			{
				"You owe ",
				this.Supplier.fullName,
				" <color=#54E717>",
				MoneyManager.FormatAmount(this.Supplier.Debt, false, false),
				"</color>. Insert cash and exit stash to pay off your debt"
			});
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000C6EC0 File Offset: 0x000C50C0
		public void RemoveCash(float amount)
		{
			float num = amount;
			int num2 = 0;
			while (num2 < this.Storage.SlotCount && num > 0f)
			{
				if (this.Storage.ItemSlots[num2].ItemInstance != null && this.Storage.ItemSlots[num2].ItemInstance is CashInstance)
				{
					CashInstance cashInstance = this.Storage.ItemSlots[num2].ItemInstance as CashInstance;
					float num3 = Mathf.Min(num, cashInstance.Balance);
					cashInstance.ChangeBalance(-num3);
					if (cashInstance.Balance > 0f)
					{
						this.Storage.ItemSlots[num2].SetStoredItem(cashInstance, false);
					}
					num -= num3;
				}
				num2++;
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000C6F85 File Offset: 0x000C5185
		private void UpdateDeadDrop()
		{
			this.Light.Enabled = (this.Storage.ItemCount > 0);
		}

		// Token: 0x04002149 RID: 8521
		public string locationDescription = "behind the X";

		// Token: 0x0400214A RID: 8522
		[Header("References")]
		public Supplier Supplier;

		// Token: 0x0400214B RID: 8523
		public StorageEntity Storage;

		// Token: 0x0400214C RID: 8524
		public InteractableObject IntObj;

		// Token: 0x0400214D RID: 8525
		public OptimizedLight Light;

		// Token: 0x0400214E RID: 8526
		public POI StashPoI;
	}
}
