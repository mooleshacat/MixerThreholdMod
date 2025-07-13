using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006AC RID: 1708
	[Serializable]
	public class DealGenerationEvent
	{
		// Token: 0x06002ED5 RID: 11989 RVA: 0x000C4704 File Offset: 0x000C2904
		public ContractInfo GenerateContractInfo(Customer customer)
		{
			return new ContractInfo(this.Payment, this.ProductList, this.DeliveryLocation.GUID.ToString(), this.DeliveryWindow, this.Expires, this.ExpiresAfter, this.PickupScheduleGroup, false);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000C4754 File Offset: 0x000C2954
		public bool ShouldGenerate(Customer customer)
		{
			if (customer.NPC.RelationData.RelationDelta < this.RelationshipRequirement)
			{
				return false;
			}
			return this.ApplicableDays.Exists((DealGenerationEvent.DayContainer x) => x.Day == NetworkSingleton<TimeManager>.Instance.CurrentDay) && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.GenerationTime, TimeManager.AddMinutesTo24HourTime(this.GenerationTime, this.GenerationWindowDuration));
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000C47CF File Offset: 0x000C29CF
		public MessageChain GetRandomRequestMessage()
		{
			return this.ProcessMessage(this.RequestMessageChains[UnityEngine.Random.Range(0, this.RequestMessageChains.Length - 1)]);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000C47F0 File Offset: 0x000C29F0
		public MessageChain ProcessMessage(MessageChain messageChain)
		{
			MessageChain messageChain2 = new MessageChain();
			foreach (string text in messageChain.Messages)
			{
				string text2 = text.Replace("<PRICE>", "<color=#46CB4F>" + MoneyManager.FormatAmount(this.Payment, false, false) + "</color>");
				text2 = text2.Replace("<PRODUCT>", this.GetProductStringList());
				text2 = text2.Replace("<QUALITY>", this.GetQualityString());
				text2 = text2.Replace("<LOCATION>", this.DeliveryLocation.GetDescription());
				text2 = text2.Replace("<WINDOW_START>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowStartTime, true));
				text2 = text2.Replace("<WINDOW_END>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowEndTime, true));
				messageChain2.Messages.Add(text2);
			}
			return messageChain2;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000C48F4 File Offset: 0x000C2AF4
		public MessageChain GetRejectionMessage()
		{
			return this.ProcessMessage(this.ContractRejectedResponses[UnityEngine.Random.Range(0, this.ContractRejectedResponses.Length - 1)]);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000C4913 File Offset: 0x000C2B13
		public MessageChain GetAcceptanceMessage()
		{
			return this.ProcessMessage(this.ContractAcceptedResponses[UnityEngine.Random.Range(0, this.ContractAcceptedResponses.Length - 1)]);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000C4932 File Offset: 0x000C2B32
		public string GetProductStringList()
		{
			return this.ProductList.GetCommaSeperatedString();
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000C493F File Offset: 0x000C2B3F
		public string GetQualityString()
		{
			return this.ProductList.GetQualityString();
		}

		// Token: 0x040020DE RID: 8414
		[Header("Settings")]
		public bool Enabled = true;

		// Token: 0x040020DF RID: 8415
		public bool CanBeAccepted = true;

		// Token: 0x040020E0 RID: 8416
		public bool CanBeRejected = true;

		// Token: 0x040020E1 RID: 8417
		[Header("Timing Settings")]
		public List<DealGenerationEvent.DayContainer> ApplicableDays = new List<DealGenerationEvent.DayContainer>();

		// Token: 0x040020E2 RID: 8418
		public int GenerationTime;

		// Token: 0x040020E3 RID: 8419
		public int GenerationWindowDuration = 60;

		// Token: 0x040020E4 RID: 8420
		[Header("Products and payment")]
		public ProductList ProductList;

		// Token: 0x040020E5 RID: 8421
		public float Payment = 100f;

		// Token: 0x040020E6 RID: 8422
		[Range(0f, 5f)]
		public float RelationshipRequirement = 1f;

		// Token: 0x040020E7 RID: 8423
		[Header("Messages")]
		[SerializeField]
		private MessageChain[] RequestMessageChains;

		// Token: 0x040020E8 RID: 8424
		public MessageChain[] ContractAcceptedResponses;

		// Token: 0x040020E9 RID: 8425
		public MessageChain[] ContractRejectedResponses;

		// Token: 0x040020EA RID: 8426
		[Header("Location settings")]
		public DeliveryLocation DeliveryLocation;

		// Token: 0x040020EB RID: 8427
		public int PickupScheduleGroup;

		// Token: 0x040020EC RID: 8428
		[Header("Window/expiry settings")]
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x040020ED RID: 8429
		public bool Expires = true;

		// Token: 0x040020EE RID: 8430
		[Tooltip("How many days after being accepted does this contract expire? Exact expiry is adjusted to match window end")]
		[Range(1f, 7f)]
		public int ExpiresAfter = 2;

		// Token: 0x020006AD RID: 1709
		[Serializable]
		public class DayContainer
		{
			// Token: 0x040020EF RID: 8431
			public EDay Day;
		}
	}
}
