using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000072 RID: 114
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x0600026E RID: 622 RVA: 0x0000DC91 File Offset: 0x0000BE91
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000297 RID: 663
		public readonly float min;
	}
}
