using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200045E RID: 1118
	[Serializable]
	public class MethProductData : ProductData
	{
		// Token: 0x06001696 RID: 5782 RVA: 0x00064476 File Offset: 0x00062676
		public MethProductData(string name, string id, EDrugType drugType, string[] properties, MethAppearanceSettings appearanceSettings) : base(name, id, drugType, properties)
		{
			this.AppearanceSettings = appearanceSettings;
		}

		// Token: 0x040014B9 RID: 5305
		public MethAppearanceSettings AppearanceSettings;
	}
}
