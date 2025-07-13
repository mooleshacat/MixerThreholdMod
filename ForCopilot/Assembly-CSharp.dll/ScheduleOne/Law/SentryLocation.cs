using System;
using System.Collections.Generic;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x0200060B RID: 1547
	public class SentryLocation : MonoBehaviour
	{
		// Token: 0x04001C00 RID: 7168
		[Header("References")]
		public List<Transform> StandPoints = new List<Transform>();

		// Token: 0x04001C01 RID: 7169
		[Header("Info")]
		public List<PoliceOfficer> AssignedOfficers = new List<PoliceOfficer>();
	}
}
