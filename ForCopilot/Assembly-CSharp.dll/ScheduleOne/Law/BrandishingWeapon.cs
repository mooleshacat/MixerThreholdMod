using System;

namespace ScheduleOne.Law
{
	// Token: 0x02000600 RID: 1536
	[Serializable]
	public class BrandishingWeapon : Crime
	{
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x00098119 File Offset: 0x00096319
		// (set) Token: 0x0600258E RID: 9614 RVA: 0x00098121 File Offset: 0x00096321
		public override string CrimeName { get; protected set; } = "Brandishing a weapon";
	}
}
