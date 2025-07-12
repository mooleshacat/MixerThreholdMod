using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B22 RID: 2850
	public class BotanistConfigPanel : ConfigPanel
	{
		// Token: 0x06004C34 RID: 19508 RVA: 0x001408D4 File Offset: 0x0013EAD4
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectField> list2 = new List<ObjectField>();
			List<ObjectListField> list3 = new List<ObjectListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				BotanistConfiguration botanistConfiguration = (BotanistConfiguration)entityConfiguration;
				if (botanistConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to BotanistConfiguration", null);
					return;
				}
				list.Add(botanistConfiguration.Home);
				list2.Add(botanistConfiguration.Supplies);
				list3.Add(botanistConfiguration.AssignedStations);
			}
			this.BedUI.Bind(list);
			this.SuppliesUI.Bind(list2);
			this.PotsUI.Bind(list3);
		}

		// Token: 0x040038C5 RID: 14533
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040038C6 RID: 14534
		public ObjectFieldUI SuppliesUI;

		// Token: 0x040038C7 RID: 14535
		public ObjectListFieldUI PotsUI;
	}
}
