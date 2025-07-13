using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200045D RID: 1117
	[Serializable]
	public class CocaineProductData : ProductData
	{
		// Token: 0x06001695 RID: 5781 RVA: 0x00064461 File Offset: 0x00062661
		public CocaineProductData(string name, string id, EDrugType drugType, string[] properties, CocaineAppearanceSettings appearanceSettings) : base(name, id, drugType, properties)
		{
			this.AppearanceSettings = appearanceSettings;
		}

		// Token: 0x040014B8 RID: 5304
		public CocaineAppearanceSettings AppearanceSettings;
	}
}
