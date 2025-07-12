using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000921 RID: 2337
	[Serializable]
	public class MethAppearanceSettings
	{
		// Token: 0x06003EF3 RID: 16115 RVA: 0x001089A3 File Offset: 0x00106BA3
		public MethAppearanceSettings(Color32 mainColor, Color32 secondaryColor)
		{
			this.MainColor = mainColor;
			this.SecondaryColor = secondaryColor;
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0000494F File Offset: 0x00002B4F
		public MethAppearanceSettings()
		{
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x001089B9 File Offset: 0x00106BB9
		public bool IsUnintialized()
		{
			return this.MainColor == Color.clear || this.SecondaryColor == Color.clear;
		}

		// Token: 0x04002D08 RID: 11528
		public Color32 MainColor;

		// Token: 0x04002D09 RID: 11529
		public Color32 SecondaryColor;
	}
}
