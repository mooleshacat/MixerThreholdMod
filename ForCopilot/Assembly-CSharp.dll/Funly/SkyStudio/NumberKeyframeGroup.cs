using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B3 RID: 435
	[Serializable]
	public class NumberKeyframeGroup : KeyframeGroup<NumberKeyframe>
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x00028012 File Offset: 0x00026212
		public NumberKeyframeGroup(string name, float min, float max) : base(name)
		{
			this.minValue = min;
			this.maxValue = max;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00028029 File Offset: 0x00026229
		public NumberKeyframeGroup(string name, float min, float max, NumberKeyframe frame) : base(name)
		{
			this.minValue = min;
			this.maxValue = max;
			base.AddKeyFrame(frame);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00028048 File Offset: 0x00026248
		public float GetFirstValue()
		{
			return base.GetKeyframe(0).value;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00028056 File Offset: 0x00026256
		public float ValueToPercent(float value)
		{
			return Mathf.Abs((value - this.minValue) / (this.maxValue - this.minValue));
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00028073 File Offset: 0x00026273
		public float ValuePercentAtTime(float time)
		{
			return this.ValueToPercent(this.NumericValueAtTime(time));
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00028082 File Offset: 0x00026282
		public float PercentToValue(float percent)
		{
			return Mathf.Clamp(this.minValue + (this.maxValue - this.minValue) * percent, this.minValue, this.maxValue);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x000280AC File Offset: 0x000262AC
		public float NumericValueAtTime(float time)
		{
			time -= (float)((int)time);
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Keyframe group has no keyframes: " + base.name);
				return this.minValue;
			}
			if (this.keyframes.Count == 1)
			{
				return base.GetKeyframe(0).value;
			}
			int index;
			int index2;
			base.GetSurroundingKeyFrames(time, out index, out index2);
			NumberKeyframe keyframe = base.GetKeyframe(index);
			NumberKeyframe keyframe2 = base.GetKeyframe(index2);
			return base.InterpolateFloat(keyframe.interpolationCurve, keyframe.interpolationDirection, time, keyframe.time, keyframe2.time, keyframe.value, keyframe2.value, this.minValue, this.maxValue);
		}

		// Token: 0x0400097D RID: 2429
		public float minValue;

		// Token: 0x0400097E RID: 2430
		public float maxValue;
	}
}
