using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B29 RID: 2857
	public class LabOvenConfigPanel : ConfigPanel
	{
		// Token: 0x06004C42 RID: 19522 RVA: 0x00140CF0 File Offset: 0x0013EEF0
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				LabOvenConfiguration labOvenConfiguration = (LabOvenConfiguration)entityConfiguration;
				if (labOvenConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to LabOvenConfiguration", null);
					return;
				}
				list.Add(labOvenConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040038D2 RID: 14546
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
