using System;
using System.Collections.Generic;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007D0 RID: 2000
	public class CornerObstacle : MonoBehaviour
	{
		// Token: 0x06003633 RID: 13875 RVA: 0x000E4738 File Offset: 0x000E2938
		public List<Tile> GetNeighbourTiles(Tile pairedTile)
		{
			List<Tile> list = new List<Tile>();
			List<Tile> surroundingTiles = pairedTile.GetSurroundingTiles();
			surroundingTiles.Add(pairedTile);
			for (int i = 0; i < surroundingTiles.Count; i++)
			{
				if (Vector3.Distance(surroundingTiles[i].transform.position, base.transform.position) < 0.5f)
				{
					list.Add(surroundingTiles[i]);
				}
			}
			return list;
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000E47A0 File Offset: 0x000E29A0
		private bool ApproxEquals(float a, float b, float precision)
		{
			return Mathf.Abs(a - b) <= precision;
		}

		// Token: 0x04002665 RID: 9829
		public bool obstacleEnabled;

		// Token: 0x04002666 RID: 9830
		public FootprintTile parentFootprint;

		// Token: 0x04002667 RID: 9831
		public Vector2 coordinates = Vector2.zero;
	}
}
