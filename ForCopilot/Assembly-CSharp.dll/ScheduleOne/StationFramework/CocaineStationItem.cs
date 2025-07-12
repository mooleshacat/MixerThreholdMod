using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008F9 RID: 2297
	public class CocaineStationItem : StationItem
	{
		// Token: 0x06003E5F RID: 15967 RVA: 0x00106D90 File Offset: 0x00104F90
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			CocaineInstance cocaineInstance = ((CocaineDefinition)itemDefinition).GetDefaultInstance(1) as CocaineInstance;
			foreach (FilledPackagingVisuals visuals2 in this.Visuals)
			{
				cocaineInstance.SetupPackagingVisuals(visuals2);
			}
		}

		// Token: 0x04002C70 RID: 11376
		public FilledPackagingVisuals[] Visuals;
	}
}
