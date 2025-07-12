using System;
using FishNet;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200056E RID: 1390
	public class VehiclePatrolBehaviour : Behaviour
	{
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x00089C44 File Offset: 0x00087E44
		private bool isDriving
		{
			get
			{
				return this.Vehicle.OccupantNPCs[0] == base.Npc;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x00089C5E File Offset: 0x00087E5E
		private VehicleAgent Agent
		{
			get
			{
				return this.Vehicle.Agent;
			}
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x00089C6B File Offset: 0x00087E6B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00089C7F File Offset: 0x00087E7F
		protected override void Begin()
		{
			base.Begin();
			this.StartPatrol();
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00089C8D File Offset: 0x00087E8D
		protected override void Resume()
		{
			base.Resume();
			this.StartPatrol();
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x00089C9C File Offset: 0x00087E9C
		protected override void Pause()
		{
			base.Pause();
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x00089D0C File Offset: 0x00087F0C
		protected override void End()
		{
			base.End();
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			(base.Npc as PoliceOfficer).BodySearchChance = 0.1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x00089D7C File Offset: 0x00087F7C
		public void SetRoute(VehiclePatrolRoute route)
		{
			this.Route = route;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00089D88 File Offset: 0x00087F88
		private void StartPatrol()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.Vehicle == null)
			{
				Console.LogError("VehiclePursuitBehaviour: Vehicle is unassigned", null);
				base.Disable_Networked(null);
				base.End_Networked(null);
				return;
			}
			if (InstanceFinder.IsServer && base.Npc.CurrentVehicle != this.Vehicle)
			{
				if (base.Npc.CurrentVehicle != null)
				{
					base.Npc.ExitVehicle();
				}
				base.Npc.EnterVehicle(null, this.Vehicle);
			}
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00089E14 File Offset: 0x00088014
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.isDriving)
			{
				return;
			}
			if (this.Agent.AutoDriving)
			{
				if (!this.Agent.NavigationCalculationInProgress && Vector3.Distance(this.Vehicle.transform.position, this.Route.Waypoints[this.CurrentWaypoint].position) < 10f)
				{
					this.CurrentWaypoint++;
					if (this.CurrentWaypoint >= this.Route.Waypoints.Length)
					{
						base.Disable_Networked(null);
						return;
					}
					this.DriveTo(this.Route.Waypoints[this.CurrentWaypoint].position);
					return;
				}
			}
			else
			{
				if (this.CurrentWaypoint >= this.Route.Waypoints.Length)
				{
					base.Disable_Networked(null);
					return;
				}
				this.DriveTo(this.Route.Waypoints[this.CurrentWaypoint].position);
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x00089F11 File Offset: 0x00088111
		private void DriveTo(Vector3 location)
		{
			if (!this.Agent.IsOnVehicleGraph())
			{
				this.End();
				return;
			}
			this.Agent.Navigate(location, null, new VehicleAgent.NavigationCallback(this.NavigationCallback));
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00089F40 File Offset: 0x00088140
		private void NavigationCallback(VehicleAgent.ENavigationResult status)
		{
			if (status == VehicleAgent.ENavigationResult.Failed)
			{
				this.consecutivePathingFailures++;
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
			if ((float)this.consecutivePathingFailures > 5f && InstanceFinder.IsServer)
			{
				base.End_Networked(null);
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00089F78 File Offset: 0x00088178
		private bool IsAsCloseAsPossible(Vector3 pos, out Vector3 closestPosition)
		{
			closestPosition = NavigationUtility.SampleVehicleGraph(pos);
			return Vector3.Distance(closestPosition, base.transform.position) < 10f;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00089FB2 File Offset: 0x000881B2
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00089FCB File Offset: 0x000881CB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePatrolBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00089FE4 File Offset: 0x000881E4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00089759 File Offset: 0x00087959
		protected override void dll()
		{
			base.Awake();
		}

		// Token: 0x04001996 RID: 6550
		public new const float MAX_CONSECUTIVE_PATHING_FAILURES = 5f;

		// Token: 0x04001997 RID: 6551
		public const float PROGRESSION_THRESHOLD = 10f;

		// Token: 0x04001998 RID: 6552
		public int CurrentWaypoint;

		// Token: 0x04001999 RID: 6553
		[Header("Settings")]
		public VehiclePatrolRoute Route;

		// Token: 0x0400199A RID: 6554
		public LandVehicle Vehicle;

		// Token: 0x0400199B RID: 6555
		private bool aggressiveDrivingEnabled = true;

		// Token: 0x0400199C RID: 6556
		private new int consecutivePathingFailures;

		// Token: 0x0400199D RID: 6557
		private bool dll_Excuted;

		// Token: 0x0400199E RID: 6558
		private bool dll_Excuted;
	}
}
