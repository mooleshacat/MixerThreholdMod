using System;
using FishNet;
using ScheduleOne.Economy;

namespace ScheduleOne.Quests
{
	// Token: 0x0200030A RID: 778
	public class Quest_WeNeedToCook : Quest
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x0004D008 File Offset: 0x0004B208
		protected override void MinPass()
		{
			base.MinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.QuestState == EQuestState.Inactive)
			{
				if (!this.MethSupplier.RelationData.Unlocked)
				{
					return;
				}
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

		// Token: 0x04001145 RID: 4421
		public Quest[] PrerequisiteQuests;

		// Token: 0x04001146 RID: 4422
		public Supplier MethSupplier;
	}
}
