using System;
using ScheduleOne.Map;
using ScheduleOne.Storage;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000758 RID: 1880
	[RequireComponent(typeof(LandVehicle))]
	public class DeliveryVehicle : MonoBehaviour
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x000D2AF4 File Offset: 0x000D0CF4
		// (set) Token: 0x06003292 RID: 12946 RVA: 0x000D2AFC File Offset: 0x000D0CFC
		public LandVehicle Vehicle { get; private set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x000D2B05 File Offset: 0x000D0D05
		// (set) Token: 0x06003294 RID: 12948 RVA: 0x000D2B0D File Offset: 0x000D0D0D
		public DeliveryInstance ActiveDelivery { get; private set; }

		// Token: 0x06003295 RID: 12949 RVA: 0x000D2B16 File Offset: 0x000D0D16
		private void Awake()
		{
			this.Vehicle = base.GetComponent<LandVehicle>();
			this.Vehicle.SetGUID(new Guid(this.GUID));
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000D2B3C File Offset: 0x000D0D3C
		public void Activate(DeliveryInstance instance)
		{
			Console.Log("Activating delivery vehicle for delivery instance " + instance.DeliveryID, null);
			this.ActiveDelivery = instance;
			ParkingLot parking = instance.LoadingDock.Parking;
			instance.LoadingDock.SetStaticOccupant(this.Vehicle);
			this.Vehicle.Park(null, new ParkData
			{
				lotGUID = parking.GUID,
				spotIndex = 0,
				alignment = parking.ParkingSpots[0].Alignment
			}, false);
			this.Vehicle.SetVisible(true);
			this.Vehicle.Storage.AccessSettings = StorageEntity.EAccessSettings.Full;
			this.Vehicle.GetComponentInChildren<StorageDoorAnimation>().OverrideState(true);
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000D2BF0 File Offset: 0x000D0DF0
		public void Deactivate()
		{
			if (this.Vehicle != null)
			{
				this.Vehicle.ExitPark(false);
				this.Vehicle.SetIsStatic(true);
				this.Vehicle.SetVisible(false);
				this.Vehicle.SetTransform(new Vector3(0f, -100f, 0f), Quaternion.identity);
			}
			if (this.ActiveDelivery != null)
			{
				this.ActiveDelivery.LoadingDock.SetStaticOccupant(null);
				this.ActiveDelivery.LoadingDock.VehicleDetector.Clear();
			}
		}

		// Token: 0x040023BA RID: 9146
		public string GUID = string.Empty;
	}
}
