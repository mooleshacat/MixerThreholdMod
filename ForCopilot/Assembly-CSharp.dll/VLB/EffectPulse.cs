using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000100 RID: 256
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-pulse/")]
	public class EffectPulse : EffectAbstractBase
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x00016BDC File Offset: 0x00014DDC
		public override void InitFrom(EffectAbstractBase source)
		{
			base.InitFrom(source);
			EffectPulse effectPulse = source as EffectPulse;
			if (effectPulse)
			{
				this.frequency = effectPulse.frequency;
				this.intensityAmplitude = effectPulse.intensityAmplitude;
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00016C17 File Offset: 0x00014E17
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CoUpdate());
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00016C2C File Offset: 0x00014E2C
		private IEnumerator CoUpdate()
		{
			float t = 0f;
			for (;;)
			{
				float num = Mathf.Sin(this.frequency * t);
				float lerpedValue = this.intensityAmplitude.GetLerpedValue(num * 0.5f + 0.5f);
				base.SetAdditiveIntensity(lerpedValue);
				yield return null;
				t += Time.deltaTime;
			}
			yield break;
		}

		// Token: 0x04000596 RID: 1430
		public new const string ClassName = "EffectPulse";

		// Token: 0x04000597 RID: 1431
		[Range(0.1f, 60f)]
		public float frequency = 10f;

		// Token: 0x04000598 RID: 1432
		[MinMaxRange(-5f, 5f)]
		public MinMaxRangeFloat intensityAmplitude = Consts.Effects.IntensityAmplitudeDefault;
	}
}
