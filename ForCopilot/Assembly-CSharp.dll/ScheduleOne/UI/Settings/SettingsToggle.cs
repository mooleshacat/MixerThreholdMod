using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AC5 RID: 2757
	public class SettingsToggle : MonoBehaviour
	{
		// Token: 0x060049FE RID: 18942 RVA: 0x00136F79 File Offset: 0x00135179
		protected virtual void Awake()
		{
			this.toggle = base.GetComponent<Toggle>();
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void OnValueChanged(bool value)
		{
		}

		// Token: 0x04003663 RID: 13923
		protected Toggle toggle;
	}
}
