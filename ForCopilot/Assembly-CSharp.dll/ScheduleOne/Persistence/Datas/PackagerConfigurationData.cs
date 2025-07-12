using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000432 RID: 1074
	[Serializable]
	public class PackagerConfigurationData : SaveData
	{
		// Token: 0x06001668 RID: 5736 RVA: 0x00063D47 File Offset: 0x00061F47
		public PackagerConfigurationData(ObjectFieldData bed, ObjectListFieldData stations, RouteListData routes)
		{
			this.Bed = bed;
			this.Stations = stations;
			this.Routes = routes;
		}

		// Token: 0x0400143B RID: 5179
		public ObjectFieldData Bed;

		// Token: 0x0400143C RID: 5180
		public ObjectListFieldData Stations;

		// Token: 0x0400143D RID: 5181
		public RouteListData Routes;
	}
}
