using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200047F RID: 1151
	public class MethLoader : ItemLoader
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00064D10 File Offset: 0x00062F10
		public override string ItemType
		{
			get
			{
				return typeof(MethData).Name;
			}
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00064D24 File Offset: 0x00062F24
		public override ItemInstance LoadItem(string itemString)
		{
			MethData methData = base.LoadData<MethData>(itemString);
			if (methData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (methData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(methData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + methData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(methData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (methData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(methData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new MethInstance(item, methData.Quantity, quality, packaging);
		}
	}
}
