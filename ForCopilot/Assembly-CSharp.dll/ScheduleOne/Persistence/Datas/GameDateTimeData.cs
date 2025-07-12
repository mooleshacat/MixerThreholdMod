using System;
using ScheduleOne.GameTime;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000409 RID: 1033
	[Serializable]
	public class GameDateTimeData : SaveData
	{
		// Token: 0x06001631 RID: 5681 RVA: 0x000638DC File Offset: 0x00061ADC
		public GameDateTimeData(int _elapsedDays, int _time)
		{
			this.ElapsedDays = _elapsedDays;
			this.Time = _time;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000638F2 File Offset: 0x00061AF2
		public GameDateTimeData(GameDateTime gameDateTime)
		{
			this.ElapsedDays = gameDateTime.elapsedDays;
			this.Time = gameDateTime.time;
		}

		// Token: 0x040013FF RID: 5119
		public int ElapsedDays;

		// Token: 0x04001400 RID: 5120
		public int Time;
	}
}
