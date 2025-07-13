using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F3 RID: 1523
	[Serializable]
	public class PossessingModerateSeverityDrug : Crime
	{
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002566 RID: 9574 RVA: 0x00097F45 File Offset: 0x00096145
		// (set) Token: 0x06002567 RID: 9575 RVA: 0x00097F4D File Offset: 0x0009614D
		public override string CrimeName { get; protected set; } = "Possession of moderate-severity drug";
	}
}
