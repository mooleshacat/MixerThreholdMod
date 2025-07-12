using System;
using System.Collections.Generic;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006A0 RID: 1696
	[Serializable]
	public class CustomerAffinityData
	{
		// Token: 0x06002E13 RID: 11795 RVA: 0x000C007C File Offset: 0x000BE27C
		public void CopyTo(CustomerAffinityData data)
		{
			using (List<ProductTypeAffinity>.Enumerator enumerator = this.ProductAffinities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProductTypeAffinity affinity = enumerator.Current;
					if (data.ProductAffinities.Exists((ProductTypeAffinity x) => x.DrugType == affinity.DrugType))
					{
						data.ProductAffinities.Find((ProductTypeAffinity x) => x.DrugType == affinity.DrugType).Affinity = affinity.Affinity;
					}
					else
					{
						data.ProductAffinities.Add(new ProductTypeAffinity
						{
							DrugType = affinity.DrugType,
							Affinity = affinity.Affinity
						});
					}
				}
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000C0150 File Offset: 0x000BE350
		public float GetAffinity(EDrugType type)
		{
			ProductTypeAffinity productTypeAffinity = this.ProductAffinities.Find((ProductTypeAffinity x) => x.DrugType == type);
			if (productTypeAffinity == null)
			{
				Debug.LogWarning("No affinity data found for product type " + type.ToString());
				return 0f;
			}
			return productTypeAffinity.Affinity;
		}

		// Token: 0x04002088 RID: 8328
		[Header("Product Affinities - How much the customer likes each product type. -1 = hates, 0 = neutral, 1 = loves.")]
		public List<ProductTypeAffinity> ProductAffinities = new List<ProductTypeAffinity>();
	}
}
