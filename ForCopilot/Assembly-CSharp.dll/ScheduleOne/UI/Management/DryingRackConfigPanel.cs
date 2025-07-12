using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B28 RID: 2856
	public class DryingRackConfigPanel : ConfigPanel
	{
		// Token: 0x06004C40 RID: 19520 RVA: 0x00140C58 File Offset: 0x0013EE58
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<QualityField> list = new List<QualityField>();
			List<ObjectField> list2 = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				DryingRackConfiguration dryingRackConfiguration = (DryingRackConfiguration)entityConfiguration;
				if (dryingRackConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to DryingRackConfiguration", null);
					return;
				}
				list.Add(dryingRackConfiguration.TargetQuality);
				list2.Add(dryingRackConfiguration.Destination);
			}
			this.QualityUI.Bind(list);
			this.DestinationUI.Bind(list2);
		}

		// Token: 0x040038D0 RID: 14544
		[Header("References")]
		public QualityFieldUI QualityUI;

		// Token: 0x040038D1 RID: 14545
		public ObjectFieldUI DestinationUI;
	}
}
