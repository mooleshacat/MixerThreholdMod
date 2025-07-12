using System;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C0A RID: 3082
	public class Meth_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06005319 RID: 21273 RVA: 0x0015F04C File Offset: 0x0015D24C
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			MethInstance methInstance = item as MethInstance;
			if (methInstance != null)
			{
				this.Visuals.Setup(methInstance.Definition as MethDefinition);
			}
		}

		// Token: 0x04003E27 RID: 15911
		public MethVisuals Visuals;
	}
}
