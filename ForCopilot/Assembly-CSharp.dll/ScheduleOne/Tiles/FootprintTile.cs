using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C7 RID: 711
	public class FootprintTile : MonoBehaviour
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00042F11 File Offset: 0x00041111
		// (set) Token: 0x06000F3F RID: 3903 RVA: 0x00042F19 File Offset: 0x00041119
		public Tile MatchedStandardTile { get; protected set; }

		// Token: 0x06000F40 RID: 3904 RVA: 0x00042F22 File Offset: 0x00041122
		protected virtual void Awake()
		{
			this.tileAppearance.SetVisible(false);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00042F30 File Offset: 0x00041130
		public virtual void Initialize(Tile matchedTile)
		{
			this.MatchedStandardTile = matchedTile;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00042F3C File Offset: 0x0004113C
		public bool AreCornerObstaclesBlocked(Tile proposedTile)
		{
			if (proposedTile == null)
			{
				return true;
			}
			for (int i = 0; i < this.Corners.Count; i++)
			{
				if (this.Corners[i].obstacleEnabled)
				{
					List<Tile> neighbourTiles = this.Corners[i].GetNeighbourTiles(proposedTile);
					if (neighbourTiles.Count >= 4)
					{
						Dictionary<GridItem, int> dictionary = new Dictionary<GridItem, int>();
						for (int j = 0; j < neighbourTiles.Count; j++)
						{
							for (int k = 0; k < neighbourTiles[j].BuildableOccupants.Count; k++)
							{
								if (!dictionary.ContainsKey(neighbourTiles[j].BuildableOccupants[k]))
								{
									dictionary.Add(neighbourTiles[j].BuildableOccupants[k], 1);
								}
								else
								{
									Dictionary<GridItem, int> dictionary2 = dictionary;
									GridItem key = neighbourTiles[j].BuildableOccupants[k];
									int num = dictionary2[key];
									dictionary2[key] = num + 1;
								}
							}
						}
						foreach (GridItem key2 in dictionary.Keys)
						{
							if (dictionary[key2] == neighbourTiles.Count)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04000F77 RID: 3959
		public TileAppearance tileAppearance;

		// Token: 0x04000F78 RID: 3960
		public TileDetector tileDetector;

		// Token: 0x04000F79 RID: 3961
		public int X;

		// Token: 0x04000F7A RID: 3962
		public int Y;

		// Token: 0x04000F7B RID: 3963
		public float RequiredOffset;

		// Token: 0x04000F7C RID: 3964
		public List<CornerObstacle> Corners = new List<CornerObstacle>();
	}
}
