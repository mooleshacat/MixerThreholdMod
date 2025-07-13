using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A2 RID: 418
	public class FogBlender : FeatureBlender
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x000270DE File Offset: 0x000252DE
		protected override string featureKey
		{
			get
			{
				return "FogFeature";
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000270E5 File Offset: 0x000252E5
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("FogDensityKey");
			helper.BlendNumber("FogLengthKey");
			helper.BlendColor("FogColorKey");
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00027108 File Offset: 0x00025308
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("FogDensityKey", 0f);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002711A File Offset: 0x0002531A
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("FogDensityKey", 0f);
		}
	}
}
