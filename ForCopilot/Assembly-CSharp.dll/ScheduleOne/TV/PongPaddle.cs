using System;
using UnityEngine;

namespace ScheduleOne.TV
{
	// Token: 0x020002A9 RID: 681
	public class PongPaddle : MonoBehaviour
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0003ED78 File Offset: 0x0003CF78
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x0003ED80 File Offset: 0x0003CF80
		public float TargetY { get; set; }

		// Token: 0x06000E2F RID: 3631 RVA: 0x0003ED89 File Offset: 0x0003CF89
		public void SetTargetY(float y)
		{
			this.TargetY = y;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0003ED92 File Offset: 0x0003CF92
		private void Update()
		{
			this.UpdateMove();
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0003ED9C File Offset: 0x0003CF9C
		private void UpdateMove()
		{
			float num = this.Rect.anchoredPosition.y;
			num = Mathf.Lerp(num, this.TargetY, 20f * Time.deltaTime * this.SpeedMultiplier);
			num = Mathf.Clamp(num, -160f, 160f);
			this.Rect.anchoredPosition = new Vector3(this.Rect.anchoredPosition.x, num);
		}

		// Token: 0x04000E9B RID: 3739
		public const float BOUND_Y = 160f;

		// Token: 0x04000E9C RID: 3740
		public const float MOVE_SPEED = 20f;

		// Token: 0x04000E9D RID: 3741
		public float SpeedMultiplier = 1f;

		// Token: 0x04000E9F RID: 3743
		public RectTransform Rect;
	}
}
