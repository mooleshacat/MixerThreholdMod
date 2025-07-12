using System;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005E5 RID: 1509
	public class UsableLightSource : MonoBehaviour
	{
		// Token: 0x04001B62 RID: 7010
		[Range(0.5f, 2f)]
		public float GrowSpeedMultiplier = 1f;

		// Token: 0x04001B63 RID: 7011
		public bool isEmitting = true;
	}
}
