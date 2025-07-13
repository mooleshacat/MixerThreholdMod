using System;
using ScheduleOne.Product;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000463 RID: 1123
	[Serializable]
	public class ContractData : QuestData
	{
		// Token: 0x0600169B RID: 5787 RVA: 0x00064560 File Offset: 0x00062760
		public ContractData(string guid, EQuestState state, bool isTracked, string title, string desc, bool isTimed, GameDateTimeData expiry, QuestEntryData[] entries, string customerGUID, float payment, ProductList productList, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTimeData acceptTime) : base(guid, state, isTracked, title, desc, isTimed, expiry, entries)
		{
			this.CustomerGUID = customerGUID;
			this.Payment = payment;
			this.ProductList = productList;
			this.DeliveryLocationGUID = deliveryLocationGUID;
			this.DeliveryWindow = deliveryWindow;
			this.PickupScheduleIndex = pickupScheduleIndex;
			this.AcceptTime = acceptTime;
		}

		// Token: 0x040014CF RID: 5327
		public string CustomerGUID;

		// Token: 0x040014D0 RID: 5328
		public float Payment;

		// Token: 0x040014D1 RID: 5329
		public ProductList ProductList;

		// Token: 0x040014D2 RID: 5330
		public string DeliveryLocationGUID;

		// Token: 0x040014D3 RID: 5331
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x040014D4 RID: 5332
		public int PickupScheduleIndex;

		// Token: 0x040014D5 RID: 5333
		public GameDateTimeData AcceptTime;
	}
}
