using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200094A RID: 2378
	[CreateAssetMenu(fileName = "WeedDefinition", menuName = "ScriptableObjects/Item Definitions/WeedDefinition", order = 1)]
	[Serializable]
	public class WeedDefinition : ProductDefinition
	{
		// Token: 0x0600403B RID: 16443 RVA: 0x0010F739 File Offset: 0x0010D939
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new WeedInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0010F744 File Offset: 0x0010D944
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes, WeedAppearanceSettings _appearance)
		{
			base.Initialize(properties, drugTypes);
			if (_appearance == null || _appearance.IsUnintialized())
			{
				Console.LogWarning("Weed definition " + this.Name + " has no or uninitialized appearance settings! Generating new", null);
				_appearance = WeedDefinition.GetAppearanceSettings(properties);
			}
			this.appearance = _appearance;
			this.MainMat = new Material(this.MainMat);
			this.MainMat.color = this.appearance.MainColor;
			this.SecondaryMat = new Material(this.SecondaryMat);
			this.SecondaryMat.color = this.appearance.SecondaryColor;
			this.LeafMat = new Material(this.LeafMat);
			this.LeafMat.color = this.appearance.LeafColor;
			this.StemMat = new Material(this.StemMat);
			this.StemMat.color = this.appearance.StemColor;
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x0010F840 File Offset: 0x0010DA40
		public override ProductData GetSaveData()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new WeedProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array, this.appearance);
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0010F8B4 File Offset: 0x0010DAB4
		public static WeedAppearanceSettings GetAppearanceSettings(List<Property> properties)
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
			Color32 a = new Color32(90, 100, 70, byte.MaxValue);
			Color32 a2 = new Color32(120, 120, 80, byte.MaxValue);
			Color32 color = Color32.Lerp(a, list[0], (float)properties[0].Tier * 0.15f);
			Color32 color2 = Color32.Lerp(a2, Color32.Lerp(list[0], list[1], 0.5f), (properties.Count > 1) ? ((float)properties[1].Tier * 0.2f) : 0.5f);
			Color32 a3 = new Color32(0, 0, 0, byte.MaxValue);
			return new WeedAppearanceSettings
			{
				MainColor = color,
				SecondaryColor = color2,
				LeafColor = Color32.Lerp(color, color2, 0.5f),
				StemColor = Color32.Lerp(a3, color, 0.8f)
			};
		}

		// Token: 0x04002DAE RID: 11694
		[Header("Weed Materials")]
		public Material MainMat;

		// Token: 0x04002DAF RID: 11695
		public Material SecondaryMat;

		// Token: 0x04002DB0 RID: 11696
		public Material LeafMat;

		// Token: 0x04002DB1 RID: 11697
		public Material StemMat;

		// Token: 0x04002DB2 RID: 11698
		private WeedAppearanceSettings appearance;
	}
}
