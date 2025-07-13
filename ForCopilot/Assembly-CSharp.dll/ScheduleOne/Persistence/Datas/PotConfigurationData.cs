using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000434 RID: 1076
	[Serializable]
	public class PotConfigurationData : SaveData
	{
		// Token: 0x0600166A RID: 5738 RVA: 0x00063D73 File Offset: 0x00061F73
		public PotConfigurationData(ItemFieldData seed, ItemFieldData additive1, ItemFieldData additive2, ItemFieldData additive3, ObjectFieldData destination)
		{
			this.Seed = seed;
			this.Additive1 = additive1;
			this.Additive2 = additive2;
			this.Additive3 = additive3;
			this.Destination = destination;
		}

		// Token: 0x0400143F RID: 5183
		public ItemFieldData Seed;

		// Token: 0x04001440 RID: 5184
		public ItemFieldData Additive1;

		// Token: 0x04001441 RID: 5185
		public ItemFieldData Additive2;

		// Token: 0x04001442 RID: 5186
		public ItemFieldData Additive3;

		// Token: 0x04001443 RID: 5187
		public ObjectFieldData Destination;
	}
}
