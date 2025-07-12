using System;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000701 RID: 1793
	[Serializable]
	public class BranchNodeData
	{
		// Token: 0x04002253 RID: 8787
		public string Guid;

		// Token: 0x04002254 RID: 8788
		public string BranchLabel;

		// Token: 0x04002255 RID: 8789
		public Vector2 Position;

		// Token: 0x04002256 RID: 8790
		public BranchOptionData[] options;
	}
}
