using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000158 RID: 344
	public static class TransformUtils
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
		public static TransformUtils.Packed GetWorldPacked(this Transform self)
		{
			return new TransformUtils.Packed
			{
				position = self.position,
				rotation = self.rotation,
				lossyScale = self.lossyScale
			};
		}

		// Token: 0x02000159 RID: 345
		public struct Packed
		{
			// Token: 0x06000681 RID: 1665 RVA: 0x0001D5DD File Offset: 0x0001B7DD
			public bool IsSame(Transform transf)
			{
				return transf.position == this.position && transf.rotation == this.rotation && transf.lossyScale == this.lossyScale;
			}

			// Token: 0x04000765 RID: 1893
			public Vector3 position;

			// Token: 0x04000766 RID: 1894
			public Quaternion rotation;

			// Token: 0x04000767 RID: 1895
			public Vector3 lossyScale;
		}
	}
}
