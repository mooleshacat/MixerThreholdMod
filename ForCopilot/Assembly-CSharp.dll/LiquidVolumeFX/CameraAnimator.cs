using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200017B RID: 379
	public class CameraAnimator : MonoBehaviour
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x00020A75 File Offset: 0x0001EC75
		private void Start()
		{
			this.y = base.transform.position.y;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00020A90 File Offset: 0x0001EC90
		private void Update()
		{
			base.transform.RotateAround(this.lookAt, Vector3.up, Time.deltaTime * this.speedX);
			this.y += this.dy;
			this.dy -= (base.transform.position.y - this.baseHeight) * Time.deltaTime * this.speedY;
			base.transform.position = new Vector3(base.transform.position.x, this.y, base.transform.position.z);
			Quaternion rotation = base.transform.rotation;
			base.transform.LookAt(this.lookAt);
			base.transform.rotation = Quaternion.Lerp(rotation, base.transform.rotation, 0.2f);
			base.transform.position += base.transform.forward * this.distSum;
			this.distSum += this.distSpeed;
			this.distDirection = ((this.distSum < 0f) ? 1f : -1f);
			this.distSpeed += Time.deltaTime * this.distDirection * this.distAcceleration;
		}

		// Token: 0x040007FB RID: 2043
		public float baseHeight = 0.6f;

		// Token: 0x040007FC RID: 2044
		public float speedY = 0.005f;

		// Token: 0x040007FD RID: 2045
		public float speedX = 5f;

		// Token: 0x040007FE RID: 2046
		public float distAcceleration = 0.0002f;

		// Token: 0x040007FF RID: 2047
		public float distSpeed = 0.0001f;

		// Token: 0x04000800 RID: 2048
		public Vector3 lookAt;

		// Token: 0x04000801 RID: 2049
		private float y;

		// Token: 0x04000802 RID: 2050
		private float dy;

		// Token: 0x04000803 RID: 2051
		private float distDirection = 1f;

		// Token: 0x04000804 RID: 2052
		private float distSum;
	}
}
