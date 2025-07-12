using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000445 RID: 1093
	[Serializable]
	public class SupplierData : NPCData
	{
		// Token: 0x0600167C RID: 5756 RVA: 0x00064060 File Offset: 0x00062260
		public SupplierData(string id, int _timeSinceMeetingStart, int _timeSinceLastMeetingEnd, float _debt, int _minsUntilDeadDropReady, StringIntPair[] _deaddropItems, bool _debtReminderSent) : base(id)
		{
			this.timeSinceMeetingStart = _timeSinceMeetingStart;
			this.timeSinceLastMeetingEnd = _timeSinceLastMeetingEnd;
			this.debt = _debt;
			this.minsUntilDeadDropReady = _minsUntilDeadDropReady;
			this.deaddropItems = _deaddropItems;
			this.debtReminderSent = _debtReminderSent;
		}

		// Token: 0x04001471 RID: 5233
		public int timeSinceMeetingStart;

		// Token: 0x04001472 RID: 5234
		public int timeSinceLastMeetingEnd;

		// Token: 0x04001473 RID: 5235
		public float debt;

		// Token: 0x04001474 RID: 5236
		public int minsUntilDeadDropReady;

		// Token: 0x04001475 RID: 5237
		public StringIntPair[] deaddropItems;

		// Token: 0x04001476 RID: 5238
		public bool debtReminderSent;
	}
}
