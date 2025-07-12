using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000466 RID: 1126
	[Serializable]
	public class QuestEntryData : SaveData
	{
		// Token: 0x0600169E RID: 5790 RVA: 0x00064630 File Offset: 0x00062830
		public QuestEntryData(string name, EQuestState state)
		{
			this.Name = name;
			this.State = state;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0006355C File Offset: 0x0006175C
		public QuestEntryData()
		{
		}

		// Token: 0x040014DF RID: 5343
		public string Name;

		// Token: 0x040014E0 RID: 5344
		public EQuestState State;
	}
}
