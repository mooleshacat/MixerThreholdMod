using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A05 RID: 2565
	public class CanvasDistanceFade : MonoBehaviour
	{
		// Token: 0x060044FD RID: 17661 RVA: 0x001218AC File Offset: 0x0011FAAC
		public void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			if (num < this.MinDistance)
			{
				this.CanvasGroup.alpha = 1f;
				return;
			}
			if (num > this.MaxDistance)
			{
				this.CanvasGroup.alpha = 0f;
				return;
			}
			this.CanvasGroup.alpha = 1f - (num - this.MinDistance) / (this.MaxDistance - this.MinDistance);
		}

		// Token: 0x040031D6 RID: 12758
		public CanvasGroup CanvasGroup;

		// Token: 0x040031D7 RID: 12759
		public float MinDistance = 5f;

		// Token: 0x040031D8 RID: 12760
		public float MaxDistance = 10f;
	}
}
