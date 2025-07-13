using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001B7 RID: 439
	[Serializable]
	public class BoolKeyframe : BaseKeyframe
	{
		// Token: 0x060008FB RID: 2299 RVA: 0x00028334 File Offset: 0x00026534
		public BoolKeyframe(float time, bool value) : base(time)
		{
			this.value = value;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00028344 File Offset: 0x00026544
		public BoolKeyframe(BoolKeyframe keyframe) : base(keyframe.time)
		{
			this.value = keyframe.value;
			base.interpolationCurve = keyframe.interpolationCurve;
			base.interpolationDirection = keyframe.interpolationDirection;
		}

		// Token: 0x04000987 RID: 2439
		public bool value;
	}
}
