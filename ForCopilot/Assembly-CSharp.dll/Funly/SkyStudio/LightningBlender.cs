using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A3 RID: 419
	public class LightningBlender : FeatureBlender
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0002712C File Offset: 0x0002532C
		protected override string featureKey
		{
			get
			{
				return "LightningFeature";
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00027134 File Offset: 0x00025334
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("LightningTintColorKey");
			helper.BlendNumber("ThunderSoundVolumeKey");
			helper.BlendNumber("ThunderSoundDelayKey");
			helper.BlendNumber("LightningProbabilityKey");
			helper.BlendNumber("LightningStrikeCoolDown");
			helper.BlendNumber("LightningIntensityKey");
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00027183 File Offset: 0x00025383
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("ThunderSoundVolumeKey", 0f);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00027195 File Offset: 0x00025395
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("ThunderSoundVolumeKey", 0f);
		}
	}
}
