using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.ProductManagerApp
{
	// Token: 0x02000AEC RID: 2796
	public class ProductAppDetailPanel : MonoBehaviour
	{
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x0013B00D File Offset: 0x0013920D
		// (set) Token: 0x06004B0C RID: 19212 RVA: 0x0013B015 File Offset: 0x00139215
		public ProductDefinition ActiveProduct { get; protected set; }

		// Token: 0x06004B0D RID: 19213 RVA: 0x0013B01E File Offset: 0x0013921E
		public void Awake()
		{
			this.ListedForSale.onValueChanged.AddListener(delegate(bool value)
			{
				this.ListingToggled();
			});
			this.ValueLabel.onEndEdit.AddListener(delegate(string value)
			{
				this.PriceSubmitted(value);
			});
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0013B058 File Offset: 0x00139258
		public void SetActiveProduct(ProductDefinition productDefinition)
		{
			this.ActiveProduct = productDefinition;
			bool flag = ProductManager.DiscoveredProducts.Contains(productDefinition);
			if (this.ActiveProduct != null)
			{
				this.NameLabel.text = productDefinition.Name;
				this.SuggestedPriceLabel.text = "Suggested: " + MoneyManager.FormatAmount(productDefinition.MarketValue, false, false);
				this.UpdatePrice();
				if (flag)
				{
					this.DescLabel.text = productDefinition.Description;
				}
				else
				{
					this.DescLabel.text = "???";
				}
				for (int i = 0; i < this.PropertyLabels.Length; i++)
				{
					if (productDefinition.Properties.Count > i)
					{
						this.PropertyLabels[i].text = "•  " + productDefinition.Properties[i].Name;
						this.PropertyLabels[i].color = productDefinition.Properties[i].LabelColor;
						this.PropertyLabels[i].gameObject.SetActive(true);
					}
					else
					{
						this.PropertyLabels[i].gameObject.SetActive(false);
					}
				}
				for (int j = 0; j < this.RecipeEntries.Length; j++)
				{
					if (productDefinition.Recipes.Count > j)
					{
						this.RecipeEntries[j].gameObject.SetActive(true);
						if (productDefinition.Recipes[j].Ingredients[0].Item is ProductDefinition)
						{
							this.RecipeEntries[j].Find("Product").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[0].Item.Icon;
							this.RecipeEntries[j].Find("Product").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[0].Item.Name;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[1].Item.Icon;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[1].Item.Name;
						}
						else
						{
							this.RecipeEntries[j].Find("Product").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[1].Item.Icon;
							this.RecipeEntries[j].Find("Product").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[1].Item.Name;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[0].Item.Icon;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[0].Item.Name;
						}
						this.RecipeEntries[j].Find("Output").GetComponent<Image>().sprite = productDefinition.Icon;
						this.RecipeEntries[j].Find("Output").GetComponent<Tooltip>().text = productDefinition.Name;
					}
					else
					{
						this.RecipeEntries[j].gameObject.SetActive(false);
					}
				}
				this.RecipesLabel.gameObject.SetActive(productDefinition.Recipes.Count > 0);
				this.NothingSelected.gameObject.SetActive(false);
				this.Container.gameObject.SetActive(true);
				this.AddictionSlider.value = productDefinition.GetAddictiveness();
				this.AddictionLabel.text = Mathf.FloorToInt(productDefinition.GetAddictiveness() * 100f).ToString() + "%";
				this.AddictionLabel.color = Color.Lerp(this.AddictionColor_Min, this.AddictionColor_Max, productDefinition.GetAddictiveness());
				ContentSizeFitter[] componentsInChildren = base.GetComponentsInChildren<ContentSizeFitter>();
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					componentsInChildren[k].enabled = false;
					componentsInChildren[k].enabled = true;
				}
				this.LayoutGroup.enabled = false;
				this.LayoutGroup.enabled = true;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.LayoutGroup.GetComponent<RectTransform>());
				this.ScrollRect.enabled = false;
				this.ScrollRect.enabled = true;
				this.ScrollRect.verticalNormalizedPosition = 1f;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.ScrollRect.GetComponent<RectTransform>());
			}
			else
			{
				this.NothingSelected.gameObject.SetActive(true);
				this.Container.gameObject.SetActive(false);
			}
			this.UpdateListed();
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0013B57F File Offset: 0x0013977F
		private void Update()
		{
			if (PlayerSingleton<ProductManagerApp>.Instance.isOpen)
			{
				this.UpdateListed();
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0013B593 File Offset: 0x00139793
		private void UpdateListed()
		{
			this.ListedForSale.SetIsOnWithoutNotify(ProductManager.ListedProducts.Contains(this.ActiveProduct));
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x0013B5B0 File Offset: 0x001397B0
		private void UpdatePrice()
		{
			this.ValueLabel.SetTextWithoutNotify(NetworkSingleton<ProductManager>.Instance.GetPrice(this.ActiveProduct).ToString());
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x0013B5E0 File Offset: 0x001397E0
		private void ListingToggled()
		{
			if (!NetworkSingleton<ProductManager>.InstanceExists)
			{
				return;
			}
			if (this.ActiveProduct == null)
			{
				return;
			}
			if (ProductManager.ListedProducts.Contains(this.ActiveProduct))
			{
				NetworkSingleton<ProductManager>.Instance.SetProductListed(this.ActiveProduct.ID, false);
			}
			else
			{
				NetworkSingleton<ProductManager>.Instance.SetProductListed(this.ActiveProduct.ID, true);
			}
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			this.UpdateListed();
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x0013B654 File Offset: 0x00139854
		private void PriceSubmitted(string value)
		{
			if (!NetworkSingleton<ProductManager>.InstanceExists)
			{
				return;
			}
			if (!PlayerSingleton<ProductManagerApp>.Instance.isOpen)
			{
				return;
			}
			if (!PlayerSingleton<Phone>.Instance.IsOpen)
			{
				return;
			}
			if (this.ActiveProduct == null)
			{
				return;
			}
			float value2;
			if (float.TryParse(value, out value2))
			{
				NetworkSingleton<ProductManager>.Instance.SendPrice(this.ActiveProduct.ID, value2);
				Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			}
			this.UpdatePrice();
		}

		// Token: 0x04003767 RID: 14183
		public Color AddictionColor_Min;

		// Token: 0x04003768 RID: 14184
		public Color AddictionColor_Max;

		// Token: 0x04003769 RID: 14185
		[Header("References")]
		public GameObject NothingSelected;

		// Token: 0x0400376A RID: 14186
		public GameObject Container;

		// Token: 0x0400376B RID: 14187
		public Text NameLabel;

		// Token: 0x0400376C RID: 14188
		public InputField ValueLabel;

		// Token: 0x0400376D RID: 14189
		public Text SuggestedPriceLabel;

		// Token: 0x0400376E RID: 14190
		public Toggle ListedForSale;

		// Token: 0x0400376F RID: 14191
		public Text DescLabel;

		// Token: 0x04003770 RID: 14192
		public Text[] PropertyLabels;

		// Token: 0x04003771 RID: 14193
		public RectTransform Listed;

		// Token: 0x04003772 RID: 14194
		public RectTransform Delisted;

		// Token: 0x04003773 RID: 14195
		public RectTransform NotDiscovered;

		// Token: 0x04003774 RID: 14196
		public RectTransform RecipesLabel;

		// Token: 0x04003775 RID: 14197
		public RectTransform[] RecipeEntries;

		// Token: 0x04003776 RID: 14198
		public VerticalLayoutGroup LayoutGroup;

		// Token: 0x04003777 RID: 14199
		public Scrollbar AddictionSlider;

		// Token: 0x04003778 RID: 14200
		public Text AddictionLabel;

		// Token: 0x04003779 RID: 14201
		public ScrollRect ScrollRect;
	}
}
