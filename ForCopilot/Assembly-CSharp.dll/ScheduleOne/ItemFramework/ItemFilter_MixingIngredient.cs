using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200097F RID: 2431
	public class ItemFilter_MixingIngredient : ItemFilter
	{
		// Token: 0x0600419B RID: 16795 RVA: 0x00114CB8 File Offset: 0x00112EB8
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			if (instance == null)
			{
				return false;
			}
			ItemDefinition definition = instance.Definition;
			if (!(definition is PropertyItemDefinition))
			{
				return false;
			}
			PropertyItemDefinition item = definition as PropertyItemDefinition;
			return NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(item) && base.DoesItemMatchFilter(instance);
		}
	}
}
