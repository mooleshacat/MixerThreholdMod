using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000920 RID: 2336
	public static class PropertyMethods
	{
		// Token: 0x06003EF0 RID: 16112 RVA: 0x0010897C File Offset: 0x00106B7C
		public static string GetName(this EProperty property)
		{
			return PropertyUtility.GetPropertyData(property).Name;
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x00108989 File Offset: 0x00106B89
		public static string GetDescription(this EProperty property)
		{
			return PropertyUtility.GetPropertyData(property).Description;
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x00108996 File Offset: 0x00106B96
		public static Color GetColor(this EProperty property)
		{
			return PropertyUtility.GetPropertyData(property).Color;
		}
	}
}
