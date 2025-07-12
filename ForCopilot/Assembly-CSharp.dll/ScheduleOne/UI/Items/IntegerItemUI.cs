using System;
using ScheduleOne.ItemFramework;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BBF RID: 3007
	public class IntegerItemUI : ItemUI
	{
		// Token: 0x06004FDE RID: 20446 RVA: 0x00150EBD File Offset: 0x0014F0BD
		public override void Setup(ItemInstance item)
		{
			this.integerItemInstance = (item as IntegerItemInstance);
			base.Setup(item);
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x00150ED2 File Offset: 0x0014F0D2
		public override void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ValueLabel.text = this.integerItemInstance.Value.ToString();
			base.UpdateUI();
		}

		// Token: 0x04003BDF RID: 15327
		public Text ValueLabel;

		// Token: 0x04003BE0 RID: 15328
		protected IntegerItemInstance integerItemInstance;
	}
}
