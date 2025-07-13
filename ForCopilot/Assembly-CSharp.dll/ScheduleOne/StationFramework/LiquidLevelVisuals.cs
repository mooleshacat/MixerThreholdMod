using System;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000907 RID: 2311
	public class LiquidLevelVisuals : MonoBehaviour
	{
		// Token: 0x06003E9A RID: 16026 RVA: 0x001075C8 File Offset: 0x001057C8
		private void Update()
		{
			if (this.Container == null)
			{
				return;
			}
			float num = this.Container.CurrentLiquidLevel / this.Container.MaxLevel;
			this.LiquidSurface.localPosition = Vector3.Lerp(this.LiquidSurface_Min.localPosition, this.LiquidSurface_Max.localPosition, num);
			this.LiquidSurface.localScale = new Vector3(this.LiquidSurface.localScale.x, num, this.LiquidSurface.localScale.z);
			this.LiquidSurface.gameObject.SetActive(this.Container.CurrentLiquidLevel > 0f);
		}

		// Token: 0x04002CA3 RID: 11427
		public LiquidContainer Container;

		// Token: 0x04002CA4 RID: 11428
		public Transform LiquidSurface;

		// Token: 0x04002CA5 RID: 11429
		public Transform LiquidSurface_Min;

		// Token: 0x04002CA6 RID: 11430
		public Transform LiquidSurface_Max;
	}
}
