using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC0 RID: 2752
	public class QualityDropdown : SettingsDropdown
	{
		// Token: 0x060049EA RID: 18922 RVA: 0x00136C74 File Offset: 0x00134E74
		protected override void Awake()
		{
			base.Awake();
			GraphicsSettings.EGraphicsQuality[] array = (GraphicsSettings.EGraphicsQuality[])Enum.GetValues(typeof(GraphicsSettings.EGraphicsQuality));
			for (int i = 0; i < array.Length; i++)
			{
				string option = array[i].ToString();
				base.AddOption(option);
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x00136CC4 File Offset: 0x00134EC4
		protected virtual void Start()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.GraphicsSettings.GraphicsQuality);
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00136CE0 File Offset: 0x00134EE0
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.GraphicsQuality = (GraphicsSettings.EGraphicsQuality)value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
