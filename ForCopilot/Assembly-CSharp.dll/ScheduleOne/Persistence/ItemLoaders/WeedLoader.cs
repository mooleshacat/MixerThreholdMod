using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000484 RID: 1156
	public class WeedLoader : ItemLoader
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x00065082 File Offset: 0x00063282
		public override string ItemType
		{
			get
			{
				return typeof(WeedData).Name;
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00065094 File Offset: 0x00063294
		public override ItemInstance LoadItem(string itemString)
		{
			WeedData weedData = base.LoadData<WeedData>(itemString);
			if (weedData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (weedData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(weedData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + weedData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(weedData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (weedData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(weedData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new WeedInstance(item, weedData.Quantity, quality, packaging);
		}
	}
}
