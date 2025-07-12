using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000421 RID: 1057
	[Serializable]
	public class LaunderOperationData : SaveData
	{
		// Token: 0x06001656 RID: 5718 RVA: 0x00063BFA File Offset: 0x00061DFA
		public LaunderOperationData(float amount, int minutesSinceStarted)
		{
			this.Amount = amount;
			this.MinutesSinceStarted = minutesSinceStarted;
		}

		// Token: 0x0400141F RID: 5151
		public float Amount;

		// Token: 0x04001420 RID: 5152
		public int MinutesSinceStarted;
	}
}
