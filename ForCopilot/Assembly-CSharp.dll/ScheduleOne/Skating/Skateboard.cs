using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Skating
{
	// Token: 0x020002D7 RID: 727
	public class Skateboard : NetworkBehaviour
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x000441A9 File Offset: 0x000423A9
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x000441B1 File Offset: 0x000423B1
		public float CurrentSteerInput { get; protected set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x000441BA File Offset: 0x000423BA
		public bool IsPushing
		{
			get
			{
				return this.isPushing;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x000441C2 File Offset: 0x000423C2
		public float TimeSincePushStart
		{
			get
			{
				return this.timeSincePushStart;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x000441CA File Offset: 0x000423CA
		public bool isGrounded
		{
			get
			{
				return this.timeGrounded > 0f;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x000441D9 File Offset: 0x000423D9
		public float AirTime
		{
			get
			{
				return this.timeAirborne;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x000441E1 File Offset: 0x000423E1
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x000441E9 File Offset: 0x000423E9
		public float JumpBuildAmount
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<JumpBuildAmount>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_JumpBuildAmount_431000436(value);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x000441F5 File Offset: 0x000423F5
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x000441FD File Offset: 0x000423FD
		public Player Rider { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x00044206 File Offset: 0x00042406
		public float TopSpeed_Ms
		{
			get
			{
				return this.TopSpeed_Kmh / 3.6f;
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00044214 File Offset: 0x00042414
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Skating.Skateboard_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00044233 File Offset: 0x00042433
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (base.IsOwner)
			{
				this.Rider = Player.Local;
			}
			else
			{
				this.Rider = Player.GetPlayer(base.Owner);
			}
			this.ApplyPlayerScale();
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00044267 File Offset: 0x00042467
		public void Update()
		{
			this.GetInput();
			if (base.IsOwner)
			{
				this.Rb.interpolation = 1;
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00044284 File Offset: 0x00042484
		private void GetInput()
		{
			if (!base.IsOwner)
			{
				return;
			}
			if (Player.Local.IsTased || Player.Local.Seizure)
			{
				if (UnityEngine.Random.Range(0f, 1f) > 0.9f)
				{
					this.horizontalInput = UnityEngine.Random.Range(-1, 2);
				}
			}
			else
			{
				this.horizontalInput = 0;
				if (GameInput.GetButton(GameInput.ButtonCode.Left))
				{
					if (Player.Local.Disoriented)
					{
						this.horizontalInput++;
					}
					else
					{
						this.horizontalInput--;
					}
				}
				if (GameInput.GetButton(GameInput.ButtonCode.Right))
				{
					if (Player.Local.Disoriented)
					{
						this.horizontalInput--;
					}
					else
					{
						this.horizontalInput++;
					}
				}
			}
			this.jumpReleased = false;
			if (GameInput.GetButton(GameInput.ButtonCode.Jump))
			{
				this.jumpHeldTime += Time.deltaTime;
			}
			else if (this.jumpHeldTime > 0f)
			{
				this.jumpReleased = true;
			}
			this.JumpBuildAmount = Mathf.Clamp01(this.jumpHeldTime / 0.5f);
			this.braking = GameInput.GetButton(GameInput.ButtonCode.Backward);
			if (GameInput.GetButton(GameInput.ButtonCode.Forward) && !this.isPushing && this.timeGrounded > 0.1f && !this.braking && this.timeSincePushStart >= 1f && this.jumpHeldTime == 0f && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve >= 12.5f && !Player.Local.IsTased && !this.pushQueued)
			{
				this.pushQueued = true;
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00044408 File Offset: 0x00042608
		private void FixedUpdate()
		{
			if (!base.IsOwner)
			{
				this.CurrentSpeed_Kmh = this.VelocityCalculator.Velocity.magnitude * 3.6f;
				this.CheckGrounded();
				return;
			}
			this.ApplyInput();
			this.ApplyLateralFriction();
			this.UpdateHover();
			this.CheckGrounded();
			this.CheckJump();
			this.ApplyGravity();
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00044464 File Offset: 0x00042664
		private void LateUpdate()
		{
			if (!base.IsOwner)
			{
				return;
			}
			this.ClampRotation();
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00044478 File Offset: 0x00042678
		private void ApplyInput()
		{
			Vector3 vector = base.transform.InverseTransformVector(this.Rb.velocity);
			bool flag = false;
			if (this.horizontalInput == 1)
			{
				this.CurrentSteerInput = Mathf.MoveTowards(this.CurrentSteerInput, 1f, Time.fixedDeltaTime * this.TurnChangeRate);
				flag = true;
			}
			else if (this.horizontalInput == -1)
			{
				this.CurrentSteerInput = Mathf.MoveTowards(this.CurrentSteerInput, -1f, Time.fixedDeltaTime * this.TurnChangeRate);
				flag = true;
			}
			else
			{
				this.CurrentSteerInput = Mathf.MoveTowards(this.CurrentSteerInput, 0f, Time.fixedDeltaTime * this.TurnReturnToRestRate);
			}
			float num = this.CurrentSteerInput * this.TurnForce * this.TurnForceMap.Evaluate(Mathf.Clamp01(Mathf.Abs(this.CurrentSpeed_Kmh / this.TopSpeed_Kmh)));
			if (vector.z < 0f)
			{
				num *= -1f;
			}
			this.Rb.AddTorque(base.transform.up * num, 5);
			if (flag)
			{
				this.Rb.AddForce(base.transform.forward * Mathf.Abs(this.CurrentSteerInput) * this.TurnSpeedBoost, 5);
			}
			this.timeSincePushStart += Time.deltaTime;
			if (this.pushQueued)
			{
				this.Push();
			}
			if (this.isPushing)
			{
				float d = this.PushForceMultiplierMap.Evaluate(Mathf.Clamp01(this.Rb.velocity.magnitude / this.TopSpeed_Ms));
				this.Rb.AddForce(base.transform.forward * this.thisFramePushForce * this.PushForceMultiplier * d, 5);
			}
			if (this.timeGrounded == 0f && this.AirMovementEnabled)
			{
				float d2 = 1f;
				if (this.timeAirborne < this.AirMovementJumpReductionDuration)
				{
					d2 = this.AirMovementJumpReductionCurve.Evaluate(this.timeAirborne / this.AirMovementJumpReductionDuration);
				}
				this.Rb.AddTorque(base.transform.right * GameInput.MotionAxis.y * this.AirMovementForce * d2, 5);
			}
			if (this.braking)
			{
				float d3 = 1f;
				if (vector.z < 0f)
				{
					float num2 = Mathf.Clamp01(vector.z / -this.ReverseTopSpeed_Kmh);
					d3 = 1f - num2;
				}
				this.Rb.AddForce(-base.transform.forward * this.BrakeForce * d3, 5);
			}
			float magnitude = this.Rb.velocity.magnitude;
			this.CurrentSpeed_Kmh = magnitude * 3.6f;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00044748 File Offset: 0x00042948
		private void ApplyLateralFriction()
		{
			if (!this.FrictionEnabled)
			{
				return;
			}
			Vector3 vector = base.transform.InverseTransformVector(this.Rb.velocity);
			Vector3 vector2 = Vector3.zero;
			float d = vector.x * this.LateralFrictionForceMultiplier;
			vector2 += -base.transform.right * d;
			float num = this.LongitudinalFrictionCurve.Evaluate(Mathf.Clamp01(vector.z) / this.TopSpeed_Ms);
			float d2 = vector.z * num;
			Vector3 a = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
			vector2 += -a * d2;
			this.Rb.AddForce(vector2, 5);
			Vector3 velocity = this.Rb.velocity;
			float surfaceSmoothness = this.GetSurfaceSmoothness();
			this.Rb.AddForce(-velocity * (1f - surfaceSmoothness), 5);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0004483C File Offset: 0x00042A3C
		private void UpdateHover()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < this.HoverPoints.Length; i++)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(this.HoverPoints[i].position, -this.HoverPoints[i].up, ref raycastHit, this.HoverRayLength, this.GroundDetectionMask))
				{
					list.Add(raycastHit.distance);
					Debug.DrawLine(this.HoverPoints[i].position, raycastHit.point, Color.red);
					PID pid = this.hoverPIDs[i];
					pid.pFactor = this.Hover_P;
					pid.iFactor = this.Hover_I;
					pid.dFactor = this.Hover_D;
					float num = pid.Update(this.HoverHeight, raycastHit.distance, Time.fixedDeltaTime);
					num *= this.HoverForce;
					num = Mathf.Max(num, 0f);
					this.Rb.AddForceAtPosition(this.HoverPoints[i].up * num, this.HoverPoints[i].position, 5);
				}
				else
				{
					list.Add(this.HoverRayLength);
					Debug.DrawRay(this.HoverPoints[i].position, -this.HoverPoints[i].up * this.HoverRayLength, Color.blue);
					this.hoverPIDs[i].Update(this.HoverHeight, this.HoverRayLength, Time.fixedDeltaTime);
				}
			}
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x000449BD File Offset: 0x00042BBD
		private void ApplyGravity()
		{
			this.Rb.AddForce(Vector3.down * this.Gravity * Mathf.Sqrt(PlayerMovement.GravityMultiplier), 5);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000449EC File Offset: 0x00042BEC
		private void CheckGrounded()
		{
			if (this.IsGrounded())
			{
				this.timeGrounded += Time.fixedDeltaTime;
				if (this.timeGrounded > 0.02f)
				{
					if (this.timeAirborne > 0.2f && this.OnLand != null)
					{
						this.OnLand.Invoke();
					}
					this.timeAirborne = 0f;
					return;
				}
			}
			else
			{
				this.timeAirborne += Time.fixedDeltaTime;
				this.timeGrounded = 0f;
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00044A6C File Offset: 0x00042C6C
		private void CheckJump()
		{
			this.timeSinceLastJump += Time.fixedDeltaTime;
			if (this.frontAxleForce > 0f)
			{
				this.Rb.AddForceAtPosition(Vector3.up * this.frontAxleForce, this.FrontAxlePosition.position, 5);
			}
			if (this.frontAxleForce > 0f)
			{
				this.Rb.AddForceAtPosition(Vector3.up * this.rearAxleForce, this.RearAxlePosition.position, 5);
			}
			if (this.jumpForwardForce > 0f)
			{
				this.Rb.AddForce(Vector3.ProjectOnPlane(base.transform.forward, Vector3.up) * this.jumpForwardForce, 5);
			}
			if (this.jumpReleased)
			{
				if (this.timeGrounded > 0.3f)
				{
					this.Jump();
				}
				this.jumpHeldTime = 0f;
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00044B52 File Offset: 0x00042D52
		[ServerRpc(RunLocally = true)]
		private void SendJump(float jumpHeldTime)
		{
			this.RpcWriter___Server_SendJump_431000436(jumpHeldTime);
			this.RpcLogic___SendJump_431000436(jumpHeldTime);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00044B68 File Offset: 0x00042D68
		[ObserversRpc(RunLocally = true)]
		private void ReceiveJump(float _jumpHeldTime)
		{
			this.RpcWriter___Observers_ReceiveJump_431000436(_jumpHeldTime);
			this.RpcLogic___ReceiveJump_431000436(_jumpHeldTime);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00044B8C File Offset: 0x00042D8C
		private void Jump()
		{
			Skateboard.<>c__DisplayClass112_0 CS$<>8__locals1 = new Skateboard.<>c__DisplayClass112_0();
			CS$<>8__locals1.<>4__this = this;
			this.SendJump(this.jumpHeldTime);
			float t = Mathf.Clamp01(this.jumpHeldTime / 0.5f);
			CS$<>8__locals1.JumpDuration = Mathf.Lerp(this.JumpDuration_Min, this.JumpDuration_Max, t);
			base.StartCoroutine(CS$<>8__locals1.<Jump>g__Jump|0());
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00044BE9 File Offset: 0x00042DE9
		private void Push()
		{
			this.pushQueued = false;
			this.isPushing = true;
			this.timeSincePushStart = 0f;
			if (this.OnPushStart != null)
			{
				this.OnPushStart.Invoke();
			}
			base.StartCoroutine(this.<Push>g__Push|113_0());
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00044C24 File Offset: 0x00042E24
		public bool IsGrounded()
		{
			RaycastHit raycastHit;
			return this.IsGrounded(out raycastHit);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00044C3C File Offset: 0x00042E3C
		public bool IsGrounded(out RaycastHit hit)
		{
			return Physics.Raycast(this.FrontAxlePosition.position + base.transform.up * 0.01f, -base.transform.up, ref hit, this.HoverRayLength + 0.02f, this.GroundDetectionMask) || Physics.Raycast(this.RearAxlePosition.position + base.transform.up * 0.01f, -base.transform.up, ref hit, this.HoverRayLength + 0.02f, this.GroundDetectionMask);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00044CF6 File Offset: 0x00042EF6
		public void SetVelocity(Vector3 velocity)
		{
			this.Rb.isKinematic = false;
			this.Rb.velocity = velocity;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00044D10 File Offset: 0x00042F10
		private void ClampRotation()
		{
			Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
			Vector3 normalized2 = Vector3.ProjectOnPlane(base.transform.right, Vector3.up).normalized;
			float num = Vector3.SignedAngle(base.transform.forward, normalized, base.transform.right);
			float num2 = Vector3.SignedAngle(normalized2, base.transform.right, base.transform.forward);
			if (Mathf.Abs(num) > 60f)
			{
				this.Rb.AddTorque(base.transform.right * num * this.RotationClampForce, 5);
			}
			if (Mathf.Abs(num2) > 20f)
			{
				this.Rb.AddTorque(base.transform.forward * -num2 * this.RotationClampForce, 5);
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00044DFC File Offset: 0x00042FFC
		public void ApplyPlayerScale()
		{
			if (this.Rider == null)
			{
				return;
			}
			this.IKAlignmentsContainer.localScale = Vector3.one * this.Rider.Scale;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00044E30 File Offset: 0x00043030
		public float GetSurfaceSmoothness()
		{
			RaycastHit raycastHit;
			if (!this.IsGrounded(out raycastHit))
			{
				return 1f;
			}
			if (raycastHit.collider.gameObject.tag == "Terrain" && this.SlowOnTerrain)
			{
				return 0.4f;
			}
			return 1f;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00044E80 File Offset: 0x00043080
		public bool IsOnTerrain()
		{
			RaycastHit raycastHit;
			return this.IsGrounded(out raycastHit) && raycastHit.collider.gameObject.tag == "Terrain";
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00045023 File Offset: 0x00043223
		[CompilerGenerated]
		private IEnumerator <Push>g__Push|113_0()
		{
			yield return new WaitForSeconds(this.PushDelay);
			float i = 0f;
			while (i < this.PushForceDuration && !this.braking && this.timeGrounded != 0f)
			{
				this.thisFramePushForce = this.PushForceCurve.Evaluate(i / this.PushForceDuration);
				PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-(12.5f * Time.deltaTime) / this.PushForceDuration, true);
				yield return new WaitForEndOfFrame();
				i += Time.deltaTime;
			}
			this.isPushing = false;
			this.thisFramePushForce = 0f;
			yield break;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00045034 File Offset: 0x00043234
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Skating.SkateboardAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Skating.SkateboardAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<JumpBuildAmount>k__BackingField = new SyncVar<float>(this, 0U, 0, 0, -1f, 1, this.<JumpBuildAmount>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_JumpBuildAmount_431000436));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SendJump_431000436));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveJump_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Skating.Skateboard));
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x000450D4 File Offset: 0x000432D4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Skating.SkateboardAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Skating.SkateboardAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<JumpBuildAmount>k__BackingField.SetRegistered();
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x000450F2 File Offset: 0x000432F2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00045100 File Offset: 0x00043300
		private void RpcWriter___Server_set_JumpBuildAmount_431000436(float value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(value, 0);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00045206 File Offset: 0x00043406
		public void RpcLogic___set_JumpBuildAmount_431000436(float value)
		{
			this.sync___set_value_<JumpBuildAmount>k__BackingField(value, true);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00045210 File Offset: 0x00043410
		private void RpcReader___Server_set_JumpBuildAmount_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_JumpBuildAmount_431000436(value);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00045258 File Offset: 0x00043458
		private void RpcWriter___Server_SendJump_431000436(float jumpHeldTime)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(jumpHeldTime, 0);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0004535E File Offset: 0x0004355E
		private void RpcLogic___SendJump_431000436(float jumpHeldTime)
		{
			this.ReceiveJump(jumpHeldTime);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00045368 File Offset: 0x00043568
		private void RpcReader___Server_SendJump_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float num = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendJump_431000436(num);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x000453BC File Offset: 0x000435BC
		private void RpcWriter___Observers_ReceiveJump_431000436(float _jumpHeldTime)
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
			writer.WriteSingle(_jumpHeldTime, 0);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00045478 File Offset: 0x00043678
		private void RpcLogic___ReceiveJump_431000436(float _jumpHeldTime)
		{
			if (this.timeSinceLastJump < 0.3f)
			{
				return;
			}
			this.timeSinceLastJump = 0f;
			this.timeGrounded = 0f;
			float arg = Mathf.Clamp01(_jumpHeldTime / 0.5f);
			if (this.OnJump != null)
			{
				this.OnJump.Invoke(arg);
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x000454CC File Offset: 0x000436CC
		private void RpcReader___Observers_ReceiveJump_431000436(PooledReader PooledReader0, Channel channel)
		{
			float num = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveJump_431000436(num);
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0004550C File Offset: 0x0004370C
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x00045514 File Offset: 0x00043714
		public float SyncAccessor_<JumpBuildAmount>k__BackingField
		{
			get
			{
				return this.<JumpBuildAmount>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<JumpBuildAmount>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<JumpBuildAmount>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00045550 File Offset: 0x00043750
		public override bool Skateboard(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<JumpBuildAmount>k__BackingField(this.syncVar___<JumpBuildAmount>k__BackingField.GetValue(true), true);
				return true;
			}
			float value = PooledReader0.ReadSingle(0);
			this.sync___set_value_<JumpBuildAmount>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000455A8 File Offset: 0x000437A8
		private void dll()
		{
			this.Rb.centerOfMass = this.Rb.transform.InverseTransformPoint(this.CoM.position);
			this.Rb.useGravity = false;
			this.Rb.automaticInertiaTensor = false;
			for (int i = 0; i < this.HoverPoints.Length; i++)
			{
				PID item = new PID(this.Hover_P, this.Hover_I, this.Hover_D);
				this.hoverPIDs.Add(item);
			}
			this.Rider = Player.Local;
			this.ApplyPlayerScale();
		}

		// Token: 0x04000FC0 RID: 4032
		public const float JumpCooldown = 0.3f;

		// Token: 0x04000FC1 RID: 4033
		public const float JumpForceMin = 0.5f;

		// Token: 0x04000FC2 RID: 4034
		public const float JumpForceBuildTime = 0.5f;

		// Token: 0x04000FC3 RID: 4035
		public const float PushCooldown = 1f;

		// Token: 0x04000FC4 RID: 4036
		public const float PushStaminaConsumption = 12.5f;

		// Token: 0x04000FC5 RID: 4037
		public const float PitchLimit = 60f;

		// Token: 0x04000FC6 RID: 4038
		public const float RollLimit = 20f;

		// Token: 0x04000FC7 RID: 4039
		[Header("Info - Readonly")]
		public float CurrentSpeed_Kmh;

		// Token: 0x04000FCB RID: 4043
		[Header("References")]
		public Rigidbody Rb;

		// Token: 0x04000FCC RID: 4044
		public Transform CoM;

		// Token: 0x04000FCD RID: 4045
		public Transform[] HoverPoints;

		// Token: 0x04000FCE RID: 4046
		public Transform FrontAxlePosition;

		// Token: 0x04000FCF RID: 4047
		public Transform RearAxlePosition;

		// Token: 0x04000FD0 RID: 4048
		public Transform PlayerContainer;

		// Token: 0x04000FD1 RID: 4049
		public SkateboardAnimation Animation;

		// Token: 0x04000FD2 RID: 4050
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x04000FD3 RID: 4051
		public AverageAcceleration Accelerometer;

		// Token: 0x04000FD4 RID: 4052
		public Skateboard_Equippable Equippable;

		// Token: 0x04000FD5 RID: 4053
		public Transform IKAlignmentsContainer;

		// Token: 0x04000FD6 RID: 4054
		[Header("Turn Settings")]
		public float TurnForce = 1f;

		// Token: 0x04000FD7 RID: 4055
		public float TurnChangeRate = 2f;

		// Token: 0x04000FD8 RID: 4056
		public float TurnReturnToRestRate = 1f;

		// Token: 0x04000FD9 RID: 4057
		public float TurnSpeedBoost = 1f;

		// Token: 0x04000FDA RID: 4058
		public AnimationCurve TurnForceMap;

		// Token: 0x04000FDB RID: 4059
		[Header("Settings")]
		public float Gravity = 10f;

		// Token: 0x04000FDC RID: 4060
		public float BrakeForce = 1f;

		// Token: 0x04000FDD RID: 4061
		public float ReverseTopSpeed_Kmh = 5f;

		// Token: 0x04000FDE RID: 4062
		public LayerMask GroundDetectionMask;

		// Token: 0x04000FDF RID: 4063
		public Collider[] MainColliders;

		// Token: 0x04000FE0 RID: 4064
		public float RotationClampForce = 1f;

		// Token: 0x04000FE1 RID: 4065
		public bool SlowOnTerrain = true;

		// Token: 0x04000FE2 RID: 4066
		[Header("Friction Settings")]
		public bool FrictionEnabled = true;

		// Token: 0x04000FE3 RID: 4067
		public AnimationCurve LongitudinalFrictionCurve;

		// Token: 0x04000FE4 RID: 4068
		public float LongitudinalFrictionMultiplier = 1f;

		// Token: 0x04000FE5 RID: 4069
		public float LateralFrictionForceMultiplier = 1f;

		// Token: 0x04000FE6 RID: 4070
		[Header("Jump Settings")]
		public float JumpForce = 1f;

		// Token: 0x04000FE7 RID: 4071
		public float JumpDuration_Min = 0.2f;

		// Token: 0x04000FE8 RID: 4072
		public float JumpDuration_Max = 0.5f;

		// Token: 0x04000FE9 RID: 4073
		public AnimationCurve FrontAxleJumpCurve;

		// Token: 0x04000FEA RID: 4074
		public AnimationCurve RearAxleJumpCurve;

		// Token: 0x04000FEB RID: 4075
		public AnimationCurve JumpForwardForceCurve;

		// Token: 0x04000FEC RID: 4076
		public float JumpForwardBoost = 1f;

		// Token: 0x04000FED RID: 4077
		[Header("Hover Settings")]
		public float HoverForce = 1f;

		// Token: 0x04000FEE RID: 4078
		public float HoverRayLength = 0.1f;

		// Token: 0x04000FEF RID: 4079
		public float HoverHeight = 0.05f;

		// Token: 0x04000FF0 RID: 4080
		public float Hover_P = 1f;

		// Token: 0x04000FF1 RID: 4081
		public float Hover_I = 1f;

		// Token: 0x04000FF2 RID: 4082
		public float Hover_D = 1f;

		// Token: 0x04000FF3 RID: 4083
		[Header("Pushing Setings")]
		[Tooltip("Top speed in m/s")]
		public float TopSpeed_Kmh = 10f;

		// Token: 0x04000FF4 RID: 4084
		public float PushForceMultiplier = 1f;

		// Token: 0x04000FF5 RID: 4085
		public AnimationCurve PushForceMultiplierMap;

		// Token: 0x04000FF6 RID: 4086
		public float PushForceDuration = 0.4f;

		// Token: 0x04000FF7 RID: 4087
		public float PushDelay = 0.35f;

		// Token: 0x04000FF8 RID: 4088
		public AnimationCurve PushForceCurve;

		// Token: 0x04000FF9 RID: 4089
		[Header("Air Movement")]
		public bool AirMovementEnabled = true;

		// Token: 0x04000FFA RID: 4090
		public float AirMovementForce = 1f;

		// Token: 0x04000FFB RID: 4091
		public float AirMovementJumpReductionDuration = 0.25f;

		// Token: 0x04000FFC RID: 4092
		public AnimationCurve AirMovementJumpReductionCurve;

		// Token: 0x04000FFD RID: 4093
		[Header("Events")]
		public UnityEvent OnPushStart;

		// Token: 0x04000FFE RID: 4094
		public UnityEvent<float> OnJump;

		// Token: 0x04000FFF RID: 4095
		public UnityEvent OnLand;

		// Token: 0x04001000 RID: 4096
		private int horizontalInput;

		// Token: 0x04001001 RID: 4097
		private bool jumpReleased;

		// Token: 0x04001002 RID: 4098
		private float timeSinceLastJump;

		// Token: 0x04001003 RID: 4099
		private float timeGrounded;

		// Token: 0x04001004 RID: 4100
		private float timeAirborne = 0.21f;

		// Token: 0x04001005 RID: 4101
		private float jumpHeldTime;

		// Token: 0x04001006 RID: 4102
		private float frontAxleForce;

		// Token: 0x04001007 RID: 4103
		private float rearAxleForce;

		// Token: 0x04001008 RID: 4104
		private float jumpForwardForce;

		// Token: 0x04001009 RID: 4105
		private List<PID> hoverPIDs = new List<PID>();

		// Token: 0x0400100A RID: 4106
		private bool pushQueued;

		// Token: 0x0400100B RID: 4107
		private bool isPushing;

		// Token: 0x0400100C RID: 4108
		private float thisFramePushForce;

		// Token: 0x0400100D RID: 4109
		private float timeSincePushStart = 2f;

		// Token: 0x0400100E RID: 4110
		private bool braking;

		// Token: 0x0400100F RID: 4111
		public SyncVar<float> syncVar___<JumpBuildAmount>k__BackingField;

		// Token: 0x04001010 RID: 4112
		private bool dll_Excuted;

		// Token: 0x04001011 RID: 4113
		private bool dll_Excuted;
	}
}
