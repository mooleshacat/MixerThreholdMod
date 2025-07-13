using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000460 RID: 1120
	[Serializable]
	public class WeedProductData : ProductData
	{
		// Token: 0x06001698 RID: 5784 RVA: 0x000644B0 File Offset: 0x000626B0
		public WeedProductData(string name, string id, EDrugType drugType, string[] properties, WeedAppearanceSettings appearanceSettings) : base(name, id, drugType, properties)
		{
			this.AppearanceSettings = appearanceSettings;
		}

		// Token: 0x040014BE RID: 5310
		public WeedAppearanceSettings AppearanceSettings;
	}
}
