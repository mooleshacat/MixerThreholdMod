using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200021E RID: 542
	[Serializable]
	public class MinMaxFacialBlendshapes
	{
		// Token: 0x06000BC5 RID: 3013 RVA: 0x000365B3 File Offset: 0x000347B3
		public float GetRandom()
		{
			return UnityEngine.Random.Range(this.Min, this.Max);
		}

		// Token: 0x04000C9A RID: 3226
		public string name;

		// Token: 0x04000C9B RID: 3227
		[Range(-100f, 100f)]
		public float Min;

		// Token: 0x04000C9C RID: 3228
		[Range(-100f, 100f)]
		public float Max;
	}
}
