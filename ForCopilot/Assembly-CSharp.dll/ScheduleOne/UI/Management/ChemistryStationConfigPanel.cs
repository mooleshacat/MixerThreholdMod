using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B26 RID: 2854
	public class ChemistryStationConfigPanel : ConfigPanel
	{
		// Token: 0x06004C3C RID: 19516 RVA: 0x00140B28 File Offset: 0x0013ED28
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<StationRecipeField> list = new List<StationRecipeField>();
			List<ObjectField> list2 = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				ChemistryStationConfiguration chemistryStationConfiguration = (ChemistryStationConfiguration)entityConfiguration;
				if (chemistryStationConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to ChemistryStationConfiguration", null);
					return;
				}
				list2.Add(chemistryStationConfiguration.Destination);
				list.Add(chemistryStationConfiguration.Recipe);
			}
			this.RecipeUI.Bind(list);
			this.DestinationUI.Bind(list2);
		}

		// Token: 0x040038CC RID: 14540
		[Header("References")]
		public StationRecipeFieldUI RecipeUI;

		// Token: 0x040038CD RID: 14541
		public ObjectFieldUI DestinationUI;
	}
}
