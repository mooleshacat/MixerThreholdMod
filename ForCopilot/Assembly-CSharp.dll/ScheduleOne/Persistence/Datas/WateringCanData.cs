using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041F RID: 1055
	[Serializable]
	public class WateringCanData : ItemData
	{
		// Token: 0x06001654 RID: 5716 RVA: 0x00063BE9 File Offset: 0x00061DE9
		public WateringCanData(string iD, int quantity, float currentFillLevel) : base(iD, quantity)
		{
			this.CurrentFillAmount = currentFillLevel;
		}

		// Token: 0x0400141E RID: 5150
		public float CurrentFillAmount;
	}
}
