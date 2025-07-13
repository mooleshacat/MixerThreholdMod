using System;
using ScheduleOne.Economy;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FD RID: 765
	public class Quest_MovingUp : Quest
	{
		// Token: 0x0600111C RID: 4380 RVA: 0x0004C220 File Offset: 0x0004A420
		protected override void MinPass()
		{
			base.MinPass();
			if (this.ReachCustomersEntry.State == EQuestState.Active)
			{
				int count = Customer.UnlockedCustomers.Count;
				this.ReachCustomersEntry.SetEntryTitle("Unlock 10 customers (" + count.ToString() + "/10)");
				if (count >= 10 && this.ReachCustomersEntry.State != EQuestState.Completed)
				{
					this.ReachCustomersEntry.Complete();
				}
			}
		}

		// Token: 0x04001117 RID: 4375
		public QuestEntry ReachCustomersEntry;
	}
}
