using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F5 RID: 1525
	[Serializable]
	public class Evading : Crime
	{
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x00097F8D File Offset: 0x0009618D
		// (set) Token: 0x0600256D RID: 9581 RVA: 0x00097F95 File Offset: 0x00096195
		public override string CrimeName { get; protected set; } = "Evading arrest";
	}
}
