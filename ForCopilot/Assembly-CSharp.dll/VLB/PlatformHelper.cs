using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200013D RID: 317
	public class PlatformHelper
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x0001A08F File Offset: 0x0001828F
		public static string GetCurrentPlatformSuffix()
		{
			return PlatformHelper.GetPlatformSuffix(Application.platform);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001A09B File Offset: 0x0001829B
		private static string GetPlatformSuffix(RuntimePlatform platform)
		{
			return platform.ToString();
		}
	}
}
