using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC7 RID: 2759
	public class SSAOToggle : SettingsToggle
	{
		// Token: 0x06004A05 RID: 18949 RVA: 0x00137047 File Offset: 0x00135247
		protected virtual void Start()
		{
			this.toggle.SetIsOnWithoutNotify(Singleton<Settings>.Instance.GraphicsSettings.SSAO);
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x00137063 File Offset: 0x00135263
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.SSAO = value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
