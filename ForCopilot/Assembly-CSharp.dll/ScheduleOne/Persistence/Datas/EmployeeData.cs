using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000442 RID: 1090
	[Serializable]
	public class EmployeeData : NPCData
	{
		// Token: 0x06001679 RID: 5753 RVA: 0x00063F78 File Offset: 0x00062178
		public EmployeeData(string id, string assignedProperty, string firstName, string lastName, bool isMale, int appearanceIndex, Vector3 position, Quaternion rotation, Guid guid, bool paidForToday) : base(id)
		{
			this.AssignedProperty = assignedProperty;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.IsMale = isMale;
			this.AppearanceIndex = appearanceIndex;
			this.Position = position;
			this.Rotation = rotation;
			this.GUID = guid.ToString();
			this.PaidForToday = paidForToday;
		}

		// Token: 0x04001463 RID: 5219
		public string AssignedProperty;

		// Token: 0x04001464 RID: 5220
		public string FirstName;

		// Token: 0x04001465 RID: 5221
		public string LastName;

		// Token: 0x04001466 RID: 5222
		public bool IsMale;

		// Token: 0x04001467 RID: 5223
		public int AppearanceIndex;

		// Token: 0x04001468 RID: 5224
		public Vector3 Position;

		// Token: 0x04001469 RID: 5225
		public Quaternion Rotation;

		// Token: 0x0400146A RID: 5226
		public string GUID;

		// Token: 0x0400146B RID: 5227
		public bool PaidForToday;
	}
}
