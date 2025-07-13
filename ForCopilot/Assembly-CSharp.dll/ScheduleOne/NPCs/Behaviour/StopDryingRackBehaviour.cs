using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x02000551 RID: 1361
	public class StopDryingRackBehaviour : Behaviour
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x000838E2 File Offset: 0x00081AE2
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x000838EA File Offset: 0x00081AEA
		public DryingRack Rack { get; protected set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x000838F3 File Offset: 0x00081AF3
		// (set) Token: 0x06001FFC RID: 8188 RVA: 0x000838FB File Offset: 0x00081AFB
		public bool WorkInProgress { get; protected set; }

		// Token: 0x06001FFD RID: 8189 RVA: 0x00083904 File Offset: 0x00081B04
		protected override void Begin()
		{
			base.Begin();
			this.StartWork();
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00083912 File Offset: 0x00081B12
		protected override void Resume()
		{
			base.Resume();
			this.StartWork();
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00083920 File Offset: 0x00081B20
		protected override void Pause()
		{
			base.Pause();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0007C40F File Offset: 0x0007A60F
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x00083938 File Offset: 0x00081B38
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

		// Token: 0x06002002 RID: 8194 RVA: 0x00083998 File Offset: 0x00081B98
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

		// Token: 0x06002003 RID: 8195 RVA: 0x000839FC File Offset: 0x00081BFC
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

		// Token: 0x06002004 RID: 8196 RVA: 0x00083A58 File Offset: 0x00081C58
		public void AssignRack(DryingRack rack)
		{
			if (this.Rack == rack)
			{
				return;
			}
			if (this.Rack != null && this.Rack.NPCUserObject == base.Npc.NetworkObject)
			{
				Console.Log("Clearing NPC user from previous rack: " + this.Rack.name, null);
				this.Rack.SetNPCUser(null);
			}
			this.Rack = rack;
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00083ACD File Offset: 0x00081CCD
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position, 0.5f);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00083AFA File Offset: 0x00081CFA
		public void GoToStation()
		{
			base.SetDestination(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position, true);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00083B1C File Offset: 0x00081D1C
		[ObserversRpc(RunLocally = true)]
		public void BeginAction()
		{
			this.RpcWriter___Observers_BeginAction_2166136261();
			this.RpcLogic___BeginAction_2166136261();
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00083B35 File Offset: 0x00081D35
		private void StopCauldron()
		{
			if (this.workRoutine != null)
			{
				base.StopCoroutine(this.workRoutine);
			}
			this.WorkInProgress = false;
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00083B54 File Offset: 0x00081D54
		public bool IsRackReady(DryingRack rack)
		{
			if (rack == null)
			{
				return false;
			}
			if (((IUsable)rack).IsInUse && (rack.PlayerUserObject != null || rack.NPCUserObject != base.Npc.NetworkObject))
			{
				return false;
			}
			List<DryingOperation> operationsAtTargetQuality = rack.GetOperationsAtTargetQuality();
			bool flag = false;
			foreach (DryingOperation dryingOperation in operationsAtTargetQuality)
			{
				if (rack.GetOutputCapacityForOperation(dryingOperation, dryingOperation.GetQuality()) > 0)
				{
					flag = true;
				}
			}
			return flag && base.Npc.Movement.CanGetTo(rack.transform.position, 1f);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00083C1C File Offset: 0x00081E1C
		[CompilerGenerated]
		private IEnumerator <BeginAction>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetTrigger("GrabItem");
			yield return new WaitForSeconds(0.5f);
			if (InstanceFinder.IsServer)
			{
				DryingOperation dryingOperation = this.Rack.GetOperationsAtTargetQuality().FirstOrDefault((DryingOperation x) => this.Rack.GetOutputCapacityForOperation(x, x.GetQuality()) > 0);
				if (dryingOperation != null)
				{
					this.Rack.TryEndOperation(this.Rack.DryingOperations.IndexOf(dryingOperation), true, dryingOperation.GetQuality(), UnityEngine.Random.Range(int.MinValue, int.MaxValue));
				}
			}
			this.WorkInProgress = false;
			this.workRoutine = null;
			yield break;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00083C42 File Offset: 0x00081E42
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginAction_2166136261));
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x00083C72 File Offset: 0x00081E72
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00083C8B File Offset: 0x00081E8B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00083C9C File Offset: 0x00081E9C
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

		// Token: 0x06002011 RID: 8209 RVA: 0x00083D48 File Offset: 0x00081F48
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

		// Token: 0x06002012 RID: 8210 RVA: 0x00083DAC File Offset: 0x00081FAC
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

		// Token: 0x06002013 RID: 8211 RVA: 0x00083DD6 File Offset: 0x00081FD6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018DE RID: 6366
		public const float TIME_PER_ITEM = 1f;

		// Token: 0x040018E1 RID: 6369
		private Coroutine workRoutine;

		// Token: 0x040018E2 RID: 6370
		private bool dll_Excuted;

		// Token: 0x040018E3 RID: 6371
		private bool dll_Excuted;
	}
}
