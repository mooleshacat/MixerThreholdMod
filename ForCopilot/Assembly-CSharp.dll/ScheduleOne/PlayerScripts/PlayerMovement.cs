using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000637 RID: 1591
	public class PlayerMovement : PlayerSingleton<PlayerMovement>
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x000A8BCE File Offset: 0x000A6DCE
		// (set) Token: 0x060028FE RID: 10494 RVA: 0x000A8BD5 File Offset: 0x000A6DD5
		public static float GravityMultiplier { get; set; } = 1f;

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x000A8BDD File Offset: 0x000A6DDD
		// (set) Token: 0x06002900 RID: 10496 RVA: 0x000A8BE5 File Offset: 0x000A6DE5
		public float playerHeight { get; protected set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06002901 RID: 10497 RVA: 0x000A8BEE File Offset: 0x000A6DEE
		public Vector3 Movement
		{
			get
			{
				return this.movement;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002902 RID: 10498 RVA: 0x000A8BF6 File Offset: 0x000A6DF6
		// (set) Token: 0x06002903 RID: 10499 RVA: 0x000A8BFE File Offset: 0x000A6DFE
		public LandVehicle currentVehicle { get; protected set; }

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002904 RID: 10500 RVA: 0x000A8C07 File Offset: 0x000A6E07
		// (set) Token: 0x06002905 RID: 10501 RVA: 0x000A8C0F File Offset: 0x000A6E0F
		public float airTime { get; protected set; }

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06002906 RID: 10502 RVA: 0x000A8C18 File Offset: 0x000A6E18
		// (set) Token: 0x06002907 RID: 10503 RVA: 0x000A8C20 File Offset: 0x000A6E20
		public bool isCrouched { get; protected set; }

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002908 RID: 10504 RVA: 0x000A8C29 File Offset: 0x000A6E29
		// (set) Token: 0x06002909 RID: 10505 RVA: 0x000A8C31 File Offset: 0x000A6E31
		public float standingScale { get; protected set; } = 1f;

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x0600290A RID: 10506 RVA: 0x000A8C3A File Offset: 0x000A6E3A
		// (set) Token: 0x0600290B RID: 10507 RVA: 0x000A8C42 File Offset: 0x000A6E42
		public bool isRagdolled { get; protected set; }

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x000A8C4B File Offset: 0x000A6E4B
		// (set) Token: 0x0600290D RID: 10509 RVA: 0x000A8C53 File Offset: 0x000A6E53
		public bool isSprinting { get; protected set; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x0600290E RID: 10510 RVA: 0x000A8C5C File Offset: 0x000A6E5C
		// (set) Token: 0x0600290F RID: 10511 RVA: 0x000A8C64 File Offset: 0x000A6E64
		public float CurrentSprintMultiplier { get; protected set; } = 1f;

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002910 RID: 10512 RVA: 0x000A8C6D File Offset: 0x000A6E6D
		// (set) Token: 0x06002911 RID: 10513 RVA: 0x000A8C75 File Offset: 0x000A6E75
		public bool IsGrounded { get; private set; } = true;

		// Token: 0x06002912 RID: 10514 RVA: 0x000A8C80 File Offset: 0x000A6E80
		protected override void Awake()
		{
			base.Awake();
			this.playerHeight = this.Controller.height;
			this.Controller.detectCollisions = false;
			for (int i = 0; i < this.visibilityPointsToScale.Count; i++)
			{
				this.originalVisibilityPointOffsets.Add(this.visibilityPointsToScale[i], this.visibilityPointsToScale[i].localPosition.y);
			}
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000A8CF4 File Offset: 0x000A6EF4
		protected override void Start()
		{
			base.Start();
			Player local = Player.Local;
			local.onEnterVehicle = (Player.VehicleEvent)Delegate.Combine(local.onEnterVehicle, new Player.VehicleEvent(this.EnterVehicle));
			Player local2 = Player.Local;
			local2.onExitVehicle = (Player.VehicleTransformEvent)Delegate.Combine(local2.onExitVehicle, new Player.VehicleTransformEvent(this.ExitVehicle));
			Player.Local.Health.onRevive.AddListener(delegate()
			{
				this.SetStamina(PlayerMovement.StaminaReserveMax, false);
			});
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x000A8D74 File Offset: 0x000A6F74
		protected virtual void Update()
		{
			this.UpdateHorizontalAxis();
			this.UpdateVerticalAxis();
			if (this.isCrouched)
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 0f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			else
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 1f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			this.UpdatePlayerHeight();
			if (this.residualVelocityTimeRemaining > 0f)
			{
				this.residualVelocityTimeRemaining -= Time.deltaTime;
			}
			this.timeSinceStaminaDrain += Time.deltaTime;
			if (this.timeSinceStaminaDrain > 1f && this.CurrentStaminaReserve < PlayerMovement.StaminaReserveMax)
			{
				this.ChangeStamina(25f * Time.deltaTime, true);
			}
			this.Move();
			this.UpdateCrouchVignetteEffect();
			this.UpdateMovementEvents();
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x000A8E4E File Offset: 0x000A704E
		private void FixedUpdate()
		{
			this.IsGrounded = this.isGrounded();
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000A8E5C File Offset: 0x000A705C
		private void LateUpdate()
		{
			if (this.teleport)
			{
				this.Controller.enabled = false;
				this.Controller.transform.position = this.teleportPosition;
				this.Controller.enabled = true;
				this.teleport = false;
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000A8E9C File Offset: 0x000A709C
		protected virtual void Move()
		{
			this.isSprinting = false;
			if (!this.Controller.enabled)
			{
				this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
				return;
			}
			if (this.currentVehicle != null)
			{
				return;
			}
			if (this.IsGrounded)
			{
				this.timeGrounded += Time.deltaTime;
			}
			else
			{
				this.timeGrounded = 0f;
			}
			if (this.canMove && this.canJump && this.IsGrounded && !this.isJumping && !GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				if (!this.isCrouched)
				{
					this.isJumping = true;
					if (this.onJump != null)
					{
						this.onJump();
					}
					Player.Local.PlayJumpAnimation();
					base.StartCoroutine(this.Jump());
				}
				else
				{
					this.TryToggleCrouch();
				}
			}
			if (this.canMove && !GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && GameInput.GetButtonDown(GameInput.ButtonCode.Crouch))
			{
				this.TryToggleCrouch();
			}
			if (!this.IsGrounded)
			{
				this.airTime += Time.deltaTime;
			}
			else
			{
				this.isJumping = false;
				if (this.airTime > 0.1f && this.onLand != null)
				{
					this.onLand();
				}
				this.airTime = 0f;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Sprint) && !this.sprintActive)
			{
				this.sprintActive = true;
				this.sprintReleased = false;
			}
			else if (GameInput.GetButton(GameInput.ButtonCode.Sprint) && Singleton<Settings>.Instance.SprintMode == InputSettings.EActionMode.Hold)
			{
				this.sprintActive = true;
			}
			else if (Singleton<Settings>.Instance.SprintMode == InputSettings.EActionMode.Hold)
			{
				this.sprintActive = false;
			}
			if (!GameInput.GetButton(GameInput.ButtonCode.Sprint))
			{
				this.sprintReleased = true;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Sprint) && this.sprintReleased)
			{
				this.sprintActive = !this.sprintActive;
			}
			if (this.ForceSprint)
			{
				this.sprintActive = true;
			}
			this.isSprinting = false;
			if (this.sprintActive && this.canMove && !this.isCrouched && !Player.Local.IsTased && (this.horizontalAxis != 0f || this.verticalAxis != 0f) && this.sprintBlockers.Count == 0)
			{
				if (this.CurrentStaminaReserve > 0f || !this.SprintingRequiresStamina || this.ForceSprint)
				{
					this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, PlayerMovement.SprintMultiplier, Time.deltaTime * 4f);
					if (this.SprintingRequiresStamina && !this.ForceSprint)
					{
						this.ChangeStamina(-12.5f * Time.deltaTime, true);
					}
					this.isSprinting = true;
				}
				else
				{
					this.sprintActive = false;
					this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
				}
			}
			else
			{
				this.sprintActive = false;
				this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
			}
			if (!this.isSprinting && this.timeSinceStaminaDrain > 1f)
			{
				this.CurrentSprintMultiplier = Mathf.MoveTowards(this.CurrentSprintMultiplier, 1f, Time.deltaTime * 4f);
			}
			float num = 1f;
			if (this.isCrouched)
			{
				num = 1f - (1f - this.crouchSpeedMultipler) * (1f - this.standingScale);
			}
			float num2 = PlayerMovement.WalkSpeed * this.CurrentSprintMultiplier * num * PlayerMovement.StaticMoveSpeedMultiplier * this.MoveSpeedMultiplier;
			if (Player.Local.IsTased)
			{
				num2 *= 0.5f;
			}
			if ((Application.isEditor || Debug.isDebugBuild) && this.isSprinting)
			{
				num2 *= 1f;
			}
			if (this.Controller.isGrounded)
			{
				if (this.canMove)
				{
					Vector3 vector = this.movement;
					this.movement = new Vector3(this.horizontalAxis, -this.Controller.stepOffset, this.verticalAxis);
					this.movement = base.transform.TransformDirection(this.movement);
					this.ClampMovement();
					this.movement.x = this.movement.x * num2;
					this.movement.z = this.movement.z * num2;
				}
				else
				{
					this.movement = new Vector3(0f, -this.Controller.stepOffset, 0f);
				}
			}
			else if (this.canMove)
			{
				Vector3 vector2 = this.movement;
				this.movement = new Vector3(this.horizontalAxis, this.movement.y, this.verticalAxis);
				this.movement = base.transform.TransformDirection(this.movement);
				this.ClampMovement();
				this.movement.x = this.movement.x * num2;
				this.movement.z = this.movement.z * num2;
			}
			else
			{
				this.movement = new Vector3(0f, this.movement.y, 0f);
			}
			if (!this.canMove)
			{
				this.movement.x = Mathf.MoveTowards(this.movement.x, 0f, this.sensitivity * Time.deltaTime);
				this.movement.z = Mathf.MoveTowards(this.movement.z, 0f, this.sensitivity * Time.deltaTime);
			}
			this.movement.y = this.movement.y + Physics.gravity.y * this.gravityMultiplier * Time.deltaTime * PlayerMovement.GravityMultiplier;
			this.movement.y = this.movement.y + this.movementY;
			this.movementY = 0f;
			if (this.residualVelocityTimeRemaining > 0f)
			{
				this.movement += this.residualVelocityDirection * this.residualVelocityForce * Mathf.Clamp01(this.residualVelocityTimeRemaining / this.residualVelocityDuration) * Time.deltaTime;
			}
			if (Player.Local.Slippery)
			{
				this.movement = Vector3.Lerp(this.movement, new Vector3(this.lastFrameMovement.x, this.movement.y, this.lastFrameMovement.z), this.SlipperyMovementMultiplier);
			}
			float surfaceAngle = this.GetSurfaceAngle();
			if ((this.horizontalAxis != 0f || this.verticalAxis != 0f) && surfaceAngle > 5f)
			{
				float d = Mathf.Clamp01(surfaceAngle / this.Controller.slopeLimit);
				Vector3 b = Vector3.down * Time.deltaTime * this.slopeForce * d;
				this.Controller.Move(this.movement * Time.deltaTime + b);
			}
			else
			{
				this.Controller.Move(this.movement * Time.deltaTime);
			}
			this.lastFrameMovement = this.movement;
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000A9598 File Offset: 0x000A7798
		private void ClampMovement()
		{
			float y = this.movement.y;
			this.movement = Vector3.ClampMagnitude(new Vector3(this.movement.x, 0f, this.movement.z), 1f);
			this.movement.y = y;
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000A95F0 File Offset: 0x000A77F0
		protected float GetSurfaceAngle()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, ref raycastHit, this.slopeForceRayLength, this.groundDetectionMask))
			{
				return Vector3.Angle(raycastHit.normal, Vector3.up);
			}
			return 0f;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000A963E File Offset: 0x000A783E
		private bool isGrounded()
		{
			return Player.Local.GetIsGrounded();
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x000A964C File Offset: 0x000A784C
		protected void UpdateHorizontalAxis()
		{
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				this.horizontalAxis = 0f;
				return;
			}
			int num = (!GameInput.IsTyping) ? Mathf.RoundToInt(GameInput.MotionAxis.x) : 0;
			if (this.Player.Disoriented)
			{
				num = -num;
			}
			if (this.Player.Schizophrenic && Time.timeSinceLevelLoad % 20f < 1f)
			{
				num = -num;
			}
			float num2 = Mathf.MoveTowards(this.horizontalAxis, (float)num, this.sensitivity * Time.deltaTime);
			this.horizontalAxis = ((Mathf.Abs(num2) < this.dead) ? 0f : num2);
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000A96F4 File Offset: 0x000A78F4
		protected void UpdateVerticalAxis()
		{
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				this.verticalAxis = 0f;
				return;
			}
			int num = (!GameInput.IsTyping) ? Mathf.RoundToInt(GameInput.MotionAxis.y) : 0;
			if (this.Player.Schizophrenic && (Time.timeSinceLevelLoad + 5f) % 25f < 1f)
			{
				num = -num;
			}
			float num2 = Mathf.MoveTowards(this.verticalAxis, (float)num, this.sensitivity * Time.deltaTime);
			this.verticalAxis = ((Mathf.Abs(num2) < this.dead) ? 0f : num2);
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000A9791 File Offset: 0x000A7991
		private IEnumerator Jump()
		{
			float savedSlopeLimit = this.Controller.slopeLimit;
			this.Controller.velocity.Set(this.Controller.velocity.x, 0f, this.Controller.velocity.y);
			this.movementY += this.jumpForce * PlayerMovement.JumpMultiplier;
			this.timeGrounded = 0f;
			do
			{
				yield return new WaitForEndOfFrame();
			}
			while (this.timeGrounded < 0.05f && this.Controller.collisionFlags != 2 && this.currentVehicle == null);
			this.Controller.slopeLimit = savedSlopeLimit;
			yield break;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x000A97A0 File Offset: 0x000A79A0
		private void TryToggleCrouch()
		{
			if (this.isCrouched)
			{
				if (this.CanStand())
				{
					this.SetCrouched(false);
					return;
				}
			}
			else
			{
				this.SetCrouched(true);
			}
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000A97C4 File Offset: 0x000A79C4
		public bool CanStand()
		{
			float num = this.Controller.radius * 0.75f;
			float num2 = 0.1f;
			Vector3 vector = base.transform.position - Vector3.up * this.Controller.height * 0.5f + Vector3.up * num + Vector3.up * num2;
			float num3 = this.playerHeight - num * 2f - num2;
			RaycastHit raycastHit;
			return !Physics.SphereCast(vector, num, Vector3.up, ref raycastHit, num3, this.groundDetectionMask);
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000A9868 File Offset: 0x000A7A68
		public void SetCrouched(bool c)
		{
			this.isCrouched = c;
			this.Player.SendCrouched(this.isCrouched);
			this.Player.SetCrouchedLocal(this.isCrouched);
			VisibilityAttribute visibilityAttribute = Player.Local.Visibility.GetAttribute("Crouched");
			if (this.isCrouched)
			{
				if (visibilityAttribute == null)
				{
					visibilityAttribute = new VisibilityAttribute("Crouched", 0f, 0.8f, 1);
					return;
				}
			}
			else if (visibilityAttribute != null)
			{
				visibilityAttribute.Delete();
			}
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000A98E0 File Offset: 0x000A7AE0
		private void UpdateCrouchVignetteEffect()
		{
			if (!Singleton<PostProcessingManager>.InstanceExists)
			{
				return;
			}
			float intensity = Mathf.Lerp(this.Crouched_VigIntensity, Singleton<PostProcessingManager>.Instance.Vig_DefaultIntensity, this.standingScale);
			float smoothness = Mathf.Lerp(this.Crouched_VigSmoothness, Singleton<PostProcessingManager>.Instance.Vig_DefaultSmoothness, this.standingScale);
			Singleton<PostProcessingManager>.Instance.OverrideVignette(intensity, smoothness);
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000A993C File Offset: 0x000A7B3C
		private void UpdatePlayerHeight()
		{
			float height = this.Controller.height;
			this.Controller.height = this.playerHeight - this.playerHeight * (1f - PlayerMovement.CrouchHeightMultiplier) * (1f - this.standingScale);
			float num = this.Controller.height - height;
			if (this.IsGrounded && Mathf.Abs(num) > 1E-05f)
			{
				this.movementY += num * 0.5f;
			}
			if (Mathf.Abs(num) > 0.0001f)
			{
				for (int i = 0; i < this.visibilityPointsToScale.Count; i++)
				{
					this.visibilityPointsToScale[i].localPosition = new Vector3(this.visibilityPointsToScale[i].localPosition.x, this.originalVisibilityPointOffsets[this.visibilityPointsToScale[i]] * (this.Controller.height / this.playerHeight), this.visibilityPointsToScale[i].localPosition.z);
				}
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000A9A4F File Offset: 0x000A7C4F
		public void LerpPlayerRotation(Quaternion rotation, float lerpTime)
		{
			if (this.playerRotCoroutine != null)
			{
				base.StopCoroutine(this.playerRotCoroutine);
			}
			this.playerRotCoroutine = base.StartCoroutine(this.LerpPlayerRotation_Process(rotation, lerpTime));
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000A9A79 File Offset: 0x000A7C79
		private IEnumerator LerpPlayerRotation_Process(Quaternion endRotation, float lerpTime)
		{
			Quaternion startRot = this.Player.transform.rotation;
			this.Controller.enabled = false;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.Player.transform.rotation = Quaternion.Lerp(startRot, endRotation, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.Player.transform.rotation = endRotation;
			this.Controller.enabled = true;
			this.playerRotCoroutine = null;
			yield break;
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000A9A98 File Offset: 0x000A7C98
		private void EnterVehicle(LandVehicle vehicle)
		{
			this.currentVehicle = vehicle;
			this.canMove = false;
			this.Controller.enabled = false;
			if (this.recentlyDrivenVehicles.Contains(vehicle))
			{
				this.recentlyDrivenVehicles.Remove(vehicle);
			}
			this.recentlyDrivenVehicles.Insert(0, vehicle);
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000A9AE7 File Offset: 0x000A7CE7
		private void ExitVehicle(LandVehicle veh, Transform exitPoint)
		{
			this.currentVehicle = null;
			this.canMove = true;
			this.Controller.enabled = true;
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000A9B04 File Offset: 0x000A7D04
		public void Teleport(Vector3 position)
		{
			string str = "Player teleported: ";
			Vector3 vector = position;
			Console.Log(str + vector.ToString(), null);
			if (this.Player.ActiveSkateboard != null)
			{
				this.Player.ActiveSkateboard.Equippable.Dismount();
			}
			this.Controller.enabled = false;
			this.Controller.transform.position = position;
			this.Controller.enabled = true;
			this.teleport = true;
			this.teleportPosition = position;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000A9B8F File Offset: 0x000A7D8F
		public void SetResidualVelocity(Vector3 dir, float force, float time)
		{
			this.residualVelocityDirection = dir.normalized;
			this.residualVelocityForce = force;
			this.residualVelocityDuration = time;
			this.residualVelocityTimeRemaining = time;
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000A9BB4 File Offset: 0x000A7DB4
		public void WarpToNavMesh()
		{
			NavMeshQueryFilter navMeshQueryFilter = default(NavMeshQueryFilter);
			navMeshQueryFilter.agentTypeID = Singleton<PlayerManager>.Instance.PlayerRecoverySurface.agentTypeID;
			navMeshQueryFilter.areaMask = -1;
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(PlayerSingleton<PlayerMovement>.Instance.transform.position, ref navMeshHit, 100f, navMeshQueryFilter))
			{
				PlayerSingleton<PlayerMovement>.Instance.Teleport(navMeshHit.position + Vector3.up * 1f);
				return;
			}
			Console.LogError("Failed to find recovery point!", null);
			PlayerSingleton<PlayerMovement>.Instance.Teleport(Vector3.up * 5f);
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000A9C50 File Offset: 0x000A7E50
		public void RegisterMovementEvent(int threshold, Action action)
		{
			if (threshold < 1)
			{
				Console.LogWarning("Movement events min. threshold is 1m!", null);
				return;
			}
			if (!this.movementEvents.ContainsKey(threshold))
			{
				this.movementEvents.Add(threshold, new PlayerMovement.MovementEvent());
			}
			this.movementEvents[threshold].actions.Add(action);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000A9CA4 File Offset: 0x000A7EA4
		public void DeregisterMovementEvent(Action action)
		{
			foreach (int key in this.movementEvents.Keys)
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[key];
				if (movementEvent.actions.Contains(action))
				{
					movementEvent.actions.Remove(action);
					break;
				}
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000A9D20 File Offset: 0x000A7F20
		private void UpdateMovementEvents()
		{
			foreach (int num in this.movementEvents.Keys.ToList<int>())
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[num];
				if (Vector3.Distance(this.Player.Avatar.CenterPoint, movementEvent.LastUpdatedDistance) > (float)num)
				{
					movementEvent.Update(this.Player.Avatar.CenterPoint);
				}
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000A9DB8 File Offset: 0x000A7FB8
		public void ChangeStamina(float change, bool notify = true)
		{
			if (change < 0f)
			{
				this.timeSinceStaminaDrain = 0f;
			}
			this.SetStamina(this.CurrentStaminaReserve + change, notify);
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000A9DDC File Offset: 0x000A7FDC
		public void SetStamina(float value, bool notify = true)
		{
			if (this.CurrentStaminaReserve == value)
			{
				return;
			}
			float currentStaminaReserve = this.CurrentStaminaReserve;
			this.CurrentStaminaReserve = Mathf.Clamp(value, 0f, PlayerMovement.StaminaReserveMax);
			if (notify && this.onStaminaReserveChanged != null)
			{
				this.onStaminaReserveChanged(this.CurrentStaminaReserve - currentStaminaReserve);
			}
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000A9E2E File Offset: 0x000A802E
		public void AddSprintBlocker(string tag)
		{
			if (!this.sprintBlockers.Contains(tag))
			{
				this.sprintBlockers.Add(tag);
			}
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000A9E4A File Offset: 0x000A804A
		public void RemoveSprintBlocker(string tag)
		{
			if (this.sprintBlockers.Contains(tag))
			{
				this.sprintBlockers.Remove(tag);
			}
		}

		// Token: 0x04001D78 RID: 7544
		public const float DEV_SPRINT_MULTIPLIER = 1f;

		// Token: 0x04001D79 RID: 7545
		public const float GROUNDED_THRESHOLD = 0.05f;

		// Token: 0x04001D7A RID: 7546
		public const float SLOPE_THRESHOLD = 5f;

		// Token: 0x04001D7B RID: 7547
		public static float WalkSpeed = 3.25f;

		// Token: 0x04001D7C RID: 7548
		public static float SprintMultiplier = 1.9f;

		// Token: 0x04001D7D RID: 7549
		public static float StaticMoveSpeedMultiplier = 1f;

		// Token: 0x04001D7E RID: 7550
		public const float StaminaRestoreDelay = 1f;

		// Token: 0x04001D7F RID: 7551
		public static float JumpMultiplier = 1f;

		// Token: 0x04001D80 RID: 7552
		public static float ControllerRadius = 0.35f;

		// Token: 0x04001D81 RID: 7553
		public static float StandingControllerHeight = 1.85f;

		// Token: 0x04001D82 RID: 7554
		public static float CrouchHeightMultiplier = 0.65f;

		// Token: 0x04001D83 RID: 7555
		public static float CrouchTime = 0.2f;

		// Token: 0x04001D85 RID: 7557
		public const float StaminaDrainRate = 12.5f;

		// Token: 0x04001D86 RID: 7558
		public const float StaminaRestoreRate = 25f;

		// Token: 0x04001D87 RID: 7559
		public static float StaminaReserveMax = 100f;

		// Token: 0x04001D88 RID: 7560
		public const float SprintChangeRate = 4f;

		// Token: 0x04001D89 RID: 7561
		[Header("References")]
		public Player Player;

		// Token: 0x04001D8A RID: 7562
		public CharacterController Controller;

		// Token: 0x04001D8B RID: 7563
		[Header("Move settings")]
		[SerializeField]
		protected float sensitivity = 6f;

		// Token: 0x04001D8C RID: 7564
		[SerializeField]
		protected float dead = 0.001f;

		// Token: 0x04001D8D RID: 7565
		public bool canMove = true;

		// Token: 0x04001D8E RID: 7566
		public bool canJump = true;

		// Token: 0x04001D8F RID: 7567
		public bool SprintingRequiresStamina = true;

		// Token: 0x04001D90 RID: 7568
		public float MoveSpeedMultiplier = 1f;

		// Token: 0x04001D91 RID: 7569
		public float SlipperyMovementMultiplier = 1f;

		// Token: 0x04001D92 RID: 7570
		public bool ForceSprint;

		// Token: 0x04001D93 RID: 7571
		[Header("Jump/fall settings")]
		[SerializeField]
		protected float jumpForce = 4.5f;

		// Token: 0x04001D94 RID: 7572
		[SerializeField]
		protected float gravityMultiplier = 1f;

		// Token: 0x04001D95 RID: 7573
		[SerializeField]
		protected LayerMask groundDetectionMask;

		// Token: 0x04001D96 RID: 7574
		[Header("Slope Settings")]
		[SerializeField]
		protected float slopeForce;

		// Token: 0x04001D97 RID: 7575
		[SerializeField]
		protected float slopeForceRayLength;

		// Token: 0x04001D98 RID: 7576
		[Header("Crouch Settings")]
		[SerializeField]
		protected float crouchSpeedMultipler = 0.5f;

		// Token: 0x04001D99 RID: 7577
		[SerializeField]
		protected float Crouched_VigIntensity = 0.8f;

		// Token: 0x04001D9A RID: 7578
		[SerializeField]
		protected float Crouched_VigSmoothness = 1f;

		// Token: 0x04001D9B RID: 7579
		[Header("Visibility Points")]
		[SerializeField]
		protected List<Transform> visibilityPointsToScale = new List<Transform>();

		// Token: 0x04001D9C RID: 7580
		private Dictionary<Transform, float> originalVisibilityPointOffsets = new Dictionary<Transform, float>();

		// Token: 0x04001D9E RID: 7582
		protected Vector3 movement = Vector3.zero;

		// Token: 0x04001D9F RID: 7583
		protected float movementY;

		// Token: 0x04001DA1 RID: 7585
		public List<LandVehicle> recentlyDrivenVehicles = new List<LandVehicle>();

		// Token: 0x04001DA2 RID: 7586
		private bool isJumping;

		// Token: 0x04001DA9 RID: 7593
		public float CurrentStaminaReserve = PlayerMovement.StaminaReserveMax;

		// Token: 0x04001DAB RID: 7595
		public Action<float> onStaminaReserveChanged;

		// Token: 0x04001DAC RID: 7596
		public Action onJump;

		// Token: 0x04001DAD RID: 7597
		public Action onLand;

		// Token: 0x04001DAE RID: 7598
		public UnityEvent onCrouch;

		// Token: 0x04001DAF RID: 7599
		public UnityEvent onUncrouch;

		// Token: 0x04001DB0 RID: 7600
		protected float horizontalAxis;

		// Token: 0x04001DB1 RID: 7601
		protected float verticalAxis;

		// Token: 0x04001DB2 RID: 7602
		protected float timeGrounded;

		// Token: 0x04001DB3 RID: 7603
		private Dictionary<int, PlayerMovement.MovementEvent> movementEvents = new Dictionary<int, PlayerMovement.MovementEvent>();

		// Token: 0x04001DB4 RID: 7604
		private float timeSinceStaminaDrain = 10000f;

		// Token: 0x04001DB5 RID: 7605
		private bool sprintActive;

		// Token: 0x04001DB6 RID: 7606
		private bool sprintReleased;

		// Token: 0x04001DB7 RID: 7607
		private Vector3 residualVelocityDirection = Vector3.zero;

		// Token: 0x04001DB8 RID: 7608
		private float residualVelocityForce;

		// Token: 0x04001DB9 RID: 7609
		private float residualVelocityDuration;

		// Token: 0x04001DBA RID: 7610
		private float residualVelocityTimeRemaining;

		// Token: 0x04001DBB RID: 7611
		private bool teleport;

		// Token: 0x04001DBC RID: 7612
		private Vector3 teleportPosition = Vector3.zero;

		// Token: 0x04001DBD RID: 7613
		private List<string> sprintBlockers = new List<string>();

		// Token: 0x04001DBE RID: 7614
		private Vector3 lastFrameMovement = Vector3.zero;

		// Token: 0x04001DBF RID: 7615
		private Coroutine playerRotCoroutine;

		// Token: 0x02000638 RID: 1592
		public class MovementEvent
		{
			// Token: 0x06002934 RID: 10548 RVA: 0x000AA00C File Offset: 0x000A820C
			public void Update(Vector3 newPosition)
			{
				this.LastUpdatedDistance = newPosition;
				foreach (Action action in this.actions)
				{
					action();
				}
			}

			// Token: 0x04001DC0 RID: 7616
			public List<Action> actions = new List<Action>();

			// Token: 0x04001DC1 RID: 7617
			public Vector3 LastUpdatedDistance = Vector3.zero;
		}
	}
}
