using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001C6 RID: 454
	public class ProfileGroupSection
	{
		// Token: 0x0600091F RID: 2335 RVA: 0x0002881C File Offset: 0x00026A1C
		public ProfileGroupSection(string sectionTitle, string sectionKey, string sectionIcon, string dependsOnFeature, bool dependsOnValue, ProfileGroupDefinition[] groups)
		{
			this.sectionTitle = sectionTitle;
			this.sectionIcon = sectionIcon;
			this.sectionKey = sectionKey;
			this.groups = groups;
			this.dependsOnFeature = dependsOnFeature;
			this.dependsOnValue = dependsOnValue;
		}

		// Token: 0x040009ED RID: 2541
		public string sectionTitle;

		// Token: 0x040009EE RID: 2542
		public string sectionIcon;

		// Token: 0x040009EF RID: 2543
		public string sectionKey;

		// Token: 0x040009F0 RID: 2544
		public string dependsOnFeature;

		// Token: 0x040009F1 RID: 2545
		public bool dependsOnValue;

		// Token: 0x040009F2 RID: 2546
		public ProfileGroupDefinition[] groups;
	}
}
