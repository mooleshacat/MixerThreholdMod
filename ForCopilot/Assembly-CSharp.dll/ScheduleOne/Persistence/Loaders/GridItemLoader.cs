using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003E7 RID: 999
	public class GridItemLoader : BuildableItemLoader
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x000618AA File Offset: 0x0005FAAA
		public override string ItemType
		{
			get
			{
				return typeof(GridItemData).Name;
			}
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000618C3 File Offset: 0x0005FAC3
		public override void Load(string mainPath)
		{
			this.LoadAndCreate(mainPath);
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x000618D0 File Offset: 0x0005FAD0
		public override void Load(DynamicSaveData data)
		{
			GridItemData data2;
			if (data.TryExtractBaseData<GridItemData>(out data2))
			{
				this.LoadAndCreate(data2);
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x000618F0 File Offset: 0x0005FAF0
		protected GridItem LoadAndCreate(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "Data", out text))
			{
				GridItemData data = null;
				try
				{
					data = JsonUtility.FromJson<GridItemData>(text);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				return this.LoadAndCreate(data);
			}
			return null;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00061964 File Offset: 0x0005FB64
		protected GridItem LoadAndCreate(GridItemData data)
		{
			if (data == null)
			{
				return null;
			}
			ItemInstance itemInstance = ItemDeserializer.LoadItem(data.ItemString);
			if (itemInstance == null)
			{
				return null;
			}
			Grid @object = GUIDManager.GetObject<Grid>(new Guid(data.GridGUID));
			if (@object == null)
			{
				Console.LogWarning("Failed to find grid for " + data.GridGUID, null);
				return null;
			}
			return Singleton<BuildManager>.Instance.CreateGridItem(itemInstance, @object, data.OriginCoordinate, data.Rotation, data.GUID);
		}
	}
}
