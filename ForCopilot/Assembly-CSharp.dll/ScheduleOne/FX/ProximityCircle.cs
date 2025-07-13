using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.FX
{
	// Token: 0x0200065E RID: 1630
	public class ProximityCircle : MonoBehaviour
	{
		// Token: 0x06002A4A RID: 10826 RVA: 0x000AEFE6 File Offset: 0x000AD1E6
		private void LateUpdate()
		{
			if (!this.enabledThisFrame)
			{
				this.SetAlpha(0f);
				this.enabledThisFrame = false;
			}
			this.enabledThisFrame = false;
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000AF009 File Offset: 0x000AD209
		public void SetRadius(float rad)
		{
			this.Circle.size = new Vector3(rad * 2f, rad * 2f, 3f);
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000AF02E File Offset: 0x000AD22E
		public void SetAlpha(float alpha)
		{
			this.enabledThisFrame = true;
			this.Circle.fadeFactor = alpha;
			this.Circle.enabled = (alpha > 0f);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000AF056 File Offset: 0x000AD256
		public void SetColor(Color col)
		{
			this.Circle.material.color = col;
		}

		// Token: 0x04001EFC RID: 7932
		[Header("References")]
		public DecalProjector Circle;

		// Token: 0x04001EFD RID: 7933
		private bool enabledThisFrame;
	}
}
