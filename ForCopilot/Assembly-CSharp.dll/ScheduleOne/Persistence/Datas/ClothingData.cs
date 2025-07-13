using System;
using ScheduleOne.Clothing;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000415 RID: 1045
	[Serializable]
	public class ClothingData : ItemData
	{
		// Token: 0x0600164A RID: 5706 RVA: 0x00063B52 File Offset: 0x00061D52
		public ClothingData(string iD, int quantity, EClothingColor color) : base(iD, quantity)
		{
			this.Color = color;
		}

		// Token: 0x04001414 RID: 5140
		public EClothingColor Color;
	}
}
