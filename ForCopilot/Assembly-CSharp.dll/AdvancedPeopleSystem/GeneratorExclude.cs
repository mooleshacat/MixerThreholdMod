using System;
using System.Collections.Generic;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200021D RID: 541
	[Serializable]
	public class GeneratorExclude
	{
		// Token: 0x04000C97 RID: 3223
		public ExcludeItem ExcludeItem;

		// Token: 0x04000C98 RID: 3224
		public int targetIndex;

		// Token: 0x04000C99 RID: 3225
		public List<ExcludeIndexes> exclude = new List<ExcludeIndexes>();
	}
}
