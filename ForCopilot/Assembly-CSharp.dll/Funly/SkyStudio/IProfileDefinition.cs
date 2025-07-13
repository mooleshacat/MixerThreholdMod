using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001D2 RID: 466
	public interface IProfileDefinition
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000933 RID: 2355
		string shaderName { get; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000934 RID: 2356
		ProfileFeatureSection[] features { get; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000935 RID: 2357
		ProfileGroupSection[] groups { get; }

		// Token: 0x06000936 RID: 2358
		ProfileFeatureDefinition GetFeatureDefinition(string featureKey);
	}
}
