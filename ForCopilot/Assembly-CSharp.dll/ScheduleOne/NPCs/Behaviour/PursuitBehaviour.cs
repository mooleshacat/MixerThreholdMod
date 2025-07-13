using System;
using System.Collections;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Noise;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Vision;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000562 RID: 1378
	public class PursuitBehaviour : Behaviour
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x0008632E File Offset: 0x0008452E
		// (set) Token: 0x060020BF RID: 8383 RVA: 0x00086336 File Offset: 0x00084536
		public Player TargetPlayer { get; protected set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x0008633F File Offset: 0x0008453F
		// (set) Token: 0x060020C1 RID: 8385 RVA: 0x00086347 File Offset: 0x00084547
		public bool IsSearching { get; protected set; }

		// Token: 0x060020C2 RID: 8386 RVA: 0x00086350 File Offset: 0x00084550
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.PursuitBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x0008636F File Offset: 0x0008456F
		private void OnDestroy()
		{
			PoliceOfficer.OnPoliceVisionEvent = (Action<VisionEventReceipt>)Delegate.Remove(PoliceOfficer.OnPoliceVisionEvent, new Action<VisionEventReceipt>(this.ProcessThirdPartyVisionEvent));
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00086391 File Offset: 0x00084591
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (base.Active && this.TargetPlayer != null)
			{
				this.AssignTarget(connection, this.TargetPlayer.NetworkObject);
			}
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000863C4 File Offset: 0x000845C4
		[ObserversRpc(RunLocally = true)]
		public virtual void AssignTarget(NetworkConnection conn, NetworkObject target)
		{
			this.RpcWriter___Observers_AssignTarget_1824087381(conn, target);
			this.RpcLogic___AssignTarget_1824087381(conn, target);
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000863F0 File Offset: 0x000845F0
		protected override void Begin()
		{
			base.Begin();
			this.CheckPlayerVisibility();
			this.sync___set_value_isTargetVisible(true, true);
			this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			this.officer.ProxCircle.SetRadius(2.5f);
			this.officer.Avatar.EmotionManager.AddEmotionOverride((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? "Angry" : "Annoyed", "pursuit", 0f, 0);
			this.officer.Movement.SetStance(NPCMovement.EStance.Stanced);
			this.officer.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000864A8 File Offset: 0x000846A8
		protected override void Resume()
		{
			base.Resume();
			this.CheckPlayerVisibility();
			this.sync___set_value_isTargetVisible(true, true);
			this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			this.officer.ProxCircle.SetRadius(2.5f);
			this.officer.Avatar.EmotionManager.AddEmotionOverride((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? "Angry" : "Annoyed", "pursuit", 0f, 0);
			this.officer.Movement.SetStance(NPCMovement.EStance.Stanced);
			this.officer.Movement.SetAgentType(NPCMovement.EAgentType.IgnoreCosts);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00086560 File Offset: 0x00084760
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.UpdateLookAt();
			this.UpdateArrest(Time.deltaTime);
			this.UpdateArrestCircle();
			this.SetWorldspaceIconsActive(this.timeSinceLastSighting < 3f || this.timeSinceLastSighting > 10f);
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000865B0 File Offset: 0x000847B0
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (Time.time > this.nextAngryVO)
			{
				EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Angry : EVOLineType.Command;
				base.Npc.PlayVO(lineType);
				this.nextAngryVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			}
			if (InstanceFinder.IsServer)
			{
				if (!this.IsTargetValid())
				{
					base.Disable_Networked(null);
					return;
				}
				if (this.rangedWeaponRoutine != null && this.TargetPlayer.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.NonLethal && this.TargetPlayer.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.Lethal)
				{
					this.StopRangedWeaponRoutine();
				}
			}
			if (!this.IsTargetValid())
			{
				return;
			}
			if (this.IsSearching)
			{
				if (this.SyncAccessor_isTargetVisible || this.TargetPlayer.CrimeData.TimeSinceSighted < 1f)
				{
					this.StopSearching();
				}
			}
			else
			{
				switch (this.TargetPlayer.CrimeData.CurrentPursuitLevel)
				{
				case PlayerCrimeData.EPursuitLevel.None:
					if (InstanceFinder.IsServer)
					{
						base.End_Networked(null);
					}
					break;
				case PlayerCrimeData.EPursuitLevel.Investigating:
					this.UpdateInvestigatingBehaviour();
					break;
				case PlayerCrimeData.EPursuitLevel.Arresting:
					this.UpdateArrestBehaviour();
					break;
				case PlayerCrimeData.EPursuitLevel.NonLethal:
					this.UpdateNonLethalBehaviour();
					break;
				case PlayerCrimeData.EPursuitLevel.Lethal:
					this.UpdateLethalBehaviour();
					break;
				}
			}
			this.UpdateEquippable();
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000866EC File Offset: 0x000848EC
		private bool IsTargetValid()
		{
			return !(this.TargetPlayer == null) && !this.TargetPlayer.IsArrested && !this.TargetPlayer.IsUnconscious && this.TargetPlayer.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None;
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x0008673C File Offset: 0x0008493C
		protected virtual void FixedUpdate()
		{
			if (!base.Active)
			{
				return;
			}
			this.CheckPlayerVisibility();
			this.currentPursuitLevelDuration += Time.fixedDeltaTime;
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00086760 File Offset: 0x00084960
		protected virtual void UpdateInvestigatingBehaviour()
		{
			this.arrestingEnabled = false;
			if (InstanceFinder.IsServer && !base.Npc.Movement.SpeedController.DoesSpeedControlExist("investigating"))
			{
				base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("investigating", 50, 0.35f));
			}
			if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) >= 2.5f)
			{
				if (this.isTargetStrictlyVisible && this.SyncAccessor_isTargetVisible)
				{
					this.TargetPlayer.CrimeData.Escalate();
					return;
				}
				if (InstanceFinder.IsServer)
				{
					if (!base.Npc.Movement.IsMoving && Vector3.Distance(this.TargetPlayer.CrimeData.LastKnownPosition, base.transform.position) < 2.5f)
					{
						this.StartSearching();
						return;
					}
					if (!base.Npc.Movement.IsMoving || Vector3.Distance(this.TargetPlayer.CrimeData.LastKnownPosition, base.Npc.Movement.CurrentDestination) > 2.5f)
					{
						if (base.Npc.Movement.CanGetTo(this.TargetPlayer.CrimeData.LastKnownPosition, 2.5f))
						{
							base.Npc.Movement.SetDestination(this.TargetPlayer.CrimeData.LastKnownPosition);
							return;
						}
						this.StartSearching();
						return;
					}
				}
			}
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000868E0 File Offset: 0x00084AE0
		protected virtual void UpdateArrestBehaviour()
		{
			this.arrestingEnabled = true;
			if (InstanceFinder.IsServer)
			{
				if (!base.Npc.Movement.SpeedController.DoesSpeedControlExist("arresting"))
				{
					base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("arresting", 50, 0.6f));
				}
				if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) >= 2.5f)
				{
					if (this.SyncAccessor_isTargetVisible)
					{
						bool flag = false;
						if (!base.Npc.Movement.IsMoving)
						{
							flag = true;
						}
						if (Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.Npc.Movement.CurrentDestination) > 2.5f)
						{
							flag = true;
						}
						if (flag)
						{
							base.Npc.Movement.SetDestination(this.GetNewArrestDestination());
						}
					}
					else
					{
						if (!base.Npc.Movement.IsMoving && Vector3.Distance(this.TargetPlayer.CrimeData.LastKnownPosition, base.transform.position) < 2.5f)
						{
							this.StartSearching();
							return;
						}
						if (!base.Npc.Movement.IsMoving || Vector3.Distance(this.TargetPlayer.CrimeData.LastKnownPosition, base.Npc.Movement.CurrentDestination) > 2.5f)
						{
							if (!base.Npc.Movement.CanGetTo(this.TargetPlayer.CrimeData.LastKnownPosition, 2.5f))
							{
								this.StartSearching();
								return;
							}
							base.Npc.Movement.SetDestination(this.TargetPlayer.CrimeData.LastKnownPosition);
						}
					}
				}
			}
			if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) > Mathf.Max(15f, this.distanceOnPursuitStart + 5f) && this.timeSinceLastSighting < 1f)
			{
				Debug.Log("Target too far! Escalating");
				this.TargetPlayer.CrimeData.Escalate();
			}
			if (this.TargetPlayer.CurrentVehicle != null && !this.targetWasDrivingOnPursuitStart && this.timeSinceLastSighting < 1f)
			{
				Debug.Log("Target got in vehicle! Escalating");
				this.TargetPlayer.CrimeData.Escalate();
			}
			if (this.leaveArrestCircleCount >= 3 && this.timeSinceLastSighting < 1f)
			{
				Debug.Log("Left arrest circle too many times! Escalating");
				this.TargetPlayer.CrimeData.Escalate();
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00086B78 File Offset: 0x00084D78
		private void UpdateArrest(float tick)
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) < 2.5f && this.arrestingEnabled && this.SyncAccessor_isTargetVisible)
			{
				this.timeWithinArrestRange += tick;
				if (this.timeWithinArrestRange > 0.5f)
				{
					this.wasInArrestCircleLastFrame = true;
				}
			}
			else
			{
				if (this.wasInArrestCircleLastFrame)
				{
					this.leaveArrestCircleCount++;
					this.wasInArrestCircleLastFrame = false;
				}
				this.timeWithinArrestRange = Mathf.Clamp(this.timeWithinArrestRange - tick, 0f, float.MaxValue);
			}
			if (this.TargetPlayer.IsOwner && this.timeWithinArrestRange / 1.75f > this.TargetPlayer.CrimeData.CurrentArrestProgress)
			{
				this.TargetPlayer.CrimeData.SetArrestProgress(this.timeWithinArrestRange / 1.75f);
			}
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00086C70 File Offset: 0x00084E70
		private Vector3 GetNewArrestDestination()
		{
			return this.TargetPlayer.Avatar.CenterPoint + (base.transform.position - this.TargetPlayer.Avatar.CenterPoint).normalized * 0.75f;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00086CC4 File Offset: 0x00084EC4
		private void ClearSpeedControls()
		{
			if (base.Npc.Movement.SpeedController.DoesSpeedControlExist("investigating"))
			{
				base.Npc.Movement.SpeedController.RemoveSpeedControl("investigating");
			}
			if (base.Npc.Movement.SpeedController.DoesSpeedControlExist("arresting"))
			{
				base.Npc.Movement.SpeedController.RemoveSpeedControl("arresting");
			}
			if (base.Npc.Movement.SpeedController.DoesSpeedControlExist("chasing"))
			{
				base.Npc.Movement.SpeedController.RemoveSpeedControl("chasing");
			}
			if (base.Npc.Movement.SpeedController.DoesSpeedControlExist("shooting"))
			{
				base.Npc.Movement.SpeedController.RemoveSpeedControl("shooting");
			}
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00086DA9 File Offset: 0x00084FA9
		protected virtual void UpdateNonLethalBehaviour()
		{
			this.arrestingEnabled = true;
			if (InstanceFinder.IsServer && this.rangedWeaponRoutine == null)
			{
				this.rangedWeaponRoutine = base.StartCoroutine(this.RangedWeaponRoutine());
			}
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x00086DA9 File Offset: 0x00084FA9
		protected virtual void UpdateLethalBehaviour()
		{
			this.arrestingEnabled = true;
			if (InstanceFinder.IsServer && this.rangedWeaponRoutine == null)
			{
				this.rangedWeaponRoutine = base.StartCoroutine(this.RangedWeaponRoutine());
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00086DD3 File Offset: 0x00084FD3
		private IEnumerator RangedWeaponRoutine()
		{
			PursuitBehaviour.EPursuitAction currentAction = PursuitBehaviour.EPursuitAction.None;
			float currentActionDuration = 0f;
			float currentActionTime = 0f;
			for (;;)
			{
				if (this.rangedWeapon == null)
				{
					yield return new WaitForEndOfFrame();
				}
				else
				{
					float num = Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint);
					if (this.SyncAccessor_isTargetVisible && num > this.rangedWeapon.MinUseRange && num < this.rangedWeapon.MaxUseRange)
					{
						currentActionDuration += Time.deltaTime;
						if (currentActionDuration > currentActionTime)
						{
							currentAction = PursuitBehaviour.EPursuitAction.None;
						}
						if (currentAction == PursuitBehaviour.EPursuitAction.None)
						{
							currentActionDuration = 0f;
							PursuitBehaviour.EPursuitAction epursuitAction;
							if (this.rangedWeapon.CanShootWhileMoving)
							{
								if (UnityEngine.Random.Range(0, 3) == 0)
								{
									epursuitAction = PursuitBehaviour.EPursuitAction.Move;
								}
								else if ((double)num < (double)this.rangedWeapon.MaxUseRange * 0.5)
								{
									epursuitAction = PursuitBehaviour.EPursuitAction.Shoot;
								}
								else
								{
									epursuitAction = PursuitBehaviour.EPursuitAction.MoveAndShoot;
								}
							}
							else
							{
								epursuitAction = ((UnityEngine.Random.Range(0, 2) == 0) ? PursuitBehaviour.EPursuitAction.Move : PursuitBehaviour.EPursuitAction.Shoot);
							}
							if (this.TargetPlayer.CrimeData.timeSinceLastShot < 2f)
							{
								epursuitAction = PursuitBehaviour.EPursuitAction.Move;
							}
							this.SetWeaponRaised(epursuitAction == PursuitBehaviour.EPursuitAction.Shoot || epursuitAction == PursuitBehaviour.EPursuitAction.MoveAndShoot);
							this.consecutiveMissedShots = 0;
							this.ClearSpeedControls();
							if (epursuitAction == PursuitBehaviour.EPursuitAction.Move)
							{
								base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("chasing", 60, 0.8f));
							}
							else if (epursuitAction == PursuitBehaviour.EPursuitAction.MoveAndShoot)
							{
								base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("shooting", 60, 0.15f));
							}
							currentActionTime = UnityEngine.Random.Range(3f, 6f);
							currentAction = epursuitAction;
						}
						switch (currentAction)
						{
						case PursuitBehaviour.EPursuitAction.Move:
							if (this.arrestingEnabled)
							{
								if (!base.Npc.Movement.IsMoving && Vector3.Distance(base.Npc.Movement.transform.position, this.TargetPlayer.Avatar.CenterPoint) < 2.5f)
								{
									currentAction = PursuitBehaviour.EPursuitAction.None;
								}
								else if (!base.Npc.Movement.IsMoving || Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.Avatar.CenterPoint) > 2.5f)
								{
									base.Npc.Movement.SetDestination(this.TargetPlayer.Avatar.CenterPoint);
								}
							}
							else if (!base.Npc.Movement.IsMoving && Vector3.Distance(base.Npc.Movement.transform.position, this.TargetPlayer.Avatar.CenterPoint) < this.rangedWeapon.MaxUseRange)
							{
								currentAction = PursuitBehaviour.EPursuitAction.None;
							}
							else if (!base.Npc.Movement.IsMoving || Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.Avatar.CenterPoint) > this.rangedWeapon.MaxUseRange)
							{
								float randomRadius = Mathf.Max(Mathf.Min(this.rangedWeapon.MaxUseRange * 0.6f, num), this.rangedWeapon.MinUseRange * 2f);
								base.Npc.Movement.SetDestination(this.GetRandomReachablePointNear(this.TargetPlayer.Avatar.CenterPoint, randomRadius, this.rangedWeapon.MinUseRange));
							}
							break;
						case PursuitBehaviour.EPursuitAction.Shoot:
							if (base.Npc.Movement.IsMoving)
							{
								base.Npc.Movement.Stop();
							}
							if (Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) > this.rangedWeapon.MaxUseRange)
							{
								currentAction = PursuitBehaviour.EPursuitAction.None;
							}
							if (this.CanShoot() && this.Shoot())
							{
								currentAction = PursuitBehaviour.EPursuitAction.None;
							}
							break;
						case PursuitBehaviour.EPursuitAction.MoveAndShoot:
							if (this.arrestingEnabled)
							{
								if (!base.Npc.Movement.IsMoving || Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.Avatar.CenterPoint) > 2.5f)
								{
									base.Npc.Movement.SetDestination(this.TargetPlayer.Avatar.CenterPoint);
								}
							}
							else if (!base.Npc.Movement.IsMoving || Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.Avatar.CenterPoint) > this.rangedWeapon.MaxUseRange)
							{
								float randomRadius2 = Mathf.Max(Mathf.Min(this.rangedWeapon.MaxUseRange * 0.6f, num), this.rangedWeapon.MinUseRange * 2f);
								base.Npc.Movement.SetDestination(this.GetRandomReachablePointNear(this.TargetPlayer.Avatar.CenterPoint, randomRadius2, this.rangedWeapon.MinUseRange));
							}
							if (this.CanShoot() && this.Shoot())
							{
								currentAction = PursuitBehaviour.EPursuitAction.None;
							}
							break;
						}
					}
					else
					{
						this.ClearSpeedControls();
						base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("chasing", 60, 0.8f));
						this.SetWeaponRaised(false);
						currentAction = PursuitBehaviour.EPursuitAction.Move;
						if (this.SyncAccessor_isTargetVisible)
						{
							if (this.arrestingEnabled)
							{
								if (!base.Npc.Movement.IsMoving || Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.Avatar.CenterPoint) > 2.5f)
								{
									base.Npc.Movement.SetDestination(this.TargetPlayer.Avatar.CenterPoint);
								}
							}
							else if (!base.Npc.Movement.IsMoving || Vector3.Distance(base.Npc.Movement.CurrentDestination, this.TargetPlayer.Avatar.CenterPoint) > this.rangedWeapon.MaxUseRange)
							{
								float randomRadius3 = Mathf.Max(Mathf.Min(this.rangedWeapon.MaxUseRange * 0.6f, num), this.rangedWeapon.MinUseRange * 2f);
								base.Npc.Movement.SetDestination(this.GetRandomReachablePointNear(this.TargetPlayer.Avatar.CenterPoint, randomRadius3, this.rangedWeapon.MinUseRange));
							}
						}
						else
						{
							if (!base.Npc.Movement.IsMoving && Vector3.Distance(this.TargetPlayer.CrimeData.LastKnownPosition, base.transform.position) < 2.5f)
							{
								break;
							}
							if (!base.Npc.Movement.IsMoving || Vector3.Distance(this.TargetPlayer.CrimeData.LastKnownPosition, base.Npc.Movement.CurrentDestination) > 2.5f)
							{
								if (!base.Npc.Movement.CanGetTo(this.TargetPlayer.CrimeData.LastKnownPosition, 2.5f))
								{
									goto IL_7B3;
								}
								base.Npc.Movement.SetDestination(this.TargetPlayer.CrimeData.LastKnownPosition);
							}
						}
					}
					yield return new WaitForEndOfFrame();
				}
			}
			this.StartSearching();
			this.StopRangedWeaponRoutine();
			yield break;
			IL_7B3:
			this.StartSearching();
			this.StopRangedWeaponRoutine();
			yield break;
			yield break;
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00086DE4 File Offset: 0x00084FE4
		private bool CanShoot()
		{
			return !base.Npc.IsInVehicle && !base.Npc.Avatar.Ragdolled && !base.Npc.Avatar.Anim.StandUpAnimationPlaying && this.isTargetStrictlyVisible && this.rangedWeapon.CanShoot();
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00086E44 File Offset: 0x00085044
		private bool Shoot()
		{
			bool flag = false;
			float num = Mathf.Lerp(this.rangedWeapon.HitChange_MinRange, this.rangedWeapon.HitChange_MaxRange, Mathf.Clamp01(Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint) / this.rangedWeapon.MaxUseRange));
			num *= this.TargetPlayer.CrimeData.GetShotAccuracyMultiplier();
			num *= 1f + 0.1f * (float)this.consecutiveMissedShots;
			if (UnityEngine.Random.Range(0f, 1f) < num)
			{
				flag = true;
			}
			Vector3 vector = this.TargetPlayer.Avatar.CenterPoint;
			bool flag2 = false;
			if (flag && this.rangedWeapon.IsPlayerInLoS(this.TargetPlayer))
			{
				flag2 = true;
			}
			else
			{
				vector += UnityEngine.Random.insideUnitSphere * 4f;
				Vector3 normalized = (vector - this.rangedWeapon.MuzzlePoint.position).normalized;
				RaycastHit raycastHit;
				if (Physics.Raycast(this.rangedWeapon.MuzzlePoint.position, normalized, ref raycastHit, this.rangedWeapon.MaxUseRange, LayerMask.GetMask(AvatarRangedWeapon.RaycastLayers)))
				{
					vector = raycastHit.point;
				}
				else
				{
					vector = this.rangedWeapon.MuzzlePoint.position + normalized * this.rangedWeapon.MaxUseRange;
				}
			}
			if (flag2)
			{
				this.consecutiveMissedShots = 0;
				if (this.rangedWeapon is Taser)
				{
					this.TargetPlayer.Taze();
				}
				if (this.rangedWeapon.Damage > 0f && this.TargetPlayer.Health.CanTakeDamage)
				{
					this.TargetPlayer.Health.TakeDamage(this.rangedWeapon.Damage, true, true);
				}
				if (this.rangedWeapon is Handgun)
				{
					NoiseUtility.EmitNoise(this.rangedWeapon.MuzzlePoint.position, ENoiseType.Gunshot, 25f, base.Npc.gameObject);
				}
				this.TargetPlayer.CrimeData.ResetShotAccuracy();
			}
			else
			{
				this.consecutiveMissedShots++;
			}
			base.Npc.SendEquippableMessage_Networked_Vector(null, "Shoot", vector);
			return flag2;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00087072 File Offset: 0x00085272
		private void SetWeaponRaised(bool raised)
		{
			if (this.rangedWeapon.IsRaised != raised)
			{
				if (raised)
				{
					base.Npc.SendEquippableMessage_Networked(null, "Raise");
					return;
				}
				base.Npc.SendEquippableMessage_Networked(null, "Lower");
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000870A8 File Offset: 0x000852A8
		private void StopRangedWeaponRoutine()
		{
			if (this.rangedWeaponRoutine != null)
			{
				base.StopCoroutine(this.rangedWeaponRoutine);
				this.rangedWeaponRoutine = null;
			}
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000870C5 File Offset: 0x000852C5
		protected virtual void UpdateLookAt()
		{
			if (this.TargetPlayer != null && this.SyncAccessor_isTargetVisible)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.TargetPlayer.MimicCamera.position, 10, true);
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00087108 File Offset: 0x00085308
		protected virtual void UpdateEquippable()
		{
			if (!base.Active)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.rangedWeapon = null;
			string text = string.Empty;
			if (this.TargetPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				text = this.officer.BatonPrefab.AssetPath;
				this.officer.belt.SetBatonVisible(false);
			}
			else
			{
				this.officer.belt.SetBatonVisible(true);
			}
			if (this.TargetPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal)
			{
				text = this.officer.TaserPrefab.AssetPath;
				this.officer.belt.SetTaserVisible(false);
			}
			else
			{
				this.officer.belt.SetTaserVisible(true);
			}
			if (this.TargetPlayer.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Lethal)
			{
				text = this.officer.GunPrefab.AssetPath;
				this.officer.belt.SetGunVisible(false);
			}
			else
			{
				this.officer.belt.SetGunVisible(true);
			}
			if (text != string.Empty)
			{
				if (base.Npc.Avatar.CurrentEquippable == null || base.Npc.Avatar.CurrentEquippable.AssetPath != text)
				{
					base.Npc.SetEquippable_Networked(null, text);
				}
				if (base.Npc.Avatar.CurrentEquippable is AvatarRangedWeapon)
				{
					this.rangedWeapon = (base.Npc.Avatar.CurrentEquippable as AvatarRangedWeapon);
					return;
				}
			}
			else if (base.Npc.Avatar.CurrentEquippable != null)
			{
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000872B6 File Offset: 0x000854B6
		public override void Disable()
		{
			base.Disable();
			this.TargetPlayer = null;
			this.End();
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000872CB File Offset: 0x000854CB
		protected override void Pause()
		{
			base.Pause();
			this.Stop();
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000872D9 File Offset: 0x000854D9
		protected override void End()
		{
			base.End();
			this.Stop();
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000872E8 File Offset: 0x000854E8
		private void Stop()
		{
			this.ClearSpeedControls();
			this.SetArrestCircleAlpha(0f);
			this.StopSearching();
			this.StopRangedWeaponRoutine();
			this.ClearEquippables();
			this.officer.Movement.SetAgentType(NPCMovement.EAgentType.Humanoid);
			this.officer.Movement.SetStance(NPCMovement.EStance.None);
			this.officer.Avatar.EmotionManager.RemoveEmotionOverride("pursuit");
			this.arrestingEnabled = false;
			this.timeSinceLastSighting = 10000f;
			this.currentPursuitLevelDuration = 0f;
			this.timeWithinArrestRange = 0f;
			this.rangedWeapon = null;
			if (this.TargetPlayer != null)
			{
				base.Npc.awareness.VisionCone.StateSettings[this.TargetPlayer][PlayerVisualState.EVisualState.Visible].Enabled = false;
			}
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000873C0 File Offset: 0x000855C0
		private void ClearEquippables()
		{
			base.Npc.SetEquippable_Networked(null, string.Empty);
			if (this.officer.belt != null)
			{
				this.officer.belt.SetBatonVisible(true);
				this.officer.belt.SetTaserVisible(true);
				this.officer.belt.SetGunVisible(true);
			}
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00087424 File Offset: 0x00085624
		protected void CheckPlayerVisibility()
		{
			if (this.TargetPlayer == null)
			{
				return;
			}
			if (this.IsPlayerVisible())
			{
				this.playerSightedDuration += Time.fixedDeltaTime;
				this.sync___set_value_isTargetVisible(true, true);
				this.isTargetStrictlyVisible = true;
			}
			else
			{
				this.playerSightedDuration = 0f;
				this.timeSinceLastSighting += Time.fixedDeltaTime;
				this.sync___set_value_isTargetVisible(false, true);
				this.isTargetStrictlyVisible = false;
				if (this.timeSinceLastSighting < 2f)
				{
					this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
					this.sync___set_value_isTargetVisible(true, true);
				}
			}
			if (this.SyncAccessor_isTargetVisible)
			{
				this.TargetPlayer.CrimeData.RecordLastKnownPosition(this.timeSinceLastSighting < 2f);
			}
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000874E2 File Offset: 0x000856E2
		public void MarkPlayerVisible()
		{
			if (this.IsPlayerVisible())
			{
				this.TargetPlayer.CrimeData.RecordLastKnownPosition(true);
				this.timeSinceLastSighting = 0f;
				return;
			}
			this.TargetPlayer.CrimeData.RecordLastKnownPosition(false);
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0008751A File Offset: 0x0008571A
		protected bool IsPlayerVisible()
		{
			return base.Npc.awareness.VisionCone.IsPlayerVisible(this.TargetPlayer);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00087538 File Offset: 0x00085738
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject && visionEventReceipt.State == PlayerVisualState.EVisualState.SearchedFor)
			{
				this.MarkPlayerVisible();
				this.sync___set_value_isTargetVisible(true, true);
				this.isTargetStrictlyVisible = true;
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00087584 File Offset: 0x00085784
		private void ProcessThirdPartyVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (!base.Active)
			{
				return;
			}
			if (visionEventReceipt.TargetPlayer == this.TargetPlayer.NetworkObject && visionEventReceipt.State == PlayerVisualState.EVisualState.SearchedFor)
			{
				this.sync___set_value_isTargetVisible(true, true);
				this.isTargetStrictlyVisible = true;
			}
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000875C0 File Offset: 0x000857C0
		protected virtual void UpdateArrestCircle()
		{
			if (this.TargetPlayer == null || !this.arrestingEnabled || this.TargetPlayer != Player.Local)
			{
				this.SetArrestCircleAlpha(0f);
				return;
			}
			if (this.TargetPlayer.CrimeData.NearestOfficer != base.Npc)
			{
				this.SetArrestCircleAlpha(0f);
				return;
			}
			if (!this.SyncAccessor_isTargetVisible)
			{
				this.SetArrestCircleAlpha(0f);
				return;
			}
			float num = Vector3.Distance(this.TargetPlayer.Avatar.CenterPoint, base.transform.position);
			if (num < 2.5f)
			{
				this.SetArrestCircleAlpha(this.ArrestCircle_MaxOpacity);
				this.SetArrestCircleColor(new Color32(byte.MaxValue, 50, 50, byte.MaxValue));
				return;
			}
			if (num < this.ArrestCircle_MaxVisibleDistance)
			{
				float arrestCircleAlpha = Mathf.Lerp(this.ArrestCircle_MaxOpacity, 0f, (num - 2.5f) / (this.ArrestCircle_MaxVisibleDistance - 2.5f));
				this.SetArrestCircleAlpha(arrestCircleAlpha);
				this.SetArrestCircleColor(Color.white);
				return;
			}
			this.SetArrestCircleAlpha(0f);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000876DE File Offset: 0x000858DE
		public void ResetArrestProgress()
		{
			this.timeWithinArrestRange = 0f;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000876EB File Offset: 0x000858EB
		private void SetArrestCircleAlpha(float alpha)
		{
			this.officer.ProxCircle.SetAlpha(alpha);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000876FE File Offset: 0x000858FE
		private void SetArrestCircleColor(Color col)
		{
			this.officer.ProxCircle.SetColor(col);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00087711 File Offset: 0x00085911
		private void StartSearching()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.IsSearching = true;
			this.searchRoutine = base.StartCoroutine(this.SearchRoutine());
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00087734 File Offset: 0x00085934
		private void StopSearching()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.IsSearching = false;
			this.hasSearchDestination = false;
			if (this.searchRoutine != null)
			{
				base.StopCoroutine(this.searchRoutine);
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00087760 File Offset: 0x00085960
		private IEnumerator SearchRoutine()
		{
			while (this.IsSearching)
			{
				if (!this.hasSearchDestination)
				{
					this.currentSearchDestination = this.GetNextSearchLocation();
					base.Npc.Movement.SetDestination(this.currentSearchDestination);
					this.hasSearchDestination = true;
				}
				for (;;)
				{
					if (!base.Npc.Movement.IsMoving && base.Npc.Movement.CanMove())
					{
						base.Npc.Movement.SetDestination(this.currentSearchDestination);
					}
					if (Vector3.Distance(base.transform.position, this.currentSearchDestination) < 2.5f)
					{
						break;
					}
					yield return new WaitForSeconds(1f);
				}
				this.hasSearchDestination = false;
				yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 6f));
			}
			this.searchRoutine = null;
			this.StopSearching();
			yield break;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00087770 File Offset: 0x00085970
		private Vector3 GetNextSearchLocation()
		{
			float num = Mathf.Lerp(25f, 80f, Mathf.Clamp(this.timeSinceLastSighting / this.TargetPlayer.CrimeData.GetSearchTime(), 0f, 1f));
			num = Mathf.Min(num, Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint));
			return this.GetRandomReachablePointNear(this.TargetPlayer.Avatar.CenterPoint, num, 0f);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000877F8 File Offset: 0x000859F8
		private Vector3 GetRandomReachablePointNear(Vector3 point, float randomRadius, float minDistance = 0f)
		{
			bool flag = false;
			Vector3 result = point;
			int num = 0;
			while (!flag)
			{
				Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
				Vector3 normalized = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y).normalized;
				NavMeshHit navMeshHit;
				NavMeshUtility.SamplePosition(point + normalized * randomRadius, out navMeshHit, 5f, base.Npc.Movement.Agent.areaMask, true);
				if (base.Npc.Movement.CanGetTo(navMeshHit.position, 2.5f) && Vector3.Distance(point, navMeshHit.position) > minDistance)
				{
					result = navMeshHit.position;
					break;
				}
				num++;
				if (num > 10)
				{
					Console.LogError("Failed to find search destination", null);
					break;
				}
			}
			return result;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000878BF File Offset: 0x00085ABF
		private void SetWorldspaceIconsActive(bool active)
		{
			base.Npc.awareness.VisionCone.WorldspaceIconsEnabled = active;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x0008790C File Offset: 0x00085B0C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PursuitBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PursuitBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___isTargetVisible = new SyncVar<bool>(this, 0U, 1, 0, 0.25f, 0, this.isTargetVisible);
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_AssignTarget_1824087381));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.NPCs.Behaviour.PursuitBehaviour));
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00087984 File Offset: 0x00085B84
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PursuitBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PursuitBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___isTargetVisible.SetRegistered();
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000879A8 File Offset: 0x00085BA8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000879B8 File Offset: 0x00085BB8
		private void RpcWriter___Observers_AssignTarget_1824087381(NetworkConnection conn, NetworkObject target)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkConnection(conn);
			writer.WriteNetworkObject(target);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x00087A7C File Offset: 0x00085C7C
		public virtual void RpcLogic___AssignTarget_1824087381(NetworkConnection conn, NetworkObject target)
		{
			this.TargetPlayer = target.GetComponent<Player>();
			this.playerSightedDuration = 0f;
			this.timeSinceLastSighting = 0f;
			this.timeWithinArrestRange = 0f;
			this.leaveArrestCircleCount = 0;
			this.wasInArrestCircleLastFrame = false;
			this.distanceOnPursuitStart = Vector3.Distance(base.transform.position, this.TargetPlayer.Avatar.CenterPoint);
			this.targetWasDrivingOnPursuitStart = (this.TargetPlayer.CurrentVehicle != null);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00087B04 File Offset: 0x00085D04
		private void RpcReader___Observers_AssignTarget_1824087381(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AssignTarget_1824087381(conn, target);
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060020F5 RID: 8437 RVA: 0x00087B50 File Offset: 0x00085D50
		// (set) Token: 0x060020F6 RID: 8438 RVA: 0x00087B58 File Offset: 0x00085D58
		public bool SyncAccessor_isTargetVisible
		{
			get
			{
				return this.isTargetVisible;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.isTargetVisible = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___isTargetVisible.SetValue(value, value);
				}
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00087B94 File Offset: 0x00085D94
		public override bool PursuitBehaviour(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_isTargetVisible(this.syncVar___isTargetVisible.GetValue(true), true);
				return true;
			}
			bool value = PooledReader0.ReadBoolean();
			this.sync___set_value_isTargetVisible(value, Boolean2);
			return true;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00087BE8 File Offset: 0x00085DE8
		protected override void dll()
		{
			base.Awake();
			this.officer = (base.Npc as PoliceOfficer);
			VisionCone visionCone = this.officer.awareness.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
			PoliceOfficer.OnPoliceVisionEvent = (Action<VisionEventReceipt>)Delegate.Combine(PoliceOfficer.OnPoliceVisionEvent, new Action<VisionEventReceipt>(this.ProcessThirdPartyVisionEvent));
		}

		// Token: 0x04001937 RID: 6455
		public const float ARREST_RANGE = 2.5f;

		// Token: 0x04001938 RID: 6456
		public const float ARREST_TIME = 1.75f;

		// Token: 0x04001939 RID: 6457
		public const float EXTRA_VISIBILITY_TIME = 2f;

		// Token: 0x0400193A RID: 6458
		public const float MOVE_SPEED_INVESTIGATING = 0.35f;

		// Token: 0x0400193B RID: 6459
		public const float MOVE_SPEED_ARRESTING = 0.6f;

		// Token: 0x0400193C RID: 6460
		public const float MOVE_SPEED_CHASE = 0.8f;

		// Token: 0x0400193D RID: 6461
		public const float MOVE_SPEED_SHOOTING = 0.15f;

		// Token: 0x0400193E RID: 6462
		public const float SEARCH_RADIUS_MIN = 25f;

		// Token: 0x0400193F RID: 6463
		public const float SEARCH_RADIUS_MAX = 80f;

		// Token: 0x04001940 RID: 6464
		public const float ARREST_MAX_DISTANCE = 15f;

		// Token: 0x04001941 RID: 6465
		public const int LEAVE_ARREST_CIRCLE_LIMIT = 3;

		// Token: 0x04001942 RID: 6466
		public const float CONSECUTIVE_MISS_ACCURACY_BOOST = 0.1f;

		// Token: 0x04001945 RID: 6469
		[Header("Settings")]
		public float ArrestCircle_MaxVisibleDistance = 5f;

		// Token: 0x04001946 RID: 6470
		public float ArrestCircle_MaxOpacity = 0.25f;

		// Token: 0x04001947 RID: 6471
		[SyncVar]
		public bool isTargetVisible;

		// Token: 0x04001948 RID: 6472
		protected bool isTargetStrictlyVisible;

		// Token: 0x04001949 RID: 6473
		protected bool arrestingEnabled;

		// Token: 0x0400194A RID: 6474
		protected float timeSinceLastSighting = 10000f;

		// Token: 0x0400194B RID: 6475
		protected float currentPursuitLevelDuration;

		// Token: 0x0400194C RID: 6476
		protected float timeWithinArrestRange;

		// Token: 0x0400194D RID: 6477
		protected float playerSightedDuration;

		// Token: 0x0400194E RID: 6478
		protected float distanceOnPursuitStart;

		// Token: 0x0400194F RID: 6479
		private Coroutine searchRoutine;

		// Token: 0x04001950 RID: 6480
		private Coroutine rangedWeaponRoutine;

		// Token: 0x04001951 RID: 6481
		private Vector3 currentSearchDestination = Vector3.zero;

		// Token: 0x04001952 RID: 6482
		private bool hasSearchDestination;

		// Token: 0x04001953 RID: 6483
		private PoliceOfficer officer;

		// Token: 0x04001954 RID: 6484
		private bool targetWasDrivingOnPursuitStart;

		// Token: 0x04001955 RID: 6485
		private bool wasInArrestCircleLastFrame;

		// Token: 0x04001956 RID: 6486
		private int leaveArrestCircleCount;

		// Token: 0x04001957 RID: 6487
		private AvatarRangedWeapon rangedWeapon;

		// Token: 0x04001958 RID: 6488
		private int consecutiveMissedShots;

		// Token: 0x04001959 RID: 6489
		private float nextAngryVO;

		// Token: 0x0400195A RID: 6490
		public SyncVar<bool> syncVar___isTargetVisible;

		// Token: 0x0400195B RID: 6491
		private bool dll_Excuted;

		// Token: 0x0400195C RID: 6492
		private bool dll_Excuted;

		// Token: 0x02000563 RID: 1379
		private enum EPursuitAction
		{
			// Token: 0x0400195E RID: 6494
			None,
			// Token: 0x0400195F RID: 6495
			Move,
			// Token: 0x04001960 RID: 6496
			Shoot,
			// Token: 0x04001961 RID: 6497
			MoveAndShoot
		}
	}
}
