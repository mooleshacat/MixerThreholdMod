using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000444 RID: 1092
	[Serializable]
	public class PackagerData : EmployeeData
	{
		// Token: 0x0600167B RID: 5755 RVA: 0x00064034 File Offset: 0x00062234
		public PackagerData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x04001470 RID: 5232
		public MoveItemData MoveItemData;
	}
}
