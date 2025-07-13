using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000461 RID: 1121
	[Serializable]
	public class ProductManagerData : SaveData
	{
		// Token: 0x06001699 RID: 5785 RVA: 0x000644C8 File Offset: 0x000626C8
		public ProductManagerData(string[] discoveredProducts, string[] listedProducts, NewMixOperation activeOperation, bool isMixComplete, MixRecipeData[] mixRecipes, StringIntPair[] productPrices, string[] favouritedProducts, WeedProductData[] createdWeed, MethProductData[] createdMeth, CocaineProductData[] createdCocaine)
		{
			this.DiscoveredProducts = discoveredProducts;
			this.ListedProducts = listedProducts;
			this.ActiveMixOperation = activeOperation;
			this.IsMixComplete = isMixComplete;
			this.MixRecipes = mixRecipes;
			this.ProductPrices = productPrices;
			this.FavouritedProducts = favouritedProducts;
			this.CreatedWeed = createdWeed;
			this.CreatedMeth = createdMeth;
			this.CreatedCocaine = createdCocaine;
		}

		// Token: 0x040014BF RID: 5311
		public string[] DiscoveredProducts;

		// Token: 0x040014C0 RID: 5312
		public string[] ListedProducts;

		// Token: 0x040014C1 RID: 5313
		public NewMixOperation ActiveMixOperation;

		// Token: 0x040014C2 RID: 5314
		public bool IsMixComplete;

		// Token: 0x040014C3 RID: 5315
		public MixRecipeData[] MixRecipes;

		// Token: 0x040014C4 RID: 5316
		public StringIntPair[] ProductPrices;

		// Token: 0x040014C5 RID: 5317
		public string[] FavouritedProducts;

		// Token: 0x040014C6 RID: 5318
		public WeedProductData[] CreatedWeed;

		// Token: 0x040014C7 RID: 5319
		public MethProductData[] CreatedMeth;

		// Token: 0x040014C8 RID: 5320
		public CocaineProductData[] CreatedCocaine;
	}
}
