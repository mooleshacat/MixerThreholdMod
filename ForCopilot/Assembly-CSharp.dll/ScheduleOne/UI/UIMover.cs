using System;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A92 RID: 2706
	public class UIMover : MonoBehaviour
	{
		// Token: 0x060048B5 RID: 18613 RVA: 0x00131020 File Offset: 0x0012F220
		private void Start()
		{
			this.speed = new Vector2(UnityEngine.Random.Range(this.MinSpeed.x, this.MaxSpeed.x), UnityEngine.Random.Range(this.MinSpeed.y, this.MaxSpeed.y));
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x00131070 File Offset: 0x0012F270
		public void Update()
		{
			Vector2 b = this.speed * this.SpeedMultiplier * Time.deltaTime;
			this.Rect.anchoredPosition += b;
		}

		// Token: 0x04003556 RID: 13654
		public RectTransform Rect;

		// Token: 0x04003557 RID: 13655
		public Vector2 MinSpeed = Vector2.one;

		// Token: 0x04003558 RID: 13656
		public Vector2 MaxSpeed = Vector2.one;

		// Token: 0x04003559 RID: 13657
		public float SpeedMultiplier = 1f;

		// Token: 0x0400355A RID: 13658
		private Vector2 speed = Vector2.zero;
	}
}
