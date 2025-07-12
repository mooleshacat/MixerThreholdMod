using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000417 RID: 1047
	[Serializable]
	public class IntegerItemData : ItemData
	{
		// Token: 0x0600164C RID: 5708 RVA: 0x00063B70 File Offset: 0x00061D70
		public IntegerItemData(string iD, int quantity, int value) : base(iD, quantity)
		{
			this.Value = value;
		}

		// Token: 0x04001415 RID: 5141
		public int Value;
	}
}
