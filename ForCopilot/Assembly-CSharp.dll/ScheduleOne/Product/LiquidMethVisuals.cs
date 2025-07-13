using System;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000916 RID: 2326
	public class LiquidMethVisuals : MonoBehaviour
	{
		// Token: 0x06003ED4 RID: 16084 RVA: 0x001081F8 File Offset: 0x001063F8
		public void Setup(LiquidMethDefinition def)
		{
			if (def == null)
			{
				return;
			}
			if (this.StaticLiquidMesh != null)
			{
				this.StaticLiquidMesh.material.color = def.StaticLiquidColor;
			}
			if (this.LiquidContainer != null)
			{
				this.LiquidContainer.SetLiquidColor(def.LiquidVolumeColor, true, true);
			}
			if (this.PourParticles != null)
			{
				this.PourParticles.main.startColor = def.PourParticlesColor;
			}
		}

		// Token: 0x04002CE4 RID: 11492
		public MeshRenderer StaticLiquidMesh;

		// Token: 0x04002CE5 RID: 11493
		public LiquidContainer LiquidContainer;

		// Token: 0x04002CE6 RID: 11494
		public ParticleSystem PourParticles;
	}
}
