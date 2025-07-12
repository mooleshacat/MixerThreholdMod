using System;
using ScheduleOne.Economy;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x020006C2 RID: 1730
	public class DealerStaticDoor : StaticDoor
	{
		// Token: 0x06002F9C RID: 12188 RVA: 0x000C8364 File Offset: 0x000C6564
		protected override bool IsKnockValid(out string message)
		{
			if (this.Building.OccupantCount == 0 && Vector3.Distance(base.transform.position, this.Dealer.transform.position) > 2f)
			{
				message = this.Dealer.FirstName + " is out dealing";
				return false;
			}
			return base.IsKnockValid(out message);
		}

		// Token: 0x04002175 RID: 8565
		public Dealer Dealer;
	}
}
