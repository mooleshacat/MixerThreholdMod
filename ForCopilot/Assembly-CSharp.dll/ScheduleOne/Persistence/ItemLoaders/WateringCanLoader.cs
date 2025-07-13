using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000483 RID: 1155
	public class WateringCanLoader : ItemLoader
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060016D5 RID: 5845 RVA: 0x00064FF0 File Offset: 0x000631F0
		public override string ItemType
		{
			get
			{
				return typeof(WateringCanData).Name;
			}
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00065004 File Offset: 0x00063204
		public override ItemInstance LoadItem(string itemString)
		{
			WateringCanData wateringCanData = base.LoadData<WateringCanData>(itemString);
			if (wateringCanData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (wateringCanData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(wateringCanData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + wateringCanData.ID, null);
				return null;
			}
			return new WateringCanInstance(item, wateringCanData.Quantity, wateringCanData.CurrentFillAmount);
		}
	}
}
