using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Money
{
	// Token: 0x02000BDC RID: 3036
	public class CashSlot : HotbarSlot
	{
		// Token: 0x060050B2 RID: 20658 RVA: 0x001554EF File Offset: 0x001536EF
		public override void ClearStoredInstance(bool _internal = false)
		{
			(base.ItemInstance as CashInstance).SetBalance(0f, true);
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x000022C9 File Offset: 0x000004C9
		public override bool CanSlotAcceptCash()
		{
			return true;
		}

		// Token: 0x04003C97 RID: 15511
		public const float MAX_CASH_PER_SLOT = 1000f;
	}
}
