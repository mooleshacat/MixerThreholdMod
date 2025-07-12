using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x02000922 RID: 2338
	[CreateAssetMenu(fileName = "MethDefinition", menuName = "ScriptableObjects/Item Definitions/MethDefinition", order = 1)]
	[Serializable]
	public class MethDefinition : ProductDefinition
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x001089E9 File Offset: 0x00106BE9
		// (set) Token: 0x06003EF7 RID: 16119 RVA: 0x001089F1 File Offset: 0x00106BF1
		public MethAppearanceSettings AppearanceSettings { get; private set; }

		// Token: 0x06003EF8 RID: 16120 RVA: 0x001089FA File Offset: 0x00106BFA
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			if (NetworkSingleton<ProductManager>.InstanceExists && !ProductManager.MethDiscovered)
			{
				NetworkSingleton<ProductManager>.Instance.SetMethDiscovered();
			}
			return new MethInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00108A20 File Offset: 0x00106C20
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes, MethAppearanceSettings _appearance)
		{
			base.Initialize(properties, drugTypes);
			if (_appearance == null || _appearance.IsUnintialized())
			{
				Console.LogWarning("Meth definition " + this.Name + " has no or uninitialized appearance settings! Generating new", null);
				_appearance = MethDefinition.GetAppearanceSettings(properties);
			}
			this.AppearanceSettings = _appearance;
			this.CrystalMaterial = new Material(this.CrystalMaterial);
			this.CrystalMaterial.color = this.AppearanceSettings.MainColor;
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00108A98 File Offset: 0x00106C98
		public override ProductData GetSaveData()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new MethProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array, this.AppearanceSettings);
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00108B0C File Offset: 0x00106D0C
		public static MethAppearanceSettings GetAppearanceSettings(List<Property> properties)
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
			Color32 mainColor = Color32.Lerp(a, list[0], (float)properties[0].Tier * 0.2f);
			Color32 secondaryColor = Color32.Lerp(a2, Color32.Lerp(list[0], list[1], 0.5f), (properties.Count > 1) ? ((float)properties[1].Tier * 0.2f) : 0.5f);
			return new MethAppearanceSettings
			{
				MainColor = mainColor,
				SecondaryColor = secondaryColor
			};
		}

		// Token: 0x04002D0A RID: 11530
		public Material CrystalMaterial;

		// Token: 0x04002D0B RID: 11531
		[ColorUsage(true, true)]
		[SerializeField]
		public Color TintColor = Color.white;
	}
}
