using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001C7 RID: 455
	[Serializable]
	public class ProfileFeatureSection
	{
		// Token: 0x06000920 RID: 2336 RVA: 0x00028851 File Offset: 0x00026A51
		public ProfileFeatureSection(string sectionTitle, string sectionKey, ProfileFeatureDefinition[] featureDefinitions)
		{
			this.sectionTitle = sectionTitle;
			this.sectionKey = sectionKey;
			this.featureDefinitions = featureDefinitions;
		}

		// Token: 0x040009F3 RID: 2547
		public string sectionTitle;

		// Token: 0x040009F4 RID: 2548
		public string sectionKey;

		// Token: 0x040009F5 RID: 2549
		public string sectionIcon;

		// Token: 0x040009F6 RID: 2550
		public ProfileFeatureDefinition[] featureDefinitions;
	}
}
