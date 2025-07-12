using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Quests
{
	// Token: 0x0200030B RID: 779
	[Serializable]
	public class QuestStateSetter
	{
		// Token: 0x0600115D RID: 4445 RVA: 0x0004D068 File Offset: 0x0004B268
		public void Execute()
		{
			Quest quest = Quest.GetQuest(this.QuestName);
			if (quest == null)
			{
				Console.LogWarning("Failed to find quest with name: " + this.QuestName, null);
				return;
			}
			if (this.SetQuestState)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(quest.GUID.ToString(), this.QuestState);
			}
			if (this.SetQuestEntryState)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestEntryState(quest.GUID.ToString(), this.QuestEntryIndex, this.QuestEntryState);
			}
		}

		// Token: 0x04001147 RID: 4423
		public string QuestName;

		// Token: 0x04001148 RID: 4424
		public bool SetQuestState;

		// Token: 0x04001149 RID: 4425
		public QuestManager.EQuestAction QuestState;

		// Token: 0x0400114A RID: 4426
		public bool SetQuestEntryState;

		// Token: 0x0400114B RID: 4427
		public int QuestEntryIndex;

		// Token: 0x0400114C RID: 4428
		public EQuestState QuestEntryState;
	}
}
