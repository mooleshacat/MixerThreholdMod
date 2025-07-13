using System;
using System.Collections.Generic;
using ScheduleOne.AvatarFramework.Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000717 RID: 1815
	public class CameraOrbit : MonoBehaviour
	{
		// Token: 0x0600312B RID: 12587 RVA: 0x000CD6E8 File Offset: 0x000CB8E8
		private void Start()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this.rb = base.GetComponent<Rigidbody>();
			if (this.rb != null)
			{
				this.rb.freezeRotation = true;
			}
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000CD740 File Offset: 0x000CB940
		private void Update()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			this.raycaster.Raycast(pointerEventData, list);
			this.hoveringUI = (list.Count > 0);
			this.LookAt.OverrideLookTarget(this.cam.transform.position, 100, false);
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000CD7A8 File Offset: 0x000CB9A8
		private void LateUpdate()
		{
			if (this.target)
			{
				if (Input.GetMouseButton(0) && !this.hoveringUI)
				{
					this.targetx += Input.GetAxis("Mouse X") * this.xSpeed * this.distance * 0.02f * (5f / (this.distance + 2f));
					this.targety -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
				}
				this.targety = CameraOrbit.ClampAngle(this.targety, this.yMinLimit, this.yMaxLimit);
				this.x = Mathf.LerpAngle(this.x, this.targetx, 0.1f);
				this.y = Mathf.LerpAngle(this.y, this.targety, 1f);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				if (!this.hoveringUI)
				{
					this.targetdistance = Mathf.Clamp(this.targetdistance - Input.GetAxis("Mouse ScrollWheel") * this.ScrollSensativity, this.distanceMin, this.distanceMax);
				}
				this.distance = Mathf.Lerp(this.distance, this.targetdistance, 0.1f);
				RaycastHit raycastHit;
				if (Physics.Linecast(this.target.position, base.transform.position, ref raycastHit))
				{
					this.targetdistance -= raycastHit.distance;
				}
				Vector3 point = new Vector3(0f, 0f, -this.distance);
				Vector3 position = rotation * point + this.target.position;
				base.transform.rotation = rotation;
				base.transform.position = position;
			}
			this.cam.position = base.transform.position;
			this.cam.rotation = base.transform.rotation;
			this.cam.position = this.cam.position - base.transform.right * this.sideOffset * Vector3.Distance(this.cam.position, this.target.position);
			if (Input.GetKey(KeyCode.KeypadPlus))
			{
				base.GetComponent<Camera>().fieldOfView += 0.3f;
			}
			if (Input.GetKey(KeyCode.KeypadMinus))
			{
				base.GetComponent<Camera>().fieldOfView -= 0.3f;
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x00008A3D File Offset: 0x00006C3D
		public static float ClampAngle(float angle, float min, float max)
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

		// Token: 0x04002293 RID: 8851
		[Header("Required")]
		public Transform target;

		// Token: 0x04002294 RID: 8852
		public Transform cam;

		// Token: 0x04002295 RID: 8853
		public GraphicRaycaster raycaster;

		// Token: 0x04002296 RID: 8854
		public AvatarLookController LookAt;

		// Token: 0x04002297 RID: 8855
		[Header("Config")]
		public float targetdistance = 5f;

		// Token: 0x04002298 RID: 8856
		public float xSpeed = 120f;

		// Token: 0x04002299 RID: 8857
		public float ySpeed = 120f;

		// Token: 0x0400229A RID: 8858
		public float sideOffset = 1f;

		// Token: 0x0400229B RID: 8859
		public float yMinLimit = -20f;

		// Token: 0x0400229C RID: 8860
		public float yMaxLimit = 80f;

		// Token: 0x0400229D RID: 8861
		public float distanceMin = 0.5f;

		// Token: 0x0400229E RID: 8862
		public float distanceMax = 15f;

		// Token: 0x0400229F RID: 8863
		public float ScrollSensativity = 4f;

		// Token: 0x040022A0 RID: 8864
		private Rigidbody rb;

		// Token: 0x040022A1 RID: 8865
		private float x;

		// Token: 0x040022A2 RID: 8866
		private float y;

		// Token: 0x040022A3 RID: 8867
		private float targetx;

		// Token: 0x040022A4 RID: 8868
		private float targety;

		// Token: 0x040022A5 RID: 8869
		private float distance = 5f;

		// Token: 0x040022A6 RID: 8870
		private bool hoveringUI;
	}
}
