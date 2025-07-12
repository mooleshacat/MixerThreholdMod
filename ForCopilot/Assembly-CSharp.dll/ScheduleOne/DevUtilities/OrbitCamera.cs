using System;
using EasyButtons;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000729 RID: 1833
	public class OrbitCamera : MonoBehaviour
	{
		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060031A5 RID: 12709 RVA: 0x000CF25F File Offset: 0x000CD45F
		// (set) Token: 0x060031A6 RID: 12710 RVA: 0x000CF267 File Offset: 0x000CD467
		public bool isEnabled { get; protected set; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060031A7 RID: 12711 RVA: 0x00046252 File Offset: 0x00044452
		protected Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000CF270 File Offset: 0x000CD470
		protected virtual void Awake()
		{
			this.targetTransform = new GameObject("_OrbitCamTarget").transform;
			this.targetTransform.SetParent(GameObject.Find("_Temp").transform);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000CF2A1 File Offset: 0x000CD4A1
		protected virtual void Update()
		{
			if (this.isEnabled)
			{
				this.UpdateRotation();
			}
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000CF2B1 File Offset: 0x000CD4B1
		protected virtual void LateUpdate()
		{
			if (this.isEnabled)
			{
				this.FinalizeCameraMovement();
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000CF2C4 File Offset: 0x000CD4C4
		[Button]
		public void Enable()
		{
			this.isEnabled = true;
			this.cameraStartPoint.LookAt(this.centrePoint);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(80f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.cam.position, this.cam.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.blockNextStopTransformOverride = true;
			Vector3 eulerAngles = this.cameraStartPoint.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this.targetTransform.position = this.cameraStartPoint.position;
			this.targetTransform.rotation = this.cameraStartPoint.rotation;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000CF37E File Offset: 0x000CD57E
		public void Disable()
		{
			this.isEnabled = false;
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.25f, false, true);
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000CF3A8 File Offset: 0x000CD5A8
		protected void UpdateRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick))
			{
				this.distance = Vector3.Distance(this.centrePoint.position, this.targetTransform.position);
				this.rotationOriginPoint = this.centrePoint.position;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.TertiaryClick))
			{
				this.x += GameInput.MouseDelta.x * OrbitCamera.xSpeed * 0.02f;
				this.y -= GameInput.MouseDelta.y * OrbitCamera.ySpeed * 0.02f;
				this.y = OrbitCamera.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.rotationOriginPoint;
				this.targetTransform.rotation = rotation;
				this.targetTransform.position = position;
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x00008A3D File Offset: 0x00006C3D
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

		// Token: 0x060031B0 RID: 12720 RVA: 0x000CF4B4 File Offset: 0x000CD6B4
		private void FinalizeCameraMovement()
		{
			this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * this.targetFollowSpeed);
			this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * this.targetFollowSpeed);
		}

		// Token: 0x040022F0 RID: 8944
		[Header("References")]
		[SerializeField]
		protected Transform cameraStartPoint;

		// Token: 0x040022F1 RID: 8945
		[SerializeField]
		protected Transform centrePoint;

		// Token: 0x040022F2 RID: 8946
		[Header("Settings")]
		public float targetFollowSpeed = 1f;

		// Token: 0x040022F3 RID: 8947
		public float yMinLimit = -20f;

		// Token: 0x040022F4 RID: 8948
		public float yMaxLimit = 80f;

		// Token: 0x040022F5 RID: 8949
		public static float xSpeed = 200f;

		// Token: 0x040022F6 RID: 8950
		public static float ySpeed = 100f;

		// Token: 0x040022F8 RID: 8952
		private Vector3 rotationOriginPoint = Vector3.zero;

		// Token: 0x040022F9 RID: 8953
		private float distance = 10f;

		// Token: 0x040022FA RID: 8954
		private float prevDistance;

		// Token: 0x040022FB RID: 8955
		private float x;

		// Token: 0x040022FC RID: 8956
		private float y;

		// Token: 0x040022FD RID: 8957
		private Transform targetTransform;
	}
}
