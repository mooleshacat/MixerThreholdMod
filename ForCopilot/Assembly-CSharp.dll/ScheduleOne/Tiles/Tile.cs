using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.EntityFramework;
using ScheduleOne.Lighting;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002CD RID: 717
	[Serializable]
	public class Tile : MonoBehaviour
	{
		// Token: 0x06000F5B RID: 3931 RVA: 0x000435B5 File Offset: 0x000417B5
		public void InitializePropertyTile(int _x, int _y, float _available_Offset, Grid _ownerGrid)
		{
			this.x = _x;
			this.y = _y;
			this.AvailableOffset = _available_Offset;
			this.OwnerGrid = _ownerGrid;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x000435D4 File Offset: 0x000417D4
		public void AddOccupant(GridItem occ, FootprintTile tile)
		{
			this.BuildableOccupants.Remove(occ);
			this.BuildableOccupants.Add(occ);
			this.OccupantTiles.Remove(tile);
			this.OccupantTiles.Add(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00043628 File Offset: 0x00041828
		public void AddOccupant(Constructable_GridBased occ, FootprintTile tile)
		{
			this.ConstructableOccupants.Remove(occ);
			this.ConstructableOccupants.Add(occ);
			this.OccupantTiles.Remove(tile);
			this.OccupantTiles.Add(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0004367B File Offset: 0x0004187B
		public void RemoveOccupant(GridItem occ, FootprintTile tile)
		{
			this.BuildableOccupants.Remove(occ);
			this.OccupantTiles.Remove(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x000436AB File Offset: 0x000418AB
		public void RemoveOccupant(Constructable_GridBased occ, FootprintTile tile)
		{
			this.ConstructableOccupants.Remove(occ);
			this.OccupantTiles.Remove(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x000436DB File Offset: 0x000418DB
		public virtual bool CanBeBuiltOn()
		{
			return !(this.OwnerGrid.GetComponentInParent<Property>() != null) || this.OwnerGrid.GetComponentInParent<Property>().IsOwned;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00043708 File Offset: 0x00041908
		public List<Tile> GetSurroundingTiles()
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Tile tile = this.OwnerGrid.GetTile(new Coordinate(this.x + i - 1, this.y + j - 1));
					if (tile != null && tile != this && !list.Contains(tile))
					{
						list.Add(tile);
					}
				}
			}
			return list;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual bool IsIndoorTile()
		{
			return false;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0004377B File Offset: 0x0004197B
		public void SetVisible(bool vis)
		{
			base.transform.Find("Model").gameObject.SetActive(vis);
		}

		// Token: 0x04000F8E RID: 3982
		public static float TileSize = 0.5f;

		// Token: 0x04000F8F RID: 3983
		public int x;

		// Token: 0x04000F90 RID: 3984
		public int y;

		// Token: 0x04000F91 RID: 3985
		[Header("Settings")]
		public float AvailableOffset = 1000f;

		// Token: 0x04000F92 RID: 3986
		[Header("References")]
		public Grid OwnerGrid;

		// Token: 0x04000F93 RID: 3987
		public LightExposureNode LightExposureNode;

		// Token: 0x04000F94 RID: 3988
		[Header("Occupants")]
		public List<GridItem> BuildableOccupants = new List<GridItem>();

		// Token: 0x04000F95 RID: 3989
		public List<Constructable_GridBased> ConstructableOccupants = new List<Constructable_GridBased>();

		// Token: 0x04000F96 RID: 3990
		public List<FootprintTile> OccupantTiles = new List<FootprintTile>();

		// Token: 0x04000F97 RID: 3991
		public Tile.TileChange onTileChanged;

		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x06000F67 RID: 3943
		public delegate void TileChange(Tile thisTile);
	}
}
