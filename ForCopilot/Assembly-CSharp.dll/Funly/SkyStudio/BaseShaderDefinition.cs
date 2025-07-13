using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D1 RID: 465
	[Serializable]
	public abstract class BaseShaderDefinition : IProfileDefinition
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000288F7 File Offset: 0x00026AF7
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x000288FF File Offset: 0x00026AFF
		public string shaderName { get; protected set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00028908 File Offset: 0x00026B08
		public ProfileGroupSection[] groups
		{
			get
			{
				ProfileGroupSection[] result;
				if ((result = this.m_ProfileDefinitions) == null)
				{
					result = (this.m_ProfileDefinitions = this.ProfileDefinitionTable());
				}
				return result;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00028930 File Offset: 0x00026B30
		public ProfileFeatureSection[] features
		{
			get
			{
				ProfileFeatureSection[] result;
				if ((result = this.m_ProfileFeatures) == null)
				{
					result = (this.m_ProfileFeatures = this.ProfileFeatureSection());
				}
				return result;
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00028958 File Offset: 0x00026B58
		public ProfileFeatureDefinition GetFeatureDefinition(string featureKey)
		{
			if (this.m_KeyToFeature == null)
			{
				this.m_KeyToFeature = new Dictionary<string, ProfileFeatureDefinition>();
				ProfileFeatureSection[] features = this.features;
				for (int i = 0; i < features.Length; i++)
				{
					foreach (ProfileFeatureDefinition profileFeatureDefinition in features[i].featureDefinitions)
					{
						if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.BooleanValue || profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
						{
							this.m_KeyToFeature[profileFeatureDefinition.featureKey] = profileFeatureDefinition;
						}
						else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
						{
							foreach (string key in profileFeatureDefinition.featureKeys)
							{
								this.m_KeyToFeature[key] = profileFeatureDefinition;
							}
						}
					}
				}
			}
			if (featureKey == null)
			{
				return null;
			}
			if (!this.m_KeyToFeature.ContainsKey(featureKey))
			{
				return null;
			}
			return this.m_KeyToFeature[featureKey];
		}

		// Token: 0x06000930 RID: 2352
		protected abstract ProfileFeatureSection[] ProfileFeatureSection();

		// Token: 0x06000931 RID: 2353
		protected abstract ProfileGroupSection[] ProfileDefinitionTable();

		// Token: 0x04000ABB RID: 2747
		private ProfileGroupSection[] m_ProfileDefinitions;

		// Token: 0x04000ABC RID: 2748
		[SerializeField]
		private ProfileFeatureSection[] m_ProfileFeatures;

		// Token: 0x04000ABD RID: 2749
		private Dictionary<string, ProfileFeatureDefinition> m_KeyToFeature;
	}
}
