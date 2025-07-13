using System;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.HandheldBin
{
	// Token: 0x02000C54 RID: 3156
	public class StoredItem_Bin : StoredItem
	{
		// Token: 0x0400411C RID: 16668
		[Header("References")]
		public HandheldBin_Functional bin;
	}
}
