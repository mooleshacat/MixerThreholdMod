using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Properties;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200092A RID: 2346
	[CreateAssetMenu(fileName = "ProductDefinition", menuName = "ScriptableObjects/ProductDefinition", order = 1)]
	[Serializable]
	public class ProductDefinition : PropertyItemDefinition, ISaveable
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003F1B RID: 16155 RVA: 0x001094B7 File Offset: 0x001076B7
		public EDrugType DrugType
		{
			get
			{
				return this.DrugTypes[0].DrugType;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003F1C RID: 16156 RVA: 0x001094CA File Offset: 0x001076CA
		public float Price
		{
			get
			{
				return NetworkSingleton<ProductManager>.Instance.GetPrice(this);
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003F1D RID: 16157 RVA: 0x001094D7 File Offset: 0x001076D7
		// (set) Token: 0x06003F1E RID: 16158 RVA: 0x001094DF File Offset: 0x001076DF
		public List<StationRecipe> Recipes { get; private set; } = new List<StationRecipe>();

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003F1F RID: 16159 RVA: 0x001094E8 File Offset: 0x001076E8
		public string SaveFolderName
		{
			get
			{
				return SaveManager.SanitizeFileName(this.ID);
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003F20 RID: 16160 RVA: 0x001094E8 File Offset: 0x001076E8
		public string SaveFileName
		{
			get
			{
				return SaveManager.SanitizeFileName(this.ID);
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x001094F5 File Offset: 0x001076F5
		// (set) Token: 0x06003F24 RID: 16164 RVA: 0x001094FD File Offset: 0x001076FD
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x00109506 File Offset: 0x00107706
		// (set) Token: 0x06003F26 RID: 16166 RVA: 0x0010950E File Offset: 0x0010770E
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x00109517 File Offset: 0x00107717
		// (set) Token: 0x06003F28 RID: 16168 RVA: 0x0010951F File Offset: 0x0010771F
		public bool HasChanged { get; set; }

		// Token: 0x06003F29 RID: 16169 RVA: 0x00109528 File Offset: 0x00107728
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new ProductItemInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00109533 File Offset: 0x00107733
		public void OnValidate()
		{
			this.MarketValue = ProductManager.CalculateProductValue(this, this.BasePrice);
			this.CleanRecipes();
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00109550 File Offset: 0x00107750
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes)
		{
			base.Initialize(properties);
			this.DrugTypes = new List<DrugTypeContainer>();
			for (int i = 0; i < drugTypes.Count; i++)
			{
				this.DrugTypes.Add(new DrugTypeContainer
				{
					DrugType = drugTypes[i]
				});
			}
			this.CleanRecipes();
			this.MarketValue = ProductManager.CalculateProductValue(this, this.BasePrice);
			this.InitializeSaveable();
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x001095BC File Offset: 0x001077BC
		public float GetAddictiveness()
		{
			float num = this.BaseAddictiveness;
			for (int i = 0; i < this.Properties.Count; i++)
			{
				num += this.Properties[i].Addictiveness;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00109600 File Offset: 0x00107800
		public void CleanRecipes()
		{
			for (int i = this.Recipes.Count - 1; i >= 0; i--)
			{
				if (this.Recipes[i] == null)
				{
					this.Recipes.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00109645 File Offset: 0x00107845
		public void AddRecipe(StationRecipe recipe)
		{
			if (recipe.Product.Item != this)
			{
				Debug.LogError("Recipe product does not match this product.");
				return;
			}
			if (!this.Recipes.Contains(recipe))
			{
				this.Recipes.Add(recipe);
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00109680 File Offset: 0x00107880
		public virtual ProductData GetSaveData()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new ProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x001096EB File Offset: 0x001078EB
		public virtual string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x04002D26 RID: 11558
		[Header("Product Settings")]
		public List<DrugTypeContainer> DrugTypes;

		// Token: 0x04002D27 RID: 11559
		public float LawIntensityChange = 1f;

		// Token: 0x04002D28 RID: 11560
		public float BasePrice = 1f;

		// Token: 0x04002D29 RID: 11561
		public float MarketValue = 1f;

		// Token: 0x04002D2A RID: 11562
		public FunctionalProduct FunctionalProduct;

		// Token: 0x04002D2B RID: 11563
		public int EffectsDuration = 180;

		// Token: 0x04002D2C RID: 11564
		[Range(0f, 1f)]
		public float BaseAddictiveness;

		// Token: 0x04002D2D RID: 11565
		[Header("Packaging that can be applied to this product. MUST BE ORDERED FROm LOWEST TO HIGHEST QUANTITY")]
		public PackagingDefinition[] ValidPackaging;
	}
}
