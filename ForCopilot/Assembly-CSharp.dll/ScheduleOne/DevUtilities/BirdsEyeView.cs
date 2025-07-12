using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000714 RID: 1812
	public class BirdsEyeView : Singleton<BirdsEyeView>
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x00046252 File Offset: 0x00044452
		private Transform playerCam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x000CCF93 File Offset: 0x000CB193
		// (set) Token: 0x06003115 RID: 12565 RVA: 0x000CCF9B File Offset: 0x000CB19B
		public bool isEnabled { get; protected set; }

		// Token: 0x06003116 RID: 12566 RVA: 0x000CCFA4 File Offset: 0x000CB1A4
		protected override void Awake()
		{
			base.Awake();
			this.targetTransform = new GameObject("_TargetCameraTransform").transform;
			this.targetTransform.SetParent(GameObject.Find("_Temp").transform);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000CCFDB File Offset: 0x000CB1DB
		protected virtual void Update()
		{
			if (this.isEnabled)
			{
				this.UpdateLateralMovement();
				this.UpdateRotation();
				this.UpdateScrollMovement();
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000CCFF7 File Offset: 0x000CB1F7
		protected virtual void LateUpdate()
		{
			if (this.isEnabled)
			{
				this.FinalizeCameraMovement();
			}
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000CD008 File Offset: 0x000CB208
		public void Enable(Vector3 startPosition, Quaternion startRotation)
		{
			this.isEnabled = true;
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(startPosition, startRotation, 0f, false);
			Vector3 eulerAngles = startRotation.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this.targetTransform.position = startPosition;
			this.targetTransform.rotation = startRotation;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000CD07A File Offset: 0x000CB27A
		public void Disable(bool reenableCameraLook = true)
		{
			this.isEnabled = false;
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, reenableCameraLook, true);
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000CD0A4 File Offset: 0x000CB2A4
		protected void UpdateLateralMovement()
		{
			float num = GameInput.MotionAxis.y;
			float d = GameInput.MotionAxis.x;
			int num2 = 0;
			if (Input.GetKey(KeyCode.Space))
			{
				num2++;
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				num2--;
			}
			if (num != 0f || num2 != 0)
			{
				this.CancelOriginSlide();
			}
			Vector3 forward = this.playerCam.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 right = this.playerCam.right;
			right.y = 0f;
			right.Normalize();
			Vector3 b = forward * num * this.lateralMovementSpeed * Time.deltaTime;
			Vector3 b2 = right * d * this.lateralMovementSpeed * Time.deltaTime;
			Vector3 b3 = Vector3.up * (float)num2 * this.lateralMovementSpeed * Time.deltaTime * 0.5f;
			this.targetTransform.position += b;
			this.targetTransform.position += b2;
			this.targetTransform.position += b3;
			this.rotationOriginPoint += b;
			this.rotationOriginPoint += b2;
			this.rotationOriginPoint += b3;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000CD224 File Offset: 0x000CB424
		protected void UpdateScrollMovement()
		{
			float num = Input.mouseScrollDelta.y;
			Vector3 normalized = this.playerCam.forward.normalized;
			if (GameInput.GetButton(GameInput.ButtonCode.TertiaryClick) || GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				this.distance += num * this.scrollMovementSpeed * Time.deltaTime;
				return;
			}
			this.targetTransform.position += normalized * num * this.scrollMovementSpeed * Time.deltaTime;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000CD2B0 File Offset: 0x000CB4B0
		protected void UpdateRotation()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick) || GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				Plane plane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));
				Ray ray = new Ray(this.targetTransform.position, this.targetTransform.forward);
				float num = 0f;
				plane.Raycast(ray, out num);
				this.distance = num;
				this.rotationOriginPoint = ray.GetPoint(num);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.TertiaryClick) || GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				this.x += GameInput.MouseDelta.x * this.xSpeed * 0.02f;
				this.y -= GameInput.MouseDelta.y * this.ySpeed * 0.02f;
				this.y = BirdsEyeView.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.rotationOriginPoint;
				this.targetTransform.rotation = rotation;
				this.targetTransform.position = position;
			}
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000CD404 File Offset: 0x000CB604
		private void FinalizeCameraMovement()
		{
			this.playerCam.position = Vector3.Lerp(this.playerCam.position, this.targetTransform.position, Time.deltaTime * this.targetFollowSpeed);
			this.playerCam.rotation = Quaternion.Lerp(this.playerCam.rotation, this.targetTransform.rotation, Time.deltaTime * this.targetFollowSpeed);
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x00008A3D File Offset: 0x00006C3D
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

		// Token: 0x06003120 RID: 12576 RVA: 0x000CD475 File Offset: 0x000CB675
		private void CancelOriginSlide()
		{
			if (this.originSlideRoutine != null)
			{
				base.StopCoroutine(this.originSlideRoutine);
				this.originSlideRoutine = null;
			}
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000CD494 File Offset: 0x000CB694
		public void SlideCameraOrigin(Vector3 position, float offsetDistance, float time = 0f)
		{
			BirdsEyeView.<>c__DisplayClass33_0 CS$<>8__locals1 = new BirdsEyeView.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.position = position;
			CS$<>8__locals1.time = time;
			if (this.originSlideRoutine != null)
			{
				base.StopCoroutine(this.originSlideRoutine);
			}
			Plane plane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));
			Ray ray = new Ray(this.targetTransform.position, this.targetTransform.forward);
			float num = 0f;
			plane.Raycast(ray, out num);
			Vector3 point = ray.GetPoint(num);
			Vector3 vector = this.targetTransform.position - point;
			CS$<>8__locals1.position += vector.normalized * offsetDistance;
			this.originSlideRoutine = base.StartCoroutine(CS$<>8__locals1.<SlideCameraOrigin>g__Routine|0());
		}

		// Token: 0x0400227A RID: 8826
		[Header("Settings")]
		public Vector3 bounds_Min;

		// Token: 0x0400227B RID: 8827
		public Vector3 bounds_Max;

		// Token: 0x0400227C RID: 8828
		[Header("Camera settings")]
		public float lateralMovementSpeed = 1f;

		// Token: 0x0400227D RID: 8829
		public float scrollMovementSpeed = 1f;

		// Token: 0x0400227E RID: 8830
		public float targetFollowSpeed = 1f;

		// Token: 0x0400227F RID: 8831
		[Header("Camera orbit settings")]
		public float xSpeed = 250f;

		// Token: 0x04002280 RID: 8832
		public float ySpeed = 120f;

		// Token: 0x04002281 RID: 8833
		public float yMinLimit = -20f;

		// Token: 0x04002282 RID: 8834
		public float yMaxLimit = 80f;

		// Token: 0x04002283 RID: 8835
		private Vector3 rotationOriginPoint = Vector3.zero;

		// Token: 0x04002284 RID: 8836
		private float distance = 10f;

		// Token: 0x04002285 RID: 8837
		private float prevDistance;

		// Token: 0x04002286 RID: 8838
		private float x;

		// Token: 0x04002287 RID: 8839
		private float y;

		// Token: 0x04002288 RID: 8840
		private Transform targetTransform;

		// Token: 0x0400228A RID: 8842
		private Coroutine originSlideRoutine;
	}
}
