using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F2 RID: 1522
	[Serializable]
	public class PossessingLowSeverityDrug : Crime
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002563 RID: 9571 RVA: 0x00097F21 File Offset: 0x00096121
		// (set) Token: 0x06002564 RID: 9572 RVA: 0x00097F29 File Offset: 0x00096129
		public override string CrimeName { get; protected set; } = "Possession of low-severity drug";
	}
}
