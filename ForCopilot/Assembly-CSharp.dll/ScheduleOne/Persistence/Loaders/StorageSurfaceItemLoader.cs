using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003F9 RID: 1017
	public class StorageSurfaceItemLoader : SurfaceItemLoader
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x00062BAE File Offset: 0x00060DAE
		public override string ItemType
		{
			get
			{
				return typeof(StorageSurfaceItemData).Name;
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00062BC0 File Offset: 0x00060DC0
		public override void Load(string mainPath)
		{
			SurfaceItem surfaceItem = base.LoadAndCreate(mainPath);
			if (surfaceItem == null)
			{
				Console.LogWarning("Failed to load surface item", null);
				return;
			}
			SurfaceStorageEntity surfaceStorageEntity = surfaceItem as SurfaceStorageEntity;
			if (surfaceStorageEntity == null)
			{
				Console.LogWarning("Failed to cast surface item to storage entity", null);
				return;
			}
			StorageSurfaceItemData data = base.GetData<StorageSurfaceItemData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load storage surface item data", null);
				return;
			}
			data.Contents.LoadTo(surfaceStorageEntity.StorageEntity.ItemSlots);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00062C34 File Offset: 0x00060E34
		public override void Load(DynamicSaveData data)
		{
			SurfaceItem surfaceItem = null;
			SurfaceItemData data2;
			if (data.TryExtractBaseData<SurfaceItemData>(out data2))
			{
				surfaceItem = base.LoadAndCreate(data2);
			}
			if (surfaceItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			SurfaceStorageEntity surfaceStorageEntity = surfaceItem as SurfaceStorageEntity;
			if (surfaceStorageEntity == null)
			{
				Console.LogWarning("Failed to cast surface item to storage entity", null);
				return;
			}
			StorageSurfaceItemData storageSurfaceItemData;
			if (data.TryExtractBaseData<StorageSurfaceItemData>(out storageSurfaceItemData))
			{
				storageSurfaceItemData.Contents.LoadTo(surfaceStorageEntity.StorageEntity.ItemSlots);
			}
		}
	}
}
