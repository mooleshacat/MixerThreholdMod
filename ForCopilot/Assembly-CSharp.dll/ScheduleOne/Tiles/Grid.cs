using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.EntityFramework;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C8 RID: 712
	public class Grid : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x000430B3 File Offset: 0x000412B3
		// (set) Token: 0x06000F45 RID: 3909 RVA: 0x000430BB File Offset: 0x000412BB
		public Guid GUID { get; protected set; }

		// Token: 0x06000F46 RID: 3910 RVA: 0x000430C4 File Offset: 0x000412C4
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x000430D4 File Offset: 0x000412D4
		protected virtual void Awake()
		{
			if (this.IsStatic)
			{
				if (!GUIDManager.IsGUIDValid(this.StaticGUID))
				{
					Console.LogError("Static GUID is not valid.", null);
				}
				((IGUIDRegisterable)this).SetGUID(this.StaticGUID);
			}
			if (base.GetComponentInParent<Property>() != null && !this.IsStatic)
			{
				Debug.LogWarning("Grid is a child of a Property, but is not marked as static!");
			}
			this.SetInvisible();
			this.ProcessCoordinateDataPairs();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0004313C File Offset: 0x0004133C
		public virtual void DestroyGrid()
		{
			GridItem[] componentsInChildren = this.Container.GetComponentsInChildren<GridItem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DestroyItem(true);
			}
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0004316C File Offset: 0x0004136C
		private void ProcessCoordinateDataPairs()
		{
			foreach (CoordinateTilePair coordinateTilePair in this.CoordinateTilePairs)
			{
				this._coordinateToTile.Add(coordinateTilePair.coord, coordinateTilePair.tile);
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000431D0 File Offset: 0x000413D0
		public void RegisterTile(Tile tile)
		{
			this.Tiles.Add(tile);
			CoordinateTilePair item = default(CoordinateTilePair);
			item.coord = new Coordinate(tile.x, tile.y);
			item.tile = tile;
			this.CoordinateTilePairs.Add(item);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00043220 File Offset: 0x00041420
		public void DeregisterTile(Tile tile)
		{
			Console.Log("Deregistering tile: " + tile.x.ToString() + ", " + tile.y.ToString(), null);
			this.Tiles.Remove(tile);
			for (int i = 0; i < this.CoordinateTilePairs.Count; i++)
			{
				if (this.CoordinateTilePairs[i].tile == tile)
				{
					this.CoordinateTilePairs.RemoveAt(i);
					i--;
					return;
				}
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000432A8 File Offset: 0x000414A8
		public Coordinate GetMatchedCoordinate(FootprintTile tileToMatch)
		{
			Vector3 vector = base.transform.InverseTransformPoint(tileToMatch.transform.position);
			return new Coordinate(Mathf.RoundToInt(vector.x / Grid.GridSideLength), Mathf.RoundToInt(vector.z / Grid.GridSideLength));
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000432F4 File Offset: 0x000414F4
		public bool IsTileValidAtCoordinate(Coordinate gridCoord, FootprintTile tile, GridItem tileOwner = null)
		{
			if (!this._coordinateToTile.ContainsKey(gridCoord))
			{
				return false;
			}
			Tile tile2 = this._coordinateToTile[gridCoord];
			return tile2.ConstructableOccupants.Count <= 0 && (tile2.BuildableOccupants.Count <= 0 || (!(tileOwner == null) && tileOwner.CanShareTileWith(tile2.BuildableOccupants))) && (tile2.AvailableOffset == 0f || tile.RequiredOffset == 0f || tile2.AvailableOffset >= tile.RequiredOffset) && tile2.CanBeBuiltOn();
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00043388 File Offset: 0x00041588
		public bool IsTileValidAtCoordinate(Coordinate gridCoord, FootprintTile tile, Constructable_GridBased ignoreConstructable)
		{
			if (!this._coordinateToTile.ContainsKey(gridCoord))
			{
				return false;
			}
			Tile tile2 = this._coordinateToTile[gridCoord];
			if (tile2.BuildableOccupants.Count > 0)
			{
				return false;
			}
			for (int i = 0; i < tile2.ConstructableOccupants.Count; i++)
			{
				if (tile2.ConstructableOccupants[i] != ignoreConstructable)
				{
					return false;
				}
			}
			return (tile2.AvailableOffset == 0f || tile.RequiredOffset == 0f || tile2.AvailableOffset >= tile.RequiredOffset) && tile2.CanBeBuiltOn();
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00043420 File Offset: 0x00041620
		public Tile GetTile(Coordinate coord)
		{
			return this.CoordinateTilePairs.Find((CoordinateTilePair x) => x.coord.Equals(coord)).tile;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00043458 File Offset: 0x00041658
		[Button]
		public void SetVisible()
		{
			for (int i = 0; i < this.CoordinateTilePairs.Count; i++)
			{
				this.CoordinateTilePairs[i].tile.SetVisible(true);
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00043494 File Offset: 0x00041694
		[Button]
		public void SetInvisible()
		{
			for (int i = 0; i < this.CoordinateTilePairs.Count; i++)
			{
				this.CoordinateTilePairs[i].tile.SetVisible(false);
			}
		}

		// Token: 0x04000F7E RID: 3966
		public static float GridSideLength = 0.5f;

		// Token: 0x04000F7F RID: 3967
		public List<Tile> Tiles = new List<Tile>();

		// Token: 0x04000F80 RID: 3968
		public List<CoordinateTilePair> CoordinateTilePairs = new List<CoordinateTilePair>();

		// Token: 0x04000F81 RID: 3969
		public Transform Container;

		// Token: 0x04000F82 RID: 3970
		public bool IsStatic;

		// Token: 0x04000F83 RID: 3971
		public string StaticGUID = string.Empty;

		// Token: 0x04000F85 RID: 3973
		protected Dictionary<Coordinate, Tile> _coordinateToTile = new Dictionary<Coordinate, Tile>();
	}
}
