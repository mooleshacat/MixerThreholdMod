using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001BD RID: 445
	[Serializable]
	public class SpherePointKeyframe : BaseKeyframe
	{
		// Token: 0x06000908 RID: 2312 RVA: 0x00028410 File Offset: 0x00026610
		public SpherePointKeyframe(SpherePoint spherePoint, float time) : base(time)
		{
			if (spherePoint == null)
			{
				Debug.LogError("Passed null sphere point, created empty point");
				this.spherePoint = new SpherePoint(0f, 0f);
			}
			else
			{
				this.spherePoint = spherePoint;
			}
			base.interpolationDirection = InterpolationDirection.Auto;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002844C File Offset: 0x0002664C
		public SpherePointKeyframe(SpherePointKeyframe keyframe) : base(keyframe.time)
		{
			this.spherePoint = new SpherePoint(keyframe.spherePoint.horizontalRotation, keyframe.spherePoint.verticalRotation);
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000992 RID: 2450
		public SpherePoint spherePoint;
	}
}
