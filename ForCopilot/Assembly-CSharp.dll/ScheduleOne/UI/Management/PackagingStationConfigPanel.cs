using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B2C RID: 2860
	public class PackagingStationConfigPanel : ConfigPanel
	{
		// Token: 0x06004C48 RID: 19528 RVA: 0x00140EC0 File Offset: 0x0013F0C0
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				PackagingStationConfiguration packagingStationConfiguration = (PackagingStationConfiguration)entityConfiguration;
				if (packagingStationConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to PackagingStationConfiguration", null);
					return;
				}
				list.Add(packagingStationConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040038D8 RID: 14552
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
