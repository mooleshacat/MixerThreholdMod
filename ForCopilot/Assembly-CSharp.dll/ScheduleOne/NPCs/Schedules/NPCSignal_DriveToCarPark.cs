using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.Map;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x020004B8 RID: 1208
	public class NPCSignal_DriveToCarPark : NPCSignal
	{
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x00072259 File Offset: 0x00070459
		public new string ActionName
		{
			get
			{
				return "Drive to car park";
			}
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00072260 File Offset: 0x00070460
		public override string GetName()
		{
			if (this.ParkingLot == null)
			{
				return this.ActionName + " (No Parking Lot)";
			}
			return this.ActionName + " (" + this.ParkingLot.gameObject.name + ")";
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000722B1 File Offset: 0x000704B1
		protected override void OnValidate()
		{
			base.OnValidate();
			this.priority = 12;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x000722C1 File Offset: 0x000704C1
		public override void Started()
		{
			base.Started();
			this.isAtDestination = false;
			this.CheckValidForStart();
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000722D6 File Offset: 0x000704D6
		public override void End()
		{
			base.End();
			if (this.npc.CurrentVehicle != null)
			{
				this.npc.ExitVehicle();
			}
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000722FC File Offset: 0x000704FC
		public override void LateStarted()
		{
			base.LateStarted();
			this.isAtDestination = false;
			this.CheckValidForStart();
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x00072311 File Offset: 0x00070511
		private void CheckValidForStart()
		{
			if (this.Vehicle.CurrentParkingLot == this.ParkingLot)
			{
				this.End();
			}
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x00072334 File Offset: 0x00070534
		public override void Interrupt()
		{
			base.Interrupt();
			this.Park();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.IsInVehicle)
			{
				this.Vehicle.Agent.StopNavigating();
				this.npc.ExitVehicle();
				return;
			}
			this.npc.Movement.Stop();
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0007238E File Offset: 0x0007058E
		public override void Resume()
		{
			base.Resume();
			this.isAtDestination = false;
			this.CheckValidForStart();
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000723A3 File Offset: 0x000705A3
		public override void Skipped()
		{
			base.Skipped();
			this.Park();
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000723B1 File Offset: 0x000705B1
		public override void ResumeFailed()
		{
			base.ResumeFailed();
			this.Park();
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x000723BF File Offset: 0x000705BF
		public override void JumpTo()
		{
			base.JumpTo();
			this.isAtDestination = false;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000723D0 File Offset: 0x000705D0
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (this.npc.IsInVehicle)
			{
				this.timeInVehicle += 1f;
			}
			else
			{
				this.timeInVehicle = 0f;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.IsInVehicle && this.npc.CurrentVehicle.CurrentParkingLot == this.ParkingLot)
			{
				this.timeAtDestination += 1f;
				if (this.timeAtDestination > 1f)
				{
					this.End();
				}
			}
			else
			{
				this.timeAtDestination = 0f;
			}
			if (!this.isAtDestination)
			{
				if (this.npc.IsInVehicle)
				{
					if (this.Vehicle.isParked)
					{
						if (this.timeInVehicle > 1f)
						{
							this.Vehicle.ExitPark_Networked(null, this.Vehicle.CurrentParkingLot.UseExitPoint);
							return;
						}
					}
					else if (!this.Vehicle.Agent.AutoDriving)
					{
						this.Vehicle.Agent.Navigate(this.ParkingLot.EntryPoint.position, null, new VehicleAgent.NavigationCallback(this.DriveCallback));
						return;
					}
				}
				else if ((!this.npc.Movement.IsMoving || Vector3.Distance(this.npc.Movement.CurrentDestination, this.GetWalkDestination()) > 1f) && this.npc.Movement.CanMove())
				{
					if (this.npc.Movement.CanGetTo(this.GetWalkDestination(), 2f))
					{
						base.SetDestination(this.GetWalkDestination(), true);
						return;
					}
					this.npc.EnterVehicle(null, this.Vehicle);
					Console.LogWarning(string.Concat(new string[]
					{
						"NPC ",
						this.npc.name,
						" was unable to reach vehicle ",
						this.Vehicle.name,
						" and was teleported to it."
					}), null);
					Debug.DrawLine(this.npc.transform.position, this.GetWalkDestination(), Color.red, 10f);
				}
			}
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000725FB File Offset: 0x000707FB
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Success || result == NPCMovement.WalkResult.Partial)
			{
				this.npc.EnterVehicle(null, this.Vehicle);
			}
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00072630 File Offset: 0x00070830
		private Vector3 GetWalkDestination()
		{
			if (!this.Vehicle.IsVisible && this.Vehicle.CurrentParkingLot != null && this.Vehicle.CurrentParkingLot.HiddenVehicleAccessPoint != null)
			{
				return this.Vehicle.CurrentParkingLot.HiddenVehicleAccessPoint.position;
			}
			return this.Vehicle.driverEntryPoint.position;
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0007269B File Offset: 0x0007089B
		private void DriveCallback(VehicleAgent.ENavigationResult result)
		{
			if (!base.IsActive)
			{
				return;
			}
			this.isAtDestination = true;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.Park();
			base.StartCoroutine(this.<DriveCallback>g__Wait|23_0());
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x000726C8 File Offset: 0x000708C8
		private void Park()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			int randomFreeSpotIndex = this.ParkingLot.GetRandomFreeSpotIndex();
			EParkingAlignment alignment = EParkingAlignment.FrontToKerb;
			if (randomFreeSpotIndex != -1)
			{
				alignment = (this.OverrideParkingType ? this.ParkingType : this.ParkingLot.ParkingSpots[randomFreeSpotIndex].Alignment);
			}
			this.Vehicle.Park(null, new ParkData
			{
				lotGUID = this.ParkingLot.GUID,
				alignment = alignment,
				spotIndex = randomFreeSpotIndex
			}, true);
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00072747 File Offset: 0x00070947
		private EParkingAlignment GetParkingType()
		{
			if (this.OverrideParkingType)
			{
				return this.ParkingType;
			}
			return this.ParkingLot.GetRandomFreeSpot().Alignment;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00072770 File Offset: 0x00070970
		[CompilerGenerated]
		private IEnumerator <DriveCallback>g__Wait|23_0()
		{
			yield return new WaitForSeconds(1f);
			this.End();
			yield break;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0007277F File Offset: 0x0007097F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00072798 File Offset: 0x00070998
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000727B1 File Offset: 0x000709B1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x000727BF File Offset: 0x000709BF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400167A RID: 5754
		public ParkingLot ParkingLot;

		// Token: 0x0400167B RID: 5755
		public LandVehicle Vehicle;

		// Token: 0x0400167C RID: 5756
		[Header("Parking Settings")]
		public bool OverrideParkingType;

		// Token: 0x0400167D RID: 5757
		public EParkingAlignment ParkingType;

		// Token: 0x0400167E RID: 5758
		private bool isAtDestination;

		// Token: 0x0400167F RID: 5759
		private float timeInVehicle;

		// Token: 0x04001680 RID: 5760
		private float timeAtDestination;

		// Token: 0x04001681 RID: 5761
		private bool dll_Excuted;

		// Token: 0x04001682 RID: 5762
		private bool dll_Excuted;
	}
}
