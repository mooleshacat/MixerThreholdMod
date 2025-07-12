using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000817 RID: 2071
	public class VehicleObstacle : MonoBehaviour
	{
		// Token: 0x0400282E RID: 10286
		public Collider col;

		// Token: 0x0400282F RID: 10287
		[Header("Settings")]
		public bool twoSided = true;

		// Token: 0x04002830 RID: 10288
		public VehicleObstacle.EObstacleType type;

		// Token: 0x02000818 RID: 2072
		public enum EObstacleType
		{
			// Token: 0x04002832 RID: 10290
			Generic,
			// Token: 0x04002833 RID: 10291
			TrafficLight
		}
	}
}
