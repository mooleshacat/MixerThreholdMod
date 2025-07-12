using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000156 RID: 342
	public static class SpotLightHelper
	{
		// Token: 0x06000677 RID: 1655 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
		public static float GetIntensity(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.intensity;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001D4BB File Offset: 0x0001B6BB
		public static float GetSpotAngle(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.spotAngle;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001D4D2 File Offset: 0x0001B6D2
		public static float GetFallOffEnd(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.range;
		}
	}
}
