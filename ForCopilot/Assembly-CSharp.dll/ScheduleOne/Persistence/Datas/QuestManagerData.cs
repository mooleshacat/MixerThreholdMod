using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000467 RID: 1127
	public class QuestManagerData : SaveData
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x00064646 File Offset: 0x00062846
		public QuestManagerData(QuestData[] quests, ContractData[] contracts, DeaddropQuestData[] deaddropQuests)
		{
			this.Quests = quests;
			this.Contracts = contracts;
			this.DeaddropQuests = deaddropQuests;
		}

		// Token: 0x040014E1 RID: 5345
		public QuestData[] Quests;

		// Token: 0x040014E2 RID: 5346
		public ContractData[] Contracts;

		// Token: 0x040014E3 RID: 5347
		public DeaddropQuestData[] DeaddropQuests;
	}
}
