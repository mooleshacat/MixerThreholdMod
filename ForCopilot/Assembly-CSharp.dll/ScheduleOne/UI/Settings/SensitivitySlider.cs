using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC2 RID: 2754
	public class SensitivitySlider : SettingsSlider
	{
		// Token: 0x060049F2 RID: 18930 RVA: 0x00136DD2 File Offset: 0x00134FD2
		protected virtual void Start()
		{
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.InputSettings.MouseSensitivity / 0.033333335f);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x00136DF4 File Offset: 0x00134FF4
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.InputSettings.MouseSensitivity = value * 0.033333335f;
			Singleton<Settings>.Instance.ReloadInputSettings();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
		}

		// Token: 0x0400365B RID: 13915
		public const float MULTIPLIER = 0.033333335f;
	}
}
