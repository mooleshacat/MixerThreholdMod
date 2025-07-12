using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001A9 RID: 425
	public class SunBlender : FeatureBlender
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0002757E File Offset: 0x0002577E
		protected override string featureKey
		{
			get
			{
				return "SunFeature";
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00027588 File Offset: 0x00025788
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("SunColorKey");
			helper.BlendNumber("SunSizeKey");
			helper.BlendNumber("SunEdgeFeatheringKey");
			helper.BlendNumber("SunColorIntensityKey");
			helper.BlendNumber("SunAlphaKey");
			helper.BlendColor("SunLightColorKey");
			helper.BlendNumber("SunLightIntensityKey");
			helper.BlendSpherePoint("SunPositionKey");
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x000275ED File Offset: 0x000257ED
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("SunAlphaKey", 0f);
			helper.BlendNumberIn("SunLightIntensityKey", 0f);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002760F File Offset: 0x0002580F
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("SunAlphaKey", 0f);
			helper.BlendNumberOut("SunLightIntensityKey", 0f);
		}
	}
}
