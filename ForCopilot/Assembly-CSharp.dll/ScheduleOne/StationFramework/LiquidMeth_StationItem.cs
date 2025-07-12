using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008F7 RID: 2295
	public class LiquidMeth_StationItem : StationItem
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x00106794 File Offset: 0x00104994
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			LiquidMethDefinition liquidMethDefinition = itemDefinition as LiquidMethDefinition;
			if (this.Visuals != null)
			{
				this.Visuals.Setup(liquidMethDefinition);
			}
			base.GetModule<CookableModule>().LiquidColor = liquidMethDefinition.CookableLiquidColor;
			base.GetModule<CookableModule>().SolidColor = liquidMethDefinition.CookableSolidColor;
			base.GetModule<PourableModule>().LiquidColor = liquidMethDefinition.LiquidVolumeColor;
			base.GetModule<PourableModule>().PourParticlesColor = liquidMethDefinition.PourParticlesColor;
		}

		// Token: 0x04002C5C RID: 11356
		public LiquidMethVisuals Visuals;
	}
}
