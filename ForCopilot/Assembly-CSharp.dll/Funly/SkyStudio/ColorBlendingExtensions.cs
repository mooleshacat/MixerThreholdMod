using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A1 RID: 417
	public static class ColorBlendingExtensions
	{
		// Token: 0x0600087E RID: 2174 RVA: 0x000270C0 File Offset: 0x000252C0
		public static Color Clear(this Color color)
		{
			return new Color(color.r, color.g, color.b, 0f);
		}
	}
}
