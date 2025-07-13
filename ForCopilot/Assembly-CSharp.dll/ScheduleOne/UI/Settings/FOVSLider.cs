using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB8 RID: 2744
	public class FOVSLider : SettingsSlider
	{
		// Token: 0x060049C6 RID: 18886 RVA: 0x001367F2 File Offset: 0x001349F2
		protected virtual void Start()
		{
			this.slider.SetValueWithoutNotify(Singleton<Settings>.Instance.GraphicsSettings.FOV);
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x0013680E File Offset: 0x00134A0E
		protected override void OnValueChanged(float value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.FOV = value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
