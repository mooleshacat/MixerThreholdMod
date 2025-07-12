using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000953 RID: 2387
	public class LiquidMeth_Equippable : Equippable_Viewmodel
	{
		// Token: 0x0600406E RID: 16494 RVA: 0x00110760 File Offset: 0x0010E960
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			LiquidMethDefinition def = item.Definition as LiquidMethDefinition;
			if (this.Visuals != null)
			{
				this.Visuals.Setup(def);
			}
		}

		// Token: 0x04002DE0 RID: 11744
		public LiquidMethVisuals Visuals;
	}
}
