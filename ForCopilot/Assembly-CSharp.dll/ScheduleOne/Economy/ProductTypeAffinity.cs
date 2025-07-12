using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x0200069F RID: 1695
	[Serializable]
	public class ProductTypeAffinity
	{
		// Token: 0x04002086 RID: 8326
		public EDrugType DrugType;

		// Token: 0x04002087 RID: 8327
		[Range(-1f, 1f)]
		public float Affinity;
	}
}
