using System;
using System.Collections.Generic;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000981 RID: 2433
	public class ItemFilter_UnpackagedProduct : ItemFilter_Category
	{
		// Token: 0x0600419E RID: 16798 RVA: 0x00114CFD File Offset: 0x00112EFD
		public ItemFilter_UnpackagedProduct() : base(new List<EItemCategory>
		{
			EItemCategory.Product
		})
		{
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x00114D44 File Offset: 0x00112F44
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			return productItemInstance != null && !(productItemInstance.AppliedPackaging != null) && base.DoesItemMatchFilter(instance);
		}
	}
}
