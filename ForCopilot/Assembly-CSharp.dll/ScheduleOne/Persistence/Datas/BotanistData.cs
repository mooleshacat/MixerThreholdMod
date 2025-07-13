using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200043E RID: 1086
	[Serializable]
	public class BotanistData : EmployeeData
	{
		// Token: 0x06001675 RID: 5749 RVA: 0x00063EBC File Offset: 0x000620BC
		public BotanistData(string id, string assignedProperty, string firstName, string lastName, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday, MoveItemData moveItemData) : base(id, assignedProperty, firstName, lastName, male, appearanceIndex, position, rotation, guid, paidForToday)
		{
			this.MoveItemData = moveItemData;
		}

		// Token: 0x0400145A RID: 5210
		public MoveItemData MoveItemData;
	}
}
