using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Datas.Characters;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI.Handover;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000514 RID: 1300
	public class Thomas : NPC
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x00077513 File Offset: 0x00075713
		// (set) Token: 0x06001C9B RID: 7323 RVA: 0x0007751B File Offset: 0x0007571B
		public bool MeetingReminderSent { get; protected set; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00077524 File Offset: 0x00075724
		// (set) Token: 0x06001C9D RID: 7325 RVA: 0x0007752C File Offset: 0x0007572C
		public bool HandoverReminderSent { get; protected set; }

		// Token: 0x06001C9E RID: 7326 RVA: 0x00077535 File Offset: 0x00075735
		protected override void Start()
		{
			base.Start();
			this.dialogueHandler.onDialogueChoiceChosen.AddListener(new UnityAction<string>(this.DialogueChoiceCallback));
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00077559 File Offset: 0x00075759
		public void SetFirstMeetingEventActive(bool active)
		{
			this.FirstMeetingEvent.gameObject.SetActive(active);
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0007756C File Offset: 0x0007576C
		public void SetHandoverEventActive(bool active)
		{
			this.HandoverEvent.gameObject.SetActive(active);
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00077580 File Offset: 0x00075780
		public void SendMeetingReminder()
		{
			if (this.MeetingReminderSent)
			{
				Console.LogWarning("Reminder message already sent", null);
				return;
			}
			this.MeetingReminderSent = true;
			base.HasChanged = true;
			Message message = new Message();
			message.text = "Either you haven't read our note or are choosing to ignore it - for your sake I'll assume the former. We have business to discuss at Hyland Manor ASAP. - TB";
			message.sender = Message.ESenderType.Other;
			message.endOfGroup = true;
			base.MSGConversation.SetIsKnown(false);
			base.MSGConversation.SendMessage(message, true, true);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000775E8 File Offset: 0x000757E8
		public void SendHandoverReminder()
		{
			if (this.HandoverReminderSent)
			{
				Console.LogWarning("Reminder message already sent", null);
				return;
			}
			Debug.Log("Sending reminder");
			this.HandoverReminderSent = true;
			base.HasChanged = true;
			Message message = new Message();
			message.text = "You haven't yet made this week's delivery. There are 24 hours left. Don't make this difficult. - TB";
			message.sender = Message.ESenderType.Other;
			message.endOfGroup = true;
			base.MSGConversation.SendMessage(message, true, true);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0007764E File Offset: 0x0007584E
		public void InitialMeetingComplete()
		{
			base.MSGConversation.SetIsKnown(true);
			this.SetFirstMeetingEventActive(false);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00077664 File Offset: 0x00075864
		private void DialogueChoiceCallback(string choiceLabel)
		{
			if (choiceLabel == "BEGIN_HANDOVER")
			{
				ProductList productList = new ProductList();
				productList.entries.Add(new ProductList.Entry
				{
					ProductID = "ogkush",
					Quantity = 15,
					Quality = EQuality.Trash
				});
				Contract contract = new GameObject("CartelContract").AddComponent<Contract>();
				contract.transform.SetParent(base.transform);
				contract.SilentlyInitializeContract("Cartel Contract", "Deliver the goods to the cartel", new QuestEntryData[0], string.Empty, base.NetworkObject, 100f, productList, string.Empty, new QuestWindowConfig(), 0, NetworkSingleton<TimeManager>.Instance.GetDateTime());
				Singleton<HandoverScreen>.Instance.Open(contract, base.GetComponent<Customer>(), HandoverScreen.EMode.Contract, new Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float>(this.ProcessItemHandover), null);
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00077730 File Offset: 0x00075930
		private void ProcessItemHandover(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float price)
		{
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				return;
			}
			Singleton<HandoverScreen>.Instance.ClearCustomerSlots(false);
			this.SetHandoverEventActive(false);
			this.HandoverReminderSent = false;
			base.HasChanged = true;
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(100f, true, false);
			if (this.onCartelContractReceived != null)
			{
				this.onCartelContractReceived.Invoke();
			}
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00077788 File Offset: 0x00075988
		public override void Load(NPCData data, string containerPath)
		{
			base.Load(data, containerPath);
			string text;
			if (((ISaveable)this).TryLoadFile(containerPath, "NPC", out text))
			{
				ThomasData thomasData = null;
				try
				{
					thomasData = JsonUtility.FromJson<ThomasData>(text);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize character data: " + ex.Message, null);
					return;
				}
				this.MeetingReminderSent = thomasData.MeetingReminderSent;
				this.HandoverReminderSent = thomasData.HandoverReminderSent;
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000777FC File Offset: 0x000759FC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00077815 File Offset: 0x00075A15
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0007782E File Offset: 0x00075A2E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0007783C File Offset: 0x00075A3C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001798 RID: 6040
		public const int CARTEL_CONTRACT_QUANTITY = 15;

		// Token: 0x04001799 RID: 6041
		public const float CARTEL_CONTRACT_PAYMENT = 100f;

		// Token: 0x0400179A RID: 6042
		public NPCEvent_LocationDialogue FirstMeetingEvent;

		// Token: 0x0400179B RID: 6043
		public NPCEvent_LocationDialogue HandoverEvent;

		// Token: 0x0400179C RID: 6044
		public UnityEvent onCartelContractReceived;

		// Token: 0x0400179F RID: 6047
		private bool dll_Excuted;

		// Token: 0x040017A0 RID: 6048
		private bool dll_Excuted;
	}
}
