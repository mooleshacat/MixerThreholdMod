using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000909 RID: 2313
	public class MethStationItem : StationItem
	{
		// Token: 0x06003E9E RID: 16030 RVA: 0x00107694 File Offset: 0x00105894
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			MethInstance methInstance = ((MethDefinition)itemDefinition).GetDefaultInstance(1) as MethInstance;
			foreach (FilledPackagingVisuals visuals2 in this.Visuals)
			{
				methInstance.SetupPackagingVisuals(visuals2);
			}
		}

		// Token: 0x04002CA8 RID: 11432
		public FilledPackagingVisuals[] Visuals;
	}
}
