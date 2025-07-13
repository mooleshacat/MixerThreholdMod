using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B5D RID: 2909
	public class ContinueScreen : MainMenuScreen
	{
		// Token: 0x06004D6D RID: 19821 RVA: 0x00145FAA File Offset: 0x001441AA
		private void Update()
		{
			if (base.IsOpen)
			{
				this.NotHostWarning.gameObject.SetActive(!Singleton<Lobby>.Instance.IsHost);
			}
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x00145FD1 File Offset: 0x001441D1
		public void LoadGame(int index)
		{
			if (!Singleton<Lobby>.Instance.IsHost)
			{
				Console.LogWarning("Only the host can start the game.", null);
				return;
			}
			Singleton<LoadManager>.Instance.StartGame(LoadManager.SaveGames[index], false);
		}

		// Token: 0x040039B1 RID: 14769
		public RectTransform NotHostWarning;
	}
}
