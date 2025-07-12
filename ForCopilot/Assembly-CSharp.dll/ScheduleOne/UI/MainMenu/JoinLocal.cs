using System;
using FishNet.Managing;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B61 RID: 2913
	public class JoinLocal : MonoBehaviour
	{
		// Token: 0x06004D7F RID: 19839 RVA: 0x00146433 File Offset: 0x00144633
		private void Awake()
		{
			base.gameObject.SetActive(Application.isEditor || Debug.isDebugBuild);
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0014644F File Offset: 0x0014464F
		public void Clicked()
		{
			UnityEngine.Object.FindObjectOfType<NetworkManager>().ClientManager.StartConnection();
		}
	}
}
