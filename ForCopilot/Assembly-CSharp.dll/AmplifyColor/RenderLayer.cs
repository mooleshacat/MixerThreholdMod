using System;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C9F RID: 3231
	[Serializable]
	public struct RenderLayer
	{
		// Token: 0x06005ABF RID: 23231 RVA: 0x0017E620 File Offset: 0x0017C820
		public RenderLayer(LayerMask mask, Color color)
		{
			this.mask = mask;
			this.color = color;
		}

		// Token: 0x040042A3 RID: 17059
		public LayerMask mask;

		// Token: 0x040042A4 RID: 17060
		public Color color;
	}
}
