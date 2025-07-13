using System;
using FishNet;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB9 RID: 2745
	public class GameSettingsWindow : MonoBehaviour
	{
		// Token: 0x060049C9 RID: 18889 RVA: 0x00136845 File Offset: 0x00134A45
		private void Awake()
		{
			this.ConsoleToggle.onValueChanged.AddListener(new UnityAction<bool>(this.ConsoleToggled));
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00136863 File Offset: 0x00134A63
		public void Start()
		{
			this.ApplySettings(NetworkSingleton<GameManager>.Instance.Settings);
			this.Blocker.SetActive(!InstanceFinder.IsServer);
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x00136888 File Offset: 0x00134A88
		public void ApplySettings(GameSettings settings)
		{
			this.ConsoleToggle.SetIsOnWithoutNotify(settings.ConsoleEnabled);
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x0013689B File Offset: 0x00134A9B
		private void ConsoleToggled(bool value)
		{
			NetworkSingleton<GameManager>.Instance.Settings.ConsoleEnabled = value;
		}

		// Token: 0x04003653 RID: 13907
		public Toggle ConsoleToggle;

		// Token: 0x04003654 RID: 13908
		public GameObject Blocker;
	}
}
