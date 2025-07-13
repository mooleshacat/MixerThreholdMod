using System;
using FishNet;
using ScheduleOne.Lighting;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using ScheduleOne.Vision;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000570 RID: 1392
	public class VehiclePursuitBehaviour : Behaviour
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x0008A117 File Offset: 0x00088317
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x0008A11F File Offset: 0x0008831F
		public Player TargetPlayer { get; protected set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x0008A128 File Offset: 0x00088328
		private bool isDriving
		{
			get
			{
				return this.vehicle.OccupantNPCs[0] == base.Npc;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x0008A142 File Offset: 0x00088342
		private VehicleAgent Agent
		{
			get
			{
				return this.vehicle.Agent;
			}
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x0008A150 File Offset: 0x00088350
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0008A16F File Offset: 0x0008836F
		private void OnDestroy()
		{
			PoliceOfficer.OnPoliceVisionEvent = (Action<VisionEventReceipt>)Delegate.Remove(PoliceOfficer.OnPoliceVisionEvent, new Action<VisionEventReceipt>(this.ProcessThirdPartyVisionEvent));
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0008A191 File Offset: 0x00088391
		public void BeginAsSighted()
		{
			this.beginAsSighted = true;
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x0008A19C File Offset: 0x0008839C
		protected override void Begin()
		{
			base.Begin();
			base.Npc.awareness.VisionCone.RangeMultiplier = 1.5f;
			if (this.beginAsSighted)
			{
				this.isTargetVisible = true;
				this.initialContactMade = true;
				this.isTargetStrictlyVisible = true;
				this.SetAggressiveDriving(this.initialContactMade);
				this.DriveTo(this.GetPlayerChasePoint());
			}
			this.StartPursuit();
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0008A204 File Offset: 0x00088404
		protected override void Resume()
		{
			base.Resume();
			this.StartPursuit();
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0008A214 File Offset: 0x00088414
		protected override void Pause()
		{
			base.Pause();
			this.initialContactMade = false;
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0008A278 File Offset: 0x00088478
		protected override void End()
		{
			base.End();
			this.Disable();
			this.initialContactMade = false;
			if (this.vehicle != null)
			{
				PoliceLight componentInChildren = this.vehicle.GetComponentInChildren<PoliceLight>();
				if (componentInChildren != null)
				{
					componentInChildren.IsOn = false;
				}
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.ExitVehicle();
				this.Agent.StopNavigating();
				if (this.TargetPlayer != null)
				{
					(base.Npc as PoliceOfficer).PursuitBehaviour.AssignTarget(null, this.TargetPlayer.NetworkObject);
					(base.Npc as PoliceOfficer).PursuitBehaviour.MarkPlayerVisible();
				}
			}
			base.Npc.awareness.VisionCone.RangeMultiplier = 1f;
			base.Npc.awareness.SetAwarenessActive(true);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0008A34E File Offset: 0x0008854E
		public virtual void AssignTarget(Player target)
		{
			this.TargetPlayer = target;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0008A358 File Offset: 0x00088558
		private void StartPursuit()
		{
			if (this.vehicle == null)
			{
				Console.LogError("VehiclePursuitBehaviour: Vehicle is unassigned", null);
				this.End();
				return;
			}
			if (this.TargetPlayer == null)
			{
				Console.LogError("VehiclePursuitBehaviour: TargetPlayer is unassigned", null);
				this.End();
				return;
			}
			if (InstanceFinder.IsServer && base.Npc.CurrentVehicle != this.vehicle)
			{
				if (base.Npc.CurrentVehicle != null)
				{
					base.Npc.ExitVehicle();
				}
				base.Npc.EnterVehicle(null, this.vehicle);
			}
			PoliceLight componentInChildren = this.vehicle.GetComponentInChildren<PoliceLight>();
			if (componentInChildren != null)
			{
				componentInChildren.IsOn = true;
			}
			if (!this.isDriving)
			{
				Console.Log("Disabling awareness", null);
				base.Npc.awareness.SetAwarenessActive(false);
			}
			this.UpdateDestination();
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0008A43A File Offset: 0x0008863A
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.timeSincePursuitStart += Time.deltaTime;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x0008A45C File Offset: 0x0008865C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsTargetValid())
			{
				base.End_Networked(null);
				return;
			}
			this.CheckExitVehicle();
			if (!this.isDriving)
			{
				return;
			}
			this.SetAggressiveDriving(this.initialContactMade);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0008A497 File Offset: 0x00088697
		protected virtual void FixedUpdate()
		{
			if (!base.Active)
			{
				return;
			}
			this.CheckPlayerVisibility();
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0008A4A8 File Offset: 0x000886A8
		private void UpdateDestination()
		{
			if (!base.Active)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.Agent.NavigationCalculationInProgress)
			{
				return;
			}
			if (!this.isDriving)
			{
				return;
			}
			if (this.Agent.GetIsStuck() && this.vehicle.speed_Kmh < 4f)
			{
				base.End_Networked(null);
				return;
			}
			if (this.vehicle.VelocityCalculator.Velocity.magnitude < 1f)
			{
				this.timeStationary += 0.2f;
				if (this.timeStationary > 3f && this.timeSincePursuitStart > 10f)
				{
					base.End_Networked(null);
					return;
				}
			}
			else
			{
				this.timeStationary = 0f;
			}
			if (this.isTargetVisible)
			{
				Vector3 b;
				if (this.IsAsCloseAsPossible(this.GetPlayerChasePoint(), out b) || this.IsAsCloseAsPossible(this.TargetPlayer.Avatar.CenterPoint, out b) || Vector3.Distance(this.vehicle.transform.position, b) < 10f)
				{
					this.vehicle.ApplyHandbrake();
					this.Agent.StopNavigating();
					if (this.vehicle.speed_Kmh < 4f)
					{
						base.End_Networked(null);
						return;
					}
				}
				else if (!this.Agent.AutoDriving || Vector3.Distance(this.vehicle.Agent.TargetLocation, this.GetPlayerChasePoint()) > 10f)
				{
					this.DriveTo(this.GetPlayerChasePoint());
				}
				float num = Vector3.Distance(this.currentDriveTarget, this.TargetPlayer.CrimeData.LastKnownPosition);
				float value = Vector3.Distance(base.transform.position, this.TargetPlayer.CrimeData.LastKnownPosition);
				if (num > this.RepathDistanceThresholdMap.Evaluate(Mathf.Clamp(value, 0f, 100f)))
				{
					this.DriveTo(this.GetPlayerChasePoint());
					return;
				}
			}
			else
			{
				if (!this.Agent.AutoDriving)
				{
					Vector3 a;
					if (this.IsAsCloseAsPossible(this.TargetPlayer.CrimeData.LastKnownPosition, out a) || Vector3.Distance(a, this.vehicle.transform.position) < 10f)
					{
						if (this.vehicle.speed_Kmh < 4f)
						{
							base.End_Networked(null);
							return;
						}
					}
					else
					{
						this.DriveTo(this.TargetPlayer.CrimeData.LastKnownPosition);
					}
				}
				float num2 = Vector3.Distance(this.currentDriveTarget, this.TargetPlayer.CrimeData.LastKnownPosition);
				float value2 = Vector3.Distance(base.transform.position, this.TargetPlayer.CrimeData.LastKnownPosition);
				if (num2 > this.RepathDistanceThresholdMap.Evaluate(Mathf.Clamp(value2, 0f, 100f)))
				{
					this.DriveTo(this.TargetPlayer.CrimeData.LastKnownPosition);
				}
			}
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0008A768 File Offset: 0x00088968
		private bool IsTargetValid()
		{
			return !(this.TargetPlayer == null) && !this.TargetPlayer.IsArrested && !this.TargetPlayer.IsUnconscious && this.TargetPlayer.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None;
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0008A7B8 File Offset: 0x000889B8
		private void CheckExitVehicle()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.isDriving && this.vehicle.OccupantNPCs[0] == null)
			{
				base.End_Networked(null);
				return;
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0008A7E8 File Offset: 0x000889E8
		private Vector3 GetPlayerChasePoint()
		{
			Mathf.Min(5f, Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position));
			Mathf.Clamp01(this.TargetPlayer.VelocityCalculator.Velocity.magnitude / 8f);
			return this.TargetPlayer.Avatar.CenterPoint;
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0008A854 File Offset: 0x00088A54
		private void SetAggressiveDriving(bool aggressive)
		{
			bool flag = this.aggressiveDrivingEnabled;
			this.aggressiveDrivingEnabled = aggressive;
			if (aggressive)
			{
				this.vehicle.Agent.Flags.OverriddenSpeed = 80f;
				this.vehicle.Agent.Flags.OverriddenReverseSpeed = 20f;
				this.vehicle.Agent.Flags.OverrideSpeed = true;
				this.vehicle.Agent.Flags.AutoBrakeAtDestination = false;
				this.vehicle.Agent.Flags.IgnoreTrafficLights = true;
				this.vehicle.Agent.Flags.UseRoads = false;
				this.vehicle.Agent.Flags.ObstacleMode = DriveFlags.EObstacleMode.IgnoreOnlySquishy;
			}
			else
			{
				this.vehicle.Agent.Flags.OverrideSpeed = false;
				this.vehicle.Agent.Flags.SpeedLimitMultiplier = 1.5f;
				this.vehicle.Agent.Flags.AutoBrakeAtDestination = true;
				this.vehicle.Agent.Flags.IgnoreTrafficLights = true;
				this.vehicle.Agent.Flags.UseRoads = true;
				this.vehicle.Agent.Flags.ObstacleMode = DriveFlags.EObstacleMode.Default;
			}
			if (aggressive != flag && this.vehicle.Agent.AutoDriving)
			{
				this.vehicle.Agent.RecalculateNavigation();
			}
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x0008A9CA File Offset: 0x00088BCA
		private void DriveTo(Vector3 location)
		{
			if (!this.Agent.IsOnVehicleGraph())
			{
				this.End();
				return;
			}
			this.targetChanges++;
			this.currentDriveTarget = location;
			this.Agent.Navigate(location, null, null);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0008AA03 File Offset: 0x00088C03
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

		// Token: 0x06002197 RID: 8599 RVA: 0x00089F78 File Offset: 0x00088178
		private bool IsAsCloseAsPossible(Vector3 pos, out Vector3 closestPosition)
		{
			closestPosition = NavigationUtility.SampleVehicleGraph(pos);
			return Vector3.Distance(closestPosition, base.transform.position) < 10f;
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x0008AA3B File Offset: 0x00088C3B
		private bool IsPlayerVisible()
		{
			return base.Npc.awareness.VisionCone.IsPlayerVisible(this.TargetPlayer);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0008AA58 File Offset: 0x00088C58
		private void CheckPlayerVisibility()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.isTargetVisible)
			{
				this.playerSightedDuration += Time.fixedDeltaTime;
				if (this.IsPlayerVisible())
				{
					this.initialContactMade = true;
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(true);
					this.timeSinceLastSighting = 0f;
				}
				else
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
				}
			}
			if (!this.IsPlayerVisible())
			{
				this.playerSightedDuration = 0f;
				this.timeSinceLastSighting += Time.fixedDeltaTime;
				this.isTargetVisible = false;
				this.isTargetStrictlyVisible = false;
				if (this.timeSinceLastSighting < 6f)
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
					this.isTargetVisible = true;
					return;
				}
			}
			else
			{
				this.isTargetStrictlyVisible = true;
			}
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0008AB2C File Offset: 0x00088D2C
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject && visionEventReceipt.State == PlayerVisualState.EVisualState.SearchedFor)
			{
				this.isTargetVisible = true;
				this.initialContactMade = true;
				this.isTargetStrictlyVisible = true;
				this.DriveTo(this.GetPlayerChasePoint());
				if (this.TargetPlayer.IsOwner && this.TargetPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating)
				{
					this.TargetPlayer.CrimeData.Escalate();
				}
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x0008ABB4 File Offset: 0x00088DB4
		private void ProcessThirdPartyVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject && visionEventReceipt.State == PlayerVisualState.EVisualState.SearchedFor)
			{
				this.isTargetVisible = true;
				this.isTargetStrictlyVisible = true;
				this.DriveTo(this.GetPlayerChasePoint());
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0008AC23 File Offset: 0x00088E23
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x0008AC3C File Offset: 0x00088E3C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.VehiclePursuitBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x0008AC55 File Offset: 0x00088E55
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x0008AC64 File Offset: 0x00088E64
		protected override void dll()
		{
			base.Awake();
			if (InstanceFinder.IsOffline || InstanceFinder.IsServer)
			{
				VisionCone visionCone = base.Npc.awareness.VisionCone;
				visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
				base.InvokeRepeating("UpdateDestination", 0.5f, 0.2f);
			}
			PoliceOfficer.OnPoliceVisionEvent = (Action<VisionEventReceipt>)Delegate.Combine(PoliceOfficer.OnPoliceVisionEvent, new Action<VisionEventReceipt>(this.ProcessThirdPartyVisionEvent));
		}

		// Token: 0x040019A2 RID: 6562
		public new const float MAX_CONSECUTIVE_PATHING_FAILURES = 5f;

		// Token: 0x040019A3 RID: 6563
		public const float EXTRA_VISIBILITY_TIME = 6f;

		// Token: 0x040019A4 RID: 6564
		public const float EXIT_VEHICLE_MAX_SPEED = 4f;

		// Token: 0x040019A5 RID: 6565
		public const float CLOSE_ENOUGH_THRESHOLD = 10f;

		// Token: 0x040019A6 RID: 6566
		public const float UPDATE_FREQUENCY = 0.2f;

		// Token: 0x040019A7 RID: 6567
		public const float STATIONARY_THRESHOLD = 1f;

		// Token: 0x040019A8 RID: 6568
		public const float TIME_STATIONARY_TO_EXIT = 3f;

		// Token: 0x040019AA RID: 6570
		[Header("Settings")]
		public AnimationCurve RepathDistanceThresholdMap;

		// Token: 0x040019AB RID: 6571
		public LandVehicle vehicle;

		// Token: 0x040019AC RID: 6572
		private bool initialContactMade;

		// Token: 0x040019AD RID: 6573
		private bool aggressiveDrivingEnabled;

		// Token: 0x040019AE RID: 6574
		private bool isTargetVisible;

		// Token: 0x040019AF RID: 6575
		private bool isTargetStrictlyVisible;

		// Token: 0x040019B0 RID: 6576
		private float playerSightedDuration;

		// Token: 0x040019B1 RID: 6577
		private float timeSinceLastSighting = 10000f;

		// Token: 0x040019B2 RID: 6578
		private new int consecutivePathingFailures;

		// Token: 0x040019B3 RID: 6579
		private float timeStationary;

		// Token: 0x040019B4 RID: 6580
		private Vector3 currentDriveTarget = Vector3.zero;

		// Token: 0x040019B5 RID: 6581
		private int targetChanges;

		// Token: 0x040019B6 RID: 6582
		private float timeSincePursuitStart;

		// Token: 0x040019B7 RID: 6583
		private bool beginAsSighted;

		// Token: 0x040019B8 RID: 6584
		private bool dll_Excuted;

		// Token: 0x040019B9 RID: 6585
		private bool dll_Excuted;
	}
}
