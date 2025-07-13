using System;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005DE RID: 1502
	[RequireComponent(typeof(Light))]
	public class FlickeringLight : MonoBehaviour
	{
		// Token: 0x060024DC RID: 9436 RVA: 0x000961FA File Offset: 0x000943FA
		private void Start()
		{
			this.lightSource = base.GetComponent<Light>();
			this.UpdateTargetValues();
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00096210 File Offset: 0x00094410
		private void Update()
		{
			this.lightSource.intensity = Mathf.Lerp(this.lightSource.intensity, this.targetIntensity, this.flickerSpeed * Time.deltaTime);
			if (this.enableColorShift)
			{
				this.lightSource.color = Color.Lerp(this.lightSource.color, this.targetColor, this.flickerSpeed * Time.deltaTime);
			}
			if (Mathf.Abs(this.lightSource.intensity - this.targetIntensity) < 0.05f)
			{
				this.UpdateTargetValues();
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000962A3 File Offset: 0x000944A3
		private void UpdateTargetValues()
		{
			this.targetIntensity = UnityEngine.Random.Range(this.minIntensity, this.maxIntensity);
			if (this.enableColorShift)
			{
				this.targetColor = Color.Lerp(this.minColor, this.maxColor, UnityEngine.Random.value);
			}
		}

		// Token: 0x04001B39 RID: 6969
		[Header("Intensity Settings")]
		[Tooltip("The minimum light intensity.")]
		public float minIntensity = 0.8f;

		// Token: 0x04001B3A RID: 6970
		[Tooltip("The maximum light intensity.")]
		public float maxIntensity = 1.2f;

		// Token: 0x04001B3B RID: 6971
		[Header("Color Settings")]
		[Tooltip("Enable slight color shifts to simulate a warm flame.")]
		public bool enableColorShift = true;

		// Token: 0x04001B3C RID: 6972
		public Color minColor = new Color(1f, 0.8f, 0.6f);

		// Token: 0x04001B3D RID: 6973
		public Color maxColor = new Color(1f, 0.9f, 0.7f);

		// Token: 0x04001B3E RID: 6974
		[Header("Flicker Speed")]
		[Tooltip("How quickly the light flickers (lower is faster).")]
		public float flickerSpeed = 0.1f;

		// Token: 0x04001B3F RID: 6975
		private Light lightSource;

		// Token: 0x04001B40 RID: 6976
		private float targetIntensity;

		// Token: 0x04001B41 RID: 6977
		private Color targetColor;
	}
}
