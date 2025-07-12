using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Variables
{
	// Token: 0x0200029E RID: 670
	[Serializable]
	public class QuestCondition
	{
		// Token: 0x06000DE3 RID: 3555 RVA: 0x0003D780 File Offset: 0x0003B980
		public bool Evaluate()
		{
			Quest quest = Quest.GetQuest(this.QuestName);
			if (quest == null)
			{
				Console.LogError("Quest " + this.QuestName + " not found", null);
				return false;
			}
			if (this.CheckQuestState && quest.QuestState != this.QuestState)
			{
				return false;
			}
			if (this.CheckQuestEntryState)
			{
				if (quest.Entries.Count <= this.QuestEntryIndex)
				{
					Console.LogError("Quest " + this.QuestName + " does not have entry " + this.QuestEntryIndex.ToString(), null);
					return false;
				}
				if (quest.Entries[this.QuestEntryIndex].State != this.QuestEntryState)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000E54 RID: 3668
		public bool CheckQuestState = true;

		// Token: 0x04000E55 RID: 3669
		public string QuestName = "Quest name";

		// Token: 0x04000E56 RID: 3670
		public EQuestState QuestState = EQuestState.Active;

		// Token: 0x04000E57 RID: 3671
		public bool CheckQuestEntryState;

		// Token: 0x04000E58 RID: 3672
		public int QuestEntryIndex;

		// Token: 0x04000E59 RID: 3673
		public EQuestState QuestEntryState = EQuestState.Active;
	}
}
