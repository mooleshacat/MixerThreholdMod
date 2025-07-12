using System;
using UnityEngine;

namespace ScheduleOne.Skating
{
	// Token: 0x020002DF RID: 735
	[RequireComponent(typeof(Skateboard))]
	public class SkateboardEffects : MonoBehaviour
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x00046A61 File Offset: 0x00044C61
		private void Awake()
		{
			this.skateboard = base.GetComponent<Skateboard>();
			this.trailsOpacity = this.Trails[0].startColor.a;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00046A88 File Offset: 0x00044C88
		private void FixedUpdate()
		{
			foreach (TrailRenderer trailRenderer in this.Trails)
			{
				Color startColor = trailRenderer.startColor;
				startColor.a = this.trailsOpacity * Mathf.Clamp01(this.skateboard.CurrentSpeed_Kmh / this.skateboard.TopSpeed_Kmh);
				trailRenderer.startColor = startColor;
			}
		}

		// Token: 0x0400106B RID: 4203
		private Skateboard skateboard;

		// Token: 0x0400106C RID: 4204
		[Header("References")]
		public TrailRenderer[] Trails;

		// Token: 0x0400106D RID: 4205
		private float trailsOpacity;
	}
}
