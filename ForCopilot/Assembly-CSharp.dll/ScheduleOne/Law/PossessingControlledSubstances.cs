using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005F1 RID: 1521
	[Serializable]
	public class PossessingControlledSubstances : Crime
	{
		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x00097EFD File Offset: 0x000960FD
		// (set) Token: 0x06002561 RID: 9569 RVA: 0x00097F05 File Offset: 0x00096105
		public override string CrimeName { get; protected set; } = "Possession of controlled substances";
	}
}
