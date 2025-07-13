using System;
using UnityEngine;

namespace UnityTemplateProjects
{
	// Token: 0x020000E4 RID: 228
	public class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x060003BC RID: 956 RVA: 0x0001538B File Offset: 0x0001358B
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000153B0 File Offset: 0x000135B0
		private Vector3 GetInputTranslationDirection()
		{
			Vector3 vector = default(Vector3);
			if (Input.GetKey(KeyCode.W))
			{
				vector += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				vector += Vector3.back;
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D))
			{
				vector += Vector3.right;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				vector += Vector3.down;
			}
			if (Input.GetKey(KeyCode.E))
			{
				vector += Vector3.up;
			}
			return vector;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00015444 File Offset: 0x00013644
		private void Update()
		{
			Vector3 vector = Vector3.zero;
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
			if (Input.GetMouseButtonDown(1))
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (Input.GetMouseButtonUp(1))
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			if (Input.GetMouseButton(1))
			{
				Vector2 vector2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (float)(this.invertY ? 1 : -1));
				float num = this.mouseSensitivityCurve.Evaluate(vector2.magnitude);
				this.m_TargetCameraState.yaw += vector2.x * num;
				this.m_TargetCameraState.pitch += vector2.y * num;
			}
			vector = this.GetInputTranslationDirection() * Time.deltaTime;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 10f;
			}
			this.boost += Input.mouseScrollDelta.y * 0.2f;
			vector *= Mathf.Pow(2f, this.boost);
			this.m_TargetCameraState.Translate(vector);
			float positionLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime);
			float rotationLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime);
			this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, positionLerpPct, rotationLerpPct);
			this.m_InterpolatingCameraState.UpdateTransform(base.transform);
		}

		// Token: 0x04000497 RID: 1175
		private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000498 RID: 1176
		private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000499 RID: 1177
		[Header("Movement Settings")]
		[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
		public float boost = 3.5f;

		// Token: 0x0400049A RID: 1178
		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float positionLerpTime = 0.2f;

		// Token: 0x0400049B RID: 1179
		[Header("Rotation Settings")]
		[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x0400049C RID: 1180
		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float rotationLerpTime = 0.01f;

		// Token: 0x0400049D RID: 1181
		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool invertY;

		// Token: 0x020000E5 RID: 229
		private class CameraState
		{
			// Token: 0x060003C0 RID: 960 RVA: 0x00015674 File Offset: 0x00013874
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x060003C1 RID: 961 RVA: 0x000156E8 File Offset: 0x000138E8
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x060003C2 RID: 962 RVA: 0x0001574C File Offset: 0x0001394C
			public void LerpTowards(SimpleCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x060003C3 RID: 963 RVA: 0x000157E9 File Offset: 0x000139E9
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x0400049E RID: 1182
			public float yaw;

			// Token: 0x0400049F RID: 1183
			public float pitch;

			// Token: 0x040004A0 RID: 1184
			public float roll;

			// Token: 0x040004A1 RID: 1185
			public float x;

			// Token: 0x040004A2 RID: 1186
			public float y;

			// Token: 0x040004A3 RID: 1187
			public float z;
		}
	}
}
