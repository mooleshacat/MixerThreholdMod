using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF8 RID: 3064
	public class BedItem : PlaceableStorageEntity
	{
		// Token: 0x0600522B RID: 21035 RVA: 0x0015B145 File Offset: 0x00159345
		protected override void Start()
		{
			base.Start();
			this.Bed.EmployeeStationThing.onAssignedEmployeeChanged.AddListener(new UnityAction(this.UpdateBriefcase));
			this.UpdateBriefcase();
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x0015B174 File Offset: 0x00159374
		public static bool IsBedValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (!(obj is BedItem))
			{
				return false;
			}
			BedItem bedItem = obj as BedItem;
			if (bedItem.Bed.AssignedEmployee != null)
			{
				reason = "Already assigned to " + bedItem.Bed.AssignedEmployee.fullName;
				return false;
			}
			return true;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x0015B1CC File Offset: 0x001593CC
		private void UpdateBriefcase()
		{
			this.Briefcase.gameObject.SetActive(this.Bed.AssignedEmployee != null || this.Storage.ItemCount > 0);
			if (this.Bed.AssignedEmployee != null)
			{
				this.Storage.StorageEntityName = this.Bed.AssignedEmployee.FirstName + "'s Briefcase";
				string text = "<color=#54E717>" + MoneyManager.FormatAmount(this.Bed.AssignedEmployee.DailyWage, false, false) + "</color>";
				this.Storage.StorageEntitySubtitle = string.Concat(new string[]
				{
					this.Bed.AssignedEmployee.fullName,
					" will draw ",
					this.Bed.AssignedEmployee.IsMale ? "his" : "her",
					" daily wage of ",
					text,
					" from this briefcase."
				});
				return;
			}
			this.Storage.StorageEntityName = "Briefcase";
			this.Storage.StorageEntitySubtitle = string.Empty;
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x0015B2F8 File Offset: 0x001594F8
		public float GetCashSum()
		{
			float num = 0f;
			foreach (ItemSlot itemSlot in this.Storage.ItemSlots)
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is CashInstance)
				{
					num += (itemSlot.ItemInstance as CashInstance).Balance;
				}
			}
			return num;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x0015B378 File Offset: 0x00159578
		public void RemoveCash(float amount)
		{
			foreach (ItemSlot itemSlot in this.Storage.ItemSlots)
			{
				if (amount <= 0f)
				{
					break;
				}
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is CashInstance)
				{
					CashInstance cashInstance = itemSlot.ItemInstance as CashInstance;
					float num = Mathf.Min(amount, cashInstance.Balance);
					cashInstance.ChangeBalance(-num);
					itemSlot.ReplicateStoredInstance();
					amount -= num;
				}
			}
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x0015B41C File Offset: 0x0015961C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x0015B435 File Offset: 0x00159635
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.BedItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x0015B44E File Offset: 0x0015964E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x0015B45C File Offset: 0x0015965C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003D98 RID: 15768
		public Bed Bed;

		// Token: 0x04003D99 RID: 15769
		public StorageEntity Storage;

		// Token: 0x04003D9A RID: 15770
		public GameObject Briefcase;

		// Token: 0x04003D9B RID: 15771
		private bool dll_Excuted;

		// Token: 0x04003D9C RID: 15772
		private bool dll_Excuted;
	}
}
