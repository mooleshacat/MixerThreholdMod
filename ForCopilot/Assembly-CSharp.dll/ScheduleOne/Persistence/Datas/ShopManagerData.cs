using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200046B RID: 1131
	[Serializable]
	public class ShopManagerData : SaveData
	{
		// Token: 0x060016A7 RID: 5799 RVA: 0x0006472D File Offset: 0x0006292D
		public ShopManagerData(ShopData[] shops)
		{
			this.Shops = shops;
		}

		// Token: 0x040014ED RID: 5357
		public ShopData[] Shops;
	}
}
