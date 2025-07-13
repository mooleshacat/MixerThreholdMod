using System;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008C6 RID: 2246
	public class FilledPackaging_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06003C95 RID: 15509 RVA: 0x000FF2DA File Offset: 0x000FD4DA
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			(item as ProductItemInstance).SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x04002B64 RID: 11108
		public FilledPackagingVisuals Visuals;
	}
}
