using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB5 RID: 2741
	public class CameraBobSlider : SettingsSlider
	{
		// Token: 0x060049B8 RID: 18872 RVA: 0x0013656C File Offset: 0x0013476C
		protected virtual void Start()
		{
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.DisplaySettings.CameraBobbing * 10f);
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x0013658E File Offset: 0x0013478E
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.CameraBobbing = value / 10f;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
