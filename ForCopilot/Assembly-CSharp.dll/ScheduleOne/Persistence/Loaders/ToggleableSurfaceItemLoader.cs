using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003FC RID: 1020
	public class ToggleableSurfaceItemLoader : SurfaceItemLoader
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00062E9B File Offset: 0x0006109B
		public override string ItemType
		{
			get
			{
				return typeof(ToggleableSurfaceItemData).Name;
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00062EAC File Offset: 0x000610AC
		public override void Load(string mainPath)
		{
			SurfaceItem surfaceItem = base.LoadAndCreate(mainPath);
			if (surfaceItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			ToggleableSurfaceItemData data = base.GetData<ToggleableSurfaceItemData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load ToggleableSurfaceItemData", null);
				return;
			}
			ToggleableSurfaceItem toggleableSurfaceItem = surfaceItem as ToggleableSurfaceItem;
			if (toggleableSurfaceItem != null && data.IsOn)
			{
				toggleableSurfaceItem.TurnOn(true);
			}
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00062F0C File Offset: 0x0006110C
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
			ToggleableSurfaceItemData toggleableSurfaceItemData;
			if (data.TryExtractBaseData<ToggleableSurfaceItemData>(out toggleableSurfaceItemData))
			{
				ToggleableSurfaceItem toggleableSurfaceItem = surfaceItem as ToggleableSurfaceItem;
				if (toggleableSurfaceItem != null && toggleableSurfaceItemData.IsOn)
				{
					toggleableSurfaceItem.TurnOn(true);
				}
			}
		}
	}
}
