using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000218 RID: 536
	[Serializable]
	public class MinMaxIndex
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x00036507 File Offset: 0x00034707
		public int GetRandom(int max)
		{
			return Mathf.Clamp(UnityEngine.Random.Range(this.Min, this.Max), -1, max);
		}

		// Token: 0x04000C87 RID: 3207
		public int Min;

		// Token: 0x04000C88 RID: 3208
		public int Max;
	}
}
