using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Employees;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F8 RID: 760
	public abstract class Quest_Employees : Quest
	{
		// Token: 0x0600110B RID: 4363
		public abstract List<Employee> GetEmployees();

		// Token: 0x0600110C RID: 4364 RVA: 0x0004BEE8 File Offset: 0x0004A0E8
		protected override void MinPass()
		{
			base.MinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.AssignBedEntry.State == EQuestState.Active && this.AreAnyEmployeesAssignedBeds())
			{
				this.AssignBedEntry.Complete();
			}
			if (this.PayEntry.State == EQuestState.Active && this.AreAnyEmployeesPaid())
			{
				this.PayEntry.Complete();
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0004BF48 File Offset: 0x0004A148
		protected bool AreAnyEmployeesAssignedBeds()
		{
			using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetHome() != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0004BFA8 File Offset: 0x0004A1A8
		protected bool AreAnyEmployeesPaid()
		{
			using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.PaidForToday)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001109 RID: 4361
		public EEmployeeType EmployeeType;

		// Token: 0x0400110A RID: 4362
		public QuestEntry AssignBedEntry;

		// Token: 0x0400110B RID: 4363
		public QuestEntry PayEntry;
	}
}
