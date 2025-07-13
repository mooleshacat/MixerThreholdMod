using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Product;
using ScheduleOne.Variables;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200051A RID: 1306
	public class Benji : Dealer
	{
		// Token: 0x06001CC6 RID: 7366 RVA: 0x00077A8C File Offset: 0x00075C8C
		protected override void MinPass()
		{
			base.MinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_Recommended", base.HasBeenRecommended.ToString(), true);
			int num = 0;
			for (int i = 0; i < base.Inventory.ItemSlots.Count; i++)
			{
				if (base.Inventory.ItemSlots[i].Quantity != 0 && base.Inventory.ItemSlots[i].ItemInstance is WeedInstance)
				{
					num += (base.Inventory.ItemSlots[i].ItemInstance as WeedInstance).Amount * base.Inventory.ItemSlots[i].Quantity;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_WeedCount", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_CashAmount", base.Cash.ToString(), true);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x00077B8C File Offset: 0x00075D8C
		protected override void AddCustomer(Customer customer)
		{
			base.AddCustomer(customer);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_CustomerCount", this.AssignedCustomers.Count.ToString(), true);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x00077BC4 File Offset: 0x00075DC4
		public override void RemoveCustomer(Customer customer)
		{
			base.RemoveCustomer(customer);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Benji_CustomerCount", this.AssignedCustomers.Count.ToString(), true);
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x00077BFB File Offset: 0x00075DFB
		protected override void RecruitmentRequested()
		{
			base.RecruitmentRequested();
			if (this.onRecruitmentRequested != null)
			{
				this.onRecruitmentRequested.Invoke();
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x00077C16 File Offset: 0x00075E16
		protected override void UpdatePotentialDealerPoI()
		{
			base.UpdatePotentialDealerPoI();
			base.potentialDealerPoI.enabled = false;
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00077C2A File Offset: 0x00075E2A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00077C43 File Offset: 0x00075E43
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BenjiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00077C5C File Offset: 0x00075E5C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00077C6A File Offset: 0x00075E6A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017AD RID: 6061
		public UnityEvent onRecruitmentRequested;

		// Token: 0x040017AE RID: 6062
		private bool dll_Excuted;

		// Token: 0x040017AF RID: 6063
		private bool dll_Excuted;
	}
}
