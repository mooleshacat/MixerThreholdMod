using System;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BC9 RID: 3017
	public class QualityItemInfoContent : ItemInfoContent
	{
		// Token: 0x06005021 RID: 20513 RVA: 0x00152F90 File Offset: 0x00151190
		public override void Initialize(ItemInstance instance)
		{
			base.Initialize(instance);
			QualityItemInstance qualityItemInstance = instance as QualityItemInstance;
			if (qualityItemInstance == null)
			{
				Console.LogError("QualityItemInfoContent can only be used with QualityItemInstance!", null);
				return;
			}
			this.QualityLabel.text = qualityItemInstance.Quality.ToString();
			this.QualityLabel.color = ItemQuality.GetColor(qualityItemInstance.Quality);
			this.Star.color = ItemQuality.GetColor(qualityItemInstance.Quality);
			this.QualityLabel.gameObject.SetActive(true);
		}

		// Token: 0x04003C1F RID: 15391
		public Image Star;

		// Token: 0x04003C20 RID: 15392
		public TextMeshProUGUI QualityLabel;
	}
}
