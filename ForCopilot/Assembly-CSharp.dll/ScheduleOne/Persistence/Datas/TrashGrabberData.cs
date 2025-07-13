using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041E RID: 1054
	[Serializable]
	public class TrashGrabberData : ItemData
	{
		// Token: 0x06001653 RID: 5715 RVA: 0x00063BD8 File Offset: 0x00061DD8
		public TrashGrabberData(string iD, int quantity, TrashContentData content) : base(iD, quantity)
		{
			this.Content = content;
		}

		// Token: 0x0400141D RID: 5149
		public TrashContentData Content;
	}
}
