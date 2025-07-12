using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.Recording
{
	// Token: 0x0200081E RID: 2078
	[Serializable]
	public class VehicleKeyFrame
	{
		// Token: 0x0400286B RID: 10347
		public Vector3 position;

		// Token: 0x0400286C RID: 10348
		public Quaternion rotation;

		// Token: 0x0400286D RID: 10349
		public bool brakesApplied;

		// Token: 0x0400286E RID: 10350
		public bool reversing;

		// Token: 0x0400286F RID: 10351
		public bool headlightsOn;

		// Token: 0x04002870 RID: 10352
		public List<VehicleKeyFrame.WheelTransform> wheels = new List<VehicleKeyFrame.WheelTransform>();

		// Token: 0x0200081F RID: 2079
		[Serializable]
		public class WheelTransform
		{
			// Token: 0x04002871 RID: 10353
			public float yPos;

			// Token: 0x04002872 RID: 10354
			public Quaternion rotation;
		}
	}
}
