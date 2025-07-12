using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000ABB RID: 2747
	public class IntefaceScaleSlider : SettingsSlider
	{
		// Token: 0x060049D1 RID: 18897 RVA: 0x00136908 File Offset: 0x00134B08
		protected virtual void OnEnable()
		{
			this.slider.minValue = 7f;
			this.slider.maxValue = 14f;
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.DisplaySettings.UIScale / 0.1f);
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00136955 File Offset: 0x00134B55
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.UIScale = value * 0.1f;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00136980 File Offset: 0x00134B80
		protected override string GetDisplayValue(float value)
		{
			return Mathf.Round(value * 10f).ToString() + "%";
		}

		// Token: 0x04003655 RID: 13909
		public const float MULTIPLIER = 0.1f;

		// Token: 0x04003656 RID: 13910
		public const float MinScale = 0.7f;

		// Token: 0x04003657 RID: 13911
		public const float MaxScale = 1.4f;
	}
}
