using System;

namespace ScheduleOne.Combat
{
	// Token: 0x02000774 RID: 1908
	public struct ExplosionData
	{
		// Token: 0x06003364 RID: 13156 RVA: 0x000D5F38 File Offset: 0x000D4138
		public ExplosionData(float damageRadius, float maxDamage, float maxPushForce)
		{
			this.DamageRadius = damageRadius;
			this.MaxDamage = maxDamage;
			this.PushForceRadius = damageRadius * 2f;
			this.MaxPushForce = maxPushForce;
		}

		// Token: 0x04002439 RID: 9273
		public float DamageRadius;

		// Token: 0x0400243A RID: 9274
		public float MaxDamage;

		// Token: 0x0400243B RID: 9275
		public float PushForceRadius;

		// Token: 0x0400243C RID: 9276
		public float MaxPushForce;

		// Token: 0x0400243D RID: 9277
		public static readonly ExplosionData DefaultSmall = new ExplosionData(6f, 200f, 500f);
	}
}
