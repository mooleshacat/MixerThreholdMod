using System;
using ScheduleOne;
using UnityEngine;

namespace StylizedGrassDemo
{
	// Token: 0x0200016E RID: 366
	public class OrbitCamera : MonoBehaviour
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x0001FC04 File Offset: 0x0001DE04
		private void Start()
		{
			this.cam = Camera.main.transform;
			this.cameraRotSide = base.transform.eulerAngles.y;
			this.cameraRotSideCur = base.transform.eulerAngles.y;
			this.cameraRotUp = base.transform.eulerAngles.x;
			this.cameraRotUpCur = base.transform.eulerAngles.x;
			this.distance = -this.cam.localPosition.z;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001FC90 File Offset: 0x0001DE90
		private void LateUpdate()
		{
			Cursor.visible = false;
			if (!this.pivot)
			{
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.enableMouse)
			{
				this.cameraRotSide += GameInput.MouseDelta.x * 5f;
				this.cameraRotUp -= GameInput.MouseDelta.y * 5f;
			}
			else
			{
				this.cameraRotSide += this.idleRotationSpeed;
			}
			this.cameraRotSideCur = Mathf.LerpAngle(this.cameraRotSideCur, this.cameraRotSide, Time.deltaTime * this.lookSmoothSpeed);
			this.cameraRotUpCur = Mathf.Lerp(this.cameraRotUpCur, this.cameraRotUp, Time.deltaTime * this.lookSmoothSpeed);
			if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) && this.enableMouse)
			{
				this.distance *= 1f - 0.1f * GameInput.MouseDelta.y;
			}
			if (this.enableMouse)
			{
				this.distance *= 1f - 1f * GameInput.MouseScrollDelta;
			}
			Vector3 position = this.pivot.position;
			base.transform.position = Vector3.Lerp(base.transform.position, position, Time.deltaTime * this.moveSmoothSpeed);
			base.transform.rotation = Quaternion.Euler(this.cameraRotUpCur, this.cameraRotSideCur, 0f);
			float d = Mathf.Lerp(-this.cam.transform.localPosition.z, this.distance, Time.deltaTime * this.scrollSmoothSpeed);
			this.cam.localPosition = -Vector3.forward * d;
		}

		// Token: 0x040007CE RID: 1998
		[Space]
		public Transform pivot;

		// Token: 0x040007CF RID: 1999
		[Space]
		public bool enableMouse = true;

		// Token: 0x040007D0 RID: 2000
		public float idleRotationSpeed = 0.05f;

		// Token: 0x040007D1 RID: 2001
		public float lookSmoothSpeed = 5f;

		// Token: 0x040007D2 RID: 2002
		public float moveSmoothSpeed = 5f;

		// Token: 0x040007D3 RID: 2003
		public float scrollSmoothSpeed = 5f;

		// Token: 0x040007D4 RID: 2004
		private Transform cam;

		// Token: 0x040007D5 RID: 2005
		private float cameraRotSide;

		// Token: 0x040007D6 RID: 2006
		private float cameraRotUp;

		// Token: 0x040007D7 RID: 2007
		private float cameraRotSideCur;

		// Token: 0x040007D8 RID: 2008
		private float cameraRotUpCur;

		// Token: 0x040007D9 RID: 2009
		private float distance;
	}
}
