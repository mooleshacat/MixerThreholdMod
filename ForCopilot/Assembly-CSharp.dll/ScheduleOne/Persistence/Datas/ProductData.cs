using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200045F RID: 1119
	[Serializable]
	public class ProductData : SaveData
	{
		// Token: 0x06001697 RID: 5783 RVA: 0x0006448B File Offset: 0x0006268B
		public ProductData(string name, string id, EDrugType drugType, string[] properties)
		{
			this.Name = name;
			this.ID = id;
			this.DrugType = drugType;
			this.Properties = properties;
		}

		// Token: 0x040014BA RID: 5306
		public string Name;

		// Token: 0x040014BB RID: 5307
		public string ID;

		// Token: 0x040014BC RID: 5308
		public EDrugType DrugType;

		// Token: 0x040014BD RID: 5309
		public string[] Properties;
	}
}
