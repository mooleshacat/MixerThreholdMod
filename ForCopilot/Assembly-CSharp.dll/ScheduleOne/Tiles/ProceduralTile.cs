using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002CB RID: 715
	public class ProceduralTile : MonoBehaviour
	{
		// Token: 0x06000F57 RID: 3927 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00043529 File Offset: 0x00041729
		public void AddOccupant(FootprintTile footprint, ProceduralGridItem item)
		{
			if (!this.Occupants.Contains(item))
			{
				this.Occupants.Add(item);
			}
			if (!this.OccupantTiles.Contains(footprint))
			{
				this.OccupantTiles.Add(footprint);
			}
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0004355F File Offset: 0x0004175F
		public void RemoveOccupant(FootprintTile footprint, ProceduralGridItem item)
		{
			if (this.Occupants.Contains(item))
			{
				this.Occupants.Remove(item);
			}
			if (this.OccupantTiles.Contains(footprint))
			{
				this.OccupantTiles.Remove(footprint);
			}
		}

		// Token: 0x04000F87 RID: 3975
		[Header("Settings")]
		public ProceduralTile.EProceduralTileType TileType;

		// Token: 0x04000F88 RID: 3976
		[Header("References")]
		public BuildableItem ParentBuildableItem;

		// Token: 0x04000F89 RID: 3977
		public FootprintTile MatchedFootprintTile;

		// Token: 0x04000F8A RID: 3978
		[Header("Occupants")]
		public List<ProceduralGridItem> Occupants = new List<ProceduralGridItem>();

		// Token: 0x04000F8B RID: 3979
		public List<FootprintTile> OccupantTiles = new List<FootprintTile>();

		// Token: 0x020002CC RID: 716
		public enum EProceduralTileType
		{
			// Token: 0x04000F8D RID: 3981
			Rack
		}
	}
}
