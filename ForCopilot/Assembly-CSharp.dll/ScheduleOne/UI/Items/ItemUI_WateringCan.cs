using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BB6 RID: 2998
	public class ItemUI_WateringCan : ItemUI
	{
		// Token: 0x06004FA1 RID: 20385 RVA: 0x0014FD98 File Offset: 0x0014DF98
		public override void Setup(ItemInstance item)
		{
			this.wcInstance = (item as WateringCanInstance);
			base.Setup(item);
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x0014FDB0 File Offset: 0x0014DFB0
		public override void UpdateUI()
		{
			base.UpdateUI();
			if (this.Destroyed)
			{
				return;
			}
			if (this.wcInstance == null)
			{
				return;
			}
			this.AmountLabel.text = ((float)Mathf.RoundToInt(this.wcInstance.CurrentFillAmount * 10f) / 10f).ToString() + "L";
		}

		// Token: 0x04003BB1 RID: 15281
		protected WateringCanInstance wcInstance;

		// Token: 0x04003BB2 RID: 15282
		public Text AmountLabel;
	}
}
