using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000917 RID: 2327
	[Serializable]
	public class CocaineAppearanceSettings
	{
		// Token: 0x06003ED6 RID: 16086 RVA: 0x00108280 File Offset: 0x00106480
		public CocaineAppearanceSettings(Color32 mainColor, Color32 secondaryColor)
		{
			this.MainColor = mainColor;
			this.SecondaryColor = secondaryColor;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0000494F File Offset: 0x00002B4F
		public CocaineAppearanceSettings()
		{
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x00108296 File Offset: 0x00106496
		public bool IsUnintialized()
		{
			return this.MainColor == Color.clear || this.SecondaryColor == Color.clear;
		}

		// Token: 0x04002CE7 RID: 11495
		public Color32 MainColor;

		// Token: 0x04002CE8 RID: 11496
		public Color32 SecondaryColor;
	}
}
