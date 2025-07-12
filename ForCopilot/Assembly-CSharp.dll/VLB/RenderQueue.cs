using System;

namespace VLB
{
	// Token: 0x0200010D RID: 269
	public enum RenderQueue
	{
		// Token: 0x040005C6 RID: 1478
		Custom,
		// Token: 0x040005C7 RID: 1479
		Background = 1000,
		// Token: 0x040005C8 RID: 1480
		Geometry = 2000,
		// Token: 0x040005C9 RID: 1481
		AlphaTest = 2450,
		// Token: 0x040005CA RID: 1482
		GeometryLast = 2500,
		// Token: 0x040005CB RID: 1483
		Transparent = 3000,
		// Token: 0x040005CC RID: 1484
		Overlay = 4000
	}
}
