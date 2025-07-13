using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC9 RID: 2761
	public class VSyncToggle : SettingsToggle
	{
		// Token: 0x06004A0B RID: 18955 RVA: 0x001370E0 File Offset: 0x001352E0
		protected virtual void OnEnable()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.DisplaySettings.VSync);
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x001370FC File Offset: 0x001352FC
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.VSync = value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
