using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D8 RID: 472
	public abstract class ColorHelper
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0002E3F2 File Offset: 0x0002C5F2
		public static Color ColorWithHex(uint hex)
		{
			return ColorHelper.ColorWithHexAlpha(hex << 8 | 255U);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0002E404 File Offset: 0x0002C604
		public static Color ColorWithHexAlpha(uint hex)
		{
			float r = (hex >> 24 & 255U) / 255f;
			float g = (hex >> 16 & 255U) / 255f;
			float b = (hex >> 8 & 255U) / 255f;
			float a = (hex & 255U) / 255f;
			return new Color(r, g, b, a);
		}
	}
}
