using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC1 RID: 2753
	public class ResolutionDropdown : SettingsDropdown
	{
		// Token: 0x060049EE RID: 18926 RVA: 0x00136D18 File Offset: 0x00134F18
		protected override void Awake()
		{
			base.Awake();
			foreach (Resolution resolution in DisplaySettings.GetResolutions().ToArray())
			{
				base.AddOption(resolution.width.ToString() + "x" + resolution.height.ToString());
			}
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x00136D7A File Offset: 0x00134F7A
		protected virtual void OnEnable()
		{
			this.dropdown.SetValueWithoutNotify(Mathf.Clamp(Singleton<Settings>.Instance.DisplaySettings.ResolutionIndex, 0, this.dropdown.options.Count - 1));
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x00136DAE File Offset: 0x00134FAE
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.ResolutionIndex = value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}
	}
}
