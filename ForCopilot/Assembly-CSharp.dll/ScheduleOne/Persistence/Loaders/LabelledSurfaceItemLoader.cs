using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003E9 RID: 1001
	public class LabelledSurfaceItemLoader : SurfaceItemLoader
	{
		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x00061AD3 File Offset: 0x0005FCD3
		public override string ItemType
		{
			get
			{
				return typeof(LabelledSurfaceItemData).Name;
			}
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00061AEC File Offset: 0x0005FCEC
		public override void Load(string mainPath)
		{
			SurfaceItem surfaceItem = base.LoadAndCreate(mainPath);
			if (surfaceItem == null)
			{
				Console.LogWarning("Failed to load surface item", null);
				return;
			}
			LabelledSurfaceItemData data = base.GetData<LabelledSurfaceItemData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load LabelledSurfaceItemData", null);
				return;
			}
			LabelledSurfaceItem labelledSurfaceItem = surfaceItem as LabelledSurfaceItem;
			if (labelledSurfaceItem != null)
			{
				labelledSurfaceItem.SetMessage(null, data.Message);
			}
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00061B4C File Offset: 0x0005FD4C
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
			LabelledSurfaceItemData labelledSurfaceItemData;
			if (data.TryExtractBaseData<LabelledSurfaceItemData>(out labelledSurfaceItemData))
			{
				LabelledSurfaceItem labelledSurfaceItem = surfaceItem as LabelledSurfaceItem;
				if (labelledSurfaceItem != null)
				{
					labelledSurfaceItem.SetMessage(null, labelledSurfaceItemData.Message);
				}
			}
		}
	}
}
