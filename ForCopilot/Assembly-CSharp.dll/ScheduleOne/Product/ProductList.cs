using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000931 RID: 2353
	[Serializable]
	public class ProductList
	{
		// Token: 0x06003F5A RID: 16218 RVA: 0x0010A040 File Offset: 0x00108240
		public string GetCommaSeperatedString()
		{
			string text = string.Empty;
			foreach (ProductList.Entry entry in this.entries)
			{
				text = text + entry.Quantity.ToString() + "x ";
				text += Registry.GetItem(entry.ProductID).Name;
				if (entry != this.entries[this.entries.Count - 1])
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x0010A0E8 File Offset: 0x001082E8
		public string GetLineSeperatedString()
		{
			string text = "\n";
			foreach (ProductList.Entry entry in this.entries)
			{
				text = text + entry.Quantity.ToString() + "x ";
				text += Registry.GetItem(entry.ProductID).Name;
				if (entry != this.entries[this.entries.Count - 1])
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x0010A190 File Offset: 0x00108390
		public string GetQualityString()
		{
			ProductList.Entry entry = this.entries[0];
			return string.Concat(new string[]
			{
				"<color=#",
				ColorUtility.ToHtmlStringRGBA(ItemQuality.GetColor(entry.Quality)),
				">",
				entry.Quality.ToString(),
				"</color> "
			});
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x0010A1F4 File Offset: 0x001083F4
		public int GetTotalQuantity()
		{
			int num = 0;
			foreach (ProductList.Entry entry in this.entries)
			{
				num += entry.Quantity;
			}
			return num;
		}

		// Token: 0x04002D44 RID: 11588
		public List<ProductList.Entry> entries = new List<ProductList.Entry>();

		// Token: 0x02000932 RID: 2354
		[Serializable]
		public class Entry
		{
			// Token: 0x04002D45 RID: 11589
			public string ProductID;

			// Token: 0x04002D46 RID: 11590
			public EQuality Quality;

			// Token: 0x04002D47 RID: 11591
			public int Quantity;
		}
	}
}
