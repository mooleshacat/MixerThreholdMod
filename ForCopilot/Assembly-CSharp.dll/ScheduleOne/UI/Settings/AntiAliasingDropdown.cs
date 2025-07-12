using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB3 RID: 2739
	public class AntiAliasingDropdown : SettingsDropdown
	{
		// Token: 0x060049B1 RID: 18865 RVA: 0x0013631C File Offset: 0x0013451C
		protected override void Awake()
		{
			base.Awake();
			GraphicsSettings.EAntiAliasingMode[] array = (GraphicsSettings.EAntiAliasingMode[])Enum.GetValues(typeof(GraphicsSettings.EAntiAliasingMode));
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToString();
				text = text.Replace("MSAAx2", "2x MSAA");
				text = text.Replace("MSAAx4", "4x MSAA");
				text = text.Replace("MSAAx8", "8x MSAA");
				base.AddOption(text);
			}
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x0013639F File Offset: 0x0013459F
		protected virtual void Start()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.GraphicsSettings.AntiAliasingMode);
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x001363BB File Offset: 0x001345BB
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.GraphicsSettings.AntiAliasingMode = (GraphicsSettings.EAntiAliasingMode)value;
			Singleton<Settings>.Instance.ReloadGraphicsSettings();
			Singleton<Settings>.Instance.WriteGraphicsSettings(Singleton<Settings>.Instance.GraphicsSettings);
		}
	}
}
