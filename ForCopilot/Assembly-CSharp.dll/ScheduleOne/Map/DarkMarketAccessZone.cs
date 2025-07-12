using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Map
{
	// Token: 0x02000C6C RID: 3180
	public class DarkMarketAccessZone : TimedAccessZone
	{
		// Token: 0x06005980 RID: 22912 RVA: 0x0017A1EE File Offset: 0x001783EE
		protected override bool GetIsOpen()
		{
			return NetworkSingleton<DarkMarket>.Instance.IsOpen && NetworkSingleton<DarkMarket>.Instance.Unlocked && base.GetIsOpen();
		}
	}
}
