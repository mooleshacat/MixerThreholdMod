using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200047D RID: 1149
	public class IntegerItemLoader : ItemLoader
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00064B57 File Offset: 0x00062D57
		public override string ItemType
		{
			get
			{
				return typeof(IntegerItemData).Name;
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00064B68 File Offset: 0x00062D68
		public override ItemInstance LoadItem(string itemString)
		{
			IntegerItemData integerItemData = base.LoadData<IntegerItemData>(itemString);
			if (integerItemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (integerItemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(integerItemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + integerItemData.ID, null);
				return null;
			}
			return new IntegerItemInstance(item, integerItemData.Quantity, integerItemData.Value);
		}
	}
}
