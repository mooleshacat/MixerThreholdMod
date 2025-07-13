using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F7 RID: 1527
	[Serializable]
	public class DrugTrafficking : Crime
	{
		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x00097FD5 File Offset: 0x000961D5
		// (set) Token: 0x06002573 RID: 9587 RVA: 0x00097FDD File Offset: 0x000961DD
		public override string CrimeName { get; protected set; } = "Drug trafficking";
	}
}
