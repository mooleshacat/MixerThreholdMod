using System;

namespace ScheduleOne.Polling
{
	// Token: 0x0200033F RID: 831
	[Serializable]
	public class PollAnswer
	{
		// Token: 0x06001240 RID: 4672 RVA: 0x0004F07A File Offset: 0x0004D27A
		public PollAnswer(int _pollId, int _answer, string _ticket)
		{
			this.pollId = _pollId;
			this.answer = _answer;
			this.ticket = _ticket;
		}

		// Token: 0x04001197 RID: 4503
		public int pollId;

		// Token: 0x04001198 RID: 4504
		public int answer;

		// Token: 0x04001199 RID: 4505
		public string ticket;
	}
}
