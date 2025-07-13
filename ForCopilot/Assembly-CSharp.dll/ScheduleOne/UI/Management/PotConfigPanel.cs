using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B2D RID: 2861
	public class PotConfigPanel : ConfigPanel
	{
		// Token: 0x06004C4A RID: 19530 RVA: 0x00140F3C File Offset: 0x0013F13C
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ItemField> list = new List<ItemField>();
			List<ItemField> list2 = new List<ItemField>();
			List<ItemField> list3 = new List<ItemField>();
			List<ItemField> list4 = new List<ItemField>();
			List<NPCField> list5 = new List<NPCField>();
			List<ObjectField> list6 = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				PotConfiguration potConfiguration = (PotConfiguration)entityConfiguration;
				if (potConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to PotConfiguration", null);
					return;
				}
				list.Add(potConfiguration.Seed);
				list2.Add(potConfiguration.Additive1);
				list3.Add(potConfiguration.Additive2);
				list4.Add(potConfiguration.Additive3);
				list5.Add(potConfiguration.AssignedBotanist);
				list6.Add(potConfiguration.Destination);
			}
			this.SeedUI.Bind(list);
			this.Additive1UI.Bind(list2);
			this.Additive2UI.Bind(list3);
			this.Additive3UI.Bind(list4);
			this.DestinationUI.Bind(list6);
		}

		// Token: 0x040038D9 RID: 14553
		[Header("References")]
		public ItemFieldUI SeedUI;

		// Token: 0x040038DA RID: 14554
		public ItemFieldUI Additive1UI;

		// Token: 0x040038DB RID: 14555
		public ItemFieldUI Additive2UI;

		// Token: 0x040038DC RID: 14556
		public ItemFieldUI Additive3UI;

		// Token: 0x040038DD RID: 14557
		public ObjectFieldUI DestinationUI;
	}
}
