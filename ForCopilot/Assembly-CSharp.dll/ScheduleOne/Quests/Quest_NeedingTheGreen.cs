using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FE RID: 766
	public class Quest_NeedingTheGreen : Quest
	{
		// Token: 0x0600111E RID: 4382 RVA: 0x0004C28C File Offset: 0x0004A48C
		protected override void MinPass()
		{
			base.MinPass();
			string text = MoneyManager.FormatAmount(this.LifetimeEarningsRequirement, false, false);
			this.EarnEntry.SetEntryTitle(string.Concat(new string[]
			{
				"Earn ",
				text,
				" (",
				MoneyManager.FormatAmount(NetworkSingleton<MoneyManager>.Instance.LifetimeEarnings, false, false),
				" / ",
				text,
				")"
			}));
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.QuestState == EQuestState.Inactive)
			{
				Quest[] prerequisiteQuests = this.PrerequisiteQuests;
				for (int i = 0; i < prerequisiteQuests.Length; i++)
				{
					if (prerequisiteQuests[i].QuestState != EQuestState.Completed)
					{
						return;
					}
				}
				this.Begin(true);
			}
		}

		// Token: 0x04001118 RID: 4376
		public Quest[] PrerequisiteQuests;

		// Token: 0x04001119 RID: 4377
		public QuestEntry EarnEntry;

		// Token: 0x0400111A RID: 4378
		public float LifetimeEarningsRequirement = 10000f;
	}
}
