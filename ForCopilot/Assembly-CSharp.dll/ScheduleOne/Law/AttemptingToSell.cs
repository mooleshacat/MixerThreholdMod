using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005FB RID: 1531
	[Serializable]
	public class AttemptingToSell : Crime
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x00098065 File Offset: 0x00096265
		// (set) Token: 0x0600257F RID: 9599 RVA: 0x0009806D File Offset: 0x0009626D
		public override string CrimeName { get; protected set; } = "Attempting to sell illicit items";
	}
}
