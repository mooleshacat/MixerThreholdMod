using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000559 RID: 1369
	public class GenericDialogueBehaviour : Behaviour
	{
		// Token: 0x06002067 RID: 8295 RVA: 0x000851EC File Offset: 0x000833EC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendTargetPlayer(NetworkObject player)
		{
			this.RpcWriter___Server_SendTargetPlayer_3323014238(player);
			this.RpcLogic___SendTargetPlayer_3323014238(player);
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00085204 File Offset: 0x00083404
		[ObserversRpc(RunLocally = true)]
		private void SetTargetPlayer(NetworkObject player)
		{
			this.RpcWriter___Observers_SetTargetPlayer_3323014238(player);
			this.RpcLogic___SetTargetPlayer_3323014238(player);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x00085225 File Offset: 0x00083425
		public override void Enable()
		{
			base.Enable();
			base.beh.Update();
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x00085238 File Offset: 0x00083438
		protected override void Begin()
		{
			base.Begin();
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00085262 File Offset: 0x00083462
		protected override void Resume()
		{
			base.Resume();
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0008528C File Offset: 0x0008348C
		protected override void End()
		{
			base.End();
			base.Npc.Movement.ResumeMovement();
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000852A4 File Offset: 0x000834A4
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			if (!base.Npc.Movement.FaceDirectionInProgress && base.Npc.Avatar.Anim.TimeSinceSitEnd >= 0.5f)
			{
				float num;
				Player closestPlayer = Player.GetClosestPlayer(base.transform.position, out num, null);
				if (closestPlayer == null)
				{
					return;
				}
				Vector3 vector = closestPlayer.transform.position - base.Npc.transform.position;
				vector.y = 0f;
				if (Vector3.Angle(base.Npc.transform.forward, vector) > 10f)
				{
					base.Npc.Movement.FaceDirection(vector, 0.5f);
				}
			}
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00085390 File Offset: 0x00083590
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SendTargetPlayer_3323014238));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetTargetPlayer_3323014238));
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000853E2 File Offset: 0x000835E2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.GenericDialogueBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000853FB File Offset: 0x000835FB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0008540C File Offset: 0x0008360C
		private void RpcWriter___Server_SendTargetPlayer_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(15U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000854B3 File Offset: 0x000836B3
		public void RpcLogic___SendTargetPlayer_3323014238(NetworkObject player)
		{
			this.SetTargetPlayer(player);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000854BC File Offset: 0x000836BC
		private void RpcReader___Server_SendTargetPlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendTargetPlayer_3323014238(player);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000854FC File Offset: 0x000836FC
		private void RpcWriter___Observers_SetTargetPlayer_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000855B4 File Offset: 0x000837B4
		private void RpcLogic___SetTargetPlayer_3323014238(NetworkObject player)
		{
			if (Singleton<DialogueCanvas>.Instance.isActive && this.targetPlayer != null && this.targetPlayer.Owner.IsLocalClient && player != null && !player.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			if (player != null)
			{
				this.targetPlayer = player.GetComponent<Player>();
				return;
			}
			this.targetPlayer = null;
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0008562C File Offset: 0x0008382C
		private void RpcReader___Observers_SetTargetPlayer_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetTargetPlayer_3323014238(player);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00085667 File Offset: 0x00083867
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001908 RID: 6408
		private Player targetPlayer;

		// Token: 0x04001909 RID: 6409
		private bool dll_Excuted;

		// Token: 0x0400190A RID: 6410
		private bool dll_Excuted;
	}
}
