using System;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Skating
{
	// Token: 0x020002DE RID: 734
	[RequireComponent(typeof(Skateboard))]
	public class SkateboardCamera : NetworkBehaviour
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x00046252 File Offset: 0x00044452
		private Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00046260 File Offset: 0x00044460
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Skating.SkateboardCamera_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0004627F File Offset: 0x0004447F
		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!this.board.IsOwner)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0004629B File Offset: 0x0004449B
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this.targetTransform.gameObject);
			UnityEngine.Object.Destroy(this.cameraDolly.gameObject);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000462BD File Offset: 0x000444BD
		private void Update()
		{
			if (!base.IsSpawned)
			{
				return;
			}
			this.timeSinceCameraManuallyAdjusted += Time.deltaTime;
			this.CheckForClick();
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x000462E0 File Offset: 0x000444E0
		private void CheckForClick()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick) && this.timeSinceCameraManuallyAdjusted > 0.01f)
				{
					this.cameraAdjusted = true;
					Vector3 eulerAngles = this.cam.rotation.eulerAngles;
					this.x = eulerAngles.y;
					this.y = eulerAngles.x;
					this.orbitDistance = Mathf.Sqrt(Mathf.Pow(this.HorizontalOffset, 2f) + Mathf.Pow(this.VerticalOffset, 2f));
				}
				if (this.cameraAdjusted)
				{
					this.timeSinceCameraManuallyAdjusted = 0f;
					return;
				}
			}
			else
			{
				this.cameraAdjusted = false;
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00046387 File Offset: 0x00044587
		private void LateUpdate()
		{
			if (!base.IsSpawned)
			{
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (!this.board.Owner.IsLocalClient)
			{
				return;
			}
			this.UpdateCamera();
			this.UpdateFOV();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x000463BC File Offset: 0x000445BC
		private void UpdateCamera()
		{
			this.targetTransform.position = this.LimitCameraPosition(this.GetTargetCameraPosition());
			this.targetTransform.LookAt(this.cameraOrigin);
			this.cameraDolly.position = Vector3.Lerp(this.cameraDolly.position, this.targetTransform.position, Time.deltaTime * 7.5f);
			this.cameraDolly.rotation = Quaternion.Lerp(this.cameraDolly.rotation, this.targetTransform.rotation, Time.deltaTime * 7.5f);
			this.orbitDistance = Mathf.Clamp(Vector3.Distance(this.cameraOrigin.position, this.cameraDolly.position), Mathf.Sqrt(Mathf.Pow(this.HorizontalOffset, 2f) + Mathf.Pow(this.VerticalOffset, 2f)), 100f);
			if (this.timeSinceCameraManuallyAdjusted <= 0.01f)
			{
				if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
				{
					this.x += GameInput.MouseDelta.x * 60f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
					this.y -= GameInput.MouseDelta.y * 40f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
					this.y = SkateboardCamera.ClampAngle(this.y, -20f, 89f);
					Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
					Vector3 targetPosition = rotation * new Vector3(0f, 0f, -this.orbitDistance) + this.cameraOrigin.position;
					this.cam.rotation = rotation;
					this.cam.position = this.LimitCameraPosition(targetPosition);
				}
				else
				{
					Vector3 normalized = (this.cameraOrigin.TransformPoint(this.lastFrameCameraOffset) - this.cameraOrigin.position).normalized;
					Vector3 targetPosition2 = this.cameraOrigin.position + normalized * this.orbitDistance;
					this.cam.position = this.LimitCameraPosition(targetPosition2);
					this.cam.LookAt(this.cameraOrigin);
					Vector3 eulerAngles = this.cam.rotation.eulerAngles;
					this.x = eulerAngles.y;
					this.y = eulerAngles.x;
				}
				this.lastManualOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
			}
			else if (this.timeSinceCameraManuallyAdjusted < 0.61f)
			{
				this.targetTransform.position = Vector3.Lerp(this.cameraOrigin.TransformPoint(this.lastManualOffset), this.targetTransform.position, (this.timeSinceCameraManuallyAdjusted - 0.01f) / 0.6f);
				this.targetTransform.LookAt(this.cameraOrigin);
				this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 7.5f);
				this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 7.5f);
			}
			else
			{
				this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 7.5f);
				this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 7.5f);
			}
			this.lastFrameCameraOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0004679C File Offset: 0x0004499C
		private void UpdateFOV()
		{
			float b = Mathf.Lerp(this.FOVMultiplier_MinSpeed, this.FOVMultiplier_MaxSpeed, Mathf.Clamp01(this.board.Rb.velocity.magnitude / this.board.TopSpeed_Ms));
			this.currentFovMultiplier = Mathf.Lerp(this.currentFovMultiplier, b, Time.deltaTime * this.FOVMultiplierChangeRate);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.currentFovMultiplier * Singleton<Settings>.Instance.CameraFOV, 0f);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00008A3D File Offset: 0x00006C3D
		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00046824 File Offset: 0x00044A24
		private Vector3 GetTargetCameraPosition()
		{
			Vector3 a = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
			Vector3 up = Vector3.up;
			return base.transform.position + a * this.HorizontalOffset + up * this.VerticalOffset;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0004687C File Offset: 0x00044A7C
		private Vector3 LimitCameraPosition(Vector3 targetPosition)
		{
			Vector3 vector = targetPosition;
			default(LayerMask) | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			float num = 0.45f;
			Vector3 vector2 = Vector3.Normalize(vector - this.cameraOrigin.position);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.cameraOrigin.position, vector2, ref raycastHit, Vector3.Distance(base.transform.position, vector) + num, 1 << LayerMask.NameToLayer("Default")))
			{
				vector = raycastHit.point - vector2 * num;
			}
			return vector;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x000469C0 File Offset: 0x00044BC0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x000469D3 File Offset: 0x00044BD3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Skating.SkateboardCameraAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000469E6 File Offset: 0x00044BE6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x000469F4 File Offset: 0x00044BF4
		private void dll()
		{
			this.board = base.GetComponent<Skateboard>();
			this.targetTransform = new GameObject("VehicleCameraTargetTransform").transform;
			this.targetTransform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			this.cameraDolly = new GameObject("VehicleCameraDolly").transform;
			this.cameraDolly.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
		}

		// Token: 0x0400104E RID: 4174
		private const float followDelta = 7.5f;

		// Token: 0x0400104F RID: 4175
		private const float yMinLimit = -20f;

		// Token: 0x04001050 RID: 4176
		private const float manualOverrideTime = 0.01f;

		// Token: 0x04001051 RID: 4177
		private const float manualOverrideReturnTime = 0.6f;

		// Token: 0x04001052 RID: 4178
		private const float xSpeed = 60f;

		// Token: 0x04001053 RID: 4179
		private const float ySpeed = 40f;

		// Token: 0x04001054 RID: 4180
		private const float yMaxLimit = 89f;

		// Token: 0x04001055 RID: 4181
		[Header("References")]
		public Transform cameraOrigin;

		// Token: 0x04001056 RID: 4182
		[Header("Settings")]
		public float CameraFollowSpeed = 10f;

		// Token: 0x04001057 RID: 4183
		public float HorizontalOffset = -2.5f;

		// Token: 0x04001058 RID: 4184
		public float VerticalOffset = 2f;

		// Token: 0x04001059 RID: 4185
		public float CameraDownAngle = 18f;

		// Token: 0x0400105A RID: 4186
		[Header("Settings")]
		public float FOVMultiplier_MinSpeed = 1f;

		// Token: 0x0400105B RID: 4187
		public float FOVMultiplier_MaxSpeed = 1.3f;

		// Token: 0x0400105C RID: 4188
		public float FOVMultiplierChangeRate = 3f;

		// Token: 0x0400105D RID: 4189
		private Skateboard board;

		// Token: 0x0400105E RID: 4190
		private float currentFovMultiplier = 1f;

		// Token: 0x0400105F RID: 4191
		private bool cameraReversed;

		// Token: 0x04001060 RID: 4192
		private bool cameraAdjusted;

		// Token: 0x04001061 RID: 4193
		private float timeSinceCameraManuallyAdjusted = float.MaxValue;

		// Token: 0x04001062 RID: 4194
		private float orbitDistance;

		// Token: 0x04001063 RID: 4195
		private Vector3 lastFrameCameraOffset = Vector3.zero;

		// Token: 0x04001064 RID: 4196
		private Vector3 lastManualOffset = Vector3.zero;

		// Token: 0x04001065 RID: 4197
		private Transform targetTransform;

		// Token: 0x04001066 RID: 4198
		private Transform cameraDolly;

		// Token: 0x04001067 RID: 4199
		private float x;

		// Token: 0x04001068 RID: 4200
		private float y;

		// Token: 0x04001069 RID: 4201
		private bool dll_Excuted;

		// Token: 0x0400106A RID: 4202
		private bool dll_Excuted;
	}
}
