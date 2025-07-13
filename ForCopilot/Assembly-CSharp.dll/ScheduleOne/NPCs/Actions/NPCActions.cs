using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Actions
{
	// Token: 0x020004D1 RID: 1233
	public class NPCActions : NetworkBehaviour
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x000750CD File Offset: 0x000732CD
		protected NPCBehaviour behaviour
		{
			get
			{
				return this.npc.behaviour;
			}
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000750DA File Offset: 0x000732DA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Actions.NPCActions_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000750EE File Offset: 0x000732EE
		public void Cower()
		{
			this.behaviour.GetBehaviour("Cowering").Enable_Networked(null);
			base.StartCoroutine(this.<Cower>g__Wait|4_0());
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x00075114 File Offset: 0x00073314
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CallPolice_Networked(Player player)
		{
			this.RpcWriter___Server_CallPolice_Networked_1385486242(player);
			this.RpcLogic___CallPolice_Networked_1385486242(player);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00075135 File Offset: 0x00073335
		public void SetCallPoliceBehaviourCrime(Crime crime)
		{
			this.npc.behaviour.CallPoliceBehaviour.ReportedCrime = crime;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000045B1 File Offset: 0x000027B1
		public void FacePlayer(Player player)
		{
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x0007514D File Offset: 0x0007334D
		[CompilerGenerated]
		private IEnumerator <Cower>g__Wait|4_0()
		{
			yield return new WaitForSeconds(10f);
			this.behaviour.GetBehaviour("Cowering").Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0007515C File Offset: 0x0007335C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CallPolice_Networked_1385486242));
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x00075186 File Offset: 0x00073386
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Actions.NPCActionsAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00075199 File Offset: 0x00073399
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000751A8 File Offset: 0x000733A8
		private void RpcWriter___Server_CallPolice_Networked_1385486242(Player player)
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
			writer.Write___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generated(player);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x00075250 File Offset: 0x00073450
		public void RpcLogic___CallPolice_Networked_1385486242(Player player)
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			if (!this.npc.IsConscious)
			{
				return;
			}
			Console.Log(this.npc.fullName + " is calling the police on " + player.PlayerName, null);
			if (player.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				Console.LogWarning("Player is already being pursued, ignoring call police request.", null);
				return;
			}
			this.npc.behaviour.CallPoliceBehaviour.Target = player;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.npc.behaviour.CallPoliceBehaviour.Enable_Networked(null);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000752E8 File Offset: 0x000734E8
		private void RpcReader___Server_CallPolice_Networked_1385486242(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Player player = GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CallPolice_Networked_1385486242(player);
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00075326 File Offset: 0x00073526
		protected virtual void dll()
		{
			this.npc = base.GetComponentInParent<NPC>();
		}

		// Token: 0x040016E8 RID: 5864
		private NPC npc;

		// Token: 0x040016E9 RID: 5865
		private bool dll_Excuted;

		// Token: 0x040016EA RID: 5866
		private bool dll_Excuted;
	}
}
