using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Employees;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008DD RID: 2269
	public interface IStorageEntity
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003D04 RID: 15620
		Transform storedItemContainer { get; }

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06003D05 RID: 15621
		Dictionary<StoredItem, Employee> reservedItems { get; }

		// Token: 0x06003D06 RID: 15622
		List<StoredItem> GetStoredItems();

		// Token: 0x06003D07 RID: 15623
		List<StorageGrid> GetStorageGrids();

		// Token: 0x06003D08 RID: 15624 RVA: 0x00100F3C File Offset: 0x000FF13C
		List<StoredItem> GetStoredItemsByID(string ID)
		{
			List<StoredItem> storedItems = this.GetStoredItems();
			List<StoredItem> list = new List<StoredItem>();
			for (int i = 0; i < storedItems.Count; i++)
			{
				if (storedItems[i].item.ID == ID)
				{
					list.Add(storedItems[i]);
				}
			}
			return list;
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x00100F90 File Offset: 0x000FF190
		void ReserveItem(StoredItem item, Employee employee)
		{
			if (this.IsItemReserved(item))
			{
				if (this.reservedItems[item] != employee)
				{
					Console.LogWarning("Item already reserved by someone else!", null);
				}
				return;
			}
			this.reservedItems.Add(item, employee);
			(this as MonoBehaviour).StartCoroutine(this.ClearReserve(item));
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00100FE6 File Offset: 0x000FF1E6
		void DereserveItem(StoredItem item)
		{
			if (this.reservedItems.ContainsKey(item))
			{
				this.reservedItems.Remove(item);
			}
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00101003 File Offset: 0x000FF203
		bool IsItemReserved(StoredItem item)
		{
			return this.reservedItems.ContainsKey(item);
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00101011 File Offset: 0x000FF211
		Employee WhoIsReserving(StoredItem item)
		{
			if (this.reservedItems.ContainsKey(item))
			{
				return this.reservedItems[item];
			}
			return null;
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00101030 File Offset: 0x000FF230
		List<StoredItem> GetNonReservedItemsByPrefabID(string prefabID, Employee whosAskin)
		{
			List<StoredItem> storedItemsByID = this.GetStoredItemsByID(prefabID);
			List<StoredItem> list = new List<StoredItem>();
			for (int i = 0; i < storedItemsByID.Count; i++)
			{
				Employee x = this.WhoIsReserving(storedItemsByID[i]);
				if (x == null || x == whosAskin)
				{
					list.Add(storedItemsByID[i]);
				}
			}
			return list;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x0010108A File Offset: 0x000FF28A
		IEnumerator ClearReserve(StoredItem item)
		{
			yield return new WaitForSeconds(60f);
			if (item != null)
			{
				this.DereserveItem(item);
			}
			yield break;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x001010A0 File Offset: 0x000FF2A0
		bool TryFitItem(int sizeX, int sizeY, out StorageGrid grid, out Coordinate originCoordinate, out float rotation)
		{
			grid = null;
			originCoordinate = new Coordinate(0, 0);
			rotation = 0f;
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			for (int i = 0; i < storageGrids.Count; i++)
			{
				grid = storageGrids[i];
				if (storageGrids[i].TryFitItem(sizeX, sizeY, new List<Coordinate>(), out originCoordinate, out rotation))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x00101100 File Offset: 0x000FF300
		int HowManyCanFit(int sizeX, int sizeY, int limit = 2147483647)
		{
			int num = 0;
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			for (int i = 0; i < storageGrids.Count; i++)
			{
				List<Coordinate> list = new List<Coordinate>();
				Coordinate originCoord;
				float rot;
				while (storageGrids[i].TryFitItem(sizeX, sizeY, list, out originCoord, out rot) && num < limit)
				{
					num++;
					List<CoordinatePair> list2 = Coordinate.BuildCoordinateMatches(originCoord, sizeX, sizeY, rot);
					for (int j = 0; j < list2.Count; j++)
					{
						list.Add(list2[i].coord2);
					}
				}
			}
			return num;
		}
	}
}
