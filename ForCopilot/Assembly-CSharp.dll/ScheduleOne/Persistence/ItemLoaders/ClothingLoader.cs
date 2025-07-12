using System;
using ScheduleOne.Clothing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200047B RID: 1147
	public class ClothingLoader : ItemLoader
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x000649F1 File Offset: 0x00062BF1
		public override string ItemType
		{
			get
			{
				return typeof(ClothingData).Name;
			}
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00064A04 File Offset: 0x00062C04
		public override ItemInstance LoadItem(string itemString)
		{
			ClothingData clothingData = base.LoadData<ClothingData>(itemString);
			if (clothingData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (clothingData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(clothingData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + clothingData.ID, null);
				return null;
			}
			return new ClothingInstance(item, clothingData.Quantity, clothingData.Color);
		}
	}
}
