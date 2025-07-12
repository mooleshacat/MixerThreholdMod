using System;

namespace ScheduleOne.Messaging
{
	// Token: 0x0200057D RID: 1405
	public interface IMessageEntity
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060021D5 RID: 8661
		// (set) Token: 0x060021D6 RID: 8662
		MSGConversation MsgConversation { get; set; }

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060021D7 RID: 8663
		// (remove) Token: 0x060021D8 RID: 8664
		event ResponseCallback onResponseChosen;
	}
}
