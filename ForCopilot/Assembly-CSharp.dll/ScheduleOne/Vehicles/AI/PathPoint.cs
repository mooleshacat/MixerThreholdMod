using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x02000837 RID: 2103
	public class PathPoint : MonoBehaviour
	{
		// Token: 0x040028DC RID: 10460
		public List<PathPoint> connections = new List<PathPoint>();

		// Token: 0x040028DD RID: 10461
		public bool unique;
	}
}
