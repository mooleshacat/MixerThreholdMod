using System;

namespace ScheduleOne.Economy
{
	// Token: 0x020006B0 RID: 1712
	public struct DealWindowInfo
	{
		// Token: 0x06002EE2 RID: 12002 RVA: 0x000C49CB File Offset: 0x000C2BCB
		public DealWindowInfo(int startTime, int endTime)
		{
			this.StartTime = startTime;
			this.EndTime = endTime;
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000C49DB File Offset: 0x000C2BDB
		public static DealWindowInfo GetWindowInfo(EDealWindow window)
		{
			switch (window)
			{
			case EDealWindow.Morning:
				return DealWindowInfo.Morning;
			case EDealWindow.Afternoon:
				return DealWindowInfo.Afternoon;
			case EDealWindow.Night:
				return DealWindowInfo.Night;
			case EDealWindow.LateNight:
				return DealWindowInfo.LateNight;
			default:
				return DealWindowInfo.Morning;
			}
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000C4A14 File Offset: 0x000C2C14
		public static EDealWindow GetWindow(int time)
		{
			if (time >= DealWindowInfo.Morning.StartTime && time < DealWindowInfo.Morning.EndTime)
			{
				return EDealWindow.Morning;
			}
			if (time >= DealWindowInfo.Afternoon.StartTime && time < DealWindowInfo.Afternoon.EndTime)
			{
				return EDealWindow.Afternoon;
			}
			if (time >= DealWindowInfo.Night.StartTime && time < DealWindowInfo.Night.EndTime)
			{
				return EDealWindow.Night;
			}
			return EDealWindow.LateNight;
		}

		// Token: 0x040020F7 RID: 8439
		public const int WINDOW_DURATION_MINS = 360;

		// Token: 0x040020F8 RID: 8440
		public const int WINDOW_COUNT = 4;

		// Token: 0x040020F9 RID: 8441
		public int StartTime;

		// Token: 0x040020FA RID: 8442
		public int EndTime;

		// Token: 0x040020FB RID: 8443
		public static readonly DealWindowInfo Morning = new DealWindowInfo(600, 1200);

		// Token: 0x040020FC RID: 8444
		public static readonly DealWindowInfo Afternoon = new DealWindowInfo(1200, 1800);

		// Token: 0x040020FD RID: 8445
		public static readonly DealWindowInfo Night = new DealWindowInfo(1800, 2400);

		// Token: 0x040020FE RID: 8446
		public static readonly DealWindowInfo LateNight = new DealWindowInfo(0, 600);
	}
}
