using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FF RID: 767
	public class Quest_OnTheGrind : Quest
	{
		// Token: 0x06001120 RID: 4384 RVA: 0x0004C34C File Offset: 0x0004A54C
		protected override void MinPass()
		{
			base.MinPass();
			int num = Mathf.RoundToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Completed_Contracts_Count"));
			if (this.CompleteDealsEntry.State == EQuestState.Active)
			{
				this.CompleteDealsEntry.SetEntryTitle("Complete 3 deals (" + num.ToString() + "/3)");
			}
		}

		// Token: 0x0400111B RID: 4379
		public QuestEntry CompleteDealsEntry;
	}
}
