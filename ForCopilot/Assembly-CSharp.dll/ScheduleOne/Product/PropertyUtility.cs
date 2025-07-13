using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000940 RID: 2368
	public class PropertyUtility : Singleton<PropertyUtility>
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x0010F354 File Offset: 0x0010D554
		protected override void Awake()
		{
			base.Awake();
			foreach (Property property in this.AllProperties)
			{
				this.PropertiesDict.Add(property.ID, property);
			}
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x0010F3B8 File Offset: 0x0010D5B8
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x0010F3C0 File Offset: 0x0010D5C0
		public List<Property> GetProperties(int tier)
		{
			bool excludePostMixingRework = false;
			if (SaveManager.GetVersionNumber(Singleton<MetadataManager>.Instance.CreationVersion) < 27f)
			{
				excludePostMixingRework = true;
			}
			return this.AllProperties.FindAll((Property x) => x.Tier == tier && (!excludePostMixingRework || x.ImplementedPriorMixingRework));
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x0010F418 File Offset: 0x0010D618
		public List<Property> GetProperties(List<string> ids)
		{
			List<Property> list = new List<Property>();
			using (List<string>.Enumerator enumerator = ids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string id = enumerator.Current;
					if (this.AllProperties.FirstOrDefault((Property x) => x.ID == id) == null)
					{
						Console.LogWarning("PropertyUtility: Property ID '" + id + "' not found!", null);
					}
					else
					{
						list.Add(this.PropertiesDict[id]);
					}
				}
			}
			return this.AllProperties.FindAll((Property x) => ids.Contains(x.ID));
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0010F4EC File Offset: 0x0010D6EC
		public static PropertyUtility.PropertyData GetPropertyData(EProperty property)
		{
			return Singleton<PropertyUtility>.Instance.PropertyDatas.Find((PropertyUtility.PropertyData x) => x.Property == property);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x0010F524 File Offset: 0x0010D724
		public static PropertyUtility.DrugTypeData GetDrugTypeData(EDrugType drugType)
		{
			return Singleton<PropertyUtility>.Instance.DrugTypeDatas.Find((PropertyUtility.DrugTypeData x) => x.DrugType == drugType);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x0010F55C File Offset: 0x0010D75C
		public static List<Color32> GetOrderedPropertyColors(List<Property> properties)
		{
			properties.Sort((Property x, Property y) => x.Tier.CompareTo(y.Tier));
			List<Color32> list = new List<Color32>();
			foreach (Property property in properties)
			{
				list.Add(property.ProductColor);
			}
			return list;
		}

		// Token: 0x04002D95 RID: 11669
		public List<PropertyUtility.PropertyData> PropertyDatas = new List<PropertyUtility.PropertyData>();

		// Token: 0x04002D96 RID: 11670
		public List<PropertyUtility.DrugTypeData> DrugTypeDatas = new List<PropertyUtility.DrugTypeData>();

		// Token: 0x04002D97 RID: 11671
		public List<Property> AllProperties = new List<Property>();

		// Token: 0x04002D98 RID: 11672
		[Header("Test Mixing")]
		public List<ProductDefinition> Products = new List<ProductDefinition>();

		// Token: 0x04002D99 RID: 11673
		public List<PropertyItemDefinition> Properties = new List<PropertyItemDefinition>();

		// Token: 0x04002D9A RID: 11674
		private Dictionary<string, Property> PropertiesDict = new Dictionary<string, Property>();

		// Token: 0x02000941 RID: 2369
		[Serializable]
		public class PropertyData
		{
			// Token: 0x04002D9B RID: 11675
			public EProperty Property;

			// Token: 0x04002D9C RID: 11676
			public string Name;

			// Token: 0x04002D9D RID: 11677
			public string Description;

			// Token: 0x04002D9E RID: 11678
			public Color Color;
		}

		// Token: 0x02000942 RID: 2370
		[Serializable]
		public class DrugTypeData
		{
			// Token: 0x04002D9F RID: 11679
			public EDrugType DrugType;

			// Token: 0x04002DA0 RID: 11680
			public string Name;

			// Token: 0x04002DA1 RID: 11681
			public Color Color;
		}
	}
}
