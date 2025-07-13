using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x0200090F RID: 2319
	[CreateAssetMenu(fileName = "StationRecipe", menuName = "StationFramework/StationRecipe", order = 1)]
	[Serializable]
	public class StationRecipe : ScriptableObject
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003EC4 RID: 16068 RVA: 0x00107E96 File Offset: 0x00106096
		public float CookTemperatureLowerBound
		{
			get
			{
				return this.CookTemperature - this.CookTemperatureTolerance;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x00107EA5 File Offset: 0x001060A5
		public float CookTemperatureUpperBound
		{
			get
			{
				return this.CookTemperature + this.CookTemperatureTolerance;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x00107EB4 File Offset: 0x001060B4
		public string RecipeID
		{
			get
			{
				return this.Product.Quantity.ToString() + "x" + this.Product.Item.ID;
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00107EE0 File Offset: 0x001060E0
		public StorableItemInstance GetProductInstance(List<ItemInstance> ingredients)
		{
			StorableItemInstance storableItemInstance = this.Product.Item.GetDefaultInstance(this.Product.Quantity) as StorableItemInstance;
			if (storableItemInstance is QualityItemInstance)
			{
				EQuality quality = this.CalculateQuality(ingredients);
				(storableItemInstance as QualityItemInstance).Quality = quality;
			}
			return storableItemInstance;
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x00107F2C File Offset: 0x0010612C
		public StorableItemInstance GetProductInstance(EQuality quality)
		{
			StorableItemInstance storableItemInstance = this.Product.Item.GetDefaultInstance(this.Product.Quantity) as StorableItemInstance;
			if (storableItemInstance is QualityItemInstance)
			{
				(storableItemInstance as QualityItemInstance).Quality = quality;
			}
			return storableItemInstance;
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x00107F70 File Offset: 0x00106170
		public bool DoIngredientsSuffice(List<ItemInstance> ingredients)
		{
			for (int i = 0; i < this.Ingredients.Count; i++)
			{
				List<ItemInstance> list = new List<ItemInstance>();
				using (List<ItemDefinition>.Enumerator enumerator = this.Ingredients[i].Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemDefinition ingredientVariant = enumerator.Current;
						List<ItemInstance> collection = (from x in ingredients
						where x.ID == ingredientVariant.ID
						select x).ToList<ItemInstance>();
						list.AddRange(collection);
					}
				}
				int num = 0;
				for (int j = 0; j < list.Count; j++)
				{
					num += list[j].Quantity;
				}
				if (num < this.Ingredients[i].Quantity)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x00108058 File Offset: 0x00106258
		public EQuality CalculateQuality(List<ItemInstance> ingredients)
		{
			EQuality result = EQuality.Standard;
			if (this.QualityCalculationMethod == StationRecipe.EQualityCalculationMethod.Additive)
			{
				int num = 0;
				for (int i = 0; i < ingredients.Count; i++)
				{
					if (ingredients[i] is QualityItemInstance)
					{
						switch ((ingredients[i] as QualityItemInstance).Quality)
						{
						case EQuality.Trash:
							num -= 2;
							break;
						case EQuality.Poor:
							num--;
							break;
						case EQuality.Standard:
							num = num;
							break;
						case EQuality.Premium:
							num++;
							break;
						case EQuality.Heavenly:
							num += 2;
							break;
						}
					}
				}
				if ((float)num <= -2f)
				{
					result = EQuality.Trash;
				}
				else if ((float)num == -1f)
				{
					result = EQuality.Poor;
				}
				else if ((float)num == 0f)
				{
					result = EQuality.Standard;
				}
				else if ((float)num == 1f)
				{
					result = EQuality.Premium;
				}
				else if ((float)num >= 2f)
				{
					result = EQuality.Heavenly;
				}
			}
			return result;
		}

		// Token: 0x04002CCD RID: 11469
		[HideInInspector]
		public bool IsDiscovered;

		// Token: 0x04002CCE RID: 11470
		public string RecipeTitle;

		// Token: 0x04002CCF RID: 11471
		public bool Unlocked;

		// Token: 0x04002CD0 RID: 11472
		public List<StationRecipe.IngredientQuantity> Ingredients = new List<StationRecipe.IngredientQuantity>();

		// Token: 0x04002CD1 RID: 11473
		public StationRecipe.ItemQuantity Product;

		// Token: 0x04002CD2 RID: 11474
		public Color FinalLiquidColor = Color.white;

		// Token: 0x04002CD3 RID: 11475
		[Tooltip("The time it takes to cook this recipe in minutes")]
		public int CookTime_Mins = 180;

		// Token: 0x04002CD4 RID: 11476
		[Tooltip("The temperature at which this recipe should be cooked")]
		[Range(0f, 500f)]
		public float CookTemperature = 250f;

		// Token: 0x04002CD5 RID: 11477
		[Range(0f, 100f)]
		public float CookTemperatureTolerance = 25f;

		// Token: 0x04002CD6 RID: 11478
		public StationRecipe.EQualityCalculationMethod QualityCalculationMethod;

		// Token: 0x02000910 RID: 2320
		public enum EQualityCalculationMethod
		{
			// Token: 0x04002CD8 RID: 11480
			Additive
		}

		// Token: 0x02000911 RID: 2321
		[Serializable]
		public class ItemQuantity
		{
			// Token: 0x04002CD9 RID: 11481
			public ItemDefinition Item;

			// Token: 0x04002CDA RID: 11482
			public int Quantity = 1;
		}

		// Token: 0x02000912 RID: 2322
		[Serializable]
		public class IngredientQuantity
		{
			// Token: 0x170008BE RID: 2238
			// (get) Token: 0x06003ECD RID: 16077 RVA: 0x00108169 File Offset: 0x00106369
			public ItemDefinition Item
			{
				get
				{
					return this.Items.FirstOrDefault<ItemDefinition>();
				}
			}

			// Token: 0x04002CDB RID: 11483
			public List<ItemDefinition> Items = new List<ItemDefinition>();

			// Token: 0x04002CDC RID: 11484
			public int Quantity = 1;
		}
	}
}
