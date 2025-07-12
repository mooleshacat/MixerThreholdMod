using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003FA RID: 1018
	public class SurfaceItemLoader : BuildableItemLoader
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x00062CA6 File Offset: 0x00060EA6
		public override string ItemType
		{
			get
			{
				return typeof(SurfaceItemData).Name;
			}
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00062CB7 File Offset: 0x00060EB7
		public override void Load(string mainPath)
		{
			this.LoadAndCreate(mainPath);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00062CC4 File Offset: 0x00060EC4
		public override void Load(DynamicSaveData data)
		{
			SurfaceItemData data2;
			if (data.TryExtractBaseData<SurfaceItemData>(out data2))
			{
				this.LoadAndCreate(data2);
			}
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x00062CE4 File Offset: 0x00060EE4
		protected SurfaceItem LoadAndCreate(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "Data", out text))
			{
				SurfaceItemData data = null;
				try
				{
					data = JsonUtility.FromJson<SurfaceItemData>(text);
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

		// Token: 0x06001603 RID: 5635 RVA: 0x00062D58 File Offset: 0x00060F58
		protected SurfaceItem LoadAndCreate(SurfaceItemData data)
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
			Surface @object = GUIDManager.GetObject<Surface>(new Guid(data.ParentSurfaceGUID));
			if (@object == null)
			{
				Console.LogWarning("Failed to find parent surface for " + data.ParentSurfaceGUID, null);
				return null;
			}
			return Singleton<BuildManager>.Instance.CreateSurfaceItem(itemInstance, @object, data.RelativePosition, data.RelativeRotation, data.GUID);
		}
	}
}
