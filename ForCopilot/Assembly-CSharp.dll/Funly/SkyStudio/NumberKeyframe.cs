using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001BC RID: 444
	[Serializable]
	public class NumberKeyframe : BaseKeyframe
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x000283CE File Offset: 0x000265CE
		public NumberKeyframe(float time, float value) : base(time)
		{
			this.value = value;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000283DE File Offset: 0x000265DE
		public NumberKeyframe(NumberKeyframe keyframe) : base(keyframe.time)
		{
			this.value = keyframe.value;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000991 RID: 2449
		public float value;
	}
}
