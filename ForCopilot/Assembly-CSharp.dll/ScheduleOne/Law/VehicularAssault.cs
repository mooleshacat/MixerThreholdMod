using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F6 RID: 1526
	[Serializable]
	public class VehicularAssault : Crime
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x00097FB1 File Offset: 0x000961B1
		// (set) Token: 0x06002570 RID: 9584 RVA: 0x00097FB9 File Offset: 0x000961B9
		public override string CrimeName { get; protected set; } = "Vehicular assault";
	}
}
