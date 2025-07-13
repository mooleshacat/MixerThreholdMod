using System;

namespace VLB
{
	// Token: 0x02000112 RID: 274
	[Flags]
	public enum ShadowUpdateRate
	{
		// Token: 0x040005DE RID: 1502
		Never = 1,
		// Token: 0x040005DF RID: 1503
		OnEnable = 2,
		// Token: 0x040005E0 RID: 1504
		OnBeamMove = 4,
		// Token: 0x040005E1 RID: 1505
		EveryXFrames = 8,
		// Token: 0x040005E2 RID: 1506
		OnBeamMoveAndEveryXFrames = 12
	}
}
