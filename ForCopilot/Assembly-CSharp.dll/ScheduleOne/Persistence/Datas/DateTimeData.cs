using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000403 RID: 1027
	[Serializable]
	public class DateTimeData : SaveData
	{
		// Token: 0x06001621 RID: 5665 RVA: 0x00063564 File Offset: 0x00061764
		public DateTimeData(DateTime date)
		{
			this.Year = date.Year;
			this.Month = date.Month;
			this.Day = date.Day;
			this.Hour = date.Hour;
			this.Minute = date.Minute;
			this.Second = date.Second;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x000635C5 File Offset: 0x000617C5
		public DateTime GetDateTime()
		{
			return new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second);
		}

		// Token: 0x040013ED RID: 5101
		public int Year;

		// Token: 0x040013EE RID: 5102
		public int Month;

		// Token: 0x040013EF RID: 5103
		public int Day;

		// Token: 0x040013F0 RID: 5104
		public int Hour;

		// Token: 0x040013F1 RID: 5105
		public int Minute;

		// Token: 0x040013F2 RID: 5106
		public int Second;
	}
}
