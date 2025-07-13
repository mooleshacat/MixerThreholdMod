using System;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C85 RID: 3205
	public class ParkingSpot : MonoBehaviour
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060059F4 RID: 23028 RVA: 0x0017B879 File Offset: 0x00179A79
		// (set) Token: 0x060059F5 RID: 23029 RVA: 0x0017B881 File Offset: 0x00179A81
		public LandVehicle OccupantVehicle { get; protected set; }

		// Token: 0x060059F6 RID: 23030 RVA: 0x0017B88A File Offset: 0x00179A8A
		private void Awake()
		{
			this.Init();
			if (this.ParentLot == null)
			{
				Debug.LogError("ParkingSpot has not parent ParkingLot!");
			}
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x0017B8AC File Offset: 0x00179AAC
		private void Init()
		{
			if (this.ParentLot == null)
			{
				this.ParentLot = base.GetComponentInParent<ParkingLot>();
			}
			if (this.ParentLot == null)
			{
				Debug.LogError("ParkingSpot has not parent ParkingLot!");
			}
			this.ParentLot.ParkingSpots.Add(this);
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x0017B8FC File Offset: 0x00179AFC
		public void SetOccupant(LandVehicle vehicle)
		{
			this.OccupantVehicle = vehicle;
			this.OccupantVehicle_Readonly = this.OccupantVehicle;
		}

		// Token: 0x04004202 RID: 16898
		private ParkingLot ParentLot;

		// Token: 0x04004203 RID: 16899
		public Transform AlignmentPoint;

		// Token: 0x04004204 RID: 16900
		public EParkingAlignment Alignment;

		// Token: 0x04004205 RID: 16901
		[SerializeField]
		private LandVehicle OccupantVehicle_Readonly;
	}
}
