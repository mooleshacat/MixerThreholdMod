using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002B7 RID: 695
	[Serializable]
	public struct GameDateTime
	{
		// Token: 0x06000E96 RID: 3734 RVA: 0x00040A1B File Offset: 0x0003EC1B
		public GameDateTime(int _elapsedDays, int _time)
		{
			this.elapsedDays = _elapsedDays;
			this.time = _time;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00040A2C File Offset: 0x0003EC2C
		public GameDateTime(int _minSum)
		{
			this.elapsedDays = _minSum / 1440;
			int minSum = _minSum % 1440;
			if (_minSum < 0)
			{
				minSum = -_minSum % 1440;
			}
			this.time = TimeManager.Get24HourTimeFromMinSum(minSum);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00040A67 File Offset: 0x0003EC67
		public GameDateTime(GameDateTimeData data)
		{
			this.elapsedDays = data.ElapsedDays;
			this.time = data.Time;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00040A81 File Offset: 0x0003EC81
		public int GetMinSum()
		{
			return this.elapsedDays * 1440 + TimeManager.GetMinSumFrom24HourTime(this.time);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00040A9B File Offset: 0x0003EC9B
		public GameDateTime AddMins(int mins)
		{
			return new GameDateTime(this.GetMinSum() + mins);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00040AAA File Offset: 0x0003ECAA
		public static GameDateTime operator +(GameDateTime a, GameDateTime b)
		{
			return new GameDateTime(a.GetMinSum() + b.GetMinSum());
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00040AC0 File Offset: 0x0003ECC0
		public static GameDateTime operator -(GameDateTime a, GameDateTime b)
		{
			return new GameDateTime(a.GetMinSum() - b.GetMinSum());
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00040AD6 File Offset: 0x0003ECD6
		public static bool operator >(GameDateTime a, GameDateTime b)
		{
			return a.GetMinSum() > b.GetMinSum();
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00040AE8 File Offset: 0x0003ECE8
		public static bool operator <(GameDateTime a, GameDateTime b)
		{
			return a.GetMinSum() < b.GetMinSum();
		}

		// Token: 0x04000F14 RID: 3860
		public int elapsedDays;

		// Token: 0x04000F15 RID: 3861
		public int time;
	}
}
