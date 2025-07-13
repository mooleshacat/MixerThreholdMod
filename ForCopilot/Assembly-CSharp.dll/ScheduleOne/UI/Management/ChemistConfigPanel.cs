using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B25 RID: 2853
	public class ChemistConfigPanel : ConfigPanel
	{
		// Token: 0x06004C3A RID: 19514 RVA: 0x00140A90 File Offset: 0x0013EC90
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectListField> list2 = new List<ObjectListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				ChemistConfiguration chemistConfiguration = (ChemistConfiguration)entityConfiguration;
				if (chemistConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to BotanistConfiguration", null);
					return;
				}
				list.Add(chemistConfiguration.Home);
				list2.Add(chemistConfiguration.Stations);
			}
			this.BedUI.Bind(list);
			this.StationsUI.Bind(list2);
		}

		// Token: 0x040038CA RID: 14538
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040038CB RID: 14539
		public ObjectListFieldUI StationsUI;
	}
}
