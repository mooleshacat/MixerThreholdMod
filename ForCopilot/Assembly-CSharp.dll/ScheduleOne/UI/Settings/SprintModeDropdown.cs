using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC6 RID: 2758
	public class SprintModeDropdown : SettingsDropdown
	{
		// Token: 0x06004A01 RID: 18945 RVA: 0x00136FA4 File Offset: 0x001351A4
		protected override void Awake()
		{
			base.Awake();
			InputSettings.EActionMode[] array = (InputSettings.EActionMode[])Enum.GetValues(typeof(InputSettings.EActionMode));
			for (int i = 0; i < array.Length; i++)
			{
				string option = array[i].ToString();
				base.AddOption(option);
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x00136FF4 File Offset: 0x001351F4
		protected virtual void Start()
		{
			this.dropdown.SetValueWithoutNotify((int)Singleton<Settings>.Instance.InputSettings.SprintMode);
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x00137010 File Offset: 0x00135210
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.InputSettings.SprintMode = (InputSettings.EActionMode)value;
			Singleton<Settings>.Instance.ReloadInputSettings();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
		}
	}
}
