using System;
using UnityEngine;

namespace StylizedGrassDemo
{
	// Token: 0x0200016F RID: 367
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0001FE88 File Offset: 0x0001E088
		private void Start()
		{
			this.rb = base.GetComponent<Rigidbody>();
			if (!this.cam)
			{
				this.cam = Camera.main;
			}
			this.isGrounded = true;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
		private void FixedUpdate()
		{
			Vector3 a = new Vector3(this.cam.transform.forward.x, 0f, this.cam.transform.forward.z);
			a *= Input.GetAxis("Vertical");
			a = a.normalized;
			this.rb.AddForce(a * this.speed);
			if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
			{
				this.rb.AddForce(Vector3.up * this.jumpForce * this.rb.mass);
				this.isGrounded = false;
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001FF70 File Offset: 0x0001E170
		private void Update()
		{
			if (!this.isGrounded)
			{
				Physics.Raycast(base.transform.position, -Vector3.up, ref this.raycastHit, 0.5f);
				if (this.raycastHit.collider && this.raycastHit.collider.GetType() == typeof(TerrainCollider))
				{
					this.isGrounded = true;
					if (this.landBendEffect)
					{
						this.landBendEffect.Emit(1);
					}
				}
			}
		}

		// Token: 0x040007DA RID: 2010
		public Camera cam;

		// Token: 0x040007DB RID: 2011
		private float speed = 15f;

		// Token: 0x040007DC RID: 2012
		private float jumpForce = 350f;

		// Token: 0x040007DD RID: 2013
		private Rigidbody rb;

		// Token: 0x040007DE RID: 2014
		private bool isGrounded;

		// Token: 0x040007DF RID: 2015
		public ParticleSystem landBendEffect;

		// Token: 0x040007E0 RID: 2016
		private RaycastHit raycastHit;
	}
}
