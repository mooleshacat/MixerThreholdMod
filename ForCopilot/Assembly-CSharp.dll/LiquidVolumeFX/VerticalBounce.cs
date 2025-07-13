using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000181 RID: 385
	public class VerticalBounce : MonoBehaviour
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x0002130C File Offset: 0x0001F50C
		private void Update()
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.y, base.transform.localPosition.z);
			this.y += this.speed;
			this.direction = ((this.y < 0f) ? 1f : -1f);
			this.speed += Time.deltaTime * this.direction * this.acceleration;
		}

		// Token: 0x04000828 RID: 2088
		[Range(0f, 0.1f)]
		public float acceleration = 0.1f;

		// Token: 0x04000829 RID: 2089
		private float direction = 1f;

		// Token: 0x0400082A RID: 2090
		private float y;

		// Token: 0x0400082B RID: 2091
		private float speed = 0.01f;
	}
}
