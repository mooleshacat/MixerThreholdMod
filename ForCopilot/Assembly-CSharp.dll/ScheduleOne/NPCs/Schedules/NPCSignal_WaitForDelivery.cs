using System;
using FishNet;
using ScheduleOne.Economy;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004C0 RID: 1216
	public class NPCSignal_WaitForDelivery : NPCSignal
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x0007376C File Offset: 0x0007196C
		public new string ActionName
		{
			get
			{
				return "Wait for delivery";
			}
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00073773 File Offset: 0x00071973
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCSignal_WaitForDelivery_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00073787 File Offset: 0x00071987
		protected override void OnValidate()
		{
			base.OnValidate();
			this.priority = 100;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x00073797 File Offset: 0x00071997
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0007379F File Offset: 0x0007199F
		public override void Started()
		{
			base.Started();
			base.SetDestination(this.Location.CustomerStandPoint.position, true);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000737C0 File Offset: 0x000719C0
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (this.npc.Movement.IsMoving)
			{
				if (Vector3.Distance(this.npc.Movement.CurrentDestination, this.Location.CustomerStandPoint.position) > 1.5f)
				{
					base.SetDestination(this.Location.CustomerStandPoint.position, true);
				}
				return;
			}
			if (!this.IsAtDestination())
			{
				base.SetDestination(this.Location.CustomerStandPoint.position, true);
				return;
			}
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0007385A File Offset: 0x00071A5A
		public override void LateStarted()
		{
			base.LateStarted();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.SetDestination(this.Location.CustomerStandPoint.position, true);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00073892 File Offset: 0x00071A92
		public override void JumpTo()
		{
			base.JumpTo();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.SetDestination(this.Location.CustomerStandPoint.position, true);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000738CA File Offset: 0x00071ACA
		public override void Interrupt()
		{
			base.Interrupt();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(false);
			this.npc.Movement.Stop();
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000738F3 File Offset: 0x00071AF3
		public override void Resume()
		{
			base.Resume();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0007390C File Offset: 0x00071B0C
		public override void End()
		{
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(false);
			base.StartedThisCycle = false;
			base.End();
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0007392C File Offset: 0x00071B2C
		public override void Skipped()
		{
			base.Skipped();
			if (InstanceFinder.IsServer)
			{
				this.npc.Movement.Warp(this.Location.CustomerStandPoint.position);
			}
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0007395B File Offset: 0x00071B5B
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Location.CustomerStandPoint.position) < 1.5f;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x0007398C File Offset: 0x00071B8C
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				return;
			}
			this.npc.Movement.FaceDirection(this.Location.CustomerStandPoint.forward, 0.5f);
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000739E4 File Offset: 0x00071BE4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000739FD File Offset: 0x00071BFD
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00073A16 File Offset: 0x00071C16
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00073A24 File Offset: 0x00071C24
		protected override void dll()
		{
			base.Awake();
			this.priority = 1000;
			this.MaxDuration = 720;
		}

		// Token: 0x040016A0 RID: 5792
		public const float DESTINATION_THRESHOLD = 1.5f;

		// Token: 0x040016A1 RID: 5793
		public DeliveryLocation Location;

		// Token: 0x040016A2 RID: 5794
		private bool dll_Excuted;

		// Token: 0x040016A3 RID: 5795
		private bool dll_Excuted;
	}
}
