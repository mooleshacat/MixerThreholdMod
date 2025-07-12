using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003F6 RID: 1014
	public class ProceduralGridItemLoader : BuildableItemLoader
	{
		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x00062821 File Offset: 0x00060A21
		public override string ItemType
		{
			get
			{
				return typeof(ProceduralGridItemData).Name;
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00062832 File Offset: 0x00060A32
		public override void Load(string mainPath)
		{
			this.LoadAndCreate(mainPath);
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0006283C File Offset: 0x00060A3C
		public override void Load(DynamicSaveData data)
		{
			ProceduralGridItemData data2;
			if (data.TryExtractBaseData<ProceduralGridItemData>(out data2))
			{
				this.LoadAndCreate(data2);
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0006285C File Offset: 0x00060A5C
		protected ProceduralGridItem LoadAndCreate(string mainPath)
		{
			string text;
			if (base.TryLoadFile(mainPath, "Data", out text))
			{
				ProceduralGridItemData data = null;
				try
				{
					data = JsonUtility.FromJson<ProceduralGridItemData>(text);
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

		// Token: 0x060015F1 RID: 5617 RVA: 0x000628D0 File Offset: 0x00060AD0
		protected ProceduralGridItem LoadAndCreate(ProceduralGridItemData data)
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
			List<CoordinateProceduralTilePair> list = new List<CoordinateProceduralTilePair>();
			for (int i = 0; i < data.FootprintMatches.Length; i++)
			{
				CoordinateProceduralTilePair item = default(CoordinateProceduralTilePair);
				item.coord = new Coordinate(Mathf.RoundToInt(data.FootprintMatches[i].FootprintCoordinate.x), Mathf.RoundToInt(data.FootprintMatches[i].FootprintCoordinate.y));
				item.tileIndex = data.FootprintMatches[i].TileIndex;
				BuildableItem @object = GUIDManager.GetObject<BuildableItem>(new Guid(data.FootprintMatches[i].TileOwnerGUID));
				if (@object == null)
				{
					Debug.LogError("Failed to find tile parent for " + data.FootprintMatches[i].TileOwnerGUID);
					return null;
				}
				item.tileParent = @object.NetworkObject;
				list.Add(item);
			}
			return Singleton<BuildManager>.Instance.CreateProceduralGridItem(itemInstance, data.Rotation, list, data.GUID);
		}
	}
}
