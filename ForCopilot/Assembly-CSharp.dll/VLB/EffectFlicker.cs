using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000FB RID: 251
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-flicker/")]
	public class EffectFlicker : EffectAbstractBase
	{
		// Token: 0x06000415 RID: 1045 RVA: 0x000167D4 File Offset: 0x000149D4
		public override void InitFrom(EffectAbstractBase source)
		{
			base.InitFrom(source);
			EffectFlicker effectFlicker = source as EffectFlicker;
			if (effectFlicker)
			{
				this.frequency = effectFlicker.frequency;
				this.performPauses = effectFlicker.performPauses;
				this.flickeringDuration = effectFlicker.flickeringDuration;
				this.pauseDuration = effectFlicker.pauseDuration;
				this.restoreIntensityOnPause = effectFlicker.restoreIntensityOnPause;
				this.intensityAmplitude = effectFlicker.intensityAmplitude;
				this.smoothing = effectFlicker.smoothing;
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001684B File Offset: 0x00014A4B
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CoUpdate());
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00016860 File Offset: 0x00014A60
		private IEnumerator CoUpdate()
		{
			for (;;)
			{
				yield return this.CoFlicker();
				if (this.performPauses)
				{
					yield return this.CoChangeIntensity(this.pauseDuration.randomValue, this.restoreIntensityOnPause ? 0f : this.m_CurrentAdditiveIntensity);
				}
			}
			yield break;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001686F File Offset: 0x00014A6F
		private IEnumerator CoFlicker()
		{
			float remainingDuration = this.flickeringDuration.randomValue;
			float deltaTime = Time.deltaTime;
			while (!this.performPauses || remainingDuration > 0f)
			{
				float freqDuration = 1f / this.frequency;
				yield return this.CoChangeIntensity(freqDuration, this.intensityAmplitude.randomValue);
				remainingDuration -= freqDuration;
			}
			yield break;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001687E File Offset: 0x00014A7E
		private IEnumerator CoChangeIntensity(float expectedDuration, float nextIntensity)
		{
			float velocity = 0f;
			float t = 0f;
			while (t < expectedDuration)
			{
				this.m_CurrentAdditiveIntensity = Mathf.SmoothDamp(this.m_CurrentAdditiveIntensity, nextIntensity, ref velocity, this.smoothing);
				base.SetAdditiveIntensity(this.m_CurrentAdditiveIntensity);
				t += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400057B RID: 1403
		public new const string ClassName = "EffectFlicker";

		// Token: 0x0400057C RID: 1404
		[Range(1f, 60f)]
		public float frequency = 10f;

		// Token: 0x0400057D RID: 1405
		public bool performPauses;

		// Token: 0x0400057E RID: 1406
		[MinMaxRange(0f, 10f)]
		public MinMaxRangeFloat flickeringDuration = Consts.Effects.FlickeringDurationDefault;

		// Token: 0x0400057F RID: 1407
		[MinMaxRange(0f, 10f)]
		public MinMaxRangeFloat pauseDuration = Consts.Effects.PauseDurationDefault;

		// Token: 0x04000580 RID: 1408
		public bool restoreIntensityOnPause;

		// Token: 0x04000581 RID: 1409
		[MinMaxRange(-5f, 5f)]
		public MinMaxRangeFloat intensityAmplitude = Consts.Effects.IntensityAmplitudeDefault;

		// Token: 0x04000582 RID: 1410
		[Range(0f, 0.25f)]
		public float smoothing = 0.05f;

		// Token: 0x04000583 RID: 1411
		private float m_CurrentAdditiveIntensity;
	}
}
