using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005FC RID: 1532
	[Serializable]
	public class Assault : Crime
	{
		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x00098089 File Offset: 0x00096289
		// (set) Token: 0x06002582 RID: 9602 RVA: 0x00098091 File Offset: 0x00096291
		public override string CrimeName { get; protected set; } = "Assault";
	}
}
