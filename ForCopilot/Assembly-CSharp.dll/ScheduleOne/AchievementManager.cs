using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using Steamworks;

namespace ScheduleOne
{
	// Token: 0x0200027F RID: 639
	public class AchievementManager : PersistentSingleton<AchievementManager>
	{
		// Token: 0x06000D6C RID: 3436 RVA: 0x0003B3E0 File Offset: 0x000395E0
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<AchievementManager>.Instance == null || Singleton<AchievementManager>.Instance != this)
			{
				return;
			}
			this.achievements = (AchievementManager.EAchievement[])Enum.GetValues(typeof(AchievementManager.EAchievement));
			foreach (AchievementManager.EAchievement key in this.achievements)
			{
				this.achievementUnlocked.Add(key, false);
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0003B44E File Offset: 0x0003964E
		protected override void Start()
		{
			base.Start();
			if (Singleton<AchievementManager>.Instance == null || Singleton<AchievementManager>.Instance != this)
			{
				return;
			}
			this.PullAchievements();
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003B478 File Offset: 0x00039678
		private void PullAchievements()
		{
			if (!SteamManager.Initialized)
			{
				Console.LogWarning("Steamworks not initialized, cannot pull achievement stats", null);
				return;
			}
			foreach (AchievementManager.EAchievement key in this.achievements)
			{
				bool value;
				SteamUserStats.GetAchievement(key.ToString(), ref value);
				this.achievementUnlocked[key] = value;
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003B4D4 File Offset: 0x000396D4
		public void UnlockAchievement(AchievementManager.EAchievement achievement)
		{
			if (!SteamManager.Initialized)
			{
				Console.LogWarning("Steamworks not initialized, cannot unlock achievement", null);
				return;
			}
			if (this.achievementUnlocked[achievement])
			{
				return;
			}
			Console.Log(string.Format("Unlocking achievement: {0}", achievement), null);
			SteamUserStats.SetAchievement(achievement.ToString());
			SteamUserStats.StoreStats();
			this.achievementUnlocked[achievement] = true;
		}

		// Token: 0x04000DB9 RID: 3513
		private AchievementManager.EAchievement[] achievements;

		// Token: 0x04000DBA RID: 3514
		private Dictionary<AchievementManager.EAchievement, bool> achievementUnlocked = new Dictionary<AchievementManager.EAchievement, bool>();

		// Token: 0x02000280 RID: 640
		public enum EAchievement
		{
			// Token: 0x04000DBC RID: 3516
			COMPLETE_PROLOGUE,
			// Token: 0x04000DBD RID: 3517
			RV_DESTROYED,
			// Token: 0x04000DBE RID: 3518
			DEALER_RECRUITED,
			// Token: 0x04000DBF RID: 3519
			MASTER_CHEF,
			// Token: 0x04000DC0 RID: 3520
			BUSINESSMAN,
			// Token: 0x04000DC1 RID: 3521
			BIGWIG,
			// Token: 0x04000DC2 RID: 3522
			MAGNATE,
			// Token: 0x04000DC3 RID: 3523
			UPSTANDING_CITIZEN,
			// Token: 0x04000DC4 RID: 3524
			ROLLING_IN_STYLE,
			// Token: 0x04000DC5 RID: 3525
			LONG_ARM_OF_THE_LAW,
			// Token: 0x04000DC6 RID: 3526
			INDIAN_DEALER
		}
	}
}
