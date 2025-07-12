using System;
using ScheduleOne.Economy;
using ScheduleOne.NPCs.CharacterClasses;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FC RID: 764
	public class Quest_GettingStarted : Quest
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x0004C1FB File Offset: 0x0004A3FB
		protected override void MinPass()
		{
			base.MinPass();
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0004C203 File Offset: 0x0004A403
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
		}

		// Token: 0x04001114 RID: 4372
		public float CashAmount = 375f;

		// Token: 0x04001115 RID: 4373
		public DeadDrop CashDrop;

		// Token: 0x04001116 RID: 4374
		public UncleNelson Nelson;
	}
}
