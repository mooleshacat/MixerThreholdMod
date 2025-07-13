using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000427 RID: 1063
	[Serializable]
	public class ChemistConfigurationData : SaveData
	{
		// Token: 0x0600165D RID: 5725 RVA: 0x00063C7F File Offset: 0x00061E7F
		public ChemistConfigurationData(ObjectFieldData bed, ObjectListFieldData stations)
		{
			this.Bed = bed;
			this.Stations = stations;
		}

		// Token: 0x0400142B RID: 5163
		public ObjectFieldData Bed;

		// Token: 0x0400142C RID: 5164
		public ObjectListFieldData Stations;
	}
}
