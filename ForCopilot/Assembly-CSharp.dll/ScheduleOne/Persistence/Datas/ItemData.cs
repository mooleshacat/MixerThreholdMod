using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000418 RID: 1048
	[Serializable]
	public class ItemData : SaveData
	{
		// Token: 0x0600164D RID: 5709 RVA: 0x00063B81 File Offset: 0x00061D81
		public ItemData(string iD, int quantity)
		{
			this.ID = iD;
			this.Quantity = quantity;
		}

		// Token: 0x04001416 RID: 5142
		public string ID;

		// Token: 0x04001417 RID: 5143
		public int Quantity;
	}
}
