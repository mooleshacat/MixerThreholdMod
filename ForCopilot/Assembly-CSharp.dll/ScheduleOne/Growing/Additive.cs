using System;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x020008BC RID: 2236
	public class Additive : MonoBehaviour
	{
		// Token: 0x04002B36 RID: 11062
		public string AdditiveName = "Name";

		// Token: 0x04002B37 RID: 11063
		public string AssetPath;

		// Token: 0x04002B38 RID: 11064
		[Header("Plant effector settings")]
		public float QualityChange;

		// Token: 0x04002B39 RID: 11065
		public float YieldChange;

		// Token: 0x04002B3A RID: 11066
		public float GrowSpeedMultiplier = 1f;

		// Token: 0x04002B3B RID: 11067
		public float InstantGrowth;
	}
}
