using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ScheduleOne.Map;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Quests;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x020006A7 RID: 1703
	public class Dealer : NPC, IItemSlotOwner
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000C077E File Offset: 0x000BE97E
		// (set) Token: 0x06002E35 RID: 11829 RVA: 0x000C0786 File Offset: 0x000BE986
		public bool IsRecruited { get; private set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002E36 RID: 11830 RVA: 0x000C078F File Offset: 0x000BE98F
		// (set) Token: 0x06002E37 RID: 11831 RVA: 0x000C0797 File Offset: 0x000BE997
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002E38 RID: 11832 RVA: 0x000C07A0 File Offset: 0x000BE9A0
		// (set) Token: 0x06002E39 RID: 11833 RVA: 0x000C07A8 File Offset: 0x000BE9A8
		public float Cash
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Cash>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<Cash>k__BackingField(value, true);
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002E3A RID: 11834 RVA: 0x000C07B2 File Offset: 0x000BE9B2
		// (set) Token: 0x06002E3B RID: 11835 RVA: 0x000C07BA File Offset: 0x000BE9BA
		public bool HasBeenRecommended { get; private set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x000C07C3 File Offset: 0x000BE9C3
		// (set) Token: 0x06002E3D RID: 11837 RVA: 0x000C07CB File Offset: 0x000BE9CB
		public NPCPoI potentialDealerPoI { get; protected set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x000C07D4 File Offset: 0x000BE9D4
		// (set) Token: 0x06002E3F RID: 11839 RVA: 0x000C07DC File Offset: 0x000BE9DC
		public NPCPoI dealerPoI { get; protected set; }

		// Token: 0x06002E40 RID: 11840 RVA: 0x000C07E8 File Offset: 0x000BE9E8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Economy.Dealer_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000C0807 File Offset: 0x000BEA07
		protected override void OnValidate()
		{
			base.OnValidate();
			this.HomeEvent.Building = this.Home;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000C0820 File Offset: 0x000BEA20
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Dealer.AllDealers.Remove(this);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000C0834 File Offset: 0x000BEA34
		protected override void Start()
		{
			base.Start();
			if (Application.isEditor)
			{
				foreach (Customer customer in this.InitialCustomers)
				{
					this.SendAddCustomer(customer.NPC.ID);
				}
				foreach (ProductDefinition productDefinition in this.InitialItems)
				{
					base.Inventory.InsertItem(productDefinition.GetDefaultInstance(10), true);
				}
			}
			for (int i = 0; i < base.Inventory.ItemSlots.Count; i++)
			{
				base.Inventory.ItemSlots[i].AddFilter(new ItemFilter_PackagedProduct());
			}
			this.SetUpDialogue();
			this.SetupPoI();
			NPCRelationData relationData = this.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.OnDealerUnlocked));
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000C0960 File Offset: 0x000BEB60
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			if (this.IsRecruited)
			{
				this.SetIsRecruited(connection);
			}
			foreach (Customer customer in this.AssignedCustomers)
			{
				this.AddCustomer(connection, customer.NPC.ID);
			}
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000C09E0 File Offset: 0x000BEBE0
		private void SetupPoI()
		{
			if (this.dealerPoI == null)
			{
				this.dealerPoI = UnityEngine.Object.Instantiate<NPCPoI>(NetworkSingleton<NPCManager>.Instance.NPCPoIPrefab, base.transform);
				this.dealerPoI.SetMainText(base.fullName + "\n(Dealer)");
				this.dealerPoI.SetNPC(this);
				this.dealerPoI.transform.localPosition = Vector3.zero;
				this.dealerPoI.enabled = this.IsRecruited;
			}
			if (this.potentialDealerPoI == null)
			{
				this.potentialDealerPoI = UnityEngine.Object.Instantiate<NPCPoI>(NetworkSingleton<NPCManager>.Instance.PotentialDealerPoIPrefab, base.transform);
				this.potentialDealerPoI.SetMainText("Potential Dealer\n" + base.fullName);
				this.potentialDealerPoI.SetNPC(this);
				float y = (float)(this.FirstName[0] % '$') * 10f;
				float d = Mathf.Clamp((float)this.FirstName.Length * 1.5f, 1f, 10f);
				Vector3 vector = base.transform.forward;
				vector = Quaternion.Euler(0f, y, 0f) * vector;
				this.potentialDealerPoI.transform.localPosition = vector * d;
			}
			this.UpdatePotentialDealerPoI();
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000C0B30 File Offset: 0x000BED30
		private void SetUpDialogue()
		{
			this.recruitChoice = new DialogueController.DialogueChoice();
			this.recruitChoice.ChoiceText = "Do you want to work for me as a distributor?";
			this.recruitChoice.Enabled = !this.IsRecruited;
			this.recruitChoice.Conversation = this.RecruitDialogue;
			this.recruitChoice.onChoosen.AddListener(new UnityAction(this.RecruitmentRequested));
			this.recruitChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.CanOfferRecruitment);
			this.DialogueController.AddDialogueChoice(this.recruitChoice, 0);
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "Nevermind";
			dialogueChoice.Enabled = true;
			this.DialogueController.AddDialogueChoice(dialogueChoice, 0);
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000C0BEC File Offset: 0x000BEDEC
		protected override void MinPass()
		{
			base.MinPass();
			this.UpdatePotentialDealerPoI();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			if (this.currentContract != null)
			{
				this.UpdateCurrentDeal();
			}
			else
			{
				this.CheckAttendStart();
			}
			this.HomeEvent.gameObject.SetActive(true);
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000C0C47 File Offset: 0x000BEE47
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void MarkAsRecommended()
		{
			this.RpcWriter___Server_MarkAsRecommended_2166136261();
			this.RpcLogic___MarkAsRecommended_2166136261();
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000C0C55 File Offset: 0x000BEE55
		[ObserversRpc(RunLocally = true)]
		private void SetRecommended()
		{
			this.RpcWriter___Observers_SetRecommended_2166136261();
			this.RpcLogic___SetRecommended_2166136261();
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000C0C63 File Offset: 0x000BEE63
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void InitialRecruitment()
		{
			this.RpcWriter___Server_InitialRecruitment_2166136261();
			this.RpcLogic___InitialRecruitment_2166136261();
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000C0C74 File Offset: 0x000BEE74
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void SetIsRecruited(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsRecruited_328543758(conn);
				this.RpcLogic___SetIsRecruited_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_SetIsRecruited_328543758(conn);
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000C0CA9 File Offset: 0x000BEEA9
		protected virtual void OnDealerUnlocked(NPCRelationData.EUnlockType unlockType, bool b)
		{
			this.UpdatePotentialDealerPoI();
			NetworkSingleton<MoneyManager>.Instance.CashSound.Play();
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000C0CC0 File Offset: 0x000BEEC0
		protected virtual void UpdatePotentialDealerPoI()
		{
			this.potentialDealerPoI.enabled = (this.RelationData.IsMutuallyKnown() && !this.RelationData.Unlocked);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000C0CEC File Offset: 0x000BEEEC
		private void TradeItems()
		{
			this.dialogueHandler.SkipNextDialogueBehaviourEnd();
			this.itemCountOnTradeStart = base.Inventory.GetTotalItemCount();
			Singleton<StorageMenu>.Instance.Open(base.Inventory, base.fullName + "'s Inventory", "Place <color=#4CB0FF>packaged product</color> here and the dealer will sell it to assigned customers");
			Singleton<StorageMenu>.Instance.onClosed.AddListener(new UnityAction(this.TradeItemsDone));
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000C0D58 File Offset: 0x000BEF58
		private void TradeItemsDone()
		{
			Singleton<StorageMenu>.Instance.onClosed.RemoveListener(new UnityAction(this.TradeItemsDone));
			this.behaviour.GenericDialogueBehaviour.SendDisable();
			if (base.Inventory.GetTotalItemCount() > this.itemCountOnTradeStart)
			{
				this.dialogueHandler.WorldspaceRend.ShowText("Thanks boss", 2.5f);
				base.PlayVO(EVOLineType.Thanks);
			}
			this.TryMoveOverflowItems();
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000C0DCA File Offset: 0x000BEFCA
		private bool CanCollectCash(out string reason)
		{
			reason = string.Empty;
			return this.Cash > 0f;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000C0DE3 File Offset: 0x000BEFE3
		private void UpdateCollectCashChoice(float oldCash, float newCash, bool asServer)
		{
			if (this.collectCashChoice == null)
			{
				return;
			}
			this.collectCashChoice.ChoiceText = "I need to collect the earnings <color=#54E717>(" + MoneyManager.FormatAmount(this.Cash, false, false) + ")</color>";
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000C0E15 File Offset: 0x000BF015
		private void CollectCash()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.Cash, true, true);
			this.SetCash(0f);
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000C0E34 File Offset: 0x000BF034
		private void UpdateCurrentDeal()
		{
			if (this.currentContract.QuestState != EQuestState.Active)
			{
				this.currentContract.SetDealer(null);
				this.currentContract = null;
				this.DealSignal.gameObject.SetActive(false);
				return;
			}
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000C0E6C File Offset: 0x000BF06C
		private bool CanOfferRecruitment(out string reason)
		{
			reason = string.Empty;
			if (this.IsRecruited)
			{
				return false;
			}
			if (!this.RelationData.IsMutuallyKnown())
			{
				reason = "Unlock one of " + this.FirstName + "'s connections";
				return false;
			}
			if (!this.HasBeenRecommended)
			{
				reason = "One of " + this.FirstName + "'s connections must first recommend you";
				return false;
			}
			return true;
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000C0ED4 File Offset: 0x000BF0D4
		private void CheckAttendStart()
		{
			Contract contract = this.ActiveContracts.FirstOrDefault<Contract>();
			if (contract == null)
			{
				return;
			}
			int time = TimeManager.AddMinutesTo24HourTime(contract.DeliveryWindow.WindowStartTime, 30);
			int num = Mathf.CeilToInt(Vector3.Distance(this.Avatar.CenterPoint, contract.DeliveryLocation.CustomerStandPoint.position) / base.Movement.WalkSpeed * 1.5f);
			num = Mathf.Clamp(num, 15, 360);
			int min = TimeManager.AddMinutesTo24HourTime(time, -num);
			int minsUntilExpiry = contract.GetMinsUntilExpiry();
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, contract.DeliveryWindow.WindowEndTime) || minsUntilExpiry <= 240)
			{
				Debug.Log("Dealer start attend deal: " + contract.Title);
				this.currentContract = contract;
				this.DealSignal.SetStartTime(NetworkSingleton<TimeManager>.Instance.CurrentTime);
				this.DealSignal.AssignContract(contract);
				this.DealSignal.gameObject.SetActive(true);
			}
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000C0FCC File Offset: 0x000BF1CC
		public virtual bool ShouldAcceptContract(ContractInfo contractInfo, Customer customer)
		{
			foreach (ProductList.Entry entry in contractInfo.Products.entries)
			{
				string productID = entry.ProductID;
				EQuality minQuality = customer.CustomerData.Standards.GetCorrespondingQuality();
				EQuality maxQuality = customer.CustomerData.Standards.GetCorrespondingQuality();
				if (this.SellInsufficientQualityItems)
				{
					minQuality = EQuality.Trash;
				}
				if (this.SellExcessQualityItems)
				{
					maxQuality = EQuality.Heavenly;
				}
				int productCount = this.GetProductCount(productID, minQuality, maxQuality);
				if (entry.Quantity > productCount)
				{
					Console.Log(string.Concat(new string[]
					{
						"Dealer ",
						base.fullName,
						" does not have enough ",
						productID,
						" for ",
						customer.NPC.fullName
					}), null);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000C10C0 File Offset: 0x000BF2C0
		public virtual void ContractedOffered(ContractInfo contractInfo, Customer customer)
		{
			if (!this.ShouldAcceptContract(contractInfo, customer))
			{
				Console.Log("Contract accepted by dealer " + base.fullName, null);
				return;
			}
			EDealWindow dealWindow = this.GetDealWindow();
			Console.Log("Contract accepted by dealer " + base.fullName + " in window " + dealWindow.ToString(), null);
			this.SyncAccessor_acceptedContractGUIDs.Add(customer.ContractAccepted(dealWindow, false));
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000C1130 File Offset: 0x000BF330
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendAddCustomer(string npcID)
		{
			this.RpcWriter___Server_SendAddCustomer_3615296227(npcID);
			this.RpcLogic___SendAddCustomer_3615296227(npcID);
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000C1148 File Offset: 0x000BF348
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void AddCustomer(NetworkConnection conn, string npcID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_AddCustomer_2971853958(conn, npcID);
				this.RpcLogic___AddCustomer_2971853958(conn, npcID);
			}
			else
			{
				this.RpcWriter___Target_AddCustomer_2971853958(conn, npcID);
			}
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000C1189 File Offset: 0x000BF389
		protected virtual void AddCustomer(Customer customer)
		{
			if (this.AssignedCustomers.Contains(customer))
			{
				return;
			}
			this.AssignedCustomers.Add(customer);
			customer.AssignDealer(this);
			customer.onContractAssigned.AddListener(new UnityAction<Contract>(this.CustomerContractStarted));
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000C11C4 File Offset: 0x000BF3C4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendRemoveCustomer(string npcID)
		{
			this.RpcWriter___Server_SendRemoveCustomer_3615296227(npcID);
			this.RpcLogic___SendRemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000C11DC File Offset: 0x000BF3DC
		[ObserversRpc(RunLocally = true)]
		private void RemoveCustomer(string npcID)
		{
			this.RpcWriter___Observers_RemoveCustomer_3615296227(npcID);
			this.RpcLogic___RemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000C11FD File Offset: 0x000BF3FD
		public virtual void RemoveCustomer(Customer customer)
		{
			if (!this.AssignedCustomers.Contains(customer))
			{
				return;
			}
			this.AssignedCustomers.Remove(customer);
			customer.AssignDealer(null);
			customer.onContractAssigned.RemoveListener(new UnityAction<Contract>(this.CustomerContractStarted));
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000C1239 File Offset: 0x000BF439
		public void ChangeCash(float change)
		{
			this.SetCash(this.Cash + change);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000C1249 File Offset: 0x000BF449
		[ServerRpc(RequireOwnership = false)]
		public void SetCash(float cash)
		{
			this.RpcWriter___Server_SetCash_431000436(cash);
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000C1258 File Offset: 0x000BF458
		[ServerRpc(RequireOwnership = false)]
		public virtual void CompletedDeal()
		{
			this.RpcWriter___Server_CompletedDeal_2166136261();
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000C126C File Offset: 0x000BF46C
		[ServerRpc(RequireOwnership = false)]
		public void SubmitPayment(float payment)
		{
			this.RpcWriter___Server_SubmitPayment_431000436(payment);
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000C1284 File Offset: 0x000BF484
		public List<ProductDefinition> GetOrderableProducts()
		{
			List<ProductDefinition> list = new List<ProductDefinition>();
			foreach (ItemSlot itemSlot in this.GetAllSlots())
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is ProductItemInstance)
				{
					ProductItemInstance product = itemSlot.ItemInstance as ProductItemInstance;
					if (list.Find((ProductDefinition x) => x.ID == product.ID) == null)
					{
						list.Add(product.Definition as ProductDefinition);
					}
				}
			}
			return list;
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000C1334 File Offset: 0x000BF534
		public int GetProductCount(string productID, EQuality minQuality, EQuality maxQuality)
		{
			int num = 0;
			foreach (ItemSlot itemSlot in this.GetAllSlots())
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
					if (productItemInstance.ID == productID && productItemInstance.Quality >= minQuality && productItemInstance.Quality <= maxQuality)
					{
						num += productItemInstance.Quantity * productItemInstance.Amount;
					}
				}
			}
			return num;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x000C13D4 File Offset: 0x000BF5D4
		private EDealWindow GetDealWindow()
		{
			EDealWindow window = DealWindowInfo.GetWindow(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			int num = (int)window;
			int num2 = TimeManager.GetMinSumFrom24HourTime(DealWindowInfo.GetWindowInfo(window).EndTime) - TimeManager.GetMinSumFrom24HourTime(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			List<EDealWindow> list = new List<EDealWindow>();
			if (num2 > 120)
			{
				list.Add(window);
			}
			for (int i = 1; i < 4; i++)
			{
				int item = (num + i) % 4;
				list.Add((EDealWindow)item);
			}
			int num3 = 3;
			for (;;)
			{
				foreach (EDealWindow edealWindow in list)
				{
					if (this.GetContractCountInWindow(edealWindow) <= num3)
					{
						return edealWindow;
					}
				}
				num3++;
			}
			EDealWindow result;
			return result;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x000C149C File Offset: 0x000BF69C
		private int GetContractCountInWindow(EDealWindow window)
		{
			int num = 0;
			using (List<Contract>.Enumerator enumerator = this.ActiveContracts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (DealWindowInfo.GetWindow(TimeManager.AddMinutesTo24HourTime(enumerator.Current.DeliveryWindow.WindowStartTime, 1)) == window)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000C1508 File Offset: 0x000BF708
		private void CustomerContractStarted(Contract contract)
		{
			if (!this.SyncAccessor_acceptedContractGUIDs.Contains(contract.GUID.ToString()))
			{
				return;
			}
			this.ActiveContracts.Add(contract);
			contract.SetDealer(this);
			contract.onQuestEnd.AddListener(delegate(EQuestState <p0>)
			{
				this.CustomerContractEnded(contract);
			});
			contract.ShouldSendExpiredNotification = false;
			contract.ShouldSendExpiryReminder = false;
			base.Invoke("SortContracts", 0.05f);
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000C15B4 File Offset: 0x000BF7B4
		private void CustomerContractEnded(Contract contract)
		{
			if (!this.ActiveContracts.Contains(contract))
			{
				return;
			}
			this.ActiveContracts.Remove(contract);
			contract.SetDealer(null);
			if (InstanceFinder.IsServer && this.GetTotalInventoryItemCount() == 0)
			{
				DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Dealer, "inventory_depleted");
				base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 0f, true, true);
			}
			base.Invoke("SortContracts", 0.05f);
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000C1632 File Offset: 0x000BF832
		private void SortContracts()
		{
			this.ActiveContracts = (from x in this.ActiveContracts
			orderby x.GetMinsUntilExpiry()
			select x).ToList<Contract>();
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void RecruitmentRequested()
		{
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000C166C File Offset: 0x000BF86C
		public bool RemoveContractItems(Contract contract, EQuality targetQuality, out List<ItemInstance> items)
		{
			Dealer.<>c__DisplayClass101_0 CS$<>8__locals1 = new Dealer.<>c__DisplayClass101_0();
			CS$<>8__locals1.targetQuality = targetQuality;
			CS$<>8__locals1.<>4__this = this;
			items = new List<ItemInstance>();
			foreach (ProductList.Entry entry in contract.ProductList.entries)
			{
				int num;
				List<ItemInstance> items2 = this.GetItems(entry.ProductID, entry.Quantity, new Func<ProductItemInstance, bool>(CS$<>8__locals1.<RemoveContractItems>g__DoesQualityMatch|0), out num);
				if (num < entry.Quantity)
				{
					Console.LogWarning("Could not find enough items for contract entry: " + entry.ProductID, null);
				}
				items.AddRange(items2);
			}
			this.TryMoveOverflowItems();
			return true;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000C1728 File Offset: 0x000BF928
		private List<ItemInstance> GetItems(string ID, int requiredQuantity, Func<ProductItemInstance, bool> qualityCheck, out int returnedQuantity)
		{
			List<ItemInstance> list = new List<ItemInstance>();
			returnedQuantity = 0;
			List<ItemSlot> allSlots = this.GetAllSlots();
			for (int i = 0; i < allSlots.Count; i++)
			{
				if (allSlots[i].ItemInstance == null)
				{
					allSlots.RemoveAt(i);
					i--;
				}
				else
				{
					ProductItemInstance productItemInstance = allSlots[i].ItemInstance as ProductItemInstance;
					if (productItemInstance == null || productItemInstance.ID != ID || productItemInstance.AppliedPackaging == null || !qualityCheck(productItemInstance))
					{
						allSlots.RemoveAt(i);
						i--;
					}
				}
			}
			allSlots.Sort(delegate(ItemSlot x, ItemSlot y)
			{
				if (x.ItemInstance == null)
				{
					return 1;
				}
				if (y.ItemInstance == null)
				{
					return -1;
				}
				return (y.ItemInstance as ProductItemInstance).Amount.CompareTo((x.ItemInstance as ProductItemInstance).Amount);
			});
			foreach (ItemSlot itemSlot in allSlots)
			{
				int amount = (itemSlot.ItemInstance as ProductItemInstance).Amount;
				while (requiredQuantity >= amount && itemSlot.Quantity > 0)
				{
					list.Add(itemSlot.ItemInstance.GetCopy(1));
					itemSlot.ChangeQuantity(-1, false);
					returnedQuantity += amount;
					requiredQuantity -= amount;
				}
			}
			if (requiredQuantity > 0)
			{
				while (requiredQuantity > 0)
				{
					allSlots = this.GetAllSlots();
					for (int j = 0; j < allSlots.Count; j++)
					{
						if (allSlots[j].ItemInstance == null)
						{
							allSlots.RemoveAt(j);
							j--;
						}
						else
						{
							ProductItemInstance productItemInstance2 = allSlots[j].ItemInstance as ProductItemInstance;
							if (productItemInstance2 == null || productItemInstance2.ID != ID || productItemInstance2.AppliedPackaging == null || !qualityCheck(productItemInstance2))
							{
								allSlots.RemoveAt(j);
								j--;
							}
						}
					}
					if (allSlots.Count == 0)
					{
						Console.LogWarning("Dealer " + base.fullName + " has no items to fulfill contract", null);
						return list;
					}
					allSlots.Sort(delegate(ItemSlot x, ItemSlot y)
					{
						if (x.ItemInstance == null)
						{
							return -1;
						}
						if (y.ItemInstance == null)
						{
							return 1;
						}
						return (x.ItemInstance as ProductItemInstance).Amount.CompareTo((y.ItemInstance as ProductItemInstance).Amount);
					});
					ItemSlot itemSlot2 = allSlots[0];
					int amount2 = (itemSlot2.ItemInstance as ProductItemInstance).Amount;
					if (requiredQuantity >= amount2)
					{
						while (requiredQuantity >= amount2)
						{
							if (itemSlot2.Quantity <= 0)
							{
								break;
							}
							Console.Log(string.Concat(new string[]
							{
								"Removing 1x ",
								itemSlot2.ItemInstance.Name,
								"(",
								(itemSlot2.ItemInstance as ProductItemInstance).AppliedPackaging.Name,
								")"
							}), null);
							list.Add(itemSlot2.ItemInstance.GetCopy(1));
							itemSlot2.ChangeQuantity(-1, false);
							returnedQuantity += amount2;
							requiredQuantity -= amount2;
						}
					}
					else
					{
						PackagingDefinition appliedPackaging = (itemSlot2.ItemInstance as ProductItemInstance).AppliedPackaging;
						ProductDefinition productDefinition = (itemSlot2.ItemInstance as ProductItemInstance).Definition as ProductDefinition;
						PackagingDefinition packagingDefinition = null;
						for (int k = 0; k < productDefinition.ValidPackaging.Length; k++)
						{
							if (productDefinition.ValidPackaging[k].ID == appliedPackaging.ID && k > 0)
							{
								packagingDefinition = productDefinition.ValidPackaging[k - 1];
							}
						}
						if (packagingDefinition == null)
						{
							Console.LogWarning("Failed to find next packaging smaller than " + appliedPackaging.ID, null);
							break;
						}
						int quantity = packagingDefinition.Quantity;
						int overrideQuantity = appliedPackaging.Quantity / quantity;
						Console.Log(string.Concat(new string[]
						{
							"Splitting 1x ",
							itemSlot2.ItemInstance.Name,
							"(",
							appliedPackaging.Name,
							") into ",
							overrideQuantity.ToString(),
							"x ",
							packagingDefinition.Name
						}), null);
						ProductItemInstance productItemInstance3 = itemSlot2.ItemInstance.GetCopy(overrideQuantity) as ProductItemInstance;
						productItemInstance3.SetPackaging(packagingDefinition);
						itemSlot2.ChangeQuantity(-1, false);
						this.AddItemToInventory(productItemInstance3);
					}
				}
			}
			return list;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000C1B48 File Offset: 0x000BFD48
		public List<ItemSlot> GetAllSlots()
		{
			List<ItemSlot> list = new List<ItemSlot>(base.Inventory.ItemSlots);
			list.AddRange(this.OverflowSlots);
			return list;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000C1B68 File Offset: 0x000BFD68
		public void AddItemToInventory(ItemInstance item)
		{
			while (base.Inventory.CanItemFit(item) && item.Quantity > 0)
			{
				base.Inventory.InsertItem(item.GetCopy(1), true);
				item.ChangeQuantity(-1);
			}
			if (item.Quantity > 0 && !ItemSlot.TryInsertItemIntoSet(this.OverflowSlots.ToList<ItemSlot>(), item))
			{
				Console.LogWarning("Dealer " + base.fullName + " has doesn't have enough space for item " + item.ID, null);
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000C1BE8 File Offset: 0x000BFDE8
		public void TryMoveOverflowItems()
		{
			foreach (ItemSlot itemSlot in this.OverflowSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					while (base.Inventory.CanItemFit(itemSlot.ItemInstance) && itemSlot.ItemInstance.Quantity > 0)
					{
						base.Inventory.InsertItem(itemSlot.ItemInstance.GetCopy(1), true);
						itemSlot.ItemInstance.ChangeQuantity(-1);
					}
				}
			}
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000C1C60 File Offset: 0x000BFE60
		public int GetTotalInventoryItemCount()
		{
			List<ItemSlot> allSlots = this.GetAllSlots();
			int num = 0;
			foreach (ItemSlot itemSlot in allSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					num += itemSlot.ItemInstance.Quantity;
				}
			}
			return num;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000C1CC8 File Offset: 0x000BFEC8
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000C1CF0 File Offset: 0x000BFEF0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetStoredInstance_Internal(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
			else
			{
				this.RpcWriter___Target_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000C1D4F File Offset: 0x000BFF4F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000C1D6D File Offset: 0x000BFF6D
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000C1D8B File Offset: 0x000BFF8B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000C1DC4 File Offset: 0x000BFFC4
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetSlotLocked_Internal(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
			else
			{
				this.RpcWriter___Target_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000C1E43 File Offset: 0x000C0043
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotFilter(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.RpcWriter___Server_SetSlotFilter_527532783(conn, itemSlotIndex, filter);
			this.RpcLogic___SetSlotFilter_527532783(conn, itemSlotIndex, filter);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000C1E6C File Offset: 0x000C006C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetSlotFilter_Internal(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
				this.RpcLogic___SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
			}
			else
			{
				this.RpcWriter___Target_SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
				this.RpcLogic___SetSlotFilter_Internal_527532783(conn, itemSlotIndex, filter);
			}
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000C1ECC File Offset: 0x000C00CC
		public override NPCData GetNPCData()
		{
			string[] array = new string[this.AssignedCustomers.Count];
			for (int i = 0; i < this.AssignedCustomers.Count; i++)
			{
				array[i] = this.AssignedCustomers[i].NPC.ID;
			}
			string[] array2 = new string[this.ActiveContracts.Count];
			for (int j = 0; j < this.ActiveContracts.Count; j++)
			{
				array2[j] = this.ActiveContracts[j].GUID.ToString();
			}
			return new DealerData(this.ID, this.IsRecruited, array, array2, this.Cash, new ItemSet(this.OverflowSlots), this.HasBeenRecommended);
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x000C1F90 File Offset: 0x000C0190
		public override void Load(DynamicSaveData dynamicData, NPCData npcData)
		{
			base.Load(dynamicData, npcData);
			DealerData dealerData;
			if (dynamicData.TryExtractBaseData<DealerData>(out dealerData))
			{
				if (dealerData.Recruited)
				{
					this.SetIsRecruited(null);
				}
				this.SetCash(dealerData.Cash);
				for (int i = 0; i < dealerData.AssignedCustomerIDs.Length; i++)
				{
					NPC npc = NPCManager.GetNPC(dealerData.AssignedCustomerIDs[i]);
					if (npc == null)
					{
						Console.LogWarning("Failed to find customer NPC with ID " + dealerData.AssignedCustomerIDs[i], null);
					}
					else
					{
						Customer component = npc.GetComponent<Customer>();
						if (component == null)
						{
							Console.LogWarning("NPC is not a customer: " + npc.fullName, null);
						}
						else
						{
							this.SendAddCustomer(component.NPC.ID);
						}
					}
				}
				if (dealerData.ActiveContractGUIDs != null)
				{
					for (int j = 0; j < dealerData.ActiveContractGUIDs.Length; j++)
					{
						if (!GUIDManager.IsGUIDValid(dealerData.ActiveContractGUIDs[j]))
						{
							Console.LogWarning("Invalid contract GUID: " + dealerData.ActiveContractGUIDs[j], null);
						}
						else
						{
							Contract @object = GUIDManager.GetObject<Contract>(new Guid(dealerData.ActiveContractGUIDs[j]));
							if (@object != null)
							{
								this.SyncAccessor_acceptedContractGUIDs.Add(@object.GUID.ToString());
								this.CustomerContractStarted(@object);
							}
						}
					}
				}
				if (dealerData.HasBeenRecommended)
				{
					this.MarkAsRecommended();
				}
				dealerData.OverflowItems.LoadTo(this.OverflowSlots);
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000C2104 File Offset: 0x000C0304
		public override void Load(NPCData data, string containerPath)
		{
			base.Load(data, containerPath);
			string text;
			if (((ISaveable)this).TryLoadFile(containerPath, "NPC", out text))
			{
				DealerData dealerData = null;
				try
				{
					dealerData = JsonUtility.FromJson<DealerData>(text);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize character data: " + ex.Message, null);
					return;
				}
				if (dealerData == null)
				{
					return;
				}
				if (dealerData.Recruited)
				{
					this.SetIsRecruited(null);
				}
				this.SetCash(dealerData.Cash);
				for (int i = 0; i < dealerData.AssignedCustomerIDs.Length; i++)
				{
					NPC npc = NPCManager.GetNPC(dealerData.AssignedCustomerIDs[i]);
					if (npc == null)
					{
						Console.LogWarning("Failed to find customer NPC with ID " + dealerData.AssignedCustomerIDs[i], null);
					}
					else
					{
						Customer component = npc.GetComponent<Customer>();
						if (component == null)
						{
							Console.LogWarning("NPC is not a customer: " + npc.fullName, null);
						}
						else
						{
							this.SendAddCustomer(component.NPC.ID);
						}
					}
				}
				if (dealerData.ActiveContractGUIDs != null)
				{
					for (int j = 0; j < dealerData.ActiveContractGUIDs.Length; j++)
					{
						if (!GUIDManager.IsGUIDValid(dealerData.ActiveContractGUIDs[j]))
						{
							Console.LogWarning("Invalid contract GUID: " + dealerData.ActiveContractGUIDs[j], null);
						}
						else
						{
							Contract @object = GUIDManager.GetObject<Contract>(new Guid(dealerData.ActiveContractGUIDs[j]));
							if (@object != null)
							{
								this.SyncAccessor_acceptedContractGUIDs.Add(@object.GUID.ToString());
								this.CustomerContractStarted(@object);
							}
						}
					}
				}
				if (dealerData.HasBeenRecommended)
				{
					this.MarkAsRecommended();
				}
				dealerData.OverflowItems.LoadTo(this.OverflowSlots);
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000C2384 File Offset: 0x000C0584
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___acceptedContractGUIDs = new SyncVar<List<string>>(this, 2U, 0, 0, -1f, 0, this.acceptedContractGUIDs);
			this.syncVar___<Cash>k__BackingField = new SyncVar<float>(this, 1U, 0, 0, -1f, 0, this.<Cash>k__BackingField);
			this.syncVar___<Cash>k__BackingField.OnChange += this.UpdateCollectCashChoice;
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_MarkAsRecommended_2166136261));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_SetRecommended_2166136261));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_InitialRecruitment_2166136261));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsRecruited_328543758));
			base.RegisterTargetRpc(39U, new ClientRpcDelegate(this.RpcReader___Target_SetIsRecruited_328543758));
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SendAddCustomer_3615296227));
			base.RegisterObserversRpc(41U, new ClientRpcDelegate(this.RpcReader___Observers_AddCustomer_2971853958));
			base.RegisterTargetRpc(42U, new ClientRpcDelegate(this.RpcReader___Target_AddCustomer_2971853958));
			base.RegisterServerRpc(43U, new ServerRpcDelegate(this.RpcReader___Server_SendRemoveCustomer_3615296227));
			base.RegisterObserversRpc(44U, new ClientRpcDelegate(this.RpcReader___Observers_RemoveCustomer_3615296227));
			base.RegisterServerRpc(45U, new ServerRpcDelegate(this.RpcReader___Server_SetCash_431000436));
			base.RegisterServerRpc(46U, new ServerRpcDelegate(this.RpcReader___Server_CompletedDeal_2166136261));
			base.RegisterServerRpc(47U, new ServerRpcDelegate(this.RpcReader___Server_SubmitPayment_431000436));
			base.RegisterServerRpc(48U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(49U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(50U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(51U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(52U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(53U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(54U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(55U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterServerRpc(56U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotFilter_527532783));
			base.RegisterObserversRpc(57U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotFilter_Internal_527532783));
			base.RegisterTargetRpc(58U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotFilter_Internal_527532783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Economy.Dealer));
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000C264F File Offset: 0x000C084F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___acceptedContractGUIDs.SetRegistered();
			this.syncVar___<Cash>k__BackingField.SetRegistered();
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000C267E File Offset: 0x000C087E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000C268C File Offset: 0x000C088C
		private void RpcWriter___Server_MarkAsRecommended_2166136261()
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
			base.SendServerRpc(35U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000C2726 File Offset: 0x000C0926
		public void RpcLogic___MarkAsRecommended_2166136261()
		{
			this.SetRecommended();
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000C2730 File Offset: 0x000C0930
		private void RpcReader___Server_MarkAsRecommended_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkAsRecommended_2166136261();
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000C2760 File Offset: 0x000C0960
		private void RpcWriter___Observers_SetRecommended_2166136261()
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
			base.SendObserversRpc(36U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000C2809 File Offset: 0x000C0A09
		private void RpcLogic___SetRecommended_2166136261()
		{
			if (this.HasBeenRecommended)
			{
				return;
			}
			this.HasBeenRecommended = true;
			base.HasChanged = true;
			if (this.onRecommended != null)
			{
				this.onRecommended.Invoke();
			}
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000C2838 File Offset: 0x000C0A38
		private void RpcReader___Observers_SetRecommended_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetRecommended_2166136261();
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000C2864 File Offset: 0x000C0A64
		private void RpcWriter___Server_InitialRecruitment_2166136261()
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
			base.SendServerRpc(37U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000C237A File Offset: 0x000C057A
		public void RpcLogic___InitialRecruitment_2166136261()
		{
			this.SetIsRecruited(null);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000C2900 File Offset: 0x000C0B00
		private void RpcReader___Server_InitialRecruitment_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___InitialRecruitment_2166136261();
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000C2930 File Offset: 0x000C0B30
		private void RpcWriter___Observers_SetIsRecruited_328543758(NetworkConnection conn)
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
			base.SendObserversRpc(38U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000C29DC File Offset: 0x000C0BDC
		public virtual void RpcLogic___SetIsRecruited_328543758(NetworkConnection conn)
		{
			if (this.IsRecruited)
			{
				return;
			}
			this.IsRecruited = true;
			DialogueController.GreetingOverride greetingOverride = new DialogueController.GreetingOverride();
			greetingOverride.Greeting = "Hi boss, what do you need?";
			greetingOverride.PlayVO = true;
			greetingOverride.VOType = EVOLineType.Greeting;
			greetingOverride.ShouldShow = true;
			this.DialogueController.AddGreetingOverride(greetingOverride);
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "I need to trade some items";
			dialogueChoice.Enabled = true;
			dialogueChoice.onChoosen.AddListener(new UnityAction(this.TradeItems));
			this.DialogueController.AddDialogueChoice(dialogueChoice, 5);
			this.collectCashChoice = new DialogueController.DialogueChoice();
			this.UpdateCollectCashChoice(0f, 0f, false);
			this.collectCashChoice.Enabled = true;
			this.collectCashChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.CanCollectCash);
			this.collectCashChoice.onChoosen.AddListener(new UnityAction(this.CollectCash));
			this.collectCashChoice.Conversation = this.CollectCashDialogue;
			this.DialogueController.AddDialogueChoice(this.collectCashChoice, 4);
			this.assignCustomersChoice = new DialogueController.DialogueChoice();
			this.assignCustomersChoice.ChoiceText = "How do I assign customers to you?";
			this.assignCustomersChoice.Enabled = true;
			this.assignCustomersChoice.Conversation = this.AssignCustomersDialogue;
			this.DialogueController.AddDialogueChoice(this.assignCustomersChoice, 3);
			if (this.dealerPoI != null)
			{
				this.dealerPoI.enabled = true;
			}
			if (!this.RelationData.Unlocked)
			{
				this.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, false);
			}
			if (this.recruitChoice != null)
			{
				this.recruitChoice.Enabled = false;
			}
			if (Dealer.onDealerRecruited != null)
			{
				Dealer.onDealerRecruited(this);
			}
			base.HasChanged = true;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000C2B94 File Offset: 0x000C0D94
		private void RpcReader___Observers_SetIsRecruited_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsRecruited_328543758(null);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000C2BC0 File Offset: 0x000C0DC0
		private void RpcWriter___Target_SetIsRecruited_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(39U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000C2C68 File Offset: 0x000C0E68
		private void RpcReader___Target_SetIsRecruited_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsRecruited_328543758(base.LocalConnection);
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000C2C90 File Offset: 0x000C0E90
		private void RpcWriter___Server_SendAddCustomer_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendServerRpc(40U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000C2D37 File Offset: 0x000C0F37
		public void RpcLogic___SendAddCustomer_3615296227(string npcID)
		{
			this.AddCustomer(null, npcID);
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000C2D44 File Offset: 0x000C0F44
		private void RpcReader___Server_SendAddCustomer_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAddCustomer_3615296227(npcID);
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x000C2D84 File Offset: 0x000C0F84
		private void RpcWriter___Observers_AddCustomer_2971853958(NetworkConnection conn, string npcID)
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
			writer.WriteString(npcID);
			base.SendObserversRpc(41U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x000C2E3C File Offset: 0x000C103C
		private void RpcLogic___AddCustomer_2971853958(NetworkConnection conn, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogWarning("Failed to find NPC with ID: " + npcID, null);
				return;
			}
			Customer component = npc.GetComponent<Customer>();
			if (component == null)
			{
				Console.LogWarning("NPC " + npcID + " is not a customer", null);
				return;
			}
			this.AddCustomer(component);
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000C2E9C File Offset: 0x000C109C
		private void RpcReader___Observers_AddCustomer_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddCustomer_2971853958(null, npcID);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000C2ED8 File Offset: 0x000C10D8
		private void RpcWriter___Target_AddCustomer_2971853958(NetworkConnection conn, string npcID)
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
			writer.WriteString(npcID);
			base.SendTargetRpc(42U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000C2F90 File Offset: 0x000C1190
		private void RpcReader___Target_AddCustomer_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddCustomer_2971853958(base.LocalConnection, npcID);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000C2FC8 File Offset: 0x000C11C8
		private void RpcWriter___Server_SendRemoveCustomer_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendServerRpc(43U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000C306F File Offset: 0x000C126F
		public void RpcLogic___SendRemoveCustomer_3615296227(string npcID)
		{
			this.RemoveCustomer(npcID);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000C3078 File Offset: 0x000C1278
		private void RpcReader___Server_SendRemoveCustomer_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendRemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000C30B8 File Offset: 0x000C12B8
		private void RpcWriter___Observers_RemoveCustomer_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendObserversRpc(44U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000C3170 File Offset: 0x000C1370
		private void RpcLogic___RemoveCustomer_3615296227(string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogWarning("Failed to find NPC with ID: " + npcID, null);
				return;
			}
			Customer component = npc.GetComponent<Customer>();
			if (component == null)
			{
				Console.LogWarning("NPC " + npcID + " is not a customer", null);
				return;
			}
			this.RemoveCustomer(component);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000C31D0 File Offset: 0x000C13D0
		private void RpcReader___Observers_RemoveCustomer_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000C320C File Offset: 0x000C140C
		private void RpcWriter___Server_SetCash_431000436(float cash)
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
			writer.WriteSingle(cash, 0);
			base.SendServerRpc(45U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000C32B8 File Offset: 0x000C14B8
		public void RpcLogic___SetCash_431000436(float cash)
		{
			this.Cash = Mathf.Clamp(cash, 0f, float.MaxValue);
			base.HasChanged = true;
			this.UpdateCollectCashChoice(0f, 0f, false);
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000C32E8 File Offset: 0x000C14E8
		private void RpcReader___Server_SetCash_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float cash = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetCash_431000436(cash);
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000C3320 File Offset: 0x000C1520
		private void RpcWriter___Server_CompletedDeal_2166136261()
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
			base.SendServerRpc(46U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000C33BC File Offset: 0x000C15BC
		public virtual void RpcLogic___CompletedDeal_2166136261()
		{
			this.RelationData.ChangeRelationship(0.05f, true);
			if (this.CompletedDealsVariable != string.Empty)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.CompletedDealsVariable, (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>(this.CompletedDealsVariable) + 1f).ToString(), true);
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000C341C File Offset: 0x000C161C
		private void RpcReader___Server_CompletedDeal_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___CompletedDeal_2166136261();
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x000C343C File Offset: 0x000C163C
		private void RpcWriter___Server_SubmitPayment_431000436(float payment)
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
			writer.WriteSingle(payment, 0);
			base.SendServerRpc(47U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000C34E8 File Offset: 0x000C16E8
		public void RpcLogic___SubmitPayment_431000436(float payment)
		{
			if (payment <= 0f)
			{
				return;
			}
			Console.Log("Dealer " + base.fullName + " received payment: " + payment.ToString(), null);
			float cash = this.Cash;
			this.ChangeCash(payment * (1f - this.Cut));
			if (InstanceFinder.IsServer && this.Cash >= 500f && cash < 500f)
			{
				base.MSGConversation.SendMessage(new Message("Hey boss, just letting you know I've got " + MoneyManager.FormatAmount(this.Cash, false, false) + " ready for you to collect.", Message.ESenderType.Other, true, -1), true, true);
			}
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x000C3588 File Offset: 0x000C1788
		private void RpcReader___Server_SubmitPayment_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float payment = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SubmitPayment_431000436(payment);
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000C35C0 File Offset: 0x000C17C0
		private void RpcWriter___Server_SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendServerRpc(48U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000C3686 File Offset: 0x000C1886
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000C36B0 File Offset: 0x000C18B0
		private void RpcReader___Server_SetStoredInstance_2652194801(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_2652194801(conn2, itemSlotIndex, instance);
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000C3718 File Offset: 0x000C1918
		private void RpcWriter___Observers_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendObserversRpc(49U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x000C37E0 File Offset: 0x000C19E0
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x000C380C File Offset: 0x000C1A0C
		private void RpcReader___Observers_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(null, itemSlotIndex, instance);
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000C3860 File Offset: 0x000C1A60
		private void RpcWriter___Target_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteItemInstance(instance);
			base.SendTargetRpc(50U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000C3928 File Offset: 0x000C1B28
		private void RpcReader___Target_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(base.LocalConnection, itemSlotIndex, instance);
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000C3980 File Offset: 0x000C1B80
		private void RpcWriter___Server_SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendServerRpc(51U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000C3A3E File Offset: 0x000C1C3E
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000C3A48 File Offset: 0x000C1C48
		private void RpcReader___Server_SetItemSlotQuantity_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000C3AA4 File Offset: 0x000C1CA4
		private void RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteInt32(quantity, 1);
			base.SendObserversRpc(52U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000C3B71 File Offset: 0x000C1D71
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000C3B88 File Offset: 0x000C1D88
		private void RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			int quantity = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000C3BE0 File Offset: 0x000C1DE0
		private void RpcWriter___Server_SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendServerRpc(53U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000C3CC0 File Offset: 0x000C1EC0
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000C3CF0 File Offset: 0x000C1EF0
		private void RpcReader___Server_SetSlotLocked_3170825843(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_3170825843(conn2, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000C3D78 File Offset: 0x000C1F78
		private void RpcWriter___Target_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendTargetRpc(54U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000C3E59 File Offset: 0x000C2059
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000C3E88 File Offset: 0x000C2088
		private void RpcReader___Target_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(base.LocalConnection, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000C3F04 File Offset: 0x000C2104
		private void RpcWriter___Observers_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendObserversRpc(55U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000C3FE8 File Offset: 0x000C21E8
		private void RpcReader___Observers_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(null, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000C405C File Offset: 0x000C225C
		private void RpcWriter___Server_SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendServerRpc(56U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000C4122 File Offset: 0x000C2322
		public void RpcLogic___SetSlotFilter_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotFilter_Internal(null, itemSlotIndex, filter);
				return;
			}
			this.SetSlotFilter_Internal(conn, itemSlotIndex, filter);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000C414C File Offset: 0x000C234C
		private void RpcReader___Server_SetSlotFilter_527532783(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_527532783(conn2, itemSlotIndex, filter);
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000C41B4 File Offset: 0x000C23B4
		private void RpcWriter___Observers_SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendObserversRpc(57U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000C427C File Offset: 0x000C247C
		private void RpcLogic___SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
		{
			this.ItemSlots[itemSlotIndex].SetPlayerFilter(filter, true);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000C4294 File Offset: 0x000C2494
		private void RpcReader___Observers_SetSlotFilter_Internal_527532783(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_Internal_527532783(null, itemSlotIndex, filter);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000C42E8 File Offset: 0x000C24E8
		private void RpcWriter___Target_SetSlotFilter_Internal_527532783(NetworkConnection conn, int itemSlotIndex, SlotFilter filter)
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
			writer.WriteInt32(itemSlotIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generated(filter);
			base.SendTargetRpc(58U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000C43B0 File Offset: 0x000C25B0
		private void RpcReader___Target_SetSlotFilter_Internal_527532783(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(1);
			SlotFilter filter = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.SlotFilterFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotFilter_Internal_527532783(base.LocalConnection, itemSlotIndex, filter);
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x000C4407 File Offset: 0x000C2607
		// (set) Token: 0x06002EC5 RID: 11973 RVA: 0x000C440F File Offset: 0x000C260F
		public float SyncAccessor_<Cash>k__BackingField
		{
			get
			{
				return this.<Cash>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Cash>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Cash>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000C444C File Offset: 0x000C264C
		public override bool Dealer(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_acceptedContractGUIDs(this.syncVar___acceptedContractGUIDs.GetValue(true), true);
					return true;
				}
				List<string> value = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
				this.sync___set_value_acceptedContractGUIDs(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 1U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<Cash>k__BackingField(this.syncVar___<Cash>k__BackingField.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(0);
				this.sync___set_value_<Cash>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x000C44E7 File Offset: 0x000C26E7
		// (set) Token: 0x06002EC8 RID: 11976 RVA: 0x000C44EF File Offset: 0x000C26EF
		public List<string> SyncAccessor_acceptedContractGUIDs
		{
			get
			{
				return this.acceptedContractGUIDs;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.acceptedContractGUIDs = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___acceptedContractGUIDs.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000C452C File Offset: 0x000C272C
		protected override void dll()
		{
			base.Awake();
			this.HomeEvent.Building = this.Home;
			this.OverflowSlots = new ItemSlot[10];
			for (int i = 0; i < 10; i++)
			{
				this.OverflowSlots[i] = new ItemSlot();
				this.OverflowSlots[i].SetSlotOwner(this);
			}
			if (this.RelationData.Unlocked)
			{
				this.SetIsRecruited(null);
			}
			else
			{
				NPCRelationData relationData = this.RelationData;
				relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType <p0>, bool <p1>)
				{
					this.SetIsRecruited(null);
				}));
			}
			if (!Dealer.AllDealers.Contains(this))
			{
				Dealer.AllDealers.Add(this);
			}
		}

		// Token: 0x040020A8 RID: 8360
		public const int MAX_CUSTOMERS = 8;

		// Token: 0x040020A9 RID: 8361
		public const int DEAL_ARRIVAL_DELAY = 30;

		// Token: 0x040020AA RID: 8362
		public const int MIN_TRAVEL_TIME = 15;

		// Token: 0x040020AB RID: 8363
		public const int MAX_TRAVEL_TIME = 360;

		// Token: 0x040020AC RID: 8364
		public const int OVERFLOW_SLOT_COUNT = 10;

		// Token: 0x040020AD RID: 8365
		public const float CASH_REMINDER_THRESHOLD = 500f;

		// Token: 0x040020AE RID: 8366
		public const float RELATIONSHIP_CHANGE_PER_DEAL = 0.05f;

		// Token: 0x040020AF RID: 8367
		public static Action<Dealer> onDealerRecruited;

		// Token: 0x040020B0 RID: 8368
		public static Color32 DealerLabelColor = new Color32(120, 200, byte.MaxValue, byte.MaxValue);

		// Token: 0x040020B1 RID: 8369
		public static List<Dealer> AllDealers = new List<Dealer>();

		// Token: 0x040020B4 RID: 8372
		[Header("Debug")]
		public List<Customer> InitialCustomers = new List<Customer>();

		// Token: 0x040020B5 RID: 8373
		public List<ProductDefinition> InitialItems = new List<ProductDefinition>();

		// Token: 0x040020B6 RID: 8374
		[Header("Dealer References")]
		public NPCEnterableBuilding Home;

		// Token: 0x040020B7 RID: 8375
		public NPCSignal_HandleDeal DealSignal;

		// Token: 0x040020B8 RID: 8376
		public NPCEvent_StayInBuilding HomeEvent;

		// Token: 0x040020B9 RID: 8377
		public DialogueController_Dealer DialogueController;

		// Token: 0x040020BA RID: 8378
		[Header("Dialogue stuff")]
		public DialogueContainer RecruitDialogue;

		// Token: 0x040020BB RID: 8379
		public DialogueContainer CollectCashDialogue;

		// Token: 0x040020BC RID: 8380
		public DialogueContainer AssignCustomersDialogue;

		// Token: 0x040020BD RID: 8381
		[Header("Dealer Settings")]
		public string HomeName = "Home";

		// Token: 0x040020BE RID: 8382
		public float SigningFee = 500f;

		// Token: 0x040020BF RID: 8383
		public float Cut = 0.2f;

		// Token: 0x040020C0 RID: 8384
		public bool SellInsufficientQualityItems;

		// Token: 0x040020C1 RID: 8385
		public bool SellExcessQualityItems = true;

		// Token: 0x040020C2 RID: 8386
		[Header("Variables")]
		public string CompletedDealsVariable = string.Empty;

		// Token: 0x040020C4 RID: 8388
		public List<Customer> AssignedCustomers = new List<Customer>();

		// Token: 0x040020C5 RID: 8389
		public List<Contract> ActiveContracts = new List<Contract>();

		// Token: 0x040020C7 RID: 8391
		public UnityEvent onRecommended = new UnityEvent();

		// Token: 0x040020C8 RID: 8392
		protected ItemSlot[] OverflowSlots;

		// Token: 0x040020C9 RID: 8393
		private Contract currentContract;

		// Token: 0x040020CA RID: 8394
		private DialogueController.DialogueChoice recruitChoice;

		// Token: 0x040020CB RID: 8395
		private DialogueController.DialogueChoice collectCashChoice;

		// Token: 0x040020CC RID: 8396
		private DialogueController.DialogueChoice assignCustomersChoice;

		// Token: 0x040020CF RID: 8399
		[SyncVar]
		public List<string> acceptedContractGUIDs = new List<string>();

		// Token: 0x040020D0 RID: 8400
		private int itemCountOnTradeStart;

		// Token: 0x040020D1 RID: 8401
		public SyncVar<float> syncVar___<Cash>k__BackingField;

		// Token: 0x040020D2 RID: 8402
		public SyncVar<List<string>> syncVar___acceptedContractGUIDs;

		// Token: 0x040020D3 RID: 8403
		private bool dll_Excuted;

		// Token: 0x040020D4 RID: 8404
		private bool dll_Excuted;
	}
}
