using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200097D RID: 2429
	public class ItemFilter_ID : ItemFilter
	{
		// Token: 0x06004196 RID: 16790 RVA: 0x00114C22 File Offset: 0x00112E22
		public ItemFilter_ID(List<string> ids)
		{
			this.IDs = ids;
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x00114C43 File Offset: 0x00112E43
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			if (instance == null)
			{
				return false;
			}
			if (this.IsWhitelist)
			{
				if (!this.IDs.Contains(instance.ID))
				{
					return false;
				}
			}
			else if (this.IDs.Contains(instance.ID))
			{
				return false;
			}
			return base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002EC5 RID: 11973
		public bool IsWhitelist = true;

		// Token: 0x04002EC6 RID: 11974
		public List<string> IDs = new List<string>();
	}
}
