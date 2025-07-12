using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B23 RID: 2851
	public class BrickPressConfigPanel : ConfigPanel
	{
		// Token: 0x06004C36 RID: 19510 RVA: 0x00140998 File Offset: 0x0013EB98
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				BrickPressConfiguration brickPressConfiguration = (BrickPressConfiguration)entityConfiguration;
				if (brickPressConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to BrickPressConfiguration", null);
					return;
				}
				list.Add(brickPressConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040038C8 RID: 14536
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
