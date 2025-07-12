using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.MainMenu;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000ABF RID: 2751
	public class MonitorDropdown : SettingsDropdown
	{
		// Token: 0x060049E5 RID: 18917 RVA: 0x00136BA0 File Offset: 0x00134DA0
		protected override void Awake()
		{
			base.Awake();
			Display[] displays = Display.displays;
			for (int i = 0; i < displays.Length; i++)
			{
				base.AddOption("Monitor " + (i + 1).ToString());
			}
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x00136BE4 File Offset: 0x00134DE4
		protected virtual void OnEnable()
		{
			Display[] displays = Display.displays;
			for (int i = 0; i < displays.Length; i++)
			{
				bool active = displays[i].active;
			}
			this.dropdown.SetValueWithoutNotify(Mathf.Clamp(MonitorDropdown.GetCurrentDisplayNumber(), 0, this.dropdown.options.Count - 1));
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00136C36 File Offset: 0x00134E36
		protected override void OnValueChanged(int value)
		{
			base.OnValueChanged(value);
			Singleton<Settings>.Instance.UnappliedDisplaySettings.ActiveDisplayIndex = value;
			base.GetComponentInParent<SettingsScreen>().DisplayChanged();
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x00136C5A File Offset: 0x00134E5A
		public static int GetCurrentDisplayNumber()
		{
			List<DisplayInfo> list = new List<DisplayInfo>();
			Screen.GetDisplayLayout(list);
			return list.IndexOf(Screen.mainWindowDisplayInfo);
		}
	}
}
