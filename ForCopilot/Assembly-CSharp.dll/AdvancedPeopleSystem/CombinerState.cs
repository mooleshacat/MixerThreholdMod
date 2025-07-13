using System;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000207 RID: 519
	public enum CombinerState : byte
	{
		// Token: 0x04000C25 RID: 3109
		NotCombined,
		// Token: 0x04000C26 RID: 3110
		InProgressCombineMesh,
		// Token: 0x04000C27 RID: 3111
		InProgressBlendshapeTransfer,
		// Token: 0x04000C28 RID: 3112
		InProgressClear,
		// Token: 0x04000C29 RID: 3113
		Combined,
		// Token: 0x04000C2A RID: 3114
		UsedPreBuitMeshes
	}
}
