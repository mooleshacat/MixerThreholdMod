using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D4 RID: 468
	[CreateAssetMenu(fileName = "skyProfile.asset", menuName = "Sky Studio/Sky Profile", order = 0)]
	public class SkyProfile : ScriptableObject
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0002A876 File Offset: 0x00028A76
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x0002A880 File Offset: 0x00028A80
		public Material skyboxMaterial
		{
			get
			{
				return this.m_SkyboxMaterial;
			}
			set
			{
				if (value == null)
				{
					this.m_SkyboxMaterial = null;
					return;
				}
				if (this.m_SkyboxMaterial && this.m_SkyboxMaterial.shader.name != value.shader.name)
				{
					this.m_SkyboxMaterial = value;
					this.m_ShaderName = value.shader.name;
					this.ReloadDefinitions();
					return;
				}
				this.m_SkyboxMaterial = value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0002A8F3 File Offset: 0x00028AF3
		public string shaderName
		{
			get
			{
				return this.m_ShaderName;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0002A8FB File Offset: 0x00028AFB
		public ProfileGroupSection[] groupDefinitions
		{
			get
			{
				if (this.profileDefinition == null)
				{
					return null;
				}
				return this.profileDefinition.groups;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0002A912 File Offset: 0x00028B12
		public ProfileFeatureSection[] featureDefinitions
		{
			get
			{
				if (this.profileDefinition == null)
				{
					return null;
				}
				return this.profileDefinition.features;
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002A929 File Offset: 0x00028B29
		public float GetNumberPropertyValue(string propertyKey)
		{
			return this.GetNumberPropertyValue(propertyKey, 0f);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002A938 File Offset: 0x00028B38
		public float GetNumberPropertyValue(string propertyKey, float timeOfDay)
		{
			NumberKeyframeGroup group = this.GetGroup<NumberKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find number group with property key: " + propertyKey);
				return -1f;
			}
			return group.NumericValueAtTime(timeOfDay);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002A96D File Offset: 0x00028B6D
		public Color GetColorPropertyValue(string propertyKey)
		{
			return this.GetColorPropertyValue(propertyKey, 0f);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002A97C File Offset: 0x00028B7C
		public Color GetColorPropertyValue(string propertyKey, float timeOfDay)
		{
			ColorKeyframeGroup group = this.GetGroup<ColorKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find color group with property key: " + propertyKey);
				return Color.white;
			}
			return group.ColorForTime(timeOfDay);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002A9B1 File Offset: 0x00028BB1
		public Texture GetTexturePropertyValue(string propertyKey)
		{
			return this.GetTexturePropertyValue(propertyKey, 0f);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002A9C0 File Offset: 0x00028BC0
		public Texture GetTexturePropertyValue(string propertyKey, float timeOfDay)
		{
			TextureKeyframeGroup group = this.GetGroup<TextureKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find texture group with property key: " + propertyKey);
				return null;
			}
			return group.TextureForTime(timeOfDay);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002A9F1 File Offset: 0x00028BF1
		public SpherePoint GetSpherePointPropertyValue(string propertyKey)
		{
			return this.GetSpherePointPropertyValue(propertyKey, 0f);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002AA00 File Offset: 0x00028C00
		public SpherePoint GetSpherePointPropertyValue(string propertyKey, float timeOfDay)
		{
			SpherePointKeyframeGroup group = this.GetGroup<SpherePointKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find a sphere point group with property key: " + propertyKey);
				return null;
			}
			return group.SpherePointForTime(timeOfDay);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002AA31 File Offset: 0x00028C31
		public bool GetBoolPropertyValue(string propertyKey)
		{
			return this.GetBoolPropertyValue(propertyKey, 0f);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002AA40 File Offset: 0x00028C40
		public bool GetBoolPropertyValue(string propertyKey, float timeOfDay)
		{
			BoolKeyframeGroup group = this.GetGroup<BoolKeyframeGroup>(propertyKey);
			if (group == null)
			{
				Debug.LogError("Can't find boolean group with property key: " + propertyKey);
				return false;
			}
			return group.BoolForTime(timeOfDay);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002AA74 File Offset: 0x00028C74
		public SkyProfile()
		{
			this.ReloadFullProfile();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002AAC0 File Offset: 0x00028CC0
		private void OnEnable()
		{
			this.ReloadFullProfile();
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002AAC8 File Offset: 0x00028CC8
		private void ReloadFullProfile()
		{
			this.ReloadDefinitions();
			this.MergeProfileWithDefinitions();
			this.RebuildKeyToGroupInfoMapping();
			this.ValidateTimelineGroupKeys();
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002AAE2 File Offset: 0x00028CE2
		private void ReloadDefinitions()
		{
			this.profileDefinition = this.GetShaderInfoForMaterial(this.m_ShaderName);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002AAF6 File Offset: 0x00028CF6
		private IProfileDefinition GetShaderInfoForMaterial(string shaderName)
		{
			return new Standard3dShaderDefinition();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002AAFD File Offset: 0x00028CFD
		public void MergeProfileWithDefinitions()
		{
			this.MergeGroupsWithDefinitions();
			this.MergeShaderKeywordsWithDefinitions();
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002AB0C File Offset: 0x00028D0C
		public void MergeGroupsWithDefinitions()
		{
			HashSet<string> propertyKeysSet = ProfilePropertyKeys.GetPropertyKeysSet();
			ProfileGroupSection[] groupDefinitions = this.groupDefinitions;
			for (int i = 0; i < groupDefinitions.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in groupDefinitions[i].groups)
				{
					if (propertyKeysSet.Contains(profileGroupDefinition.propertyKey))
					{
						if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Color)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddColorGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.color);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Number)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddNumericGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.minimumValue, profileGroupDefinition.maximumValue, profileGroupDefinition.value);
							}
							else
							{
								NumberKeyframeGroup group = this.keyframeGroups.GetGroup<NumberKeyframeGroup>(profileGroupDefinition.propertyKey);
								group.name = profileGroupDefinition.groupName;
								group.minValue = profileGroupDefinition.minimumValue;
								group.maxValue = profileGroupDefinition.maximumValue;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Texture)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddTextureGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.texture);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.SpherePoint)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddSpherePointGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.spherePoint);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
						else if (profileGroupDefinition.type == ProfileGroupDefinition.GroupType.Boolean)
						{
							if (!this.keyframeGroups.ContainsKey(profileGroupDefinition.propertyKey))
							{
								this.AddBooleanGroup(profileGroupDefinition.propertyKey, profileGroupDefinition.groupName, profileGroupDefinition.boolValue);
							}
							else
							{
								this.keyframeGroups[profileGroupDefinition.propertyKey].name = profileGroupDefinition.groupName;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002AD74 File Offset: 0x00028F74
		public Dictionary<string, ProfileGroupDefinition> GroupDefinitionDictionary()
		{
			ProfileGroupSection[] array = this.ProfileDefinitionTable();
			Dictionary<string, ProfileGroupDefinition> dictionary = new Dictionary<string, ProfileGroupDefinition>();
			ProfileGroupSection[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in array2[i].groups)
				{
					dictionary.Add(profileGroupDefinition.propertyKey, profileGroupDefinition);
				}
			}
			return dictionary;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002ADCD File Offset: 0x00028FCD
		public ProfileGroupSection[] ProfileDefinitionTable()
		{
			return this.groupDefinitions;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002ADD8 File Offset: 0x00028FD8
		private void AddNumericGroup(string propKey, string groupName, float min, float max, float value)
		{
			NumberKeyframeGroup value2 = new NumberKeyframeGroup(groupName, min, max, new NumberKeyframe(0f, value));
			this.keyframeGroups[propKey] = value2;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002AE08 File Offset: 0x00029008
		private void AddColorGroup(string propKey, string groupName, Color color)
		{
			ColorKeyframeGroup value = new ColorKeyframeGroup(groupName, new ColorKeyframe(color, 0f));
			this.keyframeGroups[propKey] = value;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002AE34 File Offset: 0x00029034
		private void AddTextureGroup(string propKey, string groupName, Texture2D texture)
		{
			TextureKeyframeGroup value = new TextureKeyframeGroup(groupName, new TextureKeyframe(texture, 0f));
			this.keyframeGroups[propKey] = value;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002AE60 File Offset: 0x00029060
		private void AddSpherePointGroup(string propKey, string groupName, SpherePoint point)
		{
			SpherePointKeyframeGroup value = new SpherePointKeyframeGroup(groupName, new SpherePointKeyframe(point, 0f));
			this.keyframeGroups[propKey] = value;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002AE8C File Offset: 0x0002908C
		private void AddBooleanGroup(string propKey, string groupName, bool value)
		{
			BoolKeyframeGroup value2 = new BoolKeyframeGroup(groupName, new BoolKeyframe(0f, value));
			this.keyframeGroups[propKey] = value2;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002AEB8 File Offset: 0x000290B8
		public T GetGroup<T>(string propertyKey) where T : class
		{
			if (!this.keyframeGroups.ContainsKey(propertyKey))
			{
				Debug.Log("Key does not exist in sky profile, ignoring: " + propertyKey);
				return default(T);
			}
			return this.keyframeGroups[propertyKey] as T;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002AF03 File Offset: 0x00029103
		public IKeyframeGroup GetGroup(string propertyKey)
		{
			return this.keyframeGroups[propertyKey];
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002AF14 File Offset: 0x00029114
		public IKeyframeGroup GetGroupWithId(string groupId)
		{
			if (groupId == null)
			{
				return null;
			}
			foreach (string aKey in this.keyframeGroups)
			{
				IKeyframeGroup keyframeGroup = this.keyframeGroups[aKey];
				if (keyframeGroup.id == groupId)
				{
					return keyframeGroup;
				}
			}
			return null;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002ADCD File Offset: 0x00028FCD
		public ProfileGroupSection[] GetProfileDefinitions()
		{
			return this.groupDefinitions;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002AF84 File Offset: 0x00029184
		public ProfileGroupSection GetSectionInfo(string sectionKey)
		{
			foreach (ProfileGroupSection profileGroupSection in this.groupDefinitions)
			{
				if (profileGroupSection.sectionKey == sectionKey)
				{
					return profileGroupSection;
				}
			}
			return null;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002AFBB File Offset: 0x000291BB
		public bool IsManagedByTimeline(string propertyKey)
		{
			return this.timelineManagedKeys.Contains(propertyKey);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002AFCC File Offset: 0x000291CC
		public void ValidateTimelineGroupKeys()
		{
			List<string> list = new List<string>();
			HashSet<string> propertyKeysSet = ProfilePropertyKeys.GetPropertyKeysSet();
			foreach (string text in this.timelineManagedKeys)
			{
				if (!this.IsManagedByTimeline(text) || !propertyKeysSet.Contains(text))
				{
					list.Add(text);
				}
			}
			foreach (string item in list)
			{
				if (this.timelineManagedKeys.Contains(item))
				{
					this.timelineManagedKeys.Remove(item);
				}
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002B094 File Offset: 0x00029294
		public List<ProfileGroupDefinition> GetGroupDefinitionsManagedByTimeline()
		{
			List<ProfileGroupDefinition> list = new List<ProfileGroupDefinition>();
			foreach (string propertyKey in this.timelineManagedKeys)
			{
				ProfileGroupDefinition groupDefinitionForKey = this.GetGroupDefinitionForKey(propertyKey);
				if (groupDefinitionForKey != null)
				{
					list.Add(groupDefinitionForKey);
				}
			}
			return list;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002B0FC File Offset: 0x000292FC
		public List<ProfileGroupDefinition> GetGroupDefinitionsNotManagedByTimeline()
		{
			List<ProfileGroupDefinition> list = new List<ProfileGroupDefinition>();
			ProfileGroupSection[] groupDefinitions = this.groupDefinitions;
			for (int i = 0; i < groupDefinitions.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in groupDefinitions[i].groups)
				{
					if (!this.IsManagedByTimeline(profileGroupDefinition.propertyKey) && this.CanGroupBeOnTimeline(profileGroupDefinition))
					{
						list.Add(profileGroupDefinition);
					}
				}
			}
			return list;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002B168 File Offset: 0x00029368
		public ProfileGroupDefinition GetGroupDefinitionForKey(string propertyKey)
		{
			ProfileGroupDefinition result = null;
			if (this.m_KeyToGroupInfo.TryGetValue(propertyKey, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002B18C File Offset: 0x0002938C
		public void RebuildKeyToGroupInfoMapping()
		{
			this.m_KeyToGroupInfo = new Dictionary<string, ProfileGroupDefinition>();
			ProfileGroupSection[] groupDefinitions = this.groupDefinitions;
			for (int i = 0; i < groupDefinitions.Length; i++)
			{
				foreach (ProfileGroupDefinition profileGroupDefinition in groupDefinitions[i].groups)
				{
					this.m_KeyToGroupInfo[profileGroupDefinition.propertyKey] = profileGroupDefinition;
				}
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0002B1EC File Offset: 0x000293EC
		public void TrimGroupToSingleKeyframe(string propertyKey)
		{
			IKeyframeGroup group = this.GetGroup(propertyKey);
			if (group == null)
			{
				return;
			}
			group.TrimToSingleKeyframe();
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0002B20C File Offset: 0x0002940C
		public bool CanGroupBeOnTimeline(ProfileGroupDefinition definition)
		{
			return definition.type != ProfileGroupDefinition.GroupType.Texture && (!definition.propertyKey.Contains("Star") || !definition.propertyKey.Contains("Density")) && !definition.propertyKey.Contains("Sprite") && definition.type != ProfileGroupDefinition.GroupType.Boolean;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002B264 File Offset: 0x00029464
		protected void MergeShaderKeywordsWithDefinitions()
		{
			ProfileFeatureSection[] features = this.profileDefinition.features;
			for (int i = 0; i < features.Length; i++)
			{
				foreach (ProfileFeatureDefinition profileFeatureDefinition in features[i].featureDefinitions)
				{
					string text = null;
					bool value = false;
					if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.BooleanValue || profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
					{
						text = profileFeatureDefinition.featureKey;
						value = profileFeatureDefinition.value;
					}
					else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
					{
						text = profileFeatureDefinition.featureKeys[profileFeatureDefinition.dropdownSelectedIndex];
						value = true;
					}
					if (text != null && !this.featureStatus.dict.ContainsKey(text))
					{
						this.SetFeatureEnabled(text, value);
					}
				}
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002B31C File Offset: 0x0002951C
		public bool IsFeatureEnabled(string featureKey, bool recursive = true)
		{
			if (featureKey == null)
			{
				return false;
			}
			ProfileFeatureDefinition featureDefinition = this.profileDefinition.GetFeatureDefinition(featureKey);
			if (featureDefinition == null)
			{
				return false;
			}
			if (!this.featureStatus.dict.ContainsKey(featureKey) || !this.featureStatus[featureKey])
			{
				return false;
			}
			if (!recursive)
			{
				return true;
			}
			ProfileFeatureDefinition featureDefinition2;
			for (ProfileFeatureDefinition profileFeatureDefinition = featureDefinition; profileFeatureDefinition != null; profileFeatureDefinition = featureDefinition2)
			{
				featureDefinition2 = this.profileDefinition.GetFeatureDefinition(profileFeatureDefinition.dependsOnFeature);
				if (featureDefinition2 == null || featureDefinition2.featureKey == null)
				{
					break;
				}
				if (this.featureStatus[featureDefinition2.featureKey] != profileFeatureDefinition.dependsOnValue)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002B3AA File Offset: 0x000295AA
		public void SetFeatureEnabled(string featureKey, bool value)
		{
			if (featureKey == null)
			{
				Debug.LogError("Can't set null feature key value");
				return;
			}
			this.featureStatus[featureKey] = value;
		}

		// Token: 0x04000AC7 RID: 2759
		public const string DefaultShaderName = "Funly/Sky Studio/Skybox/3D Standard";

		// Token: 0x04000AC8 RID: 2760
		public const string DefaultLegacyShaderName = "Funly/Sky Studio/Skybox/3D Standard - Global Keywords";

		// Token: 0x04000AC9 RID: 2761
		[SerializeField]
		private Material m_SkyboxMaterial;

		// Token: 0x04000ACA RID: 2762
		[SerializeField]
		private string m_ShaderName = "Funly/Sky Studio/Skybox/3D Standard";

		// Token: 0x04000ACB RID: 2763
		public IProfileDefinition profileDefinition;

		// Token: 0x04000ACC RID: 2764
		public List<string> timelineManagedKeys = new List<string>();

		// Token: 0x04000ACD RID: 2765
		public KeyframeGroupDictionary keyframeGroups = new KeyframeGroupDictionary();

		// Token: 0x04000ACE RID: 2766
		public BoolDictionary featureStatus = new BoolDictionary();

		// Token: 0x04000ACF RID: 2767
		public LightningArtSet lightningArtSet;

		// Token: 0x04000AD0 RID: 2768
		public RainSplashArtSet rainSplashArtSet;

		// Token: 0x04000AD1 RID: 2769
		public Texture2D starLayer1DataTexture;

		// Token: 0x04000AD2 RID: 2770
		public Texture2D starLayer2DataTexture;

		// Token: 0x04000AD3 RID: 2771
		public Texture2D starLayer3DataTexture;

		// Token: 0x04000AD4 RID: 2772
		[SerializeField]
		private int m_ProfileVersion = 2;

		// Token: 0x04000AD5 RID: 2773
		private Dictionary<string, ProfileGroupDefinition> m_KeyToGroupInfo;
	}
}
