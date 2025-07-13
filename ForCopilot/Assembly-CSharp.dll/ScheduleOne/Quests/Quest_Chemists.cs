using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F3 RID: 755
	public class Quest_Chemists : Quest_Employees
	{
		// Token: 0x060010FD RID: 4349 RVA: 0x0004BBE8 File Offset: 0x00049DE8
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Chemist).Configuration as ChemistConfiguration).TotalStations > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0004BC6C File Offset: 0x00049E6C
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Chemist);
		}

		// Token: 0x04001105 RID: 4357
		public QuestEntry AssignWorkEntry;
	}
}
