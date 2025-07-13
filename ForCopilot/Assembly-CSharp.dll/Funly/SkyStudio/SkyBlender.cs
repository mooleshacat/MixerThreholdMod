using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A7 RID: 423
	public class SkyBlender : FeatureBlender
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x000273D1 File Offset: 0x000255D1
		protected override string featureKey
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override ProfileFeatureBlendingMode BlendingMode(ProfileBlendingState state, BlendingHelper helper)
		{
			return ProfileFeatureBlendingMode.Normal;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000273D8 File Offset: 0x000255D8
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("SkyLowerColorKey");
			helper.BlendColor("SkyMiddleColorKey");
			helper.BlendColor("SkyUpperColorKey");
			helper.BlendNumber("SkyMiddleColorPosition");
			helper.BlendNumber("HorizonTransitionStartKey");
			helper.BlendNumber("HorizonTransitionLengthKey");
			helper.BlendNumber("StarTransitionStartKey");
			helper.BlendNumber("StarTransitionLengthKey");
			helper.BlendNumber("HorizonStarScaleKey");
			helper.BlendColor("AmbientLightSkyColorKey");
			helper.BlendColor("AmbientLightEquatorColorKey");
			helper.BlendColor("AmbientLightGroundColorKey");
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00027469 File Offset: 0x00025669
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("AmbientLightSkyColorKey");
			helper.BlendColor("AmbientLightEquatorColorKey");
			helper.BlendColor("AmbientLightGroundColorKey");
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00027469 File Offset: 0x00025669
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("AmbientLightSkyColorKey");
			helper.BlendColor("AmbientLightEquatorColorKey");
			helper.BlendColor("AmbientLightGroundColorKey");
		}
	}
}
