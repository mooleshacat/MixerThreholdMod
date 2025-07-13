using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC4 RID: 2756
	public class SettingsSlider : MonoBehaviour
	{
		// Token: 0x060049F9 RID: 18937 RVA: 0x00136EA4 File Offset: 0x001350A4
		protected virtual void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
			this.valueLabel = this.slider.handleRect.Find("Value").GetComponent<TextMeshProUGUI>();
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00136EFA File Offset: 0x001350FA
		protected virtual void Update()
		{
			if (this.DisplayValue && Time.time - this.timeOnValueChange > 2f)
			{
				this.valueLabel.enabled = false;
			}
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x00136F23 File Offset: 0x00135123
		protected virtual void OnValueChanged(float value)
		{
			this.timeOnValueChange = Time.time;
			if (this.DisplayValue)
			{
				this.valueLabel.text = this.GetDisplayValue(value);
				this.valueLabel.enabled = true;
			}
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x00136F56 File Offset: 0x00135156
		protected virtual string GetDisplayValue(float value)
		{
			return value.ToString();
		}

		// Token: 0x0400365E RID: 13918
		private const float VALUE_DISPLAY_TIME = 2f;

		// Token: 0x0400365F RID: 13919
		public bool DisplayValue = true;

		// Token: 0x04003660 RID: 13920
		protected Slider slider;

		// Token: 0x04003661 RID: 13921
		protected TextMeshProUGUI valueLabel;

		// Token: 0x04003662 RID: 13922
		protected float timeOnValueChange = -100f;
	}
}
