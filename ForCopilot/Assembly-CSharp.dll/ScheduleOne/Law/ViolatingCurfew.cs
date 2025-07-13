using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005FA RID: 1530
	[Serializable]
	public class ViolatingCurfew : Crime
	{
		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x00098041 File Offset: 0x00096241
		// (set) Token: 0x0600257C RID: 9596 RVA: 0x00098049 File Offset: 0x00096249
		public override string CrimeName { get; protected set; } = "Violating curfew";
	}
}
