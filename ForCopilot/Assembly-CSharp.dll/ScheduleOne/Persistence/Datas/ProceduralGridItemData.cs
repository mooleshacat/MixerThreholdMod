using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000453 RID: 1107
	[Serializable]
	public class ProceduralGridItemData : BuildableItemData
	{
		// Token: 0x0600168A RID: 5770 RVA: 0x00064320 File Offset: 0x00062520
		public ProceduralGridItemData(Guid guid, ItemInstance item, int loadOrder, int rotation, FootprintMatchData[] footprintMatches) : base(guid, item, loadOrder)
		{
			this.Rotation = rotation;
			this.FootprintMatches = footprintMatches;
		}

		// Token: 0x040014A3 RID: 5283
		public int Rotation;

		// Token: 0x040014A4 RID: 5284
		public FootprintMatchData[] FootprintMatches;
	}
}
