using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.Property.Utilities.Power;
using UnityEngine;

namespace ScheduleOne.Property.Utilities
{
	// Token: 0x0200085E RID: 2142
	public class CosmeticPowerLine : MonoBehaviour
	{
		// Token: 0x06003A21 RID: 14881 RVA: 0x000F5FE2 File Offset: 0x000F41E2
		[Button]
		public void Draw()
		{
			PowerLine.DrawPowerLine(this.startPoint.position, this.endPoint.position, this.segments, this.LengthFactor);
		}

		// Token: 0x040029D7 RID: 10711
		public Transform startPoint;

		// Token: 0x040029D8 RID: 10712
		public Transform endPoint;

		// Token: 0x040029D9 RID: 10713
		public List<Transform> segments = new List<Transform>();

		// Token: 0x040029DA RID: 10714
		public float LengthFactor = 1.002f;
	}
}
