using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000949 RID: 2377
	[Serializable]
	public class WeedAppearanceSettings
	{
		// Token: 0x06004038 RID: 16440 RVA: 0x0010F6A9 File Offset: 0x0010D8A9
		public WeedAppearanceSettings(Color32 mainColor, Color32 secondaryColor, Color32 leafColor, Color32 stemColor)
		{
			this.MainColor = mainColor;
			this.SecondaryColor = secondaryColor;
			this.LeafColor = leafColor;
			this.StemColor = stemColor;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0000494F File Offset: 0x00002B4F
		public WeedAppearanceSettings()
		{
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0010F6D0 File Offset: 0x0010D8D0
		public bool IsUnintialized()
		{
			return this.MainColor == Color.clear || this.SecondaryColor == Color.clear || this.LeafColor == Color.clear || this.StemColor == Color.clear;
		}

		// Token: 0x04002DAA RID: 11690
		public Color32 MainColor;

		// Token: 0x04002DAB RID: 11691
		public Color32 SecondaryColor;

		// Token: 0x04002DAC RID: 11692
		public Color32 LeafColor;

		// Token: 0x04002DAD RID: 11693
		public Color32 StemColor;
	}
}
