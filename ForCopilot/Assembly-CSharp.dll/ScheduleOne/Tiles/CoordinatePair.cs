using System;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C6 RID: 710
	public class CoordinatePair
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x00042EFB File Offset: 0x000410FB
		public CoordinatePair(Coordinate _c1, Coordinate _c2)
		{
			this.coord1 = _c1;
			this.coord2 = _c2;
		}

		// Token: 0x04000F75 RID: 3957
		public Coordinate coord1;

		// Token: 0x04000F76 RID: 3958
		public Coordinate coord2;
	}
}
