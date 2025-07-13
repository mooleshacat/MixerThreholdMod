using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200097A RID: 2426
	public class ItemFilter_Category : ItemFilter
	{
		// Token: 0x0600418D RID: 16781 RVA: 0x00114AFE File Offset: 0x00112CFE
		public ItemFilter_Category(List<EItemCategory> acceptedCategories)
		{
			this.AcceptedCategories = acceptedCategories;
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x00114B18 File Offset: 0x00112D18
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return this.AcceptedCategories.Contains(instance.Category) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002EC3 RID: 11971
		public List<EItemCategory> AcceptedCategories = new List<EItemCategory>();
	}
}
