using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F9 RID: 1529
	[Serializable]
	public class TransportingIllicitItems : Crime
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x0009801D File Offset: 0x0009621D
		// (set) Token: 0x06002579 RID: 9593 RVA: 0x00098025 File Offset: 0x00096225
		public override string CrimeName { get; protected set; } = "Transporting illicit items";
	}
}
