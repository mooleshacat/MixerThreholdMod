using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000480 RID: 1152
	public class ProductItemLoader : ItemLoader
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060016CC RID: 5836 RVA: 0x00064DE7 File Offset: 0x00062FE7
		public override string ItemType
		{
			get
			{
				return typeof(ProductItemData).Name;
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00064DF8 File Offset: 0x00062FF8
		public override ItemInstance LoadItem(string itemString)
		{
			ProductItemData productItemData = base.LoadData<ProductItemData>(itemString);
			if (productItemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (productItemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(productItemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + productItemData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(productItemData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (productItemData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(productItemData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new ProductItemInstance(item, productItemData.Quantity, quality, packaging);
		}
	}
}
