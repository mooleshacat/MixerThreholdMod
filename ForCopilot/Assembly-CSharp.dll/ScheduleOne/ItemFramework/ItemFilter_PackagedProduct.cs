using System;
using System.Collections.Generic;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000980 RID: 2432
	public class ItemFilter_PackagedProduct : ItemFilter_Category
	{
		// Token: 0x0600419C RID: 16796 RVA: 0x00114CFD File Offset: 0x00112EFD
		public ItemFilter_PackagedProduct() : base(new List<EItemCategory>
		{
			EItemCategory.Product
		})
		{
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x00114D14 File Offset: 0x00112F14
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			return productItemInstance != null && !(productItemInstance.AppliedPackaging == null) && base.DoesItemMatchFilter(instance);
		}
	}
}
