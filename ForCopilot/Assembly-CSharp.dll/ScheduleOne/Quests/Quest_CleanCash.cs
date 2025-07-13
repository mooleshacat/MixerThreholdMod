using System;
using FishNet;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F4 RID: 756
	public class Quest_CleanCash : Quest
	{
		// Token: 0x06001100 RID: 4352 RVA: 0x0004BC7C File Offset: 0x00049E7C
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Inactive && InstanceFinder.IsServer && ATM.WeeklyDepositSum >= 10000f)
			{
				this.Begin(true);
			}
			if (base.QuestState == EQuestState.Completed)
			{
				return;
			}
			if (InstanceFinder.IsServer && this.BuyBusinessEntry.State == EQuestState.Active && Business.OwnedBusinesses.Count > 0)
			{
				this.BuyBusinessEntry.Complete();
			}
			if (this.GoToBusinessEntry.State == EQuestState.Active)
			{
				if (Business.OwnedBusinesses.Count > 0)
				{
					this.GoToBusinessEntry.transform.position = Business.OwnedBusinesses[0].PoI.transform.position;
				}
				if (Player.Local.CurrentBusiness != null)
				{
					this.GoToBusinessEntry.Complete();
				}
			}
		}

		// Token: 0x04001106 RID: 4358
		public QuestEntry BuyBusinessEntry;

		// Token: 0x04001107 RID: 4359
		public QuestEntry GoToBusinessEntry;
	}
}
