using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A1E RID: 2590
	public class EyeLidOverlaySetter : MonoBehaviour
	{
		// Token: 0x060045B9 RID: 17849 RVA: 0x00124ECD File Offset: 0x001230CD
		private void OnEnable()
		{
			if (Singleton<EyelidOverlay>.InstanceExists)
			{
				Singleton<EyelidOverlay>.Instance.AutoUpdate = false;
			}
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00124EE1 File Offset: 0x001230E1
		private void OnDisable()
		{
			if (Singleton<EyelidOverlay>.InstanceExists)
			{
				Singleton<EyelidOverlay>.Instance.AutoUpdate = true;
			}
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00124EF5 File Offset: 0x001230F5
		private void Update()
		{
			if (Singleton<EyelidOverlay>.InstanceExists)
			{
				Singleton<EyelidOverlay>.Instance.SetOpen(this.OpenOverride);
			}
		}

		// Token: 0x04003279 RID: 12921
		[Range(0f, 1f)]
		public float OpenOverride = 1f;
	}
}
