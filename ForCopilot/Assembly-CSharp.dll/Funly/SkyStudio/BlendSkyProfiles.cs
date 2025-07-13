using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001AA RID: 426
	public class BlendSkyProfiles : MonoBehaviour
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00027631 File Offset: 0x00025831
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x00027639 File Offset: 0x00025839
		public SkyProfile fromProfile { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x00027642 File Offset: 0x00025842
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0002764A File Offset: 0x0002584A
		public SkyProfile toProfile { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00027653 File Offset: 0x00025853
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x0002765B File Offset: 0x0002585B
		public SkyProfile blendedProfile { get; private set; }

		// Token: 0x060008AF RID: 2223 RVA: 0x00027664 File Offset: 0x00025864
		public SkyProfile StartBlending(TimeOfDayController controller, SkyProfile fromProfile, SkyProfile toProfile, float duration)
		{
			if (controller == null)
			{
				Debug.LogWarning("Can't transition with null TimeOfDayController");
				return null;
			}
			if (fromProfile == null)
			{
				Debug.LogWarning("Can't transition to null 'from' sky profile.");
				return null;
			}
			if (toProfile == null)
			{
				Debug.LogWarning("Can't transition to null 'to' sky profile");
				return null;
			}
			if (!fromProfile.IsFeatureEnabled("GradientSkyFeature", true) || !toProfile.IsFeatureEnabled("GradientSkyFeature", true))
			{
				Debug.LogWarning("Sky Studio doesn't currently support automatic transition blending with cubemap backgrounds.");
			}
			this.m_TimeOfDayController = controller;
			this.fromProfile = fromProfile;
			this.toProfile = toProfile;
			this.m_StartTime = Time.time;
			this.m_EndTime = this.m_StartTime + duration;
			this.blendedProfile = UnityEngine.Object.Instantiate<SkyProfile>(fromProfile);
			this.blendedProfile.skyboxMaterial = fromProfile.skyboxMaterial;
			this.m_TimeOfDayController.skyProfile = this.blendedProfile;
			this.m_State = new ProfileBlendingState(this.blendedProfile, fromProfile, toProfile, 0f, 0f, 0f, this.m_TimeOfDayController.timeOfDay);
			this.blendingHelper = new BlendingHelper(this.m_State);
			this.UpdateBlendedProfile();
			return this.blendedProfile;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002777D File Offset: 0x0002597D
		public void CancelBlending()
		{
			this.TearDownBlending();
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00027785 File Offset: 0x00025985
		public void TearDownBlending()
		{
			if (this.m_TimeOfDayController == null)
			{
				return;
			}
			this.m_TimeOfDayController = null;
			this.blendedProfile = null;
			base.enabled = false;
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000277B6 File Offset: 0x000259B6
		private void Update()
		{
			if (this.blendedProfile == null)
			{
				return;
			}
			this.UpdateBlendedProfile();
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000277D0 File Offset: 0x000259D0
		private void UpdateBlendedProfile()
		{
			if (this.m_TimeOfDayController == null)
			{
				return;
			}
			float num = this.m_EndTime - this.m_StartTime;
			float num2 = Time.time - this.m_StartTime;
			this.m_State.progress = num2 / num;
			this.m_State.inProgress = this.PercentForMode(ProfileFeatureBlendingMode.FadeFeatureIn, this.m_State.progress);
			this.m_State.outProgress = this.PercentForMode(ProfileFeatureBlendingMode.FadeFeatureOut, this.m_State.progress);
			this.blendingHelper.UpdateState(this.m_State);
			if (this.m_State.progress > 0.5f && this.m_IsBlendingFirstHalf)
			{
				this.m_IsBlendingFirstHalf = false;
				this.blendedProfile = UnityEngine.Object.Instantiate<SkyProfile>(this.toProfile);
				this.m_State.blendedProfile = this.blendedProfile;
				this.m_TimeOfDayController.skyProfile = this.blendedProfile;
			}
			this.blendingHelper.UpdateState(this.m_State);
			foreach (FeatureBlender featureBlender in new FeatureBlender[]
			{
				this.skyBlender,
				this.sunBlender,
				this.moonBlender,
				this.cloudBlender,
				this.starLayer1Blender,
				this.starLayer2Blender,
				this.starLayer3Blender,
				this.rainBlender,
				this.rainSplashBlender,
				this.lightningBlender,
				this.fogBlender
			})
			{
				if (!(featureBlender == null))
				{
					featureBlender.Blend(this.m_State, this.blendingHelper);
				}
			}
			this.m_TimeOfDayController.skyProfile = this.blendedProfile;
			if (this.m_State.progress >= 1f)
			{
				this.onBlendComplete(this);
				this.TearDownBlending();
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00027997 File Offset: 0x00025B97
		private float PercentForMode(ProfileFeatureBlendingMode mode, float percent)
		{
			if (mode == ProfileFeatureBlendingMode.FadeFeatureOut)
			{
				return Mathf.Clamp01(percent * 2f);
			}
			if (mode == ProfileFeatureBlendingMode.FadeFeatureIn)
			{
				return Mathf.Clamp01((percent - 0.5f) * 2f);
			}
			return percent;
		}

		// Token: 0x0400095C RID: 2396
		[Tooltip("Called when blending finishes.")]
		public Action<BlendSkyProfiles> onBlendComplete;

		// Token: 0x0400095D RID: 2397
		[HideInInspector]
		private float m_StartTime = -1f;

		// Token: 0x0400095E RID: 2398
		[HideInInspector]
		private float m_EndTime = -1f;

		// Token: 0x0400095F RID: 2399
		[Tooltip("Blender used for basic sky background properties.")]
		public FeatureBlender skyBlender;

		// Token: 0x04000960 RID: 2400
		[Tooltip("Blender used for the sun properties.")]
		public FeatureBlender sunBlender;

		// Token: 0x04000961 RID: 2401
		[Tooltip("Blender used moon properties.")]
		public FeatureBlender moonBlender;

		// Token: 0x04000962 RID: 2402
		[Tooltip("Blender used cloud properties.")]
		public FeatureBlender cloudBlender;

		// Token: 0x04000963 RID: 2403
		[Tooltip("Blender used star layer 1 properties.")]
		public FeatureBlender starLayer1Blender;

		// Token: 0x04000964 RID: 2404
		[Tooltip("Blender used star layer 2 properties.")]
		public FeatureBlender starLayer2Blender;

		// Token: 0x04000965 RID: 2405
		[Tooltip("Blender used star layer 3 properties.")]
		public FeatureBlender starLayer3Blender;

		// Token: 0x04000966 RID: 2406
		[Tooltip("Blender used by the rain downfall feature.")]
		public FeatureBlender rainBlender;

		// Token: 0x04000967 RID: 2407
		[Tooltip("Blender used by the rain splash feature.")]
		public FeatureBlender rainSplashBlender;

		// Token: 0x04000968 RID: 2408
		[Tooltip("Blender used for lightning feature properties.")]
		public FeatureBlender lightningBlender;

		// Token: 0x04000969 RID: 2409
		[Tooltip("Blender used for fog properties.")]
		public FeatureBlender fogBlender;

		// Token: 0x0400096A RID: 2410
		private bool m_IsBlendingFirstHalf = true;

		// Token: 0x0400096B RID: 2411
		private ProfileBlendingState m_State;

		// Token: 0x0400096C RID: 2412
		private TimeOfDayController m_TimeOfDayController;

		// Token: 0x0400096D RID: 2413
		private BlendingHelper blendingHelper;
	}
}
