using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000407 RID: 1031
	[Serializable]
	public class FootprintMatchData
	{
		// Token: 0x0600162E RID: 5678 RVA: 0x00063888 File Offset: 0x00061A88
		public FootprintMatchData(string tileOwnerGUID, int tileIndex, Vector2 footprintCoordinate)
		{
			this.TileOwnerGUID = tileOwnerGUID;
			this.TileIndex = tileIndex;
			this.FootprintCoordinate = footprintCoordinate;
		}

		// Token: 0x040013F9 RID: 5113
		public string TileOwnerGUID;

		// Token: 0x040013FA RID: 5114
		public int TileIndex;

		// Token: 0x040013FB RID: 5115
		public Vector2 FootprintCoordinate;
	}
}
