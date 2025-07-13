using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Dialogue;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B0 RID: 1200
	public class NPCEvent_LocationDialogue : NPCEvent
	{
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x000700CA File Offset: 0x0006E2CA
		public new string ActionName
		{
			get
			{
				return "Location-based dialogue";
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x000700D4 File Offset: 0x0006E2D4
		public override string GetName()
		{
			if (this.Destination == null)
			{
				return this.ActionName + " (No destination set)";
			}
			string actionName = this.ActionName;
			string str = " (";
			Transform destination = this.Destination;
			return actionName + str + ((destination != null) ? destination.name : null) + ")";
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00070127 File Offset: 0x0006E327
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

		// Token: 0x06001979 RID: 6521 RVA: 0x00070148 File Offset: 0x0006E348
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

		// Token: 0x0600197A RID: 6522 RVA: 0x00070174 File Offset: 0x0006E374
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
			else if (this.IsAtDestination())
			{
				if (this.FaceDestinationDir && !this.npc.Movement.FaceDirectionInProgress && Vector3.Angle(base.transform.forward, this.Destination.forward) > 5f)
				{
					this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
					return;
				}
			}
			else
			{
				base.SetDestination(this.Destination.position, true);
			}
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00070256 File Offset: 0x0006E456
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

		// Token: 0x0600197C RID: 6524 RVA: 0x00070280 File Offset: 0x0006E480
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
				this.WalkCallback(NPCMovement.WalkResult.Success);
			}
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x000702ED File Offset: 0x0006E4ED
		public override void End()
		{
			base.End();
			if (this.IsActionStarted)
			{
				this.EndAction();
			}
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00070303 File Offset: 0x0006E503
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

		// Token: 0x0600197F RID: 6527 RVA: 0x0007033B File Offset: 0x0006E53B
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

		// Token: 0x06001980 RID: 6528 RVA: 0x00070365 File Offset: 0x0006E565
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.npc.Movement.Warp(this.Destination.position);
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00070390 File Offset: 0x0006E590
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Destination.position) < this.DestinationThreshold;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x000703BA File Offset: 0x0006E5BA
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

		// Token: 0x06001983 RID: 6531 RVA: 0x000703E0 File Offset: 0x0006E5E0
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

		// Token: 0x06001984 RID: 6532 RVA: 0x00070418 File Offset: 0x0006E618
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndAction()
		{
			this.RpcWriter___Observers_EndAction_2166136261();
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0007045C File Offset: 0x0006E65C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_StartAction_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndAction_2166136261));
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x000704C5 File Offset: 0x0006E6C5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x000704DE File Offset: 0x0006E6DE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x000704EC File Offset: 0x0006E6EC
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

		// Token: 0x0600198A RID: 6538 RVA: 0x00070598 File Offset: 0x0006E798
		protected virtual void RpcLogic___StartAction_328543758(NetworkConnection conn)
		{
			if (this.IsActionStarted)
			{
				Console.LogWarning("Dialogue action already started", null);
				return;
			}
			if (this.FaceDestinationDir)
			{
				this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
			}
			this.IsActionStarted = true;
			DialogueController component = this.npc.dialogueHandler.GetComponent<DialogueController>();
			if (this.DialogueOverride != null)
			{
				component.OverrideContainer = this.DialogueOverride;
				return;
			}
			component.OverrideContainer = null;
			if (component.GreetingOverrides.Count > this.GreetingOverrideToEnable && this.GreetingOverrideToEnable >= 0)
			{
				component.GreetingOverrides[this.GreetingOverrideToEnable].ShouldShow = true;
			}
			if (component.Choices.Count > this.ChoiceToEnable && this.ChoiceToEnable >= 0)
			{
				component.Choices[this.ChoiceToEnable].Enabled = true;
			}
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00070684 File Offset: 0x0006E884
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

		// Token: 0x0600198C RID: 6540 RVA: 0x000706B0 File Offset: 0x0006E8B0
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

		// Token: 0x0600198D RID: 6541 RVA: 0x00070758 File Offset: 0x0006E958
		private void RpcReader___Target_StartAction_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartAction_328543758(base.LocalConnection);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00070780 File Offset: 0x0006E980
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

		// Token: 0x0600198F RID: 6543 RVA: 0x0007082C File Offset: 0x0006EA2C
		protected virtual void RpcLogic___EndAction_2166136261()
		{
			if (!this.IsActionStarted)
			{
				return;
			}
			this.IsActionStarted = false;
			DialogueController component = this.npc.dialogueHandler.GetComponent<DialogueController>();
			if (this.DialogueOverride != null)
			{
				component.OverrideContainer = null;
				return;
			}
			if (component.GreetingOverrides.Count > this.GreetingOverrideToEnable && this.GreetingOverrideToEnable >= 0)
			{
				component.GreetingOverrides[this.GreetingOverrideToEnable].ShouldShow = false;
			}
			if (component.Choices.Count > this.ChoiceToEnable && this.ChoiceToEnable >= 0)
			{
				component.Choices[this.ChoiceToEnable].Enabled = false;
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x000708D8 File Offset: 0x0006EAD8
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

		// Token: 0x06001991 RID: 6545 RVA: 0x00070902 File Offset: 0x0006EB02
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001647 RID: 5703
		public Transform Destination;

		// Token: 0x04001648 RID: 5704
		public bool FaceDestinationDir = true;

		// Token: 0x04001649 RID: 5705
		public float DestinationThreshold = 1f;

		// Token: 0x0400164A RID: 5706
		public bool WarpIfSkipped;

		// Token: 0x0400164B RID: 5707
		[Header("Dialogue Settings")]
		public int GreetingOverrideToEnable = -1;

		// Token: 0x0400164C RID: 5708
		public int ChoiceToEnable = -1;

		// Token: 0x0400164D RID: 5709
		public DialogueContainer DialogueOverride;

		// Token: 0x0400164E RID: 5710
		protected bool IsActionStarted;

		// Token: 0x0400164F RID: 5711
		private bool dll_Excuted;

		// Token: 0x04001650 RID: 5712
		private bool dll_Excuted;
	}
}
