using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AA9 RID: 2729
	public class StationRecipeEntry : MonoBehaviour
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600494F RID: 18767 RVA: 0x00133CA9 File Offset: 0x00131EA9
		// (set) Token: 0x06004950 RID: 18768 RVA: 0x00133CB1 File Offset: 0x00131EB1
		public bool IsValid { get; private set; }

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06004951 RID: 18769 RVA: 0x00133CBA File Offset: 0x00131EBA
		// (set) Token: 0x06004952 RID: 18770 RVA: 0x00133CC2 File Offset: 0x00131EC2
		public StationRecipe Recipe { get; private set; }

		// Token: 0x06004953 RID: 18771 RVA: 0x00133CCC File Offset: 0x00131ECC
		public void AssignRecipe(StationRecipe recipe)
		{
			this.Recipe = recipe;
			this.Icon.sprite = recipe.Product.Item.Icon;
			this.TitleLabel.text = recipe.RecipeTitle;
			if (recipe.Product.Quantity > 1)
			{
				this.TitleLabel.text = this.TitleLabel.text + "(" + recipe.Product.Quantity.ToString() + "x)";
			}
			this.Icon.GetComponent<ItemDefinitionInfoHoverable>().AssignedItem = recipe.Product.Item;
			int num = recipe.CookTime_Mins / 60;
			int num2 = recipe.CookTime_Mins % 60;
			this.CookingTimeLabel.text = string.Format("{0}h", num);
			if (num2 > 0)
			{
				TextMeshProUGUI cookingTimeLabel = this.CookingTimeLabel;
				cookingTimeLabel.text += string.Format(" {0}m", num2);
			}
			this.IngredientQuantities = new TextMeshProUGUI[this.IngredientRects.Length];
			for (int i = 0; i < this.IngredientRects.Length; i++)
			{
				if (i < recipe.Ingredients.Count)
				{
					this.IngredientRects[i].Find("Icon").GetComponent<Image>().sprite = recipe.Ingredients[i].Item.Icon;
					this.IngredientQuantities[i] = this.IngredientRects[i].Find("Quantity").GetComponent<TextMeshProUGUI>();
					this.IngredientQuantities[i].text = recipe.Ingredients[i].Quantity.ToString() + "x";
					this.IngredientRects[i].GetComponent<ItemDefinitionInfoHoverable>().AssignedItem = recipe.Ingredients[i].Item;
					this.IngredientRects[i].gameObject.SetActive(true);
				}
				else
				{
					this.IngredientRects[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00133ECC File Offset: 0x001320CC
		public void RefreshValidity(List<ItemInstance> ingredients)
		{
			if (!this.Recipe.Unlocked)
			{
				this.IsValid = false;
				base.gameObject.SetActive(false);
				return;
			}
			this.IsValid = true;
			for (int i = 0; i < this.Recipe.Ingredients.Count; i++)
			{
				List<ItemInstance> list = new List<ItemInstance>();
				using (List<ItemDefinition>.Enumerator enumerator = this.Recipe.Ingredients[i].Items.GetEnumerator())
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
				if (num >= this.Recipe.Ingredients[i].Quantity)
				{
					this.IngredientQuantities[i].color = StationRecipeEntry.ValidColor;
				}
				else
				{
					this.IngredientQuantities[i].color = StationRecipeEntry.InvalidColor;
					this.IsValid = false;
				}
			}
			base.gameObject.SetActive(true);
			this.Button.interactable = this.IsValid;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x00134030 File Offset: 0x00132230
		public float GetIngredientsMatchDelta(List<ItemInstance> ingredients)
		{
			int num = this.Recipe.Ingredients.Sum((StationRecipe.IngredientQuantity x) => x.Quantity);
			int num2 = 0;
			for (int i = 0; i < this.Recipe.Ingredients.Count; i++)
			{
				List<ItemInstance> list = new List<ItemInstance>();
				using (List<ItemDefinition>.Enumerator enumerator = this.Recipe.Ingredients[i].Items.GetEnumerator())
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
				int num3 = 0;
				for (int j = 0; j < list.Count; j++)
				{
					num3 += list[j].Quantity;
				}
				num2 += Mathf.Min(num3, this.Recipe.Ingredients[i].Quantity);
			}
			return (float)num2 / (float)num;
		}

		// Token: 0x040035F4 RID: 13812
		public static Color ValidColor = Color.white;

		// Token: 0x040035F5 RID: 13813
		public static Color InvalidColor = new Color32(byte.MaxValue, 80, 80, byte.MaxValue);

		// Token: 0x040035F6 RID: 13814
		public Button Button;

		// Token: 0x040035F7 RID: 13815
		public Image Icon;

		// Token: 0x040035F8 RID: 13816
		public TextMeshProUGUI TitleLabel;

		// Token: 0x040035F9 RID: 13817
		public TextMeshProUGUI CookingTimeLabel;

		// Token: 0x040035FA RID: 13818
		public RectTransform[] IngredientRects;

		// Token: 0x040035FB RID: 13819
		private TextMeshProUGUI[] IngredientQuantities;
	}
}
