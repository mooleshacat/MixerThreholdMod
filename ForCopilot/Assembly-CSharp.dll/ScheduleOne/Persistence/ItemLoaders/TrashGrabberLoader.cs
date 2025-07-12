using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000482 RID: 1154
	public class TrashGrabberLoader : ItemLoader
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x00064F59 File Offset: 0x00063159
		public override string ItemType
		{
			get
			{
				return typeof(TrashGrabberData).Name;
			}
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00064F6C File Offset: 0x0006316C
		public override ItemInstance LoadItem(string itemString)
		{
			TrashGrabberData trashGrabberData = base.LoadData<TrashGrabberData>(itemString);
			if (trashGrabberData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (trashGrabberData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(trashGrabberData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + trashGrabberData.ID, null);
				return null;
			}
			TrashGrabberInstance trashGrabberInstance = new TrashGrabberInstance(item, trashGrabberData.Quantity);
			trashGrabberInstance.LoadContentData(trashGrabberData.Content);
			return trashGrabberInstance;
		}
	}
}
