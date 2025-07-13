using System;

namespace ScheduleOne.Persistence.Datas.Characters
{
	// Token: 0x02000479 RID: 1145
	public class ThomasData : NPCData
	{
		// Token: 0x060016B8 RID: 5816 RVA: 0x0006493C File Offset: 0x00062B3C
		public ThomasData(string id, bool meetingReminderSent, bool handoverReminderSent) : base(id)
		{
			this.MeetingReminderSent = meetingReminderSent;
			this.HandoverReminderSent = handoverReminderSent;
		}

		// Token: 0x0400150E RID: 5390
		public bool MeetingReminderSent;

		// Token: 0x0400150F RID: 5391
		public bool HandoverReminderSent;
	}
}
