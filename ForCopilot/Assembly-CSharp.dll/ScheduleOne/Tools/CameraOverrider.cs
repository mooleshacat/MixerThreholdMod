using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000882 RID: 2178
	public class CameraOverrider : MonoBehaviour
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x000FC600 File Offset: 0x000FA800
		public void LateUpdate()
		{
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(base.transform.position, base.transform.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.FOV, 0f);
		}

		// Token: 0x04002A92 RID: 10898
		public float FOV = 70f;
	}
}
