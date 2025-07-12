using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using UnityEngine;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000BB1 RID: 2993
	[Serializable]
	public class ShopListing
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool IsInStock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004F88 RID: 20360 RVA: 0x0014FB54 File Offset: 0x0014DD54
		public float Price
		{
			get
			{
				if (!this.OverridePrice)
				{
					return this.Item.BasePurchasePrice;
				}
				return this.OverriddenPrice;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06004F89 RID: 20361 RVA: 0x0014FB70 File Offset: 0x0014DD70
		public bool IsUnlimitedStock
		{
			get
			{
				return !this.LimitedStock;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004F8A RID: 20362 RVA: 0x0014FB7B File Offset: 0x0014DD7B
		// (set) Token: 0x06004F8B RID: 20363 RVA: 0x0014FB83 File Offset: 0x0014DD83
		public ShopInterface Shop { get; private set; }

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06004F8C RID: 20364 RVA: 0x0014FB8C File Offset: 0x0014DD8C
		// (set) Token: 0x06004F8D RID: 20365 RVA: 0x0014FB94 File Offset: 0x0014DD94
		public int CurrentStock { get; protected set; }

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x0014FB9D File Offset: 0x0014DD9D
		// (set) Token: 0x06004F8F RID: 20367 RVA: 0x0014FBA5 File Offset: 0x0014DDA5
		public int QuantityInCart { get; private set; }

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06004F90 RID: 20368 RVA: 0x0014FBAE File Offset: 0x0014DDAE
		public int CurrentStockMinusCart
		{
			get
			{
				return this.CurrentStock - this.QuantityInCart;
			}
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x0014FBBD File Offset: 0x0014DDBD
		public void Initialize(ShopInterface shop)
		{
			this.Shop = shop;
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x0014FBC6 File Offset: 0x0014DDC6
		public void Restock(bool network)
		{
			this.SetStock(this.DefaultStock, true);
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x0014FBD5 File Offset: 0x0014DDD5
		public void RemoveStock(int quantity)
		{
			this.SetStock(this.CurrentStock - quantity, true);
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x0014FBE8 File Offset: 0x0014DDE8
		public void SetStock(int quantity, bool network = true)
		{
			if (this.IsUnlimitedStock)
			{
				return;
			}
			if (network && NetworkSingleton<ShopManager>.InstanceExists && this.Shop != null)
			{
				NetworkSingleton<ShopManager>.Instance.SendStock(this.Shop.ShopCode, this.Item.ID, quantity);
			}
			this.CurrentStock = quantity;
			if (this.CurrentStock < 0)
			{
				this.CurrentStock = 0;
			}
			if (this.onStockChanged != null)
			{
				this.onStockChanged();
			}
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x0014FC61 File Offset: 0x0014DE61
		public virtual bool ShouldShow()
		{
			return !this.EnforceMinimumGameCreationVersion || SaveManager.GetVersionNumber(Singleton<MetadataManager>.Instance.CreationVersion) >= this.MinimumGameCreationVersion;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x0014FC88 File Offset: 0x0014DE88
		public virtual bool DoesListingMatchCategoryFilter(EShopCategory category)
		{
			return category == EShopCategory.All || this.Item.ShopCategories.Find((ShopListing.CategoryInstance x) => x.Category == category) != null;
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x0014FCCB File Offset: 0x0014DECB
		public virtual bool DoesListingMatchSearchTerm(string searchTerm)
		{
			return this.Item.Name.ToLower().Contains(searchTerm.ToLower());
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x0014FCE8 File Offset: 0x0014DEE8
		public void SetQuantityInCart(int quantity)
		{
			this.QuantityInCart = quantity;
			if (this.onStockChanged != null)
			{
				this.onStockChanged();
			}
		}

		// Token: 0x04003B99 RID: 15257
		public string name;

		// Token: 0x04003B9A RID: 15258
		public StorableItemDefinition Item;

		// Token: 0x04003B9B RID: 15259
		[Header("Pricing")]
		[SerializeField]
		protected bool OverridePrice;

		// Token: 0x04003B9C RID: 15260
		[SerializeField]
		protected float OverriddenPrice = 10f;

		// Token: 0x04003B9D RID: 15261
		[Header("Stock")]
		public bool LimitedStock;

		// Token: 0x04003B9E RID: 15262
		public int DefaultStock = -1;

		// Token: 0x04003B9F RID: 15263
		public ShopListing.ERestockRate RestockRate;

		// Token: 0x04003BA0 RID: 15264
		[Header("Settings")]
		public bool EnforceMinimumGameCreationVersion;

		// Token: 0x04003BA1 RID: 15265
		public float MinimumGameCreationVersion = 27f;

		// Token: 0x04003BA2 RID: 15266
		public bool CanBeDelivered;

		// Token: 0x04003BA3 RID: 15267
		[Header("Color")]
		public bool UseIconTint;

		// Token: 0x04003BA4 RID: 15268
		public Color IconTint = Color.white;

		// Token: 0x04003BA8 RID: 15272
		public Action onStockChanged;

		// Token: 0x02000BB2 RID: 2994
		[Serializable]
		public class CategoryInstance
		{
			// Token: 0x04003BA9 RID: 15273
			public EShopCategory Category;
		}

		// Token: 0x02000BB3 RID: 2995
		public enum ERestockRate
		{
			// Token: 0x04003BAB RID: 15275
			Daily,
			// Token: 0x04003BAC RID: 15276
			Weekly,
			// Token: 0x04003BAD RID: 15277
			Never
		}
	}
}
