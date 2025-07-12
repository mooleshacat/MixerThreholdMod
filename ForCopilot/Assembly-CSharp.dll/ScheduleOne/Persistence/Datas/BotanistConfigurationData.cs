using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000424 RID: 1060
	[Serializable]
	public class BotanistConfigurationData : SaveData
	{
		// Token: 0x0600165A RID: 5722 RVA: 0x00063C44 File Offset: 0x00061E44
		public BotanistConfigurationData(ObjectFieldData bed, ObjectFieldData supplies, ObjectListFieldData pots)
		{
			this.Bed = bed;
			this.Supplies = supplies;
			this.Pots = pots;
		}

		// Token: 0x04001426 RID: 5158
		public ObjectFieldData Bed;

		// Token: 0x04001427 RID: 5159
		public ObjectFieldData Supplies;

		// Token: 0x04001428 RID: 5160
		public ObjectListFieldData Pots;
	}
}
