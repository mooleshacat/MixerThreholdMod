using System;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200097C RID: 2428
	public class ItemFilter_Dryable : ItemFilter
	{
		// Token: 0x06004194 RID: 16788 RVA: 0x00114BA5 File Offset: 0x00112DA5
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return ItemFilter_Dryable.IsItemDryable(instance) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x00114BB8 File Offset: 0x00112DB8
		public static bool IsItemDryable(ItemInstance instance)
		{
			if (instance == null)
			{
				return false;
			}
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			return (productItemInstance != null && (productItemInstance.Definition as ProductDefinition).DrugType == EDrugType.Marijuana && productItemInstance.AppliedPackaging == null && productItemInstance.Quality < EQuality.Heavenly) || (instance.ID == "cocaleaf" && (instance as QualityItemInstance).Quality < EQuality.Heavenly);
		}
	}
}
