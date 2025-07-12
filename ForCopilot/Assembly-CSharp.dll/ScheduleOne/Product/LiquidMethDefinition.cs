using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000915 RID: 2325
	[CreateAssetMenu(fileName = "LiquidMethDefinition", menuName = "ScriptableObjects/LiquidMethDefinition", order = 1)]
	[Serializable]
	public class LiquidMethDefinition : QualityItemDefinition
	{
		// Token: 0x04002CDF RID: 11487
		[Header("Liquid Meth Color Settings")]
		public Color StaticLiquidColor;

		// Token: 0x04002CE0 RID: 11488
		public Color LiquidVolumeColor;

		// Token: 0x04002CE1 RID: 11489
		public Color PourParticlesColor;

		// Token: 0x04002CE2 RID: 11490
		public Color CookableLiquidColor;

		// Token: 0x04002CE3 RID: 11491
		public Color CookableSolidColor;
	}
}
