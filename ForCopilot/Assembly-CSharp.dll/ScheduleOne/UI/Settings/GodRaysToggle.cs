using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000ABA RID: 2746
	public class GodRaysToggle : SettingsToggle
	{
		// Token: 0x060049CE RID: 18894 RVA: 0x001368AD File Offset: 0x00134AAD
		protected virtual void Start()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.GraphicsSettings.GodRays);
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x001368C9 File Offset: 0x00134AC9
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.GodRays = value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
