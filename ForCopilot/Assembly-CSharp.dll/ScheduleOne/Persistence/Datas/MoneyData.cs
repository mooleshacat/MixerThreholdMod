using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000439 RID: 1081
	[Serializable]
	public class MoneyData : SaveData
	{
		// Token: 0x0600166F RID: 5743 RVA: 0x00063DFA File Offset: 0x00061FFA
		public MoneyData(float onlineBalance, float netWorth, float lifetimeEarnings, float weeklyDepositSum)
		{
			this.OnlineBalance = onlineBalance;
			this.Networth = netWorth;
			this.LifetimeEarnings = lifetimeEarnings;
			this.WeeklyDepositSum = weeklyDepositSum;
		}

		// Token: 0x0400144C RID: 5196
		public float OnlineBalance;

		// Token: 0x0400144D RID: 5197
		public float Networth;

		// Token: 0x0400144E RID: 5198
		public float LifetimeEarnings;

		// Token: 0x0400144F RID: 5199
		public float WeeklyDepositSum;
	}
}
