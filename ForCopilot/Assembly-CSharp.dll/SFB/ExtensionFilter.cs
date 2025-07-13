using System;

namespace SFB
{
	// Token: 0x02000171 RID: 369
	public struct ExtensionFilter
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x0002001C File Offset: 0x0001E21C
		public ExtensionFilter(string filterName, params string[] filterExtensions)
		{
			this.Name = filterName;
			this.Extensions = filterExtensions;
		}

		// Token: 0x040007E1 RID: 2017
		public string Name;

		// Token: 0x040007E2 RID: 2018
		public string[] Extensions;
	}
}
