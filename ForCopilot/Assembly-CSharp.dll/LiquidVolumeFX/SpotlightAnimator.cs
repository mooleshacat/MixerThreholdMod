using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000180 RID: 384
	public class SpotlightAnimator : MonoBehaviour
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00021191 File Offset: 0x0001F391
		private void Awake()
		{
			this.spotLight = base.GetComponent<Light>();
			this.spotLight.intensity = 0f;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000211B0 File Offset: 0x0001F3B0
		private void Update()
		{
			if (Time.time < this.lightOnDelay)
			{
				return;
			}
			float num = (Time.time - this.lightOnDelay) / this.duration;
			this.spotLight.intensity = Mathf.Lerp(this.initialIntensity, this.targetIntensity, num);
			if (Time.time - this.lastColorChange > this.nextColorInterval)
			{
				if (this.changingColor)
				{
					num = (Time.time - this.colorChangeStarted) / this.colorChangeDuration;
					if (num >= 1f)
					{
						this.changingColor = false;
						this.lastColorChange = Time.time;
					}
					this.spotLight.color = Color.Lerp(this.currentColor, this.nextColor, num);
					return;
				}
				this.currentColor = this.spotLight.color;
				this.nextColor = new Color(Mathf.Clamp01(UnityEngine.Random.value + 0.25f), Mathf.Clamp01(UnityEngine.Random.value + 0.25f), Mathf.Clamp01(UnityEngine.Random.value + 0.25f), 1f);
				this.changingColor = true;
				this.colorChangeStarted = Time.time;
			}
		}

		// Token: 0x0400081C RID: 2076
		public float lightOnDelay = 2f;

		// Token: 0x0400081D RID: 2077
		public float targetIntensity = 3.5f;

		// Token: 0x0400081E RID: 2078
		public float initialIntensity;

		// Token: 0x0400081F RID: 2079
		public float duration = 3f;

		// Token: 0x04000820 RID: 2080
		public float nextColorInterval = 2f;

		// Token: 0x04000821 RID: 2081
		public float colorChangeDuration = 2f;

		// Token: 0x04000822 RID: 2082
		private Light spotLight;

		// Token: 0x04000823 RID: 2083
		private float lastColorChange;

		// Token: 0x04000824 RID: 2084
		private float colorChangeStarted;

		// Token: 0x04000825 RID: 2085
		private Color currentColor;

		// Token: 0x04000826 RID: 2086
		private Color nextColor;

		// Token: 0x04000827 RID: 2087
		private bool changingColor;
	}
}
