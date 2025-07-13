using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A4 RID: 420
	public class MoonBlender : FeatureBlender
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x000271A7 File Offset: 0x000253A7
		protected override string featureKey
		{
			get
			{
				return "MoonFeature";
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000271B0 File Offset: 0x000253B0
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("MoonColorKey");
			helper.BlendNumber("MoonSizeKey");
			helper.BlendNumber("MoonEdgeFeatheringKey");
			helper.BlendNumber("MoonColorIntensityKey");
			helper.BlendNumber("MoonAlphaKey");
			helper.BlendColor("MoonLightColorKey");
			helper.BlendNumber("MoonLightIntensityKey");
			helper.BlendSpherePoint("MoonPositionKey");
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00027215 File Offset: 0x00025415
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("MoonAlphaKey", 0f);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00027227 File Offset: 0x00025427
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("MoonAlphaKey", 0f);
		}
	}
}
