using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000918 RID: 2328
	[CreateAssetMenu(fileName = "CocaineDefinition", menuName = "ScriptableObjects/Item Definitions/CocaineDefinition", order = 1)]
	[Serializable]
	public class CocaineDefinition : ProductDefinition
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x001082C6 File Offset: 0x001064C6
		// (set) Token: 0x06003EDA RID: 16090 RVA: 0x001082CE File Offset: 0x001064CE
		public CocaineAppearanceSettings AppearanceSettings { get; private set; }

		// Token: 0x06003EDB RID: 16091 RVA: 0x001082D7 File Offset: 0x001064D7
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			if (NetworkSingleton<ProductManager>.InstanceExists && !ProductManager.CocaineDiscovered)
			{
				NetworkSingleton<ProductManager>.Instance.SetCocaineDiscovered();
			}
			return new CocaineInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x001082FC File Offset: 0x001064FC
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes, CocaineAppearanceSettings _appearance)
		{
			base.Initialize(properties, drugTypes);
			if (_appearance == null || _appearance.IsUnintialized())
			{
				Console.LogWarning("Coke definition " + this.Name + " has no or uninitialized appearance settings! Generating new", null);
				_appearance = CocaineDefinition.GetAppearanceSettings(properties);
			}
			this.AppearanceSettings = _appearance;
			this.RockMaterial = new Material(this.RockMaterial);
			this.RockMaterial.color = this.AppearanceSettings.MainColor;
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00108374 File Offset: 0x00106574
		public override ProductData GetSaveData()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new CocaineProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array, this.AppearanceSettings);
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x001083E8 File Offset: 0x001065E8
		public static CocaineAppearanceSettings GetAppearanceSettings(List<Property> properties)
		{
			properties.Sort((Property x, Property y) => x.Tier.CompareTo(y.Tier));
			List<Color32> list = new List<Color32>();
			foreach (Property property in properties)
			{
				list.Add(property.ProductColor);
			}
			if (list.Count == 1)
			{
				list.Add(list[0]);
			}
			Color32 a = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Color32 a2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Color32 mainColor = Color32.Lerp(a, list[0], (float)properties[0].Tier * 0.13f);
			Color32 secondaryColor = Color32.Lerp(a2, Color32.Lerp(list[0], list[1], 0.5f), (properties.Count > 1) ? ((float)properties[1].Tier * 0.2f) : 0.5f);
			return new CocaineAppearanceSettings
			{
				MainColor = mainColor,
				SecondaryColor = secondaryColor
			};
		}

		// Token: 0x04002CE9 RID: 11497
		[Header("Materials")]
		public Material RockMaterial;
	}
}
