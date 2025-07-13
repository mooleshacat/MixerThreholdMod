using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000C59 RID: 3161
	public class Cash : MonoBehaviour
	{
		// Token: 0x06005910 RID: 22800 RVA: 0x001784DD File Offset: 0x001766DD
		public static int GetBillStacksToDisplay(float amount)
		{
			return Mathf.Clamp((int)(amount / 5f), 1, 50);
		}

		// Token: 0x04004135 RID: 16693
		public static float stackSize = 250f;

		// Token: 0x04004136 RID: 16694
		public static int[] amounts = new int[]
		{
			5,
			50,
			(int)Cash.stackSize
		};
	}
}
