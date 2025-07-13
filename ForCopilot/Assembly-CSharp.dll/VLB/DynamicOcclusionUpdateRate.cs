using System;

namespace VLB
{
	// Token: 0x02000110 RID: 272
	[Flags]
	public enum DynamicOcclusionUpdateRate
	{
		// Token: 0x040005D4 RID: 1492
		Never = 1,
		// Token: 0x040005D5 RID: 1493
		OnEnable = 2,
		// Token: 0x040005D6 RID: 1494
		OnBeamMove = 4,
		// Token: 0x040005D7 RID: 1495
		EveryXFrames = 8,
		// Token: 0x040005D8 RID: 1496
		OnBeamMoveAndEveryXFrames = 12
	}
}
