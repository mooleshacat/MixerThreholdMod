using System;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000BC1 RID: 3009
	public class ItemInfoContent : MonoBehaviour
	{
		// Token: 0x06004FE2 RID: 20450 RVA: 0x00150EFE File Offset: 0x0014F0FE
		public virtual void Initialize(ItemInstance instance)
		{
			this.NameLabel.text = instance.Name;
			this.DescriptionLabel.text = instance.Description;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00150F22 File Offset: 0x0014F122
		public virtual void Initialize(ItemDefinition definition)
		{
			this.NameLabel.text = definition.Name;
			this.DescriptionLabel.text = definition.Description;
		}

		// Token: 0x04003BE2 RID: 15330
		[Header("Settings")]
		public float Height = 90f;

		// Token: 0x04003BE3 RID: 15331
		[Header("References")]
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003BE4 RID: 15332
		public TextMeshProUGUI DescriptionLabel;
	}
}
