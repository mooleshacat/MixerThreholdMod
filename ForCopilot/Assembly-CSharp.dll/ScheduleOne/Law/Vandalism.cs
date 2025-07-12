using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005FE RID: 1534
	[Serializable]
	public class Vandalism : Crime
	{
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x000980D1 File Offset: 0x000962D1
		// (set) Token: 0x06002588 RID: 9608 RVA: 0x000980D9 File Offset: 0x000962D9
		public override string CrimeName { get; protected set; } = "Vandalism";
	}
}
