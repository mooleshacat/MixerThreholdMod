using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B2A RID: 2858
	public class MixingStationConfigPanel : ConfigPanel
	{
		// Token: 0x06004C44 RID: 19524 RVA: 0x00140D6C File Offset: 0x0013EF6C
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<NumberField> list2 = new List<NumberField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				MixingStationConfiguration mixingStationConfiguration = (MixingStationConfiguration)entityConfiguration;
				if (mixingStationConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to MixingStationConfiguration", null);
					return;
				}
				list.Add(mixingStationConfiguration.Destination);
				list2.Add(mixingStationConfiguration.StartThrehold);
			}
			this.DestinationUI.Bind(list);
			this.StartThresholdUI.Bind(list2);
		}

		// Token: 0x040038D3 RID: 14547
		[Header("References")]
		public ObjectFieldUI DestinationUI;

		// Token: 0x040038D4 RID: 14548
		public NumberFieldUI StartThresholdUI;
	}
}
