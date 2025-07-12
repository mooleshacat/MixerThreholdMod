using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Growing
{
	// Token: 0x020008BD RID: 2237
	public class CocaPlant : Plant
	{
		// Token: 0x06003C6A RID: 15466 RVA: 0x000FE948 File Offset: 0x000FCB48
		public override ItemInstance GetHarvestedProduct(int quantity = 1)
		{
			EQuality quality = ItemQuality.GetQuality(this.QualityLevel);
			QualityItemInstance qualityItemInstance = this.Harvestable.Product.GetDefaultInstance(quantity) as QualityItemInstance;
			qualityItemInstance.Quality = quality;
			return qualityItemInstance;
		}

		// Token: 0x04002B3C RID: 11068
		public PlantHarvestable Harvestable;
	}
}
