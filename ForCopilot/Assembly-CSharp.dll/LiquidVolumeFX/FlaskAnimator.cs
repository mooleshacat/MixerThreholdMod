using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200017E RID: 382
	public class FlaskAnimator : MonoBehaviour
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x00020E6F File Offset: 0x0001F06F
		private void Awake()
		{
			this.liquid = base.GetComponent<LiquidVolume>();
			this.level = this.liquid.level;
			this.liquid.alpha = 0f;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		private void Update()
		{
			float num = (this.duration > 0f) ? ((Time.time - this.delay) / this.duration) : 1f;
			if (num >= 1f)
			{
				this.level += this.direction * this.speed;
				if (this.level < this.minRange || this.level > this.maxRange)
				{
					this.direction *= -1f;
				}
				this.direction += Mathf.Sign(0.5f - this.level) * this.acceleration;
				this.level = Mathf.Clamp(this.level, this.minRange, this.maxRange);
				this.liquid.level = this.level;
				num = ((this.alphaDuration > 0f) ? Mathf.Clamp01((Time.time - this.duration - this.delay) / this.alphaDuration) : 1f);
				this.liquid.alpha = num;
				this.liquid.blurIntensity = num * this.finalRefractionBlur;
			}
			else if (this.initialPosition != this.finalPosition)
			{
				base.transform.position = Vector3.Lerp(this.initialPosition, this.finalPosition, num);
			}
			base.transform.Rotate(Vector3.up * Time.deltaTime * this.rotationSpeed * 57.29578f, Space.Self);
		}

		// Token: 0x0400080A RID: 2058
		public float speed = 0.01f;

		// Token: 0x0400080B RID: 2059
		public Vector3 initialPosition = Vector3.down * 4f;

		// Token: 0x0400080C RID: 2060
		public Vector3 finalPosition = Vector3.zero;

		// Token: 0x0400080D RID: 2061
		public float duration = 5f;

		// Token: 0x0400080E RID: 2062
		public float delay = 6f;

		// Token: 0x0400080F RID: 2063
		[Range(0f, 1f)]
		public float level;

		// Token: 0x04000810 RID: 2064
		[Range(0f, 1f)]
		public float minRange = 0.05f;

		// Token: 0x04000811 RID: 2065
		[Range(0f, 1f)]
		public float maxRange = 0.95f;

		// Token: 0x04000812 RID: 2066
		[Range(0f, 1f)]
		public float acceleration = 0.04f;

		// Token: 0x04000813 RID: 2067
		[Range(0f, 1f)]
		public float rotationSpeed = 0.25f;

		// Token: 0x04000814 RID: 2068
		[Range(0f, 2f)]
		public float alphaDuration = 2f;

		// Token: 0x04000815 RID: 2069
		[Range(0f, 1f)]
		public float finalRefractionBlur = 0.75f;

		// Token: 0x04000816 RID: 2070
		private LiquidVolume liquid;

		// Token: 0x04000817 RID: 2071
		private float direction = 1f;
	}
}
