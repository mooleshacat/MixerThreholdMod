using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007C6 RID: 1990
	public class BuildStop_Base : MonoBehaviour
	{
		// Token: 0x060035F6 RID: 13814 RVA: 0x000E1DB8 File Offset: 0x000DFFB8
		public virtual void Stop_Building()
		{
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
			}
			base.GetComponent<BuildUpdate_Base>().Stop();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
