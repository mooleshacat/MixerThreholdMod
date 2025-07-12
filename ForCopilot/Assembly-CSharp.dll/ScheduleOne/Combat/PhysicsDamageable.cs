using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000778 RID: 1912
	public class PhysicsDamageable : MonoBehaviour, IDamageable
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x000D6012 File Offset: 0x000D4212
		// (set) Token: 0x0600336D RID: 13165 RVA: 0x000D601A File Offset: 0x000D421A
		public Vector3 averageVelocity { get; private set; } = Vector3.zero;

		// Token: 0x0600336E RID: 13166 RVA: 0x000D6023 File Offset: 0x000D4223
		public void OnValidate()
		{
			if (this.Rb == null)
			{
				this.Rb = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000D603F File Offset: 0x000D423F
		public virtual void SendImpact(Impact impact)
		{
			this.ReceiveImpact(impact);
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000D6048 File Offset: 0x000D4248
		public virtual void ReceiveImpact(Impact impact)
		{
			if (this.impactHistory.Contains(impact.ImpactID))
			{
				return;
			}
			this.impactHistory.Add(impact.ImpactID);
			if (this.onImpacted != null)
			{
				this.onImpacted(impact);
			}
			if (this.Rb != null)
			{
				this.Rb.AddForceAtPosition(-impact.Hit.normal * impact.ImpactForce * this.ForceMultiplier, impact.Hit.point, 1);
			}
		}

		// Token: 0x0400244D RID: 9293
		public const int VELOCITY_HISTORY_LENGTH = 4;

		// Token: 0x0400244E RID: 9294
		public Rigidbody Rb;

		// Token: 0x0400244F RID: 9295
		public float ForceMultiplier = 1f;

		// Token: 0x04002450 RID: 9296
		private List<int> impactHistory = new List<int>();

		// Token: 0x04002451 RID: 9297
		public Action<Impact> onImpacted;

		// Token: 0x04002453 RID: 9299
		private List<Vector3> velocityHistory = new List<Vector3>();
	}
}
