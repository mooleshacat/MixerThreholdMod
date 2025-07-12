using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001BF RID: 447
	[Serializable]
	public class ProfileFeatureDefinition
	{
		// Token: 0x0600090C RID: 2316 RVA: 0x000284E0 File Offset: 0x000266E0
		public static ProfileFeatureDefinition CreateShaderFeature(string featureKey, string shaderKeyword, bool value, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return new ProfileFeatureDefinition
			{
				featureType = ProfileFeatureDefinition.FeatureType.ShaderKeyword,
				featureKey = featureKey,
				shaderKeyword = shaderKeyword,
				name = name,
				value = value,
				tooltip = tooltip,
				dependsOnFeature = dependsOnFeature,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00028530 File Offset: 0x00026730
		public static ProfileFeatureDefinition CreateShaderFeatureDropdown(string[] featureKeys, string[] shaderKeywords, string[] labels, int selectedIndex, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return new ProfileFeatureDefinition
			{
				featureType = ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown,
				featureKeys = featureKeys,
				shaderKeywords = shaderKeywords,
				dropdownLabels = labels,
				name = name,
				dropdownSelectedIndex = selectedIndex,
				tooltip = tooltip,
				dependsOnFeature = dependsOnFeature,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00028585 File Offset: 0x00026785
		public static ProfileFeatureDefinition CreateBooleanFeature(string featureKey, bool value, string name, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return new ProfileFeatureDefinition
			{
				featureType = ProfileFeatureDefinition.FeatureType.BooleanValue,
				featureKey = featureKey,
				name = name,
				value = value,
				tooltip = tooltip
			};
		}

		// Token: 0x04000994 RID: 2452
		public string featureKey;

		// Token: 0x04000995 RID: 2453
		public string[] featureKeys;

		// Token: 0x04000996 RID: 2454
		public ProfileFeatureDefinition.FeatureType featureType;

		// Token: 0x04000997 RID: 2455
		public string shaderKeyword;

		// Token: 0x04000998 RID: 2456
		public string[] shaderKeywords;

		// Token: 0x04000999 RID: 2457
		public string[] dropdownLabels;

		// Token: 0x0400099A RID: 2458
		public int dropdownSelectedIndex;

		// Token: 0x0400099B RID: 2459
		public string name;

		// Token: 0x0400099C RID: 2460
		public bool value;

		// Token: 0x0400099D RID: 2461
		public string tooltip;

		// Token: 0x0400099E RID: 2462
		public string dependsOnFeature;

		// Token: 0x0400099F RID: 2463
		public bool dependsOnValue;

		// Token: 0x040009A0 RID: 2464
		public bool isShaderKeywordFeature;

		// Token: 0x020001C0 RID: 448
		public enum FeatureType
		{
			// Token: 0x040009A2 RID: 2466
			ShaderKeyword,
			// Token: 0x040009A3 RID: 2467
			BooleanValue,
			// Token: 0x040009A4 RID: 2468
			ShaderKeywordDropdown
		}
	}
}
