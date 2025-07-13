using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200046A RID: 1130
	[Serializable]
	public class ShopData : SaveData
	{
		// Token: 0x060016A6 RID: 5798 RVA: 0x00064717 File Offset: 0x00062917
		public ShopData(string shopCode, StringIntPair[] itemStockQuantities)
		{
			this.ShopCode = shopCode;
			this.ItemStockQuantities = itemStockQuantities;
		}

		// Token: 0x040014EB RID: 5355
		public string ShopCode;

		// Token: 0x040014EC RID: 5356
		public StringIntPair[] ItemStockQuantities;
	}
}
