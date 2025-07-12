using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200017A RID: 378
	public class RandomRotation : MonoBehaviour
	{
		// Token: 0x0600073E RID: 1854 RVA: 0x000209D6 File Offset: 0x0001EBD6
		private void Start()
		{
			this.randomization = UnityEngine.Random.value;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000209E4 File Offset: 0x0001EBE4
		private void Update()
		{
			if (Time.time > this.lastTime)
			{
				this.lastTime = Time.time + this.randomChangeInterval + this.randomization;
				this.v = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
			}
			base.transform.Rotate(this.v * Time.deltaTime * this.speed);
		}

		// Token: 0x040007F6 RID: 2038
		[Range(1f, 50f)]
		public float speed = 10f;

		// Token: 0x040007F7 RID: 2039
		[Range(1f, 30f)]
		public float randomChangeInterval = 10f;

		// Token: 0x040007F8 RID: 2040
		private float lastTime;

		// Token: 0x040007F9 RID: 2041
		private Vector3 v;

		// Token: 0x040007FA RID: 2042
		private float randomization;
	}
}
