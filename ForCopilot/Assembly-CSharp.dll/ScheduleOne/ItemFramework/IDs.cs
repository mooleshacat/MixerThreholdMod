using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000978 RID: 2424
	public class IDs : ItemFilter
	{
		// Token: 0x06004189 RID: 16777 RVA: 0x00114ACD File Offset: 0x00112CCD
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return this.AcceptedIDs.Contains(instance.ID) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002EC2 RID: 11970
		public List<string> AcceptedIDs = new List<string>();
	}
}
