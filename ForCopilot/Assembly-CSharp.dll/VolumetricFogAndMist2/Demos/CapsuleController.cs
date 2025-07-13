using System;
using UnityEngine;

namespace VolumetricFogAndMist2.Demos
{
	// Token: 0x0200016A RID: 362
	public class CapsuleController : MonoBehaviour
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x0001F51C File Offset: 0x0001D71C
		private void Update()
		{
			float num = Time.deltaTime * this.moveSpeed;
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.Translate(-num, 0f, 0f);
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.Translate(num, 0f, 0f);
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				base.transform.Translate(0f, 0f, num);
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				base.transform.Translate(0f, 0f, -num);
			}
			if ((base.transform.position - this.lastPos).magnitude > this.distanceCheck)
			{
				this.lastPos = base.transform.position;
				this.fogVolume.SetFogOfWarAlpha(base.transform.position, this.fogHoleRadius, 0f, this.clearDuration);
			}
		}

		// Token: 0x040007AC RID: 1964
		public VolumetricFog fogVolume;

		// Token: 0x040007AD RID: 1965
		public float moveSpeed = 10f;

		// Token: 0x040007AE RID: 1966
		public float fogHoleRadius = 8f;

		// Token: 0x040007AF RID: 1967
		public float clearDuration = 0.2f;

		// Token: 0x040007B0 RID: 1968
		public float distanceCheck = 1f;

		// Token: 0x040007B1 RID: 1969
		private Vector3 lastPos = new Vector3(float.MaxValue, 0f, 0f);
	}
}
