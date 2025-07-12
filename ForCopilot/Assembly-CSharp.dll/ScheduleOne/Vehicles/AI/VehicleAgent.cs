using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using Pathfinding;
using ScheduleOne.DevUtilities;
using ScheduleOne.Math;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x0200083C RID: 2108
	[RequireComponent(typeof(LandVehicle))]
	public class VehicleAgent : MonoBehaviour
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000F06E2 File Offset: 0x000EE8E2
		// (set) Token: 0x060038F5 RID: 14581 RVA: 0x000F06EA File Offset: 0x000EE8EA
		public bool KinematicMode { get; protected set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000F06F3 File Offset: 0x000EE8F3
		// (set) Token: 0x060038F7 RID: 14583 RVA: 0x000F06FB File Offset: 0x000EE8FB
		public bool AutoDriving { get; protected set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060038F8 RID: 14584 RVA: 0x000F0704 File Offset: 0x000EE904
		public bool IsReversing
		{
			get
			{
				return this.reverseCoroutine != null;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000F070F File Offset: 0x000EE90F
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x000F0717 File Offset: 0x000EE917
		public Vector3 TargetLocation { get; protected set; } = Vector3.zero;

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060038FB RID: 14587 RVA: 0x000F0720 File Offset: 0x000EE920
		protected float sampleStepSize
		{
			get
			{
				return Mathf.Lerp(this.sampleStepSizeMin, this.sampleStepSizeMax, Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed));
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060038FC RID: 14588 RVA: 0x000F074F File Offset: 0x000EE94F
		protected float turnSpeedReductionRange
		{
			get
			{
				return Mathf.Lerp(this.turnSpeedReductionMinRange, this.turnSpeedReductionMaxRange, Mathf.Clamp(this.vehicle.speed_Kmh / this.vehicle.TopSpeed, 0f, 1f));
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x000F0788 File Offset: 0x000EE988
		protected float maxSteerAngle
		{
			get
			{
				return this.vehicle.ActualMaxSteeringAngle;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060038FE RID: 14590 RVA: 0x000F0795 File Offset: 0x000EE995
		private Vector3 FrontOfVehiclePosition
		{
			get
			{
				return base.transform.position + base.transform.forward * this.vehicleLength / 2f;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060038FF RID: 14591 RVA: 0x000F07C7 File Offset: 0x000EE9C7
		public bool NavigationCalculationInProgress
		{
			get
			{
				return this.navigationCalculationRoutine != null;
			}
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x000F07D4 File Offset: 0x000EE9D4
		private void Awake()
		{
			this.vehicle = base.GetComponent<LandVehicle>();
			this.throttlePID = new PID(0.08f, 0f, 0f);
			this.steerPID = new SteerPID();
			this.speedReductionTracker = new ValueTracker(10f);
			this.PositionHistoryTracker.historyDuration = this.StuckTimeThreshold;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x000F0834 File Offset: 0x000EEA34
		protected virtual void Start()
		{
			base.InvokeRepeating("RefreshSpeedZone", 0f, 0.25f);
			base.InvokeRepeating("UpdateStuckDetection", 1f, 1f);
			base.InvokeRepeating("InfrequentUpdate", 0f, 0.033f);
			this.InitializeVehicleData();
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x000F0888 File Offset: 0x000EEA88
		private void InitializeVehicleData()
		{
			this.vehicleLength = this.vehicle.boundingBox.transform.localScale.z;
			this.vehicleWidth = this.vehicle.boundingBox.transform.localScale.x;
			Transform transform = null;
			Transform transform2 = null;
			Transform transform3 = null;
			Transform transform4 = null;
			foreach (Wheel wheel in this.vehicle.wheels)
			{
				if (transform == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).z > this.vehicle.transform.InverseTransformPoint(transform.position).z)
				{
					transform = wheel.transform;
				}
				if (transform2 == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).z < this.vehicle.transform.InverseTransformPoint(transform2.position).z)
				{
					transform2 = wheel.transform;
				}
				if (transform4 == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).x > this.vehicle.transform.InverseTransformPoint(transform4.position).x)
				{
					transform4 = wheel.transform;
				}
				if (transform3 == null || this.vehicle.transform.InverseTransformPoint(wheel.transform.position).x < this.vehicle.transform.InverseTransformPoint(transform3.position).x)
				{
					transform3 = wheel.transform;
				}
			}
			this.wheelbase = this.vehicle.transform.InverseTransformPoint(transform.position).z - this.vehicle.transform.InverseTransformPoint(transform2.position).z;
			this.wheeltrack = this.vehicle.transform.InverseTransformPoint(transform4.position).x - this.vehicle.transform.InverseTransformPoint(transform3.position).x;
			this.sweepTrack = this.sweepOrigin_FR.localPosition.x - this.sweepOrigin_FL.localPosition.x;
			this.wheelBottomOffset = -base.transform.InverseTransformPoint(this.leftWheel.transform.position).y + this.leftWheel.wheelCollider.radius;
			this.turnRadius = this.wheelbase / Mathf.Sin(this.maxSteerAngle * 0.017453292f) + 1.35f;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x000F0B70 File Offset: 0x000EED70
		protected virtual void FixedUpdate()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			this.timeSinceLastNavigationCall += Time.deltaTime;
			if (!this.AutoDriving)
			{
				return;
			}
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			if (this.KinematicMode)
			{
				if (num < 40f * QualitySettings.lodBias)
				{
					this.KinematicMode = false;
					this.vehicle.Rb.isKinematic = (this.KinematicMode || !InstanceFinder.IsHost);
					for (int i = 0; i < this.vehicle.wheels.Count; i++)
					{
						this.vehicle.wheels[i].wheelCollider.enabled = !this.vehicle.Rb.isKinematic;
					}
					if (InstanceFinder.IsHost)
					{
						this.vehicle.Rb.velocity = base.transform.forward * this.targetSpeed / 3.6f * 0.5f;
					}
				}
			}
			else if (num > 65f * QualitySettings.lodBias)
			{
				this.KinematicMode = true;
			}
			this.vehicle.Rb.isKinematic = (this.KinematicMode || !InstanceFinder.IsHost);
			for (int j = 0; j < this.vehicle.wheels.Count; j++)
			{
				this.vehicle.wheels[j].wheelCollider.enabled = !this.vehicle.Rb.isKinematic;
			}
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x000F0D0C File Offset: 0x000EEF0C
		protected void InfrequentUpdate()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			this.UpdatePursuitMode();
			if (!this.AutoDriving)
			{
				return;
			}
			this.CheckDistanceFromPath();
			this.UpdateOvertaking();
			if (this.reverseCoroutine == null)
			{
				this.UpdateSpeed();
				this.UpdateSteering();
				this.UpdateSweep();
				this.UpdateSpeedReduction();
			}
			if (this.KinematicMode)
			{
				this.UpdateKinematic(0.033f);
			}
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x000F0D74 File Offset: 0x000EEF74
		protected void LateUpdate()
		{
			if (!this.AutoDriving)
			{
				return;
			}
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (this.DEBUG_MODE)
			{
				Debug.Log("Target speed: " + this.targetSpeed.ToString());
			}
			this.throttlePID.pFactor = 0.08f;
			this.throttlePID.iFactor = 0f;
			this.throttlePID.dFactor = 0f;
			float num = this.throttlePID.Update(this.targetSpeed, this.vehicle.speed_Kmh, Time.deltaTime);
			float num2 = 0.01f;
			if (Mathf.Abs(num) < num2)
			{
				num = 0f;
			}
			this.vehicle.throttleOverride = Mathf.Clamp(num, this.throttleMin, this.throttleMax);
			this.vehicle.steerOverride = Mathf.Lerp(this.vehicle.steerOverride, this.targetSteerAngle_Normalized, Time.deltaTime * this.steerTargetFollowRate);
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x000F0E6C File Offset: 0x000EF06C
		protected void UpdateKinematic(float deltaTime)
		{
			if (!this.AutoDriving || this.path == null)
			{
				return;
			}
			float distance = this.targetSpeed * 0.2f * deltaTime;
			Vector3 referencePoint = this.vehicle.boundingBox.transform.position - this.vehicle.boundingBox.transform.up * this.vehicle.boundingBoxDimensions.y * 0.5f;
			Vector3 aheadPoint = PathUtility.GetAheadPoint(this.path, referencePoint, distance);
			if (this.DEBUG_MODE)
			{
				Debug.DrawLine(base.transform.position, aheadPoint, Color.red, 0.5f);
			}
			if (aheadPoint == Vector3.zero)
			{
				return;
			}
			base.transform.position = aheadPoint;
			int startPointIndex;
			int num;
			float num2;
			Vector3 vector = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out num2);
			Vector3 vector2 = vector + Vector3.up * 2f;
			LayerMask mask = LayerMask.GetMask(new string[]
			{
				"Default"
			});
			mask |= LayerMask.GetMask(new string[]
			{
				"Terrain"
			});
			RaycastHit[] array = Physics.RaycastAll(vector2, Vector3.down, 3f, mask, 1);
			array = (from h in array
			orderby h.distance
			select h).ToArray<RaycastHit>();
			bool flag = false;
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].collider.transform.IsChildOf(base.transform))
				{
					raycastHit = array[i];
					flag = true;
					break;
				}
			}
			if (flag)
			{
				vector = raycastHit.point;
			}
			base.transform.position = vector + base.transform.up * this.wheelBottomOffset;
			Vector3 a = Vector3.zero;
			int num3 = 3;
			for (int j = 0; j < num3; j++)
			{
				a += PathUtility.GetAheadPoint(this.path, base.transform.position, this.vehicleLength / 2f + 1f * (float)(j + 1), startPointIndex, (float)num);
			}
			a /= (float)num3;
			Vector3 normalized = (a - vector).normalized;
			base.transform.rotation = Quaternion.LookRotation(normalized, Vector3.up);
			Vector3 axleGroundHit = this.GetAxleGroundHit(true);
			Vector3 axleGroundHit2 = this.GetAxleGroundHit(false);
			normalized = (axleGroundHit - axleGroundHit2).normalized;
			base.transform.forward = normalized;
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x000F1128 File Offset: 0x000EF328
		private Vector3 GetAxleGroundHit(bool front)
		{
			Vector3 vector = this.FrontAxlePosition.position + Vector3.up * 1f;
			if (!front)
			{
				vector = this.RearAxlePosition.position + Vector3.up * 1f;
			}
			LayerMask mask = LayerMask.GetMask(new string[]
			{
				"Default"
			});
			mask |= LayerMask.GetMask(new string[]
			{
				"Terrain"
			});
			RaycastHit[] array = Physics.RaycastAll(vector, Vector3.down, 2f, mask, 1);
			array = (from h in array
			orderby h.distance
			select h).ToArray<RaycastHit>();
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].collider.transform.IsChildOf(base.transform))
				{
					return array[i].point;
				}
			}
			if (front)
			{
				return this.FrontAxlePosition.position - base.transform.up * this.wheelBottomOffset;
			}
			return this.RearAxlePosition.position - base.transform.up * this.wheelBottomOffset;
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x000F1280 File Offset: 0x000EF480
		private void UpdateSweep()
		{
			if (this.KinematicMode)
			{
				return;
			}
			if (Mathf.Abs(this.vehicle.speed_Kmh) > 5f)
			{
				this.sweepTestFailedTime = 0f;
				return;
			}
			if (Mathf.Abs(this.targetSteerAngle_Normalized) * this.maxSteerAngle > 5f)
			{
				float num = 1.5f;
				float num2;
				Vector3 vector;
				bool flag = this.SweepTurn(VehicleAgent.ESweepType.FR, Mathf.Sign(this.targetSteerAngle_Normalized) * 30f, false, out num2, out vector, this.targetSteerAngle_Normalized * this.maxSteerAngle);
				float num3;
				Vector3 vector2;
				bool flag2 = this.SweepTurn(VehicleAgent.ESweepType.FL, Mathf.Sign(this.targetSteerAngle_Normalized) * 30f, false, out num3, out vector2, this.targetSteerAngle_Normalized * this.maxSteerAngle);
				if ((!flag || num2 >= num) && (!flag2 || num3 >= num))
				{
					this.sweepTestFailedTime = 0f;
					return;
				}
				this.sweepTestFailedTime += Time.deltaTime;
				if ((double)this.sweepTestFailedTime > 0.25)
				{
					this.StartReverse();
					this.sweepTestFailedTime = 0f;
					return;
				}
			}
			else
			{
				this.sweepTestFailedTime = 0f;
			}
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x000F138C File Offset: 0x000EF58C
		private void UpdateSpeedReduction()
		{
			if (this.path == null)
			{
				return;
			}
			if (this.path != null && Vector3.Distance(base.transform.position, this.path.vectorPath[this.path.vectorPath.Count - 1]) < 3f)
			{
				this.path = null;
				this.vehicle.overrideControls = false;
				this.vehicle.steerOverride = 0f;
				this.vehicle.throttleOverride = 0f;
				this.AutoDriving = false;
				this.vehicle.ResetMaxSteerAngle();
				if (this.storedNavigationCallback != null)
				{
					this.storedNavigationCallback(VehicleAgent.ENavigationResult.Complete);
					this.storedNavigationCallback = null;
				}
				return;
			}
			if (this.KinematicMode)
			{
				return;
			}
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			float num2 = 1f;
			float num3 = 1f;
			float num4 = this.targetSpeed;
			if (this.Flags.TurnBasedSpeedReduction)
			{
				float num5 = Mathf.Max(PathUtility.CalculateAngleChangeOverPath(this.path, startPointIndex, pointLerp, this.turnSpeedReductionRange), this.targetSteerAngle_Normalized * this.maxSteerAngle);
				if (num5 > this.minTurnSpeedReductionAngleThreshold)
				{
					num4 = Mathf.Lerp(num4, this.minTurningSpeed, Mathf.Clamp(num5 / this.turnSpeedReductionDivisor, 0f, 1f));
				}
			}
			if (this.Flags.ObstacleMode != DriveFlags.EObstacleMode.IgnoreAll)
			{
				float a;
				Vector3 vector;
				this.BetterSweepTurn(VehicleAgent.ESweepType.FL, this.vehicle.SyncAccessor_currentSteerAngle, false, this.sensor_FM.checkMask, out a, out vector);
				float b;
				Vector3 vector2;
				this.BetterSweepTurn(VehicleAgent.ESweepType.FR, this.vehicle.SyncAccessor_currentSteerAngle, false, this.sensor_FM.checkMask, out b, out vector2);
				float num6 = Mathf.Min(a, b);
				float num7 = Mathf.Lerp(1.5f, 15f, Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed));
				if (num6 < num7)
				{
					if (this.DEBUG_MODE)
					{
						Console.Log("Obstacle detected at " + num6.ToString() + "m:", null);
					}
					num2 = Mathf.Clamp((num6 - 1.5f) / (num7 - 1.5f), 0.002f, 1f);
				}
			}
			if (this.Flags.AutoBrakeAtDestination && this.path != null)
			{
				float num8 = Vector3.Distance(base.transform.position, this.path.vectorPath[this.path.vectorPath.Count - 1]);
				if (num8 < 8f)
				{
					num3 = Mathf.Clamp(num8 / 8f, 0f, 1f);
					if (num8 < 3f)
					{
						num3 = 0f;
					}
					if (num3 < 0.2f)
					{
						this.vehicle.ApplyHandbrake();
					}
				}
			}
			if (this.DEBUG_MODE)
			{
				Debug.Log("Obstacle speed multiplier: " + num2.ToString());
				Debug.Log("Destination speed multiplier: " + num3.ToString());
				Debug.Log("Turn target speed: " + num4.ToString());
			}
			float num9 = num2 * num3;
			this.speedReductionTracker.SubmitValue(num9);
			this.targetSpeed *= num9;
			this.targetSpeed = Mathf.Min(this.targetSpeed, num4);
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x000F16D0 File Offset: 0x000EF8D0
		private void UpdatePursuitMode()
		{
			if (!this.PursuitModeEnabled || this.PursuitTarget == null)
			{
				return;
			}
			if (Vector3.Distance(this.PursuitTarget.position, this.PursuitTargetLastPosition) > this.PursuitDistanceUpdateThreshold)
			{
				this.PursuitTargetLastPosition = this.PursuitTarget.position;
				this.Navigate(this.PursuitTarget.position, null, null);
			}
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x000F1738 File Offset: 0x000EF938
		private void UpdateStuckDetection()
		{
			if (!this.AutoDriving)
			{
				this.PositionHistoryTracker.ClearHistory();
				return;
			}
			if (!this.Flags.StuckDetection)
			{
				return;
			}
			if (this.speedReductionTracker.RecordedHistoryLength() < this.StuckTimeThreshold)
			{
				return;
			}
			if (this.speedReductionTracker.GetLowestValue() < 0.1f)
			{
				return;
			}
			if (this.PositionHistoryTracker.RecordedTime >= this.StuckTimeThreshold)
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.StuckSamples; i++)
				{
					vector += this.PositionHistoryTracker.GetPositionXSecondsAgo(this.StuckTimeThreshold / (float)this.StuckSamples * (float)(i + 1));
				}
				vector /= (float)this.StuckSamples;
				if (Vector3.Distance(base.transform.position, vector) < this.StuckDistanceThreshold)
				{
					if (this.DEBUG_MODE)
					{
						Console.LogWarning("Vehicle stuck", null);
					}
					if (this.IsOnVehicleGraph())
					{
						this.Teleporter.MoveToRoadNetwork(true);
					}
					else
					{
						this.Teleporter.MoveToGraph(true);
					}
					this.PositionHistoryTracker.ClearHistory();
				}
			}
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x000F1848 File Offset: 0x000EFA48
		private void CheckDistanceFromPath()
		{
			if (this.timeSinceLastNavigationCall < VehicleAgent.MinRenavigationRate)
			{
				return;
			}
			if (this.KinematicMode)
			{
				return;
			}
			if (this.path != null)
			{
				int num;
				int num2;
				float num3;
				Vector3 vector = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out num, out num2, out num3);
				vector += this.GetPathLateralDirection() * this.lateralOffset;
				if (Vector3.Distance(base.transform.position, vector) > (this.IsReversing ? 8f : 6f))
				{
					if (this.DEBUG_MODE)
					{
						Console.Log("Too far from path! Re-navigating.", null);
						Debug.DrawLine(base.transform.position, vector, Color.red, 2f);
					}
					this.Navigate(this.TargetLocation, this.currentNavigationSettings, this.storedNavigationCallback);
				}
			}
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x000F191C File Offset: 0x000EFB1C
		private void UpdateOvertaking()
		{
			this.lateralOffset = 0f;
			if (this.sensor_FM.obstruction != null && this.sensor_FM.obstruction.GetComponentInParent<LandVehicle>() != null && this.sensor_FM.obstructionDistance < 8f)
			{
				float num = this.sensor_FM.obstructionDistance / 8f;
			}
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x000F1984 File Offset: 0x000EFB84
		protected virtual void RefreshSpeedZone()
		{
			List<SpeedZone> speedZones = SpeedZone.GetSpeedZones(base.transform.position);
			if (speedZones.Count > 0)
			{
				this.currentSpeedZone = speedZones[0];
				return;
			}
			this.currentSpeedZone = null;
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x000F19C0 File Offset: 0x000EFBC0
		protected virtual void UpdateSpeed()
		{
			if (this.path == null)
			{
				this.targetSpeed = 0f;
				return;
			}
			if (this.currentSpeedZone != null)
			{
				this.targetSpeed = this.currentSpeedZone.speed * this.Flags.SpeedLimitMultiplier;
			}
			else
			{
				this.targetSpeed = VehicleAgent.UnmarkedSpeed * this.Flags.SpeedLimitMultiplier;
			}
			if (this.Flags.OverrideSpeed)
			{
				this.targetSpeed = this.Flags.OverriddenSpeed;
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000F1A44 File Offset: 0x000EFC44
		protected void UpdateSteering()
		{
			if (this.path == null || this.path.vectorPath.Count < 2 || this.KinematicMode)
			{
				this.targetSteerAngle_Normalized = 0f;
				return;
			}
			int num;
			int num2;
			float num3;
			Vector3 closestPointOnPath = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out num, out num2, out num3);
			Vector3 vector = PathUtility.GetAheadPoint(this.path, base.transform.position, this.vehicleLength / 2f + this.sampleStepSize);
			vector = closestPointOnPath;
			Vector3 vector2 = PathUtility.GetAverageAheadPoint(this.path, base.transform.position, this.aheadPointSamples, this.sampleStepSize);
			vector2 = PathUtility.GetAheadPoint(this.path, base.transform.position, 0.5f);
			Vector3 normalized = (vector2 - vector).normalized;
			Debug.DrawLine(base.transform.position, vector, Color.yellow, 0.5f);
			Debug.DrawLine(base.transform.position, vector2, Color.magenta, 0.5f);
			float error = PathUtility.CalculateCTE(this.CTE_Origin.position + base.transform.forward * Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed) * (this.vehicle.TopSpeed * 0.2778f * 0.3f), base.transform, vector, vector2, this.path);
			float num4 = Mathf.Clamp(this.steerPID.GetNewValue(error, new PID_Parameters(40f, 5f, 10f)) / this.maxSteerAngle, -1f, 1f);
			float num5 = Vector3.SignedAngle(base.transform.forward, normalized, Vector3.up);
			float num6 = 45f;
			if (Mathf.Abs(num5) > 45f)
			{
				num4 += Mathf.Clamp01(Mathf.Abs(num5 - num6) / (180f - num6)) * Mathf.Sign(num5);
			}
			this.targetSteerAngle_Normalized = Mathf.Clamp(num4, -1f, 1f);
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x000F1C64 File Offset: 0x000EFE64
		public void Navigate(Vector3 location, NavigationSettings settings = null, VehicleAgent.NavigationCallback callback = null)
		{
			if (this.navigationCalculationRoutine != null)
			{
				Console.LogWarning("Navigate called before previous navigation calculation was complete!", null);
				base.StopCoroutine(this.navigationCalculationRoutine);
			}
			if (this.GetIsStuck())
			{
				Console.LogWarning("Navigate called but vehilc is stuck! Navigation will still be attemped", null);
			}
			if (this.reverseCoroutine != null)
			{
				this.StopReversing();
			}
			if (!InstanceFinder.IsHost)
			{
				return;
			}
			this.path = null;
			this.timeSinceLastNavigationCall = 0f;
			if (settings == null)
			{
				settings = new NavigationSettings();
			}
			if (this.GetDistanceFromVehicleGraph() > 6f)
			{
				if (settings.ensureProximityToGraph)
				{
					this.Teleporter.MoveToGraph(true);
				}
				else if (callback != null)
				{
					callback(VehicleAgent.ENavigationResult.Failed);
					return;
				}
			}
			this.vehicle.Rb.isKinematic = this.KinematicMode;
			this.vehicle.Rb.interpolation = 1;
			if (this.DEBUG_MODE)
			{
				Console.Log("Navigate called...", null);
			}
			this.TargetLocation = location;
			this.AutoDriving = true;
			this.storedNavigationCallback = callback;
			this.vehicle.OverrideMaxSteerAngle(35f);
			this.vehicle.overrideControls = true;
			this.currentNavigationSettings = settings;
			this.navigationCalculationRoutine = NavigationUtility.CalculatePath(this.FrontOfVehiclePosition, this.TargetLocation, this.currentNavigationSettings, this.Flags, this.generalSeeker, this.roadSeeker, new NavigationUtility.NavigationCalculationCallback(this.NavigationCalculationCallback));
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x000F1DB0 File Offset: 0x000EFFB0
		private void NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath _path)
		{
			this.navigationCalculationRoutine = null;
			if (result == NavigationUtility.ENavigationCalculationResult.Failed)
			{
				if (this.storedNavigationCallback != null)
				{
					this.storedNavigationCallback(VehicleAgent.ENavigationResult.Failed);
				}
				this.EndDriving();
				return;
			}
			this.path = _path;
			this.path.InitializePath();
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x000F1DEA File Offset: 0x000EFFEA
		private void EndDriving()
		{
			this.AutoDriving = false;
			this.vehicle.ResetMaxSteerAngle();
			this.path = null;
			this.storedNavigationCallback = null;
			this.vehicle.overrideControls = false;
			this.currentNavigationSettings = null;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x000F1E1F File Offset: 0x000F001F
		public void StopNavigating()
		{
			if (this.navigationCalculationRoutine != null)
			{
				base.StopCoroutine(this.navigationCalculationRoutine);
			}
			if (this.storedNavigationCallback != null)
			{
				this.storedNavigationCallback(VehicleAgent.ENavigationResult.Stopped);
			}
			this.EndDriving();
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x000F1E4F File Offset: 0x000F004F
		public void RecalculateNavigation()
		{
			if (!this.AutoDriving)
			{
				return;
			}
			this.Navigate(this.TargetLocation, this.currentNavigationSettings, this.storedNavigationCallback);
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x000F1E74 File Offset: 0x000F0074
		public bool SweepTurn(VehicleAgent.ESweepType sweep, float sweepAngle, bool reverse, out float hitDistance, out Vector3 hitPoint, float steerAngle = 0f)
		{
			hitDistance = float.MaxValue;
			hitPoint = Vector3.zero;
			if (steerAngle == 0f)
			{
				steerAngle = this.maxSteerAngle;
			}
			steerAngle = Mathf.Abs(steerAngle);
			float num = Mathf.Sign(sweepAngle);
			this.FrontAxlePosition.localEulerAngles = new Vector3(0f, num * steerAngle, 0f);
			float num2 = this.turnRadius;
			Vector3 vector = Vector3.zero;
			Vector3 castStart = Vector3.zero;
			if (sweepAngle > 0f)
			{
				vector = this.sweepOrigin_FL.position + this.FrontAxlePosition.right * this.turnRadius;
			}
			else
			{
				vector = this.sweepOrigin_FR.position - this.FrontAxlePosition.right * this.turnRadius;
			}
			switch (sweep)
			{
			case VehicleAgent.ESweepType.FL:
				castStart = this.sweepOrigin_FL.position;
				break;
			case VehicleAgent.ESweepType.FR:
				castStart = this.sweepOrigin_FR.position;
				break;
			case VehicleAgent.ESweepType.RL:
				castStart = this.sweepOrigin_RL.position;
				break;
			case VehicleAgent.ESweepType.RR:
				castStart = this.sweepOrigin_RR.position;
				break;
			}
			Vector3 normalized = (castStart - vector).normalized;
			Vector3 a = Quaternion.AngleAxis(90f * num, base.transform.up) * normalized;
			num2 = Vector3.Distance(vector, castStart);
			float num3 = 0f;
			float num4 = 0f;
			Func<RaycastHit, float> <>9__0;
			RaycastHit raycastHit;
			for (;;)
			{
				float num5 = num3;
				float num6 = Mathf.Clamp(num5 + Mathf.Abs(15f), 0f, Mathf.Abs(sweepAngle));
				num3 += num6 - num5;
				float d = num2 * Mathf.Cos(num6 * 0.017453292f);
				float d2 = num2 * Mathf.Sin(num6 * 0.017453292f);
				Vector3 vector2 = vector;
				vector2 += a * d2 * (reverse ? -1f : 1f);
				vector2 += normalized * d;
				RaycastHit[] array = Physics.SphereCastAll(castStart, 0.1f, (vector2 - castStart).normalized, Vector3.Distance(castStart, vector2), this.sweepMask, 1);
				if (array.Length != 0)
				{
					IEnumerable<RaycastHit> source = array;
					Func<RaycastHit, float> keySelector;
					if ((keySelector = <>9__0) == null)
					{
						keySelector = (<>9__0 = ((RaycastHit x) => Vector3.Distance(castStart, x.point)));
					}
					array = source.OrderBy(keySelector).ToArray<RaycastHit>();
				}
				raycastHit = default(RaycastHit);
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].collider.transform.IsChildOf(base.transform))
					{
						raycastHit = array[i];
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num4 += Vector3.Distance(castStart, vector2);
				castStart = vector2;
				if (num3 >= Mathf.Abs(sweepAngle))
				{
					return false;
				}
			}
			if (raycastHit.point == Vector3.zero)
			{
				raycastHit.point = castStart;
			}
			num4 += Vector3.Distance(castStart, raycastHit.point);
			hitDistance = num4;
			hitPoint = raycastHit.point;
			return true;
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x000F21C4 File Offset: 0x000F03C4
		public void BetterSweepTurn(VehicleAgent.ESweepType sweep, float steerAngle, bool reverse, LayerMask mask, out float hitDistance, out Vector3 hitPoint)
		{
			hitDistance = float.MaxValue;
			hitPoint = Vector3.zero;
			float num = Mathf.Sign(steerAngle);
			this.FrontAxlePosition.localEulerAngles = new Vector3(0f, steerAngle, 0f);
			Vector3 vector = Vector3.zero;
			Vector3 castStart = Vector3.zero;
			float num2 = Mathf.Clamp(this.wheelbase / Mathf.Sin(steerAngle * 0.017453292f), -100f, 100f);
			vector = this.sweepOrigin_FL.position + this.FrontAxlePosition.right * num2;
			switch (sweep)
			{
			case VehicleAgent.ESweepType.FL:
				castStart = this.sweepOrigin_FL.position;
				break;
			case VehicleAgent.ESweepType.FR:
				castStart = this.sweepOrigin_FR.position;
				break;
			case VehicleAgent.ESweepType.RL:
				castStart = this.sweepOrigin_RL.position;
				break;
			case VehicleAgent.ESweepType.RR:
				castStart = this.sweepOrigin_RR.position;
				break;
			default:
				Console.LogWarning("Invalid sweep type: " + sweep.ToString(), null);
				break;
			}
			Debug.DrawLine(castStart, vector, Color.white);
			Vector3 normalized = (castStart - vector).normalized;
			Vector3 a = Quaternion.AngleAxis(90f * num, base.transform.up) * normalized;
			num2 = Vector3.Distance(vector, castStart);
			float num3 = 0f;
			int num4 = 6;
			float num5 = Mathf.Clamp(Mathf.Abs(steerAngle), 5f, 30f);
			Func<RaycastHit, float> <>9__0;
			for (float num6 = 0f; num6 < (float)num4; num6 += 1f)
			{
				float num7 = num5 * (num6 + 1f);
				float d = num2 * Mathf.Cos(num7 * 0.017453292f);
				float d2 = num2 * Mathf.Sin(num7 * 0.017453292f);
				Vector3 vector2 = vector;
				vector2 += a * d2 * (reverse ? -1f : 1f);
				vector2 += normalized * d;
				RaycastHit[] array = Physics.SphereCastAll(castStart, this.sensor_FM.checkRadius, (vector2 - castStart).normalized, Vector3.Distance(castStart, vector2), mask);
				if (array.Length != 0)
				{
					IEnumerable<RaycastHit> source = array;
					Func<RaycastHit, float> keySelector;
					if ((keySelector = <>9__0) == null)
					{
						keySelector = (<>9__0 = ((RaycastHit x) => Vector3.Distance(castStart, x.point)));
					}
					array = source.OrderBy(keySelector).ToArray<RaycastHit>();
				}
				RaycastHit raycastHit = default(RaycastHit);
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].collider.transform.IsChildOf(base.transform) && !array[i].collider.transform.IsChildOf(this.vehicle.HumanoidColliderContainer.transform.transform))
					{
						if (this.Flags.IgnoreTrafficLights)
						{
							VehicleObstacle componentInParent = array[i].transform.GetComponentInParent<VehicleObstacle>();
							if (componentInParent != null && componentInParent.type == VehicleObstacle.EObstacleType.TrafficLight)
							{
								goto IL_3EF;
							}
						}
						VehicleObstacle componentInParent2 = array[i].collider.transform.GetComponentInParent<VehicleObstacle>();
						if (componentInParent2 != null)
						{
							if (!componentInParent2.twoSided && Vector3.Angle(-componentInParent2.transform.forward, base.transform.forward) > 90f)
							{
								goto IL_3EF;
							}
						}
						else if (array[i].collider.isTrigger)
						{
							goto IL_3EF;
						}
						if (this.Flags.ObstacleMode != DriveFlags.EObstacleMode.IgnoreOnlySquishy || (!(array[i].transform.GetComponentInParent<LandVehicle>() != null) && !(array[i].transform.GetComponentInParent<Player>() != null) && !(array[i].transform.GetComponentInParent<NPC>() != null)))
						{
							raycastHit = array[i];
							flag = true;
							break;
						}
					}
					IL_3EF:;
				}
				if (flag)
				{
					if (raycastHit.point == Vector3.zero)
					{
						raycastHit.point = castStart;
					}
					num3 += Vector3.Distance(castStart, raycastHit.point);
					hitDistance = num3;
					hitPoint = raycastHit.point;
					Debug.DrawLine(castStart, raycastHit.point, Color.red);
					return;
				}
				num3 += Vector3.Distance(castStart, vector2);
				Debug.DrawLine(castStart, vector2, (num6 % 2f == 0f) ? Color.green : Color.cyan);
				castStart = vector2;
			}
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x000F268D File Offset: 0x000F088D
		public void StartReverse()
		{
			if (this.reverseCoroutine != null)
			{
				this.StopReversing();
			}
			this.reverseCoroutine = base.StartCoroutine(this.Reverse());
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x000F26AF File Offset: 0x000F08AF
		public IEnumerator Reverse()
		{
			if (this.DEBUG_MODE)
			{
				Console.Log("Starting reverse operation", null);
			}
			this.targetSpeed = 0f;
			this.targetSteerAngle_Normalized = 0f;
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			float num2 = 3f;
			Vector3 futureTarget = Vector3.zero;
			Vector3 zero = Vector3.zero;
			int num3 = 0;
			Vector3 vector;
			Vector3 a;
			for (;;)
			{
				vector = Vector3.zero;
				a = Vector3.zero;
				for (int i = 1; i <= this.aheadPointSamples; i++)
				{
					vector += PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, num2 + (float)i * this.sampleStepSizeMin);
					if (i == this.aheadPointSamples)
					{
						a = PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, num2 + (float)i * this.sampleStepSizeMin + 1f);
					}
				}
				vector /= (float)this.aheadPointSamples;
				if (Mathf.Abs(base.transform.InverseTransformPoint(vector).x) > 1f)
				{
					break;
				}
				if (num3 >= 25)
				{
					goto Block_5;
				}
				num3++;
				num2 += 1f;
			}
			futureTarget = vector;
			a - futureTarget;
			float steerAngleNormal = -Mathf.Sign(base.transform.InverseTransformPoint(futureTarget).x);
			yield return new WaitForSeconds(1f);
			VehicleAgent.ESweepType frontWheel = VehicleAgent.ESweepType.FL;
			if (steerAngleNormal < 0f)
			{
				frontWheel = VehicleAgent.ESweepType.FR;
			}
			float num4 = 10f;
			float num5 = 90f;
			Vector3 vector2 = futureTarget - base.transform.position;
			vector2.y = 0f;
			vector2.Normalize();
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			float sweepAngle = num4 + (num5 - num4) * Mathf.Clamp(Vector3.Angle(forward, vector2) / 90f, 0f, 1f);
			if (this.DEBUG_MODE)
			{
				Console.Log("Beginning straight reverse...", null);
			}
			float reverseSweepDistanceMin = 1.25f;
			this.targetSpeed = (this.Flags.OverrideSpeed ? (-this.Flags.OverriddenReverseSpeed) : (-VehicleAgent.ReverseSpeed));
			bool canBeginSwing = false;
			while (!canBeginSwing)
			{
				yield return new WaitForEndOfFrame();
				float num6 = 0f;
				float num7 = 0f;
				float num8 = 0f;
				Vector3 zero2 = Vector3.zero;
				Vector3 zero3 = Vector3.zero;
				if (this.SweepTurn(frontWheel, sweepAngle * steerAngleNormal, true, out num6, out zero2, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RL, sweepAngle * steerAngleNormal, true, out num7, out zero3, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RR, sweepAngle * steerAngleNormal, true, out num8, out zero3, 0f))
				{
					float num9 = 2f;
					if (this.sensor_RR.obstructionDistance < num9 || this.sensor_RL.obstructionDistance < num9)
					{
						if (this.DEBUG_MODE)
						{
							Console.Log("Continued straight reversing will result in collision; starting turn", null);
						}
						canBeginSwing = true;
					}
				}
				else if (base.transform.InverseTransformPoint(futureTarget).z > -this.vehicleLength)
				{
					canBeginSwing = true;
				}
			}
			if (this.DEBUG_MODE)
			{
				Console.Log("Beginning swing...", null);
			}
			this.targetSteerAngle_Normalized = steerAngleNormal;
			Vector3 faceTarget = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			Vector3 normalized = (PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, 0.5f) - faceTarget).normalized;
			faceTarget += normalized * this.vehicleLength / 2f;
			bool continueReversing = true;
			while (continueReversing)
			{
				yield return new WaitForEndOfFrame();
				if (this.path == null)
				{
					continueReversing = false;
				}
				else
				{
					vector2 = faceTarget - base.transform.position;
					vector2.y = 0f;
					vector2.Normalize();
					forward = base.transform.forward;
					forward.y = 0f;
					Debug.DrawLine(base.transform.position, faceTarget, Color.magenta);
					Debug.DrawLine(base.transform.position, base.transform.position + forward * 5f, Color.cyan);
					if (Vector3.Angle(vector2, forward) < 20f)
					{
						continueReversing = false;
					}
					float maxValue = float.MaxValue;
					float maxValue2 = float.MaxValue;
					float maxValue3 = float.MaxValue;
					Vector3 vector3;
					if ((this.SweepTurn(frontWheel, 30f * steerAngleNormal, true, out maxValue, out vector3, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RL, 30f * steerAngleNormal, true, out maxValue2, out vector3, 0f) || this.SweepTurn(VehicleAgent.ESweepType.RR, 30f * steerAngleNormal, true, out maxValue3, out vector3, 0f)) && (maxValue < reverseSweepDistanceMin || maxValue2 < reverseSweepDistanceMin || maxValue3 < reverseSweepDistanceMin))
					{
						continueReversing = false;
						if (this.DEBUG_MODE)
						{
							Console.Log("Reverse sweep obstructed", null);
						}
					}
				}
			}
			this.targetSpeed = 0f;
			yield return new WaitUntil(() => this.vehicle.speed_Kmh >= -1f);
			if (this.DEBUG_MODE)
			{
				Console.Log("Reverse finished", null);
			}
			this.reverseCoroutine = null;
			yield break;
			Block_5:
			this.reverseCoroutine = null;
			Console.LogWarning("Can't calculate average ahead point!", null);
			yield break;
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x000F26BE File Offset: 0x000F08BE
		private void StopReversing()
		{
			if (this.DEBUG_MODE)
			{
				Console.Log("Reverse stop", null);
			}
			if (this.reverseCoroutine != null)
			{
				base.StopCoroutine(this.reverseCoroutine);
				this.reverseCoroutine = null;
				this.targetSpeed = 0f;
			}
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x000F26FC File Offset: 0x000F08FC
		private Collider GetClosestForwardObstruction(out float obstructionDist)
		{
			Collider result = null;
			obstructionDist = float.MaxValue;
			foreach (Sensor sensor in new List<Sensor>
			{
				this.sensor_FL,
				this.sensor_FM,
				this.sensor_FR
			})
			{
				if (sensor.obstruction != null)
				{
					if (this.Flags.IgnoreTrafficLights)
					{
						VehicleObstacle componentInParent = sensor.obstruction.GetComponentInParent<VehicleObstacle>();
						if (componentInParent != null && componentInParent.type == VehicleObstacle.EObstacleType.TrafficLight)
						{
							continue;
						}
					}
					if ((this.Flags.ObstacleMode != DriveFlags.EObstacleMode.IgnoreOnlySquishy || (!(sensor.obstruction.GetComponentInParent<LandVehicle>() != null) && !(sensor.obstruction.GetComponentInParent<Player>() != null) && !(sensor.obstruction.GetComponentInParent<NPC>() != null))) && sensor.obstructionDistance < obstructionDist)
					{
						result = sensor.obstruction;
						obstructionDist = sensor.obstructionDistance;
					}
				}
			}
			return result;
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x000F2818 File Offset: 0x000F0A18
		public bool IsOnVehicleGraph()
		{
			return this.GetDistanceFromVehicleGraph() < 2.5f;
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x000F2828 File Offset: 0x000F0A28
		private float GetDistanceFromVehicleGraph()
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("General Vehicle Graph");
			return Vector3.Distance(AstarPath.active.GetNearest(base.transform.position, nnconstraint).position, base.transform.position - base.transform.up * this.vehicle.boundingBoxDimensions.y / 2f);
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x000F28A8 File Offset: 0x000F0AA8
		private Vector3 GetPathLateralDirection()
		{
			if (this.path == null)
			{
				Console.LogWarning("Path is null!", null);
				return Vector3.zero;
			}
			int startPointIndex;
			int num;
			float pointLerp;
			Vector3 closestPointOnPath = PathUtility.GetClosestPointOnPath(this.path, base.transform.position, out startPointIndex, out num, out pointLerp);
			Vector3 pointAheadOfPathPoint = PathUtility.GetPointAheadOfPathPoint(this.path, startPointIndex, pointLerp, 0.01f);
			return Quaternion.AngleAxis(90f, base.transform.up) * (pointAheadOfPathPoint - closestPointOnPath).normalized;
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x000F292C File Offset: 0x000F0B2C
		public bool GetIsStuck()
		{
			if (this.speedReductionTracker.RecordedHistoryLength() < this.StuckTimeThreshold)
			{
				return false;
			}
			if (this.speedReductionTracker.GetLowestValue() < 0.1f)
			{
				return false;
			}
			if (this.PositionHistoryTracker.RecordedTime >= this.StuckTimeThreshold)
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.StuckSamples; i++)
				{
					vector += this.PositionHistoryTracker.GetPositionXSecondsAgo(this.StuckTimeThreshold / (float)this.StuckSamples * (float)(i + 1));
				}
				vector /= (float)this.StuckSamples;
				if (Vector3.Distance(base.transform.position, vector) < this.StuckDistanceThreshold)
				{
					if (this.DEBUG_MODE)
					{
						Console.LogWarning("Vehicle stuck", null);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x040028EE RID: 10478
		public const string VehicleGraphName = "General Vehicle Graph";

		// Token: 0x040028EF RID: 10479
		public const string RoadGraphName = "Road Nodes";

		// Token: 0x040028F0 RID: 10480
		public const float MaxDistanceFromPath = 6f;

		// Token: 0x040028F1 RID: 10481
		public const float MaxDistanceFromPathWhenReversing = 8f;

		// Token: 0x040028F2 RID: 10482
		public static Vector3 MainGraphSamplePoint = new Vector3(31.5f, 0f, 51f);

		// Token: 0x040028F3 RID: 10483
		public static float MinRenavigationRate = 2f;

		// Token: 0x040028F4 RID: 10484
		public const float Steer_P = 40f;

		// Token: 0x040028F5 RID: 10485
		public const float Steer_I = 5f;

		// Token: 0x040028F6 RID: 10486
		public const float Steer_D = 10f;

		// Token: 0x040028F7 RID: 10487
		public const float Throttle_P = 0.08f;

		// Token: 0x040028F8 RID: 10488
		public const float Throttle_I = 0f;

		// Token: 0x040028F9 RID: 10489
		public const float Throttle_D = 0f;

		// Token: 0x040028FA RID: 10490
		public const float Steer_Rate = 135f;

		// Token: 0x040028FB RID: 10491
		public const float MaxAxlePositionShift = 3f;

		// Token: 0x040028FC RID: 10492
		public const float OBSTACLE_MIN_RANGE = 1.5f;

		// Token: 0x040028FD RID: 10493
		public const float OBSTACLE_MAX_RANGE = 15f;

		// Token: 0x040028FE RID: 10494
		public const float MAX_STEER_ANGLE_OVERRIDE = 35f;

		// Token: 0x040028FF RID: 10495
		public const float KINEMATIC_MODE_MIN_DISTANCE = 40f;

		// Token: 0x04002900 RID: 10496
		public const float INFREQUENT_UPDATE_RATE = 0.033f;

		// Token: 0x04002901 RID: 10497
		public bool DEBUG_MODE;

		// Token: 0x04002905 RID: 10501
		public DriveFlags Flags;

		// Token: 0x04002906 RID: 10502
		[Header("Seekers")]
		[SerializeField]
		protected Seeker roadSeeker;

		// Token: 0x04002907 RID: 10503
		[SerializeField]
		protected Seeker generalSeeker;

		// Token: 0x04002908 RID: 10504
		[Header("References")]
		[SerializeField]
		protected Transform CTE_Origin;

		// Token: 0x04002909 RID: 10505
		[SerializeField]
		protected Transform FrontAxlePosition;

		// Token: 0x0400290A RID: 10506
		[SerializeField]
		protected Transform RearAxlePosition;

		// Token: 0x0400290B RID: 10507
		[Header("Sensors")]
		[SerializeField]
		protected Sensor sensor_FL;

		// Token: 0x0400290C RID: 10508
		[SerializeField]
		protected Sensor sensor_FM;

		// Token: 0x0400290D RID: 10509
		[SerializeField]
		protected Sensor sensor_FR;

		// Token: 0x0400290E RID: 10510
		[SerializeField]
		protected Sensor sensor_RR;

		// Token: 0x0400290F RID: 10511
		[SerializeField]
		protected Sensor sensor_RL;

		// Token: 0x04002910 RID: 10512
		[Header("Sweeping")]
		[SerializeField]
		protected LayerMask sweepMask;

		// Token: 0x04002911 RID: 10513
		[SerializeField]
		protected Transform sweepOrigin_FL;

		// Token: 0x04002912 RID: 10514
		[SerializeField]
		protected Transform sweepOrigin_FR;

		// Token: 0x04002913 RID: 10515
		[SerializeField]
		protected Transform sweepOrigin_RL;

		// Token: 0x04002914 RID: 10516
		[SerializeField]
		protected Transform sweepOrigin_RR;

		// Token: 0x04002915 RID: 10517
		[SerializeField]
		protected Wheel leftWheel;

		// Token: 0x04002916 RID: 10518
		[SerializeField]
		protected Wheel rightWheel;

		// Token: 0x04002917 RID: 10519
		protected const float sweepSegment = 15f;

		// Token: 0x04002918 RID: 10520
		[Header("Path following")]
		[SerializeField]
		[Range(0.1f, 5f)]
		protected float sampleStepSizeMin = 2.5f;

		// Token: 0x04002919 RID: 10521
		[SerializeField]
		[Range(0.1f, 5f)]
		protected float sampleStepSizeMax = 5f;

		// Token: 0x0400291A RID: 10522
		protected int aheadPointSamples = 4;

		// Token: 0x0400291B RID: 10523
		protected const float DestinationDistanceSlowThreshold = 8f;

		// Token: 0x0400291C RID: 10524
		protected const float DestinationArrivalThreshold = 3f;

		// Token: 0x0400291D RID: 10525
		[Header("Steer settings")]
		[SerializeField]
		protected float steerTargetFollowRate = 2f;

		// Token: 0x0400291E RID: 10526
		private SteerPID steerPID;

		// Token: 0x0400291F RID: 10527
		[Header("Turning speed reduction")]
		protected float turnSpeedReductionMinRange = 2f;

		// Token: 0x04002920 RID: 10528
		protected float turnSpeedReductionMaxRange = 10f;

		// Token: 0x04002921 RID: 10529
		protected float turnSpeedReductionDivisor = 90f;

		// Token: 0x04002922 RID: 10530
		private float minTurnSpeedReductionAngleThreshold = 15f;

		// Token: 0x04002923 RID: 10531
		private float minTurningSpeed = 10f;

		// Token: 0x04002924 RID: 10532
		[Header("Throttle")]
		[SerializeField]
		protected float throttleMin = -1f;

		// Token: 0x04002925 RID: 10533
		[SerializeField]
		protected float throttleMax = 1f;

		// Token: 0x04002926 RID: 10534
		private PID throttlePID;

		// Token: 0x04002927 RID: 10535
		public static float UnmarkedSpeed = 25f;

		// Token: 0x04002928 RID: 10536
		public static float ReverseSpeed = 5f;

		// Token: 0x04002929 RID: 10537
		private ValueTracker speedReductionTracker;

		// Token: 0x0400292A RID: 10538
		[Header("Pursuit Mode")]
		public bool PursuitModeEnabled;

		// Token: 0x0400292B RID: 10539
		public Transform PursuitTarget;

		// Token: 0x0400292C RID: 10540
		public float PursuitDistanceUpdateThreshold = 5f;

		// Token: 0x0400292D RID: 10541
		private Vector3 PursuitTargetLastPosition = Vector3.zero;

		// Token: 0x0400292E RID: 10542
		[Header("Stuck Detection")]
		public VehicleTeleporter Teleporter;

		// Token: 0x0400292F RID: 10543
		public PositionHistoryTracker PositionHistoryTracker;

		// Token: 0x04002930 RID: 10544
		public float StuckTimeThreshold = 10f;

		// Token: 0x04002931 RID: 10545
		public int StuckSamples = 4;

		// Token: 0x04002932 RID: 10546
		public float StuckDistanceThreshold = 1f;

		// Token: 0x04002933 RID: 10547
		protected VehicleAgent.NavigationCallback storedNavigationCallback;

		// Token: 0x04002934 RID: 10548
		protected SpeedZone currentSpeedZone;

		// Token: 0x04002935 RID: 10549
		protected LandVehicle vehicle;

		// Token: 0x04002936 RID: 10550
		protected float wheelbase;

		// Token: 0x04002937 RID: 10551
		protected float wheeltrack;

		// Token: 0x04002938 RID: 10552
		protected float vehicleLength;

		// Token: 0x04002939 RID: 10553
		protected float vehicleWidth;

		// Token: 0x0400293A RID: 10554
		protected float turnRadius;

		// Token: 0x0400293B RID: 10555
		protected float sweepTrack;

		// Token: 0x0400293C RID: 10556
		private float wheelBottomOffset;

		// Token: 0x0400293D RID: 10557
		[Header("Control info - READONLY")]
		[SerializeField]
		protected float targetSpeed;

		// Token: 0x0400293E RID: 10558
		[SerializeField]
		protected float targetSteerAngle_Normalized;

		// Token: 0x0400293F RID: 10559
		protected float lateralOffset;

		// Token: 0x04002940 RID: 10560
		protected PathSmoothingUtility.SmoothedPath path;

		// Token: 0x04002941 RID: 10561
		private float timeSinceLastNavigationCall;

		// Token: 0x04002942 RID: 10562
		private float sweepTestFailedTime;

		// Token: 0x04002943 RID: 10563
		private NavigationSettings currentNavigationSettings;

		// Token: 0x04002944 RID: 10564
		private Coroutine navigationCalculationRoutine;

		// Token: 0x04002945 RID: 10565
		private Coroutine reverseCoroutine;

		// Token: 0x0200083D RID: 2109
		public enum ENavigationResult
		{
			// Token: 0x04002947 RID: 10567
			Failed,
			// Token: 0x04002948 RID: 10568
			Complete,
			// Token: 0x04002949 RID: 10569
			Stopped
		}

		// Token: 0x0200083E RID: 2110
		public enum EAgentStatus
		{
			// Token: 0x0400294B RID: 10571
			Inactive,
			// Token: 0x0400294C RID: 10572
			MovingToRoad,
			// Token: 0x0400294D RID: 10573
			OnRoad
		}

		// Token: 0x0200083F RID: 2111
		public enum EPathGroupStatus
		{
			// Token: 0x0400294F RID: 10575
			Inactive,
			// Token: 0x04002950 RID: 10576
			Calculating
		}

		// Token: 0x02000840 RID: 2112
		public enum ESweepType
		{
			// Token: 0x04002952 RID: 10578
			FL,
			// Token: 0x04002953 RID: 10579
			FR,
			// Token: 0x04002954 RID: 10580
			RL,
			// Token: 0x04002955 RID: 10581
			RR
		}

		// Token: 0x02000841 RID: 2113
		// (Invoke) Token: 0x06003924 RID: 14628
		public delegate void NavigationCallback(VehicleAgent.ENavigationResult status);
	}
}
