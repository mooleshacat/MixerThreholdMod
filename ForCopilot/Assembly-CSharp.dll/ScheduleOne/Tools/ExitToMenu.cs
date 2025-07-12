using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000893 RID: 2195
	public class ExitToMenu : MonoBehaviour
	{
		// Token: 0x06003BD2 RID: 15314 RVA: 0x000FCE14 File Offset: 0x000FB014
		public void Exit()
		{
			Singleton<LoadManager>.Instance.ExitToMenu(null, null, false);
		}
	}
}
