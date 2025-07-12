using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200054B RID: 1355
	public class StartDryingRackBehaviour : Behaviour
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x00082760 File Offset: 0x00080960
		// (set) Token: 0x06001FA5 RID: 8101 RVA: 0x00082768 File Offset: 0x00080968
		public DryingRack Rack { get; protected set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x00082771 File Offset: 0x00080971
		// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x00082779 File Offset: 0x00080979
		public bool WorkInProgress { get; protected set; }

		// Token: 0x06001FA8 RID: 8104 RVA: 0x00082782 File Offset: 0x00080982
		protected override void Begin()
		{
			base.Begin();
			this.StartWork();
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x00082790 File Offset: 0x00080990
		protected override void Resume()
		{
			base.Resume();
			this.StartWork();
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000827A0 File Offset: 0x000809A0
		protected override void Pause()
		{
			base.Pause();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Rack != null && this.Rack.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Rack.SetNPCUser(null);
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x00082800 File Offset: 0x00080A00
		protected override void End()
		{
			base.End();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Rack != null && this.Rack.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Rack.SetNPCUser(null);
			}
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x00082860 File Offset: 0x00080A60
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.WorkInProgress)
			{
				if (this.IsRackReady(this.Rack))
				{
					if (base.Npc.Movement.IsMoving)
					{
						return;
					}
					if (this.IsAtStation())
					{
						this.BeginAction();
						return;
					}
					this.GoToStation();
					return;
				}
				else
				{
					base.Disable_Networked(null);
				}
			}
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x000828C4 File Offset: 0x00080AC4
		private void StartWork()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsRackReady(this.Rack))
			{
				Console.LogWarning(base.Npc.fullName + " has no station to work with", null);
				base.Disable_Networked(null);
				return;
			}
			this.Rack.SetNPCUser(base.Npc.NetworkObject);
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00082920 File Offset: 0x00080B20
		public void AssignRack(DryingRack rack)
		{
			if (this.Rack == rack)
			{
				return;
			}
			if (this.Rack != null && this.Rack.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Rack.SetNPCUser(null);
			}
			this.Rack = rack;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0008297A File Offset: 0x00080B7A
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position, 0.5f);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000829A7 File Offset: 0x00080BA7
		public void GoToStation()
		{
			base.SetDestination(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position, true);
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000829C8 File Offset: 0x00080BC8
		[ObserversRpc(RunLocally = true)]
		public void BeginAction()
		{
			this.RpcWriter___Observers_BeginAction_2166136261();
			this.RpcLogic___BeginAction_2166136261();
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000829E1 File Offset: 0x00080BE1
		private void StopCauldron()
		{
			if (this.workRoutine != null)
			{
				base.StopCoroutine(this.workRoutine);
			}
			this.WorkInProgress = false;
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x00082A00 File Offset: 0x00080C00
		public bool IsRackReady(DryingRack rack)
		{
			return !(rack == null) && (!((IUsable)rack).IsInUse || (!(rack.PlayerUserObject != null) && !(rack.NPCUserObject != base.Npc.NetworkObject))) && rack.InputSlot.Quantity > 0 && rack.GetTotalDryingItems() < rack.ItemCapacity && base.Npc.Movement.CanGetTo(rack.transform.position, 1f);
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x00082A8D File Offset: 0x00080C8D
		[CompilerGenerated]
		private IEnumerator <BeginAction>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			this.Rack.InputSlot.ItemInstance.GetCopy(1);
			int itemCount = 0;
			while (this.Rack != null && this.Rack.InputSlot.Quantity > itemCount && this.Rack.GetTotalDryingItems() + itemCount < this.Rack.ItemCapacity)
			{
				base.Npc.Avatar.Anim.SetTrigger("GrabItem");
				yield return new WaitForSeconds(1f);
				int num = itemCount;
				itemCount = num + 1;
			}
			if (InstanceFinder.IsServer)
			{
				this.Rack.StartOperation();
			}
			this.WorkInProgress = false;
			this.workRoutine = null;
			yield break;
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x00082A9C File Offset: 0x00080C9C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginAction_2166136261));
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00082ACC File Offset: 0x00080CCC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x00082AE5 File Offset: 0x00080CE5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x00082AF4 File Offset: 0x00080CF4
		private void RpcWriter___Observers_BeginAction_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00082BA0 File Offset: 0x00080DA0
		public void RpcLogic___BeginAction_2166136261()
		{
			if (this.WorkInProgress)
			{
				return;
			}
			if (this.Rack == null)
			{
				return;
			}
			this.WorkInProgress = true;
			base.Npc.Movement.FacePoint(this.Rack.uiPoint.position, 0.5f);
			this.workRoutine = base.StartCoroutine(this.<BeginAction>g__Package|20_0());
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00082C04 File Offset: 0x00080E04
		private void RpcReader___Observers_BeginAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginAction_2166136261();
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00082C2E File Offset: 0x00080E2E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018BE RID: 6334
		public const float TIME_PER_ITEM = 1f;

		// Token: 0x040018C1 RID: 6337
		private Coroutine workRoutine;

		// Token: 0x040018C2 RID: 6338
		private bool dll_Excuted;

		// Token: 0x040018C3 RID: 6339
		private bool dll_Excuted;
	}
}
