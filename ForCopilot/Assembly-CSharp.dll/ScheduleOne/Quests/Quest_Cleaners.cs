using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F5 RID: 757
	public class Quest_Cleaners : Quest_Employees
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x0004BD4C File Offset: 0x00049F4C
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Cleaner).Configuration as CleanerConfiguration).binItems.Count > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0004BDD4 File Offset: 0x00049FD4
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Cleaner);
		}

		// Token: 0x04001108 RID: 4360
		public QuestEntry AssignWorkEntry;
	}
}
