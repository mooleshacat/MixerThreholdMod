using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C83 RID: 3203
	public class ParkingLot : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060059E9 RID: 23017 RVA: 0x0017B6B4 File Offset: 0x001798B4
		// (set) Token: 0x060059EA RID: 23018 RVA: 0x0017B6BC File Offset: 0x001798BC
		public Guid GUID { get; protected set; }

		// Token: 0x060059EB RID: 23019 RVA: 0x0017B6C8 File Offset: 0x001798C8
		private void Awake()
		{
			if (this.ExitPoint != null && this.ExitPointVehicleDetector == null)
			{
				Console.LogWarning("ExitPoint specified but no ExitPointVehicleDetector!", null);
			}
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x0017B73B File Offset: 0x0017993B
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x0017B74C File Offset: 0x0017994C
		public ParkingSpot GetRandomFreeSpot()
		{
			List<ParkingSpot> freeParkingSpots = this.GetFreeParkingSpots();
			if (freeParkingSpots.Count == 0)
			{
				Console.Log("No free parking spots in " + base.gameObject.name + "!", null);
				return null;
			}
			return freeParkingSpots[UnityEngine.Random.Range(0, freeParkingSpots.Count)];
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x0017B79C File Offset: 0x0017999C
		public int GetRandomFreeSpotIndex()
		{
			List<ParkingSpot> freeParkingSpots = this.GetFreeParkingSpots();
			if (freeParkingSpots.Count == 0)
			{
				return -1;
			}
			return this.ParkingSpots.IndexOf(freeParkingSpots[UnityEngine.Random.Range(0, freeParkingSpots.Count)]);
		}

		// Token: 0x060059EF RID: 23023 RVA: 0x0017B7D8 File Offset: 0x001799D8
		public List<ParkingSpot> GetFreeParkingSpots()
		{
			if (this.ParkingSpots == null || this.ParkingSpots.Count == 0)
			{
				return new List<ParkingSpot>();
			}
			return (from x in this.ParkingSpots
			where x != null && x.OccupantVehicle == null
			select x).ToList<ParkingSpot>();
		}

		// Token: 0x040041F7 RID: 16887
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x040041F9 RID: 16889
		[Header("READONLY")]
		public List<ParkingSpot> ParkingSpots = new List<ParkingSpot>();

		// Token: 0x040041FA RID: 16890
		[Header("Entry")]
		public Transform EntryPoint;

		// Token: 0x040041FB RID: 16891
		public Transform HiddenVehicleAccessPoint;

		// Token: 0x040041FC RID: 16892
		[Header("Exit")]
		public bool UseExitPoint;

		// Token: 0x040041FD RID: 16893
		public EParkingAlignment ExitAlignment = EParkingAlignment.RearToKerb;

		// Token: 0x040041FE RID: 16894
		public Transform ExitPoint;

		// Token: 0x040041FF RID: 16895
		public VehicleDetector ExitPointVehicleDetector;
	}
}
