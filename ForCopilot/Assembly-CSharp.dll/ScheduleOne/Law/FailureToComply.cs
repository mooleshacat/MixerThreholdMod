using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F8 RID: 1528
	[Serializable]
	public class FailureToComply : Crime
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x00097FF9 File Offset: 0x000961F9
		// (set) Token: 0x06002576 RID: 9590 RVA: 0x00098001 File Offset: 0x00096201
		public override string CrimeName { get; protected set; } = "Failure to comply with police instruction";
	}
}
