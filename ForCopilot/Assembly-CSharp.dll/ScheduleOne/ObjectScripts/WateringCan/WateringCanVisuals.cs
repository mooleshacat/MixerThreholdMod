using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000C52 RID: 3154
	public class WateringCanVisuals : MonoBehaviour
	{
		// Token: 0x060058EE RID: 22766 RVA: 0x00177E40 File Offset: 0x00176040
		public virtual void SetFillLevel(float normalizedFillLevel)
		{
			this.WaterTransform.localPosition = new Vector3(this.WaterTransform.localPosition.x, Mathf.Lerp(this.WaterMinY, this.WaterMaxY, normalizedFillLevel), this.WaterTransform.localPosition.z);
			this.SideWaterTransform.localScale = new Vector3(Mathf.Lerp(this.SideWaterMinScale, this.SideWaterMaxScale, normalizedFillLevel), this.SideWaterTransform.localScale.y, this.SideWaterTransform.localScale.z);
			this.SideWaterTransform.localPosition = new Vector3(this.SideWaterTransform.localPosition.x, this.SideWaterTransform.localPosition.y, -this.SideWaterTransform.localScale.x * 0.5f);
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x00177F18 File Offset: 0x00176118
		public void SetOverflowParticles(bool enabled)
		{
			if (enabled)
			{
				if (!this.OverflowParticles.isPlaying)
				{
					this.OverflowParticles.Play();
					return;
				}
			}
			else if (this.OverflowParticles.isPlaying)
			{
				this.OverflowParticles.Stop();
			}
		}

		// Token: 0x04004110 RID: 16656
		public ParticleSystem OverflowParticles;

		// Token: 0x04004111 RID: 16657
		public Transform WaterTransform;

		// Token: 0x04004112 RID: 16658
		public float WaterMaxY;

		// Token: 0x04004113 RID: 16659
		public float WaterMinY;

		// Token: 0x04004114 RID: 16660
		public Transform SideWaterTransform;

		// Token: 0x04004115 RID: 16661
		public float SideWaterMinScale;

		// Token: 0x04004116 RID: 16662
		public float SideWaterMaxScale;

		// Token: 0x04004117 RID: 16663
		public AudioSourceController FillSound;
	}
}
