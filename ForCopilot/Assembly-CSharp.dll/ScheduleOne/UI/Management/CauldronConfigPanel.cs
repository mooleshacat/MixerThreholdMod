using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B24 RID: 2852
	public class CauldronConfigPanel : ConfigPanel
	{
		// Token: 0x06004C38 RID: 19512 RVA: 0x00140A14 File Offset: 0x0013EC14
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				CauldronConfiguration cauldronConfiguration = (CauldronConfiguration)entityConfiguration;
				if (cauldronConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to CauldronConfiguration", null);
					return;
				}
				list.Add(cauldronConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040038C9 RID: 14537
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
