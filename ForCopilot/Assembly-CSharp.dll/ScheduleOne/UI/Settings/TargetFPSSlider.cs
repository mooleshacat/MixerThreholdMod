using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC8 RID: 2760
	public class TargetFPSSlider : SettingsSlider
	{
		// Token: 0x06004A08 RID: 18952 RVA: 0x0013709A File Offset: 0x0013529A
		protected virtual void OnEnable()
		{
			this.slider.SetValueWithoutNotify((float)Singleton<Settings>.Instance.DisplaySettings.TargetFPS);
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x001370B7 File Offset: 0x001352B7
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.TargetFPS = Mathf.RoundToInt(value);
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
