using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000464 RID: 1124
	[Serializable]
	public class DeaddropQuestData : QuestData
	{
		// Token: 0x0600169C RID: 5788 RVA: 0x000645B8 File Offset: 0x000627B8
		public DeaddropQuestData(string guid, EQuestState state, bool isTracked, string title, string desc, bool isTimed, GameDateTimeData expiry, QuestEntryData[] entries, string deaddropGUID) : base(guid, state, isTracked, title, desc, isTimed, expiry, entries)
		{
			this.DeaddropGUID = deaddropGUID;
		}

		// Token: 0x040014D6 RID: 5334
		public string DeaddropGUID;
	}
}
