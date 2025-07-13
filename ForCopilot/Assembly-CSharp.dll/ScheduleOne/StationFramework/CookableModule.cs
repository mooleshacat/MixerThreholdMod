using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008FA RID: 2298
	public class CookableModule : ItemModule
	{
		// Token: 0x04002C71 RID: 11377
		[Header("Cook Settings")]
		public int CookTime = 360;

		// Token: 0x04002C72 RID: 11378
		public CookableModule.ECookableType CookType;

		// Token: 0x04002C73 RID: 11379
		[Header("Product Settings")]
		public StorableItemDefinition Product;

		// Token: 0x04002C74 RID: 11380
		public int ProductQuantity = 1;

		// Token: 0x04002C75 RID: 11381
		public Rigidbody ProductShardPrefab;

		// Token: 0x04002C76 RID: 11382
		[Header("Appearance")]
		public Color LiquidColor;

		// Token: 0x04002C77 RID: 11383
		public Color SolidColor;

		// Token: 0x020008FB RID: 2299
		public enum ECookableType
		{
			// Token: 0x04002C79 RID: 11385
			Liquid,
			// Token: 0x04002C7A RID: 11386
			Solid
		}
	}
}
