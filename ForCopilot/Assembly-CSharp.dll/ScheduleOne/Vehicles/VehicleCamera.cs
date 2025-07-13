using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200080F RID: 2063
	public class VehicleCamera : MonoBehaviour
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x00046252 File Offset: 0x00044452
		private Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000EC908 File Offset: 0x000EAB08
		protected virtual void Start()
		{
			this.targetTransform = new GameObject("VehicleCameraTargetTransform").transform;
			this.targetTransform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			this.cameraDolly = new GameObject("VehicleCameraDolly").transform;
			this.cameraDolly.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			if (Player.Local != null)
			{
				this.Subscribe();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Subscribe));
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x000EC99D File Offset: 0x000EAB9D
		private void Subscribe()
		{
			Player local = Player.Local;
			local.onEnterVehicle = (Player.VehicleEvent)Delegate.Combine(local.onEnterVehicle, new Player.VehicleEvent(this.PlayerEnteredVehicle));
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x000EC9C5 File Offset: 0x000EABC5
		protected virtual void Update()
		{
			this.timeSinceCameraManuallyAdjusted += Time.deltaTime;
			this.CheckForClick();
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x000EC9E0 File Offset: 0x000EABE0
		private void PlayerEnteredVehicle(LandVehicle veh)
		{
			if (veh != this.vehicle)
			{
				return;
			}
			this.timeSinceCameraManuallyAdjusted = 100f;
			this.LateUpdate();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.targetTransform.position, this.targetTransform.rotation, 0f, false);
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x000ECA34 File Offset: 0x000EAC34
		private void CheckForClick()
		{
			if (this.vehicle.localPlayerIsInVehicle && GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick) && this.timeSinceCameraManuallyAdjusted > 0.01f)
				{
					Vector3 eulerAngles = this.cam.rotation.eulerAngles;
					this.x = eulerAngles.y;
					this.y = eulerAngles.x;
					this.orbitDistance = Mathf.Sqrt(Mathf.Pow(this.lateralOffset, 2f) + Mathf.Pow(this.verticalOffset, 2f));
				}
				this.timeSinceCameraManuallyAdjusted = 0f;
			}
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000ECAD4 File Offset: 0x000EACD4
		protected virtual void LateUpdate()
		{
			if (this.vehicle.localPlayerIsInVehicle)
			{
				if (this.vehicle.speed_Kmh > 2f)
				{
					this.cameraReversed = false;
				}
				else if (this.vehicle.speed_Kmh < -15f)
				{
					this.cameraReversed = true;
				}
				this.targetTransform.position = this.LimitCameraPosition(this.GetTargetCameraPosition());
				this.targetTransform.LookAt(this.cameraOrigin);
				this.cameraDolly.position = Vector3.Lerp(this.cameraDolly.position, this.targetTransform.position, Time.deltaTime * 10f);
				this.cameraDolly.rotation = Quaternion.Lerp(this.cameraDolly.rotation, this.targetTransform.rotation, Time.deltaTime * 10f);
				this.orbitDistance = Mathf.Clamp(Vector3.Distance(this.cameraOrigin.position, this.cameraDolly.position), Mathf.Sqrt(Mathf.Pow(this.lateralOffset, 2f) + Mathf.Pow(this.verticalOffset, 2f)), 100f);
				if (this.timeSinceCameraManuallyAdjusted <= 0.01f)
				{
					if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
					{
						this.x += GameInput.MouseDelta.x * 60f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
						this.y -= GameInput.MouseDelta.y * 40f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
						this.y = VehicleCamera.ClampAngle(this.y, -20f, 89f);
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
					this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 10f);
					this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 10f);
				}
				else
				{
					this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 10f);
					this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 10f);
				}
				this.lastFrameCameraOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
			}
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x00008A3D File Offset: 0x00006C3D
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

		// Token: 0x0600382C RID: 14380 RVA: 0x000ECEF8 File Offset: 0x000EB0F8
		private Vector3 GetTargetCameraPosition()
		{
			Vector3 a = -base.transform.forward;
			a.y = 0f;
			a.Normalize();
			if (this.cameraReversed)
			{
				a *= -1f;
			}
			return base.transform.position + a * this.lateralOffset + Vector3.up * this.verticalOffset;
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000ECF70 File Offset: 0x000EB170
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

		// Token: 0x040027EC RID: 10220
		private const float followDelta = 10f;

		// Token: 0x040027ED RID: 10221
		private const float yMinLimit = -20f;

		// Token: 0x040027EE RID: 10222
		private const float manualOverrideTime = 0.01f;

		// Token: 0x040027EF RID: 10223
		private const float manualOverrideReturnTime = 0.6f;

		// Token: 0x040027F0 RID: 10224
		private const float xSpeed = 60f;

		// Token: 0x040027F1 RID: 10225
		private const float ySpeed = 40f;

		// Token: 0x040027F2 RID: 10226
		private const float yMaxLimit = 89f;

		// Token: 0x040027F3 RID: 10227
		[Header("References")]
		public LandVehicle vehicle;

		// Token: 0x040027F4 RID: 10228
		[Header("Camera Settings")]
		[SerializeField]
		protected Transform cameraOrigin;

		// Token: 0x040027F5 RID: 10229
		[SerializeField]
		protected float lateralOffset = 4f;

		// Token: 0x040027F6 RID: 10230
		[SerializeField]
		protected float verticalOffset = 1.5f;

		// Token: 0x040027F7 RID: 10231
		protected bool cameraReversed;

		// Token: 0x040027F8 RID: 10232
		protected float timeSinceCameraManuallyAdjusted = float.MaxValue;

		// Token: 0x040027F9 RID: 10233
		protected float orbitDistance;

		// Token: 0x040027FA RID: 10234
		protected Vector3 lastFrameCameraOffset = Vector3.zero;

		// Token: 0x040027FB RID: 10235
		protected Vector3 lastManualOffset = Vector3.zero;

		// Token: 0x040027FC RID: 10236
		private Transform targetTransform;

		// Token: 0x040027FD RID: 10237
		private Transform cameraDolly;

		// Token: 0x040027FE RID: 10238
		private float x;

		// Token: 0x040027FF RID: 10239
		private float y;
	}
}
