using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005FF RID: 1535
	[Serializable]
	public class Theft : Crime
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x000980F5 File Offset: 0x000962F5
		// (set) Token: 0x0600258B RID: 9611 RVA: 0x000980FD File Offset: 0x000962FD
		public override string CrimeName { get; protected set; } = "Theft";
	}
}
