using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x02000829 RID: 2089
	[RequireComponent(typeof(BoxCollider))]
	public class FunnelZone : MonoBehaviour
	{
		// Token: 0x060038AD RID: 14509 RVA: 0x000EEDE4 File Offset: 0x000ECFE4
		protected virtual void Awake()
		{
			FunnelZone.funnelZones.Add(this);
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000EEDF4 File Offset: 0x000ECFF4
		public static FunnelZone GetFunnelZone(Vector3 point)
		{
			for (int i = 0; i < FunnelZone.funnelZones.Count; i++)
			{
				if (FunnelZone.funnelZones[i].col.bounds.Contains(point))
				{
					return FunnelZone.funnelZones[i];
				}
			}
			return null;
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000EEE44 File Offset: 0x000ED044
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.5f);
			Gizmos.DrawCube(base.transform.TransformPoint(this.col.center), new Vector3(this.col.size.x, this.col.size.y, this.col.size.z));
			Gizmos.DrawLine(base.transform.position, this.entryPoint.position);
		}

		// Token: 0x040028A2 RID: 10402
		public static List<FunnelZone> funnelZones = new List<FunnelZone>();

		// Token: 0x040028A3 RID: 10403
		public BoxCollider col;

		// Token: 0x040028A4 RID: 10404
		public Transform entryPoint;
	}
}
