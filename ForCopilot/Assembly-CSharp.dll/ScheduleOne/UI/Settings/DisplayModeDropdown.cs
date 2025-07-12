using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB7 RID: 2743
	public class DisplayModeDropdown : SettingsDropdown
	{
		// Token: 0x060049C2 RID: 18882 RVA: 0x00136740 File Offset: 0x00134940
		protected override void Awake()
		{
			base.Awake();
			DisplaySettings.EDisplayMode[] array = (DisplaySettings.EDisplayMode[])Enum.GetValues(typeof(DisplaySettings.EDisplayMode));
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToString();
				text = text.Replace("ExclusiveFullscreen", "Exclusive Fullscreen");
				text = text.Replace("FullscreenWindow", "Fullscreen Window");
				base.AddOption(text);
			}
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x001367B2 File Offset: 0x001349B2
		protected virtual void OnEnable()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.DisplaySettings.DisplayMode);
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x001367CE File Offset: 0x001349CE
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.DisplayMode = (DisplaySettings.EDisplayMode)value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
