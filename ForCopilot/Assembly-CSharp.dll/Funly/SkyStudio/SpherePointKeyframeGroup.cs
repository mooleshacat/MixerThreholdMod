using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B4 RID: 436
	[Serializable]
	public class SpherePointKeyframeGroup : KeyframeGroup<SpherePointKeyframe>
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x00028157 File Offset: 0x00026357
		public SpherePointKeyframeGroup(string name) : base(name)
		{
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00028160 File Offset: 0x00026360
		public SpherePointKeyframeGroup(string name, SpherePointKeyframe keyframe) : base(name)
		{
			base.AddKeyFrame(keyframe);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00028170 File Offset: 0x00026370
		public SpherePoint SpherePointForTime(float time)
		{
			if (this.keyframes.Count == 1)
			{
				return this.keyframes[0].spherePoint;
			}
			int index;
			int index2;
			if (!base.GetSurroundingKeyFrames(time, out index, out index2))
			{
				Debug.LogError("Failed to get surrounding sphere point for time: " + time.ToString());
				return null;
			}
			time -= (float)((int)time);
			SpherePointKeyframe keyframe = base.GetKeyframe(index);
			SpherePointKeyframe keyframe2 = base.GetKeyframe(index2);
			float t = KeyframeGroup<SpherePointKeyframe>.ProgressBetweenSurroundingKeyframes(time, keyframe.time, keyframe2.time);
			float t2 = base.CurveAdjustedBlendingTime(keyframe.interpolationCurve, t);
			return new SpherePoint(Vector3.Slerp(keyframe.spherePoint.GetWorldDirection(), keyframe2.spherePoint.GetWorldDirection(), t2).normalized);
		}

		// Token: 0x0400097F RID: 2431
		public const float MinHorizontalRotation = -3.1415927f;

		// Token: 0x04000980 RID: 2432
		public const float MaxHorizontalRotation = 3.1415927f;

		// Token: 0x04000981 RID: 2433
		public const float MinVerticalRotation = -1.5707964f;

		// Token: 0x04000982 RID: 2434
		public const float MaxVerticalRotation = 1.5707964f;
	}
}
