using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000522 RID: 1314
	public class CheckpointBehaviour : Behaviour
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001D68 RID: 7528 RVA: 0x0007A396 File Offset: 0x00078596
		// (set) Token: 0x06001D69 RID: 7529 RVA: 0x0007A39E File Offset: 0x0007859E
		public CheckpointManager.ECheckpointLocation AssignedCheckpoint { get; protected set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x0007A3A7 File Offset: 0x000785A7
		// (set) Token: 0x06001D6B RID: 7531 RVA: 0x0007A3AF File Offset: 0x000785AF
		public RoadCheckpoint Checkpoint { get; protected set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x0007A3B8 File Offset: 0x000785B8
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x0007A3C0 File Offset: 0x000785C0
		public bool IsSearching { get; protected set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x0007A3C9 File Offset: 0x000785C9
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x0007A3D1 File Offset: 0x000785D1
		public LandVehicle CurrentSearchedVehicle { get; protected set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x0007A3DA File Offset: 0x000785DA
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x0007A3E2 File Offset: 0x000785E2
		public Player Initiator { get; protected set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x0007A3EB File Offset: 0x000785EB
		private Transform standPoint
		{
			get
			{
				return this.Checkpoint.StandPoints[Mathf.Clamp(this.Checkpoint.AssignedNPCs.IndexOf(base.Npc), 0, this.Checkpoint.StandPoints.Length - 1)];
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x0007A424 File Offset: 0x00078624
		private DialogueDatabase dialogueDatabase
		{
			get
			{
				return base.Npc.dialogueHandler.Database;
			}
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x0007A438 File Offset: 0x00078638
		protected override void Begin()
		{
			base.Begin();
			this.Checkpoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.AssignedCheckpoint);
			if (!this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Add(base.Npc);
			}
			this.Checkpoint.onPlayerWalkThrough.AddListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x0007A4AC File Offset: 0x000786AC
		protected override void Resume()
		{
			base.Resume();
			this.Checkpoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.AssignedCheckpoint);
			if (!this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Add(base.Npc);
			}
			this.Checkpoint.onPlayerWalkThrough.AddListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x0007A520 File Offset: 0x00078720
		protected override void End()
		{
			base.End();
			this.IsSearching = false;
			if (this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Remove(base.Npc);
			}
			if (this.CurrentSearchedVehicle != null && this.trunkOpened)
			{
				this.CurrentSearchedVehicle.Trunk.SetIsOpen(false);
			}
			this.Checkpoint.onPlayerWalkThrough.RemoveListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x0007A5AC File Offset: 0x000787AC
		protected override void Pause()
		{
			base.Pause();
			this.IsSearching = false;
			if (this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Remove(base.Npc);
			}
			if (this.CurrentSearchedVehicle != null && this.trunkOpened)
			{
				this.CurrentSearchedVehicle.Trunk.SetIsOpen(false);
			}
			this.Checkpoint.onPlayerWalkThrough.RemoveListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0007A638 File Offset: 0x00078838
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.IsSearching && !base.Npc.Movement.IsMoving && base.Npc.Movement.IsAsCloseAsPossible(this.GetSearchPoint(), 0.5f))
			{
				if (!this.CurrentSearchedVehicle.Trunk.IsOpen)
				{
					StorageDoorAnimation trunk = this.CurrentSearchedVehicle.Trunk;
					if (trunk != null)
					{
						trunk.SetIsOpen(true);
					}
					this.trunkOpened = true;
				}
			}
			else if (this.trunkOpened && this.CurrentSearchedVehicle != null)
			{
				StorageDoorAnimation trunk2 = this.CurrentSearchedVehicle.Trunk;
				if (trunk2 != null)
				{
					trunk2.SetIsOpen(false);
				}
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.Checkpoint == null || this.Checkpoint.ActivationState == RoadCheckpoint.ECheckpointState.Disabled)
			{
				base.Disable_Networked(null);
				return;
			}
			if (!this.IsSearching)
			{
				if (!base.Npc.Movement.IsMoving)
				{
					if (base.Npc.Movement.IsAsCloseAsPossible(this.standPoint.position, 0.5f))
					{
						if (!base.Npc.Movement.FaceDirectionInProgress)
						{
							base.Npc.Movement.FaceDirection(this.standPoint.forward, 0.5f);
							return;
						}
					}
					else if (base.Npc.Movement.CanMove())
					{
						base.Npc.Movement.SetDestination(this.standPoint.position);
						return;
					}
				}
			}
			else
			{
				if (!this.Checkpoint.SearchArea1.vehicles.Contains(this.CurrentSearchedVehicle) && !this.Checkpoint.SearchArea2.vehicles.Contains(this.CurrentSearchedVehicle))
				{
					this.StopSearch();
					return;
				}
				if (!base.Npc.Movement.IsMoving)
				{
					if (base.Npc.Movement.IsAsCloseAsPossible(this.GetSearchPoint(), 1f))
					{
						if (!base.Npc.Movement.FaceDirectionInProgress)
						{
							base.Npc.Movement.FacePoint(this.CurrentSearchedVehicle.transform.position, 0.5f);
						}
						this.currentLookTime += 1f;
						if (this.currentLookTime >= 1.5f)
						{
							this.ConcludeSearch();
							return;
						}
					}
					else
					{
						this.currentLookTime = 0f;
						if (base.Npc.Movement.CanMove())
						{
							base.Npc.Movement.SetDestination(this.GetSearchPoint());
						}
					}
				}
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0007A8B8 File Offset: 0x00078AB8
		[ObserversRpc(RunLocally = true)]
		public void SetCheckpoint(CheckpointManager.ECheckpointLocation loc)
		{
			this.RpcWriter___Observers_SetCheckpoint_4087078542(loc);
			this.RpcLogic___SetCheckpoint_4087078542(loc);
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0007A8CE File Offset: 0x00078ACE
		[ObserversRpc(RunLocally = true)]
		public void SetInitiator(NetworkObject init)
		{
			this.RpcWriter___Observers_SetInitiator_3323014238(init);
			this.RpcLogic___SetInitiator_3323014238(init);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0007A8E4 File Offset: 0x00078AE4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void StartSearch(NetworkObject targetVehicle, NetworkObject initiator)
		{
			this.RpcWriter___Server_StartSearch_3694055493(targetVehicle, initiator);
			this.RpcLogic___StartSearch_3694055493(targetVehicle, initiator);
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0007A910 File Offset: 0x00078B10
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void StopSearch()
		{
			this.RpcWriter___Server_StopSearch_2166136261();
			this.RpcLogic___StopSearch_2166136261();
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0007A929 File Offset: 0x00078B29
		[ObserversRpc(RunLocally = true)]
		public void SetIsSearching(bool s)
		{
			this.RpcWriter___Observers_SetIsSearching_1140765316(s);
			this.RpcLogic___SetIsSearching_1140765316(s);
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0007A940 File Offset: 0x00078B40
		private Vector3 GetSearchPoint()
		{
			return this.CurrentSearchedVehicle.transform.position - this.CurrentSearchedVehicle.transform.forward * (this.CurrentSearchedVehicle.boundingBoxDimensions.z / 2f + 0.75f);
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0007A994 File Offset: 0x00078B94
		[ObserversRpc(RunLocally = true)]
		private void ConcludeSearch()
		{
			this.RpcWriter___Observers_ConcludeSearch_2166136261();
			this.RpcLogic___ConcludeSearch_2166136261();
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0007A9B0 File Offset: 0x00078BB0
		private bool DoesVehicleContainIllicitItems()
		{
			if (this.CurrentSearchedVehicle == null)
			{
				return false;
			}
			(from x in this.CurrentSearchedVehicle.Storage.ItemSlots
			select x.ItemInstance).ToList<ItemInstance>();
			foreach (ItemSlot itemSlot in this.CurrentSearchedVehicle.Storage.ItemSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= this.Checkpoint.MaxStealthLevel)
						{
							return true;
						}
					}
					else if (itemSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0007AAB4 File Offset: 0x00078CB4
		private void PlayerWalkedThroughCheckPoint(Player player)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (player.CrimeData.TimeSinceLastBodySearch < 60f)
			{
				return;
			}
			if (player.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				return;
			}
			if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
			{
				return;
			}
			if (this.Checkpoint.AssignedNPCs.Count == 0)
			{
				return;
			}
			List<NPC> list = new List<NPC>();
			for (int i = 0; i < this.Checkpoint.AssignedNPCs.Count; i++)
			{
				Transform transform = this.Checkpoint.StandPoints[Mathf.Clamp(i, 0, this.Checkpoint.StandPoints.Length - 1)];
				if (Vector3.Distance(this.Checkpoint.AssignedNPCs[i].transform.position, transform.position) < 6f)
				{
					list.Add(this.Checkpoint.AssignedNPCs[i]);
				}
			}
			NPC x = null;
			float num = float.MaxValue;
			for (int j = 0; j < list.Count; j++)
			{
				float num2 = Vector3.Distance(player.transform.position, list[j].transform.position);
				if (num2 < num)
				{
					num = num2;
					x = list[j];
				}
			}
			if (num > 6f)
			{
				return;
			}
			if (x != base.Npc)
			{
				return;
			}
			player.CrimeData.ResetBodysearchCooldown();
			(base.Npc as PoliceOfficer).ConductBodySearch(player);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x0007AC1C File Offset: 0x00078E1C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetCheckpoint_4087078542));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetInitiator_3323014238));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_StartSearch_3694055493));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_StopSearch_2166136261));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsSearching_1140765316));
			base.RegisterObserversRpc(20U, new ClientRpcDelegate(this.RpcReader___Observers_ConcludeSearch_2166136261));
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0007ACCA File Offset: 0x00078ECA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0007ACE3 File Offset: 0x00078EE3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x0007ACF4 File Offset: 0x00078EF4
		private void RpcWriter___Observers_SetCheckpoint_4087078542(CheckpointManager.ECheckpointLocation loc)
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
			writer.Write___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generated(loc);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0007ADAA File Offset: 0x00078FAA
		public void RpcLogic___SetCheckpoint_4087078542(CheckpointManager.ECheckpointLocation loc)
		{
			this.AssignedCheckpoint = loc;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x0007ADB4 File Offset: 0x00078FB4
		private void RpcReader___Observers_SetCheckpoint_4087078542(PooledReader PooledReader0, Channel channel)
		{
			CheckpointManager.ECheckpointLocation loc = GeneratedReaders___Internal.Read___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCheckpoint_4087078542(loc);
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x0007ADF0 File Offset: 0x00078FF0
		private void RpcWriter___Observers_SetInitiator_3323014238(NetworkObject init)
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
			writer.WriteNetworkObject(init);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x0007AEA6 File Offset: 0x000790A6
		public void RpcLogic___SetInitiator_3323014238(NetworkObject init)
		{
			this.Initiator = init.GetComponent<Player>();
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x0007AEB4 File Offset: 0x000790B4
		private void RpcReader___Observers_SetInitiator_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject init = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetInitiator_3323014238(init);
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x0007AEF0 File Offset: 0x000790F0
		private void RpcWriter___Server_StartSearch_3694055493(NetworkObject targetVehicle, NetworkObject initiator)
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
			writer.WriteNetworkObject(targetVehicle);
			writer.WriteNetworkObject(initiator);
			base.SendServerRpc(17U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x0007AFA4 File Offset: 0x000791A4
		public void RpcLogic___StartSearch_3694055493(NetworkObject targetVehicle, NetworkObject initiator)
		{
			this.currentLookTime = 0f;
			this.SetIsSearching(true);
			this.SetInitiator(initiator);
			this.CurrentSearchedVehicle = targetVehicle.GetComponent<LandVehicle>();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("searchingvehicle", 20, 0.15f));
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x0007B004 File Offset: 0x00079204
		private void RpcReader___Server_StartSearch_3694055493(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject targetVehicle = PooledReader0.ReadNetworkObject();
			NetworkObject initiator = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___StartSearch_3694055493(targetVehicle, initiator);
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x0007B054 File Offset: 0x00079254
		private void RpcWriter___Server_StopSearch_2166136261()
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
			base.SendServerRpc(18U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x0007B0F0 File Offset: 0x000792F0
		public void RpcLogic___StopSearch_2166136261()
		{
			this.SetIsSearching(false);
			if (this.CurrentSearchedVehicle != null && this.trunkOpened)
			{
				StorageDoorAnimation trunk = this.CurrentSearchedVehicle.Trunk;
				if (trunk != null)
				{
					trunk.SetIsOpen(false);
				}
			}
			this.CurrentSearchedVehicle = null;
			this.Initiator = null;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("searchingvehicle");
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x0007B164 File Offset: 0x00079364
		private void RpcReader___Server_StopSearch_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___StopSearch_2166136261();
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0007B194 File Offset: 0x00079394
		private void RpcWriter___Observers_SetIsSearching_1140765316(bool s)
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
			writer.WriteBoolean(s);
			base.SendObserversRpc(19U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x0007B24A File Offset: 0x0007944A
		public void RpcLogic___SetIsSearching_1140765316(bool s)
		{
			this.IsSearching = s;
			if (this.IsSearching)
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "checkpoint_search_start"), 3f);
			}
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x0007B284 File Offset: 0x00079484
		private void RpcReader___Observers_SetIsSearching_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool s = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsSearching_1140765316(s);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x0007B2C0 File Offset: 0x000794C0
		private void RpcWriter___Observers_ConcludeSearch_2166136261()
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
			base.SendObserversRpc(20U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x0007B36C File Offset: 0x0007956C
		private void RpcLogic___ConcludeSearch_2166136261()
		{
			if (this.CurrentSearchedVehicle == null)
			{
				Console.LogWarning("ConcludeSearch called with null vehicle", null);
			}
			if (this.CurrentSearchedVehicle != null && this.DoesVehicleContainIllicitItems() && this.Initiator != null)
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "checkpoint_items_found"), 3f);
				if (this.Initiator == Player.Local)
				{
					Player.Local.CrimeData.AddCrime(new TransportingIllicitItems(), 1);
					Player.Local.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
					(base.Npc as PoliceOfficer).BeginFootPursuit_Networked(Player.Local.NetworkObject, true);
				}
			}
			else
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "checkpoint_all_clear"), 3f);
				if (this.Checkpoint.SearchArea1.vehicles.Contains(this.CurrentSearchedVehicle))
				{
					this.Checkpoint.SetGate1Open(true);
				}
				else if (this.Checkpoint.SearchArea1.vehicles.Contains(this.CurrentSearchedVehicle))
				{
					this.Checkpoint.SetGate2Open(true);
				}
				else
				{
					this.Checkpoint.SetGate1Open(true);
					this.Checkpoint.SetGate2Open(true);
				}
			}
			this.StopSearch();
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0007B4DC File Offset: 0x000796DC
		private void RpcReader___Observers_ConcludeSearch_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ConcludeSearch_2166136261();
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0007B506 File Offset: 0x00079706
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017E7 RID: 6119
		public const float LOOK_TIME = 1.5f;

		// Token: 0x040017ED RID: 6125
		private float currentLookTime;

		// Token: 0x040017EE RID: 6126
		private bool trunkOpened;

		// Token: 0x040017EF RID: 6127
		private bool dll_Excuted;

		// Token: 0x040017F0 RID: 6128
		private bool dll_Excuted;
	}
}
