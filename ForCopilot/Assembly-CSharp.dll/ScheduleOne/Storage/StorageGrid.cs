using System;
using System.Collections.Generic;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008EA RID: 2282
	public class StorageGrid : MonoBehaviour
	{
		// Token: 0x06003DC4 RID: 15812 RVA: 0x00104B10 File Offset: 0x00102D10
		protected virtual void Awake()
		{
			this.ProcessCoordinateTilePairs();
			this.freeTiles.AddRange(this.storageTiles);
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00104B2C File Offset: 0x00102D2C
		private void ProcessCoordinateTilePairs()
		{
			foreach (CoordinateStorageTilePair coordinateStorageTilePair in this.coordinateStorageTilePairs)
			{
				this.coordinateToTile.Add(coordinateStorageTilePair.coord, coordinateStorageTilePair.tile);
			}
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x00104B90 File Offset: 0x00102D90
		public void RegisterTile(StorageTile tile)
		{
			this.storageTiles.Add(tile);
			CoordinateStorageTilePair item = default(CoordinateStorageTilePair);
			item.coord = new Coordinate(tile.x, tile.y);
			item.tile = tile;
			this.coordinateStorageTilePairs.Add(item);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00104BE0 File Offset: 0x00102DE0
		public void DeregisterTile(StorageTile tile)
		{
			this.storageTiles.Remove(tile);
			for (int i = 0; i < this.coordinateStorageTilePairs.Count; i++)
			{
				if (this.coordinateStorageTilePairs[i].tile == tile)
				{
					this.coordinateStorageTilePairs.RemoveAt(i);
					i--;
					return;
				}
			}
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x00104C3C File Offset: 0x00102E3C
		public bool IsItemPositionValid(StorageTile primaryTile, FootprintTile primaryFootprintTile, StoredItem item)
		{
			foreach (CoordinateStorageFootprintTilePair coordinateStorageFootprintTilePair in item.CoordinateFootprintTilePairs)
			{
				Coordinate matchedCoordinate = this.GetMatchedCoordinate(coordinateStorageFootprintTilePair.tile);
				if (!this.IsGridPositionValid(matchedCoordinate, coordinateStorageFootprintTilePair.tile))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00104CAC File Offset: 0x00102EAC
		public Coordinate GetMatchedCoordinate(FootprintTile tileToMatch)
		{
			Vector3 vector = base.transform.InverseTransformPoint(tileToMatch.transform.position);
			return new Coordinate(Mathf.RoundToInt(vector.x / StorageGrid.gridSize), Mathf.RoundToInt(vector.z / StorageGrid.gridSize));
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00104CF7 File Offset: 0x00102EF7
		public bool IsGridPositionValid(Coordinate gridCoord, FootprintTile tile)
		{
			return this.coordinateToTile.ContainsKey(gridCoord) && !(this.coordinateToTile[gridCoord].occupant != null);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00104D28 File Offset: 0x00102F28
		public StorageTile GetTile(Coordinate coord)
		{
			for (int i = 0; i < this.coordinateStorageTilePairs.Count; i++)
			{
				if (this.coordinateStorageTilePairs[i].coord.Equals(coord))
				{
					return this.coordinateStorageTilePairs[i].tile;
				}
			}
			return null;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00104D78 File Offset: 0x00102F78
		public int GetUserEndCapacity()
		{
			int actualY = this.GetActualY();
			int num = this.coordinateStorageTilePairs.Count / actualY;
			return (actualY - 1) * (num - 1);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x00104DA4 File Offset: 0x00102FA4
		public int GetActualY()
		{
			int result = 0;
			for (int i = 0; i < this.coordinateStorageTilePairs.Count; i++)
			{
				if (this.coordinateStorageTilePairs[i].coord.x != 0)
				{
					result = i;
					break;
				}
				i++;
			}
			return result;
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x00104DEC File Offset: 0x00102FEC
		public int GetActualX()
		{
			return this.coordinateStorageTilePairs.Count / this.GetActualY();
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x00104E00 File Offset: 0x00103000
		public int GetTotalFootprintSize()
		{
			return this.coordinateStorageTilePairs.Count;
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00104E10 File Offset: 0x00103010
		public bool TryFitItem(int sizeX, int sizeY, List<Coordinate> lockedCoordinates, out Coordinate originCoordinate, out float rotation)
		{
			foreach (CoordinateStorageTilePair coordinateStorageTilePair in this.coordinateStorageTilePairs)
			{
				if (!(coordinateStorageTilePair.tile.occupant != null))
				{
					originCoordinate = coordinateStorageTilePair.coord;
					bool flag = true;
					rotation = 0f;
					for (int i = 0; i < sizeX; i++)
					{
						for (int j = 0; j < sizeY; j++)
						{
							Coordinate coordinate = new Coordinate(coordinateStorageTilePair.tile.x + i, coordinateStorageTilePair.tile.y + j);
							for (int k = 0; k < lockedCoordinates.Count; k++)
							{
								if (coordinate.Equals(lockedCoordinates[k]))
								{
									flag = false;
								}
							}
							StorageTile tile = this.GetTile(coordinate);
							if (tile == null || tile.occupant != null)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						return true;
					}
					flag = true;
					rotation = 90f;
					for (int l = 0; l < sizeX; l++)
					{
						for (int m = 0; m < sizeY; m++)
						{
							Coordinate coordinate2 = new Coordinate(coordinateStorageTilePair.tile.x + m, coordinateStorageTilePair.tile.y - l);
							for (int n = 0; n < lockedCoordinates.Count; n++)
							{
								if (coordinate2.Equals(lockedCoordinates[n]))
								{
									flag = false;
								}
							}
							StorageTile tile2 = this.GetTile(coordinate2);
							if (tile2 == null || tile2.occupant != null)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						return true;
					}
				}
			}
			originCoordinate = new Coordinate(0, 0);
			rotation = 0f;
			return false;
		}

		// Token: 0x04002C18 RID: 11288
		public static float gridSize = 0.25f;

		// Token: 0x04002C19 RID: 11289
		public List<StorageTile> storageTiles = new List<StorageTile>();

		// Token: 0x04002C1A RID: 11290
		public List<StorageTile> freeTiles = new List<StorageTile>();

		// Token: 0x04002C1B RID: 11291
		public List<CoordinateStorageTilePair> coordinateStorageTilePairs = new List<CoordinateStorageTilePair>();

		// Token: 0x04002C1C RID: 11292
		protected Dictionary<Coordinate, StorageTile> coordinateToTile = new Dictionary<Coordinate, StorageTile>();
	}
}
