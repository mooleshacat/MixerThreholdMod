using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200017F RID: 383
	public class PortalAnimator : MonoBehaviour
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x000210D1 File Offset: 0x0001F2D1
		private void Start()
		{
			this.scale = base.transform.localScale;
			base.transform.localScale = Vector3.zero;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000210F4 File Offset: 0x0001F2F4
		private void Update()
		{
			if (Time.time < this.delay)
			{
				return;
			}
			float value;
			if (Time.time > this.delayFadeOut)
			{
				value = 1f - (Time.time - this.delayFadeOut) / this.duration;
			}
			else
			{
				value = (Time.time - this.delay) / this.duration;
			}
			base.transform.localScale = Mathf.Clamp01(value) * this.scale;
		}

		// Token: 0x04000818 RID: 2072
		public float delay = 2f;

		// Token: 0x04000819 RID: 2073
		public float duration = 1f;

		// Token: 0x0400081A RID: 2074
		public float delayFadeOut = 4f;

		// Token: 0x0400081B RID: 2075
		private Vector3 scale;
	}
}
