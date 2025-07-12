using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041B RID: 1051
	[Serializable]
	public class ProductItemData : QualityItemData
	{
		// Token: 0x06001650 RID: 5712 RVA: 0x00063B97 File Offset: 0x00061D97
		public ProductItemData(string iD, int quantity, string quality, string packagingID) : base(iD, quantity, quality)
		{
			this.PackagingID = packagingID;
		}

		// Token: 0x04001418 RID: 5144
		public string PackagingID;
	}
}
