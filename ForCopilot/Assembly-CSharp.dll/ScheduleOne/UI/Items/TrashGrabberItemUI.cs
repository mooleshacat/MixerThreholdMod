using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BCB RID: 3019
	public class TrashGrabberItemUI : ItemUI
	{
		// Token: 0x06005026 RID: 20518 RVA: 0x00153068 File Offset: 0x00151268
		public override void Setup(ItemInstance item)
		{
			this.trashGrabberInstance = (item as TrashGrabberInstance);
			base.Setup(item);
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x00153080 File Offset: 0x00151280
		public override void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ValueLabel.text = Mathf.FloorToInt(Mathf.Clamp01((float)this.trashGrabberInstance.GetTotalSize() / 20f) * 100f).ToString() + "%";
			base.UpdateUI();
		}

		// Token: 0x04003C23 RID: 15395
		public TextMeshProUGUI ValueLabel;

		// Token: 0x04003C24 RID: 15396
		protected TrashGrabberInstance trashGrabberInstance;
	}
}
