using System;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A8D RID: 2701
	public class DestroyUIAtBounds : MonoBehaviour
	{
		// Token: 0x0600489E RID: 18590 RVA: 0x00130BE4 File Offset: 0x0012EDE4
		public void Update()
		{
			if (this.Rect.anchoredPosition.x < this.MinBounds.x || this.Rect.anchoredPosition.x > this.MaxBounds.x || this.Rect.anchoredPosition.y < this.MinBounds.y || this.Rect.anchoredPosition.y > this.MaxBounds.y)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04003543 RID: 13635
		public RectTransform Rect;

		// Token: 0x04003544 RID: 13636
		public Vector2 MinBounds = new Vector2(-1000f, -1000f);

		// Token: 0x04003545 RID: 13637
		public Vector2 MaxBounds = new Vector2(1000f, 1000f);
	}
}
