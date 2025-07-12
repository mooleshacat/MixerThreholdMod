using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200091D RID: 2333
	public static class DrugTypeMethods
	{
		// Token: 0x06003EED RID: 16109 RVA: 0x00108962 File Offset: 0x00106B62
		public static string GetName(this EDrugType property)
		{
			return PropertyUtility.GetDrugTypeData(property).Name;
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0010896F File Offset: 0x00106B6F
		public static Color GetColor(this EDrugType property)
		{
			return PropertyUtility.GetDrugTypeData(property).Color;
		}
	}
}
