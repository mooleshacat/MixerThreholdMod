using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B2B RID: 2859
	public class PackagerConfigPanel : ConfigPanel
	{
		// Token: 0x06004C46 RID: 19526 RVA: 0x00140E04 File Offset: 0x0013F004
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectListField> list2 = new List<ObjectListField>();
			List<RouteListField> list3 = new List<RouteListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				PackagerConfiguration packagerConfiguration = (PackagerConfiguration)entityConfiguration;
				if (packagerConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to PackagerConfiguration", null);
					return;
				}
				list.Add(packagerConfiguration.Home);
				list2.Add(packagerConfiguration.Stations);
				list3.Add(packagerConfiguration.Routes);
			}
			this.BedUI.Bind(list);
			this.StationsUI.Bind(list2);
			this.RoutesUI.Bind(list3);
		}

		// Token: 0x040038D5 RID: 14549
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040038D6 RID: 14550
		public ObjectListFieldUI StationsUI;

		// Token: 0x040038D7 RID: 14551
		public RouteListFieldUI RoutesUI;
	}
}
