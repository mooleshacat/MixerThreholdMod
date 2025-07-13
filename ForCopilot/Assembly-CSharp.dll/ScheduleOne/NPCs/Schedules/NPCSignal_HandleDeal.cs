using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Quests;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004BA RID: 1210
	public class NPCSignal_HandleDeal : NPCSignal
	{
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0007283E File Offset: 0x00070A3E
		public new string ActionName
		{
			get
			{
				return "Handle deal";
			}
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00072845 File Offset: 0x00070A45
		public void AssignContract(Contract c)
		{
			this.contract = c;
			if (this.contract != null)
			{
				this.customer = c.Customer.GetComponent<Customer>();
			}
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0007286D File Offset: 0x00070A6D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCSignal_HandleDeal_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00072881 File Offset: 0x00070A81
		protected override void OnValidate()
		{
			base.OnValidate();
			this.priority = 10;
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00072891 File Offset: 0x00070A91
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00072899 File Offset: 0x00070A99
		public override void Started()
		{
			base.Started();
			base.SetDestination(this.GetStandPos(), true);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000728B0 File Offset: 0x00070AB0
		public override void MinPassed()
		{
			base.MinPassed();
			if (!base.IsActive)
			{
				return;
			}
			if (this.contract == null || this.contract.QuestState != EQuestState.Active)
			{
				this.End();
				base.gameObject.SetActive(false);
				this.contract = null;
				base.StartedThisCycle = false;
				return;
			}
			if (this.handoverRoutine != null)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.IsAtDestination())
				{
					if (this.IsCustomerReady())
					{
						this.BeginHandover();
						return;
					}
				}
				else
				{
					base.SetDestination(this.GetStandPos(), true);
				}
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x00072948 File Offset: 0x00070B48
		public override void LateStarted()
		{
			base.LateStarted();
			base.SetDestination(this.GetStandPos(), true);
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0007295D File Offset: 0x00070B5D
		public override void JumpTo()
		{
			base.JumpTo();
			base.SetDestination(this.GetStandPos(), true);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00072972 File Offset: 0x00070B72
		public override void Interrupt()
		{
			base.Interrupt();
			this.npc.Movement.Stop();
			this.StopHandover();
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00072990 File Offset: 0x00070B90
		public override void End()
		{
			base.End();
			this.StopHandover();
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00071594 File Offset: 0x0006F794
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0007299E File Offset: 0x00070B9E
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.GetStandPos()) < 2f;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000729C2 File Offset: 0x00070BC2
		private bool IsCustomerReady()
		{
			return this.customer.IsAtDealLocation();
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000729CF File Offset: 0x00070BCF
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				Debug.LogWarning(this.npc.fullName + ": walk to location not successful");
				return;
			}
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00072A00 File Offset: 0x00070C00
		private void BeginHandover()
		{
			if (this.handoverRoutine != null)
			{
				return;
			}
			this.handoverRoutine = base.StartCoroutine(this.<BeginHandover>g__Routine|20_0());
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00072A1D File Offset: 0x00070C1D
		private void StopHandover()
		{
			if (this.handoverRoutine != null)
			{
				base.StopCoroutine(this.handoverRoutine);
				this.handoverRoutine = null;
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00072A3C File Offset: 0x00070C3C
		private Vector3 GetStandPos()
		{
			if (this.contract == null)
			{
				return Vector3.zero;
			}
			return this.contract.DeliveryLocation.CustomerStandPoint.position + this.contract.DeliveryLocation.CustomerStandPoint.forward * 1.2f;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00072A96 File Offset: 0x00070C96
		private Vector3 GetStandDir()
		{
			return -this.contract.DeliveryLocation.CustomerStandPoint.forward;
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x00072AB2 File Offset: 0x00070CB2
		[CompilerGenerated]
		private IEnumerator <BeginHandover>g__Routine|20_0()
		{
			this.npc.Movement.FaceDirection(this.GetStandDir(), 0.5f);
			yield return new WaitForSeconds(2f);
			yield return new WaitUntil(() => this.customer.IsAtDealLocation());
			List<ItemInstance> items;
			if (!this.dealer.RemoveContractItems(this.contract, this.customer.CustomerData.Standards.GetCorrespondingQuality(), out items))
			{
				Console.LogWarning("Dealer does not have items for contract. Contract will still be marked as complete.", null);
			}
			bool flag;
			this.customer.OfferDealItems(items, false, out flag);
			this.npc.SetAnimationTrigger("GrabItem");
			this.End();
			base.gameObject.SetActive(false);
			this.contract = null;
			base.StartedThisCycle = false;
			this.handoverRoutine = null;
			yield break;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x00072AC1 File Offset: 0x00070CC1
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00072ADA File Offset: 0x00070CDA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_HandleDealAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00072AF3 File Offset: 0x00070CF3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x00072B01 File Offset: 0x00070D01
		protected override void dll()
		{
			base.Awake();
			this.priority = 100;
			this.MaxDuration = 720;
			this.dealer = (this.npc as Dealer);
		}

		// Token: 0x04001686 RID: 5766
		private Dealer dealer;

		// Token: 0x04001687 RID: 5767
		private Contract contract;

		// Token: 0x04001688 RID: 5768
		private Customer customer;

		// Token: 0x04001689 RID: 5769
		private Coroutine handoverRoutine;

		// Token: 0x0400168A RID: 5770
		private bool dll_Excuted;

		// Token: 0x0400168B RID: 5771
		private bool dll_Excuted;
	}
}
