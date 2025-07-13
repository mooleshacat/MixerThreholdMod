using System;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BB7 RID: 2999
	public class ClothingItemUI : ItemUI
	{
		// Token: 0x06004FA4 RID: 20388 RVA: 0x0014FE10 File Offset: 0x0014E010
		public override void UpdateUI()
		{
			base.UpdateUI();
			ClothingInstance clothingInstance = this.itemInstance as ClothingInstance;
			if (this.itemInstance != null && (this.itemInstance.Definition as ClothingDefinition).Colorable)
			{
				this.IconImg.color = clothingInstance.Color.GetActualColor();
			}
			else
			{
				this.IconImg.color = Color.white;
			}
			if (this.itemInstance != null)
			{
				this.ClothingTypeIcon.sprite = Singleton<ClothingUtility>.Instance.GetSlotData((this.itemInstance.Definition as ClothingDefinition).Slot).Icon;
				return;
			}
			this.ClothingTypeIcon.sprite = null;
		}

		// Token: 0x04003BB3 RID: 15283
		public Image ClothingTypeIcon;
	}
}
