using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000ABC RID: 2748
	public class InvertYToggle : SettingsToggle
	{
		// Token: 0x060049D5 RID: 18901 RVA: 0x001369AB File Offset: 0x00134BAB
		protected virtual void Start()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.InputSettings.InvertMouse);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x001369C7 File Offset: 0x00134BC7
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.InputSettings.InvertMouse = value;
			Singleton<Settings>.Instance.ReloadInputSettings();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
		}
	}
}
