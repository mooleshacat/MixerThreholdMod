using System;
using System.Linq;

namespace ScheduleOne.Polling
{
	// Token: 0x02000346 RID: 838
	[Serializable]
	public class PollResponse
	{
		// Token: 0x06001267 RID: 4711 RVA: 0x0004F898 File Offset: 0x0004DA98
		public PollData GetActive()
		{
			return this.polls.FirstOrDefault((PollData x) => x.pollId == this.active);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0004F8B1 File Offset: 0x0004DAB1
		public PollData GetConfirmed()
		{
			return this.polls.FirstOrDefault((PollData x) => x.pollId == this.confirmed);
		}

		// Token: 0x040011BE RID: 4542
		public PollData[] polls;

		// Token: 0x040011BF RID: 4543
		public int active;

		// Token: 0x040011C0 RID: 4544
		public int confirmed;
	}
}
