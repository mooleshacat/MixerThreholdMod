using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005FD RID: 1533
	[Serializable]
	public class DeadlyAssault : Crime
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x000980AD File Offset: 0x000962AD
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x000980B5 File Offset: 0x000962B5
		public override string CrimeName { get; protected set; } = "Assault with a deadly weapon";
	}
}
