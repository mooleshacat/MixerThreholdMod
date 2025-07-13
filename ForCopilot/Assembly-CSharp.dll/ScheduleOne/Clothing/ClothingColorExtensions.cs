using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Clothing
{
	// Token: 0x0200078A RID: 1930
	public static class ClothingColorExtensions
	{
		// Token: 0x060033E6 RID: 13286 RVA: 0x000D8027 File Offset: 0x000D6227
		public static Color GetActualColor(this EClothingColor color)
		{
			return Singleton<ClothingUtility>.Instance.GetColorData(color).ActualColor;
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x000D8039 File Offset: 0x000D6239
		public static Color GetLabelColor(this EClothingColor color)
		{
			return Singleton<ClothingUtility>.Instance.GetColorData(color).LabelColor;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x000D804B File Offset: 0x000D624B
		public static string GetLabel(this EClothingColor color)
		{
			return color.ToString();
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x000D805C File Offset: 0x000D625C
		public static EClothingColor GetClothingColor(Color color)
		{
			foreach (object obj in Enum.GetValues(typeof(EClothingColor)))
			{
				EClothingColor eclothingColor = (EClothingColor)obj;
				if (ClothingColorExtensions.ColorEquals(eclothingColor.GetActualColor(), color, 0.004f))
				{
					return eclothingColor;
				}
			}
			string str = "Could not find clothing color for color ";
			Color color2 = color;
			Console.LogError(str + color2.ToString(), null);
			return EClothingColor.White;
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x000D80F4 File Offset: 0x000D62F4
		public static bool ColorEquals(Color a, Color b, float tolerance = 0.004f)
		{
			return a.r <= b.r + tolerance && a.g <= b.g + tolerance && a.b <= b.b + tolerance && a.r >= b.r - tolerance && a.g >= b.g - tolerance && a.b >= b.b - tolerance;
		}
	}
}
