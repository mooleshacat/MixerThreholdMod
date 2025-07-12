using System;
using FishNet.Object;
using ScheduleOne.EntityFramework;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C4 RID: 708
	[Serializable]
	public struct CoordinateProceduralTilePair
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x00042EDE File Offset: 0x000410DE
		public ProceduralTile tile
		{
			get
			{
				return this.tileParent.GetComponent<IProceduralTileContainer>().ProceduralTiles[this.tileIndex];
			}
		}

		// Token: 0x04000F70 RID: 3952
		public Coordinate coord;

		// Token: 0x04000F71 RID: 3953
		public NetworkObject tileParent;

		// Token: 0x04000F72 RID: 3954
		public int tileIndex;
	}
}
