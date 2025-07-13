using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200051F RID: 1311
	public class Behaviour : NetworkBehaviour
	{
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x00077E22 File Offset: 0x00076022
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x00077E2A File Offset: 0x0007602A
		public bool Enabled { get; protected set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x00077E33 File Offset: 0x00076033
		// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x00077E3B File Offset: 0x0007603B
		public bool Started { get; private set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x00077E44 File Offset: 0x00076044
		// (set) Token: 0x06001CEA RID: 7402 RVA: 0x00077E4C File Offset: 0x0007604C
		public bool Active { get; private set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x00077E55 File Offset: 0x00076055
		// (set) Token: 0x06001CEC RID: 7404 RVA: 0x00077E5D File Offset: 0x0007605D
		public NPCBehaviour beh { get; private set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x00077E66 File Offset: 0x00076066
		public NPC Npc
		{
			get
			{
				return this.beh.Npc;
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x00077E73 File Offset: 0x00076073
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.Behaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x00077E87 File Offset: 0x00076087
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			if (this.Enabled)
			{
				this.Enable_Networked(connection);
				return;
			}
			this.Disable_Networked(connection);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00077EB0 File Offset: 0x000760B0
		protected override void OnValidate()
		{
			base.OnValidate();
			this.UpdateGameObjectName();
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x00077EC0 File Offset: 0x000760C0
		public virtual void Enable()
		{
			if (this.Npc.behaviour.DEBUG_MODE)
			{
				Debug.Log(this.Name + " enabled");
			}
			this.Enabled = true;
			if (this.onEnable != null)
			{
				this.onEnable.Invoke();
			}
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00077F0E File Offset: 0x0007610E
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendEnable()
		{
			this.RpcWriter___Server_SendEnable_2166136261();
			this.RpcLogic___SendEnable_2166136261();
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00077F1C File Offset: 0x0007611C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Enable_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Enable_Networked_328543758(conn);
				this.RpcLogic___Enable_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Enable_Networked_328543758(conn);
			}
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00077F48 File Offset: 0x00076148
		public virtual void Disable()
		{
			if (this.Npc.behaviour.DEBUG_MODE)
			{
				Debug.Log(this.Name + " disabled");
			}
			this.Enabled = false;
			this.Started = false;
			if (this.Active)
			{
				this.End();
			}
			if (this.onDisable != null)
			{
				this.onDisable.Invoke();
			}
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x00077FAB File Offset: 0x000761AB
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendDisable()
		{
			this.RpcWriter___Server_SendDisable_2166136261();
			this.RpcLogic___SendDisable_2166136261();
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x00077FB9 File Offset: 0x000761B9
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Disable_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Disable_Networked_328543758(conn);
				this.RpcLogic___Disable_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Disable_Networked_328543758(conn);
			}
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00077FE4 File Offset: 0x000761E4
		private void UpdateGameObjectName()
		{
			if (base.gameObject == null)
			{
				return;
			}
			base.gameObject.name = this.Name + (this.Active ? " (Active)" : " (Inactive)");
			if (!this.Active)
			{
				base.gameObject.name = base.gameObject.name + (this.Enabled ? " (Enabled)" : " (Disabled)");
			}
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00078061 File Offset: 0x00076261
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Begin_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Begin_Networked_328543758(conn);
				this.RpcLogic___Begin_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Begin_Networked_328543758(conn);
			}
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x0007808C File Offset: 0x0007628C
		protected virtual void Begin()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") started", null);
			}
			this.Started = true;
			this.Active = true;
			this.beh.activeBehaviour = this;
			this.UpdateGameObjectName();
			if (this.onBegin != null)
			{
				this.onBegin.Invoke();
			}
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000780F4 File Offset: 0x000762F4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendEnd()
		{
			this.RpcWriter___Server_SendEnd_2166136261();
			this.RpcLogic___SendEnd_2166136261();
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x00078102 File Offset: 0x00076302
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void End_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_End_Networked_328543758(conn);
				this.RpcLogic___End_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_End_Networked_328543758(conn);
			}
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x0007812C File Offset: 0x0007632C
		protected virtual void End()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") ended", null);
			}
			this.Active = false;
			this.beh.activeBehaviour = null;
			this.UpdateGameObjectName();
			if (this.onEnd != null)
			{
				this.onEnd.Invoke();
			}
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x0007818D File Offset: 0x0007638D
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Pause_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Pause_Networked_328543758(conn);
				this.RpcLogic___Pause_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Pause_Networked_328543758(conn);
			}
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000781B7 File Offset: 0x000763B7
		protected virtual void Pause()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") paused", null);
			}
			this.Active = false;
			this.UpdateGameObjectName();
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000781EE File Offset: 0x000763EE
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Resume_Networked(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Resume_Networked_328543758(conn);
				this.RpcLogic___Resume_Networked_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Resume_Networked_328543758(conn);
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x00078218 File Offset: 0x00076418
		protected virtual void Resume()
		{
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Behaviour (" + this.Name + ") resumed", null);
			}
			this.Active = true;
			this.beh.activeBehaviour = this;
			this.UpdateGameObjectName();
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void BehaviourUpdate()
		{
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void BehaviourLateUpdate()
		{
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ActiveMinPass()
		{
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x00078266 File Offset: 0x00076466
		protected void SetPriority(int p)
		{
			this.Priority = p;
			this.beh.SortBehaviourStack();
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0007827A File Offset: 0x0007647A
		protected void SetDestination(ITransitEntity transitEntity, bool teleportIfFail = true)
		{
			this.SetDestination(NavMeshUtility.GetAccessPoint(transitEntity, this.Npc).position, teleportIfFail);
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00078294 File Offset: 0x00076494
		protected void SetDestination(Vector3 position, bool teleportIfFail = true)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (teleportIfFail && this.consecutivePathingFailures >= 5 && !this.Npc.Movement.CanGetTo(position, 1f))
			{
				Console.LogWarning(this.Npc.fullName + " too many pathing failures. Warping to " + position.ToString(), null);
				this.Npc.Movement.Warp(position);
				this.WalkCallback(NPCMovement.WalkResult.Success);
			}
			this.Npc.Movement.SetDestination(position, new Action<NPCMovement.WalkResult>(this.WalkCallback), 1f, 0.1f);
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x00078338 File Offset: 0x00076538
		protected virtual void WalkCallback(NPCMovement.WalkResult result)
		{
			if (!this.Active)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Failed)
			{
				this.consecutivePathingFailures++;
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
			if (this.beh.DEBUG_MODE)
			{
				Console.Log("Walk callback result: " + result.ToString(), null);
			}
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000783BC File Offset: 0x000765BC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendEnable_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Enable_Networked_328543758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_Enable_Networked_328543758));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendDisable_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Disable_Networked_328543758));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_Disable_Networked_328543758));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_Begin_Networked_328543758));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_Begin_Networked_328543758));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendEnd_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_End_Networked_328543758));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_End_Networked_328543758));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_Pause_Networked_328543758));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_Pause_Networked_328543758));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_Resume_Networked_328543758));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_Resume_Networked_328543758));
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x00078533 File Offset: 0x00076733
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BehaviourAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x00078546 File Offset: 0x00076746
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00078554 File Offset: 0x00076754
		private void RpcWriter___Server_SendEnable_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000785EE File Offset: 0x000767EE
		public void RpcLogic___SendEnable_2166136261()
		{
			this.Enable_Networked(null);
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000785F8 File Offset: 0x000767F8
		private void RpcReader___Server_SendEnable_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEnable_2166136261();
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00078628 File Offset: 0x00076828
		private void RpcWriter___Observers_Enable_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000786D1 File Offset: 0x000768D1
		public void RpcLogic___Enable_Networked_328543758(NetworkConnection conn)
		{
			this.Enable();
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000786DC File Offset: 0x000768DC
		private void RpcReader___Observers_Enable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Enable_Networked_328543758(null);
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00078708 File Offset: 0x00076908
		private void RpcWriter___Target_Enable_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000787B0 File Offset: 0x000769B0
		private void RpcReader___Target_Enable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000787D8 File Offset: 0x000769D8
		private void RpcWriter___Server_SendDisable_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x00078872 File Offset: 0x00076A72
		public void RpcLogic___SendDisable_2166136261()
		{
			this.Disable_Networked(null);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x0007887C File Offset: 0x00076A7C
		private void RpcReader___Server_SendDisable_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDisable_2166136261();
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000788AC File Offset: 0x00076AAC
		private void RpcWriter___Observers_Disable_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00078955 File Offset: 0x00076B55
		public void RpcLogic___Disable_Networked_328543758(NetworkConnection conn)
		{
			this.Disable();
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00078960 File Offset: 0x00076B60
		private void RpcReader___Observers_Disable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Disable_Networked_328543758(null);
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x0007898C File Offset: 0x00076B8C
		private void RpcWriter___Target_Disable_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(5U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00078A34 File Offset: 0x00076C34
		private void RpcReader___Target_Disable_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Disable_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x00078A5C File Offset: 0x00076C5C
		private void RpcWriter___Observers_Begin_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00078B05 File Offset: 0x00076D05
		public void RpcLogic___Begin_Networked_328543758(NetworkConnection conn)
		{
			this.Begin();
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00078B10 File Offset: 0x00076D10
		private void RpcReader___Observers_Begin_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Begin_Networked_328543758(null);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x00078B3C File Offset: 0x00076D3C
		private void RpcWriter___Target_Begin_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(7U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x00078BE4 File Offset: 0x00076DE4
		private void RpcReader___Target_Begin_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Begin_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00078C0C File Offset: 0x00076E0C
		private void RpcWriter___Server_SendEnd_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00078CA6 File Offset: 0x00076EA6
		public void RpcLogic___SendEnd_2166136261()
		{
			this.End_Networked(null);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x00078CB0 File Offset: 0x00076EB0
		private void RpcReader___Server_SendEnd_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEnd_2166136261();
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00078CE0 File Offset: 0x00076EE0
		private void RpcWriter___Observers_End_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x00078D89 File Offset: 0x00076F89
		public void RpcLogic___End_Networked_328543758(NetworkConnection conn)
		{
			this.End();
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00078D94 File Offset: 0x00076F94
		private void RpcReader___Observers_End_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___End_Networked_328543758(null);
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00078DC0 File Offset: 0x00076FC0
		private void RpcWriter___Target_End_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(10U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00078E68 File Offset: 0x00077068
		private void RpcReader___Target_End_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___End_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00078E90 File Offset: 0x00077090
		private void RpcWriter___Observers_Pause_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00078F39 File Offset: 0x00077139
		public void RpcLogic___Pause_Networked_328543758(NetworkConnection conn)
		{
			this.Pause();
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00078F44 File Offset: 0x00077144
		private void RpcReader___Observers_Pause_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Pause_Networked_328543758(null);
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x00078F70 File Offset: 0x00077170
		private void RpcWriter___Target_Pause_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(12U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x00079018 File Offset: 0x00077218
		private void RpcReader___Target_Pause_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Pause_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x00079040 File Offset: 0x00077240
		private void RpcWriter___Observers_Resume_Networked_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(13U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000790E9 File Offset: 0x000772E9
		public void RpcLogic___Resume_Networked_328543758(NetworkConnection conn)
		{
			this.Resume();
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000790F4 File Offset: 0x000772F4
		private void RpcReader___Observers_Resume_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Resume_Networked_328543758(null);
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x00079120 File Offset: 0x00077320
		private void RpcWriter___Target_Resume_Networked_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(14U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x000791C8 File Offset: 0x000773C8
		private void RpcReader___Target_Resume_Networked_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Resume_Networked_328543758(base.LocalConnection);
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x000791EE File Offset: 0x000773EE
		protected virtual void dll()
		{
			this.beh = base.GetComponentInParent<NPCBehaviour>();
			this.Enabled = this.EnabledOnAwake;
		}

		// Token: 0x040017B8 RID: 6072
		public const int MAX_CONSECUTIVE_PATHING_FAILURES = 5;

		// Token: 0x040017B9 RID: 6073
		public bool EnabledOnAwake;

		// Token: 0x040017BB RID: 6075
		[Header("Settings")]
		public string Name = "Behaviour";

		// Token: 0x040017BC RID: 6076
		[Tooltip("Behaviour priority; higher = takes priority over lower number behaviour")]
		public int Priority;

		// Token: 0x040017C0 RID: 6080
		public UnityEvent onEnable = new UnityEvent();

		// Token: 0x040017C1 RID: 6081
		public UnityEvent onDisable = new UnityEvent();

		// Token: 0x040017C2 RID: 6082
		public UnityEvent onBegin;

		// Token: 0x040017C3 RID: 6083
		public UnityEvent onEnd;

		// Token: 0x040017C4 RID: 6084
		protected int consecutivePathingFailures;

		// Token: 0x040017C5 RID: 6085
		private bool dll_Excuted;

		// Token: 0x040017C6 RID: 6086
		private bool dll_Excuted;
	}
}
