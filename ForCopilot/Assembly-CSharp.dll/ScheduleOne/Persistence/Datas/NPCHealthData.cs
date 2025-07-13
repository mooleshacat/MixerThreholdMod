using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200043D RID: 1085
	public class NPCHealthData : SaveData
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x00063E9F File Offset: 0x0006209F
		public NPCHealthData(float health, bool isDead, int daysPassedSinceDeath)
		{
			this.Health = health;
			this.IsDead = isDead;
			this.DaysPassedSinceDeath = daysPassedSinceDeath;
		}

		// Token: 0x04001457 RID: 5207
		public float Health;

		// Token: 0x04001458 RID: 5208
		public bool IsDead;

		// Token: 0x04001459 RID: 5209
		public int DaysPassedSinceDeath;
	}
}
