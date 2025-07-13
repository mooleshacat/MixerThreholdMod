using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000512 RID: 1298
	public class SchizoGoblin : NPC
	{
		// Token: 0x06001C89 RID: 7305 RVA: 0x000772AC File Offset: 0x000754AC
		[ObserversRpc]
		public void SetTargetPlayer(NetworkObject player)
		{
			this.RpcWriter___Observers_SetTargetPlayer_3323014238(player);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000045B1 File Offset: 0x000027B1
		public void Activate()
		{
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000772C3 File Offset: 0x000754C3
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SchizoGoblinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SchizoGoblinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(35U, new ClientRpcDelegate(this.RpcReader___Observers_SetTargetPlayer_3323014238));
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000772F3 File Offset: 0x000754F3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SchizoGoblinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SchizoGoblinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0007730C File Offset: 0x0007550C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0007731C File Offset: 0x0007551C
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
			base.SendObserversRpc(35U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000773D4 File Offset: 0x000755D4
		public void RpcLogic___SetTargetPlayer_3323014238(NetworkObject player)
		{
			this.targetPlayer = player.GetComponent<Player>();
			if (this.targetPlayer.IsLocalPlayer)
			{
				LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("NPC"));
				return;
			}
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00077428 File Offset: 0x00075628
		private void RpcReader___Observers_SetTargetPlayer_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetTargetPlayer_3323014238(player);
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00077459 File Offset: 0x00075659
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001791 RID: 6033
		private Player targetPlayer;

		// Token: 0x04001792 RID: 6034
		private bool dll_Excuted;

		// Token: 0x04001793 RID: 6035
		private bool dll_Excuted;
	}
}
