using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008AB RID: 2219
	public class RandomIntervalEvent : MonoBehaviour
	{
		// Token: 0x06003C34 RID: 15412 RVA: 0x000FDC37 File Offset: 0x000FBE37
		private void OnEnable()
		{
			if (this.ExecuteOnEnable)
			{
				this.Execute();
			}
			this.nextInterval = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x000FDC64 File Offset: 0x000FBE64
		private void Update()
		{
			if (Time.time >= this.nextInterval)
			{
				this.Execute();
			}
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x000FDC79 File Offset: 0x000FBE79
		private void Execute()
		{
			if (this.OnInterval != null)
			{
				this.OnInterval.Invoke();
			}
			this.nextInterval = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
		}

		// Token: 0x04002AF9 RID: 11001
		public float MinInterval = 5f;

		// Token: 0x04002AFA RID: 11002
		public float MaxInterval = 10f;

		// Token: 0x04002AFB RID: 11003
		public bool ExecuteOnEnable;

		// Token: 0x04002AFC RID: 11004
		public UnityEvent OnInterval;

		// Token: 0x04002AFD RID: 11005
		private float nextInterval;
	}
}
