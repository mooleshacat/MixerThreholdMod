using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000420 RID: 1056
	[Serializable]
	public class WeedData : ProductItemData
	{
		// Token: 0x06001655 RID: 5717 RVA: 0x00063B63 File Offset: 0x00061D63
		public WeedData(string iD, int quantity, string quality, string packagingID) : base(iD, quantity, quality, packagingID)
		{
		}
	}
}
