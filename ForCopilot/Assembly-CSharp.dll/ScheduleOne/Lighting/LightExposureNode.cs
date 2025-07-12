using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005DF RID: 1503
	public class LightExposureNode : MonoBehaviour
	{
		// Token: 0x060024E0 RID: 9440 RVA: 0x00096350 File Offset: 0x00094550
		public float GetTotalExposure(out float growSpeedMultiplier)
		{
			float num = this.ambientExposure;
			int num2 = 0;
			growSpeedMultiplier = 0f;
			foreach (UsableLightSource usableLightSource in this.sources.Keys)
			{
				if (usableLightSource != null && usableLightSource.isEmitting)
				{
					num2++;
					num += this.sources[usableLightSource];
					growSpeedMultiplier += usableLightSource.GrowSpeedMultiplier;
				}
			}
			if (num2 > 0)
			{
				growSpeedMultiplier /= (float)num2;
			}
			return num;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000963EC File Offset: 0x000945EC
		public void AddSource(UsableLightSource source, float lightAmount)
		{
			if (this.sources.ContainsKey(source))
			{
				this.sources[source] = lightAmount;
				return;
			}
			this.sources.Add(source, lightAmount);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00096417 File Offset: 0x00094617
		public void RemoveSource(UsableLightSource source)
		{
			this.sources.Remove(source);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x00096428 File Offset: 0x00094628
		private void OnDrawGizmos()
		{
			float num;
			float totalExposure = this.GetTotalExposure(out num);
			if (totalExposure > this.ambientExposure)
			{
				Gizmos.color = new Color(1f, 1f, 1f, totalExposure);
				Gizmos.DrawSphere(base.transform.position, 0.1f);
			}
		}

		// Token: 0x04001B42 RID: 6978
		public float ambientExposure;

		// Token: 0x04001B43 RID: 6979
		public Dictionary<UsableLightSource, float> sources = new Dictionary<UsableLightSource, float>();
	}
}
