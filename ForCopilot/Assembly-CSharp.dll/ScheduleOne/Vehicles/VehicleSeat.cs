using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200081A RID: 2074
	public class VehicleSeat : MonoBehaviour
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x000EDE1C File Offset: 0x000EC01C
		public bool isOccupied
		{
			get
			{
				return this.Occupant != null;
			}
		}

		// Token: 0x04002835 RID: 10293
		public bool isDriverSeat;

		// Token: 0x04002836 RID: 10294
		public Player Occupant;
	}
}
