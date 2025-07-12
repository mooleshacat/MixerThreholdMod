using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC3 RID: 2755
	public class SettingsDropdown : MonoBehaviour
	{
		// Token: 0x060049F5 RID: 18933 RVA: 0x00136E34 File Offset: 0x00135034
		protected virtual void Awake()
		{
			this.dropdown = base.GetComponent<TMP_Dropdown>();
			this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
			foreach (string option in this.DefaultOptions)
			{
				this.AddOption(option);
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void OnValueChanged(int value)
		{
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x00136E8A File Offset: 0x0013508A
		protected void AddOption(string option)
		{
			this.dropdown.options.Add(new TMP_Dropdown.OptionData(option));
		}

		// Token: 0x0400365C RID: 13916
		public string[] DefaultOptions;

		// Token: 0x0400365D RID: 13917
		protected TMP_Dropdown dropdown;
	}
}
