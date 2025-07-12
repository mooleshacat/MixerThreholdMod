using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200043F RID: 1087
	[Serializable]
	public class ChemistData : EmployeeData
	{
		// Token: 0x06001676 RID: 5750 RVA: 0x00063EE8 File Offset: 0x000620E8
		public ChemistData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x0400145B RID: 5211
		public MoveItemData MoveItemData;
	}
}
