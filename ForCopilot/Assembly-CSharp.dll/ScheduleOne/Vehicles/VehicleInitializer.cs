using System;
using FishNet.Object;
using ScheduleOne.Map;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x02000813 RID: 2067
	[RequireComponent(typeof(LandVehicle))]
	public class VehicleInitializer : NetworkBehaviour
	{
		// Token: 0x06003837 RID: 14391 RVA: 0x000ED10C File Offset: 0x000EB30C
		public override void OnStartServer()
		{
			base.OnStartServer();
			if (this.InitialParkingLot != null && !base.GetComponent<LandVehicle>().isParked)
			{
				int randomFreeSpotIndex = this.InitialParkingLot.GetRandomFreeSpotIndex();
				if (randomFreeSpotIndex != -1)
				{
					EParkingAlignment alignment = this.InitialParkingLot.ParkingSpots[randomFreeSpotIndex].Alignment;
				}
			}
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x000ED161 File Offset: 0x000EB361
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x000ED174 File Offset: 0x000EB374
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x000ED187 File Offset: 0x000EB387
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x000ED187 File Offset: 0x000EB387
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002803 RID: 10243
		public ParkingLot InitialParkingLot;

		// Token: 0x04002804 RID: 10244
		private bool dll_Excuted;

		// Token: 0x04002805 RID: 10245
		private bool dll_Excuted;
	}
}
