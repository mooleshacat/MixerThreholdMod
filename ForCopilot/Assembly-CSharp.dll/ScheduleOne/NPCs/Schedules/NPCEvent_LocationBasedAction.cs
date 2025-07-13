using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004AF RID: 1199
	public class NPCEvent_LocationBasedAction : NPCEvent
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x0006FA20 File Offset: 0x0006DC20
		public new string ActionName
		{
			get
			{
				return "Location-based action";
			}
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0006FA28 File Offset: 0x0006DC28
		public override string GetName()
		{
			if (this.Destination == null)
			{
				return this.ActionName + " (No destination set)";
			}
			return this.ActionName + " (" + this.Destination.name + ")";
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0006FA74 File Offset: 0x0006DC74
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!base.IsActive)
			{
				return;
			}
			if (this.IsActionStarted)
			{
				this.StartAction(connection);
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0006FA95 File Offset: 0x0006DC95
		public override void Started()
		{
			base.Started();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0006FAC0 File Offset: 0x0006DCC0
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.Movement.IsMoving)
			{
				if (Vector3.Distance(this.npc.Movement.CurrentDestination, this.Destination.position) > this.DestinationThreshold)
				{
					base.SetDestination(this.Destination.position, true);
					return;
				}
			}
			else if (!this.IsAtDestination())
			{
				base.SetDestination(this.Destination.position, true);
			}
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0006FB42 File Offset: 0x0006DD42
		public override void LateStarted()
		{
			base.LateStarted();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0006FB6C File Offset: 0x0006DD6C
		public override void JumpTo()
		{
			base.JumpTo();
			if (!this.IsAtDestination())
			{
				if (this.npc.Movement.IsMoving)
				{
					this.npc.Movement.Stop();
				}
				if (InstanceFinder.IsServer)
				{
					this.npc.Movement.Warp(this.Destination.position);
				}
			}
			if (InstanceFinder.IsServer)
			{
				this.StartAction(null);
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0006FBD9 File Offset: 0x0006DDD9
		public override void End()
		{
			base.End();
			if (this.IsActionStarted)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0006FBEF File Offset: 0x0006DDEF
		public override void Interrupt()
		{
			base.Interrupt();
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.IsActionStarted)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0006FC27 File Offset: 0x0006DE27
		public override void Resume()
		{
			base.Resume();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0006FC51 File Offset: 0x0006DE51
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.npc.Movement.Warp(this.Destination.position);
			}
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0006FC7C File Offset: 0x0006DE7C
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Destination.position) < this.DestinationThreshold;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0006FCA6 File Offset: 0x0006DEA6
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
			if (InstanceFinder.IsServer)
			{
				this.StartAction(null);
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0006FCCC File Offset: 0x0006DECC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		protected virtual void StartAction(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_StartAction_328543758(conn);
				this.RpcLogic___StartAction_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_StartAction_328543758(conn);
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0006FD01 File Offset: 0x0006DF01
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndAction()
		{
			this.RpcWriter___Observers_EndAction_2166136261();
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0006FD2C File Offset: 0x0006DF2C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_StartAction_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndAction_2166136261));
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0006FD95 File Offset: 0x0006DF95
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0006FDAE File Offset: 0x0006DFAE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0006FDBC File Offset: 0x0006DFBC
		private void RpcWriter___Observers_StartAction_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0006FE68 File Offset: 0x0006E068
		protected virtual void RpcLogic___StartAction_328543758(NetworkConnection conn)
		{
			if (this.IsActionStarted)
			{
				return;
			}
			if (this.FaceDestinationDir)
			{
				this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
			}
			this.IsActionStarted = true;
			if (this.onStartAction != null)
			{
				this.onStartAction.Invoke();
			}
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0006FEC0 File Offset: 0x0006E0C0
		private void RpcReader___Observers_StartAction_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartAction_328543758(null);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0006FEEC File Offset: 0x0006E0EC
		private void RpcWriter___Target_StartAction_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0006FF94 File Offset: 0x0006E194
		private void RpcReader___Target_StartAction_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartAction_328543758(base.LocalConnection);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0006FFBC File Offset: 0x0006E1BC
		private void RpcWriter___Observers_EndAction_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00070065 File Offset: 0x0006E265
		protected virtual void RpcLogic___EndAction_2166136261()
		{
			if (!this.IsActionStarted)
			{
				return;
			}
			this.IsActionStarted = false;
			if (this.onEndAction != null)
			{
				this.onEndAction.Invoke();
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0007008C File Offset: 0x0006E28C
		private void RpcReader___Observers_EndAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x000700B6 File Offset: 0x0006E2B6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400163E RID: 5694
		public Transform Destination;

		// Token: 0x0400163F RID: 5695
		public bool FaceDestinationDir = true;

		// Token: 0x04001640 RID: 5696
		public float DestinationThreshold = 1f;

		// Token: 0x04001641 RID: 5697
		public bool WarpIfSkipped;

		// Token: 0x04001642 RID: 5698
		public bool IsActionStarted;

		// Token: 0x04001643 RID: 5699
		public UnityEvent onStartAction;

		// Token: 0x04001644 RID: 5700
		public UnityEvent onEndAction;

		// Token: 0x04001645 RID: 5701
		private bool dll_Excuted;

		// Token: 0x04001646 RID: 5702
		private bool dll_Excuted;
	}
}
