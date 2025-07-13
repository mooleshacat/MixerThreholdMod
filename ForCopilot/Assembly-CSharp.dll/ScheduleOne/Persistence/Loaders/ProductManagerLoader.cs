using System;
using System.IO;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003AB RID: 939
	public class ProductManagerLoader : Loader
	{
		// Token: 0x0600152A RID: 5418 RVA: 0x0005DAD8 File Offset: 0x0005BCD8
		public override void Load(string mainPath)
		{
			string text;
			bool flag = base.TryLoadFile(mainPath, out text, true);
			if (!flag)
			{
				base.TryLoadFile(Path.Combine(mainPath, "Products"), out text, true);
			}
			string text2 = Path.Combine(mainPath, "CreatedProducts");
			if (Directory.Exists(text2) && !flag)
			{
				Console.LogWarning("Loading legacy product data from " + text2, null);
				WeedProductLoader weedProductLoader = new WeedProductLoader();
				MethProductLoader methProductLoader = new MethProductLoader();
				CocaineProductLoader cocaineProductLoader = new CocaineProductLoader();
				string[] files = Directory.GetFiles(text2);
				for (int i = 0; i < files.Length; i++)
				{
					string text3;
					if (base.TryLoadFile(files[i], out text3, false))
					{
						ProductData productData = null;
						try
						{
							productData = JsonUtility.FromJson<ProductData>(text3);
						}
						catch (Exception ex)
						{
							Debug.LogError("Error loading product data: " + ex.Message);
						}
						if (productData != null)
						{
							bool flag2 = false;
							if (string.IsNullOrEmpty(productData.Name))
							{
								Console.LogWarning("Product name is empty; generating random name", null);
								if (Singleton<NewMixScreen>.InstanceExists)
								{
									productData.Name = Singleton<NewMixScreen>.Instance.GenerateUniqueName(null, EDrugType.Marijuana);
								}
								else
								{
									productData.Name = "Product " + UnityEngine.Random.Range(0, 1000).ToString();
								}
								flag2 = true;
							}
							if (string.IsNullOrEmpty(productData.ID))
							{
								Console.LogWarning("Product ID is empty; generating from name", null);
								productData.ID = ProductManager.MakeIDFileSafe(productData.Name);
								flag2 = true;
							}
							if (flag2)
							{
								try
								{
									File.WriteAllText(files[i], productData.GetJson(true));
								}
								catch (Exception ex2)
								{
									Console.LogError("Error saving modified product data: " + ex2.Message, null);
								}
							}
							switch (productData.DrugType)
							{
							case EDrugType.Marijuana:
								weedProductLoader.Load(files[i]);
								break;
							case EDrugType.Methamphetamine:
								methProductLoader.Load(files[i]);
								break;
							case EDrugType.Cocaine:
								cocaineProductLoader.Load(files[i]);
								break;
							default:
								Console.LogError("Unknown drug type: " + productData.DrugType.ToString(), null);
								break;
							}
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				ProductManagerData productManagerData = JsonUtility.FromJson<ProductManagerData>(text);
				if (productManagerData != null)
				{
					this.LoadProducts(productManagerData);
					if (productManagerData.DiscoveredProducts != null)
					{
						for (int j = 0; j < productManagerData.DiscoveredProducts.Length; j++)
						{
							if (productManagerData.DiscoveredProducts[j] != null)
							{
								NetworkSingleton<ProductManager>.Instance.SetProductDiscovered(null, productManagerData.DiscoveredProducts[j], false);
							}
						}
					}
					if (productManagerData.ListedProducts != null)
					{
						for (int k = 0; k < productManagerData.ListedProducts.Length; k++)
						{
							if (productManagerData.ListedProducts[k] != null)
							{
								NetworkSingleton<ProductManager>.Instance.SetProductListed(null, productManagerData.ListedProducts[k], true);
							}
						}
					}
					if (productManagerData.FavouritedProducts != null)
					{
						for (int l = 0; l < productManagerData.FavouritedProducts.Length; l++)
						{
							if (productManagerData.FavouritedProducts[l] != null)
							{
								NetworkSingleton<ProductManager>.Instance.SetProductFavourited(null, productManagerData.FavouritedProducts[l], true);
							}
						}
					}
					if (productManagerData.ActiveMixOperation != null && productManagerData.ActiveMixOperation.ProductID != string.Empty)
					{
						NetworkSingleton<ProductManager>.Instance.SendMixOperation(productManagerData.ActiveMixOperation, productManagerData.IsMixComplete);
					}
					if (productManagerData.MixRecipes != null)
					{
						for (int m = 0; m < productManagerData.MixRecipes.Length; m++)
						{
							if (productManagerData.MixRecipes[m] != null)
							{
								try
								{
									MixRecipeData mixRecipeData = productManagerData.MixRecipes[m];
									NetworkSingleton<ProductManager>.Instance.CreateMixRecipe(null, mixRecipeData.Product, mixRecipeData.Mixer, mixRecipeData.Output);
								}
								catch (Exception ex3)
								{
									Console.LogError("Error loading mix recipe: " + ex3.Message, null);
								}
							}
						}
					}
					if (productManagerData.ProductPrices != null)
					{
						for (int n = 0; n < productManagerData.ProductPrices.Length; n++)
						{
							if (productManagerData.ProductPrices[n] != null)
							{
								StringIntPair stringIntPair = productManagerData.ProductPrices[n];
								ProductDefinition item = Registry.GetItem<ProductDefinition>(stringIntPair.String);
								if (item != null)
								{
									NetworkSingleton<ProductManager>.Instance.SetPrice(null, item.ID, (float)stringIntPair.Int);
								}
							}
						}
						return;
					}
				}
			}
			else
			{
				Console.LogWarning("Did not find product data file in " + mainPath, null);
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0005DF18 File Offset: 0x0005C118
		private void SanitizeProductData(ProductData data)
		{
			if (data == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(data.Name))
			{
				Console.LogWarning("Product name is empty; generating random name", null);
				if (Singleton<NewMixScreen>.InstanceExists)
				{
					data.Name = Singleton<NewMixScreen>.Instance.GenerateUniqueName(null, EDrugType.Marijuana);
				}
				else
				{
					data.Name = "Product " + UnityEngine.Random.Range(0, 1000).ToString();
				}
			}
			if (string.IsNullOrEmpty(data.ID))
			{
				Console.LogWarning("Product ID is empty; generating from name", null);
				data.ID = ProductManager.MakeIDFileSafe(data.Name);
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0005DFA8 File Offset: 0x0005C1A8
		private void LoadProducts(ProductManagerData productData)
		{
			if (productData == null)
			{
				return;
			}
			if (productData.CreatedWeed != null)
			{
				foreach (WeedProductData weedProductData in productData.CreatedWeed)
				{
					if (weedProductData != null)
					{
						this.SanitizeProductData(weedProductData);
						NetworkSingleton<ProductManager>.Instance.CreateWeed_Server(weedProductData.Name, weedProductData.ID, weedProductData.DrugType, weedProductData.Properties.ToList<string>(), weedProductData.AppearanceSettings);
					}
				}
			}
			if (productData.CreatedMeth != null)
			{
				foreach (MethProductData methProductData in productData.CreatedMeth)
				{
					if (methProductData != null)
					{
						this.SanitizeProductData(methProductData);
						NetworkSingleton<ProductManager>.Instance.CreateMeth_Server(methProductData.Name, methProductData.ID, methProductData.DrugType, methProductData.Properties.ToList<string>(), methProductData.AppearanceSettings);
					}
				}
			}
			if (productData.CreatedCocaine != null)
			{
				foreach (CocaineProductData cocaineProductData in productData.CreatedCocaine)
				{
					if (cocaineProductData != null)
					{
						this.SanitizeProductData(cocaineProductData);
						NetworkSingleton<ProductManager>.Instance.CreateCocaine_Server(cocaineProductData.Name, cocaineProductData.ID, cocaineProductData.DrugType, cocaineProductData.Properties.ToList<string>(), cocaineProductData.AppearanceSettings);
					}
				}
			}
		}
	}
}
