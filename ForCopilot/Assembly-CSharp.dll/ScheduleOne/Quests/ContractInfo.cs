using System;
using System.Collections.Generic;
using ScheduleOne.Dialogue;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Product;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E6 RID: 742
	[Serializable]
	public class ContractInfo
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x00047B0D File Offset: 0x00045D0D
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x00047B15 File Offset: 0x00045D15
		public DeliveryLocation DeliveryLocation { get; private set; }

		// Token: 0x0600102B RID: 4139 RVA: 0x00047B20 File Offset: 0x00045D20
		public ContractInfo(float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, bool expires, int expiresAfter, int pickupScheduleIndex, bool isCounterOffer)
		{
			this.Payment = payment;
			this.Products = products;
			this.DeliveryLocationGUID = deliveryLocationGUID;
			this.DeliveryWindow = deliveryWindow;
			this.Expires = expires;
			this.ExpiresAfter = expiresAfter;
			this.PickupScheduleIndex = pickupScheduleIndex;
			this.IsCounterOffer = isCounterOffer;
			if (GUIDManager.IsGUIDValid(deliveryLocationGUID))
			{
				this.DeliveryLocation = GUIDManager.GetObject<DeliveryLocation>(new Guid(deliveryLocationGUID));
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0000494F File Offset: 0x00002B4F
		public ContractInfo()
		{
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00047B8C File Offset: 0x00045D8C
		public DialogueChain ProcessMessage(DialogueChain messageChain)
		{
			if (this.DeliveryLocation == null && GUIDManager.IsGUIDValid(this.DeliveryLocationGUID))
			{
				this.DeliveryLocation = GUIDManager.GetObject<DeliveryLocation>(new Guid(this.DeliveryLocationGUID));
			}
			List<string> list = new List<string>();
			string[] lines = messageChain.Lines;
			for (int i = 0; i < lines.Length; i++)
			{
				string text = lines[i].Replace("<PRICE>", "<color=#46CB4F>" + MoneyManager.FormatAmount(this.Payment, false, false) + "</color>");
				text = text.Replace("<PRODUCT>", this.Products.GetCommaSeperatedString());
				text = text.Replace("<QUALITY>", this.Products.GetQualityString());
				text = text.Replace("<LOCATION>", "<b>" + this.DeliveryLocation.GetDescription() + "</b>");
				text = text.Replace("<WINDOW_START>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowStartTime, true));
				text = text.Replace("<WINDOW_END>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowEndTime, true));
				list.Add(text);
			}
			return new DialogueChain
			{
				Lines = list.ToArray()
			};
		}

		// Token: 0x04001091 RID: 4241
		public float Payment;

		// Token: 0x04001092 RID: 4242
		public ProductList Products;

		// Token: 0x04001093 RID: 4243
		public string DeliveryLocationGUID;

		// Token: 0x04001094 RID: 4244
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x04001095 RID: 4245
		public bool Expires;

		// Token: 0x04001096 RID: 4246
		public int ExpiresAfter;

		// Token: 0x04001097 RID: 4247
		public int PickupScheduleIndex;

		// Token: 0x04001098 RID: 4248
		public bool IsCounterOffer;
	}
}
