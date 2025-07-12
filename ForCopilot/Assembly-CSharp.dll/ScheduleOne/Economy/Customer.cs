using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Map;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using ScheduleOne.Quests;
using ScheduleOne.UI;
using ScheduleOne.UI.Handover;
using ScheduleOne.UI.Phone.Messages;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x0200068C RID: 1676
	[DisallowMultipleComponent]
	[RequireComponent(typeof(NPC))]
	public class Customer : NetworkBehaviour, ISaveable
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002D02 RID: 11522 RVA: 0x000B977E File Offset: 0x000B797E
		// (set) Token: 0x06002D03 RID: 11523 RVA: 0x000B9786 File Offset: 0x000B7986
		public float CurrentAddiction
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentAddiction>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<CurrentAddiction>k__BackingField(value, true);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x000B9790 File Offset: 0x000B7990
		// (set) Token: 0x06002D05 RID: 11525 RVA: 0x000B9798 File Offset: 0x000B7998
		public ContractInfo OfferedContractInfo
		{
			get
			{
				return this.offeredContractInfo;
			}
			protected set
			{
				this.offeredContractInfo = value;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002D06 RID: 11526 RVA: 0x000B97A1 File Offset: 0x000B79A1
		// (set) Token: 0x06002D07 RID: 11527 RVA: 0x000B97A9 File Offset: 0x000B79A9
		public GameDateTime OfferedContractTime { get; protected set; }

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002D08 RID: 11528 RVA: 0x000B97B2 File Offset: 0x000B79B2
		// (set) Token: 0x06002D09 RID: 11529 RVA: 0x000B97BA File Offset: 0x000B79BA
		public Contract CurrentContract { get; protected set; }

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002D0A RID: 11530 RVA: 0x000B97C3 File Offset: 0x000B79C3
		// (set) Token: 0x06002D0B RID: 11531 RVA: 0x000B97CB File Offset: 0x000B79CB
		public bool IsAwaitingDelivery { get; protected set; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002D0C RID: 11532 RVA: 0x000B97D4 File Offset: 0x000B79D4
		// (set) Token: 0x06002D0D RID: 11533 RVA: 0x000B97DC File Offset: 0x000B79DC
		public int TimeSinceLastDealCompleted { get; protected set; } = 1000000;

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002D0E RID: 11534 RVA: 0x000B97E5 File Offset: 0x000B79E5
		// (set) Token: 0x06002D0F RID: 11535 RVA: 0x000B97ED File Offset: 0x000B79ED
		public int TimeSinceLastDealOffered { get; protected set; } = 1000000;

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002D10 RID: 11536 RVA: 0x000B97F6 File Offset: 0x000B79F6
		// (set) Token: 0x06002D11 RID: 11537 RVA: 0x000B97FE File Offset: 0x000B79FE
		public int TimeSincePlayerApproached { get; protected set; } = 1000000;

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002D12 RID: 11538 RVA: 0x000B9807 File Offset: 0x000B7A07
		// (set) Token: 0x06002D13 RID: 11539 RVA: 0x000B980F File Offset: 0x000B7A0F
		public int TimeSinceInstantDealOffered { get; protected set; } = 1000000;

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002D14 RID: 11540 RVA: 0x000B9818 File Offset: 0x000B7A18
		// (set) Token: 0x06002D15 RID: 11541 RVA: 0x000B9820 File Offset: 0x000B7A20
		public int OfferedDeals { get; protected set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000B9829 File Offset: 0x000B7A29
		// (set) Token: 0x06002D17 RID: 11543 RVA: 0x000B9831 File Offset: 0x000B7A31
		public int CompletedDeliveries { get; protected set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x000B983A File Offset: 0x000B7A3A
		// (set) Token: 0x06002D19 RID: 11545 RVA: 0x000B9842 File Offset: 0x000B7A42
		public bool HasBeenRecommended
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<HasBeenRecommended>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<HasBeenRecommended>k__BackingField(value, true);
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x000B984C File Offset: 0x000B7A4C
		// (set) Token: 0x06002D1B RID: 11547 RVA: 0x000B9854 File Offset: 0x000B7A54
		public NPC NPC { get; protected set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000B985D File Offset: 0x000B7A5D
		// (set) Token: 0x06002D1D RID: 11549 RVA: 0x000B9865 File Offset: 0x000B7A65
		public Dealer AssignedDealer { get; protected set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x000B986E File Offset: 0x000B7A6E
		public CustomerData CustomerData
		{
			get
			{
				return this.customerData;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002D1F RID: 11551 RVA: 0x000B9876 File Offset: 0x000B7A76
		public List<ProductDefinition> OrderableProducts
		{
			get
			{
				if (!(this.AssignedDealer != null))
				{
					return ProductManager.ListedProducts;
				}
				return this.AssignedDealer.GetOrderableProducts();
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002D20 RID: 11552 RVA: 0x000B9897 File Offset: 0x000B7A97
		private DialogueDatabase dialogueDatabase
		{
			get
			{
				return this.NPC.dialogueHandler.Database;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x000B98A9 File Offset: 0x000B7AA9
		// (set) Token: 0x06002D22 RID: 11554 RVA: 0x000B98B1 File Offset: 0x000B7AB1
		public NPCPoI potentialCustomerPoI { get; private set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x000B98BA File Offset: 0x000B7ABA
		public string SaveFolderName
		{
			get
			{
				return "CustomerData";
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002D24 RID: 11556 RVA: 0x000B98BA File Offset: 0x000B7ABA
		public string SaveFileName
		{
			get
			{
				return "CustomerData";
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002D26 RID: 11558 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x000B98C1 File Offset: 0x000B7AC1
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x000B98C9 File Offset: 0x000B7AC9
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x000B98D2 File Offset: 0x000B7AD2
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x000B98DA File Offset: 0x000B7ADA
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x000B98E3 File Offset: 0x000B7AE3
		// (set) Token: 0x06002D2C RID: 11564 RVA: 0x000B98EB File Offset: 0x000B7AEB
		public bool HasChanged { get; set; }

		// Token: 0x06002D2D RID: 11565 RVA: 0x000B98F4 File Offset: 0x000B7AF4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Economy.Customer_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000B9914 File Offset: 0x000B7B14
		protected override void OnValidate()
		{
			base.OnValidate();
			if (this.DealSignal == null)
			{
				NPCAction npcaction = base.GetComponentInChildren<NPCScheduleManager>().ActionList.Find((NPCAction x) => x != null && x.GetType() == typeof(NPCSignal_WaitForDelivery));
				if (npcaction == null)
				{
					GameObject gameObject = new GameObject("DealSignal");
					gameObject.transform.SetParent(base.GetComponentInChildren<NPCScheduleManager>().transform);
					npcaction = gameObject.AddComponent<NPCSignal_WaitForDelivery>();
				}
				this.DealSignal = (npcaction as NPCSignal_WaitForDelivery);
			}
			if (this.DealSignal != null)
			{
				this.DealSignal.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000B99C0 File Offset: 0x000B7BC0
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.DayPass));
			if (this.NPC.RelationData.Unlocked)
			{
				this.OnCustomerUnlocked(NPCRelationData.EUnlockType.DirectApproach, false);
			}
			else
			{
				NPCRelationData relationData = this.NPC.RelationData;
				relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.OnCustomerUnlocked));
			}
			foreach (NPC npc in this.NPC.RelationData.Connections)
			{
				if (!(npc == null))
				{
					NPCRelationData relationData2 = npc.RelationData;
					relationData2.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData2.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType <p0>, bool <p1>)
					{
						this.UpdatePotentialCustomerPoI();
					}));
				}
			}
			if (this.NPC.MSGConversation != null)
			{
				this.<Start>g__RegisterLoadEvent|133_0();
			}
			else
			{
				NPC npc2 = this.NPC;
				npc2.onConversationCreated = (Action)Delegate.Combine(npc2.onConversationCreated, new Action(this.<Start>g__RegisterLoadEvent|133_0));
			}
			this.SetUpDialogue();
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000B9B20 File Offset: 0x000B7D20
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.SetupPoI();
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000B9B2E File Offset: 0x000B7D2E
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			this.ReceiveCustomerData(connection, this.GetCustomerData());
			if (this.DealSignal.IsActive)
			{
				this.ConfigureDealSignal(connection, this.DealSignal.StartTime, true);
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000B9B6D File Offset: 0x000B7D6D
		private void OnDestroy()
		{
			Customer.UnlockedCustomers.Remove(this);
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000B9B7C File Offset: 0x000B7D7C
		private void SetUpDialogue()
		{
			this.sampleChoice = new DialogueController.DialogueChoice();
			this.sampleChoice.ChoiceText = "Can I interest you in a free sample?";
			this.sampleChoice.Enabled = true;
			this.sampleChoice.Conversation = null;
			this.sampleChoice.onChoosen = new UnityEvent();
			this.sampleChoice.onChoosen.AddListener(new UnityAction(this.SampleOffered));
			this.sampleChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShowDirectApproachOption);
			this.sampleChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.SampleOptionValid);
			this.NPC.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.sampleChoice, -20);
			this.completeContractChoice = new DialogueController.DialogueChoice();
			this.completeContractChoice.ChoiceText = "[Complete Deal]";
			this.completeContractChoice.Enabled = true;
			this.completeContractChoice.Conversation = null;
			this.completeContractChoice.onChoosen = new UnityEvent();
			this.completeContractChoice.onChoosen.AddListener(new UnityAction(this.HandoverChosen));
			this.completeContractChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.IsReadyForHandover);
			this.completeContractChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.IsHandoverChoiceValid);
			this.NPC.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.completeContractChoice, 10);
			this.offerDealChoice = new DialogueController.DialogueChoice();
			this.offerDealChoice.ChoiceText = "You wanna buy something? [Offer a deal]";
			this.offerDealChoice.Enabled = true;
			this.offerDealChoice.Conversation = null;
			this.offerDealChoice.onChoosen = new UnityEvent();
			this.offerDealChoice.onChoosen.AddListener(new UnityAction(this.InstantDealOffered));
			this.offerDealChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShowOfferDealOption);
			this.offerDealChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.OfferDealValid);
			this.NPC.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(this.offerDealChoice, 0);
			this.awaitingDealGreeting = new DialogueController.GreetingOverride();
			this.awaitingDealGreeting.Greeting = this.dialogueDatabase.GetLine(EDialogueModule.Customer, "awaiting_deal");
			this.awaitingDealGreeting.ShouldShow = false;
			this.awaitingDealGreeting.PlayVO = true;
			this.awaitingDealGreeting.VOType = EVOLineType.Question;
			this.NPC.dialogueHandler.GetComponent<DialogueController>().AddGreetingOverride(this.awaitingDealGreeting);
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000B9DF8 File Offset: 0x000B7FF8
		private void SetupPoI()
		{
			if (this.potentialCustomerPoI != null)
			{
				return;
			}
			this.potentialCustomerPoI = UnityEngine.Object.Instantiate<NPCPoI>(NetworkSingleton<NPCManager>.Instance.PotentialCustomerPoIPrefab, base.transform);
			this.potentialCustomerPoI.SetMainText("Potential Customer\n" + this.NPC.fullName);
			this.potentialCustomerPoI.SetNPC(this.NPC);
			float y = (float)(this.NPC.FirstName[0] % '$') * 10f;
			float d = Mathf.Clamp((float)this.NPC.FirstName.Length * 1.5f, 1f, 10f);
			Vector3 vector = base.transform.forward;
			vector = Quaternion.Euler(0f, y, 0f) * vector;
			this.potentialCustomerPoI.transform.localPosition = vector * d;
			this.UpdatePotentialCustomerPoI();
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000B9EE4 File Offset: 0x000B80E4
		protected virtual void MinPass()
		{
			int num = this.TimeSincePlayerApproached;
			this.TimeSincePlayerApproached = num + 1;
			num = this.TimeSinceLastDealCompleted;
			this.TimeSinceLastDealCompleted = num + 1;
			num = this.TimeSinceLastDealOffered;
			this.TimeSinceLastDealOffered = num + 1;
			this.minsSinceUnlocked++;
			num = this.TimeSinceInstantDealOffered;
			this.TimeSinceInstantDealOffered = num + 1;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.HasChanged = true;
			if (this.DEBUG)
			{
				string str = "Current contract: ";
				Contract currentContract = this.CurrentContract;
				Console.Log(str + ((currentContract != null) ? currentContract.ToString() : null), null);
				string str2 = "Offered contract: ";
				ContractInfo contractInfo = this.OfferedContractInfo;
				Console.Log(str2 + ((contractInfo != null) ? contractInfo.ToString() : null), null);
				Console.Log("Awaiting sample: " + this.awaitingSample.ToString(), null);
				Console.Log("Sample offered today: " + this.sampleOfferedToday.ToString(), null);
				string str3 = "Dealer: ";
				Dealer assignedDealer = this.AssignedDealer;
				Console.Log(str3 + ((assignedDealer != null) ? assignedDealer.ToString() : null), null);
				Console.Log("Awaiting deal: " + this.IsAwaitingDelivery.ToString(), null);
			}
			if (this.ShouldTryGenerateDeal())
			{
				ContractInfo contractInfo2 = this.CheckContractGeneration(false);
				if (contractInfo2 != null)
				{
					if (this.AssignedDealer != null)
					{
						if (this.AssignedDealer.ShouldAcceptContract(contractInfo2, this))
						{
							num = this.OfferedDeals;
							this.OfferedDeals = num + 1;
							this.TimeSinceLastDealOffered = 0;
							this.OfferedContractInfo = contractInfo2;
							this.OfferedContractTime = NetworkSingleton<TimeManager>.Instance.GetDateTime();
							this.HasChanged = true;
							this.AssignedDealer.ContractedOffered(contractInfo2, this);
						}
					}
					else
					{
						this.OfferContract(contractInfo2);
					}
				}
			}
			if (this.ShouldTryApproachPlayer())
			{
				float num2 = Mathf.Lerp(0f, 0.5f, this.CurrentAddiction);
				if (UnityEngine.Random.Range(0f, 1f) < num2 / 1440f)
				{
					Player randomPlayer = Player.GetRandomPlayer(true, true);
					string str4 = "Approaching player: ";
					Player player = randomPlayer;
					Console.Log(str4 + ((player != null) ? player.ToString() : null), null);
					if (randomPlayer != null)
					{
						this.RequestProduct(randomPlayer);
					}
				}
			}
			if (this.OfferedContractInfo != null)
			{
				this.UpdateOfferExpiry();
			}
			else
			{
				MSGConversation msgconversation = this.NPC.MSGConversation;
				if (msgconversation != null)
				{
					msgconversation.SetSliderValue(0f, Color.white);
				}
			}
			if (this.CurrentContract != null)
			{
				this.UpdateDealAttendance();
			}
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000BA141 File Offset: 0x000B8341
		protected virtual void DayPass()
		{
			this.sampleOfferedToday = false;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if ((float)this.TimeSinceLastDealCompleted / 60f >= 24f)
			{
				this.ChangeAddiction(-0.0625f);
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000BA174 File Offset: 0x000B8374
		private void UpdateDealAttendance()
		{
			if (this.CurrentContract == null)
			{
				return;
			}
			float num = Vector3.Distance(this.NPC.Avatar.CenterPoint, this.CurrentContract.DeliveryLocation.CustomerStandPoint.position);
			if (this.DEBUG)
			{
				Console.Log("1", null);
			}
			if (!this.NPC.IsConscious)
			{
				this.CurrentContract.Fail(true);
				return;
			}
			if (this.DEBUG)
			{
				Console.Log("2", null);
			}
			if (this.DealSignal.IsActive && this.IsAwaitingDelivery && num < 10f)
			{
				return;
			}
			int windowStartTime = this.CurrentContract.DeliveryWindow.WindowStartTime;
			int num2 = TimeManager.AddMinutesTo24HourTime(this.CurrentContract.DeliveryWindow.WindowStartTime, 10);
			int windowEndTime = this.CurrentContract.DeliveryWindow.WindowEndTime;
			if (this.DEBUG)
			{
				Console.Log("Soft start: " + windowStartTime.ToString(), null);
				Console.Log("Hard start: " + num2.ToString(), null);
				Console.Log("End time: " + windowEndTime.ToString(), null);
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(num2, windowEndTime))
			{
				if (!this.DealSignal.IsActive)
				{
					this.ConfigureDealSignal(null, NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
				}
				if (num > Vector3.Distance(this.CurrentContract.DeliveryLocation.TeleportPoint.position, this.CurrentContract.DeliveryLocation.CustomerStandPoint.position) * 2f)
				{
					this.NPC.Movement.Warp(this.CurrentContract.DeliveryLocation.TeleportPoint.position);
					return;
				}
			}
			else
			{
				if (this.DealSignal.IsActive)
				{
					return;
				}
				int num3 = Mathf.CeilToInt(num / this.NPC.Movement.WalkSpeed * 2f);
				num3 = Mathf.Clamp(num3, 15, 360);
				int min = TimeManager.AddMinutesTo24HourTime(windowStartTime, -(num3 + 10));
				if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, num2))
				{
					this.ConfigureDealSignal(null, NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
				}
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000BA3A0 File Offset: 0x000B85A0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ConfigureDealSignal(NetworkConnection conn, int startTime, bool active)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ConfigureDealSignal_338960014(conn, startTime, active);
				this.RpcLogic___ConfigureDealSignal_338960014(conn, startTime, active);
			}
			else
			{
				this.RpcWriter___Target_ConfigureDealSignal_338960014(conn, startTime, active);
			}
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000BA3F0 File Offset: 0x000B85F0
		private void UpdateOfferExpiry()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			if (this.OfferedContractInfo == null)
			{
				this.NPC.MSGConversation.SetSliderValue(0f, Color.white);
				return;
			}
			int num = this.OfferedContractTime.GetMinSum() + 600;
			int minSum = this.OfferedContractTime.GetMinSum();
			float num2 = Mathf.Clamp01((float)(NetworkSingleton<TimeManager>.Instance.GetTotalMinSum() - minSum) / 600f);
			this.NPC.MSGConversation.SetSliderValue(1f - num2, Singleton<HUD>.Instance.RedGreenGradient.Evaluate(1f - num2));
			if (NetworkSingleton<TimeManager>.Instance.GetTotalMinSum() > num)
			{
				this.ExpireOffer();
				this.OfferedContractInfo = null;
			}
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000BA4B4 File Offset: 0x000B86B4
		private ContractInfo CheckContractGeneration(bool force = false)
		{
			if (!this.ShouldTryGenerateDeal() && !force)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " should not try to generate a deal", null);
				}
				return null;
			}
			if (this.OrderableProducts.Count == 0)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " has no orderable products", null);
				}
				return null;
			}
			if (this.AssignedDealer == null)
			{
				if (!ProductManager.IsAcceptingOrders && !force)
				{
					if (this.DEBUG)
					{
						Console.LogWarning("Not accepting orders", null);
					}
					return null;
				}
				if (NetworkSingleton<ProductManager>.Instance.TimeSinceProductListingChanged < 3f && !force)
				{
					if (this.DEBUG)
					{
						Console.LogWarning("Product listing changed too recently", null);
					}
					return null;
				}
			}
			int num = 7;
			if (this.AssignedDealer == null)
			{
				List<EDay> orderDays = this.customerData.GetOrderDays(this.CurrentAddiction, this.NPC.RelationData.RelationDelta / 5f);
				num = orderDays.Count;
				if (!orderDays.Contains(NetworkSingleton<TimeManager>.Instance.CurrentDay) && !force)
				{
					if (this.DEBUG)
					{
						Console.LogWarning(this.NPC.fullName + " cannot order today", null);
					}
					return null;
				}
			}
			int orderTime = this.customerData.OrderTime;
			int max;
			if (this.AssignedDealer == null)
			{
				max = TimeManager.AddMinutesTo24HourTime(orderTime, 120);
			}
			else
			{
				max = TimeManager.AddMinutesTo24HourTime(orderTime, 360);
			}
			if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(orderTime, max) && !force)
			{
				if (this.DEBUG)
				{
					Console.LogWarning(this.NPC.fullName + " cannot order now", null);
				}
				return null;
			}
			float num2 = this.customerData.GetAdjustedWeeklySpend(this.NPC.RelationData.RelationDelta / 5f) / (float)num;
			float num3;
			ProductDefinition weightedRandomProduct = this.GetWeightedRandomProduct(out num3);
			if (weightedRandomProduct == null || num3 < 0.05f)
			{
				if (this.DEBUG)
				{
					Console.Log(this.NPC.fullName + " has too low appeal for any products", null);
				}
				return null;
			}
			EQuality correspondingQuality = this.customerData.Standards.GetCorrespondingQuality();
			float productEnjoyment = this.GetProductEnjoyment(weightedRandomProduct, correspondingQuality);
			float num4 = weightedRandomProduct.Price * Mathf.Lerp(0.66f, 1.5f, productEnjoyment);
			num2 *= Mathf.Lerp(0.66f, 1.5f, productEnjoyment);
			int num5 = Mathf.RoundToInt(num2 / weightedRandomProduct.Price);
			num5 = Mathf.Clamp(num5, 1, 1000);
			if (this.AssignedDealer != null)
			{
				int productCount = this.AssignedDealer.GetProductCount(weightedRandomProduct.ID, correspondingQuality, EQuality.Heavenly);
				if (productCount < num5)
				{
					num5 = productCount;
				}
			}
			if (num5 >= 14)
			{
				num5 = Mathf.RoundToInt((float)(num5 / 5)) * 5;
			}
			float payment = (float)(Mathf.RoundToInt(num4 * (float)num5 / 5f) * 5);
			ProductList productList = new ProductList();
			productList.entries.Add(new ProductList.Entry
			{
				ProductID = weightedRandomProduct.ID,
				Quantity = num5,
				Quality = correspondingQuality
			});
			QuestWindowConfig deliveryWindow = new QuestWindowConfig
			{
				IsEnabled = true,
				WindowStartTime = 0,
				WindowEndTime = 0
			};
			DeliveryLocation deliveryLocation = this.DefaultDeliveryLocation;
			if (!GameManager.IS_TUTORIAL)
			{
				deliveryLocation = Singleton<Map>.Instance.GetRegionData(this.NPC.Region).GetRandomUnscheduledDeliveryLocation();
				if (deliveryLocation == null)
				{
					Console.LogError("No unscheduled delivery locations found for " + this.NPC.Region.ToString(), null);
					return null;
				}
			}
			return new ContractInfo(payment, productList, deliveryLocation.GUID.ToString(), deliveryWindow, true, 1, 0, false);
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000BA860 File Offset: 0x000B8A60
		private ProductDefinition GetWeightedRandomProduct(out float appeal)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			Dictionary<ProductDefinition, float> productAppeal = new Dictionary<ProductDefinition, float>();
			for (int i = 0; i < this.OrderableProducts.Count; i++)
			{
				float productEnjoyment = this.GetProductEnjoyment(this.OrderableProducts[i], this.customerData.Standards.GetCorrespondingQuality());
				float num2 = this.OrderableProducts[i].Price / this.OrderableProducts[i].MarketValue;
				float num3 = Mathf.Lerp(1f, -1f, num2 / 2f);
				float value = productEnjoyment + num3;
				productAppeal.Add(this.OrderableProducts[i], value);
			}
			(from x in this.OrderableProducts
			orderby productAppeal[x] descending
			select x).ToList<ProductDefinition>();
			if (num <= 0.5f || this.OrderableProducts.Count <= 1)
			{
				appeal = productAppeal[this.OrderableProducts[0]];
				return this.OrderableProducts[0];
			}
			if (num <= 0.75f || this.OrderableProducts.Count <= 2)
			{
				appeal = productAppeal[this.OrderableProducts[1]];
				return this.OrderableProducts[1];
			}
			if (num <= 0.875f || this.OrderableProducts.Count <= 3)
			{
				appeal = productAppeal[this.OrderableProducts[2]];
				return this.OrderableProducts[2];
			}
			appeal = productAppeal[this.OrderableProducts[3]];
			return this.OrderableProducts[3];
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000BAA1C File Offset: 0x000B8C1C
		protected virtual void OnCustomerUnlocked(NPCRelationData.EUnlockType unlockType, bool notify)
		{
			if (notify)
			{
				Singleton<NewCustomerPopup>.Instance.PlayPopup(this);
				this.minsSinceUnlocked = 0;
			}
			if (!Customer.UnlockedCustomers.Contains(this))
			{
				Customer.UnlockedCustomers.Add(this);
			}
			if (this.onUnlocked != null)
			{
				this.onUnlocked.Invoke();
			}
			if (Customer.onCustomerUnlocked != null)
			{
				Customer.onCustomerUnlocked(this);
			}
			this.UpdatePotentialCustomerPoI();
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000BAA81 File Offset: 0x000B8C81
		public void SetHasBeenRecommended()
		{
			this.HasBeenRecommended = true;
			this.HasChanged = true;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000BAA94 File Offset: 0x000B8C94
		public virtual void OfferContract(ContractInfo info)
		{
			DialogueChain dialogueChain = this.NPC.dialogueHandler.Database.GetChain(EDialogueModule.Customer, "contract_request");
			if (this.OfferedDeals == 0 && this.NPC.dialogueHandler.Database.HasChain(EDialogueModule.Generic, "first_contract_request"))
			{
				dialogueChain = this.NPC.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "first_contract_request");
			}
			dialogueChain = info.ProcessMessage(dialogueChain);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Offered_Contract_Count", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Offered_Contract_Count") + 1f).ToString(), true);
			int offeredDeals = this.OfferedDeals;
			this.OfferedDeals = offeredDeals + 1;
			this.TimeSinceLastDealOffered = 0;
			this.OfferedContractInfo = info;
			this.OfferedContractTime = NetworkSingleton<TimeManager>.Instance.GetDateTime();
			this.NotifyPlayerOfContract(this.OfferedContractInfo, dialogueChain.GetMessageChain(), true, true, true);
			this.HasChanged = true;
			this.SetOfferedContract(this.OfferedContractInfo, this.OfferedContractTime);
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000BAB8F File Offset: 0x000B8D8F
		[ObserversRpc]
		private void SetOfferedContract(ContractInfo info, GameDateTime offerTime)
		{
			this.RpcWriter___Observers_SetOfferedContract_4277245194(info, offerTime);
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000BABA0 File Offset: 0x000B8DA0
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public virtual void ExpireOffer()
		{
			this.RpcWriter___Server_ExpireOffer_2166136261();
			this.RpcLogic___ExpireOffer_2166136261();
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000BABBC File Offset: 0x000B8DBC
		public virtual void AssignContract(Contract contract)
		{
			this.CurrentContract = contract;
			this.CurrentContract.onQuestEnd.AddListener(new UnityAction<EQuestState>(this.CurrentContractEnded));
			this.DealSignal.Location = this.CurrentContract.DeliveryLocation;
			if (this.onContractAssigned != null)
			{
				this.onContractAssigned.Invoke(contract);
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000BAC18 File Offset: 0x000B8E18
		protected virtual void NotifyPlayerOfContract(ContractInfo contract, MessageChain offerMessage, bool canAccept, bool canReject, bool canCounterOffer = true)
		{
			this.NPC.MSGConversation.SendMessageChain(offerMessage, 0f, true, true);
			List<Response> list = new List<Response>();
			if (canAccept)
			{
				list.Add(new Response(Customer.PlayerAcceptMessages[UnityEngine.Random.Range(0, Customer.PlayerAcceptMessages.Length - 1)], "ACCEPT_CONTRACT", new Action(this.AcceptContractClicked), true));
			}
			if (canCounterOffer)
			{
				list.Add(new Response("[Counter-offer]", "COUNTEROFFER", new Action(this.CounterOfferClicked), true));
			}
			if (canReject)
			{
				list.Add(new Response(Customer.PlayerRejectMessages[UnityEngine.Random.Range(0, Customer.PlayerRejectMessages.Length - 1)], "REJECT_CONTRACT", new Action(this.ContractRejected), false));
			}
			this.NPC.MSGConversation.ShowResponses(list, 0f, true);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000BACEE File Offset: 0x000B8EEE
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendSetUpResponseCallbacks()
		{
			this.RpcWriter___Server_SendSetUpResponseCallbacks_2166136261();
			this.RpcLogic___SendSetUpResponseCallbacks_2166136261();
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000BACFC File Offset: 0x000B8EFC
		[ObserversRpc(RunLocally = true)]
		private void SetUpResponseCallbacks()
		{
			this.RpcWriter___Observers_SetUpResponseCallbacks_2166136261();
			this.RpcLogic___SetUpResponseCallbacks_2166136261();
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000BAD15 File Offset: 0x000B8F15
		protected virtual void AcceptContractClicked()
		{
			if (this.OfferedContractInfo == null)
			{
				Console.LogWarning("Offered contract is null!", null);
				return;
			}
			PlayerSingleton<MessagesApp>.Instance.DealWindowSelector.SetIsOpen(true, this.NPC.MSGConversation, new Action<EDealWindow>(this.PlayerAcceptedContract));
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000BAD54 File Offset: 0x000B8F54
		protected virtual void CounterOfferClicked()
		{
			if (this.OfferedContractInfo == null)
			{
				this.NPC.MSGConversation.ClearResponses(true);
				Console.LogWarning("Offered contract is null!", null);
				return;
			}
			ProductDefinition item = Registry.GetItem<ProductDefinition>(this.OfferedContractInfo.Products.entries[0].ProductID);
			int quantity = this.OfferedContractInfo.Products.entries[0].Quantity;
			float payment = this.OfferedContractInfo.Payment;
			PlayerSingleton<MessagesApp>.Instance.CounterofferInterface.Open(item, quantity, payment, this.NPC.MSGConversation, new Action<ProductDefinition, int, float>(this.SendCounteroffer));
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000BADFC File Offset: 0x000B8FFC
		protected virtual void SendCounteroffer(ProductDefinition product, int quantity, float price)
		{
			if (this.OfferedContractInfo == null)
			{
				Console.LogWarning("Offered contract is null!", null);
				return;
			}
			if (this.OfferedContractInfo.IsCounterOffer)
			{
				Console.LogWarning("Counter offer already sent", null);
				return;
			}
			string text = string.Concat(new string[]
			{
				"How about ",
				quantity.ToString(),
				"x ",
				product.Name,
				" for ",
				MoneyManager.FormatAmount(price, false, false),
				"?"
			});
			this.NPC.MSGConversation.SendMessage(new Message(text, Message.ESenderType.Player, false, -1), true, true);
			this.NPC.MSGConversation.ClearResponses(false);
			this.ProcessCounterOfferServerSide(product.ID, quantity, price);
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000BAEBC File Offset: 0x000B90BC
		[ServerRpc(RequireOwnership = false)]
		private void ProcessCounterOfferServerSide(string productID, int quantity, float price)
		{
			this.RpcWriter___Server_ProcessCounterOfferServerSide_900355577(productID, quantity, price);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000BAEDB File Offset: 0x000B90DB
		[ObserversRpc(RunLocally = true)]
		private void SetContractIsCounterOffer()
		{
			this.RpcWriter___Observers_SetContractIsCounterOffer_2166136261();
			this.RpcLogic___SetContractIsCounterOffer_2166136261();
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000BAEEC File Offset: 0x000B90EC
		protected virtual void PlayerAcceptedContract(EDealWindow window)
		{
			Console.Log("Player accepted contract in window " + window.ToString(), null);
			if (this.OfferedContractInfo == null)
			{
				Console.LogWarning("Offered contract is null!", null);
				return;
			}
			if (this.CurrentContract != null)
			{
				Console.LogWarning("Customer already has a contract!", null);
				return;
			}
			if (this.NPC.MSGConversation != null)
			{
				string text = this.NPC.MSGConversation.GetResponse("ACCEPT_CONTRACT").text;
				if (this.OfferedContractInfo.IsCounterOffer)
				{
					switch (window)
					{
					case EDealWindow.Morning:
						text = "Morning";
						break;
					case EDealWindow.Afternoon:
						text = "Afternoon";
						break;
					case EDealWindow.Night:
						text = "Night";
						break;
					case EDealWindow.LateNight:
						text = "Late Night";
						break;
					}
				}
				this.NPC.MSGConversation.SendMessage(new Message(text, Message.ESenderType.Player, true, -1), true, true);
				this.NPC.MSGConversation.ClearResponses(true);
			}
			else
			{
				Console.LogWarning("NPC.MSGConversation is null!", null);
			}
			DealWindowInfo windowInfo = DealWindowInfo.GetWindowInfo(window);
			this.OfferedContractInfo.DeliveryWindow.WindowStartTime = windowInfo.StartTime;
			this.OfferedContractInfo.DeliveryWindow.WindowEndTime = windowInfo.EndTime;
			this.PlayContractAcceptedReaction();
			this.SendContractAccepted(window, true);
			if (!InstanceFinder.IsServer)
			{
				this.OfferedContractInfo = null;
			}
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000BB03A File Offset: 0x000B923A
		[ServerRpc(RequireOwnership = false)]
		private void SendContractAccepted(EDealWindow window, bool trackContract)
		{
			this.RpcWriter___Server_SendContractAccepted_507093020(window, trackContract);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000BB04C File Offset: 0x000B924C
		public virtual string ContractAccepted(EDealWindow window, bool trackContract)
		{
			if (this.OfferedContractInfo == null)
			{
				Console.LogWarning("Offered contract is null!", null);
				return null;
			}
			DealWindowInfo windowInfo = DealWindowInfo.GetWindowInfo(window);
			this.OfferedContractInfo.DeliveryWindow.WindowStartTime = windowInfo.StartTime;
			this.OfferedContractInfo.DeliveryWindow.WindowEndTime = windowInfo.EndTime;
			string text = GUIDManager.GenerateUniqueGUID().ToString();
			NetworkSingleton<QuestManager>.Instance.SendContractAccepted(base.NetworkObject, this.OfferedContractInfo, trackContract, text);
			this.ReceiveContractAccepted();
			return text;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000BB0D4 File Offset: 0x000B92D4
		[ObserversRpc(RunLocally = true)]
		private void ReceiveContractAccepted()
		{
			this.RpcWriter___Observers_ReceiveContractAccepted_2166136261();
			this.RpcLogic___ReceiveContractAccepted_2166136261();
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000BB0E4 File Offset: 0x000B92E4
		protected virtual void PlayContractAcceptedReaction()
		{
			DialogueChain dialogueChain = this.dialogueDatabase.GetChain(EDialogueModule.Customer, "contract_accepted");
			dialogueChain = this.OfferedContractInfo.ProcessMessage(dialogueChain);
			this.NPC.MSGConversation.SendMessageChain(dialogueChain.GetMessageChain(), 0.5f, false, true);
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000BB130 File Offset: 0x000B9330
		protected virtual bool EvaluateCounteroffer(ProductDefinition product, int quantity, float price)
		{
			float adjustedWeeklySpend = this.customerData.GetAdjustedWeeklySpend(this.NPC.RelationData.RelationDelta / 5f);
			List<EDay> orderDays = this.customerData.GetOrderDays(this.CurrentAddiction, this.NPC.RelationData.RelationDelta / 5f);
			float num = adjustedWeeklySpend / (float)orderDays.Count;
			if (price >= num * 3f)
			{
				return false;
			}
			float valueProposition = Customer.GetValueProposition(Registry.GetItem<ProductDefinition>(this.OfferedContractInfo.Products.entries[0].ProductID), this.OfferedContractInfo.Payment / (float)this.OfferedContractInfo.Products.entries[0].Quantity);
			float productEnjoyment = this.GetProductEnjoyment(product, this.customerData.Standards.GetCorrespondingQuality());
			float num2 = Mathf.InverseLerp(-1f, 1f, productEnjoyment);
			float valueProposition2 = Customer.GetValueProposition(product, price / (float)quantity);
			float num3 = Mathf.Pow((float)quantity / (float)this.OfferedContractInfo.Products.entries[0].Quantity, 0.6f);
			float num4 = Mathf.Lerp(0f, 2f, num3 * 0.5f);
			float num5 = Mathf.Lerp(1f, 0f, Mathf.Abs(num4 - 1f));
			if (valueProposition2 * num5 > valueProposition)
			{
				return true;
			}
			if (valueProposition2 < 0.12f)
			{
				return false;
			}
			float num6 = productEnjoyment * valueProposition;
			float num7 = num2 * num5 * valueProposition2;
			if (num7 > num6)
			{
				return true;
			}
			float num8 = num6 - num7;
			float num9 = Mathf.Lerp(0f, 1f, num8 / 0.2f);
			float t = Mathf.Max(this.CurrentAddiction, this.NPC.RelationData.NormalizedRelationDelta);
			float num10 = Mathf.Lerp(0f, 0.2f, t);
			return UnityEngine.Random.Range(0f, 0.9f) + num10 > num9;
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000BB314 File Offset: 0x000B9514
		public static float GetValueProposition(ProductDefinition product, float price)
		{
			float num = product.MarketValue / price;
			if (num < 1f)
			{
				num = Mathf.Pow(num, 2.5f);
			}
			return Mathf.Clamp(num, 0f, 2f);
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000BB34E File Offset: 0x000B954E
		protected virtual void ContractRejected()
		{
			if (this.OfferedContractInfo == null)
			{
				Console.LogWarning("Offered contract is null!", null);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				this.PlayContractRejectedReaction();
				this.ReceiveContractRejected();
			}
			this.OfferedContractInfo = null;
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000BB37E File Offset: 0x000B957E
		[ObserversRpc(RunLocally = true)]
		private void ReceiveContractRejected()
		{
			this.RpcWriter___Observers_ReceiveContractRejected_2166136261();
			this.RpcLogic___ReceiveContractRejected_2166136261();
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000BB38C File Offset: 0x000B958C
		protected virtual void PlayContractRejectedReaction()
		{
			DialogueChain dialogueChain = this.dialogueDatabase.GetChain(EDialogueModule.Customer, "contract_rejected");
			dialogueChain = this.OfferedContractInfo.ProcessMessage(dialogueChain);
			this.NPC.MSGConversation.SendMessageChain(dialogueChain.GetMessageChain(), 0.5f, false, true);
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000BB3D8 File Offset: 0x000B95D8
		public virtual void SetIsAwaitingDelivery(bool awaiting)
		{
			this.IsAwaitingDelivery = awaiting;
			if (awaiting && this.CurrentContract != null)
			{
				this.DealSignal.Location = this.CurrentContract.DeliveryLocation;
				int min = TimeManager.AddMinutesTo24HourTime(this.CurrentContract.DeliveryWindow.WindowEndTime, -60);
				int num = NetworkSingleton<TimeManager>.Instance.GetTotalMinSum() - this.CurrentContract.AcceptTime.GetMinSum();
				if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, this.CurrentContract.DeliveryWindow.WindowStartTime) && num > 300)
				{
					this.awaitingDealGreeting.Greeting = this.dialogueDatabase.GetLine(EDialogueModule.Customer, "late_deal");
				}
				else
				{
					this.awaitingDealGreeting.Greeting = this.dialogueDatabase.GetLine(EDialogueModule.Customer, "awaiting_deal");
				}
			}
			if (this.awaitingDealGreeting != null)
			{
				this.awaitingDealGreeting.ShouldShow = awaiting;
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000BB4C4 File Offset: 0x000B96C4
		public bool IsAtDealLocation()
		{
			return !(this.CurrentContract == null) && this.IsAwaitingDelivery && this.DealSignal.IsActive && !this.NPC.Movement.IsMoving && Vector3.Distance(base.transform.position, this.CurrentContract.DeliveryLocation.CustomerStandPoint.position) < 1f;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000BB53A File Offset: 0x000B973A
		private void UpdatePotentialCustomerPoI()
		{
			if (this.potentialCustomerPoI == null)
			{
				return;
			}
			this.potentialCustomerPoI.enabled = (!this.NPC.RelationData.Unlocked && this.IsUnlockable());
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000BB571 File Offset: 0x000B9771
		public void SetPotentialCustomerPoIEnabled(bool enabled)
		{
			if (this.potentialCustomerPoI == null)
			{
				return;
			}
			this.potentialCustomerPoI.enabled = enabled;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000BB590 File Offset: 0x000B9790
		protected virtual bool ShouldTryGenerateDeal()
		{
			if (!this.NPC.RelationData.Unlocked)
			{
				return false;
			}
			if (this.CurrentContract != null)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " already has a contract", null);
				}
				return false;
			}
			if (this.OfferedContractInfo != null)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " already offered contract", null);
				}
				return false;
			}
			int num = (int)('ɘ' + this.NPC.FirstName[0] % '\n' * '\u0014');
			if (this.TimeSinceLastDealCompleted < num)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " has not waited long enough since last deal", null);
				}
				return false;
			}
			if (this.TimeSinceLastDealOffered < num)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " has not waited long enough since last offer", null);
				}
				return false;
			}
			if (this.minsSinceUnlocked < 30)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " has not waited long enough since unlocked", null);
				}
				return false;
			}
			if (!this.NPC.IsConscious)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " is not conscious", null);
				}
				return false;
			}
			if (this.NPC.behaviour.RequestProductBehaviour.Active)
			{
				if (this.DEBUG)
				{
					Console.LogWarning("Customer " + this.NPC.fullName + " is already requesting a product", null);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000BB74C File Offset: 0x000B994C
		public virtual void OfferDealItems(List<ItemInstance> items, bool offeredByPlayer, out bool accepted)
		{
			accepted = false;
			if (this.CurrentContract == null)
			{
				return;
			}
			int num;
			float productListMatch = this.CurrentContract.GetProductListMatch(items, out num);
			accepted = (UnityEngine.Random.Range(0f, 1f) < productListMatch || GameManager.IS_TUTORIAL);
			if (accepted || !offeredByPlayer)
			{
				this.ProcessHandover(HandoverScreen.EHandoverOutcome.Finalize, this.CurrentContract, items, offeredByPlayer, true);
				return;
			}
			this.CustomerRejectedDeal(offeredByPlayer);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000BB7B8 File Offset: 0x000B99B8
		public virtual void CustomerRejectedDeal(bool offeredByPlayer)
		{
			Console.Log("Customer rejected deal", null);
			if (offeredByPlayer)
			{
				Singleton<HandoverScreen>.Instance.ClearCustomerSlots(true);
			}
			this.CurrentContract.Fail(true);
			this.NPC.RelationData.ChangeRelationship(-0.5f, true);
			this.NPC.PlayVO(EVOLineType.Annoyed);
			this.NPC.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "deal_rejected", 30f, 0);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "customer_rejected_deal"), 5f);
			this.TimeSinceLastDealCompleted = 0;
			if (this.NPC.RelationData.RelationDelta < 2.5f && offeredByPlayer && this.NPC.responses is NPCResponses_Civilian && this.NPC.Aggression > 0.5f && UnityEngine.Random.Range(0f, this.NPC.RelationData.NormalizedRelationDelta) < this.NPC.Aggression * 0.5f)
			{
				float num;
				this.NPC.behaviour.CombatBehaviour.SetTarget(null, Player.GetClosestPlayer(base.transform.position, out num, null).NetworkObject);
				this.NPC.behaviour.CombatBehaviour.Enable_Networked(null);
			}
			base.Invoke("EndWait", 1f);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000BB928 File Offset: 0x000B9B28
		public virtual void ProcessHandover(HandoverScreen.EHandoverOutcome outcome, Contract contract, List<ItemInstance> items, bool handoverByPlayer, bool giveBonuses = true)
		{
			float num;
			EDrugType drugType;
			int num2;
			float satisfaction = Mathf.Clamp01(this.EvaluateDelivery(contract, items, out num, out drugType, out num2));
			this.ChangeAddiction(num / 5f);
			float relationDelta = this.NPC.RelationData.RelationDelta;
			float relationshipChange = CustomerSatisfaction.GetRelationshipChange(satisfaction);
			float change = relationshipChange * 0.2f * Mathf.Lerp(0.75f, 1.5f, num);
			this.AdjustAffinity(drugType, change);
			this.NPC.RelationData.ChangeRelationship(relationshipChange, true);
			List<Contract.BonusPayment> list = new List<Contract.BonusPayment>();
			if (giveBonuses)
			{
				if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
				{
					list.Add(new Contract.BonusPayment("Curfew Bonus", contract.Payment * 0.2f));
				}
				if (num2 > contract.ProductList.GetTotalQuantity())
				{
					list.Add(new Contract.BonusPayment("Generosity Bonus", 10f * (float)(num2 - contract.ProductList.GetTotalQuantity())));
				}
				GameDateTime acceptTime = contract.AcceptTime;
				GameDateTime end = new GameDateTime(acceptTime.elapsedDays, TimeManager.AddMinutesTo24HourTime(contract.DeliveryWindow.WindowStartTime, 60));
				if (NetworkSingleton<TimeManager>.Instance.IsCurrentDateWithinRange(acceptTime, end))
				{
					list.Add(new Contract.BonusPayment("Quick Delivery Bonus", contract.Payment * 0.1f));
				}
			}
			float num3 = 0f;
			foreach (Contract.BonusPayment bonusPayment in list)
			{
				Console.Log("Bonus: " + bonusPayment.Title + " Amount: " + bonusPayment.Amount.ToString(), null);
				num3 += bonusPayment.Amount;
			}
			if (handoverByPlayer)
			{
				Singleton<HandoverScreen>.Instance.ClearCustomerSlots(false);
				contract.SubmitPayment(num3);
			}
			if (outcome == HandoverScreen.EHandoverOutcome.Finalize && handoverByPlayer)
			{
				Singleton<DealCompletionPopup>.Instance.PlayPopup(this, satisfaction, relationDelta, contract.Payment, list);
			}
			this.TimeSinceLastDealCompleted = 0;
			this.NPC.SendAnimationTrigger("GrabItem");
			NetworkObject networkObject = null;
			if (contract.Dealer != null)
			{
				networkObject = contract.Dealer.NetworkObject;
			}
			Console.Log(string.Concat(new string[]
			{
				"Base payment: ",
				contract.Payment.ToString(),
				" Total bonus: ",
				num3.ToString(),
				" Satisfaction: ",
				satisfaction.ToString(),
				" Dealer: ",
				(networkObject != null) ? networkObject.name : null
			}), null);
			float totalPayment = Mathf.Clamp(contract.Payment + num3, 0f, float.MaxValue);
			this.ProcessHandoverServerSide(outcome, items, handoverByPlayer, totalPayment, contract.ProductList, satisfaction, networkObject);
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000BBBE0 File Offset: 0x000B9DE0
		[ServerRpc(RequireOwnership = false)]
		private void ProcessHandoverServerSide(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, bool handoverByPlayer, float totalPayment, ProductList productList, float satisfaction, NetworkObject dealer)
		{
			this.RpcWriter___Server_ProcessHandoverServerSide_3760244802(outcome, items, handoverByPlayer, totalPayment, productList, satisfaction, dealer);
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000BBC10 File Offset: 0x000B9E10
		[ObserversRpc]
		private void ProcessHandoverClient(float satisfaction, bool handoverByPlayer, string npcToRecommend)
		{
			this.RpcWriter___Observers_ProcessHandoverClient_537707335(satisfaction, handoverByPlayer, npcToRecommend);
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000BBC30 File Offset: 0x000B9E30
		public void ContractWellReceived(string npcToRecommend)
		{
			NPC npc = null;
			if (!string.IsNullOrEmpty(npcToRecommend))
			{
				npc = NPCManager.GetNPC(npcToRecommend);
			}
			if (!(npc != null))
			{
				this.NPC.PlayVO(EVOLineType.Thanks);
				this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "deal_completed"), 5f);
				this.NPC.Avatar.EmotionManager.AddEmotionOverride("Cheery", "contract_done", 10f, 0);
				return;
			}
			if (npc is Dealer)
			{
				this.RecommendDealer(npc as Dealer);
				return;
			}
			if (npc is Supplier)
			{
				this.RecommendSupplier(npc as Supplier);
				return;
			}
			this.RecommendCustomer(npc.GetComponent<Customer>());
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000BBCE8 File Offset: 0x000B9EE8
		private void RecommendDealer(Dealer dealer)
		{
			Customer.<>c__DisplayClass182_0 CS$<>8__locals1 = new Customer.<>c__DisplayClass182_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.dealer = dealer;
			if (CS$<>8__locals1.dealer == null)
			{
				Console.LogWarning("Dealer is null!", null);
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Customer ",
				this.NPC.fullName,
				" recommended dealer ",
				CS$<>8__locals1.dealer.fullName,
				" to player"
			}), null);
			CS$<>8__locals1.alreadyRecommended = CS$<>8__locals1.dealer.HasBeenRecommended;
			CS$<>8__locals1.dealer.MarkAsRecommended();
			float num;
			if (Player.GetClosestPlayer(base.transform.position, out num, null) == Player.Local)
			{
				Customer.<>c__DisplayClass182_1 CS$<>8__locals2 = new Customer.<>c__DisplayClass182_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				string dialogueText = this.dialogueDatabase.GetLine(EDialogueModule.Customer, "post_deal_recommend_dealer").Replace("<NAME>", CS$<>8__locals2.CS$<>8__locals1.dealer.fullName);
				CS$<>8__locals2.container = new DialogueContainer();
				DialogueNodeData dialogueNodeData = new DialogueNodeData();
				dialogueNodeData.DialogueText = dialogueText;
				dialogueNodeData.choices = new DialogueChoiceData[0];
				dialogueNodeData.DialogueNodeLabel = "ENTRY";
				dialogueNodeData.VoiceLine = EVOLineType.Thanks;
				CS$<>8__locals2.container.DialogueNodeData.Add(dialogueNodeData);
				Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals2.<RecommendDealer>g__Wait|0());
			}
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000BBE40 File Offset: 0x000BA040
		private void RecommendSupplier(Supplier supplier)
		{
			Customer.<>c__DisplayClass183_0 CS$<>8__locals1 = new Customer.<>c__DisplayClass183_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.supplier = supplier;
			if (CS$<>8__locals1.supplier == null)
			{
				Console.LogWarning("Supplier is null!", null);
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Customer ",
				this.NPC.fullName,
				" recommended supplier ",
				CS$<>8__locals1.supplier.fullName,
				" to player"
			}), null);
			CS$<>8__locals1.alreadyRecommended = CS$<>8__locals1.supplier.RelationData.Unlocked;
			CS$<>8__locals1.supplier.SendUnlocked();
			float num;
			if (Player.GetClosestPlayer(base.transform.position, out num, null) == Player.Local)
			{
				string supplierRecommendMessage = CS$<>8__locals1.supplier.SupplierRecommendMessage;
				CS$<>8__locals1.container = new DialogueContainer();
				DialogueNodeData dialogueNodeData = new DialogueNodeData();
				dialogueNodeData.DialogueText = supplierRecommendMessage;
				dialogueNodeData.choices = new DialogueChoiceData[0];
				dialogueNodeData.DialogueNodeLabel = "ENTRY";
				dialogueNodeData.VoiceLine = EVOLineType.Thanks;
				CS$<>8__locals1.container.DialogueNodeData.Add(dialogueNodeData);
				Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<RecommendSupplier>g__Wait|0());
			}
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000BBF64 File Offset: 0x000BA164
		private void RecommendCustomer(Customer friend)
		{
			Customer.<>c__DisplayClass184_0 CS$<>8__locals1 = new Customer.<>c__DisplayClass184_0();
			CS$<>8__locals1.<>4__this = this;
			if (friend == null)
			{
				Console.LogWarning("Friend is null!", null);
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Customer ",
				this.NPC.fullName,
				" recommended friend ",
				friend.NPC.fullName,
				" to player"
			}), null);
			friend.SetHasBeenRecommended();
			float num;
			if (Player.GetClosestPlayer(base.transform.position, out num, null) == Player.Local)
			{
				string text = this.dialogueDatabase.GetLine(EDialogueModule.Customer, "post_deal_recommend").Replace("<NAME>", friend.NPC.fullName);
				text = text.Replace("they", friend.NPC.Avatar.GetThirdPersonAddress(false));
				text = text.Replace("them", friend.NPC.Avatar.GetThirdPersonPronoun(false));
				CS$<>8__locals1.container = new DialogueContainer();
				DialogueNodeData dialogueNodeData = new DialogueNodeData();
				dialogueNodeData.DialogueText = text;
				dialogueNodeData.choices = new DialogueChoiceData[0];
				dialogueNodeData.DialogueNodeLabel = "ENTRY";
				dialogueNodeData.VoiceLine = EVOLineType.Thanks;
				CS$<>8__locals1.container.DialogueNodeData.Add(dialogueNodeData);
				Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<RecommendCustomer>g__Wait|0());
			}
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000BC0B9 File Offset: 0x000BA2B9
		public virtual void CurrentContractEnded(EQuestState outcome)
		{
			if (outcome == EQuestState.Expired)
			{
				this.NPC.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "contract_expired", 30f, 0);
			}
			this.ConfigureDealSignal(null, 0, false);
			this.CurrentContract = null;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000BC0F4 File Offset: 0x000BA2F4
		public virtual float EvaluateDelivery(Contract contract, List<ItemInstance> providedItems, out float highestAddiction, out EDrugType mainTypeType, out int matchedProductCount)
		{
			highestAddiction = 0f;
			mainTypeType = EDrugType.Marijuana;
			using (List<ProductList.Entry>.Enumerator enumerator = contract.ProductList.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProductList.Entry entry = enumerator.Current;
					List<ItemInstance> list = (from x in providedItems
					where x.ID == entry.ProductID
					select x).ToList<ItemInstance>();
					List<ProductItemInstance> list2 = new List<ProductItemInstance>();
					for (int i = 0; i < list.Count; i++)
					{
						ProductItemInstance productItemInstance = list[i] as ProductItemInstance;
						if (!(productItemInstance.AppliedPackaging == null))
						{
							list2.Add(productItemInstance);
						}
					}
					list2 = (from x in list2
					orderby x.Quality descending
					select x).ToList<ProductItemInstance>();
					int num = entry.Quantity;
					int num2 = 0;
					while (num2 < list2.Count && num > 0)
					{
						mainTypeType = (list2[num2].Definition as ProductDefinition).DrugTypes[0].DrugType;
						float addictiveness = (list2[num2].Definition as ProductDefinition).GetAddictiveness();
						if (addictiveness > highestAddiction)
						{
							highestAddiction = addictiveness;
						}
						num--;
						num2++;
					}
				}
			}
			return contract.GetProductListMatch(providedItems, out matchedProductCount);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000BC270 File Offset: 0x000BA470
		[ServerRpc(RequireOwnership = false)]
		public void ChangeAddiction(float change)
		{
			this.RpcWriter___Server_ChangeAddiction_431000436(change);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000BC27C File Offset: 0x000BA47C
		private void ConsumeProduct(ItemInstance item)
		{
			Customer.<>c__DisplayClass188_0 CS$<>8__locals1 = new Customer.<>c__DisplayClass188_0();
			CS$<>8__locals1.item = item;
			CS$<>8__locals1.<>4__this = this;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ConsumeProduct>g__Wait|0());
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000BC2B0 File Offset: 0x000BA4B0
		protected virtual bool ShowOfferDealOption(bool enabled)
		{
			return (Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None || Player.Local.CrimeData.TimeSinceSighted >= 5f) && !(this.CurrentContract != null) && (enabled && !this.IsAwaitingDelivery && this.NPC.RelationData.Unlocked) && !this.NPC.behaviour.RequestProductBehaviour.Active;
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000BC330 File Offset: 0x000BA530
		protected virtual bool OfferDealValid(out string invalidReason)
		{
			invalidReason = string.Empty;
			if (this.TimeSinceLastDealCompleted < 360)
			{
				invalidReason = "Customer recently completed a deal";
				return false;
			}
			if (this.OfferedContractInfo != null)
			{
				invalidReason = "Customer already has a pending offer";
				return false;
			}
			if (this.TimeSinceInstantDealOffered < 360 && !this.pendingInstantDeal)
			{
				invalidReason = "Already recently offered";
				return false;
			}
			return true;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000BC38C File Offset: 0x000BA58C
		protected virtual void InstantDealOffered()
		{
			float num = Mathf.Clamp01((float)this.TimeSinceLastDealCompleted / 1440f) * 0.5f;
			float num2 = this.NPC.RelationData.NormalizedRelationDelta * 0.3f;
			float num3 = this.CurrentAddiction * 0.2f;
			float num4 = num + num2 + num3;
			this.TimeSinceInstantDealOffered = 0;
			if (UnityEngine.Random.Range(0f, 1f) < num4 || this.pendingInstantDeal)
			{
				this.NPC.PlayVO(EVOLineType.Acknowledge);
				this.pendingInstantDeal = true;
				this.NPC.dialogueHandler.SkipNextDialogueBehaviourEnd();
				Singleton<HandoverScreen>.Instance.Open(null, this, HandoverScreen.EMode.Offer, new Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float>(this.<InstantDealOffered>g__HandoverClosed|191_0), new Func<List<ItemInstance>, float, float>(this.GetOfferSuccessChance));
				return;
			}
			this.NPC.PlayVO(EVOLineType.No);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue_5s(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "offer_reject"));
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000BC474 File Offset: 0x000BA674
		public float GetOfferSuccessChance(List<ItemInstance> items, float askingPrice)
		{
			float adjustedWeeklySpend = this.CustomerData.GetAdjustedWeeklySpend(this.NPC.RelationData.RelationDelta / 5f);
			List<EDay> orderDays = this.CustomerData.GetOrderDays(this.CurrentAddiction, this.NPC.RelationData.RelationDelta / 5f);
			float num = adjustedWeeklySpend / (float)orderDays.Count;
			float num2 = 0f;
			int num3 = 0;
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = items[i] as ProductItemInstance;
					if (!(productItemInstance.AppliedPackaging == null))
					{
						float productEnjoyment = this.GetProductEnjoyment(items[i].Definition as ProductDefinition, productItemInstance.Quality);
						float num4 = Mathf.InverseLerp(-1f, 1f, productEnjoyment);
						num2 += num4 * (float)productItemInstance.Quantity * (float)productItemInstance.Amount;
						num3 += productItemInstance.Quantity * productItemInstance.Amount;
					}
				}
			}
			if (num3 == 0)
			{
				return 0f;
			}
			float num5 = num2 / (float)num3;
			float price = askingPrice / (float)num3;
			float num6 = 0f;
			for (int j = 0; j < items.Count; j++)
			{
				if (items[j] is ProductItemInstance)
				{
					ProductItemInstance productItemInstance2 = items[j] as ProductItemInstance;
					if (!(productItemInstance2.AppliedPackaging == null))
					{
						float valueProposition = Customer.GetValueProposition(productItemInstance2.Definition as ProductDefinition, price);
						num6 += valueProposition * (float)productItemInstance2.Amount * (float)productItemInstance2.Quantity;
					}
				}
			}
			float f = num6 / (float)num3;
			float num7 = askingPrice / num;
			float item = 1f;
			if (num7 > 1f)
			{
				float num8 = Mathf.Sqrt(num7);
				item = Mathf.Clamp(1f - num8 / 4f, 0.01f, 1f);
			}
			float item2 = num5 + this.CurrentAddiction * 0.25f;
			float item3 = Mathf.Pow(f, 1.5f);
			List<float> list = new List<float>
			{
				item2,
				item3,
				item
			};
			list.Sort();
			if (list[0] < 0.01f)
			{
				return 0f;
			}
			if (num7 > 3f)
			{
				return 0f;
			}
			return list[0] * 0.7f + list[1] * 0.2f + list[2] * 0.1f;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000BC6E4 File Offset: 0x000BA8E4
		protected virtual bool ShouldTryApproachPlayer()
		{
			if (!this.NPC.RelationData.Unlocked)
			{
				return false;
			}
			if (this.CurrentContract != null)
			{
				return false;
			}
			if (this.OfferedContractInfo != null)
			{
				return false;
			}
			if (this.TimeSinceLastDealCompleted < 1440)
			{
				return false;
			}
			if (this.minsSinceUnlocked < 30)
			{
				return false;
			}
			if (!this.NPC.IsConscious)
			{
				return false;
			}
			if (this.AssignedDealer != null)
			{
				return false;
			}
			if (this.NPC.behaviour.RequestProductBehaviour.Active)
			{
				return false;
			}
			if (this.NPC.dialogueHandler.IsPlaying)
			{
				return false;
			}
			if (this.CurrentAddiction < 0.33f)
			{
				return false;
			}
			if ((float)this.TimeSincePlayerApproached < Mathf.Lerp(4320f, 2160f, this.CurrentAddiction))
			{
				return false;
			}
			if (this.OrderableProducts.Count == 0)
			{
				return false;
			}
			float num;
			if (Player.GetClosestPlayer(base.transform.position, out num, null) == null)
			{
				return false;
			}
			if (num < 20f)
			{
				return false;
			}
			for (int i = 0; i < Customer.UnlockedCustomers.Count; i++)
			{
				if (Customer.UnlockedCustomers[i].NPC.behaviour.RequestProductBehaviour.Active)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000BC824 File Offset: 0x000BAA24
		[Button]
		public void RequestProduct()
		{
			this.RequestProduct(Player.GetRandomPlayer(true, true));
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000BC834 File Offset: 0x000BAA34
		public void RequestProduct(Player target)
		{
			Console.Log(this.NPC.fullName + " is requesting product from " + target.PlayerName, null);
			this.TimeSincePlayerApproached = 0;
			this.NPC.behaviour.RequestProductBehaviour.AssignTarget(target.NetworkObject);
			this.NPC.behaviour.RequestProductBehaviour.Enable_Networked(null);
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000BC89C File Offset: 0x000BAA9C
		public void PlayerRejectedProductRequest()
		{
			this.NPC.PlayVO(EVOLineType.Annoyed);
			this.NPC.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "product_rejected", 30f, 1);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "request_product_rejected"), 5f);
			if (this.NPC.responses is NPCResponses_Civilian && this.NPC.Aggression > 0.1f)
			{
				float num = Mathf.Clamp(this.NPC.Aggression, 0f, 0.7f);
				num -= this.NPC.RelationData.NormalizedRelationDelta * 0.3f;
				num += this.CurrentAddiction * 0.2f;
				if (UnityEngine.Random.Range(0f, 1f) < num)
				{
					float num2;
					this.NPC.behaviour.CombatBehaviour.SetTarget(null, Player.GetClosestPlayer(base.transform.position, out num2, null).NetworkObject);
					this.NPC.behaviour.CombatBehaviour.Enable_Networked(null);
				}
			}
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000BC9C4 File Offset: 0x000BABC4
		[ServerRpc(RequireOwnership = false)]
		public void RejectProductRequestOffer()
		{
			this.RpcWriter___Server_RejectProductRequestOffer_2166136261();
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000BC9D8 File Offset: 0x000BABD8
		[ObserversRpc(RunLocally = true)]
		private void RejectProductRequestOffer_Local()
		{
			this.RpcWriter___Observers_RejectProductRequestOffer_Local_2166136261();
			this.RpcLogic___RejectProductRequestOffer_Local_2166136261();
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000BC9F1 File Offset: 0x000BABF1
		public void AssignDealer(Dealer dealer)
		{
			this.AssignedDealer = dealer;
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000BC9FA File Offset: 0x000BABFA
		public virtual string GetSaveString()
		{
			return this.GetCustomerData().GetJson(true);
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000BCA08 File Offset: 0x000BAC08
		public CustomerData GetCustomerData()
		{
			string[] array = new string[this.OrderableProducts.Count];
			for (int i = 0; i < this.OrderableProducts.Count; i++)
			{
				array[i] = this.OrderableProducts[i].ID;
			}
			float[] array2 = new float[this.currentAffinityData.ProductAffinities.Count];
			for (int j = 0; j < this.currentAffinityData.ProductAffinities.Count; j++)
			{
				array2[j] = this.currentAffinityData.ProductAffinities[j].Affinity;
			}
			return new CustomerData(this.CurrentAddiction, array, array2, this.TimeSinceLastDealCompleted, this.TimeSinceLastDealOffered, this.OfferedDeals, this.CompletedDeliveries, this.OfferedContractInfo != null, this.OfferedContractInfo, this.OfferedContractTime, this.TimeSincePlayerApproached, this.TimeSinceInstantDealOffered, this.HasBeenRecommended);
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000594B4 File Offset: 0x000576B4
		public virtual List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000BCAE6 File Offset: 0x000BACE6
		[TargetRpc]
		private void ReceiveCustomerData(NetworkConnection conn, CustomerData data)
		{
			this.RpcWriter___Target_ReceiveCustomerData_2280244125(conn, data);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000BCAF8 File Offset: 0x000BACF8
		public virtual void Load(CustomerData data)
		{
			this.CurrentAddiction = data.Dependence;
			for (int i = 0; i < this.currentAffinityData.ProductAffinities.Count; i++)
			{
				if (i >= this.currentAffinityData.ProductAffinities.Count)
				{
					Console.LogWarning("Product affinities array is too short", null);
					break;
				}
				if (data.ProductAffinities.Length <= i || float.IsNaN(data.ProductAffinities[i]))
				{
					Console.LogWarning("Product affinity is NaN", null);
				}
				else
				{
					this.currentAffinityData.ProductAffinities[i].Affinity = data.ProductAffinities[i];
				}
			}
			this.TimeSinceLastDealCompleted = data.TimeSinceLastDealCompleted;
			this.TimeSinceLastDealOffered = data.TimeSinceLastDealOffered;
			this.OfferedDeals = data.OfferedDeals;
			this.CompletedDeliveries = data.CompletedDeals;
			int timeSincePlayerApproached = data.TimeSincePlayerApproached;
			this.TimeSincePlayerApproached = data.TimeSincePlayerApproached;
			int timeSinceInstantDealOffered = data.TimeSinceInstantDealOffered;
			this.TimeSinceInstantDealOffered = data.TimeSinceInstantDealOffered;
			bool hasBeenRecommended = data.HasBeenRecommended;
			this.HasBeenRecommended = data.HasBeenRecommended;
			if (data.IsContractOffered && data.OfferedContract != null)
			{
				this.OfferedContractInfo = data.OfferedContract;
				this.OfferedContractTime = data.OfferedContractTime;
				this.SetUpResponseCallbacks();
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000BCC28 File Offset: 0x000BAE28
		protected virtual bool IsReadyForHandover(bool enabled)
		{
			return (Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None || Player.Local.CrimeData.TimeSinceSighted >= 5f) && enabled && this.IsAwaitingDelivery;
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000BCC60 File Offset: 0x000BAE60
		protected virtual bool IsHandoverChoiceValid(out string invalidReason)
		{
			invalidReason = string.Empty;
			if (this.CurrentContract == null)
			{
				return false;
			}
			if (this.AssignedDealer != null && (this.AssignedDealer.ActiveContracts.Contains(this.CurrentContract) || this.CurrentContract.Dealer != null))
			{
				invalidReason = "Customer is waiting for a dealer";
				return false;
			}
			return true;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000BCCC7 File Offset: 0x000BAEC7
		public void HandoverChosen()
		{
			this.NPC.dialogueHandler.SkipNextDialogueBehaviourEnd();
			Singleton<HandoverScreen>.Instance.Open(this.CurrentContract, this, HandoverScreen.EMode.Contract, delegate(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float price)
			{
				if (outcome == HandoverScreen.EHandoverOutcome.Finalize)
				{
					bool flag;
					this.OfferDealItems(items, true, out flag);
					return;
				}
				this.EndWait();
			}, null);
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000BCCF8 File Offset: 0x000BAEF8
		protected virtual bool ShowDirectApproachOption(bool enabled)
		{
			return (Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None || Player.Local.CrimeData.TimeSinceSighted >= 5f) && (enabled && this.customerData.CanBeDirectlyApproached && !this.IsAwaitingDelivery) && !this.NPC.RelationData.Unlocked;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000BCD5C File Offset: 0x000BAF5C
		public virtual bool IsUnlockable()
		{
			return !this.NPC.RelationData.Unlocked && (GameManager.IS_TUTORIAL || Singleton<Map>.Instance.GetRegionData(this.NPC.Region).IsUnlocked) && this.NPC.RelationData.IsMutuallyKnown();
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000BCDB8 File Offset: 0x000BAFB8
		protected virtual bool SampleOptionValid(out string invalidReason)
		{
			if (!GameManager.IS_TUTORIAL)
			{
				MapRegionData regionData = Singleton<Map>.Instance.GetRegionData(this.NPC.Region);
				if (!regionData.IsUnlocked)
				{
					invalidReason = "'" + regionData.Name + "' region must be unlocked";
					return false;
				}
			}
			if (!this.NPC.RelationData.IsMutuallyKnown())
			{
				invalidReason = "Unlock one of " + this.NPC.FirstName + "'s connections first";
				return false;
			}
			if (this.GetSampleRequestSuccessChance() == 0f)
			{
				invalidReason = "Mutual relationship too low";
				return false;
			}
			if (this.sampleOfferedToday)
			{
				invalidReason = "Sample already offered today";
				return false;
			}
			invalidReason = string.Empty;
			return true;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000BCE64 File Offset: 0x000BB064
		public bool KnownAndRecommended()
		{
			return (GameManager.IS_TUTORIAL || Singleton<Map>.Instance.GetRegionData(this.NPC.Region).IsUnlocked) && this.HasBeenRecommended && this.NPC.RelationData.IsMutuallyKnown();
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000BCEB8 File Offset: 0x000BB0B8
		public void SampleOffered()
		{
			if (this.awaitingSample)
			{
				this.SampleAccepted();
				return;
			}
			float sampleRequestSuccessChance = this.GetSampleRequestSuccessChance();
			if (UnityEngine.Random.Range(0f, 1f) <= sampleRequestSuccessChance)
			{
				this.SampleAccepted();
				return;
			}
			this.DirectApproachRejected();
			this.sampleOfferedToday = true;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000BCF04 File Offset: 0x000BB104
		protected virtual float GetSampleRequestSuccessChance()
		{
			if (this.NPC.RelationData.Unlocked)
			{
				return 1f;
			}
			if (this.NPC.RelationData.IsMutuallyKnown())
			{
				return 1f;
			}
			if (this.customerData.GuaranteeFirstSampleSuccess)
			{
				return 1f;
			}
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return 1f;
			}
			return Mathf.InverseLerp(this.customerData.MinMutualRelationRequirement, this.customerData.MaxMutualRelationRequirement, this.NPC.RelationData.GetAverageMutualRelationship());
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000BCF94 File Offset: 0x000BB194
		protected virtual void SampleAccepted()
		{
			this.awaitingSample = true;
			this.NPC.dialogueHandler.SkipNextDialogueBehaviourEnd();
			this.NPC.PlayVO(EVOLineType.Acknowledge);
			Singleton<HandoverScreen>.Instance.Open(null, this, HandoverScreen.EMode.Sample, new Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float>(this.ProcessSample), new Func<List<ItemInstance>, float, float>(this.GetSampleSuccess));
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000BCFEC File Offset: 0x000BB1EC
		private float GetSampleSuccess(List<ItemInstance> items, float price)
		{
			float num = -1000f;
			foreach (ItemInstance itemInstance in items)
			{
				if (itemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = itemInstance as ProductItemInstance;
					float productEnjoyment = this.GetProductEnjoyment(itemInstance.Definition as ProductDefinition, productItemInstance.Quality);
					if (productEnjoyment > num)
					{
						num = productEnjoyment;
					}
				}
			}
			float num2 = this.NPC.RelationData.RelationDelta / 5f;
			if (num2 >= 0.5f)
			{
				num += Mathf.Lerp(0f, 0.2f, (num2 - 0.5f) * 2f);
			}
			num += Mathf.Lerp(0f, 0.2f, this.CurrentAddiction);
			float num3 = this.NPC.RelationData.GetAverageMutualRelationship() / 5f;
			if (num3 > 0.5f)
			{
				num += Mathf.Lerp(0f, 0.2f, (num3 - 0.5f) * 2f);
			}
			num = Mathf.Clamp01(num);
			if (num <= 0f)
			{
				return 0f;
			}
			return NetworkSingleton<ProductManager>.Instance.SampleSuccessCurve.Evaluate(num);
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000BD128 File Offset: 0x000BB328
		private void ProcessSample(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float price)
		{
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				base.Invoke("EndWait", 1.5f);
				return;
			}
			Singleton<HandoverScreen>.Instance.ClearCustomerSlots(false);
			this.awaitingSample = false;
			this.ProcessSampleServerSide(items);
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000BD158 File Offset: 0x000BB358
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void ProcessSampleServerSide(List<ItemInstance> items)
		{
			this.RpcWriter___Server_ProcessSampleServerSide_3704012609(items);
			this.RpcLogic___ProcessSampleServerSide_3704012609(items);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000BD17C File Offset: 0x000BB37C
		[ObserversRpc(RunLocally = true)]
		private void ProcessSampleClient()
		{
			this.RpcWriter___Observers_ProcessSampleClient_2166136261();
			this.RpcLogic___ProcessSampleClient_2166136261();
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000BD198 File Offset: 0x000BB398
		private void SampleConsumed()
		{
			this.NPC.behaviour.ConsumeProductBehaviour.onConsumeDone.RemoveListener(new UnityAction(this.SampleConsumed));
			this.NPC.behaviour.GenericDialogueBehaviour.SendEnable();
			if (this.consumedSample == null)
			{
				Console.LogWarning("Consumed sample is null", null);
				return;
			}
			float sampleSuccess = this.GetSampleSuccess(new List<ItemInstance>
			{
				this.consumedSample
			}, 0f);
			if (UnityEngine.Random.Range(0f, 1f) <= sampleSuccess || NetworkSingleton<GameManager>.Instance.IsTutorial || this.customerData.GuaranteeFirstSampleSuccess)
			{
				NetworkSingleton<LevelManager>.Instance.AddXP(50);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("SuccessfulSampleCount", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("SuccessfulSampleCount") + 1f).ToString(), true);
				this.SampleWasSufficient();
			}
			else
			{
				this.SampleWasInsufficient();
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("SampleRejectionCount");
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("SampleRejectionCount", (value + 1f).ToString(), true);
			}
			this.consumedSample = null;
			base.Invoke("EndWait", 1.5f);
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000BD2C9 File Offset: 0x000BB4C9
		private void EndWait()
		{
			if (this.NPC.dialogueHandler.IsPlaying)
			{
				return;
			}
			if (Singleton<HandoverScreen>.Instance.CurrentCustomer == this)
			{
				return;
			}
			this.NPC.behaviour.GenericDialogueBehaviour.SendDisable();
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000BD308 File Offset: 0x000BB508
		protected virtual void DirectApproachRejected()
		{
			if (UnityEngine.Random.Range(0f, 1f) <= this.customerData.CallPoliceChance)
			{
				this.NPC.PlayVO(EVOLineType.Angry);
				this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "sample_offer_rejected_police"), 5f);
				this.NPC.actions.SetCallPoliceBehaviourCrime(new AttemptingToSell());
				this.NPC.actions.CallPolice_Networked(Player.Local);
				return;
			}
			this.NPC.PlayVO(EVOLineType.Annoyed);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "sample_offer_rejected"), 5f);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000BD3C4 File Offset: 0x000BB5C4
		[ObserversRpc]
		private void SampleWasSufficient()
		{
			this.RpcWriter___Observers_SampleWasSufficient_2166136261();
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000BD3D8 File Offset: 0x000BB5D8
		[ObserversRpc]
		private void SampleWasInsufficient()
		{
			this.RpcWriter___Observers_SampleWasInsufficient_2166136261();
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000BD3EC File Offset: 0x000BB5EC
		public float GetProductEnjoyment(ProductDefinition product, EQuality quality)
		{
			float num = 0f;
			for (int j = 0; j < product.DrugTypes.Count; j++)
			{
				num += this.currentAffinityData.GetAffinity(product.DrugTypes[j].DrugType) * 0.3f;
			}
			float num2 = 0f;
			int i;
			Predicate<Property> <>9__0;
			int i2;
			for (i = 0; i < this.customerData.PreferredProperties.Count; i = i2 + 1)
			{
				List<Property> properties = product.Properties;
				Predicate<Property> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Property x) => x == this.customerData.PreferredProperties[i]));
				}
				if (properties.Find(match) != null)
				{
					num2 += 1f / (float)this.customerData.PreferredProperties.Count;
				}
				i2 = i;
			}
			num += num2 * 0.4f;
			float qualityScalar = CustomerData.GetQualityScalar(quality);
			float qualityScalar2 = CustomerData.GetQualityScalar(this.customerData.Standards.GetCorrespondingQuality());
			float num3 = qualityScalar - qualityScalar2;
			float num4;
			if (num3 >= 0.25f)
			{
				num4 = 1f;
			}
			else if (num3 >= 0f)
			{
				num4 = 0.5f;
			}
			else if (num3 >= -0.25f)
			{
				num4 = -0.5f;
			}
			else
			{
				num4 = -1f;
			}
			num += num4 * 0.3f;
			float a = -0.6f;
			float b = 1f;
			return Mathf.InverseLerp(a, b, num);
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000BD564 File Offset: 0x000BB764
		public List<EDrugType> GetOrderedDrugTypes()
		{
			List<EDrugType> list = new List<EDrugType>();
			for (int i = 0; i < this.currentAffinityData.ProductAffinities.Count; i++)
			{
				list.Add(this.currentAffinityData.ProductAffinities[i].DrugType);
			}
			return (from x in list
			orderby this.currentAffinityData.ProductAffinities.Find((ProductTypeAffinity y) => y.DrugType == x).Affinity descending
			select x).ToList<EDrugType>();
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000BD5C8 File Offset: 0x000BB7C8
		[ServerRpc(RequireOwnership = false)]
		public void AdjustAffinity(EDrugType drugType, float change)
		{
			this.RpcWriter___Server_AdjustAffinity_3036964899(drugType, change);
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000BD5E3 File Offset: 0x000BB7E3
		[Button]
		public void AutocreateCustomerSettings()
		{
			if (this.customerData != null)
			{
				Console.LogWarning("Customer data already exists", null);
				return;
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000BD6F0 File Offset: 0x000BB8F0
		[CompilerGenerated]
		private void <Start>g__RegisterLoadEvent|133_0()
		{
			this.SetUpResponseCallbacks();
			MSGConversation msgconversation = this.NPC.MSGConversation;
			msgconversation.onLoaded = (Action)Delegate.Combine(msgconversation.onLoaded, new Action(this.SetUpResponseCallbacks));
			MSGConversation msgconversation2 = this.NPC.MSGConversation;
			msgconversation2.onResponsesShown = (Action)Delegate.Combine(msgconversation2.onResponsesShown, new Action(this.SetUpResponseCallbacks));
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000BD75C File Offset: 0x000BB95C
		[CompilerGenerated]
		private void <InstantDealOffered>g__HandoverClosed|191_0(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float askingPrice)
		{
			this.TimeSinceInstantDealOffered = 0;
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				this.EndWait();
				return;
			}
			this.pendingInstantDeal = false;
			float offerSuccessChance = this.GetOfferSuccessChance(items, askingPrice);
			if (UnityEngine.Random.value <= offerSuccessChance)
			{
				Singleton<HandoverScreen>.Instance.ClearCustomerSlots(false);
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
							Quality = this.CustomerData.Standards.GetCorrespondingQuality()
						});
					}
				}
				contract.SilentlyInitializeContract("Offer", string.Empty, null, string.Empty, base.NetworkObject, askingPrice, productList, string.Empty, new QuestWindowConfig(), 0, NetworkSingleton<TimeManager>.Instance.GetDateTime());
				this.ProcessHandover(HandoverScreen.EHandoverOutcome.Finalize, contract, items, true, false);
			}
			else
			{
				Singleton<HandoverScreen>.Instance.ClearCustomerSlots(true);
				this.NPC.dialogueHandler.ShowWorldspaceDialogue_5s(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "sample_insufficient"));
				this.NPC.PlayVO(EVOLineType.Annoyed);
			}
			base.Invoke("EndWait", 1.5f);
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000BD900 File Offset: 0x000BBB00
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Economy.CustomerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Economy.CustomerAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<HasBeenRecommended>k__BackingField = new SyncVar<bool>(this, 1U, 0, 0, -1f, 0, this.<HasBeenRecommended>k__BackingField);
			this.syncVar___<CurrentAddiction>k__BackingField = new SyncVar<float>(this, 0U, 0, 0, -1f, 0, this.<CurrentAddiction>k__BackingField);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_ConfigureDealSignal_338960014));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_ConfigureDealSignal_338960014));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_SetOfferedContract_4277245194));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_ExpireOffer_2166136261));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendSetUpResponseCallbacks_2166136261));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetUpResponseCallbacks_2166136261));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_ProcessCounterOfferServerSide_900355577));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_SetContractIsCounterOffer_2166136261));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendContractAccepted_507093020));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveContractAccepted_2166136261));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveContractRejected_2166136261));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_ProcessHandoverServerSide_3760244802));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ProcessHandoverClient_537707335));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_ChangeAddiction_431000436));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_RejectProductRequestOffer_2166136261));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_RejectProductRequestOffer_Local_2166136261));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveCustomerData_2280244125));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_ProcessSampleServerSide_3704012609));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_ProcessSampleClient_2166136261));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_SampleWasSufficient_2166136261));
			base.RegisterObserversRpc(20U, new ClientRpcDelegate(this.RpcReader___Observers_SampleWasInsufficient_2166136261));
			base.RegisterServerRpc(21U, new ServerRpcDelegate(this.RpcReader___Server_AdjustAffinity_3036964899));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Economy.Customer));
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000BDB80 File Offset: 0x000BBD80
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Economy.CustomerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Economy.CustomerAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<HasBeenRecommended>k__BackingField.SetRegistered();
			this.syncVar___<CurrentAddiction>k__BackingField.SetRegistered();
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000BDBA9 File Offset: 0x000BBDA9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000BDBB8 File Offset: 0x000BBDB8
		private void RpcWriter___Observers_ConfigureDealSignal_338960014(NetworkConnection conn, int startTime, bool active)
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
			writer.WriteInt32(startTime, 1);
			writer.WriteBoolean(active);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000BDC80 File Offset: 0x000BBE80
		private void RpcLogic___ConfigureDealSignal_338960014(NetworkConnection conn, int startTime, bool active)
		{
			this.DealSignal.SetStartTime(startTime);
			this.DealSignal.gameObject.SetActive(active);
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000BDCA0 File Offset: 0x000BBEA0
		private void RpcReader___Observers_ConfigureDealSignal_338960014(PooledReader PooledReader0, Channel channel)
		{
			int startTime = PooledReader0.ReadInt32(1);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ConfigureDealSignal_338960014(null, startTime, active);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000BDCF4 File Offset: 0x000BBEF4
		private void RpcWriter___Target_ConfigureDealSignal_338960014(NetworkConnection conn, int startTime, bool active)
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
			writer.WriteInt32(startTime, 1);
			writer.WriteBoolean(active);
			base.SendTargetRpc(1U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000BDDBC File Offset: 0x000BBFBC
		private void RpcReader___Target_ConfigureDealSignal_338960014(PooledReader PooledReader0, Channel channel)
		{
			int startTime = PooledReader0.ReadInt32(1);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ConfigureDealSignal_338960014(base.LocalConnection, startTime, active);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000BDE0C File Offset: 0x000BC00C
		private void RpcWriter___Observers_SetOfferedContract_4277245194(ContractInfo info, GameDateTime offerTime)
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
			writer.Write___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generated(info);
			writer.Write___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generated(offerTime);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000BDECF File Offset: 0x000BC0CF
		private void RpcLogic___SetOfferedContract_4277245194(ContractInfo info, GameDateTime offerTime)
		{
			this.OfferedContractInfo = info;
			this.OfferedContractTime = offerTime;
			this.TimeSinceLastDealOffered = 0;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000BDEE8 File Offset: 0x000BC0E8
		private void RpcReader___Observers_SetOfferedContract_4277245194(PooledReader PooledReader0, Channel channel)
		{
			ContractInfo info = GeneratedReaders___Internal.Read___ScheduleOne.Quests.ContractInfoFishNet.Serializing.Generateds(PooledReader0);
			GameDateTime offerTime = GeneratedReaders___Internal.Read___ScheduleOne.GameTime.GameDateTimeFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetOfferedContract_4277245194(info, offerTime);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000BDF2C File Offset: 0x000BC12C
		private void RpcWriter___Server_ExpireOffer_2166136261()
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

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000BDFC8 File Offset: 0x000BC1C8
		public virtual void RpcLogic___ExpireOffer_2166136261()
		{
			if (this.OfferedContractInfo == null)
			{
				return;
			}
			this.NPC.MSGConversation.SendMessageChain(this.NPC.dialogueHandler.Database.GetChain(EDialogueModule.Customer, "offer_expired").GetMessageChain(), 0f, true, true);
			this.NPC.MSGConversation.ClearResponses(true);
			this.OfferedContractInfo = null;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000BE030 File Offset: 0x000BC230
		private void RpcReader___Server_ExpireOffer_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ExpireOffer_2166136261();
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000BE060 File Offset: 0x000BC260
		private void RpcWriter___Server_SendSetUpResponseCallbacks_2166136261()
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
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000BE0FA File Offset: 0x000BC2FA
		private void RpcLogic___SendSetUpResponseCallbacks_2166136261()
		{
			this.SetUpResponseCallbacks();
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000BE104 File Offset: 0x000BC304
		private void RpcReader___Server_SendSetUpResponseCallbacks_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendSetUpResponseCallbacks_2166136261();
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000BE134 File Offset: 0x000BC334
		private void RpcWriter___Observers_SetUpResponseCallbacks_2166136261()
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
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000BE1E0 File Offset: 0x000BC3E0
		private void RpcLogic___SetUpResponseCallbacks_2166136261()
		{
			if (this.NPC.MSGConversation == null)
			{
				return;
			}
			for (int i = 0; i < this.NPC.MSGConversation.currentResponses.Count; i++)
			{
				if (this.NPC.MSGConversation.currentResponses[i].label == "ACCEPT_CONTRACT")
				{
					this.NPC.MSGConversation.currentResponses[i].disableDefaultResponseBehaviour = true;
					this.NPC.MSGConversation.currentResponses[i].callback = new Action(this.AcceptContractClicked);
				}
				else if (this.NPC.MSGConversation.currentResponses[i].label == "REJECT_CONTRACT")
				{
					this.NPC.MSGConversation.currentResponses[i].callback = new Action(this.ContractRejected);
				}
				else if (this.NPC.MSGConversation.currentResponses[i].label == "COUNTEROFFER")
				{
					this.NPC.MSGConversation.currentResponses[i].callback = new Action(this.CounterOfferClicked);
					this.NPC.MSGConversation.currentResponses[i].disableDefaultResponseBehaviour = true;
				}
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000BE350 File Offset: 0x000BC550
		private void RpcReader___Observers_SetUpResponseCallbacks_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetUpResponseCallbacks_2166136261();
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000BE37C File Offset: 0x000BC57C
		private void RpcWriter___Server_ProcessCounterOfferServerSide_900355577(string productID, int quantity, float price)
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
			writer.WriteString(productID);
			writer.WriteInt32(quantity, 1);
			writer.WriteSingle(price, 0);
			base.SendServerRpc(6U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000BE448 File Offset: 0x000BC648
		private void RpcLogic___ProcessCounterOfferServerSide_900355577(string productID, int quantity, float price)
		{
			ProductDefinition item = Registry.GetItem<ProductDefinition>(productID);
			if (item == null)
			{
				Console.LogError("Product is null!", null);
				return;
			}
			if (this.EvaluateCounteroffer(item, quantity, price))
			{
				NetworkSingleton<LevelManager>.Instance.AddXP(5);
				DialogueChain chain = this.dialogueDatabase.GetChain(EDialogueModule.Customer, "counteroffer_accepted");
				this.NPC.MSGConversation.SendMessageChain(chain.GetMessageChain(), 1f, false, true);
				this.OfferedContractInfo.Payment = price;
				this.OfferedContractInfo.Products.entries[0].ProductID = item.ID;
				this.OfferedContractInfo.Products.entries[0].Quantity = quantity;
				this.SetContractIsCounterOffer();
				List<Response> list = new List<Response>();
				list.Add(new Response("[Schedule Deal]", "ACCEPT_CONTRACT", new Action(this.AcceptContractClicked), true));
				list.Add(new Response("Nevermind", "REJECT_CONTRACT", new Action(this.ContractRejected), false));
				this.NPC.MSGConversation.ShowResponses(list, 1f, true);
			}
			else
			{
				DialogueChain chain2 = this.dialogueDatabase.GetChain(EDialogueModule.Customer, "counteroffer_rejected");
				this.NPC.MSGConversation.SendMessageChain(chain2.GetMessageChain(), 0.8f, false, true);
				this.OfferedContractInfo = null;
				this.NPC.MSGConversation.ClearResponses(true);
			}
			this.HasChanged = true;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000BE5BC File Offset: 0x000BC7BC
		private void RpcReader___Server_ProcessCounterOfferServerSide_900355577(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(1);
			float price = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ProcessCounterOfferServerSide_900355577(productID, quantity, price);
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000BE61C File Offset: 0x000BC81C
		private void RpcWriter___Observers_SetContractIsCounterOffer_2166136261()
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
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000BE6C5 File Offset: 0x000BC8C5
		private void RpcLogic___SetContractIsCounterOffer_2166136261()
		{
			if (this.OfferedContractInfo != null)
			{
				this.OfferedContractInfo.IsCounterOffer = true;
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000BE6DC File Offset: 0x000BC8DC
		private void RpcReader___Observers_SetContractIsCounterOffer_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetContractIsCounterOffer_2166136261();
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000BE708 File Offset: 0x000BC908
		private void RpcWriter___Server_SendContractAccepted_507093020(EDealWindow window, bool trackContract)
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
			writer.Write___ScheduleOne.Economy.EDealWindowFishNet.Serializing.Generated(window);
			writer.WriteBoolean(trackContract);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000BE7BC File Offset: 0x000BC9BC
		private void RpcLogic___SendContractAccepted_507093020(EDealWindow window, bool trackContract)
		{
			this.ContractAccepted(window, trackContract);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000BE7C8 File Offset: 0x000BC9C8
		private void RpcReader___Server_SendContractAccepted_507093020(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			EDealWindow window = GeneratedReaders___Internal.Read___ScheduleOne.Economy.EDealWindowFishNet.Serializing.Generateds(PooledReader0);
			bool trackContract = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendContractAccepted_507093020(window, trackContract);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000BE80C File Offset: 0x000BCA0C
		private void RpcWriter___Observers_ReceiveContractAccepted_2166136261()
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

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000BE8B5 File Offset: 0x000BCAB5
		private void RpcLogic___ReceiveContractAccepted_2166136261()
		{
			this.OfferedContractInfo = null;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000BE8C0 File Offset: 0x000BCAC0
		private void RpcReader___Observers_ReceiveContractAccepted_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveContractAccepted_2166136261();
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000BE8EC File Offset: 0x000BCAEC
		private void RpcWriter___Observers_ReceiveContractRejected_2166136261()
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
			base.SendObserversRpc(10U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000BE8B5 File Offset: 0x000BCAB5
		private void RpcLogic___ReceiveContractRejected_2166136261()
		{
			this.OfferedContractInfo = null;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000BE998 File Offset: 0x000BCB98
		private void RpcReader___Observers_ReceiveContractRejected_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveContractRejected_2166136261();
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000BE9C4 File Offset: 0x000BCBC4
		private void RpcWriter___Server_ProcessHandoverServerSide_3760244802(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, bool handoverByPlayer, float totalPayment, ProductList productList, float satisfaction, NetworkObject dealer)
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
			writer.Write___ScheduleOne.UI.Handover.HandoverScreen/EHandoverOutcomeFishNet.Serializing.Generated(outcome);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generated(items);
			writer.WriteBoolean(handoverByPlayer);
			writer.WriteSingle(totalPayment, 0);
			writer.Write___ScheduleOne.Product.ProductListFishNet.Serializing.Generated(productList);
			writer.WriteSingle(satisfaction, 0);
			writer.WriteNetworkObject(dealer);
			base.SendServerRpc(11U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000BEAC4 File Offset: 0x000BCCC4
		private void RpcLogic___ProcessHandoverServerSide_3760244802(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, bool handoverByPlayer, float totalPayment, ProductList productList, float satisfaction, NetworkObject dealer)
		{
			int completedDeliveries = this.CompletedDeliveries;
			this.CompletedDeliveries = completedDeliveries + 1;
			base.Invoke("EndWait", 1.5f);
			if (handoverByPlayer)
			{
				List<string> list = new List<string>();
				List<int> list2 = new List<int>();
				foreach (ProductList.Entry entry in productList.entries)
				{
					list.Add(entry.ProductID);
					list2.Add(entry.Quantity);
				}
				for (int i = 0; i < list.Count; i++)
				{
					NetworkSingleton<DailySummary>.Instance.AddSoldItem(list[i], list2[i]);
				}
				NetworkSingleton<DailySummary>.Instance.AddPlayerMoney(totalPayment);
				NetworkSingleton<LevelManager>.Instance.AddXP(20);
			}
			else
			{
				NetworkSingleton<LevelManager>.Instance.AddXP(10);
				NetworkSingleton<DailySummary>.Instance.AddDealerMoney(totalPayment);
				if (dealer != null)
				{
					dealer.GetComponent<Dealer>().CompletedDeal();
					dealer.GetComponent<Dealer>().SubmitPayment(totalPayment);
				}
			}
			NetworkSingleton<MoneyManager>.Instance.ChangeLifetimeEarnings(totalPayment);
			if (this.CurrentContract != null)
			{
				this.CurrentContract.Complete(true);
			}
			foreach (ItemInstance item in items)
			{
				this.NPC.Inventory.InsertItem(item, true);
			}
			if (items.Count > 0)
			{
				this.ConsumeProduct(items[0]);
			}
			if (this.NPC.RelationData.NormalizedRelationDelta >= 0.5f)
			{
				Mathf.Lerp(0.33f, 1f, Mathf.InverseLerp(0.5f, 1f, this.NPC.RelationData.NormalizedRelationDelta));
			}
			NPC npc = null;
			if (this.NPC.RelationData.NormalizedRelationDelta >= 0.6f)
			{
				npc = this.NPC.RelationData.GetLockedDealers(true).FirstOrDefault<NPC>();
			}
			NPC npc2 = null;
			if (this.NPC.RelationData.NormalizedRelationDelta >= 0.6f)
			{
				npc2 = this.NPC.RelationData.GetLockedSuppliers().FirstOrDefault<NPC>();
			}
			string npcToRecommend = string.Empty;
			if (GameManager.IS_TUTORIAL && NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Completed_Contracts_Count") >= 2.9f)
			{
				npcToRecommend = "chelsey_milson";
			}
			else if (npc2 != null)
			{
				npcToRecommend = npc2.ID;
			}
			else if (npc != null)
			{
				npcToRecommend = npc.ID;
			}
			this.ProcessHandoverClient(satisfaction, handoverByPlayer, npcToRecommend);
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000BED70 File Offset: 0x000BCF70
		private void RpcReader___Server_ProcessHandoverServerSide_3760244802(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			HandoverScreen.EHandoverOutcome outcome = GeneratedReaders___Internal.Read___ScheduleOne.UI.Handover.HandoverScreen/EHandoverOutcomeFishNet.Serializing.Generateds(PooledReader0);
			List<ItemInstance> items = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generateds(PooledReader0);
			bool handoverByPlayer = PooledReader0.ReadBoolean();
			float totalPayment = PooledReader0.ReadSingle(0);
			ProductList productList = GeneratedReaders___Internal.Read___ScheduleOne.Product.ProductListFishNet.Serializing.Generateds(PooledReader0);
			float satisfaction = PooledReader0.ReadSingle(0);
			NetworkObject dealer = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ProcessHandoverServerSide_3760244802(outcome, items, handoverByPlayer, totalPayment, productList, satisfaction, dealer);
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x000BEE14 File Offset: 0x000BD014
		private void RpcWriter___Observers_ProcessHandoverClient_537707335(float satisfaction, bool handoverByPlayer, string npcToRecommend)
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
			writer.WriteSingle(satisfaction, 0);
			writer.WriteBoolean(handoverByPlayer);
			writer.WriteString(npcToRecommend);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000BEEEC File Offset: 0x000BD0EC
		private void RpcLogic___ProcessHandoverClient_537707335(float satisfaction, bool handoverByPlayer, string npcToRecommend)
		{
			this.TimeSinceLastDealCompleted = 0;
			if (satisfaction >= 0.5f)
			{
				this.ContractWellReceived(npcToRecommend);
			}
			else if (satisfaction < 0.3f)
			{
				this.NPC.PlayVO(EVOLineType.Annoyed);
			}
			if (this.onDealCompleted != null)
			{
				this.onDealCompleted.Invoke();
			}
			this.CurrentContract = null;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000BEF40 File Offset: 0x000BD140
		private void RpcReader___Observers_ProcessHandoverClient_537707335(PooledReader PooledReader0, Channel channel)
		{
			float satisfaction = PooledReader0.ReadSingle(0);
			bool handoverByPlayer = PooledReader0.ReadBoolean();
			string npcToRecommend = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ProcessHandoverClient_537707335(satisfaction, handoverByPlayer, npcToRecommend);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000BEF98 File Offset: 0x000BD198
		private void RpcWriter___Server_ChangeAddiction_431000436(float change)
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
			writer.WriteSingle(change, 0);
			base.SendServerRpc(13U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000BF044 File Offset: 0x000BD244
		public void RpcLogic___ChangeAddiction_431000436(float change)
		{
			this.CurrentAddiction = Mathf.Clamp(this.CurrentAddiction + change, this.customerData.BaseAddiction, 1f);
			this.HasChanged = true;
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000BF070 File Offset: 0x000BD270
		private void RpcReader___Server_ChangeAddiction_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float change = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ChangeAddiction_431000436(change);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000BF0A8 File Offset: 0x000BD2A8
		private void RpcWriter___Server_RejectProductRequestOffer_2166136261()
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
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000BF144 File Offset: 0x000BD344
		public void RpcLogic___RejectProductRequestOffer_2166136261()
		{
			this.RejectProductRequestOffer_Local();
			if (this.NPC.responses is NPCResponses_Civilian && this.NPC.Aggression > 0.1f)
			{
				float num = Mathf.Clamp(this.NPC.Aggression, 0f, 0.7f);
				num -= this.NPC.RelationData.NormalizedRelationDelta * 0.3f;
				num += this.CurrentAddiction * 0.2f;
				if (UnityEngine.Random.Range(0f, 1f) < num)
				{
					float num2;
					this.NPC.behaviour.CombatBehaviour.SetTarget(null, Player.GetClosestPlayer(base.transform.position, out num2, null).NetworkObject);
					this.NPC.behaviour.CombatBehaviour.Enable_Networked(null);
				}
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000BF21C File Offset: 0x000BD41C
		private void RpcReader___Server_RejectProductRequestOffer_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RejectProductRequestOffer_2166136261();
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000BF23C File Offset: 0x000BD43C
		private void RpcWriter___Observers_RejectProductRequestOffer_Local_2166136261()
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

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000BF2E8 File Offset: 0x000BD4E8
		private void RpcLogic___RejectProductRequestOffer_Local_2166136261()
		{
			this.NPC.PlayVO(EVOLineType.Annoyed);
			this.NPC.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "product_request_fail", 30f, 1);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "counteroffer_rejected"), 5f);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000BF350 File Offset: 0x000BD550
		private void RpcReader___Observers_RejectProductRequestOffer_Local_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RejectProductRequestOffer_Local_2166136261();
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000BF37C File Offset: 0x000BD57C
		private void RpcWriter___Target_ReceiveCustomerData_2280244125(NetworkConnection conn, CustomerData data)
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
			writer.Write___ScheduleOne.Persistence.Datas.CustomerDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(16U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000BF431 File Offset: 0x000BD631
		private void RpcLogic___ReceiveCustomerData_2280244125(NetworkConnection conn, CustomerData data)
		{
			this.Load(data);
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000BF43C File Offset: 0x000BD63C
		private void RpcReader___Target_ReceiveCustomerData_2280244125(PooledReader PooledReader0, Channel channel)
		{
			CustomerData data = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.CustomerDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveCustomerData_2280244125(base.LocalConnection, data);
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000BF474 File Offset: 0x000BD674
		private void RpcWriter___Server_ProcessSampleServerSide_3704012609(List<ItemInstance> items)
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
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generated(items);
			base.SendServerRpc(17U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000BF51C File Offset: 0x000BD71C
		private void RpcLogic___ProcessSampleServerSide_3704012609(List<ItemInstance> items)
		{
			this.consumedSample = (items[0] as ProductItemInstance);
			this.NPC.behaviour.ConsumeProductBehaviour.onConsumeDone.AddListener(new UnityAction(this.SampleConsumed));
			this.NPC.behaviour.ConsumeProduct(this.consumedSample);
			this.ProcessSampleClient();
			this.EndWait();
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000BF584 File Offset: 0x000BD784
		private void RpcReader___Server_ProcessSampleServerSide_3704012609(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			List<ItemInstance> items = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.ItemFramework.ItemInstance>FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ProcessSampleServerSide_3704012609(items);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000BF5C4 File Offset: 0x000BD7C4
		private void RpcWriter___Observers_ProcessSampleClient_2166136261()
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
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000BF670 File Offset: 0x000BD870
		private void RpcLogic___ProcessSampleClient_2166136261()
		{
			if (this.NPC.behaviour.ConsumeProductBehaviour.Enabled)
			{
				return;
			}
			if (this.sampleOfferedToday)
			{
				return;
			}
			this.sampleOfferedToday = true;
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "sample_consume_wait"), 5f);
			this.NPC.SetAnimationTrigger("GrabItem");
			this.NPC.PlayVO(EVOLineType.Think);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000BF6E8 File Offset: 0x000BD8E8
		private void RpcReader___Observers_ProcessSampleClient_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ProcessSampleClient_2166136261();
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000BF714 File Offset: 0x000BD914
		private void RpcWriter___Observers_SampleWasSufficient_2166136261()
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

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000BF7C0 File Offset: 0x000BD9C0
		private void RpcLogic___SampleWasSufficient_2166136261()
		{
			this.NPC.PlayVO(EVOLineType.Thanks);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "sample_sufficient"), 5f);
			this.NPC.Avatar.EmotionManager.AddEmotionOverride("Cheery", "sample_provided", 10f, 0);
			if (!this.NPC.RelationData.Unlocked)
			{
				this.NPC.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, true);
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000BF848 File Offset: 0x000BDA48
		private void RpcReader___Observers_SampleWasSufficient_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SampleWasSufficient_2166136261();
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000BF868 File Offset: 0x000BDA68
		private void RpcWriter___Observers_SampleWasInsufficient_2166136261()
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

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000BF914 File Offset: 0x000BDB14
		private void RpcLogic___SampleWasInsufficient_2166136261()
		{
			this.NPC.PlayVO(EVOLineType.Annoyed);
			this.NPC.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Customer, "sample_insufficient"), 5f);
			this.NPC.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "sample_insufficient", 5f, 0);
			if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("SampleRejectionCount") < 1f && NetworkSingleton<ProductManager>.Instance.onFirstSampleRejection != null)
			{
				NetworkSingleton<ProductManager>.Instance.onFirstSampleRejection.Invoke();
			}
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000BF9AC File Offset: 0x000BDBAC
		private void RpcReader___Observers_SampleWasInsufficient_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SampleWasInsufficient_2166136261();
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000BF9CC File Offset: 0x000BDBCC
		private void RpcWriter___Server_AdjustAffinity_3036964899(EDrugType drugType, float change)
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
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(drugType);
			writer.WriteSingle(change, 0);
			base.SendServerRpc(21U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000BFA88 File Offset: 0x000BDC88
		public void RpcLogic___AdjustAffinity_3036964899(EDrugType drugType, float change)
		{
			ProductTypeAffinity productTypeAffinity = this.currentAffinityData.ProductAffinities.Find((ProductTypeAffinity x) => x.DrugType == drugType);
			productTypeAffinity.Affinity = Mathf.Clamp(productTypeAffinity.Affinity + change, -1f, 1f);
			this.HasChanged = true;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000BFAE4 File Offset: 0x000BDCE4
		private void RpcReader___Server_AdjustAffinity_3036964899(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			EDrugType drugType = GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			float change = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___AdjustAffinity_3036964899(drugType, change);
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002DDA RID: 11738 RVA: 0x000BFB2B File Offset: 0x000BDD2B
		// (set) Token: 0x06002DDB RID: 11739 RVA: 0x000BFB33 File Offset: 0x000BDD33
		public float SyncAccessor_<CurrentAddiction>k__BackingField
		{
			get
			{
				return this.<CurrentAddiction>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentAddiction>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentAddiction>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000BFB70 File Offset: 0x000BDD70
		public override bool Customer(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<HasBeenRecommended>k__BackingField(this.syncVar___<HasBeenRecommended>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_<HasBeenRecommended>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentAddiction>k__BackingField(this.syncVar___<CurrentAddiction>k__BackingField.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(0);
				this.sync___set_value_<CurrentAddiction>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002DDD RID: 11741 RVA: 0x000BFC0B File Offset: 0x000BDE0B
		// (set) Token: 0x06002DDE RID: 11742 RVA: 0x000BFC13 File Offset: 0x000BDE13
		public bool SyncAccessor_<HasBeenRecommended>k__BackingField
		{
			get
			{
				return this.<HasBeenRecommended>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<HasBeenRecommended>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<HasBeenRecommended>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000BFC50 File Offset: 0x000BDE50
		protected virtual void dll()
		{
			bool availableInDemo = this.AvailableInDemo;
			this.NPC = base.GetComponent<NPC>();
			this.CurrentAddiction = this.customerData.BaseAddiction;
			CustomerData customerData = this.customerData;
			customerData.onChanged = (Action)Delegate.Combine(customerData.onChanged, new Action(delegate()
			{
				this.HasChanged = true;
			}));
			this.currentAffinityData = new CustomerAffinityData();
			this.customerData.DefaultAffinityData.CopyTo(this.currentAffinityData);
			this.NPC.ConversationCategories.Add(EConversationCategory.Customer);
			this.InitializeSaveable();
		}

		// Token: 0x0400201A RID: 8218
		public static Action<Customer> onCustomerUnlocked;

		// Token: 0x0400201B RID: 8219
		public static List<Customer> UnlockedCustomers = new List<Customer>();

		// Token: 0x0400201C RID: 8220
		public const float AFFINITY_MAX_EFFECT = 0.3f;

		// Token: 0x0400201D RID: 8221
		public const float PROPERTY_MAX_EFFECT = 0.4f;

		// Token: 0x0400201E RID: 8222
		public const float QUALITY_MAX_EFFECT = 0.3f;

		// Token: 0x0400201F RID: 8223
		public const float DEAL_REJECTED_RELATIONSHIP_CHANGE = -0.5f;

		// Token: 0x04002020 RID: 8224
		public bool DEBUG;

		// Token: 0x04002021 RID: 8225
		public const float APPROACH_MIN_ADDICTION = 0.33f;

		// Token: 0x04002022 RID: 8226
		public const float APPROACH_CHANCE_PER_DAY_MAX = 0.5f;

		// Token: 0x04002023 RID: 8227
		public const float APPROACH_MIN_COOLDOWN = 2160f;

		// Token: 0x04002024 RID: 8228
		public const float APPROACH_MAX_COOLDOWN = 4320f;

		// Token: 0x04002025 RID: 8229
		public const int DEAL_COOLDOWN = 600;

		// Token: 0x04002026 RID: 8230
		public static string[] PlayerAcceptMessages = new string[]
		{
			"Yes",
			"Sure thing",
			"Yep",
			"Deal",
			"Alright"
		};

		// Token: 0x04002027 RID: 8231
		public static string[] PlayerRejectMessages = new string[]
		{
			"No",
			"Not right now",
			"No, sorry"
		};

		// Token: 0x04002028 RID: 8232
		public const int DEAL_ATTENDANCE_TOLERANCE = 10;

		// Token: 0x04002029 RID: 8233
		public const int MIN_TRAVEL_TIME = 15;

		// Token: 0x0400202A RID: 8234
		public const int MAX_TRAVEL_TIME = 360;

		// Token: 0x0400202B RID: 8235
		public const int OFFER_EXPIRY_TIME_MINS = 600;

		// Token: 0x0400202C RID: 8236
		public const float MIN_ORDER_APPEAL = 0.05f;

		// Token: 0x0400202D RID: 8237
		public const float ADDICTION_DRAIN_PER_DAY = 0.0625f;

		// Token: 0x0400202E RID: 8238
		public const bool SAMPLE_REQUIRES_RECOMMENDATION = false;

		// Token: 0x0400202F RID: 8239
		public const float MIN_NORMALIZED_RELATIONSHIP_FOR_RECOMMENDATION = 0.5f;

		// Token: 0x04002030 RID: 8240
		public const float RELATIONSHIP_FOR_GUARANTEED_DEALER_RECOMMENDATION = 0.6f;

		// Token: 0x04002031 RID: 8241
		public const float RELATIONSHIP_FOR_GUARANTEED_SUPPLIER_RECOMMENDATION = 0.6f;

		// Token: 0x04002033 RID: 8243
		private ContractInfo offeredContractInfo;

		// Token: 0x04002040 RID: 8256
		public NPCSignal_WaitForDelivery DealSignal;

		// Token: 0x04002041 RID: 8257
		[Header("Settings")]
		public bool AvailableInDemo = true;

		// Token: 0x04002042 RID: 8258
		[SerializeField]
		protected CustomerData customerData;

		// Token: 0x04002043 RID: 8259
		public DeliveryLocation DefaultDeliveryLocation;

		// Token: 0x04002044 RID: 8260
		public bool CanRecommendFriends = true;

		// Token: 0x04002045 RID: 8261
		[Header("Events")]
		public UnityEvent onUnlocked;

		// Token: 0x04002046 RID: 8262
		public UnityEvent onDealCompleted;

		// Token: 0x04002047 RID: 8263
		public UnityEvent<Contract> onContractAssigned;

		// Token: 0x04002048 RID: 8264
		private bool awaitingSample;

		// Token: 0x04002049 RID: 8265
		private DialogueController.DialogueChoice sampleChoice;

		// Token: 0x0400204A RID: 8266
		private DialogueController.DialogueChoice completeContractChoice;

		// Token: 0x0400204B RID: 8267
		private DialogueController.DialogueChoice offerDealChoice;

		// Token: 0x0400204C RID: 8268
		private DialogueController.GreetingOverride awaitingDealGreeting;

		// Token: 0x0400204D RID: 8269
		private int minsSinceUnlocked = 10000;

		// Token: 0x0400204E RID: 8270
		private bool sampleOfferedToday;

		// Token: 0x04002050 RID: 8272
		private CustomerAffinityData currentAffinityData;

		// Token: 0x04002051 RID: 8273
		private bool pendingInstantDeal;

		// Token: 0x04002055 RID: 8277
		private ProductItemInstance consumedSample;

		// Token: 0x04002056 RID: 8278
		public SyncVar<float> syncVar___<CurrentAddiction>k__BackingField;

		// Token: 0x04002057 RID: 8279
		public SyncVar<bool> syncVar___<HasBeenRecommended>k__BackingField;

		// Token: 0x04002058 RID: 8280
		private bool dll_Excuted;

		// Token: 0x04002059 RID: 8281
		private bool dll_Excuted;

		// Token: 0x0200068D RID: 1677
		[Serializable]
		public class ScheduleGroupPair
		{
			// Token: 0x0400205A RID: 8282
			public GameObject NormalScheduleGroup;

			// Token: 0x0400205B RID: 8283
			public GameObject CurfewScheduleGroup;
		}

		// Token: 0x0200068E RID: 1678
		[Serializable]
		public class CustomerPreference
		{
			// Token: 0x0400205C RID: 8284
			public EDrugType DrugType;

			// Token: 0x0400205D RID: 8285
			[Header("Optionally, a specific product")]
			public ProductDefinition Definition;

			// Token: 0x0400205E RID: 8286
			public EQuality MinimumQuality;
		}

		// Token: 0x0200068F RID: 1679
		public enum ESampleFeedback
		{
			// Token: 0x04002060 RID: 8288
			WrongProduct,
			// Token: 0x04002061 RID: 8289
			WrongQuality,
			// Token: 0x04002062 RID: 8290
			Correct
		}
	}
}
