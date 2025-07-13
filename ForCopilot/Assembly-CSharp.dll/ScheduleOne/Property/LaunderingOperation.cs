using System;

namespace ScheduleOne.Property
{
	// Token: 0x0200084D RID: 2125
	public class LaunderingOperation
	{
		// Token: 0x0600397B RID: 14715 RVA: 0x000F408D File Offset: 0x000F228D
		public LaunderingOperation(Business _business, float _amount, int _minutesSinceStarted)
		{
			this.business = _business;
			this.amount = _amount;
			this.minutesSinceStarted = _minutesSinceStarted;
		}

		// Token: 0x04002981 RID: 10625
		public Business business;

		// Token: 0x04002982 RID: 10626
		public float amount;

		// Token: 0x04002983 RID: 10627
		public int minutesSinceStarted;

		// Token: 0x04002984 RID: 10628
		public int completionTime_Minutes = 1440;
	}
}
