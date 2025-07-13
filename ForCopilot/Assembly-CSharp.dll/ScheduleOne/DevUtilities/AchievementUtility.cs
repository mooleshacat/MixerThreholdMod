using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x0200070E RID: 1806
	public class AchievementUtility : MonoBehaviour
	{
		// Token: 0x06003105 RID: 12549 RVA: 0x000CCDAE File Offset: 0x000CAFAE
		public void UnlockAchievement()
		{
			Singleton<AchievementManager>.Instance.UnlockAchievement(this.Achievement);
		}

		// Token: 0x04002270 RID: 8816
		public AchievementManager.EAchievement Achievement;
	}
}
