using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F0 RID: 1520
	[Serializable]
	public class Crime
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x00097ED9 File Offset: 0x000960D9
		// (set) Token: 0x0600255E RID: 9566 RVA: 0x00097EE1 File Offset: 0x000960E1
		public virtual string CrimeName { get; protected set; } = "Crime";
	}
}
