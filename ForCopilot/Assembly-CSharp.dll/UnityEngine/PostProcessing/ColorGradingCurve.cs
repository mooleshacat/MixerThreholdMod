using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E0 RID: 224
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x00014CF4 File Offset: 0x00012EF4
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00014D20 File Offset: 0x00012F20
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe key = this.curve[length - 1];
			key.time -= this.m_Range;
			Keyframe key2 = this.curve[0];
			key2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(key);
			this.m_InternalLoopingCurve.AddKey(key2);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00014DD0 File Offset: 0x00012FD0
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x0400048E RID: 1166
		public AnimationCurve curve;

		// Token: 0x0400048F RID: 1167
		[SerializeField]
		private bool m_Loop;

		// Token: 0x04000490 RID: 1168
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x04000491 RID: 1169
		[SerializeField]
		private float m_Range;

		// Token: 0x04000492 RID: 1170
		private AnimationCurve m_InternalLoopingCurve;
	}
}
