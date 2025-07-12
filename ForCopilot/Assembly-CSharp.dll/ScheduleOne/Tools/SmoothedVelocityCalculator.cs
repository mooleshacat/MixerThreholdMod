using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B3 RID: 2227
	public class SmoothedVelocityCalculator : MonoBehaviour
	{
		// Token: 0x06003C49 RID: 15433 RVA: 0x000FE353 File Offset: 0x000FC553
		private void Start()
		{
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x000FE368 File Offset: 0x000FC568
		protected virtual void FixedUpdate()
		{
			if (this.zeroOut)
			{
				this.Velocity = Vector3.zero;
				return;
			}
			Vector3 item = (base.transform.position - this.lastFramePosition) / Time.fixedDeltaTime;
			if (item.magnitude <= this.MaxReasonableVelocity)
			{
				this.VelocityHistory.Add(new Tuple<Vector3, float>(item, Time.timeSinceLevelLoad));
			}
			if (this.VelocityHistory.Count > this.maxSamples)
			{
				this.VelocityHistory.RemoveAt(0);
			}
			this.Velocity = this.GetAverageVelocity();
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x000FE40C File Offset: 0x000FC60C
		private Vector3 GetAverageVelocity()
		{
			Vector3 a = Vector3.zero;
			int num = 0;
			int num2 = this.VelocityHistory.Count - 1;
			while (num2 >= 0 && Time.timeSinceLevelLoad - this.VelocityHistory[num2].Item2 <= this.SampleLength)
			{
				a += this.VelocityHistory[num2].Item1;
				num++;
				num2--;
			}
			if (num == 0)
			{
				return Vector3.zero;
			}
			return a / (float)num;
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x000FE487 File Offset: 0x000FC687
		public void FlushBuffer()
		{
			this.VelocityHistory.Clear();
			this.Velocity = Vector3.zero;
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x000FE4B0 File Offset: 0x000FC6B0
		public void ZeroOut(float duration)
		{
			SmoothedVelocityCalculator.<>c__DisplayClass11_0 CS$<>8__locals1 = new SmoothedVelocityCalculator.<>c__DisplayClass11_0();
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.<>4__this = this;
			this.zeroOut = true;
			base.StartCoroutine(CS$<>8__locals1.<ZeroOut>g__Routine|0());
		}

		// Token: 0x04002B10 RID: 11024
		public Vector3 Velocity = Vector3.zero;

		// Token: 0x04002B11 RID: 11025
		[Header("Settings")]
		public float SampleLength = 0.2f;

		// Token: 0x04002B12 RID: 11026
		public float MaxReasonableVelocity = 25f;

		// Token: 0x04002B13 RID: 11027
		private List<Tuple<Vector3, float>> VelocityHistory = new List<Tuple<Vector3, float>>();

		// Token: 0x04002B14 RID: 11028
		private int maxSamples = 20;

		// Token: 0x04002B15 RID: 11029
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x04002B16 RID: 11030
		private bool zeroOut;
	}
}
