using System;
using Steamworks;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009F7 RID: 2551
	public class OpenSteamOverlay : MonoBehaviour
	{
		// Token: 0x060044AF RID: 17583 RVA: 0x00120730 File Offset: 0x0011E930
		public void OpenOverlay()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			OpenSteamOverlay.EType type = this.Type;
			if (type == OpenSteamOverlay.EType.Store)
			{
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(3164500U), 0);
				return;
			}
			if (type != OpenSteamOverlay.EType.CustomLink)
			{
				return;
			}
			SteamFriends.ActivateGameOverlayToWebPage(this.CustomLink, 0);
		}

		// Token: 0x0400318A RID: 12682
		public const uint APP_ID = 3164500U;

		// Token: 0x0400318B RID: 12683
		public OpenSteamOverlay.EType Type;

		// Token: 0x0400318C RID: 12684
		public string CustomLink;

		// Token: 0x020009F8 RID: 2552
		public enum EType
		{
			// Token: 0x0400318E RID: 12686
			Store,
			// Token: 0x0400318F RID: 12687
			CustomLink
		}
	}
}
