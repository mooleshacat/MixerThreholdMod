using System;

namespace VLB
{
	// Token: 0x0200013B RID: 315
	public class MinMaxRangeAttribute : Attribute
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00019F9E File Offset: 0x0001819E
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00019FA6 File Offset: 0x000181A6
		public float minValue { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00019FAF File Offset: 0x000181AF
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00019FB7 File Offset: 0x000181B7
		public float maxValue { get; private set; }

		// Token: 0x06000566 RID: 1382 RVA: 0x00019FC0 File Offset: 0x000181C0
		public MinMaxRangeAttribute(float min, float max)
		{
			this.minValue = min;
			this.maxValue = max;
		}
	}
}
