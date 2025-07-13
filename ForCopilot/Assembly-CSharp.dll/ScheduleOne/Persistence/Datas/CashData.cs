using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000414 RID: 1044
	[Serializable]
	public class CashData : ItemData
	{
		// Token: 0x06001649 RID: 5705 RVA: 0x00063B41 File Offset: 0x00061D41
		public CashData(string iD, int quantity, float cashBalance) : base(iD, quantity)
		{
			this.CashBalance = cashBalance;
		}

		// Token: 0x04001413 RID: 5139
		public float CashBalance;
	}
}
