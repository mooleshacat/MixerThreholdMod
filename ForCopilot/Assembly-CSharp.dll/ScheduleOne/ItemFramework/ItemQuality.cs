using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200098E RID: 2446
	public static class ItemQuality
	{
		// Token: 0x060041E5 RID: 16869 RVA: 0x0011564C File Offset: 0x0011384C
		public static EQuality GetQuality(float qualityScalar)
		{
			if (qualityScalar > 0.9f)
			{
				return EQuality.Heavenly;
			}
			if (qualityScalar > 0.75f)
			{
				return EQuality.Premium;
			}
			if (qualityScalar > 0.4f)
			{
				return EQuality.Standard;
			}
			if (qualityScalar > 0.25f)
			{
				return EQuality.Poor;
			}
			return EQuality.Trash;
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x00115678 File Offset: 0x00113878
		public static Color GetColor(EQuality quality)
		{
			switch (quality)
			{
			case EQuality.Trash:
				return ItemQuality.Trash_Color;
			case EQuality.Poor:
				return ItemQuality.Poor_Color;
			case EQuality.Standard:
				return ItemQuality.Standard_Color;
			case EQuality.Premium:
				return ItemQuality.Premium_Color;
			case EQuality.Heavenly:
				return ItemQuality.Heavenly_Color;
			default:
				Console.LogWarning("Quality color not found!", null);
				return Color.white;
			}
		}

		// Token: 0x04002EFC RID: 12028
		public const float Heavenly_Threshold = 0.9f;

		// Token: 0x04002EFD RID: 12029
		public const float Premium_Threshold = 0.75f;

		// Token: 0x04002EFE RID: 12030
		public const float Standard_Threshold = 0.4f;

		// Token: 0x04002EFF RID: 12031
		public const float Poor_Threshold = 0.25f;

		// Token: 0x04002F00 RID: 12032
		public static Color Heavenly_Color = new Color32(byte.MaxValue, 200, 50, byte.MaxValue);

		// Token: 0x04002F01 RID: 12033
		public static Color Premium_Color = new Color32(225, 75, byte.MaxValue, byte.MaxValue);

		// Token: 0x04002F02 RID: 12034
		public static Color Standard_Color = new Color32(100, 190, byte.MaxValue, byte.MaxValue);

		// Token: 0x04002F03 RID: 12035
		public static Color Poor_Color = new Color32(80, 145, 50, byte.MaxValue);

		// Token: 0x04002F04 RID: 12036
		public static Color Trash_Color = new Color32(125, 50, 50, byte.MaxValue);
	}
}
