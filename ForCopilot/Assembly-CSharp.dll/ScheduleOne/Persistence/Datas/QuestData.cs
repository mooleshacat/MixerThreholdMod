using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000465 RID: 1125
	[Serializable]
	public class QuestData : SaveData
	{
		// Token: 0x0600169D RID: 5789 RVA: 0x000645E0 File Offset: 0x000627E0
		public QuestData(string guid, EQuestState state, bool isTracked, string title, string desc, bool expires, GameDateTimeData expiry, QuestEntryData[] entries)
		{
			this.GUID = guid;
			this.State = state;
			this.IsTracked = isTracked;
			this.Title = title;
			this.Description = desc;
			this.Expires = expires;
			this.ExpiryDate = expiry;
			this.Entries = entries;
		}

		// Token: 0x040014D7 RID: 5335
		public string GUID;

		// Token: 0x040014D8 RID: 5336
		public EQuestState State;

		// Token: 0x040014D9 RID: 5337
		public bool IsTracked;

		// Token: 0x040014DA RID: 5338
		public string Title;

		// Token: 0x040014DB RID: 5339
		public string Description;

		// Token: 0x040014DC RID: 5340
		public bool Expires;

		// Token: 0x040014DD RID: 5341
		public GameDateTimeData ExpiryDate;

		// Token: 0x040014DE RID: 5342
		public QuestEntryData[] Entries;
	}
}
