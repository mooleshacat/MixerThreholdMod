using System;
using UnityEngine;

namespace VLB_Samples
{
	// Token: 0x02000167 RID: 359
	public class FreeCameraController : MonoBehaviour
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001EF08 File Offset: 0x0001D108
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001EF10 File Offset: 0x0001D110
		private bool useMouseView
		{
			get
			{
				return this.m_UseMouseView;
			}
			set
			{
				this.m_UseMouseView = value;
				Cursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None);
				Cursor.visible = !value;
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001EF30 File Offset: 0x0001D130
		private void Start()
		{
			this.useMouseView = true;
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			this.rotationH = eulerAngles.y;
			this.rotationV = eulerAngles.x;
			if (this.rotationV > 180f)
			{
				this.rotationV -= 360f;
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001EF90 File Offset: 0x0001D190
		private void Update()
		{
			if (this.useMouseView)
			{
				this.rotationH += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
				this.rotationV -= Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
			}
			this.rotationV = Mathf.Clamp(this.rotationV, -90f, 90f);
			base.transform.rotation = Quaternion.AngleAxis(this.rotationH, Vector3.up);
			base.transform.rotation *= Quaternion.AngleAxis(this.rotationV, Vector3.right);
			float num = this.speedNormal;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				num *= this.speedFactorFast;
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				num *= this.speedFactorSlow;
			}
			base.transform.position += num * Input.GetAxis("Vertical") * Time.deltaTime * base.transform.forward;
			base.transform.position += num * Input.GetAxis("Horizontal") * Time.deltaTime * base.transform.right;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += this.speedClimb * Time.deltaTime * Vector3.up;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position += this.speedClimb * Time.deltaTime * Vector3.down;
			}
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				this.useMouseView = !this.useMouseView;
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.useMouseView = false;
			}
		}

		// Token: 0x0400079D RID: 1949
		public float cameraSensitivity = 90f;

		// Token: 0x0400079E RID: 1950
		public float speedNormal = 10f;

		// Token: 0x0400079F RID: 1951
		public float speedFactorSlow = 0.25f;

		// Token: 0x040007A0 RID: 1952
		public float speedFactorFast = 3f;

		// Token: 0x040007A1 RID: 1953
		public float speedClimb = 4f;

		// Token: 0x040007A2 RID: 1954
		private float rotationH;

		// Token: 0x040007A3 RID: 1955
		private float rotationV;

		// Token: 0x040007A4 RID: 1956
		private bool m_UseMouseView = true;
	}
}
