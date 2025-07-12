using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;

namespace ScheduleOne.Quests
{
	// Token: 0x02000300 RID: 768
	public class Quest_Packagers : Quest_Employees
	{
		// Token: 0x06001122 RID: 4386 RVA: 0x0004C3A4 File Offset: 0x0004A5A4
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Packager).Configuration as PackagerConfiguration).AssignedStationCount > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0004C428 File Offset: 0x0004A628
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Handler);
		}

		// Token: 0x0400111C RID: 4380
		public QuestEntry AssignWorkEntry;
	}
}
