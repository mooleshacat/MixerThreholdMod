using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000433 RID: 1075
	[Serializable]
	public class PackagingStationConfigurationData : SaveData
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x00063D64 File Offset: 0x00061F64
		public PackagingStationConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x0400143E RID: 5182
		public ObjectFieldData Destination;
	}
}
