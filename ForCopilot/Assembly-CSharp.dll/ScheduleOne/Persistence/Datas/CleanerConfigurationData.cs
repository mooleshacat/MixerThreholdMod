using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000429 RID: 1065
	[Serializable]
	public class CleanerConfigurationData : SaveData
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x00063CAB File Offset: 0x00061EAB
		public CleanerConfigurationData(ObjectFieldData bed, ObjectListFieldData bins)
		{
			this.Bed = bed;
			this.Bins = bins;
		}

		// Token: 0x0400142F RID: 5167
		public ObjectFieldData Bed;

		// Token: 0x04001430 RID: 5168
		public ObjectListFieldData Bins;
	}
}
