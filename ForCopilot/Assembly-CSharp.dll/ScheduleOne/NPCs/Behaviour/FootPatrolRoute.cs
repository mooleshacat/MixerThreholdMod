using System;
using FluffyUnderware.DevTools.Extensions;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000557 RID: 1367
	public class FootPatrolRoute : MonoBehaviour
	{
		// Token: 0x0600205C RID: 8284 RVA: 0x00084E38 File Offset: 0x00083038
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
			Gizmos.color = this.PathColor;
			for (int j = 0; j < this.Waypoints.Length - 1; j++)
			{
				if (!(this.Waypoints[j] == null))
				{
					Gizmos.DrawLine(this.Waypoints[j].position + Vector3.up * 0.5f, this.Waypoints[j + 1].position + Vector3.up * 0.5f);
				}
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00084F49 File Offset: 0x00083149
		private void OnValidate()
		{
			this.UpdateWaypoints();
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00084F51 File Offset: 0x00083151
		private void UpdateWaypoints()
		{
			this.Waypoints = base.transform.GetComponentsInChildren<Transform>();
			this.Waypoints = ArrayExt.Remove<Transform>(this.Waypoints, base.transform);
		}

		// Token: 0x04001901 RID: 6401
		[Header("Settings")]
		public string RouteName = "Foot patrol route";

		// Token: 0x04001902 RID: 6402
		public Color PathColor = Color.red;

		// Token: 0x04001903 RID: 6403
		public Transform[] Waypoints;

		// Token: 0x04001904 RID: 6404
		public int StartWaypointIndex;
	}
}
