using System;
using System.Collections.Generic;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000436 RID: 1078
	[Serializable]
	public class RouteListData
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x00063DAF File Offset: 0x00061FAF
		public RouteListData(List<AdvancedTransitRouteData> routes)
		{
			this.Routes = routes;
		}

		// Token: 0x04001445 RID: 5189
		public List<AdvancedTransitRouteData> Routes;
	}
}
