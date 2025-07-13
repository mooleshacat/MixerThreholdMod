using System;
using System.Collections;
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
using ScheduleOne.Levelling;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Quests;
using ScheduleOne.Storage;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Delivery;
using ScheduleOne.UI.Phone.Messages;
using ScheduleOne.UI.Shop;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x020006B5 RID: 1717
	public class Supplier : NPC
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x000C4C21 File Offset: 0x000C2E21
		// (set) Token: 0x06002EF2 RID: 12018 RVA: 0x000C4C29 File Offset: 0x000C2E29
		public Supplier.ESupplierStatus Status { get; private set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x000C4C32 File Offset: 0x000C2E32
		// (set) Token: 0x06002EF4 RID: 12020 RVA: 0x000C4C3A File Offset: 0x000C2E3A
		public bool DeliveriesEnabled { get; private set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002EF5 RID: 12021 RVA: 0x000C4C43 File Offset: 0x000C2E43
		public float Debt
		{
			get
			{
				return this.SyncAccessor_debt;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x000C4C4B File Offset: 0x000C2E4B
		// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x000C4C53 File Offset: 0x000C2E53
		public int minsUntilDeaddropReady { get; private set; } = -1;

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000C4C5C File Offset: 0x000C2E5C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Economy.Supplier_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000C4C70 File Offset: 0x000C2E70
		protected override void Start()
		{
			base.Start();
			NPCRelationData relationData = this.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.SupplierUnlocked));
			NPCRelationData relationData2 = this.RelationData;
			relationData2.onRelationshipChange = (Action<float>)Delegate.Combine(relationData2.onRelationshipChange, new Action<float>(this.RelationshipChange));
			string orderCompleteDialogue = this.dialogueHandler.Database.GetLine(EDialogueModule.Generic, "meeting_order_complete");
			this.Shop.onOrderCompleted.AddListener(delegate()
			{
				this.dialogueHandler.ShowWorldspaceDialogue(orderCompleteDialogue, 3f);
			});
			this.dialogueController = this.dialogueHandler.GetComponent<DialogueController>();
			this.meetingGreeting = new DialogueController.GreetingOverride();
			this.meetingGreeting.Greeting = this.dialogueHandler.Database.GetLine(EDialogueModule.Generic, "supplier_meeting_greeting");
			this.meetingGreeting.PlayVO = true;
			this.meetingGreeting.VOType = EVOLineType.Question;
			this.dialogueController.AddGreetingOverride(this.meetingGreeting);
			this.meetingChoice = new DialogueController.DialogueChoice();
			this.meetingChoice.ChoiceText = "Yes";
			this.meetingChoice.onChoosen.AddListener(delegate()
			{
				this.Shop.SetIsOpen(true);
			});
			this.meetingChoice.Enabled = false;
			this.dialogueController.AddDialogueChoice(this.meetingChoice, 0);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onTimeSkip = (Action<int>)Delegate.Combine(instance.onTimeSkip, new Action<int>(this.OnTimeSkip));
			foreach (PhoneShopInterface.Listing listing in this.OnlineShopItems)
			{
				if (listing.Item.RequiresLevelToPurchase)
				{
					NetworkSingleton<LevelManager>.Instance.AddUnlockable(new Unlockable(listing.Item.RequiredRank, listing.Item.Name, listing.Item.Icon));
				}
			}
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onHourPass = (Action)Delegate.Remove(instance2.onHourPass, new Action(this.HourPass));
			TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
			instance3.onHourPass = (Action)Delegate.Combine(instance3.onHourPass, new Action(this.HourPass));
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000C4EA0 File Offset: 0x000C30A0
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			if (this.Status == Supplier.ESupplierStatus.Meeting)
			{
				this.MeetAtLocation(connection, SupplierLocation.AllLocations.IndexOf(this.currentLocation), 360);
			}
			if (this.DeliveriesEnabled)
			{
				this.EnableDeliveries(connection);
			}
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000C4EF1 File Offset: 0x000C30F1
		[ServerRpc(RequireOwnership = false)]
		public void SendUnlocked()
		{
			this.RpcWriter___Server_SendUnlocked_2166136261();
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x000C4EF9 File Offset: 0x000C30F9
		[ObserversRpc]
		private void SetUnlocked()
		{
			this.RpcWriter___Observers_SetUnlocked_2166136261();
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x000C4F04 File Offset: 0x000C3104
		protected override void MinPass()
		{
			base.MinPass();
			this.minsSinceDeaddropOrder++;
			if (this.Status == Supplier.ESupplierStatus.Meeting)
			{
				this.minsSinceMeetingStart++;
				this.minsSinceLastMeetingEnd = 0;
				if (this.minsSinceMeetingStart > 360)
				{
					this.EndMeeting();
				}
			}
			else
			{
				this.minsSinceLastMeetingEnd++;
			}
			if (InstanceFinder.IsServer)
			{
				if (this.SyncAccessor_deadDropPreparing)
				{
					this.minsUntilDeaddropReady--;
					if (this.minsUntilDeaddropReady <= 0)
					{
						this.CompleteDeaddrop();
					}
				}
				if (this.SyncAccessor_debt > 0f && !this.Stash.Storage.IsOpened && this.Stash.CashAmount > 1f && this.minsSinceDeaddropOrder > 3)
				{
					this.TryRecoverDebt();
				}
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000C4FD4 File Offset: 0x000C31D4
		protected void HourPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.repaymentReminderSent && this.SyncAccessor_debt > this.GetDeadDropLimit() * 0.5f && !this.SyncAccessor_deadDropPreparing)
			{
				float num = 0.020833334f;
				if (UnityEngine.Random.Range(0f, 1f) < num)
				{
					this.SendDebtReminder();
				}
			}
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000C502B File Offset: 0x000C322B
		private void OnTimeSkip(int minsSlept)
		{
			if (this.Status == Supplier.ESupplierStatus.Meeting)
			{
				this.minsSinceMeetingStart += minsSlept;
			}
			if (this.SyncAccessor_deadDropPreparing)
			{
				this.minsUntilDeaddropReady -= minsSlept;
			}
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000C505C File Offset: 0x000C325C
		[ObserversRpc(RunLocally = true)]
		public void MeetAtLocation(NetworkConnection conn, int locationIndex, int expireIn)
		{
			this.RpcWriter___Observers_MeetAtLocation_3470796954(conn, locationIndex, expireIn);
			this.RpcLogic___MeetAtLocation_3470796954(conn, locationIndex, expireIn);
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000C5090 File Offset: 0x000C3290
		public void EndMeeting()
		{
			Console.Log("Meeting ended", null);
			this.Status = Supplier.ESupplierStatus.Idle;
			this.minsSinceMeetingStart = -1;
			this.meetingGreeting.ShouldShow = false;
			this.meetingChoice.Enabled = false;
			this.currentLocation.SetActiveSupplier(null);
			this.SetVisible(false);
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000C50E1 File Offset: 0x000C32E1
		protected virtual void SupplierUnlocked(NPCRelationData.EUnlockType type, bool notify)
		{
			if (notify)
			{
				this.SetUnlockMessage();
			}
			base.StartCoroutine(this.<SupplierUnlocked>g__WaitForPlayer|52_0());
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000C50FC File Offset: 0x000C32FC
		protected virtual void RelationshipChange(float change)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				if (this.RelationData.RelationDelta >= 5f && !this.DeliveriesEnabled)
				{
					this.EnableDeliveries(null);
				}
				return;
			}
			float num = this.RelationData.RelationDelta - change;
			float relationDelta = this.RelationData.RelationDelta;
			if (num < 4f && relationDelta >= 4f)
			{
				Console.Log("Supplier relationship high enough for meetings", null);
				DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "supplier_meetings_unlocked");
				if (chain == null)
				{
					return;
				}
				base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 3f, true, true);
			}
			if (relationDelta >= 5f && !this.DeliveriesEnabled)
			{
				Console.Log("Supplier relationship high enough for deliveries", null);
				this.EnableDeliveries(null);
				DialogueChain chain2 = this.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "supplier_deliveries_unlocked");
				if (chain2 != null)
				{
					base.MSGConversation.SendMessageChain(chain2.GetMessageChain(), 3f, true, true);
				}
			}
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000C51F9 File Offset: 0x000C33F9
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void EnableDeliveries(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_EnableDeliveries_328543758(conn);
				this.RpcLogic___EnableDeliveries_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_EnableDeliveries_328543758(conn);
			}
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000C5224 File Offset: 0x000C3424
		public void SetUnlockMessage()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.RelationData.Unlocked)
			{
				this.RelationData.Unlock(NPCRelationData.EUnlockType.Recommendation, false);
			}
			DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "supplier_unlocked");
			if (chain == null)
			{
				return;
			}
			base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 0f, true, true);
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000C5288 File Offset: 0x000C3488
		protected override void CreateMessageConversation()
		{
			base.CreateMessageConversation();
			SendableMessage sendableMessage = base.MSGConversation.CreateSendableMessage("I need to order a dead drop");
			sendableMessage.IsValidCheck = new SendableMessage.ValidityCheck(this.IsDeadDropValid);
			sendableMessage.disableDefaultSendBehaviour = true;
			sendableMessage.onSelected = (Action)Delegate.Combine(sendableMessage.onSelected, new Action(this.DeaddropRequested));
			SendableMessage sendableMessage2 = base.MSGConversation.CreateSendableMessage("We need to meet up");
			sendableMessage2.IsValidCheck = new SendableMessage.ValidityCheck(this.IsMeetupValid);
			sendableMessage2.onSent = (Action)Delegate.Combine(sendableMessage2.onSent, new Action(this.MeetupRequested));
			SendableMessage sendableMessage3 = base.MSGConversation.CreateSendableMessage("I want to pay off my debt");
			sendableMessage3.onSent = (Action)Delegate.Combine(sendableMessage3.onSent, new Action(this.PayDebtRequested));
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000C535C File Offset: 0x000C355C
		protected virtual void DeaddropRequested()
		{
			float orderLimit = Mathf.Max(this.GetDeadDropLimit() - this.SyncAccessor_debt, 0f);
			PlayerSingleton<MessagesApp>.Instance.PhoneShopInterface.Open("Request Dead Drop", "Select items to order from " + this.FirstName, base.MSGConversation, this.OnlineShopItems.ToList<PhoneShopInterface.Listing>(), orderLimit, this.SyncAccessor_debt, new Action<List<PhoneShopInterface.CartEntry>, float>(this.DeaddropConfirmed));
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000C53CC File Offset: 0x000C35CC
		protected virtual void DeaddropConfirmed(List<PhoneShopInterface.CartEntry> cart, float totalPrice)
		{
			if (this.SyncAccessor_deadDropPreparing)
			{
				Console.LogWarning("Already preparing a dead drop", null);
				return;
			}
			int num = cart.Sum((PhoneShopInterface.CartEntry x) => x.Quantity);
			StringIntPair[] array = new StringIntPair[cart.Count];
			for (int i = 0; i < cart.Count; i++)
			{
				array[i] = new StringIntPair(cart[i].Listing.Item.ID, cart[i].Quantity);
			}
			string text = "I need a dead drop:\n";
			for (int j = 0; j < cart.Count; j++)
			{
				if (cart[j].Quantity > 0)
				{
					text = text + cart[j].Quantity.ToString() + "x " + cart[j].Listing.Item.Name;
					if (j < cart.Count - 1)
					{
						text += "\n";
					}
				}
			}
			base.MSGConversation.SendMessage(new Message(text, Message.ESenderType.Player, false, -1), true, true);
			int num2 = Mathf.Clamp(num * 30, 30, 360);
			string text2 = this.dialogueHandler.Database.GetLine(EDialogueModule.Supplier, "deaddrop_requested");
			if (num2 < 60)
			{
				text2 = text2.Replace("<TIME>", num2.ToString() + ((num2 == 1) ? " min" : " mins"));
			}
			else
			{
				float num3 = (float)Mathf.FloorToInt((float)num2 / 60f);
				float num4 = (float)num2 - num3 * 60f;
				string text3 = num3.ToString() + ((num3 == 1f) ? " hour" : " hours");
				if (num4 > 0f)
				{
					text3 = text3 + " " + num4.ToString() + " min";
				}
				text2 = text2.Replace("<TIME>", text3);
			}
			base.MSGConversation.SendMessageChain(new MessageChain
			{
				Messages = new List<string>
				{
					text2
				},
				id = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
			}, 0.5f, false, true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Deaddrops_Ordered", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Deaddrops_Ordered") + 1f).ToString(), true);
			this.SetDeaddrop(array, num2);
			this.minsSinceDeaddropOrder = 0;
			this.ChangeDebt(totalPrice);
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000C563F File Offset: 0x000C383F
		[ServerRpc(RequireOwnership = false)]
		private void SetDeaddrop(StringIntPair[] items, int minsUntilReady)
		{
			this.RpcWriter___Server_SetDeaddrop_3971994486(items, minsUntilReady);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000C564F File Offset: 0x000C384F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void ChangeDebt(float amount)
		{
			this.RpcWriter___Server_ChangeDebt_431000436(amount);
			this.RpcLogic___ChangeDebt_431000436(amount);
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000C5668 File Offset: 0x000C3868
		private void TryRecoverDebt()
		{
			float num = Mathf.Min(this.SyncAccessor_debt, this.Stash.CashAmount);
			if (num > 0f)
			{
				Debug.Log("Recovering debt: " + num.ToString());
				float num2 = this.SyncAccessor_debt;
				this.Stash.RemoveCash(num);
				this.ChangeDebt(-num);
				this.RelationData.ChangeRelationship(num / this.MaxOrderLimit * 0.5f, true);
				float num3 = num2 - num;
				string text = "I've received " + MoneyManager.FormatAmount(num, false, false) + " cash from you.";
				if (num3 <= 0f)
				{
					text += " Your debt is now paid off.";
				}
				else
				{
					text = text + " Your debt is now " + MoneyManager.FormatAmount(num3, false, false);
				}
				this.repaymentReminderSent = false;
				base.MSGConversation.SendMessageChain(new MessageChain
				{
					Messages = new List<string>
					{
						text
					},
					id = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
				}, 0f, true, true);
			}
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000C576C File Offset: 0x000C396C
		private void CompleteDeaddrop()
		{
			Console.Log("Dead drop ready", null);
			DeadDrop randomEmptyDrop = DeadDrop.GetRandomEmptyDrop(Player.Local.transform.position);
			if (randomEmptyDrop == null)
			{
				Console.LogError("No empty dead drop locations", null);
				return;
			}
			foreach (StringIntPair stringIntPair in this.deaddropItems)
			{
				ItemDefinition item = Registry.GetItem(stringIntPair.String);
				if (item == null)
				{
					Console.LogError("Item not found: " + stringIntPair.String, null);
				}
				else
				{
					int num;
					for (int j = stringIntPair.Int; j > 0; j -= num)
					{
						num = Mathf.Min(j, item.StackLimit);
						ItemInstance defaultInstance = item.GetDefaultInstance(num);
						randomEmptyDrop.Storage.InsertItem(defaultInstance, true);
					}
				}
			}
			string text = this.dialogueHandler.Database.GetLine(EDialogueModule.Supplier, "deaddrop_ready");
			text = text.Replace("<LOCATION>", randomEmptyDrop.DeadDropDescription);
			base.MSGConversation.SendMessageChain(new MessageChain
			{
				Messages = new List<string>
				{
					text
				},
				id = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
			}, 0f, true, true);
			this.sync___set_value_deadDropPreparing(false, true);
			this.minsUntilDeaddropReady = -1;
			this.deaddropItems = null;
			if (this.onDeaddropReady != null)
			{
				this.onDeaddropReady.Invoke();
			}
			string guidString = GUIDManager.GenerateUniqueGUID().ToString();
			NetworkSingleton<QuestManager>.Instance.CreateDeaddropCollectionQuest(null, randomEmptyDrop.GUID.ToString(), guidString);
			this.SetDeaddrop(null, -1);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000C5910 File Offset: 0x000C3B10
		private void SendDebtReminder()
		{
			this.repaymentReminderSent = true;
			DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Supplier, "supplier_request_repayment");
			chain.Lines[0] = chain.Lines[0].Replace("<DEBT>", "<color=#46CB4F>" + MoneyManager.FormatAmount(this.SyncAccessor_debt, false, false) + "</color>");
			base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 0f, true, true);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000C598C File Offset: 0x000C3B8C
		protected virtual void MeetupRequested()
		{
			if (InstanceFinder.IsServer)
			{
				int locationIndex;
				SupplierLocation appropriateLocation = this.GetAppropriateLocation(out locationIndex);
				string text = this.dialogueHandler.Database.GetLine(EDialogueModule.Generic, "supplier_meet_confirm");
				text = text.Replace("<LOCATION>", appropriateLocation.LocationDescription);
				MessageChain messageChain = new MessageChain();
				messageChain.Messages.Add(text);
				messageChain.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				base.MSGConversation.SendMessageChain(messageChain, 0.5f, true, true);
				this.MeetAtLocation(null, locationIndex, 360);
			}
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000C5A1C File Offset: 0x000C3C1C
		protected virtual void PayDebtRequested()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			MessageChain messageChain = new MessageChain();
			messageChain.Messages.Add("You can pay off your debt by placing cash in my stash. It's " + this.Stash.locationDescription + ".");
			messageChain.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			base.MSGConversation.SendMessageChain(messageChain, 0.5f, true, true);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000C5A84 File Offset: 0x000C3C84
		protected SupplierLocation GetAppropriateLocation(out int locationIndex)
		{
			locationIndex = -1;
			List<SupplierLocation> list = new List<SupplierLocation>();
			list.AddRange(SupplierLocation.AllLocations);
			foreach (SupplierLocation supplierLocation in SupplierLocation.AllLocations)
			{
				if (supplierLocation.IsOccupied)
				{
					list.Remove(supplierLocation);
				}
			}
			foreach (SupplierLocation supplierLocation2 in SupplierLocation.AllLocations)
			{
				foreach (Player player in Player.PlayerList)
				{
					if (Vector3.Distance(supplierLocation2.transform.position, player.Avatar.CenterPoint) < 30f)
					{
						list.Remove(supplierLocation2);
					}
				}
			}
			if (list.Count == 0)
			{
				Console.LogError("No available locations for supplier", null);
				return null;
			}
			SupplierLocation supplierLocation3 = list[UnityEngine.Random.Range(0, list.Count)];
			locationIndex = SupplierLocation.AllLocations.IndexOf(supplierLocation3);
			return supplierLocation3;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000C5BD0 File Offset: 0x000C3DD0
		private bool IsDeadDropValid(SendableMessage message, out string invalidReason)
		{
			invalidReason = string.Empty;
			if (this.SyncAccessor_deadDropPreparing)
			{
				invalidReason = "Already waiting for a dead drop";
				return false;
			}
			return true;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000C5BEB File Offset: 0x000C3DEB
		private bool IsMeetupValid(SendableMessage message, out string invalidReason)
		{
			if (this.RelationData.RelationDelta < 4f)
			{
				invalidReason = "Insufficient trust";
				return false;
			}
			if (this.Status != Supplier.ESupplierStatus.Idle)
			{
				invalidReason = "Busy";
				return false;
			}
			invalidReason = "";
			return true;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000C5C21 File Offset: 0x000C3E21
		public virtual float GetDeadDropLimit()
		{
			return Mathf.Lerp(this.MinOrderLimit, this.MaxOrderLimit, this.RelationData.RelationDelta / 5f);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000C5C45 File Offset: 0x000C3E45
		public override NPCData GetNPCData()
		{
			return new SupplierData(this.ID, this.minsSinceMeetingStart, this.minsSinceLastMeetingEnd, this.SyncAccessor_debt, this.minsUntilDeaddropReady, this.deaddropItems, this.repaymentReminderSent);
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000C5C78 File Offset: 0x000C3E78
		public override void Load(NPCData data, string containerPath)
		{
			base.Load(data, containerPath);
			string text;
			if (((ISaveable)this).TryLoadFile(containerPath, "NPC", out text))
			{
				SupplierData supplierData = null;
				try
				{
					supplierData = JsonUtility.FromJson<SupplierData>(text);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize character data: " + ex.Message, null);
					return;
				}
				this.minsSinceMeetingStart = supplierData.timeSinceMeetingStart;
				this.minsSinceLastMeetingEnd = supplierData.timeSinceLastMeetingEnd;
				this.sync___set_value_debt(supplierData.debt, true);
				this.minsUntilDeaddropReady = supplierData.minsUntilDeadDropReady;
				if (this.minsUntilDeaddropReady > 0)
				{
					this.sync___set_value_deadDropPreparing(true, true);
				}
				if (supplierData.deaddropItems != null)
				{
					this.deaddropItems = supplierData.deaddropItems.ToArray<StringIntPair>();
				}
				this.repaymentReminderSent = supplierData.debtReminderSent;
			}
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000C5D3C File Offset: 0x000C3F3C
		public override void Load(DynamicSaveData dynamicData, NPCData npcData)
		{
			base.Load(dynamicData, npcData);
			SupplierData supplierData;
			if (dynamicData.TryExtractBaseData<SupplierData>(out supplierData))
			{
				this.minsSinceMeetingStart = supplierData.timeSinceMeetingStart;
				this.minsSinceLastMeetingEnd = supplierData.timeSinceLastMeetingEnd;
				this.sync___set_value_debt(supplierData.debt, true);
				this.minsUntilDeaddropReady = supplierData.minsUntilDeadDropReady;
				if (this.minsUntilDeaddropReady > 0)
				{
					this.sync___set_value_deadDropPreparing(true, true);
				}
				if (supplierData.deaddropItems != null)
				{
					this.deaddropItems = supplierData.deaddropItems.ToArray<StringIntPair>();
				}
				this.repaymentReminderSent = supplierData.debtReminderSent;
			}
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000C5E3C File Offset: 0x000C403C
		[CompilerGenerated]
		private IEnumerator <SupplierUnlocked>g__WaitForPlayer|52_0()
		{
			yield return new WaitUntil(() => Player.Local != null);
			base.MSGConversation.EnsureUIExists();
			yield break;
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000C5E4B File Offset: 0x000C404B
		[CompilerGenerated]
		private IEnumerator <EnableDeliveries>g__Wait|54_0()
		{
			yield return new WaitUntil(() => PlayerSingleton<DeliveryApp>.InstanceExists);
			PlayerSingleton<DeliveryApp>.Instance.GetShop(this.Shop).SetIsAvailable();
			yield break;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000C5E5C File Offset: 0x000C405C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___deadDropPreparing = new SyncVar<bool>(this, 2U, 0, 0, -1f, 0, this.deadDropPreparing);
			this.syncVar___debt = new SyncVar<float>(this, 1U, 0, 0, -1f, 0, this.debt);
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_SendUnlocked_2166136261));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_SetUnlocked_2166136261));
			base.RegisterObserversRpc(37U, new ClientRpcDelegate(this.RpcReader___Observers_MeetAtLocation_3470796954));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_EnableDeliveries_328543758));
			base.RegisterTargetRpc(39U, new ClientRpcDelegate(this.RpcReader___Target_EnableDeliveries_328543758));
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SetDeaddrop_3971994486));
			base.RegisterServerRpc(41U, new ServerRpcDelegate(this.RpcReader___Server_ChangeDebt_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Economy.Supplier));
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000C5F89 File Offset: 0x000C4189
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___deadDropPreparing.SetRegistered();
			this.syncVar___debt.SetRegistered();
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000C5FB8 File Offset: 0x000C41B8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000C5FC8 File Offset: 0x000C41C8
		private void RpcWriter___Server_SendUnlocked_2166136261()
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

		// Token: 0x06002F1F RID: 12063 RVA: 0x000C6062 File Offset: 0x000C4262
		public void RpcLogic___SendUnlocked_2166136261()
		{
			this.SetUnlocked();
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000C606C File Offset: 0x000C426C
		private void RpcReader___Server_SendUnlocked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendUnlocked_2166136261();
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000C608C File Offset: 0x000C428C
		private void RpcWriter___Observers_SetUnlocked_2166136261()
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

		// Token: 0x06002F22 RID: 12066 RVA: 0x000C6135 File Offset: 0x000C4335
		private void RpcLogic___SetUnlocked_2166136261()
		{
			this.RelationData.Unlock(NPCRelationData.EUnlockType.Recommendation, true);
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000C6144 File Offset: 0x000C4344
		private void RpcReader___Observers_SetUnlocked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetUnlocked_2166136261();
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000C6164 File Offset: 0x000C4364
		private void RpcWriter___Observers_MeetAtLocation_3470796954(NetworkConnection conn, int locationIndex, int expireIn)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(locationIndex, 1);
			writer.WriteInt32(expireIn, 1);
			base.SendObserversRpc(37U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000C6240 File Offset: 0x000C4440
		public void RpcLogic___MeetAtLocation_3470796954(NetworkConnection conn, int locationIndex, int expireIn)
		{
			SupplierLocation supplierLocation = SupplierLocation.AllLocations[locationIndex];
			if (supplierLocation == null)
			{
				Console.LogError("Location not found: " + locationIndex.ToString(), null);
				return;
			}
			if (supplierLocation.SupplierStandPoint == null)
			{
				Console.LogError("Supplier stand point not set up for location: " + supplierLocation.name, null);
				return;
			}
			if (this.meetingGreeting == null || this.meetingChoice == null)
			{
				Console.LogError("Meeting greeting or choice not set up", null);
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				base.fullName,
				" meeting at ",
				supplierLocation.name,
				" for ",
				expireIn.ToString(),
				" minutes"
			}), null);
			this.Status = Supplier.ESupplierStatus.Meeting;
			this.currentLocation = supplierLocation;
			this.minsSinceMeetingStart = 0;
			supplierLocation.SetActiveSupplier(this);
			ShopInterface shop = this.Shop;
			StorageEntity[] deliveryBays = supplierLocation.DeliveryBays;
			shop.DeliveryBays = deliveryBays;
			this.meetingGreeting.ShouldShow = true;
			this.meetingChoice.Enabled = true;
			this.movement.Warp(supplierLocation.SupplierStandPoint.position);
			this.movement.FaceDirection(supplierLocation.SupplierStandPoint.forward, 0.5f);
			this.SetVisible(true);
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000C6380 File Offset: 0x000C4580
		private void RpcReader___Observers_MeetAtLocation_3470796954(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			int locationIndex = PooledReader0.ReadInt32(1);
			int expireIn = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___MeetAtLocation_3470796954(conn, locationIndex, expireIn);
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000C63E8 File Offset: 0x000C45E8
		private void RpcWriter___Observers_EnableDeliveries_328543758(NetworkConnection conn)
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

		// Token: 0x06002F28 RID: 12072 RVA: 0x000C6491 File Offset: 0x000C4691
		private void RpcLogic___EnableDeliveries_328543758(NetworkConnection conn)
		{
			this.DeliveriesEnabled = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<EnableDeliveries>g__Wait|54_0());
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000C64AC File Offset: 0x000C46AC
		private void RpcReader___Observers_EnableDeliveries_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnableDeliveries_328543758(null);
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000C64D8 File Offset: 0x000C46D8
		private void RpcWriter___Target_EnableDeliveries_328543758(NetworkConnection conn)
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

		// Token: 0x06002F2B RID: 12075 RVA: 0x000C6580 File Offset: 0x000C4780
		private void RpcReader___Target_EnableDeliveries_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___EnableDeliveries_328543758(base.LocalConnection);
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000C65A8 File Offset: 0x000C47A8
		private void RpcWriter___Server_SetDeaddrop_3971994486(StringIntPair[] items, int minsUntilReady)
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
			writer.Write___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generated(items);
			writer.WriteInt32(minsUntilReady, 1);
			base.SendServerRpc(40U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000C6661 File Offset: 0x000C4861
		private void RpcLogic___SetDeaddrop_3971994486(StringIntPair[] items, int minsUntilReady)
		{
			if (items != null)
			{
				this.minsSinceDeaddropOrder = 0;
				this.sync___set_value_deadDropPreparing(true, true);
			}
			else
			{
				this.sync___set_value_deadDropPreparing(false, true);
			}
			this.minsUntilDeaddropReady = minsUntilReady;
			this.deaddropItems = items;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000C6690 File Offset: 0x000C4890
		private void RpcReader___Server_SetDeaddrop_3971994486(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			StringIntPair[] items = GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generateds(PooledReader0);
			int minsUntilReady = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetDeaddrop_3971994486(items, minsUntilReady);
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000C66D8 File Offset: 0x000C48D8
		private void RpcWriter___Server_ChangeDebt_431000436(float amount)
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
			writer.WriteSingle(amount, 0);
			base.SendServerRpc(41U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000C6784 File Offset: 0x000C4984
		private void RpcLogic___ChangeDebt_431000436(float amount)
		{
			this.sync___set_value_debt(Mathf.Clamp(this.SyncAccessor_debt + amount, 0f, this.GetDeadDropLimit()), true);
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000C67A8 File Offset: 0x000C49A8
		private void RpcReader___Server_ChangeDebt_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float amount = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ChangeDebt_431000436(amount);
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002F32 RID: 12082 RVA: 0x000C67EB File Offset: 0x000C49EB
		// (set) Token: 0x06002F33 RID: 12083 RVA: 0x000C67F3 File Offset: 0x000C49F3
		public float SyncAccessor_debt
		{
			get
			{
				return this.debt;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.debt = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___debt.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000C6830 File Offset: 0x000C4A30
		public override bool Supplier(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_deadDropPreparing(this.syncVar___deadDropPreparing.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_deadDropPreparing(value, Boolean2);
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
					this.sync___set_value_debt(this.syncVar___debt.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(0);
				this.sync___set_value_debt(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002F35 RID: 12085 RVA: 0x000C68CB File Offset: 0x000C4ACB
		// (set) Token: 0x06002F36 RID: 12086 RVA: 0x000C68D3 File Offset: 0x000C4AD3
		public bool SyncAccessor_deadDropPreparing
		{
			get
			{
				return this.deadDropPreparing;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.deadDropPreparing = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___deadDropPreparing.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000C690F File Offset: 0x000C4B0F
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x0400210D RID: 8461
		public const float MEETUP_RELATIONSHIP_REQUIREMENT = 4f;

		// Token: 0x0400210E RID: 8462
		public const int MEETUP_DURATION_MINS = 360;

		// Token: 0x0400210F RID: 8463
		public const int MEETING_COOLDOWN_MINS = 720;

		// Token: 0x04002110 RID: 8464
		public const int DEADDROP_WAIT_PER_ITEM = 30;

		// Token: 0x04002111 RID: 8465
		public const int DEADDROP_MAX_WAIT = 360;

		// Token: 0x04002112 RID: 8466
		public const int DEADDROP_ITEM_LIMIT = 10;

		// Token: 0x04002113 RID: 8467
		public const float DELIVERY_RELATIONSHIP_REQUIREMENT = 5f;

		// Token: 0x04002114 RID: 8468
		public static Color32 SupplierLabelColor = new Color32(byte.MaxValue, 150, 145, byte.MaxValue);

		// Token: 0x04002117 RID: 8471
		[Header("Supplier Settings")]
		public float MinOrderLimit = 100f;

		// Token: 0x04002118 RID: 8472
		public float MaxOrderLimit = 500f;

		// Token: 0x04002119 RID: 8473
		public PhoneShopInterface.Listing[] OnlineShopItems;

		// Token: 0x0400211A RID: 8474
		[TextArea(3, 10)]
		public string SupplierRecommendMessage = "My friend <NAME> can hook you up with <PRODUCT>. I've passed your number on to them.";

		// Token: 0x0400211B RID: 8475
		[TextArea(3, 10)]
		public string SupplierUnlockHint = "You can now order <PRODUCT> from <NAME>. <PRODUCT> can be used to <PURPOSE>.";

		// Token: 0x0400211C RID: 8476
		[Header("References")]
		public ShopInterface Shop;

		// Token: 0x0400211D RID: 8477
		public SupplierStash Stash;

		// Token: 0x0400211E RID: 8478
		public UnityEvent onDeaddropReady;

		// Token: 0x0400211F RID: 8479
		private int minsSinceMeetingStart = -1;

		// Token: 0x04002120 RID: 8480
		private int minsSinceLastMeetingEnd = 720;

		// Token: 0x04002121 RID: 8481
		private SupplierLocation currentLocation;

		// Token: 0x04002122 RID: 8482
		private DialogueController dialogueController;

		// Token: 0x04002123 RID: 8483
		private DialogueController.GreetingOverride meetingGreeting;

		// Token: 0x04002124 RID: 8484
		private DialogueController.DialogueChoice meetingChoice;

		// Token: 0x04002125 RID: 8485
		[SyncVar]
		public float debt;

		// Token: 0x04002126 RID: 8486
		[SyncVar]
		public bool deadDropPreparing;

		// Token: 0x04002128 RID: 8488
		private StringIntPair[] deaddropItems;

		// Token: 0x04002129 RID: 8489
		private int minsSinceDeaddropOrder;

		// Token: 0x0400212A RID: 8490
		private bool repaymentReminderSent;

		// Token: 0x0400212B RID: 8491
		public SyncVar<float> syncVar___debt;

		// Token: 0x0400212C RID: 8492
		public SyncVar<bool> syncVar___deadDropPreparing;

		// Token: 0x0400212D RID: 8493
		private bool dll_Excuted;

		// Token: 0x0400212E RID: 8494
		private bool dll_Excuted;

		// Token: 0x020006B6 RID: 1718
		public enum ESupplierStatus
		{
			// Token: 0x04002130 RID: 8496
			Idle,
			// Token: 0x04002131 RID: 8497
			PreppingDeadDrop,
			// Token: 0x04002132 RID: 8498
			Meeting
		}
	}
}
