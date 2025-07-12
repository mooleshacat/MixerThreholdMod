using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A0 RID: 416
	public class CloudBlender : FeatureBlender
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0002701D File Offset: 0x0002521D
		protected override string featureKey
		{
			get
			{
				return "CloudFeature";
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00027024 File Offset: 0x00025224
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("CloudDensityKey");
			helper.BlendNumber("CloudTextureTiling");
			helper.BlendNumber("CloudSpeedKey");
			helper.BlendNumber("CloudDirectionKey");
			helper.BlendNumber("CloudFadeAmountKey");
			helper.BlendNumber("CloudFadePositionKey");
			helper.BlendNumber("CloudAlphaKey");
			helper.BlendColor("CloudColor1Key");
			helper.BlendColor("CloudColor2Key");
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00027094 File Offset: 0x00025294
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("CloudAlphaKey", 0f);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000270A6 File Offset: 0x000252A6
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("CloudAlphaKey", 0f);
		}
	}
}
