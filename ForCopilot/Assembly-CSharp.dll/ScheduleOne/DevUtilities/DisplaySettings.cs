using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200073F RID: 1855
	[Serializable]
	public struct DisplaySettings
	{
		// Token: 0x06003210 RID: 12816 RVA: 0x000D0CD4 File Offset: 0x000CEED4
		public static List<Resolution> GetResolutions()
		{
			Resolution[] resolutions = Screen.resolutions;
			RefreshRate refreshRateRatio = resolutions[resolutions.Length - 1].refreshRateRatio;
			float num = refreshRateRatio.numerator / refreshRateRatio.denominator;
			List<Resolution> list = new List<Resolution>();
			int i;
			Predicate<Resolution> <>9__0;
			int j;
			for (i = 0; i < resolutions.Length; i = j + 1)
			{
				List<Resolution> list2 = list;
				Predicate<Resolution> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Resolution x) => x.width == resolutions[i].width && x.height == resolutions[i].height));
				}
				if (!list2.Exists(match))
				{
					Resolution item = resolutions[i];
					if (item.refreshRateRatio.numerator / item.refreshRateRatio.denominator >= num - 0.1f)
					{
						list.Add(item);
					}
				}
				j = i;
			}
			return list;
		}

		// Token: 0x04002346 RID: 9030
		public int ResolutionIndex;

		// Token: 0x04002347 RID: 9031
		public DisplaySettings.EDisplayMode DisplayMode;

		// Token: 0x04002348 RID: 9032
		public bool VSync;

		// Token: 0x04002349 RID: 9033
		public int TargetFPS;

		// Token: 0x0400234A RID: 9034
		public float UIScale;

		// Token: 0x0400234B RID: 9035
		public float CameraBobbing;

		// Token: 0x0400234C RID: 9036
		public int ActiveDisplayIndex;

		// Token: 0x02000740 RID: 1856
		public enum EDisplayMode
		{
			// Token: 0x0400234E RID: 9038
			Windowed,
			// Token: 0x0400234F RID: 9039
			FullscreenWindow,
			// Token: 0x04002350 RID: 9040
			ExclusiveFullscreen
		}
	}
}
