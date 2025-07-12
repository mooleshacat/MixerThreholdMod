using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000481 RID: 1153
	public class QualityItemLoader : ItemLoader
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x00064EBB File Offset: 0x000630BB
		public override string ItemType
		{
			get
			{
				return typeof(QualityItemData).Name;
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00064ECC File Offset: 0x000630CC
		public override ItemInstance LoadItem(string itemString)
		{
			QualityItemData qualityItemData = base.LoadData<QualityItemData>(itemString);
			if (qualityItemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (qualityItemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(qualityItemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + qualityItemData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(qualityItemData.Quality, out equality) ? equality : EQuality.Standard;
			return new QualityItemInstance(item, qualityItemData.Quantity, quality);
		}
	}
}
