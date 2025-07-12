using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003FB RID: 1019
	public class ToggleableItemLoader : GridItemLoader
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x00062DCB File Offset: 0x00060FCB
		public override string ItemType
		{
			get
			{
				return typeof(ToggleableItemData).Name;
			}
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00062DDC File Offset: 0x00060FDC
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			ToggleableItemData data = base.GetData<ToggleableItemData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load toggleableitem data", null);
				return;
			}
			ToggleableItem toggleableItem = gridItem as ToggleableItem;
			if (toggleableItem != null && data.IsOn)
			{
				toggleableItem.TurnOn(true);
			}
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00062E3C File Offset: 0x0006103C
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
			ToggleableItemData toggleableItemData;
			if (data.TryExtractBaseData<ToggleableItemData>(out toggleableItemData))
			{
				ToggleableItem toggleableItem = gridItem as ToggleableItem;
				if (toggleableItem != null && toggleableItemData.IsOn)
				{
					toggleableItem.TurnOn(true);
				}
			}
		}
	}
}
