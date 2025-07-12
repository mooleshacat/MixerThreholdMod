using System;
using System.Collections.Generic;
using ScheduleOne.Management;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000423 RID: 1059
	[Serializable]
	public class AdvancedTransitRouteData
	{
		// Token: 0x06001658 RID: 5720 RVA: 0x00063C1F File Offset: 0x00061E1F
		public AdvancedTransitRouteData(string sourceGUID, string destinationGUID, ManagementItemFilter.EMode filtermode, List<string> filterGUIDs)
		{
			this.SourceGUID = sourceGUID;
			this.DestinationGUID = destinationGUID;
			this.FilterMode = filtermode;
			this.FilterItemIDs = filterGUIDs;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0000494F File Offset: 0x00002B4F
		public AdvancedTransitRouteData()
		{
		}

		// Token: 0x04001422 RID: 5154
		public string SourceGUID;

		// Token: 0x04001423 RID: 5155
		public string DestinationGUID;

		// Token: 0x04001424 RID: 5156
		public ManagementItemFilter.EMode FilterMode;

		// Token: 0x04001425 RID: 5157
		public List<string> FilterItemIDs;
	}
}
