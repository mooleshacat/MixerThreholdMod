using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI;
using ScheduleOne.UI.Handover;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000567 RID: 1383
	public class RequestProductBehaviour : Behaviour
	{
		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x00088678 File Offset: 0x00086878
		// (set) Token: 0x0600210D RID: 8461 RVA: 0x00088680 File Offset: 0x00086880
		public Player TargetPlayer { get; private set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x00088689 File Offset: 0x00086889
		// (set) Token: 0x0600210F RID: 8463 RVA: 0x00088691 File Offset: 0x00086891
		public RequestProductBehaviour.EState State { get; private set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x0008869A File Offset: 0x0008689A
		private Customer customer
		{
			get
			{
				return base.Npc.GetComponent<Customer>();
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000886A7 File Offset: 0x000868A7
		[ObserversRpc(RunLocally = true)]
		public void AssignTarget(NetworkObject plr)
		{
			this.RpcWriter___Observers_AssignTarget_3323014238(plr);
			this.RpcLogic___AssignTarget_3323014238(plr);
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000886BD File Offset: 0x000868BD
		protected virtual void Start()
		{
			this.SetUpDialogue();
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000886C8 File Offset: 0x000868C8
		protected override void Begin()
		{
			base.Begin();
			this.State = RequestProductBehaviour.EState.InitialApproach;
			this.requestGreeting.Greeting = base.Npc.dialogueHandler.Database.GetLine(EDialogueModule.Customer, "request_product_initial");
			if (InstanceFinder.IsServer)
			{
				Transform target = NetworkSingleton<NPCManager>.Instance.GetOrderedDistanceWarpPoints(this.TargetPlayer.transform.position)[1];
				base.Npc.Movement.Warp(target);
				if (base.Npc.isInBuilding)
				{
					base.Npc.ExitBuilding("");
				}
				base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
				base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("requestproduct", 5, 0.4f));
			}
			this.requestGreeting.ShouldShow = (this.TargetPlayer != null && this.TargetPlayer.Owner.IsLocalClient);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000887C4 File Offset: 0x000869C4
		protected override void End()
		{
			base.End();
			if (this.requestGreeting != null)
			{
				this.requestGreeting.ShouldShow = false;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
				base.Npc.Movement.SpeedController.RemoveSpeedControl("requestproduct");
			}
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0007BF58 File Offset: 0x0007A158
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00088820 File Offset: 0x00086A20
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (base.Npc.dialogueHandler.IsPlaying)
			{
				this.minsSinceLastDialogue = 0;
			}
			this.minsSinceLastDialogue++;
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.TargetPlayer.Owner.IsLocalClient)
			{
				if (this.State == RequestProductBehaviour.EState.InitialApproach && this.CanStartDialogue())
				{
					this.SendStartInitialDialogue();
				}
				if (this.State == RequestProductBehaviour.EState.FollowPlayer && this.minsSinceLastDialogue >= 90 && this.CanStartDialogue())
				{
					this.minsSinceLastDialogue = 0;
					this.SendStartFollowUpDialogue();
				}
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<HandoverScreen>.Instance.CurrentCustomer == this.customer)
			{
				return;
			}
			if (!RequestProductBehaviour.IsTargetValid(this.TargetPlayer))
			{
				base.SendDisable();
				return;
			}
			if (this.State == RequestProductBehaviour.EState.InitialApproach)
			{
				if (!this.IsTargetDestinationValid())
				{
					Vector3 destination;
					if (this.GetNewDestination(out destination))
					{
						base.Npc.Movement.SetDestination(destination);
						return;
					}
					base.SendDisable();
					return;
				}
			}
			else if (this.State == RequestProductBehaviour.EState.FollowPlayer && !this.IsTargetDestinationValid())
			{
				Vector3 destination2;
				if (this.GetNewDestination(out destination2))
				{
					base.Npc.Movement.SetDestination(destination2);
					return;
				}
				base.SendDisable();
				return;
			}
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00088954 File Offset: 0x00086B54
		private bool IsTargetDestinationValid()
		{
			return base.Npc.Movement.IsMoving && Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.transform.position) <= ((this.State == RequestProductBehaviour.EState.InitialApproach) ? 2.5f : 5f) && base.Npc.Movement.Agent.path != null;
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000889CC File Offset: 0x00086BCC
		private bool GetNewDestination(out Vector3 dest)
		{
			dest = this.TargetPlayer.transform.position;
			if (this.State == RequestProductBehaviour.EState.InitialApproach)
			{
				dest += this.TargetPlayer.transform.forward * 1.5f;
			}
			else if (this.State == RequestProductBehaviour.EState.InitialApproach)
			{
				dest += (base.Npc.transform.position - this.TargetPlayer.transform.position).normalized * 2.5f;
			}
			NavMeshHit navMeshHit;
			if (NavMeshUtility.SamplePosition(dest, out navMeshHit, 15f, -1, true))
			{
				dest = navMeshHit.position;
				return true;
			}
			Console.LogError("Failed to find valid destination for RequestProductBehaviour: stopping", null);
			return false;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00088AA8 File Offset: 0x00086CA8
		public static bool IsTargetValid(Player player)
		{
			return !(player == null) && !player.IsArrested && player.Health.IsAlive && player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && !player.CrimeData.BodySearchPending && !player.IsSleeping;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00088B04 File Offset: 0x00086D04
		public bool CanStartDialogue()
		{
			return RequestProductBehaviour.IsTargetValid(this.TargetPlayer) && this.TargetPlayer.Owner.IsLocalClient && !Singleton<DialogueCanvas>.Instance.isActive && Vector3.Distance(base.Npc.transform.position, this.TargetPlayer.transform.position) <= 2.5f && !Singleton<HandoverScreen>.Instance.IsOpen && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount <= 0;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x00088B90 File Offset: 0x00086D90
		private void SetUpDialogue()
		{
			if (this.requestGreeting != null)
			{
				return;
			}
			this.acceptRequestChoice = new DialogueController.DialogueChoice();
			this.acceptRequestChoice.ChoiceText = "[Make an offer]";
			this.acceptRequestChoice.Enabled = true;
			this.acceptRequestChoice.Conversation = null;
			this.acceptRequestChoice.onChoosen = new UnityEvent();
			this.acceptRequestChoice.onChoosen.AddListener(new UnityAction(this.RequestAccepted));
			this.acceptRequestChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.DialogueActive);
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.acceptRequestChoice, 0);
			this.followChoice = new DialogueController.DialogueChoice();
			this.followChoice.ChoiceText = "Follow me, I need to grab it first";
			this.followChoice.Enabled = true;
			this.followChoice.Conversation = null;
			this.followChoice.onChoosen = new UnityEvent();
			this.followChoice.onChoosen.AddListener(new UnityAction(this.Follow));
			this.followChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.DialogueActive);
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.followChoice, 0);
			this.rejectChoice = new DialogueController.DialogueChoice();
			this.rejectChoice.ChoiceText = "Get out of here";
			this.rejectChoice.Enabled = true;
			this.rejectChoice.Conversation = null;
			this.rejectChoice.onChoosen = new UnityEvent();
			this.rejectChoice.onChoosen.AddListener(new UnityAction(this.RequestRejected));
			this.rejectChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.DialogueActive);
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.rejectChoice, 0);
			this.requestGreeting = new DialogueController.GreetingOverride();
			this.requestGreeting.Greeting = base.Npc.dialogueHandler.Database.GetLine(EDialogueModule.Customer, "request_product_initial");
			this.requestGreeting.ShouldShow = false;
			this.requestGreeting.PlayVO = true;
			this.requestGreeting.VOType = EVOLineType.Question;
			base.Npc.dialogueHandler.GetComponent<DialogueController>().AddGreetingOverride(this.requestGreeting);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x00088DD0 File Offset: 0x00086FD0
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendStartInitialDialogue()
		{
			this.RpcWriter___Server_SendStartInitialDialogue_2166136261();
			this.RpcLogic___SendStartInitialDialogue_2166136261();
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00088DE0 File Offset: 0x00086FE0
		[ObserversRpc(RunLocally = true)]
		private void StartInitialDialogue()
		{
			this.RpcWriter___Observers_StartInitialDialogue_2166136261();
			this.RpcLogic___StartInitialDialogue_2166136261();
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x00088DF9 File Offset: 0x00086FF9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendStartFollowUpDialogue()
		{
			this.RpcWriter___Server_SendStartFollowUpDialogue_2166136261();
			this.RpcLogic___SendStartFollowUpDialogue_2166136261();
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00088E08 File Offset: 0x00087008
		[ObserversRpc(RunLocally = true)]
		private void StartFollowUpDialogue()
		{
			this.RpcWriter___Observers_StartFollowUpDialogue_2166136261();
			this.RpcLogic___StartFollowUpDialogue_2166136261();
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00088E21 File Offset: 0x00087021
		private bool DialogueActive(bool enabled)
		{
			return base.Active && this.TargetPlayer.Owner.IsLocalClient;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00088E3D File Offset: 0x0008703D
		private void RequestAccepted()
		{
			this.minsSinceLastDialogue = 0;
			Singleton<HandoverScreen>.Instance.Open(null, this.customer, HandoverScreen.EMode.Offer, new Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float>(this.HandoverClosed), new Func<List<ItemInstance>, float, float>(this.customer.GetOfferSuccessChance));
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00088E78 File Offset: 0x00087078
		private void HandoverClosed(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float askingPrice)
		{
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				Singleton<DialogueCanvas>.Instance.SkipNextRollout = true;
				Singleton<CoroutineService>.Instance.StartCoroutine(this.<HandoverClosed>g__Wait|36_0());
				return;
			}
			float offerSuccessChance = this.customer.GetOfferSuccessChance(items, askingPrice);
			if (UnityEngine.Random.value < offerSuccessChance)
			{
				Contract contract = new Contract();
				ProductList productList = new ProductList();
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i] is ProductItemInstance)
					{
						productList.entries.Add(new ProductList.Entry
						{
							ProductID = items[i].ID,
							Quantity = items[i].Quantity,
							Quality = this.customer.CustomerData.Standards.GetCorrespondingQuality()
						});
					}
				}
				contract.SilentlyInitializeContract("Offer", string.Empty, null, string.Empty, base.Npc.NetworkObject, askingPrice, productList, string.Empty, new QuestWindowConfig(), 0, NetworkSingleton<TimeManager>.Instance.GetDateTime());
				this.customer.ProcessHandover(HandoverScreen.EHandoverOutcome.Finalize, contract, items, true, false);
			}
			else
			{
				Singleton<HandoverScreen>.Instance.ClearCustomerSlots(true);
				this.customer.RejectProductRequestOffer();
			}
			base.SendDisable();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00088FA0 File Offset: 0x000871A0
		private void Follow()
		{
			this.minsSinceLastDialogue = 0;
			this.State = RequestProductBehaviour.EState.FollowPlayer;
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("requestproduct", 5, 0.6f));
			this.requestGreeting.Greeting = base.Npc.dialogueHandler.Database.GetLine(EDialogueModule.Customer, "request_product_after_follow");
			base.Npc.dialogueHandler.ShowWorldspaceDialogue("Ok...", 3f);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00089020 File Offset: 0x00087220
		private void RequestRejected()
		{
			this.minsSinceLastDialogue = 0;
			this.customer.PlayerRejectedProductRequest();
			base.SendDisable();
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0008903A File Offset: 0x0008723A
		[CompilerGenerated]
		private IEnumerator <HandoverClosed>g__Wait|36_0()
		{
			yield return new WaitForEndOfFrame();
			this.StartInitialDialogue();
			yield break;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0008904C File Offset: 0x0008724C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_AssignTarget_3323014238));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SendStartInitialDialogue_2166136261));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_StartInitialDialogue_2166136261));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SendStartFollowUpDialogue_2166136261));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_StartFollowUpDialogue_2166136261));
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000890E3 File Offset: 0x000872E3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RequestProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000890FC File Offset: 0x000872FC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0008910C File Offset: 0x0008730C
		private void RpcWriter___Observers_AssignTarget_3323014238(NetworkObject plr)
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
			writer.WriteNetworkObject(plr);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000891C2 File Offset: 0x000873C2
		public void RpcLogic___AssignTarget_3323014238(NetworkObject plr)
		{
			this.TargetPlayer = ((plr != null) ? plr.GetComponent<Player>() : null);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000891DC File Offset: 0x000873DC
		private void RpcReader___Observers_AssignTarget_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject plr = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AssignTarget_3323014238(plr);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00089218 File Offset: 0x00087418
		private void RpcWriter___Server_SendStartInitialDialogue_2166136261()
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
			base.SendServerRpc(16U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000892B2 File Offset: 0x000874B2
		private void RpcLogic___SendStartInitialDialogue_2166136261()
		{
			this.StartInitialDialogue();
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000892BC File Offset: 0x000874BC
		private void RpcReader___Server_SendStartInitialDialogue_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendStartInitialDialogue_2166136261();
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000892EC File Offset: 0x000874EC
		private void RpcWriter___Observers_StartInitialDialogue_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00089398 File Offset: 0x00087598
		private void RpcLogic___StartInitialDialogue_2166136261()
		{
			if (this.TargetPlayer != null && this.TargetPlayer.IsOwner && !base.Npc.dialogueHandler.IsPlaying)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
				{
					Singleton<GameInput>.Instance.ExitAll();
				}
				base.Npc.dialogueHandler.GetComponent<DialogueController>().StartGenericDialogue(false);
			}
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00089400 File Offset: 0x00087600
		private void RpcReader___Observers_StartInitialDialogue_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartInitialDialogue_2166136261();
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0008942C File Offset: 0x0008762C
		private void RpcWriter___Server_SendStartFollowUpDialogue_2166136261()
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

		// Token: 0x06002134 RID: 8500 RVA: 0x000894C6 File Offset: 0x000876C6
		private void RpcLogic___SendStartFollowUpDialogue_2166136261()
		{
			this.StartFollowUpDialogue();
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000894D0 File Offset: 0x000876D0
		private void RpcReader___Server_SendStartFollowUpDialogue_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendStartFollowUpDialogue_2166136261();
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00089500 File Offset: 0x00087700
		private void RpcWriter___Observers_StartFollowUpDialogue_2166136261()
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
			base.SendObserversRpc(19U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000895AC File Offset: 0x000877AC
		private void RpcLogic___StartFollowUpDialogue_2166136261()
		{
			if (this.TargetPlayer != null && this.TargetPlayer.IsOwner && !base.Npc.dialogueHandler.IsPlaying)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
				{
					Singleton<GameInput>.Instance.ExitAll();
				}
				base.Npc.dialogueHandler.GetComponent<DialogueController>().StartGenericDialogue(false);
			}
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00089614 File Offset: 0x00087814
		private void RpcReader___Observers_StartFollowUpDialogue_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartFollowUpDialogue_2166136261();
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x0008963E File Offset: 0x0008783E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400196F RID: 6511
		public const float CONVERSATION_RANGE = 2.5f;

		// Token: 0x04001970 RID: 6512
		public const float FOLLOW_MAX_RANGE = 5f;

		// Token: 0x04001971 RID: 6513
		public const int MINS_TO_ASK_AGAIN = 90;

		// Token: 0x04001974 RID: 6516
		private int minsSinceLastDialogue;

		// Token: 0x04001975 RID: 6517
		private DialogueController.GreetingOverride requestGreeting;

		// Token: 0x04001976 RID: 6518
		private DialogueController.DialogueChoice acceptRequestChoice;

		// Token: 0x04001977 RID: 6519
		private DialogueController.DialogueChoice followChoice;

		// Token: 0x04001978 RID: 6520
		private DialogueController.DialogueChoice rejectChoice;

		// Token: 0x04001979 RID: 6521
		private bool dll_Excuted;

		// Token: 0x0400197A RID: 6522
		private bool dll_Excuted;

		// Token: 0x02000568 RID: 1384
		public enum EState
		{
			// Token: 0x0400197C RID: 6524
			InitialApproach,
			// Token: 0x0400197D RID: 6525
			FollowPlayer
		}
	}
}
