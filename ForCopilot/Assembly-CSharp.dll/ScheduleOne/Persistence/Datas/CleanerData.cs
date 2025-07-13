using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000440 RID: 1088
	[Serializable]
	public class CleanerData : EmployeeData
	{
		// Token: 0x06001677 RID: 5751 RVA: 0x00063F14 File Offset: 0x00062114
		public CleanerData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x0400145C RID: 5212
		public MoveItemData MoveItemData;
	}
}
