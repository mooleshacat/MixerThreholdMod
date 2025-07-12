using System;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x02000836 RID: 2102
	public class PathGroup
	{
		// Token: 0x040028D8 RID: 10456
		public Vector3 entryPoint;

		// Token: 0x040028D9 RID: 10457
		public Path startToEntryPath;

		// Token: 0x040028DA RID: 10458
		public Path entryToExitPath;

		// Token: 0x040028DB RID: 10459
		public Path exitToDestinationPath;
	}
}
