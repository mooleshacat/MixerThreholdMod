using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000441 RID: 1089
	public class DealerData : NPCData
	{
		// Token: 0x06001678 RID: 5752 RVA: 0x00063F40 File Offset: 0x00062140
		public DealerData(string id, bool recruited, string[] assignedCustomerIDs, string[] activeContractGUIDs, float cash, ItemSet overflowItems, bool hasBeenRecommended) : base(id)
		{
			this.Recruited = recruited;
			this.AssignedCustomerIDs = assignedCustomerIDs;
			this.ActiveContractGUIDs = activeContractGUIDs;
			this.Cash = cash;
			this.OverflowItems = overflowItems;
			this.HasBeenRecommended = hasBeenRecommended;
		}

		// Token: 0x0400145D RID: 5213
		public bool Recruited;

		// Token: 0x0400145E RID: 5214
		public string[] AssignedCustomerIDs;

		// Token: 0x0400145F RID: 5215
		public string[] ActiveContractGUIDs;

		// Token: 0x04001460 RID: 5216
		public float Cash;

		// Token: 0x04001461 RID: 5217
		public ItemSet OverflowItems;

		// Token: 0x04001462 RID: 5218
		public bool HasBeenRecommended;
	}
}
