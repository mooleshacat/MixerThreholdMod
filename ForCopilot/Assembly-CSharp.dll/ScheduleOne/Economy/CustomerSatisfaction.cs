using System;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006B2 RID: 1714
	public class CustomerSatisfaction
	{
		// Token: 0x06002EED RID: 12013 RVA: 0x000C4B98 File Offset: 0x000C2D98
		public static float GetRelationshipChange(float satisfaction)
		{
			return Mathf.Lerp(-0.5f, 0.5f, satisfaction);
		}
	}
}
