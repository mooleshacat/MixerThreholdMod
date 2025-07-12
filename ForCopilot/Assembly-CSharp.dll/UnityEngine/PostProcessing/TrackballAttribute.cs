using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000073 RID: 115
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x0600026F RID: 623 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04000298 RID: 664
		public readonly string method;
	}
}
