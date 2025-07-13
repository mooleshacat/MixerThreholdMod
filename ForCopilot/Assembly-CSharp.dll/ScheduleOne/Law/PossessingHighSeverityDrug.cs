using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F4 RID: 1524
	[Serializable]
	public class PossessingHighSeverityDrug : Crime
	{
		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x00097F69 File Offset: 0x00096169
		// (set) Token: 0x0600256A RID: 9578 RVA: 0x00097F71 File Offset: 0x00096171
		public override string CrimeName { get; protected set; } = "Possession of high-severity drug";
	}
}
