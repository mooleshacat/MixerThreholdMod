using System;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x0200083B RID: 2107
	[Serializable]
	public struct PID_Parameters
	{
		// Token: 0x060038F3 RID: 14579 RVA: 0x000F06CB File Offset: 0x000EE8CB
		public PID_Parameters(float P, float I, float D)
		{
			this.P = P;
			this.I = I;
			this.D = D;
		}

		// Token: 0x040028EB RID: 10475
		public float P;

		// Token: 0x040028EC RID: 10476
		public float I;

		// Token: 0x040028ED RID: 10477
		public float D;
	}
}
