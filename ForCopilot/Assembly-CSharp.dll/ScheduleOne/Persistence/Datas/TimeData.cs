using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200046E RID: 1134
	[Serializable]
	public class TimeData : SaveData
	{
		// Token: 0x060016AC RID: 5804 RVA: 0x000647BD File Offset: 0x000629BD
		public TimeData(int timeOfDay, int elapsedDays, int playtime)
		{
			this.TimeOfDay = timeOfDay;
			this.ElapsedDays = elapsedDays;
			this.Playtime = playtime;
		}

		// Token: 0x040014F4 RID: 5364
		public int TimeOfDay;

		// Token: 0x040014F5 RID: 5365
		public int ElapsedDays;

		// Token: 0x040014F6 RID: 5366
		public int Playtime;
	}
}
