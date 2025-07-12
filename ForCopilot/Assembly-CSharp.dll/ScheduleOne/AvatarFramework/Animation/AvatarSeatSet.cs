using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x020009DB RID: 2523
	public class AvatarSeatSet : MonoBehaviour
	{
		// Token: 0x06004436 RID: 17462 RVA: 0x0011EC88 File Offset: 0x0011CE88
		public AvatarSeat GetFirstFreeSeat()
		{
			for (int i = 0; i < this.Seats.Length; i++)
			{
				if (!this.Seats[i].IsOccupied)
				{
					return this.Seats[i];
				}
			}
			Console.LogWarning("Failed to find a free seat! Returning the first seat.", null);
			return this.Seats[0];
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x0011ECD4 File Offset: 0x0011CED4
		public AvatarSeat GetRandomFreeSeat()
		{
			List<AvatarSeat> list = (from x in this.Seats
			where !x.IsOccupied
			select x).ToList<AvatarSeat>();
			if (list.Count == 0)
			{
				Console.LogWarning("Failed to find a free seat! Returning the first seat.", null);
				return this.Seats[0];
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x04003122 RID: 12578
		public AvatarSeat[] Seats;
	}
}
