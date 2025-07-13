using System;
using FishNet;
using ScheduleOne.Economy;

namespace ScheduleOne.Quests
{
	// Token: 0x02000301 RID: 769
	public class Quest_SecuringSupplies : Quest
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x0004C435 File Offset: 0x0004A635
		protected override void MinPass()
		{
			base.MinPass();
			if (InstanceFinder.IsServer)
			{
				EQuestState questState = base.QuestState;
			}
		}

		// Token: 0x0400111D RID: 4381
		public Supplier Supplier;
	}
}
