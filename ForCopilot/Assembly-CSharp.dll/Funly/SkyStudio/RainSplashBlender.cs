using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A6 RID: 422
	public class RainSplashBlender : FeatureBlender
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0002731F File Offset: 0x0002551F
		protected override string featureKey
		{
			get
			{
				return "RainSplashFeature";
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00027328 File Offset: 0x00025528
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("RainSplashMaxConcurrentKey");
			helper.BlendNumber("RainSplashAreaStartKey");
			helper.BlendNumber("RainSplashAreaLengthKey");
			helper.BlendNumber("RainSplashScaleKey");
			helper.BlendNumber("RainSplashScaleVarienceKey");
			helper.BlendNumber("RainSplashIntensityKey");
			helper.BlendNumber("RainSplashSurfaceOffsetKey");
			helper.BlendColor("RainSplashTintColorKey");
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002738D File Offset: 0x0002558D
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("RainSplashIntensityKey", 0f);
			helper.BlendNumberIn("RainSplashMaxConcurrentKey", 0f);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000273AF File Offset: 0x000255AF
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("RainSplashIntensityKey", 0f);
			helper.BlendNumberOut("RainSplashMaxConcurrentKey", 0f);
		}
	}
}
