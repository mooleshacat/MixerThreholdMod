using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x020000F9 RID: 249
	[AddComponentMenu("")]
	public class EffectAbstractBase : MonoBehaviour
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00016583 File Offset: 0x00014783
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0001658B File Offset: 0x0001478B
		[Obsolete("Use 'restoreIntensityOnDisable' instead")]
		public bool restoreBaseIntensity
		{
			get
			{
				return this.restoreIntensityOnDisable;
			}
			set
			{
				this.restoreIntensityOnDisable = value;
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00016594 File Offset: 0x00014794
		public virtual void InitFrom(EffectAbstractBase Source)
		{
			if (Source)
			{
				this.componentsToChange = Source.componentsToChange;
				this.restoreIntensityOnDisable = Source.restoreIntensityOnDisable;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000165B6 File Offset: 0x000147B6
		private void GetIntensity(VolumetricLightBeamSD beam)
		{
			if (beam)
			{
				this.m_BaseIntensityBeamInside = beam.intensityInside;
				this.m_BaseIntensityBeamOutside = beam.intensityOutside;
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000165D8 File Offset: 0x000147D8
		private void GetIntensity(VolumetricLightBeamHD beam)
		{
			if (beam)
			{
				this.m_BaseIntensityBeamOutside = beam.intensity;
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000165EE File Offset: 0x000147EE
		private void SetIntensity(VolumetricLightBeamSD beam, float additive)
		{
			if (beam)
			{
				beam.intensityInside = Mathf.Max(0f, this.m_BaseIntensityBeamInside + additive);
				beam.intensityOutside = Mathf.Max(0f, this.m_BaseIntensityBeamOutside + additive);
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00016628 File Offset: 0x00014828
		private void SetIntensity(VolumetricLightBeamHD beam, float additive)
		{
			if (beam)
			{
				beam.intensity = Mathf.Max(0f, this.m_BaseIntensityBeamOutside + additive);
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001664C File Offset: 0x0001484C
		protected void SetAdditiveIntensity(float additive)
		{
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.VolumetricLightBeam) && this.m_Beam)
			{
				this.SetIntensity(this.m_Beam as VolumetricLightBeamSD, additive);
				this.SetIntensity(this.m_Beam as VolumetricLightBeamHD, additive);
			}
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.UnityLight) && this.m_Light)
			{
				this.m_Light.intensity = Mathf.Max(0f, this.m_BaseIntensityLight + additive);
			}
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.VolumetricDustParticles) && this.m_Particles)
			{
				this.m_Particles.alphaAdditionalRuntime = 1f + additive;
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001671C File Offset: 0x0001491C
		private void Awake()
		{
			this.m_Beam = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.m_Light = base.GetComponent<Light>();
			this.m_Particles = base.GetComponent<VolumetricDustParticles>();
			this.GetIntensity(this.m_Beam as VolumetricLightBeamSD);
			this.GetIntensity(this.m_Beam as VolumetricLightBeamHD);
			this.m_BaseIntensityLight = (this.m_Light ? this.m_Light.intensity : 0f);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00016794 File Offset: 0x00014994
		protected virtual void OnEnable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001679C File Offset: 0x0001499C
		private void OnDisable()
		{
			base.StopAllCoroutines();
			if (this.restoreIntensityOnDisable)
			{
				this.SetAdditiveIntensity(0f);
			}
		}

		// Token: 0x0400056E RID: 1390
		public const string ClassName = "EffectAbstractBase";

		// Token: 0x0400056F RID: 1391
		public EffectAbstractBase.ComponentsToChange componentsToChange = (EffectAbstractBase.ComponentsToChange)2147483647;

		// Token: 0x04000570 RID: 1392
		[FormerlySerializedAs("restoreBaseIntensity")]
		public bool restoreIntensityOnDisable = true;

		// Token: 0x04000571 RID: 1393
		protected VolumetricLightBeamAbstractBase m_Beam;

		// Token: 0x04000572 RID: 1394
		protected Light m_Light;

		// Token: 0x04000573 RID: 1395
		protected VolumetricDustParticles m_Particles;

		// Token: 0x04000574 RID: 1396
		protected float m_BaseIntensityBeamInside;

		// Token: 0x04000575 RID: 1397
		protected float m_BaseIntensityBeamOutside;

		// Token: 0x04000576 RID: 1398
		protected float m_BaseIntensityLight;

		// Token: 0x020000FA RID: 250
		[Flags]
		public enum ComponentsToChange
		{
			// Token: 0x04000578 RID: 1400
			UnityLight = 1,
			// Token: 0x04000579 RID: 1401
			VolumetricLightBeam = 2,
			// Token: 0x0400057A RID: 1402
			VolumetricDustParticles = 4
		}
	}
}
