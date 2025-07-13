using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B0 RID: 432
	[Serializable]
	public class ColorKeyframeGroup : KeyframeGroup<ColorKeyframe>
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x00027B47 File Offset: 0x00025D47
		public ColorKeyframeGroup(string name) : base(name)
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00027B50 File Offset: 0x00025D50
		public ColorKeyframeGroup(string name, ColorKeyframe frame) : base(name)
		{
			base.AddKeyFrame(frame);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00027B60 File Offset: 0x00025D60
		public Color ColorForTime(float time)
		{
			time -= (float)((int)time);
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't return color since there aren't any keyframes.");
				return Color.white;
			}
			if (this.keyframes.Count == 1)
			{
				return base.GetKeyframe(0).color;
			}
			int index;
			int index2;
			base.GetSurroundingKeyFrames(time, out index, out index2);
			ColorKeyframe keyframe = base.GetKeyframe(index);
			ColorKeyframe keyframe2 = base.GetKeyframe(index2);
			float t = KeyframeGroup<ColorKeyframe>.ProgressBetweenSurroundingKeyframes(time, keyframe, keyframe2);
			float t2 = base.CurveAdjustedBlendingTime(keyframe.interpolationCurve, t);
			return Color.Lerp(keyframe.color, keyframe2.color, t2);
		}
	}
}
