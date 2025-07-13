using System;
using ScheduleOne.ItemFramework;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BCA RID: 3018
	public class QualityItemUI : ItemUI
	{
		// Token: 0x06005023 RID: 20515 RVA: 0x0015301B File Offset: 0x0015121B
		public override void Setup(ItemInstance item)
		{
			this.qualityItemInstance = (item as QualityItemInstance);
			base.Setup(item);
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x00153030 File Offset: 0x00151230
		public override void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.QualityIcon.enabled = true;
			this.QualityIcon.color = ItemQuality.GetColor(this.qualityItemInstance.Quality);
			base.UpdateUI();
		}

		// Token: 0x04003C21 RID: 15393
		public Image QualityIcon;

		// Token: 0x04003C22 RID: 15394
		protected QualityItemInstance qualityItemInstance;
	}
}
