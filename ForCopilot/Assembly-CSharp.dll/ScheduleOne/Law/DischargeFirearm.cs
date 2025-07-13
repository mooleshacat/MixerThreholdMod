using System;

namespace ScheduleOne.Law
{
	// Token: 0x02000601 RID: 1537
	[Serializable]
	public class DischargeFirearm : Crime
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002590 RID: 9616 RVA: 0x0009813D File Offset: 0x0009633D
		// (set) Token: 0x06002591 RID: 9617 RVA: 0x00098145 File Offset: 0x00096345
		public override string CrimeName { get; protected set; } = "Discharge of a firearm in a public place";
	}
}
