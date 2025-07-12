using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003F8 RID: 1016
	public class StorageRackLoader : GridItemLoader
	{
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x00062AB4 File Offset: 0x00060CB4
		public override string ItemType
		{
			get
			{
				return typeof(PlaceableStorageData).Name;
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00062AC8 File Offset: 0x00060CC8
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			PlaceableStorageEntity placeableStorageEntity = gridItem as PlaceableStorageEntity;
			if (placeableStorageEntity == null)
			{
				Console.LogWarning("Failed to cast grid item to rack", null);
				return;
			}
			PlaceableStorageData data = base.GetData<PlaceableStorageData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load storage rack data", null);
				return;
			}
			data.Contents.LoadTo(placeableStorageEntity.StorageEntity.ItemSlots);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00062B3C File Offset: 0x00060D3C
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
			PlaceableStorageEntity placeableStorageEntity = gridItem as PlaceableStorageEntity;
			if (placeableStorageEntity == null)
			{
				Console.LogWarning("Failed to cast grid item to rack", null);
				return;
			}
			PlaceableStorageData placeableStorageData;
			if (data.TryExtractBaseData<PlaceableStorageData>(out placeableStorageData))
			{
				placeableStorageData.Contents.LoadTo(placeableStorageEntity.StorageEntity.ItemSlots);
			}
		}
	}
}
