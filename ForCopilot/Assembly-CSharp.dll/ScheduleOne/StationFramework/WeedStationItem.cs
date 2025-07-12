using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000914 RID: 2324
	public class WeedStationItem : StationItem
	{
		// Token: 0x06003ED1 RID: 16081 RVA: 0x001081A8 File Offset: 0x001063A8
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			WeedInstance weedInstance = ((WeedDefinition)itemDefinition).GetDefaultInstance(1) as WeedInstance;
			foreach (FilledPackagingVisuals visuals2 in this.Visuals)
			{
				weedInstance.SetupPackagingVisuals(visuals2);
			}
		}

		// Token: 0x04002CDE RID: 11486
		public FilledPackagingVisuals[] Visuals;
	}
}
