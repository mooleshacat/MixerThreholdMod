using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000553 RID: 1363
	public class FacePlayerBehaviour : Behaviour
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x00083EF7 File Offset: 0x000820F7
		// (set) Token: 0x0600201B RID: 8219 RVA: 0x00083EFF File Offset: 0x000820FF
		public Player Player { get; private set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x00083F08 File Offset: 0x00082108
		// (set) Token: 0x0600201D RID: 8221 RVA: 0x00083F10 File Offset: 0x00082110
		public float Countdown { get; private set; }

		// Token: 0x0600201E RID: 8222 RVA: 0x00083F1C File Offset: 0x0008211C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetTarget(NetworkObject player, float countDown = 5f)
		{
			this.RpcWriter___Server_SetTarget_244313061(player, countDown);
			this.RpcLogic___SetTarget_244313061(player, countDown);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x00083F45 File Offset: 0x00082145
		[ObserversRpc(RunLocally = true)]
		private void SetTargetLocal(NetworkObject player)
		{
			this.RpcWriter___Observers_SetTargetLocal_3323014238(player);
			this.RpcLogic___SetTargetLocal_3323014238(player);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00083F5B File Offset: 0x0008215B
		protected override void Begin()
		{
			base.Begin();
			base.Npc.Movement.Stop();
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00083F74 File Offset: 0x00082174
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (base.Active)
			{
				if (this.Player != null)
				{
					base.Npc.Avatar.LookController.OverrideLookTarget(this.Player.EyePosition, 1, true);
				}
				if (InstanceFinder.IsServer)
				{
					this.Countdown -= Time.deltaTime;
					if (this.Countdown <= 0f)
					{
						base.Disable_Networked(null);
					}
				}
			}
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00083FEC File Offset: 0x000821EC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SetTarget_244313061));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetTargetLocal_3323014238));
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0008403E File Offset: 0x0008223E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FacePlayerBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00084057 File Offset: 0x00082257
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00084068 File Offset: 0x00082268
		private void RpcWriter___Server_SetTarget_244313061(NetworkObject player, float countDown = 5f)
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
			writer.WriteSingle(countDown, 0);
			base.SendServerRpc(15U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00084124 File Offset: 0x00082324
		public void RpcLogic___SetTarget_244313061(NetworkObject player, float countDown = 5f)
		{
			Console.Log("SetTarget: " + ((player != null) ? player.ToString() : null), null);
			this.Countdown = countDown;
			this.Player = ((player != null) ? player.GetComponent<Player>() : null);
			this.SetTargetLocal(player);
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00084174 File Offset: 0x00082374
		private void RpcReader___Server_SetTarget_244313061(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			float countDown = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetTarget_244313061(player, countDown);
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x000841C8 File Offset: 0x000823C8
		private void RpcWriter___Observers_SetTargetLocal_3323014238(NetworkObject player)
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

		// Token: 0x0600202B RID: 8235 RVA: 0x0008427E File Offset: 0x0008247E
		private void RpcLogic___SetTargetLocal_3323014238(NetworkObject player)
		{
			this.Player = ((player != null) ? player.GetComponent<Player>() : null);
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00084298 File Offset: 0x00082498
		private void RpcReader___Observers_SetTargetLocal_3323014238(PooledReader PooledReader0, Channel channel)
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
			this.RpcLogic___SetTargetLocal_3323014238(player);
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000842D3 File Offset: 0x000824D3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018E9 RID: 6377
		private bool dll_Excuted;

		// Token: 0x040018EA RID: 6378
		private bool dll_Excuted;
	}
}
