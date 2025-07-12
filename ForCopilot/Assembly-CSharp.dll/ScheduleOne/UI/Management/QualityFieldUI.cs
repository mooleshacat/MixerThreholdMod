using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B35 RID: 2869
	public class QualityFieldUI : MonoBehaviour
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x00142150 File Offset: 0x00140350
		// (set) Token: 0x06004C7F RID: 19583 RVA: 0x00142158 File Offset: 0x00140358
		public List<QualityField> Fields { get; protected set; } = new List<QualityField>();

		// Token: 0x06004C80 RID: 19584 RVA: 0x00142164 File Offset: 0x00140364
		public void Bind(List<QualityField> field)
		{
			this.Fields = new List<QualityField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onValueChanged.AddListener(new UnityAction<EQuality>(this.Refresh));
			for (int i = 0; i < this.QualityButtons.Length; i++)
			{
				EQuality quality = (EQuality)i;
				this.QualityButtons[i].onClick.AddListener(delegate()
				{
					this.ValueChanged(quality);
				});
			}
			this.Refresh(this.Fields[0].Value);
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x00142214 File Offset: 0x00140414
		private void Refresh(EQuality value)
		{
			if (this.AreFieldsUniform())
			{
				EQuality value2 = this.Fields[0].Value;
				for (int i = 0; i < this.QualityButtons.Length; i++)
				{
					EQuality equality = (EQuality)i;
					this.QualityButtons[i].interactable = (equality != value2);
				}
				return;
			}
			Button[] qualityButtons = this.QualityButtons;
			for (int j = 0; j < qualityButtons.Length; j++)
			{
				qualityButtons[j].interactable = true;
			}
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00142288 File Offset: 0x00140488
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].Value != this.Fields[i + 1].Value)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x001422D8 File Offset: 0x001404D8
		public void ValueChanged(EQuality value)
		{
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].SetValue(value, true);
			}
		}

		// Token: 0x0400390B RID: 14603
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x0400390C RID: 14604
		public Button[] QualityButtons;
	}
}
