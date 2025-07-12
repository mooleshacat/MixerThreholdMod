using System;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x020009DA RID: 2522
	public class AvatarSeat : MonoBehaviour
	{
		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06004430 RID: 17456 RVA: 0x0011EC43 File Offset: 0x0011CE43
		public bool IsOccupied
		{
			get
			{
				return this.Occupant != null;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x0011EC51 File Offset: 0x0011CE51
		// (set) Token: 0x06004432 RID: 17458 RVA: 0x0011EC59 File Offset: 0x0011CE59
		public NPC Occupant { get; protected set; }

		// Token: 0x06004433 RID: 17459 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0011EC62 File Offset: 0x0011CE62
		public void SetOccupant(NPC npc)
		{
			if (npc != null && this.IsOccupied)
			{
				Debug.LogWarning("Seat is already occupied");
				return;
			}
			this.Occupant = npc;
		}

		// Token: 0x04003120 RID: 12576
		public Transform SittingPoint;

		// Token: 0x04003121 RID: 12577
		public Transform AccessPoint;
	}
}
