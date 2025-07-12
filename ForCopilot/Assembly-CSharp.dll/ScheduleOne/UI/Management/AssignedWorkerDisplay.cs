using System;
using ScheduleOne.NPCs;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B1E RID: 2846
	public class AssignedWorkerDisplay : MonoBehaviour
	{
		// Token: 0x06004C21 RID: 19489 RVA: 0x00140595 File Offset: 0x0013E795
		public void Set(NPC npc)
		{
			if (npc != null)
			{
				this.Icon.sprite = npc.MugshotSprite;
			}
			base.gameObject.SetActive(npc != null);
		}

		// Token: 0x040038B4 RID: 14516
		public Image Icon;
	}
}
