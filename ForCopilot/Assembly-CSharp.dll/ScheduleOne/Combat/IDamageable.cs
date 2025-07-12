using System;

namespace ScheduleOne.Combat
{
	// Token: 0x02000775 RID: 1909
	public interface IDamageable
	{
		// Token: 0x06003366 RID: 13158
		void SendImpact(Impact impact);

		// Token: 0x06003367 RID: 13159
		void ReceiveImpact(Impact impact);
	}
}
