using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B27 RID: 2855
	public class CleanerConfigPanel : ConfigPanel
	{
		// Token: 0x06004C3E RID: 19518 RVA: 0x00140BC0 File Offset: 0x0013EDC0
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectListField> list2 = new List<ObjectListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				CleanerConfiguration cleanerConfiguration = (CleanerConfiguration)entityConfiguration;
				if (cleanerConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to CleanerConfiguration", null);
					return;
				}
				list.Add(cleanerConfiguration.Home);
				list2.Add(cleanerConfiguration.Bins);
			}
			this.BedUI.Bind(list);
			this.BinsUI.Bind(list2);
		}

		// Token: 0x040038CE RID: 14542
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040038CF RID: 14543
		public ObjectListFieldUI BinsUI;
	}
}
