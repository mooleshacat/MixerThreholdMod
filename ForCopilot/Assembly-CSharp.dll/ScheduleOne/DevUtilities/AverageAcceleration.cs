using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000713 RID: 1811
	public class AverageAcceleration : MonoBehaviour
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x000CCDF8 File Offset: 0x000CAFF8
		// (set) Token: 0x0600310F RID: 12559 RVA: 0x000CCE00 File Offset: 0x000CB000
		public Vector3 Acceleration { get; private set; } = Vector3.zero;

		// Token: 0x06003110 RID: 12560 RVA: 0x000CCE0C File Offset: 0x000CB00C
		private void Start()
		{
			if (this.Rb == null)
			{
				this.Rb = base.GetComponent<Rigidbody>();
			}
			this.accelerations = new Vector3[Mathf.CeilToInt(this.TimeWindow / Time.fixedDeltaTime)];
			for (int i = 0; i < this.accelerations.Length; i++)
			{
				this.accelerations[i] = Vector3.zero;
			}
			this.prevVelocity = this.Rb.velocity;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000CCE84 File Offset: 0x000CB084
		private void FixedUpdate()
		{
			this.timer += Time.fixedDeltaTime;
			if (this.timer >= this.TimeWindow)
			{
				this.timer -= Time.fixedDeltaTime;
				this.accelerations[this.currentIndex] = Vector3.zero;
				this.currentIndex = (this.currentIndex + 1) % this.accelerations.Length;
			}
			Vector3 vector = (this.Rb.velocity - this.prevVelocity) / Time.fixedDeltaTime;
			this.accelerations[this.currentIndex] = vector;
			this.prevVelocity = this.Rb.velocity;
			Vector3 a = Vector3.zero;
			for (int i = 0; i < this.accelerations.Length; i++)
			{
				a += this.accelerations[i];
			}
			this.Acceleration = a / (float)this.accelerations.Length;
		}

		// Token: 0x04002274 RID: 8820
		public Rigidbody Rb;

		// Token: 0x04002275 RID: 8821
		public float TimeWindow = 0.5f;

		// Token: 0x04002276 RID: 8822
		private Vector3[] accelerations;

		// Token: 0x04002277 RID: 8823
		private int currentIndex;

		// Token: 0x04002278 RID: 8824
		private float timer;

		// Token: 0x04002279 RID: 8825
		private Vector3 prevVelocity;
	}
}
