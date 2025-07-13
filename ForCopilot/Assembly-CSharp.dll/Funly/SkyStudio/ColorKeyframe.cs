using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B8 RID: 440
	[Serializable]
	public class ColorKeyframe : BaseKeyframe
	{
		// Token: 0x060008FD RID: 2301 RVA: 0x00028376 File Offset: 0x00026576
		public ColorKeyframe(Color c, float time) : base(time)
		{
			this.color = c;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00028391 File Offset: 0x00026591
		public ColorKeyframe(ColorKeyframe keyframe) : base(keyframe.time)
		{
			this.color = keyframe.color;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000988 RID: 2440
		public Color color = Color.white;
	}
}
