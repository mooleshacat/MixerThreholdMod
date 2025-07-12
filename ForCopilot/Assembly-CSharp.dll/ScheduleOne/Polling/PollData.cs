using System;

namespace ScheduleOne.Polling
{
	// Token: 0x02000340 RID: 832
	[Serializable]
	public class PollData
	{
		// Token: 0x0400119A RID: 4506
		public int pollId;

		// Token: 0x0400119B RID: 4507
		public string question;

		// Token: 0x0400119C RID: 4508
		public string[] answers;

		// Token: 0x0400119D RID: 4509
		public int winnerIndex;

		// Token: 0x0400119E RID: 4510
		public string confirmationMessage;
	}
}
