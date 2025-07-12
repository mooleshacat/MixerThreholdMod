using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003FD RID: 1021
	public class TrashContainerLoader : GridItemLoader
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x00062F6B File Offset: 0x0006116B
		public override string ItemType
		{
			get
			{
				return typeof(TrashContainerData).Name;
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00062F7C File Offset: 0x0006117C
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			TrashContainerData data = base.GetData<TrashContainerData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load toggleableitem data", null);
				return;
			}
			TrashContainerItem trashContainerItem = gridItem as TrashContainerItem;
			if (trashContainerItem != null)
			{
				trashContainerItem.Container.Content.LoadFromData(data.ContentData);
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00062FE4 File Offset: 0x000611E4
		public override void Load(DynamicSaveData data)
		{
			GridItem gridItem = null;
			GridItemData data2;
			if (data.TryExtractBaseData<GridItemData>(out data2))
			{
				gridItem = base.LoadAndCreate(data2);
			}
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			TrashContainerItem trashContainerItem = gridItem as TrashContainerItem;
			if (trashContainerItem == null)
			{
				Console.LogWarning("Failed to load trash container item", null);
				return;
			}
			TrashContainerData trashContainerData;
			if (data.TryExtractBaseData<TrashContainerData>(out trashContainerData))
			{
				trashContainerItem.Container.Content.LoadFromData(trashContainerData.ContentData);
			}
		}
	}
}
