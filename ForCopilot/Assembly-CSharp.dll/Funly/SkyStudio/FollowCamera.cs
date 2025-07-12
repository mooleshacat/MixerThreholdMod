using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D9 RID: 473
	[ExecuteInEditMode]
	public class FollowCamera : MonoBehaviour
	{
		// Token: 0x06000A71 RID: 2673 RVA: 0x0002E460 File Offset: 0x0002C660
		private void Update()
		{
			Camera main;
			if (this.followCamera != null)
			{
				main = this.followCamera;
			}
			else
			{
				main = Camera.main;
			}
			if (main == null)
			{
				return;
			}
			base.transform.position = main.transform.TransformPoint(this.offset);
		}

		// Token: 0x04000B57 RID: 2903
		public Camera followCamera;

		// Token: 0x04000B58 RID: 2904
		public Vector3 offset = Vector3.zero;
	}
}
