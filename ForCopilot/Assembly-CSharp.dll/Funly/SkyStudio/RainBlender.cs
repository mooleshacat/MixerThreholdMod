using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A5 RID: 421
	public class RainBlender : FeatureBlender
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00027239 File Offset: 0x00025439
		protected override string featureKey
		{
			get
			{
				return "RainFeature";
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00027240 File Offset: 0x00025440
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("RainSoundVolume");
			helper.BlendNumber("RainNearIntensityKey");
			helper.BlendNumber("RainNearSpeedKey");
			helper.BlendNumber("RainNearTextureTiling");
			helper.BlendNumber("RainFarIntensityKey");
			helper.BlendNumber("RainFarSpeedKey");
			helper.BlendNumber("RainFarTextureTiling");
			helper.BlendColor("RainTintColorKey");
			helper.BlendNumber("RainWindTurbulenceKey");
			helper.BlendNumber("RainWindTurbulenceSpeedKey");
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x000272BB File Offset: 0x000254BB
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("RainSoundVolume", 0f);
			helper.BlendNumberIn("RainNearIntensityKey", 0f);
			helper.BlendNumberIn("RainFarIntensityKey", 0f);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000272ED File Offset: 0x000254ED
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("RainSoundVolume", 0f);
			helper.BlendNumberOut("RainNearIntensityKey", 0f);
			helper.BlendNumberOut("RainFarIntensityKey", 0f);
		}
	}
}
