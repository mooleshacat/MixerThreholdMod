using System;
using ScheduleOne.NPCs.Relation;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F6 RID: 758
	public class Quest_Connections : Quest
	{
		// Token: 0x06001105 RID: 4357 RVA: 0x0004BDE4 File Offset: 0x00049FE4
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			foreach (QuestEntry questEntry in this.Entries)
			{
				if (questEntry.GetComponent<NPCUnlockTracker>().Npc.RelationData.Unlocked)
				{
					questEntry.SetState(EQuestState.Completed, true);
				}
				else
				{
					questEntry.SetState(EQuestState.Active, true);
				}
			}
		}
	}
}
