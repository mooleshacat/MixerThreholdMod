using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200056F RID: 1391
	public class VehiclePatrolRoute : MonoBehaviour
	{
		// Token: 0x0600217E RID: 8574 RVA: 0x00089FF4 File Offset: 0x000881F4
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.5f, 0.5f);
			Gizmos.color = Color.yellow;
			for (int i = 0; i < this.Waypoints.Length; i++)
			{
				if (!(this.Waypoints[i] == null))
				{
					Gizmos.DrawWireSphere(this.Waypoints[i].position + Vector3.up * 0.5f, 0.5f);
				}
			}
			Gizmos.color = Color.red;
			for (int j = 0; j < this.Waypoints.Length - 1; j++)
			{
				if (!(this.Waypoints[j] == null))
				{
					Gizmos.DrawLine(this.Waypoints[j].position + Vector3.up * 0.5f, this.Waypoints[j + 1].position + Vector3.up * 0.5f);
				}
			}
		}

		// Token: 0x0400199F RID: 6559
		[Header("Settings")]
		public string RouteName = "Vehicle patrol route";

		// Token: 0x040019A0 RID: 6560
		public Transform[] Waypoints;

		// Token: 0x040019A1 RID: 6561
		public int StartWaypointIndex;
	}
}
