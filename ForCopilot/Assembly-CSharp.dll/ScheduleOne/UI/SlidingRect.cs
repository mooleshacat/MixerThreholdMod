using System;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A8F RID: 2703
	public class SlidingRect : MonoBehaviour
	{
		// Token: 0x060048A6 RID: 18598 RVA: 0x00130DF8 File Offset: 0x0012EFF8
		public void Update()
		{
			this._time += Time.deltaTime * this.SpeedMultiplier;
			if (this._time > this.Duration)
			{
				this._time -= this.Duration;
			}
			float t = this._time / this.Duration;
			this.Rect.anchoredPosition = Vector2.Lerp(this.Start, this.End, t);
		}

		// Token: 0x0400354C RID: 13644
		public RectTransform Rect;

		// Token: 0x0400354D RID: 13645
		public Vector2 Start;

		// Token: 0x0400354E RID: 13646
		public Vector2 End;

		// Token: 0x0400354F RID: 13647
		public float Duration = 1f;

		// Token: 0x04003550 RID: 13648
		public float SpeedMultiplier = 1f;

		// Token: 0x04003551 RID: 13649
		private float _time;
	}
}
