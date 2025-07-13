using System;
using UnityEngine;

namespace VolumetricFogAndMist2.Demos
{
	// Token: 0x0200016C RID: 364
	public class FPS_Controller : MonoBehaviour
	{
		// Token: 0x06000700 RID: 1792 RVA: 0x0001F78C File Offset: 0x0001D98C
		private void Start()
		{
			this.characterController = base.gameObject.AddComponent<CharacterController>();
			this.mainCamera = Camera.main.transform;
			this.characterController.height = this.characterHeight;
			this.characterController.center = Vector3.up * this.characterHeight / 2f;
			this.mainCamera.position = base.transform.position + Vector3.up * this.characterHeight;
			this.mainCamera.rotation = Quaternion.identity;
			this.mainCamera.parent = base.transform;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001F848 File Offset: 0x0001DA48
		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			if (mousePosition.x < 0f || mousePosition.x >= (float)Screen.width || mousePosition.y < 0f || mousePosition.y >= (float)Screen.height)
			{
				return;
			}
			this.isGrounded = this.characterController.isGrounded;
			this.inputHor = Input.GetAxis("Horizontal");
			this.inputVert = Input.GetAxis("Vertical");
			this.mouseHor = Input.GetAxis("Mouse X");
			this.mouseVert = Input.GetAxis("Mouse Y");
			base.transform.Rotate(0f, this.mouseHor * this.rotationSpeed * this.mouseSensitivity * this.mouseInvertX, 0f);
			Vector3 a = base.transform.forward * this.inputVert + base.transform.right * this.inputHor;
			a *= this.speed;
			if (this.isGrounded)
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					if (this.sprint < this.sprintMax)
					{
						this.sprint += 10f * Time.deltaTime;
					}
				}
				else if (this.sprint > 1f)
				{
					this.sprint -= 10f * Time.deltaTime;
				}
				if (Input.GetKeyDown(KeyCode.Space))
				{
					this.jumpDirection.y = this.jumpHeight;
				}
				else
				{
					this.jumpDirection.y = -1f;
				}
			}
			else
			{
				a *= this.airControl;
			}
			this.jumpDirection.y = this.jumpDirection.y - this.gravity * Time.deltaTime;
			this.characterController.Move(a * this.sprint * Time.deltaTime);
			this.characterController.Move(this.jumpDirection * Time.deltaTime);
			this.camVertAngle += this.mouseVert * this.rotationSpeed * this.mouseSensitivity * this.mouseInvertY;
			this.camVertAngle = Mathf.Clamp(this.camVertAngle, -85f, 85f);
			this.mainCamera.localEulerAngles = new Vector3(this.camVertAngle, 0f, 0f);
		}

		// Token: 0x040007B6 RID: 1974
		private CharacterController characterController;

		// Token: 0x040007B7 RID: 1975
		private Transform mainCamera;

		// Token: 0x040007B8 RID: 1976
		private float inputHor;

		// Token: 0x040007B9 RID: 1977
		private float inputVert;

		// Token: 0x040007BA RID: 1978
		private float mouseHor;

		// Token: 0x040007BB RID: 1979
		private float mouseVert;

		// Token: 0x040007BC RID: 1980
		private float mouseInvertX = 1f;

		// Token: 0x040007BD RID: 1981
		private float mouseInvertY = -1f;

		// Token: 0x040007BE RID: 1982
		private float camVertAngle;

		// Token: 0x040007BF RID: 1983
		private bool isGrounded;

		// Token: 0x040007C0 RID: 1984
		private Vector3 jumpDirection = Vector3.zero;

		// Token: 0x040007C1 RID: 1985
		private float sprint = 1f;

		// Token: 0x040007C2 RID: 1986
		public float sprintMax = 2f;

		// Token: 0x040007C3 RID: 1987
		public float airControl = 1.5f;

		// Token: 0x040007C4 RID: 1988
		public float jumpHeight = 10f;

		// Token: 0x040007C5 RID: 1989
		public float gravity = 20f;

		// Token: 0x040007C6 RID: 1990
		public float characterHeight = 1.8f;

		// Token: 0x040007C7 RID: 1991
		public float cameraHeight = 1.7f;

		// Token: 0x040007C8 RID: 1992
		public float speed = 15f;

		// Token: 0x040007C9 RID: 1993
		public float rotationSpeed = 2f;

		// Token: 0x040007CA RID: 1994
		public float mouseSensitivity = 1f;
	}
}
