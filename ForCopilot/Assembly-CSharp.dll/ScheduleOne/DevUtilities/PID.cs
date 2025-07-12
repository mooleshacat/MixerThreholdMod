using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000730 RID: 1840
	[Serializable]
	public class PID
	{
		// Token: 0x060031C6 RID: 12742 RVA: 0x000CFDF5 File Offset: 0x000CDFF5
		public PID(float pFactor, float iFactor, float dFactor)
		{
			this.pFactor = pFactor;
			this.iFactor = iFactor;
			this.dFactor = dFactor;
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000CFE14 File Offset: 0x000CE014
		public float Update(float setpoint, float actual, float timeFrame)
		{
			float num = setpoint - actual;
			this.integral += num * timeFrame;
			float num2 = (num - this.lastError) / timeFrame;
			this.lastError = num;
			return num * this.pFactor + this.integral * this.iFactor + num2 * this.dFactor;
		}

		// Token: 0x0400230B RID: 8971
		public float pFactor;

		// Token: 0x0400230C RID: 8972
		public float iFactor;

		// Token: 0x0400230D RID: 8973
		public float dFactor;

		// Token: 0x0400230E RID: 8974
		private float integral;

		// Token: 0x0400230F RID: 8975
		private float lastError;
	}
}
