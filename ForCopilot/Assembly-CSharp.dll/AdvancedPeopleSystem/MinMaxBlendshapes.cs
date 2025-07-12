using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200021A RID: 538
	[Serializable]
	public class MinMaxBlendshapes
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x0003658D File Offset: 0x0003478D
		public float GetRandom()
		{
			return UnityEngine.Random.Range(this.Min, this.Max);
		}

		// Token: 0x04000C8B RID: 3211
		[Range(-100f, 100f)]
		public float Min;

		// Token: 0x04000C8C RID: 3212
		[Range(-100f, 100f)]
		public float Max;
	}
}
