using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041C RID: 1052
	[Serializable]
	public class QualityItemData : ItemData
	{
		// Token: 0x06001651 RID: 5713 RVA: 0x00063BAA File Offset: 0x00061DAA
		public QualityItemData(string iD, int quantity, string quality) : base(iD, quantity)
		{
			this.Quality = quality;
		}

		// Token: 0x04001419 RID: 5145
		public string Quality;
	}
}
