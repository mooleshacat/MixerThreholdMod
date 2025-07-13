using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Growing
{
	// Token: 0x020008C5 RID: 2245
	public class WeedPlant : Plant
	{
		// Token: 0x06003C93 RID: 15507 RVA: 0x000FF2A4 File Offset: 0x000FD4A4
		public override ItemInstance GetHarvestedProduct(int quantity = 1)
		{
			EQuality quality = ItemQuality.GetQuality(this.QualityLevel);
			QualityItemInstance qualityItemInstance = this.BranchPrefab.Product.GetDefaultInstance(quantity) as QualityItemInstance;
			qualityItemInstance.Quality = quality;
			return qualityItemInstance;
		}

		// Token: 0x04002B63 RID: 11107
		public PlantHarvestable BranchPrefab;
	}
}
