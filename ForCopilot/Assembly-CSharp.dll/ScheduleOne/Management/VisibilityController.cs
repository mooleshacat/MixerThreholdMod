using System;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005C2 RID: 1474
	public class VisibilityController : MonoBehaviour
	{
		// Token: 0x06002456 RID: 9302 RVA: 0x00095055 File Offset: 0x00093255
		private void Start()
		{
			bool flag = this.visibleOnlyInFullscreen;
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0009505E File Offset: 0x0009325E
		private void OnEnterFullScreen()
		{
			if (this.visibleOnlyInFullscreen)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x00095074 File Offset: 0x00093274
		private void OnExitFullScreen()
		{
			if (this.visibleOnlyInFullscreen)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04001AF0 RID: 6896
		public bool visibleOnlyInFullscreen = true;
	}
}
