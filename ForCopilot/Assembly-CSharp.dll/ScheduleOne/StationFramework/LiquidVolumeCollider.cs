using System;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000908 RID: 2312
	public class LiquidVolumeCollider : MonoBehaviour
	{
		// Token: 0x06003E9C RID: 16028 RVA: 0x00107676 File Offset: 0x00105876
		private void Awake()
		{
			if (this.LiquidContainer == null)
			{
				this.LiquidContainer = base.GetComponentInParent<LiquidContainer>();
			}
		}

		// Token: 0x04002CA7 RID: 11431
		public LiquidContainer LiquidContainer;
	}
}
