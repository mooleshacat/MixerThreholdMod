using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C1 RID: 1729
	public class DarkMarketRollerDoors : SensorRollerDoors
	{
		// Token: 0x06002F9A RID: 12186 RVA: 0x000C834F File Offset: 0x000C654F
		protected override bool CanOpen()
		{
			return NetworkSingleton<DarkMarket>.Instance.IsOpen;
		}
	}
}
