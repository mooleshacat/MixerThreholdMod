using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B31 RID: 2865
	public class NumberFieldUI : MonoBehaviour
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x0014168E File Offset: 0x0013F88E
		// (set) Token: 0x06004C61 RID: 19553 RVA: 0x00141696 File Offset: 0x0013F896
		public List<NumberField> Fields { get; protected set; } = new List<NumberField>();

		// Token: 0x06004C62 RID: 19554 RVA: 0x001416A0 File Offset: 0x0013F8A0
		public void Bind(List<NumberField> field)
		{
			this.Fields = new List<NumberField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onItemChanged.AddListener(new UnityAction<float>(this.Refresh));
			this.MinValueLabel.text = this.Fields[0].MinValue.ToString();
			this.MaxValueLabel.text = this.Fields[0].MaxValue.ToString();
			this.Slider.minValue = this.Fields[0].MinValue;
			this.Slider.maxValue = this.Fields[0].MaxValue;
			this.Slider.wholeNumbers = this.Fields[0].WholeNumbers;
			this.Slider.onValueChanged.AddListener(new UnityAction<float>(this.ValueChanged));
			this.Refresh(this.Fields[0].Value);
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x001417C1 File Offset: 0x0013F9C1
		private void Refresh(float newVal)
		{
			if (this.AreFieldsUniform())
			{
				this.ValueLabel.text = newVal.ToString();
			}
			else
			{
				this.ValueLabel.text = "#";
			}
			this.Slider.SetValueWithoutNotify(newVal);
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x001417FC File Offset: 0x0013F9FC
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

		// Token: 0x06004C65 RID: 19557 RVA: 0x0014184C File Offset: 0x0013FA4C
		public void ValueChanged(float value)
		{
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].SetValue(value, true);
			}
		}

		// Token: 0x040038EF RID: 14575
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x040038F0 RID: 14576
		public Slider Slider;

		// Token: 0x040038F1 RID: 14577
		public TextMeshProUGUI ValueLabel;

		// Token: 0x040038F2 RID: 14578
		public TextMeshProUGUI MinValueLabel;

		// Token: 0x040038F3 RID: 14579
		public TextMeshProUGUI MaxValueLabel;
	}
}
