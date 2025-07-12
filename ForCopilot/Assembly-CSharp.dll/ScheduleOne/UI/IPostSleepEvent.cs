using System;

namespace ScheduleOne.UI
{
	// Token: 0x02000A68 RID: 2664
	public interface IPostSleepEvent
	{
		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060047AC RID: 18348
		bool IsRunning { get; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060047AD RID: 18349
		int Order { get; }

		// Token: 0x060047AE RID: 18350
		void StartEvent();
	}
}
