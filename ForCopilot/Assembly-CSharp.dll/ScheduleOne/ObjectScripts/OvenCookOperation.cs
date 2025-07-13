using System;
using FishNet.Serializing.Helping;
using ScheduleOne.ItemFramework;
using ScheduleOne.StationFramework;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C35 RID: 3125
	[Serializable]
	public class OvenCookOperation
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600567D RID: 22141 RVA: 0x0016DD29 File Offset: 0x0016BF29
		[CodegenExclude]
		public StorableItemDefinition Ingredient
		{
			get
			{
				if (this._itemDefinition == null)
				{
					this._itemDefinition = (Registry.GetItem(this.IngredientID) as StorableItemDefinition);
				}
				return this._itemDefinition;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600567E RID: 22142 RVA: 0x0016DD55 File Offset: 0x0016BF55
		[CodegenExclude]
		public StorableItemDefinition Product
		{
			get
			{
				if (this._productionDefinition == null)
				{
					this._productionDefinition = (Registry.GetItem(this.ProductID) as StorableItemDefinition);
				}
				return this._productionDefinition;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600567F RID: 22143 RVA: 0x0016DD81 File Offset: 0x0016BF81
		[CodegenExclude]
		public CookableModule Cookable
		{
			get
			{
				if (this._cookable == null)
				{
					this._cookable = this.Ingredient.StationItem.GetModule<CookableModule>();
				}
				return this._cookable;
			}
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x0016DDAD File Offset: 0x0016BFAD
		public OvenCookOperation(string ingredientID, EQuality ingredientQuality, int ingredientQuantity, string productID)
		{
			this.IngredientID = ingredientID;
			this.IngredientQuality = ingredientQuality;
			this.IngredientQuantity = ingredientQuantity;
			this.ProductID = productID;
			this.CookProgress = 0;
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x0016DDE7 File Offset: 0x0016BFE7
		public OvenCookOperation(string ingredientID, EQuality ingredientQuality, int ingredientQuantity, string productID, int progress)
		{
			this.IngredientID = ingredientID;
			this.IngredientQuality = ingredientQuality;
			this.IngredientQuantity = ingredientQuantity;
			this.ProductID = productID;
			this.CookProgress = progress;
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x0016DE22 File Offset: 0x0016C022
		public OvenCookOperation()
		{
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x0016DE38 File Offset: 0x0016C038
		public void UpdateCookProgress(int change)
		{
			this.CookProgress += change;
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x0016DE48 File Offset: 0x0016C048
		public int GetCookDuration()
		{
			if (this.cookDuration == -1)
			{
				this.cookDuration = this.Ingredient.StationItem.GetModule<CookableModule>().CookTime;
			}
			return this.cookDuration;
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x0016DE74 File Offset: 0x0016C074
		public ItemInstance GetProductItem(int quantity)
		{
			ItemInstance defaultInstance = this.Product.GetDefaultInstance(quantity);
			if (defaultInstance is QualityItemInstance)
			{
				(defaultInstance as QualityItemInstance).Quality = this.IngredientQuality;
			}
			return defaultInstance;
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x0016DEA8 File Offset: 0x0016C0A8
		public bool IsReady()
		{
			return this.CookProgress >= this.GetCookDuration();
		}

		// Token: 0x04003FFD RID: 16381
		[CodegenExclude]
		private StorableItemDefinition _itemDefinition;

		// Token: 0x04003FFE RID: 16382
		[CodegenExclude]
		private StorableItemDefinition _productionDefinition;

		// Token: 0x04003FFF RID: 16383
		[CodegenExclude]
		private CookableModule _cookable;

		// Token: 0x04004000 RID: 16384
		public string IngredientID;

		// Token: 0x04004001 RID: 16385
		public EQuality IngredientQuality;

		// Token: 0x04004002 RID: 16386
		public int IngredientQuantity = 1;

		// Token: 0x04004003 RID: 16387
		public string ProductID;

		// Token: 0x04004004 RID: 16388
		public int CookProgress;

		// Token: 0x04004005 RID: 16389
		[CodegenExclude]
		private int cookDuration = -1;
	}
}
