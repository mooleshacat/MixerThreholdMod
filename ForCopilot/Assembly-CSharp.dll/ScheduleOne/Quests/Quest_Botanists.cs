using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F2 RID: 754
	public class Quest_Botanists : Quest_Employees
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x0004BA14 File Offset: 0x00049C14
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignSuppliesEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Botanist).Configuration as BotanistConfiguration).Supplies.SelectedObject != null)
						{
							this.AssignSuppliesEntry.Complete();
							break;
						}
					}
				}
			}
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Botanist).Configuration as BotanistConfiguration).AssignedPots.Count > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
			if (this.AssignDestinationEntry.State == EQuestState.Active)
			{
				foreach (Employee employee in this.GetEmployees())
				{
					using (List<Pot>.Enumerator enumerator2 = ((employee as Botanist).Configuration as BotanistConfiguration).AssignedPots.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if ((enumerator2.Current.Configuration as PotConfiguration).Destination.SelectedObject != null)
							{
								this.AssignDestinationEntry.Complete();
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004BBD0 File Offset: 0x00049DD0
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Botanist);
		}

		// Token: 0x04001102 RID: 4354
		public QuestEntry AssignSuppliesEntry;

		// Token: 0x04001103 RID: 4355
		public QuestEntry AssignWorkEntry;

		// Token: 0x04001104 RID: 4356
		public QuestEntry AssignDestinationEntry;
	}
}
