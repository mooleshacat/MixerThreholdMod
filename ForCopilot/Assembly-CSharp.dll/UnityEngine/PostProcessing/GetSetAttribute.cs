using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000071 RID: 113
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x0600026D RID: 621 RVA: 0x0000DC82 File Offset: 0x0000BE82
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04000295 RID: 661
		public readonly string name;

		// Token: 0x04000296 RID: 662
		public bool dirty;
	}
}
